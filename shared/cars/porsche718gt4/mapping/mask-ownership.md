# Native paint-mask overlap finding — 2026-10-19

Applies to the exact 2048-square template in template/identity.json. Confirmed physical panel identities do not make their rectangular source bounds safe clipping masks.

## Rejected construction

The initial violet/lime project's largest-native-blue-component selection inside each panel's locator rectangle produced overlapping source claims. The rear shell I007 box `[978,10,2042,282)` overlaps the front duct I009 box `[801,58,1036,169)` at `[978,58,1036,169)`. In r002, 6,421 pixels in that overlap had a full secondary-color mask despite I009's intended primary treatment. Both front corners showed unwanted lime patches because I009 uses shared texels.

Inspectable in-car evidence is archived unchanged in rendered/review-r002-20261019/Screenshot 2026-10-19 163241.png and 163244.png. Other saved views there show r002's surrounding paint, rear and hood. These are paint-review screenshots, not new numbered UV calibration.

Checking only for absence of template blue was insufficient: a pixel can have the wrong island's color or texture and still pass that test. Do not use the count of included panel IDs as proof of correct physical ownership or finished seams.

## r003 correction and limits

The livery renderer now resolves overlapping native-component claims before assigning paint roles, giving the smaller confirmed component precedence. This removed 127,647 duplicate claims across the 28 confirmed entries. I009 is additionally kept primary; all 25,082 selected I009 pixels were checked in both exported driver paints. The r003 wing, handles, intake surrounds, mirrors and rear have their own independently mapped pattern samples.

This is an interim native-mask ownership rule, **not confirmation of new panel boundaries**. It avoids the observed role contamination without using JPEG segmentation as a final clipping boundary. Source masks, wire-edge bleed, separate-part phase and physical coverage still need in-car review. If another overlap conflicts with the native Wire and screenshot evidence, correct the local geometry instead of promoting the smaller-rectangle heuristic to ground truth.

## Hood/fender continuity

Later r006 review exposed the opposite failure of the smaller-rectangle heuristic: a hard primary patch at the driver rear corner. Local I007/I009 ownership was refined using eroded reference cores and native green-wire barriers, returning1,056 rear pixels to I007. See corrections-r006.md. Do not treat the r003 heuristic as a verified production boundary.

G01/G02/G03 support broad fore/aft correspondence and independent orientations for I122/I081/I136. S01/S02 still have no paired seam samples. d007-hood-fender-check is a frozen, numbered overlay prepared from r003 for a targeted front-overhead view; it is not installed and not present in livery exports. Equal numbers at x=640,704,768,832,896 are candidate correspondences to confirm or reject, not established matches. See diagnostics/d007-plan.json.
