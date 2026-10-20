# Native basecoat, rear flow and mirror sampling findings

2026-10-19, exact template identity in template/identity.json. User requests correcting remaining blue, a downward rear pattern and smoother passenger mirror; main fine breakaway accepted in principle. Number box deferred.

## Native basecoat root cause

Read the original PSD Base Color layer's raw channels with the existing PSD decoder: index1, full2048x2048 bounds, alpha255 and RGB62,137,205 at every pixel. Earlier livery builds left that native layer unchanged and painted only masks. This allowed template blue outside those masks, including unidentified small surfaces. r006 sets the native Paintable Area/Base Color raster to each driver's primary color, restores Rick before saving, and retains the original material/spec Parts and clearcoat groups.

Car Mandatory is inside the **Turn Off Before Exporting TGA** guide group. Its alpha is useful for avoiding detailed art over template fixtures; it does not establish that a physical part is unpaintable, nor substitute for setting the actual native basecoat. The profile's protected classification is a placement policy for detailed art, not a reason to leave the underlying base blue. No new mirror-rim or hood-pin UV identity is claimed by setting the basecoat.

169,597 unprotected native-blue mask pixels additionally receive primary base finish. The native basecoat correction recolors1,589,267 exact-blue pixels outside masks and50 partially blended pixels; native spec channels there remain unchanged. The fullpaint canvas includes unused UV background, so this is not a count of visible car pixels.

## Local rear/front ownership

Existing D02/D03 establish I009 shared front surround; G01/G03/G04 and D07/D08 establish the rear. r003's smaller-locator-first ownership was an interim heuristic, not a boundary. The user now shows a hard primary patch at the driver rear corner. r006 revisits only overlap x978..1035/y58..168: eroded reference I009/I007 cores seed labels, native green-wire lines dilated1px block propagation, and local nearest labels cover line pixels. This restores1,056 pixels from false I009 ownership to I007. It uses native wire and established in-car identities, rather than a rectangle or JPEG fringe alone. Exact edge bleed/physical fit remains provisional.

## Rear direction and mirror detail

G04 confirms I007 +x across rear, +y down. D07/D08 confirm independently upright I021. The user's requested top-to-bottom breakup uses source-u along native +y. Earlier notes calling all vertical rear grain wrong were too broad: the coarse r003 texture was rejected, and the later circumferential fine streaks also do not satisfy the user's intended flow. Retain that distinction.

D01/D05 confirm outer shells I078/I156 and independent orientation. Their earlier narrow source window enlarged a few original bitmap pixels. r006 samples3x the former window about the same local anchor, using bilinear interpolation. No remapping of the accepted body field. Inner-rim per-part IDs remain unresolved; the primary native basecoat supplies plain color without fabricated detailed mapping.

G02 and E07 show blue-gray hood-pin fittings under different paint colors; this suggests separate fitting/material behavior but does not identify an atlas. Native basecoat beneath is corrected; no direct hardware repaint or unpaintable-part claim. Final appearance needs ordinary car review.

## Evidence and verification

Original review screenshots saved unchanged in rendered/review-fine-20261019/. Their capture times17:28..17:32 predate r005 installation17:33:45; do not mislabel them as confirmed r005 screenshots. Final r006 checks retain770 shared hood/fender samples and2,153,784 full-coverage art pixels outside corrections. Spec bytes unchanged at3,743,586 outside-correction pixels. These are file checks, not physical acceptance. No diagnostic installed or additional mapping batch requested.
