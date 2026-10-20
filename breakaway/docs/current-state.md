# Breakaway current state

Updated 2026-10-20. Implementation branch: `feature/violet-lime-hex-livery`; the user subsequently authorized committing and squash-merging into main. Use `git branch --show-current` for the live branch. Machine-readable authority: `workflow-state.json`; active commands: `automation.md`.

## Source and review status

- Working source: `source/breakaway-master.psd`; SHA-256 `59348ae0f1ca1e7940439b59f0416da1ca400fe772c4b95b8db871c0cf9eab68`. Approved palette migration from r008; original colors and geometry preserved.
- Rollback: `source/breakaway-r008.psd`, SHA-256 `e1b16f7783336be502d98fcde58a29f6e47f02a8471bcec40e126f9d019f83af`. Its four paint/spec outputs exactly match the migrated master. No whole-design accepted revision is established; rear flow remains rejected/provisional.
- The user-adopted Task 1 document states rear continuity through r008 was rejected. r008 remains the unchanged migration parent/rollback; file validation is not acceptance. No r009 or rear-seam calibration in this task.
- Photoshop 27.9.0 physically inspected r008 on 2026-10-20 without changing its hash: six paint fills, twelve finish fills, nine raster clipping bases. No masks rebuilt. Disposable linked-palette Photoshop/pixel tests PASS; four actual swatches (two per driver). User reviewed the demo and said looks good; approval applied to the palette structure and real-master migration. Detailed manual-step execution was not separately reported; scripted four-edit/save/reopen/export checks passed. Historical demo removed per user direction; retained evidence: `workflow-audit/palette-validation.json` and `photoshop-layers.png`.
- Active migrated exports: `exports/current/`, with source/output hash manifest. All four TGAs are byte-identical to historical `exports/r008/`. Rick paint recorded hash `a87bbe55dd2e23e0bf38bf6f5a5a324ae4dc54d3b24c7ff398b957cff48bdfcb`; spec `8e1e16a53aec611722076b11ec57857664754a62d020bc94e5b7f44e69e768e3`.

## Installation and recovery

- Latest actual Rick paint hash: `f95b7296169f5eba0a5c26f584c39204299f4b2c084aa73de3734627a6786d80`. It differs from recorded r008 and the earlier ad29044d audit snapshot. Migration did not install paint; preserve the newer external state. Origin is not established. Actual spec hash: `8e1e16a53aec611722076b11ec57857664754a62d020bc94e5b7f44e69e768e3`; compare installed-session record through preflight.
- Shared diagnostic session records restored d008 and its last normal r008 installation; this is historical, not proof of current bytes. No diagnostic currently recorded as installed. Stale d006 claims are superseded.
- No installed-paint backups by default. No ordinary full PSD checkpoint. Keep exactly one current source plus r008 rollback; user authorized removal of the nine other historical PSDs and the demo bundle with dependency updates.
- Original template hash remains `5585e01015e643ed33f1f7545d0a8c0a66cb830c7e83f541d1cbe07af3c00b21`.

## Mapping and deferred work

- Reuse shared panels, evidence and exact seam records. S14/S15 and the complete deck/lip/bumper edge chain remain provisional/unregistered. Source-space paired boundaries are absent; no inference from interpolation or nearby wire edges.
- Existing 770 hood/fender sample agreements are artwork-file checks; structured S01/S02 still have no confirmed physical paired boundaries. Do not promote them by inference.
- Hood pins, number box and branding remain deferred. Rick/Justin reversed violet/lime and existing finishes remain the real source design; disposable test colors are not a design revision.

## Next action

Use the approved working master and PALETTES controls for authorized color edits; preserve and close it before export with the documented launcher. Rear calibration/r009 requires its separately scoped Task 2. See `editing-guide.md` and `workflow-audit/` for evidence and limitations.

## Naming and file review

Livery name/folder: `breakaway`. Source filenames use breakaway; existing branch names stay unchanged. Project AGENTS.md requires a referenced-file review and direction request at each meaningful step. The open master was saved before moving and reexported without rebuilding. Its fresh outputs and source manifest are in exports/current; four TGAs exactly match r008. Existing branches are unchanged. Referenced-file review direction for this step was received: remove nine historical PSDs and demo bundle; retire their runtime dependencies.

Cleanup completed: 207 files / 459,287,552 bytes removed. Exactly two source PSDs remain: active master and r008 rollback. Historical demo and reconstruct/test entry points retired; current saved-master export/validation checked successfully. Before the next meaningful step review remaining referenced legacy exports/derived graphics and ask for direction; the current cleanup decision is already complete.
