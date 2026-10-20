"""Read-only workflow report. No Photoshop startup, renderer, copying, or installation."""
import argparse
import hashlib
import json
import struct
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]


def sha(path):
    with path.open('rb') as stream:
        return hashlib.file_digest(stream, 'sha256').hexdigest()


def load(path):
    return json.loads(path.read_text(encoding='utf-8-sig'))


def report(root=ROOT, parts=(), seams=(), action='inspect', diagnostic_plan=None):
    errors, warnings, gaps = [], [], []
    state = load(root / 'docs/workflow-state.json')
    mapping = (root / state['mapping']).resolve()
    identity = load(mapping / 'template/identity.json')
    panels = {p['id']: p for p in load(mapping / 'panels.json')['panels']}
    seam_data = {s['id']: s for s in load(mapping / 'seams.json')['seams']}
    evidence = {e['id']: e for e in load(mapping / 'evidence.json')['sources']}
    if action == 'diagnostic':
        if not diagnostic_plan:
            errors.append('Diagnostic blocked: a specific gap plan is required, with unresolved_question, evidence_checked, target_parts and persistence_path')
        else:
            plan = load(Path(diagnostic_plan))
            for key in ('unresolved_question', 'evidence_checked', 'target_parts', 'persistence_path'):
                if not plan.get(key):
                    errors.append('Diagnostic plan missing: ' + key)
            for eid in plan.get('evidence_checked', []):
                if eid not in evidence:
                    errors.append('Diagnostic plan cites unknown evidence: ' + eid)
            parts = list(set(parts) | set(plan.get('target_parts', [])))
            target = (mapping / plan.get('persistence_path', '')).resolve()
            if target == mapping or not target.is_relative_to(mapping):
                errors.append('Diagnostic persistence must be within shared car mapping')
    if state['car_folder'] != identity['car_folder']:
        errors.append('Car identity mismatch')
    files = {}

    def required(path, expected=None):
        path = path.resolve()
        if not path.is_file():
            errors.append('Missing required file: ' + str(path))
            return None
        digest = sha(path)
        files[str(path)] = digest
        if expected and digest != expected:
            errors.append('Identity/hash mismatch: ' + str(path))
        return digest

    required(root / state['template'], identity['psd_sha256'])
    source = (root / state['source']).resolve()
    digest = required(source)
    if digest:
        with source.open('rb') as stream:
            header = stream.read(26)
        if len(header) != 26 or header[:6] != b'8BPS\x00\x01':
            errors.append('Working source is not a readable PSD v1 header')
        elif struct.unpack_from('>IIHH', header, 14) != (identity['height'], identity['width'], identity['bit_depth'], 3):
            errors.append('Working source dimensions, depth or RGB mode differ from template')
    changed = digest != state['recorded_source_sha256']
    if changed:
        warnings.append('Source changed since recorded baseline: preserve manual edits; export actual saved PSD only')
        if action == 'build':
            errors.append('Rebuild blocked: changed source requires explicit merge/rebuild decision')
    required(root / state['rollback'], state.get('rollback_sha256'))
    if action == 'install':
        manifest_path = root / state['exports'] / 'export-manifest.json'
        if manifest_path.is_file():
            manifest = load(manifest_path)
            if manifest.get('source_sha256') != digest or Path(manifest.get('source','')).resolve() != source:
                errors.append('Installation blocked: active export manifest does not match actual saved source')
        elif changed:
            errors.append('Installation blocked: changed working source is not associated with the historical validated exports')
    for name in ('invoke-photoshop.ps1', 'workflow.jsx', 'json.jsxinc', 'PatternRenderer.cs',
                 'render-r008.ps1', 'validate-exports.py', 'validate-workflow.py', 'export_readers.py', 'install-review.ps1'):
        required(root / 'scripts' / name)
    selected = set(parts)
    for sid in seams:
        if sid not in seam_data:
            errors.append('Unknown seam: ' + sid)
        else:
            selected.update(seam_data[sid]['panels'])
    for pid in selected - panels.keys():
        errors.append('Unknown panel: ' + pid)
    selected_seams = [s for s in seam_data.values()
                      if s['id'] in seams or selected.intersection(s['panels'])]
    evidence_ids = set()
    panel_report = []
    for pid in sorted(selected & panels.keys()):
        p = panels[pid]
        required(mapping / p['mask_file'])
        evidence_ids.update(p['evidence'])
        panel_report.append({k: p.get(k) for k in
                             ('id', 'name', 'status', 'orientation', 'geometry_status', 'evidence')})
        if p['status'] != 'confirmed':
            gaps.append({'id': pid, 'gap': 'Physical identity unconfirmed; do not guess'})
    for s in selected_seams:
        evidence_ids.update(s['adjacency_evidence'])
        if s['status'] != 'confirmed' or not s['paired_boundary_points_px']:
            gaps.append({'id': s['id'], 'gap': s['limitation']})
    if selected.intersection({'I007', 'I021', 'I040', 'I041', 'I129'}):
        gaps.append({'id': 'rear-deck-lip-bumper-registration',
                     'gap': 'No complete registered deck/lip/bumper edge chain. Artwork field interpolation is provisional, not verified geometry.'})
    for eid in sorted(evidence_ids):
        if eid not in evidence:
            errors.append('Missing evidence ID: ' + eid)
        else:
            e = evidence[eid]
            required(mapping / e['file'], e.get('sha256'))
    installed = load(root / state.get('installed_record', state['exports'] + '/installed-session.json'))
    installed_report = {'recorded_revision': installed['revision'], 'files': {}}
    for name, value in installed['files'].items():
        path = Path(installed['paint_folder']) / name
        actual = sha(path) if path.is_file() else None
        matched = actual == value['sha256']
        installed_report['files'][name] = {'actual_sha256': actual, 'recorded_sha256': value['sha256'], 'matches_record': matched}
        if not matched:
            warnings.append('Installed file differs/missing; preserve it: ' + name)
    diagnostic = load(mapping / 'diagnostics/installed-session.json')
    installed_report['diagnostic'] = {k: diagnostic.get(k) for k in ('status', 'diagnostic', 'current_normal_paint')}
    return {
        'status': 'FAIL' if errors else 'PASS_WITH_GAPS' if gaps else 'PASS',
        'action': action, 'project': str(root.resolve()),
        'car': identity['car'], 'template': {'path': str((root/state['template']).resolve()), 'sha256': identity['psd_sha256']},
        'source': {'path': str(source), 'sha256': digest, 'changed_since_record': changed},
        'exports': str((root/state['exports']).resolve()),
        'panels': panel_report,
        'seams': [{k: s.get(k) for k in ('id', 'panels', 'status', 'paired_boundary_points_px', 'adjacency_evidence')} for s in selected_seams],
        'evidence_ids': sorted(evidence_ids), 'gaps': gaps,
        'continuity_proven': bool(selected_seams) and not gaps and not errors,
        'mapping_complete': False,
        'commands': state['commands'], 'installed': installed_report,
        'latest_accepted_revision': state['latest_accepted_revision'], 'latest_rejected_revision': state['latest_rejected_revision'],
        'checkpoint_policy': state['checkpoint_policy'], 'rollback': state['rollback'], 'rollback_status': state['rollback_status'],
        'installed_backup_policy': state['installed_backup_policy'], 'human_editability': state['human_editability'],
        'cleanup_review_policy': state.get('cleanup_review_policy'),
        'deferred': state['deferred'], 'errors': errors, 'warnings': warnings,
    }


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--parts', nargs='*', default=[])
    parser.add_argument('--seams', nargs='*', default=[])
    parser.add_argument('--action', choices=['inspect', 'export', 'build', 'diagnostic', 'install'], default='inspect')
    parser.add_argument('--diagnostic-plan', type=Path)
    args = parser.parse_args()
    try:
        result = report(parts=args.parts, seams=args.seams, action=args.action, diagnostic_plan=args.diagnostic_plan)
    except (OSError, ValueError, KeyError, TypeError) as exc:
        result = {'status': 'FAIL', 'errors': [str(exc)]}
    print(json.dumps(result, separators=(',', ':')))
    sys.exit(1 if result['status'] == 'FAIL' else 0)
