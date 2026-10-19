# Current state — cel-shaded Porsche 718 GT4

Updated: 2026-10-18. Read this with `../README.md` when resuming. Historical revisions remain in [progress.md](progress.md); reusable UV evidence remains in [the 718 mapping notes](../../shared/cars/porsche718gt4/mapping/notes.md).

## Source and Photoshop state

- Master: `source/rick-cel-shaded.psd`, one layered source for Rick and Justin. Passenger headlight marks are now exact flipped copies of the driver pixels, replacing the rejected hand-drawn substitutes. Heavy loops remain removed. Source checkpoint: `archive/before-headlight-mark-copy-20261018/`.
- Photoshop 27.9.0 preflight confirmed the master was closed before Apply and Export. Recheck the live state before any later Apply/Export stage. Never substitute the on-disk PSD for an unsaved open document.
- The original `inputs/Porsche 718 GT4.psd` and template ZIP are untouched. One-time build scripts must not be replayed over the master.

## Current output and recipients

- Rick: `exports/current/car_844824.tga` and `car_spec_844824.tga` include the copied marks, passed 2048×2048 24-bit TGA checks, and are installed under customer 844824 with matching hashes. Prior installed files: `archive/before-headlight-mark-copy-20261018/installed/`.
- Justin: orange `#E55D01` preview pair under `design/previews/justin-on-844824/`; that filename targets Rick's local viewer for a temporary preview, not Justin's customer ID. Rick and Justin share the art but retain separate color fills.
- Flat previews are in `design/previews/`. They verify UV pixels, not projection on the car.

## Confirmed mapping and decisions

- Driver side is the lower side island; passenger side is the upper reversed island. Trace each independently. See [Mapped regions and orientation](../../shared/cars/porsche718gt4/mapping/notes.md#mapped-regions-and-orientation).
- User approved the door number panels and hood team logo. The first headlight bumper returns were rejected by in-car views. See [Complete headlight sketch surrounds](../../shared/cars/porsche718gt4/mapping/notes.md#complete-headlight-sketch-surrounds-2026-10-18).
- The base traced panel outlines remain around the headlight openings. The passenger marks are 774 actual pixels copied from `03 Short echoes and corner hatching`, with transform `x'=x, y'=2536-y`; no resampling or drawing. The saved PSD copy matches the donor pixel-for-pixel. Driver crop remains identical. Source selection and copy proof: `design/diagnostics/headlight-copy-source/`.
- When the user requests matching marks, copy the existing artwork with flips/rotations. Do not draw similar replacements.

## Rejected; do not repeat

- Passenger bumper path `(325,730)–(326,811)–(340,850)` and driver `(340,1683)–(326,1740)–(325,1800)` run toward the wheels. Do not use them as lower lamp returns.
- The later curved bumper returns and the three heavy fender sweeps were also rejected as a visual treatment. Do not rerun the headlight loop refresh over the current master.
- The four invented passenger strokes were rejected and removed, including their generator and apply script from active files. They are retained only in the latest checkpoint.
- Do not rerun one-time build/conversion JSX over the active PSD. Do not retry sandbox Photoshop COM after `CO_E_SERVER_EXEC_FAILURE`; use the verified elevated route in [automation.md](automation.md).

## Provisional and next action

- The passenger accents passed flat UV and paint/spec checks, but their projected appearance still needs an in-car look. The inset BUNDY windshield lettering and other provisional surfaces remain indexed in the mapping notes.
- Next action: review close-up car views of both lights with the short passenger accents installed.
