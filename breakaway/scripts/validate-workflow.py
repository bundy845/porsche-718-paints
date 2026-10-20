"""Exercise guards without starting Photoshop or modifying real artwork/installation."""
import json
import argparse
import struct
from pathlib import Path
from unittest.mock import patch

import sys
sys.dont_write_bytecode=True
from preflight import ROOT, load, report, sha
from export_readers import tga


def validate_saved_export(folder, source, expected_hash):
    assert sha(source) == expected_hash, 'Saved source changed during export'
    files = {}
    for driver, customer in [('Rick','844824'), ('Justin','845005')]:
        for spec in (False,True):
            name = f'car_{"spec_" if spec else ""}{customer}'
            path = folder/(name+'.tga')
            assert len(tga(path)) == 2048*2048*3
            png = folder/(name+'.png')
            with png.open('rb') as stream:
                header = stream.read(24)
            assert header[:8] == b'\x89PNG\r\n\x1a\n' and struct.unpack_from('>II',header,16) == (2048,2048)
            for file in (path,png):
                files[file.name] = {'sha256':sha(file),'bytes':file.stat().st_size,'driver':driver,'state':'spec' if spec else 'paint','dimensions':[2048,2048]}
    result = {'status':'PASS','revision':load(ROOT/'docs/workflow-state.json')['revision'],'source':str(source.resolve()),'source_sha256':expected_hash,'source_unchanged':True,
              'files':files,'states':'One selected driver; paint/spec separate; guides hidden in temporary document; original source never saved',
              'limits':'File structure/readability, dimensions, hashes and ownership; not in-car/material acceptance or independent verification of arbitrary manual artwork finish masks'}
    (folder/'export-manifest.json').write_text(json.dumps(result,indent=2)+'\n',encoding='utf-8',newline='\r\n')
    return result


def run():
    results = []
    state = load(ROOT / 'docs/workflow-state.json')
    source = ROOT / state['source']
    before = sha(source)
    sources_before = sorted((p.name, p.stat().st_size) for p in (ROOT/'source').glob('*.psd'))
    known = report(parts=['I122'])
    assert not known['errors'] and known['panels'][0]['status'] == 'confirmed'
    assert known['panels'][0]['evidence'] and 'invoke-photoshop.ps1' in known['commands']['photoshop_launcher']
    results.append({'test': 'known-panel-preflight', 'status': 'PASS', 'note': 'Existing I122 evidence only; no Photoshop, diagnostics, mapping writes or PSD copies'})
    rear = report(parts=['I007'], seams=['S14', 'S15'])
    assert rear['status'] == 'PASS_WITH_GAPS' and not rear['continuity_proven']
    gaps = {g['id'] for g in rear['gaps']}
    assert {'S14', 'S15', 'rear-deck-lip-bumper-registration'} <= gaps
    results.append({'test': 'rear-seam-preflight', 'status': 'PASS', 'gaps': sorted(gaps)})
    original_load = load

    def modified_state(path, **changes):
        value = original_load(path)
        return dict(value, **changes) if path.name == 'workflow-state.json' else value

    with patch('preflight.load', side_effect=lambda p: modified_state(p, recorded_source_sha256='0'*64)):
        assert report(action='build')['status'] == 'FAIL'
        exported = report(action='export')
        assert exported['status'] != 'FAIL' and exported['source']['changed_since_record']
    results.append({'test': 'saved-manual-source-change-policy', 'status': 'PASS', 'note': 'Changed source blocks build but allows read-only export; no source rewritten'})
    with patch('preflight.load', side_effect=lambda p: modified_state(p, template='inputs/missing-required-template.psd')):
        assert report()['status'] == 'FAIL'
    # An existing non-template file exercises hash rejection without copying a PSD.
    with patch('preflight.load', side_effect=lambda p: modified_state(p, template='README.md')):
        assert report()['status'] == 'FAIL'
    results.append({'test': 'missing-and-incompatible-template', 'status': 'PASS'})
    assert report(action='diagnostic')['status'] == 'FAIL'
    results.append({'test': 'diagnostic-requires-specific-gap-plan', 'status': 'PASS'})
    assert not report(parts=['NOT-A-PANEL'])['status'] == 'PASS'
    assert sha(source) == before
    assert sources_before == sorted((p.name, p.stat().st_size) for p in (ROOT/'source').glob('*.psd'))
    results.append({'test': 'revision-policy-no-extra-PSD', 'status': 'PASS', 'source_count': len(sources_before), 'new_psd_copies': 0})
    # The recorded state is deliberately not forced to match an unexpected installed paint.
    assert 'matches_record' in rear['installed']['files']['car_844824.tga']
    results.append({'test': 'installed-drift-detected', 'status': 'PASS', 'matches_record': rear['installed']['files']['car_844824.tga']['matches_record']})
    summary = (ROOT/'docs/current-state.md').read_text(encoding='utf-8',newline='\r\n')
    assert state['source'] in summary and state['recorded_source_sha256'] in summary, 'Current-state source drift'
    automation = (ROOT/'docs/automation.md').read_text(encoding='utf-8',newline='\r\n')
    assert 'workflow.jsx -Operation export' in automation and '## Active r003' not in automation, 'Active exporter documentation drift'
    assert state['latest_rejected_revision'] in summary and 'rejected' in summary.lower(), 'Review status missing'
    results.append({'test': 'active-docs-source-and-command-agreement', 'status': 'PASS'})
    return {'status': 'PASS', 'scope': 'Deterministic guard tests; not a fresh model session or Photoshop human acceptance test', 'results': results}


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--exports',type=Path)
    parser.add_argument('--source',type=Path)
    parser.add_argument('--source-hash')
    args = parser.parse_args()
    if args.exports:
        if not args.source or not args.source_hash:
            parser.error('--exports requires --source and --source-hash')
        result = validate_saved_export(args.exports,args.source,args.source_hash)
        print(json.dumps({'status':result['status'],'source_unchanged':True,'validated_files':len(result['files']),'manifest':str(args.exports/'export-manifest.json')},separators=(',',':')))
    else:
        print(json.dumps(run(), separators=(',', ':')))
