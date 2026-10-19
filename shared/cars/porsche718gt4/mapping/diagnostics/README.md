# Reversible mapping diagnostics

Temporary reference paints only; never publish or copy into livery exports. Verified target is Rick **844824**, `Documents/iRacing/paint/porsche718gt4`. PNG/TGA size is 2048-square. New TGAs use uncompressed 24-bit BGR, top-left origin.

| Paint | Labels | Purpose |
| --- | --- | --- |
| Original coordinate diagnostic | 128px cells, column,row | Broad G01–G06 mapping. |
| [d003](d003-small-parts.png) | Three digits = I002–I157; arrows +UV x | Separate small parts, sharing and orientation; major body panels neutral. Tiny parts may need a larger local test. |
| [d004](d004-headlight-joins.png) | column,row at 32px spacing; arrows +UV x | Lamp/fender/bumper joins. Passenger patch x288–672/y704–1056; driver x288–672/y1472–1856. Cell c,r means x [32c,32(c+1)), y [32r,32(r+1)). |
| [d005](d005-remaining-parts.png) | Larger component IDs, single `2` for I002 | Unresolved parts highlighted; confirmed exterior parts gray. Active set frozen in [d005-plan.json](d005-plan.json). |
| [d006](d006-exterior-codes.png) | Single-letter codes; white ticks on very thin strips | Prepared refinement for tiny fin, mirror-rim and inner-intake pieces. See [manifest.json](manifest.json) for code-to-component mapping. Inside banner is gray; no cockpit review required for this pass. |

Diagnostics use original-template base and provisional reference masks. Blue bleed/fringes can remain around selections. Installed livery spec TGA/MIP remains in place: old gloss/spec-only traces may be visible; do not mistake them for diagnostic paint geometry. Digits/colors are the mapping signal.

## Preview / restore

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

For d003: both mirrors/handles/intakes, front openings/louvers, wing top/bottom/all endplate faces, rear lip/center inset. For d004: both lamps including fender tips and bumper below, plus a straight-on front overview. For d006, capture one batch of close, readable views covering both front corners, both sides, the rear, and the wing from above and below. Include the visible letter codes in each frame; no cockpit view is needed. The [manifest](manifest.json) maps each code to its provisional island ID. Perspective screenshot pixels are not UV coordinates.

The session file always states which version is actually installed; a prepared file is not an installed file. Evidence records include the exact diagnostic TGA hash. Never regenerate a captured diagnostic from newer panel statuses; use its frozen plan or create a new version.
