# Breakaway â€” Porsche 718 GT4

Shared livery for Rick (844824) and Justin (845005). Rick's violet front breaks into yellow/lime toward the rear; Justin reverses those colors. r008 restores the preferred fine r002 breakup, retaining expanded coverage, an earlier/longer transition and colored wing. r003's coarse patches were rejected; r008 revises only bumper side returns, retaining rearward flow beside the wheels and delaying the turn onto the rear face. Add branding after pattern review. The user-selected livery name and folder are `breakaway`; existing Git branch names remain unchanged.

- [Current state](docs/current-state.md)
- [Creative brief](docs/brief.md)
- [Progress](docs/progress.md)
- [Mapping and draft placement](docs/placement.md)
- [Automation and review](docs/automation.md)
- [Shared car profile](../shared/cars/porsche718gt4/mapping/README.md)

The approved [editable working master](source/breakaway-master.psd) has four linked palette controls and preserves the r008 source design. Its [active paint/spec exports](exports/current/) are byte-identical to r008. The [original r008 source](source/breakaway-r008.psd) is the rollback. The user rejected rear continuity through r008; file validation does not establish acceptance. The actual installed Rick paint differs from its recorded r008 hash and must be preserved. Spec matches r008. No installed backups by default. See [current state](docs/current-state.md), [active commands](docs/automation.md) and [machine-readable authority](docs/workflow-state.json) before work. Only the working master and r008 rollback are retained in source; hood pins, number box and branding stay deferred.

Workflow cleanup includes a read-only preflight, guarded saved-master exporter and accepted palette architecture. The historical test bundle and obsolete source PSDs were removed per user direction. The user reviewed the demo, approved the palette workflow, and authorized migration; it is complete. It does not include rear calibration or r009.

Original template: `inputs/Porsche 718 GT4.psd`, copied without modification from the existing original in `../cel-shaded/inputs/`. Its SHA-256 must match the shared profile. Existing cel-shaded artwork and installed paints are outside this project's build route.

Use the existing workspace Git repository. Keep large generated PSD/TGA assets in the project workspace without staging them into ordinary Git until a binary-storage policy is chosen. Do not initialize a nested repository or configure LFS implicitly.
