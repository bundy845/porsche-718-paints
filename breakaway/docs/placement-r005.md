# r005 reference-based placement

Active source r005; transforms in graphics/r005/placement.json. Parent placement preserved in placement-r004.md. Read shared mapping/panel-guide.md and reference-fit-r005.md for evidence and limits.

G01/G02/G03/G05/G06 show consistent native fore/aft x on body, hood, roof and deck. r005 uses u = .18 + .72*(nativeX-400)/1600 on those five panels. Earlier separate bbox-scaled roof/deck placements sampled different fore/aft positions. Hood/roof/deck edges now use v=.35 passenger and .65 driver; corresponding upper body contours sample the same row. A 96px taper blends that correction into the existing side field. Cross-section contours come from the native-owned masks; numbered in-car views establish their broad adjacency. This is a provisional fit, not new pixel-level physical verification.

Handles I143/I120 use independent confirmed orientation and the saved body recess anchors near 1372,1787 and 1372,750. Face span maps body x1420 down to1324. Driver y1798 down to1776; passenger y739 up to761. Intake I153 maps x1367..1516/y1794..1977; I080 x1368..1516/y746..563, fitted to saved side grid cells and local arrow orientation. These sample the actual neighboring body field instead of independent texture phases. Mirror shells retain reference-local anchors but use the same body field. Projection/scale remain provisional.

All other part placement and coverage retained from r004: front pieces primary, rear grain horizontal, wing predominantly secondary with sparse primary remnants, splitter charcoal. No palette/finish changes or new generated texture. Unknown parts remain unassigned.

Validation: full driver paint/spec checks pass; 770 hood/fender contour samples match. Both flat paint previews inspected. Normal r005 installed for ordinary physical review; no numbered diagnostic installed or requested.
