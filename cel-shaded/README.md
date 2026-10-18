# Cel-shaded Porsche 718 GT4

This folder holds one shared editable iRacing livery for Rick (844824) and Justin (845005). Rick's personal color is preserved in the PSD; Justin's fill is bright orange. The master retains its original Rick filename.

- [Layered Photoshop master](source/rick-cel-shaded.psd): `Paintable Area > [SHARED] Cel-shaded artwork` contains the common artwork, shading, and editable color fills. Driver and passenger geometry is independently traced. `Custom Spec Map` holds the matching satin ink finish; the body remains glossy.
- [Color editing instructions](docs/color-controls.md): change either driver's paint color without duplicating artwork.
- [Current paint and spec exports](exports/current/): the two 2048×2048 TGA files generated from the PSD for iRacing. These are output files, not the editing source. Only one current export set is kept.
- [Current flat preview](design/previews/paint-flat-current.png): useful for checking UV placement, but not proof of the result on the 3D car.
- [Creative brief](docs/brief.md) and [progress log](docs/progress.md): design intent, mapping findings, and review state.
- [Shared Porsche 718 GT4 mapping notes](../shared/cars/porsche718gt4/mapping/notes.md): wire boundaries, UV orientation, sources, and checks reusable for later liveries.
- `inputs/Porsche 718 GT4.psd` and `inputs/119_template_P718.zip`: untouched source template.

**Review state:** the user approved the shared door number panels and hood team logo. In-car close-ups showed that the first heavy headlight pass used the wrong bumper segments and drew trails toward the wheels. Those segments are removed; the latest export moves the bumper sweeps forward to the lower lamp return using the numbered mapping and wire. The new joins need another in-car close-up. Rick's current paint/spec pair is installed under customer ID 844824; Justin's #E55D01 preview pair is refreshed in `design/previews/justin-on-844824/`. The black exterior BUNDY windshield banner also awaits confirmation of its latest inset fit.

Treat the current PSD as the source of truth. Future changes and TGA exports should start from that PSD so edits made directly in Photoshop remain part of the livery.

No Git repository is used for this livery.
