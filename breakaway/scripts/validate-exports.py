"""Validate the active saved-master exports; historical rebuild validation is retired."""
import argparse
import importlib.util
import json
from pathlib import Path
import sys
sys.dont_write_bytecode=True
from preflight import ROOT, load, sha

if __name__ == '__main__':
    parser=argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--folder',type=Path)
    args=parser.parse_args()
    state=load(ROOT/'docs/workflow-state.json')
    source=ROOT/state['source']
    folder=args.folder or ROOT/state['exports']
    spec=importlib.util.spec_from_file_location('workflow_validation',ROOT/'scripts/validate-workflow.py')
    module=importlib.util.module_from_spec(spec);spec.loader.exec_module(module)
    result=module.validate_saved_export(folder,source,sha(source))
    print(json.dumps({'status':result['status'],'source':str(source),'folder':str(folder),'validated_files':len(result['files'])},separators=(',',':')))
