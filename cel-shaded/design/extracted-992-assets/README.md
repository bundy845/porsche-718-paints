# Reusable artwork from recovered Porsche 992 Cup paint

Source: `C:\Dropbox\2 Old 2 Slow\Racing\iRacing\Paints\MWM Season 12\Porsche 992_2 Cup-Recovered.psd`

Each asset has a transparent PNG for quick reuse and a layered PSD that retains the original smart object or editable text. `992-recovered-artwork-original-positions.psd` contains all five layers at their original UV positions on a 2048 × 2300 transparent canvas. The recovered source PSD was not changed.

**Placement is specific to this Porsche 992 Cup template.** The coordinates and combined PSD preserve where the artwork sat in the recovered source; they are not placement instructions for the 718 GT4 or any other car. For a new car, use its own PSD wire/UV map and a car-viewer check to place and orient each asset.

| Asset basename (`.png` and `.psd`) | Original layer in `Paintable Area` | Original layer bounds (left, top, right, bottom) |
| --- | --- | --- |
| `2-old-2-slow-passenger-door` | `2old2slow logo - Passenger Door` | 922, 734, 1148, 946 |
| `2-old-2-slow-driver-door` | `2old2slow logo - Driver Door` | 932, 1727, 1158, 1939 |
| `stay-ready-bumper-text` | `You don't have to get ready if you stay ready` | 646, 72, 1160, 100 |
| `truth-bumper-text` | `Nothing echoes louder than the truth` | 171, 72, 613, 98 |
| `me-outlined` | `Me outlined` | 772, 1890, 908, 2160 |

The door logos are deliberately stored in their original, different 992 UV orientations. Orient each for the destination car rather than assuming that a simple mirror produces the correct mapping. `Me outlined` extends below the 2048-pixel source canvas; the extracted PNG includes the full layer height.

`extraction-report.txt` records the Photoshop layer type and PNG dimensions. `cel-shaded/scripts/extract-992-recovered-assets.jsx` in the Porsche project reproduces the editable PSDs and PNGs from the source.
