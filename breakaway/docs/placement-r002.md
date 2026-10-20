# r002 paint-breakaway placement

Shared mapping README, panel-guide, evidence and seams were read before this revision. Detailed confirmed bounds, independent orientations and evidence IDs for all 28 included entries are recorded in [placement.json](../graphics/r002/placement.json). Masks are clipped from native template blue components, not from the reference JPEG segmentation. This revision adds the confirmed entries omitted in r001.

| Treatment | Islands | Orientation and evidence |
| --- | --- | --- |
| Main side transition | I136 driver, I081 passenger | Both +x front to rear; driver +y down, passenger +y up. G01/G03/G06. Independent side transforms; no universal car reflection. |
| Roof transition | I128 | +x windshield to rear glass, +y passenger to driver. G02/G05/G06. Sample u=.38–.85. |
| Rear-deck surround transition | I129 | +x roof to tail, +y passenger to driver. G04/G05/G06. Sample u=.78–1.0. Center texture atlases excluded. |
| Quiet primary front | I116 bumper, I122 hood | Bumper +y across nose toward driver, central +x toward hood; hood +x nose to windshield. G01/G02/G03/G06. No detailed lamp joins. |
| Primary small parts | I002, I009, I078, I080, I120, I125, I134, I143, I153, I156 | Plain color, not directional artwork. Independent transforms and exact bounds remain as recorded in placement.json and the shared guide. Includes formerly blue front louvers, handles and banner. Shared front texels remain shared. |
| Secondary rear | I007, I021, I040 | Rear shell/inset +x driver to passenger, +y down; upper lip +x toward driver. G04/D06/D07/D08. No contrasting rear-number rectangle. |
| Charcoal aero/trim | I004, I020, I030, I041, I044, I045, I063, I064, I119 | Plain color. Includes both wing inner faces, underside, top, deck rear face, front duct inner return and splitter. Verified identities; no directional lettering. |

Texture sampling uses the original generated source, mapped continuously through `clamp((u-.22)/.78)`. It is a true irregular paint texture; there are no hexagon, cube or shard polygons. Source and prompt provenance: [texture-provenance.md](../graphics/r002/texture-provenance.md).

All 28 confirmed entries are present; 2,434,905 full-coverage native mask pixels were checked for absence of template blue and zero metallic value in both exported spec maps. This establishes coverage of the selected native masks, not physical edge bleed or full coverage of unverified regions. Unknown components, cockpit-only banner and internal atlases remain outside the artwork scope.

Exact paired seam samples S01–S20 remain uncalibrated. Main panels have appropriate fore/aft progression and related color ranges, but no individual scratch is asserted to align across separate islands. A design-critical exact join needs the shared profile's targeted numbered/ticked diagnostic and in-car evidence before confirmation.

User-supplied r001 screenshots showed blue handles/banner/louver surround/rear trim and rejected cube-like graphics. These images remain in the chat; local copies are not claimed. They informed the coverage checklist, not new UV coordinate guesses. r002 requires a new in-car review before marking these physical omissions resolved. [r001 placement](placement-r001.md) is historical.
