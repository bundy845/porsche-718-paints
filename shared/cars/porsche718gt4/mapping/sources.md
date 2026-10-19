# Mapping sources and registration

## Template

Car folder: `porsche718gt4`; Porsche 718 Cayman GT4 Clubsport. Original `Porsche 718 GT4.psd` and `119_template_P718.zip` remain untouched in `cel-shaded/inputs/`. Canvas is 2048 × 2048, 8-bit RGB. [identity.json](template/identity.json) records SHA-256 hashes and whether the PSD matches the archive member. No template release number was supplied.

T01 [wire composite](wire/template-wire-r000.png), T02 [isolated Wire](wire/wire-source.png), and [base reference](template/base-reference.png) were exported from the original template during the livery project. The [Number Blocks](template/template-number-blocks.png) and [Car Decal](template/template-car-decals.png) exports were copied unchanged into this profile on 2026-10-19. They establish number footprints and direction, not physical panel boundaries.

## Clyde parts guide

Author: Clyde Coman. Listing: [Porsche 718 GT4 Parts](https://www.tradingpaints.com/showroom/view/241344/Porsche-718-GT4-Parts). Original retrieval date was not recorded; local sources were copied into this profile on 2026-10-18. This audit used those local files, with no new download/account action.

- C01 [image 1](references/clyde-718-parts-1.jpg) is a rendered front/rear reference pair showing panel colors and wing TOP/L/R.
- C02 [image 2](references/clyde-718-parts-2.jpg) **is a 2048-square flat color-coded atlas**, not another rendered screenshot. The previous documentation was incorrect. It contains TOP/INSIDE/L/R labels and white direction stripes. It is a JPEG reference, not a lossless paint TGA.
- Native layout was visually compared with T01: wing, side silhouettes, hood, paired banners, roof, rear-glass opening and embedded radiator/crest textures agree in location. Diagnostics use an identity transform, without scaling/rotation/translation. Parts edges sit inside the template bleed in several places. Exact residual error has not been measured; masks are not production clipping or seam boundaries.
- Selected identities are independently confirmed by d003 in-car labels. Others remain unknown/provisional. Repeating guide colors do not prove shared texels; readable I009/I020 on both sides do.
- Keep attribution. References/derivatives are mapping aids, not original artwork for publication.

## Screenshots

[evidence.json](evidence.json) records paths, SHA-256 hashes, original names, roles and observations. G01–G06 were recovered on 2026-10-19 from the user's `Pictures/Screenshots` folder: the six original numbered views dated 2026-10-16, times 160427 through 160515. Exact filenames are in the index. E01–E07 are recovered livery views of fuel-cap, banner, rejected lamp returns, number panels and hood. D01–D11 are new d003 views supplied during the audit and copied unchanged from Screenshots.

## Native mandatory overlay and template structure

[inventory.json](template/inventory.json) was read directly from the untouched PSD: 52 layer/group records, channel and bounds data, mask presence, blend/clipping/visibility, text/smart-object tags, and image-resource IDs. [inspect-template.py](scripts/inspect-template.py) reads raster channels without opening Photoshop; it does not flatten or rewrite the PSD. The original template has a small Pitbox layer at x1826–1852/y2022–2046, reserved separately from body art.

T05 [original Car Mandatory RGBA](template/original-mandatory-raster.png) has no separate layer mask. Its native alpha was measured against every reference component by [measure-mandatory.ps1](scripts/measure-mandatory.ps1). Exact counts are in [mandatory-coverage.csv](template/mandatory-coverage.csv); [the red protection preview](template/mandatory-protection-preview.png) makes opaque overlay pixels visible without relying on a viewer's transparency handling. A raw transparent PNG may display underlying RGB even at zero alpha; use the measured alpha or opaque preview.

111 components have at least 95% opaque coverage by this original mandatory overlay. They receive `protected_template_overlay` placement policy. This reserves those texture selections from custom body graphics; it does not invent names or claim that each hidden fixture was inspected in-game. Edge percentages come from measured JPEG component overlap, not a percentage of physical mapping completed. The original mandatory alpha is authoritative for exact covered pixels; uncovered JPEG fringes are not a new paintable surface. Physical identity remains unknown where no readable car evidence exists.

The remaining 17 unresolved placement areas are explicitly listed in [panel-guide.md](panel-guide.md). This count changes only when new evidence resolves an entry, not when an unknown is guessed.

## Component segmentation

[build-atlas.ps1](scripts/build-atlas.ps1) segments C02: RGB distance >50 from sampled background `(212,202,200)`, four-connectivity, minimum 32 pixels. [segmentation.json](segmentation.json) records 157 components and 354 discarded sub-32-pixel fragments totaling 451 pixels. I001 is the 8,189-pixel outer image frame and is excluded from panels.json. The other 156 components retain I002–I157 IDs for this source hash/algorithm. Do not silently reuse IDs after changing thresholds.

Masks are native RGBA: selected pixels white/opaque, others transparent. [islands.csv](islands.csv) records measured half-open bounds and pixel counts. JPEG fringes, holes, touching shapes, labels and white stripes affect segmentation. Contact sheets show the original rectangular crop, sometimes including neighbors; the actual mask isolates its component. Small components can be symbols or texture fragments. No coverage percentage is claimed.

[build-profile.py](scripts/build-profile.py) contains reviewed semantic findings and generates the panel/evidence/seam indices. Update its reviewed findings when evidence changes, then regenerate; do not run an outdated generator over newer JSON edits. [build-diagnostics.ps1](scripts/build-diagnostics.ps1) creates raster diagnostics without opening or modifying Photoshop documents.
