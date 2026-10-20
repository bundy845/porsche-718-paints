# Current Breakaway workflow

`workflow-state.json` is the machine-readable authority; current-state.md is the short human summary. Project naming and per-step cleanup rules are in ../AGENTS.md. Existing Git branch names remain unchanged.

From the breakaway project root:

```powershell
python scripts/preflight.py --parts I122
python scripts/preflight.py --parts I007 --seams S14 S15
pwsh -NoProfile -File scripts/invoke-photoshop.ps1 -Script workflow.jsx -Operation inspect
pwsh -NoProfile -File scripts/invoke-photoshop.ps1 -Script workflow.jsx -Operation export -OutputDirectory "$PWD/exports/new-review-name"
python scripts/validate-exports.py
python scripts/validate-workflow.py
python scripts/review-files.py --output docs/workflow-audit/file-review.json
```

These are project-specific IDs/commands; the shared skill discovers them here. Preflight is read-only and never starts Photoshop. Reuse the established elevated Photoshop COM launcher; do not retest the environment on a fresh thread. Known panels reuse existing mapping/evidence. S14/S15 and the deck/lip/bumper registration chain remain provisional; PASS_WITH_GAPS never proves continuity.

Working master: source/breakaway-master.psd. Rollback: source/breakaway-r008.psd. Current exports: exports/current. All four renamed-master exports match r008 byte-for-byte. The original template remains inputs/Porsche 718 GT4.psd. See editing-guide.md for four human palette controls.

Only saved-source inspect/export/preview operations are supported by the launcher. Export opens the saved PSD, makes an in-memory duplicate, selects paint/spec states there, hides guides and writes four TGAs plus four PNGs to a new directory. The established TGA decoder in export_readers.py validates readability, dimensions and ownership; the manifest records source/output hashes. The source file is never saved by export, and unsaved open sources are refused. Actual saved palette, masks and manual artwork are authoritative; no reconstruction from cached masks.

Historical reconstruction, per-revision wrappers and disposable-demo validation are retired because their nine historical PSD inputs and demo bundle were explicitly removed by the user. Their compact audit results and revision history remain evidence, not runnable commands. validate-exports.py now validates the active saved-master export set; validate-workflow.py checks workflow guards and source/docs agreement. PatternRenderer.cs and the latest frozen render-r008.ps1 remain for recorded geometry lineage; they are not ordinary export steps and must not overwrite current masks. Original clipping bases remain preserved.

Installation is separate through install-review.ps1, Rick only and no backups by default. It uses the state's active export folder and verifies its manifest against the current saved master. The historical installed-session record remains separate; actual installed Rick paint differs from that record and must be preserved until an explicit overwrite decision. No paint or MIP installation/removal was performed during rename/cleanup.

At each meaningful requested step run the read-only file review, present referenced historical/redundant groups and ask for keep/archive/remove direction. A step is not each tool call. The current step's direction authorizes unreferenced cleanup plus removal of nine historical PSDs/demo with dependency updates. Preserve active master, rollback, originals and reusable shared mapping evidence. No silent reference removal merely to justify deletion. Cleanup records are in workflow-audit/cleanup-result.json; inventories do not count as dependencies.
