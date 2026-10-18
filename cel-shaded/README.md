# Cel-shaded Porsche 718 GT4

This folder holds one shared editable iRacing livery for Rick (844824) and Justin (845005). Rick's personal color is preserved in the PSD; Justin's fill is bright orange. The master retains its original Rick filename.

- [Layered Photoshop master](source/rick-cel-shaded.psd): `Paintable Area > [SHARED] Cel-shaded artwork` contains the common artwork, shading, and editable color fills. Driver and passenger geometry is independently traced. `Custom Spec Map` holds the matching satin ink finish; the body remains glossy.
- [Color editing instructions](docs/color-controls.md): change either driver's paint color without duplicating artwork.
- [Current paint and spec exports](exports/current/): the two 2048×2048 TGA files generated from the PSD for iRacing. These are output files, not the editing source. Only one current export set is kept.
- [Current flat preview](design/previews/paint-flat-current.png): useful for checking UV placement, but not proof of the result on the 3D car.
- [Current state](docs/current-state.md): short resume point with the active source, output, mapping status, and next action.
- [Creative brief](docs/brief.md), [progress log](docs/progress.md), and [automation](docs/automation.md): design intent, history, and the repeatable Photoshop route.
- [Shared Porsche 718 GT4 mapping notes](../shared/cars/porsche718gt4/mapping/notes.md): wire boundaries, UV orientation, sources, and checks reusable for later liveries.
- `inputs/Porsche 718 GT4.psd` and `inputs/119_template_P718.zip`: untouched source template.

**Review state and next action:** see [current state](docs/current-state.md). Update that file after accepted revisions so this README remains a stable entry point.

Treat the current PSD as the source of truth. Future changes and TGA exports should start from that PSD so edits made directly in Photoshop remain part of the livery.

No Git repository is used for this livery.
