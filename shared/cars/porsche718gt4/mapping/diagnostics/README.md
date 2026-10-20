# Reversible mapping diagnostics

Temporary reference paints only; never publish or copy into livery exports. Verified target is Rick **844824**, `Documents/iRacing/paint/porsche718gt4`. PNG/TGA size is 2048-square. New TGAs use uncompressed 24-bit BGR, top-left origin.

| Paint | Labels | Purpose |
| --- | --- | --- |
| Original coordinate diagnostic | 128px cells, column,row | Broad G01–G06 mapping. |
| [d003](d003-small-parts.png) | Three digits = I002–I157; arrows +UV x | Separate small parts, sharing and orientation; major body panels neutral. Tiny parts may need a larger local test. |
| [d004](d004-headlight-joins.png) | column,row at 32px spacing; arrows +UV x | Lamp/fender/bumper joins. Passenger patch x288–672/y704–1056; driver x288–672/y1472–1856. Cell c,r means x [32c,32(c+1)), y [32r,32(r+1)). |
| [d005](d005-remaining-parts.png) | Larger component IDs, single `2` for I002 | Unresolved parts highlighted; confirmed exterior parts gray. Active set frozen in [d005-plan.json](d005-plan.json). |
| [d006](d006-exterior-codes.png) | Single-letter codes; white ticks on very thin strips | Prepared refinement for tiny fin, mirror-rim and inner-intake pieces. See [manifest.json](manifest.json) for code-to-component mapping. Inside banner is gray; no cockpit review required for this pass. |
| [d007](d007-hood-fender-check.png) | Numbers 1–5 at native x=640,704,768,832,896 on hood and both fenders | Prepared from violet/lime r003 to check S01/S02 fore/aft correspondence. **Not installed.** [Frozen plan](d007-plan.json); normal livery exports contain no markers. |
| [d008](d008-continuity-grid.png) | UV column,row, 64px large parts and 32px listed small parts | **Restored; no diagnostic installed.** Consolidated hood/fender, body/handle/intake/mirror, roof/deck and rear continuity calibration. [Frozen plan](d008-plan.json), [validation](d008-validation.json). Normal r004 export is the restore source. |

Diagnostics use original-template base and provisional reference masks. Blue bleed/fringes can remain around selections. Installed livery spec TGA/MIP remains in place: old gloss/spec-only traces may be visible; do not mistake them for diagnostic paint geometry. Digits/colors are the mapping signal.

## Preview / restore

**Historical d008 session (restored):** the dedicated helper below which checks the installed/source hashes, makes no backups (user instruction), leaves specs alone and restores from the existing validated r004 export. The earlier d006 record is retained as `previous_session` in installed-session.json.

```powershell
pwsh -NoProfile -File shared/cars/porsche718gt4/mapping/scripts/preview-continuity-grid.ps1 -Action Restore
```

The previous six-view request is withdrawn. Current work uses saved G01-G06/D01/D05 references and ordinary livery review. Do not reinstall d008 for a routine revision. If later explicitly used, archive originals with the exact diagnostic hash. Cell labels indicate native source coordinates; 32px and 64px grids use different scales. Do not treat matching labels as established physical correspondence.

d008 was generated with prepare-continuity-grid.ps1 and checked with validate-continuity-grid.ps1: PNG/TGA pixels identical, all 2,111,386 pixels outside selected masks unchanged, 2,082,918 diagnostic pixels changed inside. A pre-install DPI-scaled attempt is retained as d008-dpi-rejected; never installed or used as evidence. Final renderer uses explicit pixel rectangles to prevent GDI DPI scaling.

### Historical launcher

The commands below are for older diagnostics and their older backup policy. Do not use them for the active no-backup d008 session.

Run from repository root with write access to the verified iRacing paint folder:

```powershell
pwsh -NoProfile -File shared/cars/porsche718gt4/mapping/scripts/preview-diagnostic.ps1 -Action Install -Diagnostic d003-small-parts
pwsh -NoProfile -File shared/cars/porsche718gt4/mapping/scripts/preview-diagnostic.ps1 -Action Install -Diagnostic d004-headlight-joins
pwsh -NoProfile -File shared/cars/porsche718gt4/mapping/scripts/preview-diagnostic.ps1 -Action Install -Diagnostic d005-remaining-parts
pwsh -NoProfile -File shared/cars/porsche718gt4/mapping/scripts/preview-diagnostic.ps1 -Action Install -Diagnostic d006-exterior-codes
pwsh -NoProfile -File shared/cars/porsche718gt4/mapping/scripts/preview-diagnostic.ps1 -Action Restore
```

First install backs up/hashes current paint and spec files under `backups/<session-id>/`. Diagnostic switches retain the original backup. [installed-session.json](installed-session.json) records current/source hashes. Restore changes only paint; spec files were never modified. If another app changes the installed paint mid-session, the script stops rather than replacing that intervening change.

Refresh Paint Shop after each switch/restore. Save relevant screenshots under `mapping/rendered/<diagnostic>/`, with exact diagnostic hash/angle in evidence records. Capture may use the user's usual Screenshots folder; copy relevant originals unchanged.

d007 was prepared with `scripts/prepare-hood-fender-check.ps1`. The legacy Install/Restore launcher above does not allowlist it and its old backup policy does not apply implicitly to the user's current no-backup preference. Before installing d007, verify the current livery hash and record the matching project export as the restore source. Take one front-overhead image showing hood and both fenders, then restore normal r003. No exact S01/S02 join is confirmed until those numbered marks are observed in-car.

For d003: both mirrors/handles/intakes, front openings/louvers, wing top/bottom/all endplate faces, rear lip/center inset. For d004: both lamps including fender tips and bumper below, plus a straight-on front overview. For d006, capture one batch of close, readable views covering both front corners, both sides, the rear, and the wing from above and below. Include the visible letter codes in each frame; no cockpit view is needed. The [manifest](manifest.json) maps each code to its provisional island ID. Perspective screenshot pixels are not UV coordinates.

The session file always states which version is actually installed; a prepared file is not an installed file. Evidence records include the exact diagnostic TGA hash. Never regenerate a captured diagnostic from newer panel statuses; use its frozen plan or create a new version.
