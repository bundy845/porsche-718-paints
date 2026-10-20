# r003 placement and pending fit checks

Continue from the r003 source and [placement.json](../graphics/r003/placement.json). The shared panel guide and evidence establish the 28 named surface identities and broad orientations. [r002 placement](placement-r002.md) and [r001 placement](placement-r001.md) are historical.

## Corrected ownership

The native paint mask candidates overlapped. I007 rear paint reached into I009 front-duct texels, producing lime patches at both front openings. r003 assigns each overlapping native pixel to one confirmed component before painting. Smaller confirmed components take precedence over large body candidates. This removed 127,647 duplicate claims. All 25,082 selected I009 pixels now match the primary color in both driver exports. Shared reusable findings and screenshots are in the car profile's mask-ownership.md and rendered/review-r002-20261019/.

The ownership heuristic is an interim geometry treatment, not a substitute for native wire-boundary and bleed verification. Unknown regions remain unassigned. No new physical identity is inferred from a neighboring wire edge or color.

## Pattern placement

- I136 driver and I081 passenger: earlier, longer irregular breakup with different lateral source regions. Main-body +x remains fore/aft; passenger vertical direction independently inverted.
- I122 hood: uses the same coarse longitudinal source coordinate `(x-400)/1600` as the fenders, based on G01/G02/G03. Exact S01/S02 correspondence is uncalibrated. Prepared d007 numbered strips for a targeted front-overhead check before claiming an exact match.
- I128 roof and I129 rear deck: mapped fore/aft with related lateral sampling, with primary-color remnants carried rearward.
- I120/I143 handles: independent local transforms sample the approximate neighboring body positions F02/F03. Those anchors and exact streak phase remain provisional; review both sides.
- I080/I153 intake surrounds: independently upright +x-rearward mappings, sampling the surrounding side transition. Exact border continuation still needs in-car confirmation.
- I078/I156 mirror outer shells: independent known directions and local source-field samples. Far faces, inner rims/inserts/stalks are not newly mapped by this revision; do not claim those are confirmed or paintable.
- I002/I009/I020 front surrounds/returns and I116 front skin: primary front treatment without rear-mask contamination. Shared texels are not treated as independent side graphics. Grille/duct mesh and native mandatory items remain protected.
- I007/I021 rear shell/inset and I040/I041 rear-deck lips: secondary color with sparse primary remnants, replacing plain rear panels.
- I030 wing top and I044/I045 outer plates: secondary-dominant paint with primary remnants using independent directions. I004 underside and I063/I064 inner plates are secondary; exact fine orientation there remains uncalibrated. Wing top is over 90% within four RGB levels of the secondary swatch in the selected mask, with purple remnants and no charcoal.
- I119 splitter retains charcoal; I125 exterior banner and I134 lettering retain primary. Glass, technical atlases and protected template details are not recolored by inference.

Color swatches and material values are unchanged from r002 (primary roughness 90, secondary 45, charcoal 160; new artwork nonmetallic). The updated spec masks follow the revised paint masks. Physical matching, especially hood/fenders, handles, intake edges and mirror field position, remains subject to in-car review. Diagnostics are separate from normal paint exports.
