"""Validate evidence integrity, references, coordinates, and diagnostic TGA headers."""
import hashlib
import json
import re
import struct
from pathlib import Path
from urllib.parse import unquote

root=Path(__file__).resolve().parents[1]
def read(name): return json.loads((root/name).read_text(encoding='utf-8-sig'))
def sha(p):
    with p.open('rb') as f: return hashlib.file_digest(f,'sha256').hexdigest()
def png_size(p):
    h=p.open('rb').read(24)
    assert h[:8]==b'\x89PNG\r\n\x1a\n',str(p)
    return struct.unpack('>II',h[16:24])

profile=read('panels.json'); evidence=read('evidence.json')['sources']; seams=read('seams.json')
eids={e['id'] for e in evidence}; pids={p['id'] for p in profile['panels']}
assert len(eids)==len(evidence) and len(pids)==len(profile['panels'])
for e in evidence:
    assert sha(root/e['file'])==e['sha256'],e['id']
    if 'diagnostic_file' in e: assert sha(root/e['diagnostic_file'])==e['diagnostic_sha256'],e['id']+' diagnostic hash'
for p in profile['panels']:
    assert p['status'] in ('confirmed','provisional','unknown')
    assert set(p['evidence'])<=eids,p['id']
    x0,y0,x1,y1=p['bbox_px']
    assert 0<=x0<x1<=2048 and 0<=y0<y1<=2048,p['id']
    assert png_size(root/p['mask_file'])==(2048,2048),p['id']
    assert p['geometry_status']=='provisional_reference_segmentation'
for f in profile['features']:
    assert f['parent'] in pids and set(f['evidence'])<=eids
for s in seams['seams']:
    assert set(s['panels'])<=pids and set(s['adjacency_evidence'])<=eids
    if s['status']=='confirmed': assert s['paired_boundary_points_px']
for item in seams['rejected']: assert set(item['evidence'])<=eids
for name in ('d003-small-parts','d004-headlight-joins','d005-remaining-parts','d006-exterior-codes'):
    p=root/f'diagnostics/{name}.tga'; h=p.open('rb').read(18)
    assert p.stat().st_size==18+2048*2048*3
    assert h[2]==2 and struct.unpack('<HH',h[12:16])==(2048,2048) and h[16:18]==bytes([24,32])
    assert png_size(root/f'diagnostics/{name}.png')==(2048,2048)
identity=read('template/identity.json')
assert sha((root/identity['source_path_from_mapping']).resolve())==identity['psd_sha256']
assert identity['psd_matches_archive']
session=read('diagnostics/installed-session.json')
assert sha(root/session['backup_paint'])==session['original_sha256']
if session['status']=='installed': assert sha(Path(session['target']))==session['diagnostic_sha256']
elif session['status']=='restored': assert sha(Path(session['target']))==session['original_sha256']
for file in root.glob('*.md'):
    for target in re.findall(r'\]\(([^)]+)\)',file.read_text(encoding='utf-8')):
        if target.startswith(('https:','http:','#')): continue
        path=unquote(target.split('#')[0])
        assert (file.parent/path).exists(),f'{file.name}: {target}'
report=dict(evidence_files=len(evidence),reference_components=len(pids),features=len(profile['features']),
            seams=len(seams['seams']),identity_status_counts={s:sum(p['status']==s for p in profile['panels']) for s in ('confirmed','provisional','unknown')},
            diagnostic_session_status=session['status'],active_diagnostic=session['diagnostic'],
            placement_policy_counts={s:sum(p['placement_policy']==s for p in profile['panels']) for s in sorted({p['placement_policy'] for p in profile['panels']})},
            checks='Evidence hashes, unique IDs, cross references, source bounds, PNG sizes, TGA headers/lengths, template/archive equality, installation/backup hashes, top-level Markdown link targets passed.',
            full_mapping_complete=profile['profile_complete'])
(root/'validation.json').write_text(json.dumps(report,indent=2)+'\n',encoding='utf-8')
print(json.dumps(report,indent=2))
