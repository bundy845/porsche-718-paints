# r008 local bumper side-return revision

Parent placement preserved in placement-r007.md. Active transforms in graphics/r008/placement.json and shared mapping/side-returns-r008.md.

Only I007 side regions change. Load r007 masks without recomputing its accepted quarter/deck/lip field. Driver native x<=1216 and passenger x>=1856 retain source-u fore/aft and source-v varying with height only; blend to unchanged rear projection within x1216..1344 and1856..1728. Rear middle x1344..1728 remains exact r007 pixels.

Provisional independent virtual body fits: driver bodyX=rearX+768, bodyY=1792+.8*rearY; passenger bodyX=3840-rearX, bodyY=768-.8*rearY. Native directions from G01/G03 and rejected r007 images motivate the local orientation correction. These numeric fits do not establish exact physical edge pairing or confirmed S14/S15 seams.

Base/ink and every panel mask unchanged. Hood pins, number box, main pattern, mirrors, deck/lips and rear center unchanged. Both driver paint previews inspected. Full validation passes and4,055,308 paint/spec pixels outside side returns match r007 exactly. Ready for ordinary physical review; no in-car sign-off claimed.
