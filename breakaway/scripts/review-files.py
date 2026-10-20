"""Read-only dependency review for this project; no deletion or Photoshop actions."""
import argparse
import hashlib
import json
import os
import re
from collections import Counter
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
TEXT = {'.md', '.json', '.py', '.ps1', '.jsx', '.jsxinc', '.cs', '.txt', '.yaml', '.yml', '.toml'}
REVISION = re.compile(r'r00[1-8]')


def normalized(text):
    return text.replace('\\\\', '/').replace('\\', '/').lower()


def review():
    files = [p for p in ROOT.rglob('*') if p.is_file()]
    counts = Counter(p.name.lower() for p in files)
    state = json.loads((ROOT/'docs/workflow-state.json').read_text(encoding='utf-8-sig'))
    texts = []
    for p in ROOT.parent.rglob('*'):
        if not p.is_file() or '.git' in p.parts or p.suffix.lower() not in TEXT:
            continue
        if p.name in {'file-review.json', 'cleanup-result.json'}:
            continue  # An inventory/deletion log is evidence, not an asset dependency.
        try:
            text = normalized(p.read_text(encoding='utf-8-sig'))
        except UnicodeDecodeError:
            continue
        # Expand the established revision placeholders used by historical validators.
        if p.suffix.lower() in {'.py','.ps1','.jsx','.cs'}:
            text += '\n' + '\n'.join(text.replace('{revision}',rev).replace('$revision',rev)
                                     for rev in ['r001','r002','r003','r004','r005','r006','r007','r008'])
        texts.append((p, text))
    referenced, unreferenced = [], []
    for p in sorted(files):
        rel = p.relative_to(ROOT).as_posix()
        refs = []
        if p.name in {'README.md', 'AGENTS.md', '.gitignore'}:
            refs.append('Implicit project/directory entry point')
        if rel in {state['source'], state['rollback'], state['template']}:
            refs.append('Required active source, rollback or original template in workflow-state.json')
        if p.name in {'file-review.json', 'cleanup-result.json'}:
            refs.append('Retained cleanup audit record')
        # Keep dynamic masks only in directories named by surviving code, not retired validators.
        for source,text in texts:
            if source.suffix.lower() not in {'.py','.ps1','.jsx','.cs'}:
                continue
            folders=set(re.findall(r'graphics/r00[1-8](?:/panel-masks)?',text))
            parent=p.parent.relative_to(ROOT).as_posix()
            if p.name.endswith('-mask.png') and parent in folders:
                refs.append(source.relative_to(ROOT.parent).as_posix()+' (dynamic role-mask directory)')
            if '/panel-masks/' in rel and (parent in folders or parent.removesuffix('/panel-masks') in folders):
                refs.append(source.relative_to(ROOT.parent).as_posix()+' (dynamic panel-mask directory)')
        if '__pycache__' not in p.parts:
            for source, text in texts:
                if source == p:
                    continue
                aliases = {normalized(str(p)), normalized(p.relative_to(ROOT.parent).as_posix()),
                           normalized(os.path.relpath(p, source.parent))}
                if source.is_relative_to(ROOT):
                    aliases.add(normalized(rel))
                if counts[p.name.lower()] == 1:
                    aliases.add(p.name.lower())
                if any(alias in text for alias in aliases if alias not in {'.', '..'}):
                    refs.append(source.relative_to(ROOT.parent).as_posix())
                if source.suffix.lower() in {'.py','.ps1','.jsx','.cs'} and not p.name.startswith('car_') and p.name.lower() in text:
                    refs.append(source.relative_to(ROOT.parent).as_posix()+' (dynamic filename consumer/producer)')
                if 'r006-first-rear-check' in rel and 'r006-first-rear-check' in text:
                    refs.append(source.relative_to(ROOT.parent).as_posix()+' (named historical checkpoint set)')
                if p.name == 'breakaway-r006-before-native-basecoat.psd' and 'native-basecoat pre-change source' in text:
                    refs.append(source.relative_to(ROOT.parent).as_posix()+' (historical checkpoint description)')
                # Historical validators build output filenames using customer/revision variables.
                parent = normalized(p.parent.relative_to(ROOT).as_posix())
                if source.suffix in {'.py','.ps1','.jsx','.cs'} and p.suffix.lower() in {'.png','.tga'}:
                    folder=p.parent.name.lower()
                    if p.name.startswith('car_') and (parent in text or '/'+folder+"'" in text or '/'+folder+'"' in text or "'"+folder+"'" in text or '"'+folder+'"' in text):
                        refs.append(source.relative_to(ROOT.parent).as_posix()+' (dynamic driver export path)')
        item = {'path': rel, 'bytes': p.stat().st_size, 'references': sorted(set(refs))}
        if refs:
            referenced.append(item)
        else:
            item['sha256'] = hashlib.sha256(p.read_bytes()).hexdigest()
            unreferenced.append(item)
    return {'livery':'breakaway','root':str(ROOT),'method':'Conservative explicit path/unique filename references, manifest-relative paths and known renderer/validator dynamic inputs. Inventories are not dependencies. Deletion scope excludes shared assets and other liveries.',
            'unreferenced':unreferenced,'unreferenced_bytes':sum(p['bytes'] for p in unreferenced),
            'referenced':referenced,'referenced_bytes':sum(p['bytes'] for p in referenced)}


if __name__ == '__main__':
    parser=argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--output',type=Path)
    args=parser.parse_args()
    result=review()
    if args.output:
        args.output.write_bytes((json.dumps(result,indent=2)+'\n').replace('\n','\r\n').encode('utf-8'))
    print(json.dumps({'unreferenced':result['unreferenced'],'unreferenced_bytes':result['unreferenced_bytes'],
                      'referenced_count':len(result['referenced']),'referenced_bytes':result['referenced_bytes']},separators=(',',':')))
