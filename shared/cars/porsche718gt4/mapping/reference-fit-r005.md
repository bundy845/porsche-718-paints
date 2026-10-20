# Saved-reference registration, 2026-10-19

The user directed using existing evidence rather than another mapping pass. d008 was restored and the six-view request withdrawn. No new diagnostic screenshots were acquired. These observations reuse the original in-car numbered views and do not change confirmed identities or promote exact seams to confirmed.

## Evidence inspected

- G01 driver / G03 passenger: 128px native columns follow fore/aft positions; side body row direction is independent of the separate intake/handle. Driver handle lies near column10/row14; passenger recess near column10/row5. Body door grids support the recorded F02/F03 anchors, but not exact projected outlines.
- G02 front-top / G06 overhead: hood and adjoining front fenders share coarse native fore/aft columns, while their native lateral rows differ. Applying separate bbox-normalized texture rows at the join creates visible mismatch even if longitudinal positions agree.
- G05 rear-overhead / G06: roof, side roof rails and deck likewise share coarse native fore/aft columns. Independently stretching texture to each bounding box offsets the progression between panels.
- D05 driver / D01 passenger: handle and mirror have independent local orientation; intake +x is rearward and +y down, including the passenger intake whose body rows run upward.
- Archived review-r003-20261019 screenshots show the consequences: mismatched hood edge, handle/intake phase, vertical rear grain and large chips rejected by the user.

## Applied correction and limits

r005 artwork uses a common longitudinal field on I081/I136/I122/I128/I129: `u=.18+.72*(nativeX-400)/1600`. Hood/roof/deck section edges share texture rows `.35` passenger and `.65` driver with the neighboring upper body contours. The correction tapers over 96 native pixels into the original side field. Cross-section boundaries are derived from native-owned masks after established adjacency/orientation was read from the in-car references. The native ownership heuristic remains provisional at precise wire edges.

Handles and intakes sample this body field through independently oriented local transforms. F02/F03 remain approximate anchors; fitted source/body ranges and evidence IDs are recorded in breakaway/graphics/r005/placement.json. No claim of exact handle projection or fully calibrated intake seams. Mirror shell field sampling reuses known local anchors; unknown inserts/stalks are not newly identified.

770 hood/fender contour sample pairs agree in exported masks. This verifies shared bitmap sampling, **not physical pixel-scale correspondence**. The saved 128px views support this ordinary correction without new mapping; exact S01-S20 statuses remain provisional. Review the normal livery in-car before accepting visual fit. No new paired_boundary_points_px are invented from screenshot pixels or nearby wire edges.
