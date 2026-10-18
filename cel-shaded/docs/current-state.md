# Current state — cel-shaded Porsche 718 GT4

Updated: 2026-10-18. Read this with `../README.md` when resuming. Historical revisions remain in [progress.md](progress.md); reusable UV evidence remains in [the 718 mapping notes](../../shared/cars/porsche718gt4/mapping/notes.md).

## Source and Photoshop state

- Master: `source/rick-cel-shaded.psd`, one layered source for Rick and Justin. Last documented automated artwork revision: 2026-10-18 headlight bumper remap. The PSD changed on disk during this workflow refactor; the later manual changes have not been inspected or exported by this work.
- Photoshop 27.9.0 initially showed this master open with unsaved edits on 2026-10-18; a later read-only check showed **no open documents**. Recheck the live state before any Apply/Export stage. Never substitute the on-disk PSD for an unsaved open document.
- The original `inputs/Porsche 718 GT4.psd` and template ZIP are untouched. One-time build scripts must not be replayed over the master.

## Current output and recipients

- Rick: `exports/current/car_844824.tga` and `car_spec_844824.tga`; the latest documented pair was installed for customer 844824 with matching hashes. The PSD now has a newer modification time than these exports, so they are **not verified against the current master**. Re-export after reviewing the manual PSD change.
- Justin: orange `#E55D01` preview pair under `design/previews/justin-on-844824/`; that filename targets Rick's local viewer for a temporary preview, not Justin's customer ID. Rick and Justin share the art but retain separate color fills.
- Flat previews are in `design/previews/`. They verify UV pixels, not projection on the car.

## Confirmed mapping and decisions

- Driver side is the lower side island; passenger side is the upper reversed island. Trace each independently. See [Mapped regions and orientation](../../shared/cars/porsche718gt4/mapping/notes.md#mapped-regions-and-orientation).
- User approved the door number panels and hood team logo. The first headlight bumper returns were rejected by in-car views. See [Complete headlight sketch surrounds](../../shared/cars/porsche718gt4/mapping/notes.md#complete-headlight-sketch-surrounds-2026-10-18).
- Fender headlight sweeps followed the upper lamp contours in the supplied close-ups. The corrected bumper anchors are passenger `(333,844)–(345,930)–(350,999)` and driver `(350,1540)–(345,1600)–(333,1690)`.

## Rejected; do not repeat

- Passenger bumper path `(325,730)–(326,811)–(340,850)` and driver `(340,1683)–(326,1740)–(325,1800)` run toward the wheels. Do not use them as lower lamp returns.
- Do not rerun one-time build/conversion JSX over the active PSD. Do not retry sandbox Photoshop COM after `CO_E_SERVER_EXEC_FAILURE`; use the verified elevated route in [automation.md](automation.md).

## Provisional and next action

- The corrected lower lamp endpoint alignment and complete ring continuity await new in-car close-ups of both front corners. The inset BUNDY windshield lettering also awaits a car-viewer check. Other provisional surfaces are indexed in the mapping notes.
- Next action: inspect the newly saved manual PSD state and both front-corner screenshots against `design/diagnostics/headlight-loops-{passenger,driver}-wire.png`; adjust only `scripts/RenderHeadlightLoops.cs` anchors if evidence requires it, then run `scripts/iterate.ps1` through Preview, Apply, and ExportAndValidate after checking Photoshop's live saved state. Keep the in-car status provisional until the user checks the result.
