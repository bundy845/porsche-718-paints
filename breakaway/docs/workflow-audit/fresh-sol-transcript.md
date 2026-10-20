# Independent fresh-Sol read-only scenario transcript

2026-10-20. Separate gpt-6.1-sol, medium, fork_turns none. User Task 1 explicitly requests fresh-Sol behavioral testing. Agent was given the actual installed skill, project path and three scenarios, without prior conclusions; no edits, Photoshop/render/install, diagnostics, PSD copies or additional agents were allowed.

Read workspace AGENTS.md; iracing-livery SKILL.md and preflight/editability/export references; README/current-state/automation; mapping README, panel guide, evidence, seams, ownership and r008 side-return findings; launcher and workflow.jsx.

## Known panel

Command: `python breakaway/scripts/preflight.py --parts I122`.

Result PASS_WITH_GAPS, no errors, unchanged source. I122 confirmed, locator bounds [365,977,947,1563), +x nose to windshield, +y passenger to driver. G02 front-overhead numbered view, G06 overhead, E07 hood/banner and C01/C02 establish identity/orientation. Geometry segmentation remains provisional. S01/S02/S03 have no paired samples. Decision: reuse saved artwork/masks, inspect affected controls only, use existing launcher; no mapping, diagnostic, environment retest or full checkpoint. Global four-value editing is not claimed on real legacy source.

## Rear continuity

Command: `python breakaway/scripts/preflight.py --parts I007 I081 I136 I129 I040 I041 --seams S14 S15 S12 S13`.

Result PASS_WITH_GAPS, continuity_proven false, no errors. S14 uses G03/G04, S15 G01/G04, both paired-boundary arrays empty; S12/S13 likewise. I007 +x driver to passenger and +y down rear, corners distort; I081/I136 independently oriented. I129 +x roof to tail, +y passenger to driver. I040/I041 +x toward driver; inverted from rear. D06-D09 distinguish lips from wing. Explicit rear-deck-lip-bumper-registration gap. Artwork virtual field fits and equal samples are software checks, not physical registration. No new diagnostic justified merely by a thread start; smallest unresolved target and saved evidence needed first. Rear calibration/r009 outside this task.

## Export and manual safety

Command identified (not executed by evaluating agent): `pwsh -NoProfile -File scripts/invoke-photoshop.ps1 -Script workflow.jsx -Operation export -OutputDirectory exports/<new-folder>` from project root.

Source inspection: launcher hashes source, refuses open sources, requires new project output, gates historical rebuild. Workflow exports from a temporary in-memory duplicate and closes it without saving source. Changed legacy source requires explicit merge/migration decision; linked migrated source uses saved content. No ordinary PSD copy or install. PASS by read-only source inspection; root performed actual Photoshop tests separately.

## Drift and follow-up

Initial read saw stale active r003/r002 documentation, r008 awaiting review versus latest_rejected r008, and deferred-scope mismatch. Root was concurrently updating those files. On a second read of README/current-state/automation, evaluator confirmed all three were resolved. Unexpected installed Rick paint remained, correctly documented as external state to preserve, not instruction drift. Minor pending-demo wording was updated after root completed validation. Initial PowerShell profile cache access errors were avoided with login:false thereafter.

Verdict: three read-only behavioral scenarios PASS with documented seam/manual-legacy limits. No mutation was performed by evaluator. This does not replace the user's actual Photoshop editing acceptance or authorize real migration.
