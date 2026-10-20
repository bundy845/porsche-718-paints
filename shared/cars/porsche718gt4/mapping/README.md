# Porsche 718 GT4 mapping profile

Updated 2026-10-19. Start here for every new livery. **Full mapping is in progress:** body placement and many separate exterior parts are confirmed; exact cross-panel joins, remaining atlas pieces, and some orientations are still uncalibrated. Never turn an unknown entry into a guessed placement.

This profile applies to the 2048 × 2048, 8-bit RGB `Porsche 718 GT4.psd` from `119_template_P718.zip`. Exact hashes are in [template/identity.json](template/identity.json). Compare the native [Wire layer](wire/wire-source.png) before using another template version.

## Find a surface

1. Open [panel-guide.md](panel-guide.md) for named surfaces, source bounds, orientation and limitations. [panels.json](panels.json) includes every indexed reference component, masks, features and evidence IDs.
2. Locate its ID on [overlay.png](overlay.png) or the seven contact sheets linked in the guide. Bounds use top-left origin, x right, y down, and exclusive right/bottom limits. Older A/B names mean passenger/driver main body; current IDs are I081/I136.
3. Read its sources in [evidence.json](evidence.json). Original grid screenshots and new close-ups are saved under [rendered/](rendered/). A confirmed identity does not confirm every transform or boundary.
4. Before crossing a part boundary, consult [seams.json](seams.json). Empty paired-boundary samples mean the join still needs a targeted check.

The 156 indexed components come from the flat Clyde reference, including tiny symbols and separate fragments. They are **not a claim of 156 physical panels**. Generated masks are provisional JPEG-derived selections for locating/diagnosing parts. Use original Wire and verified local geometry for final clipping and bleed.

## Reuse rules

- Name driver/left and passenger/right from the seated driver. Main passenger body is inverted, but separate parts have independent transforms: passenger intake I080 is upright; driver handle I143 is inverted. There is no universal side reflection.
- Front duct surround I009 and inner return I020 appear on both sides. Same texels affect both; I009 rotates 180 degrees between faces. Independently positioned side graphics are unsupported there.
- Door number reserves, passenger fuel cap and roof flap are indexed as features. Reserve number space before drawing. The fuel cap is not the door handle.
- Radiator/grille and crest images inside the rear-glass UV opening are texture atlases, not rear-window body paint. Do not trace every small atlas border as an exterior seam.
- Keep diagnostics out of final livery exports.
- Native component masks can overlap even on confirmed parts. Read [mask-ownership.md](mask-ownership.md) for the observed I007/I009 rear-to-front color contamination and the limits of locator bounds before constructing paint masks.

## Active verification

See [verification.md](verification.md) for remaining work and [installed-session.json](diagnostics/installed-session.json) for the historical diagnostic session. As of the 2026-10-20 workflow audit, d008 is recorded restored; no diagnostic is recorded installed. Its last normal record points to r008, but actual Rick paint bytes differ from that record. The livery preflight hashes the actual installed paint and is authoritative for current bytes. Do not repeat d006/d008 or restore historical paint from stale summaries. Preview commands in diagnostics/README.md are historical and are gated by a specific gap plan; the old backup-generating installer is retired.

Provenance and mask limits are in [sources.md](sources.md). [notes.md](notes.md) preserves earlier livery observations and rejections; the structured profile supersedes its coarse tables. Rejected headlight paths and hand-fitted outlines are not seam coordinates.
