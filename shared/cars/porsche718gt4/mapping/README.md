# Porsche 718 GT4 mapping profile

This profile is the starting point for every livery using the matching 2048 × 2048 `Porsche 718 GT4.psd` from `119_template_P718.zip`. Before placing art, compare the template Wire layer with [the saved wire preview](wire/template-wire-r000.png). If the layout differs, treat the coordinates as unverified.

## Placement check

1. Name the physical part and the corresponding UV island using the [full coordinate grid](diagnostics/coordinate-grid.png), [parts references](sources.md), and [mapped regions](notes.md#mapped-regions-and-orientation).
2. Check the recorded orientation, neighboring islands, and evidence status in [notes.md](notes.md) and [verification.md](verification.md). Use the native wire image for exact source pixels.
3. For an unresolved part or cross-panel join, mark the placement provisional and use a small labeled diagnostic on that part. Confirm it in the car viewer before claiming the join is correct.
4. Record a newly confirmed mapping here, with the template identity and the specific car-view evidence. Keep livery styling and revision history in the livery project.

The grid covers the full template, but the saved evidence does not confirm every seam or precise join. See [verification.md](verification.md) for the current coverage and remaining discovery work. Flat UV previews establish source placement only.

Reusable assets are stored here: [wire preview](wire/template-wire-r000.png), [isolated Wire layer](wire/wire-source.png), [full coordinate grid](diagnostics/coordinate-grid.png), [diagnostic TGA](diagnostics/car_844824-coordinate-diagnostic.tga), and [Clyde parts references](references/clyde-718-parts-1.jpg). The livery project retains its own working copies for its existing scripts.
