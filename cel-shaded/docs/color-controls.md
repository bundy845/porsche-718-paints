# Shared artwork and driver colors

Open `source/rick-cel-shaded.psd`. This is now the shared master for both drivers.

## Change a color

1. Expand `Paintable Area > [SHARED] Cel-shaded artwork > 01 DRIVER COLORS - show ONE fill; double-click swatch`.
2. Double-click the **color swatch** on the Rick or Justin Solid Color Fill layer and choose a color.
3. Show the desired driver's fill and hide the other. Keep `Paintable surface - shared clipping mask` visible.

Rick's fill retains the personal color you selected in Photoshop. Justin's current trial fill is **#E55D01**. His previous orange fill is hidden for comparison. Both fills clip to the same white surface layer. Its transparency defines paint coverage. Do not paint the white layer to recolor the car.

The original blue template layer is renamed `Template blue fallback - color controls are in SHARED`. It supplies unused UV gutters underneath the masked artwork. Use the Solid Color controls to change the car.

## Shared details

- Eight regional primary seam layers and the short sketch accents remain independently editable.
- `04 Body contours, handle frames and inset ducts` contains the new detail pass.
- `Shared cel shadows` uses Multiply; `Shared highlights` uses Screen. Adjust their opacity to tune the strength. Their neutral colors work with either driver's base.
- `Custom Spec Map` contains shared satin ink masks. Update these when changing painted ink shapes. Shadows retain the glossy body finish.

Editing a shared artwork layer changes both variants. No second set of body art is required. The master saves with Rick visible and guides/spec hidden.

## Export

`scripts/export-shared-current.jsx` opens the current master, preserves artwork edits, and exports Rick's current paint/spec pair plus a Justin orange flat preview. It uses each fill's actual swatch color, not the hex text in its name. Save manual changes before running it.

To preview Justin's current color on Rick's local car, use `scripts/export-justin-preview-for-rick.jsx`. It writes `design/previews/justin-on-844824/car_844824.tga` and `car_spec_844824.tga`. The filename suffix is the iRacing customer ID, not the race number. The temporary installed preview replaces the local Rick paint until the Rick pair from `exports/current/` is copied back. The layered master retains both colors and the regular Rick export is not changed.

The exporter refreshes Photoshop and creates a temporary merged snapshot for each visibility state. Direct consecutive saveAs calls produced a stale color composite during verification; never accept an export solely because the fill visibility looks correct. Check base and shaded pixel colors in the outputs.

Only current output names are retained; there are no revision export copies. The temporary flattened snapshot is closed after export; the master stays layered.

As of the 2026-10-17 detail correction, the installed review paint is Rick's current blue. Justin's #E55D01 preview files were refreshed but are not the installed state. Both variants share the corrected art.
