# Rear wrap: separate side-return and rear-face flow

2026-10-19, same template identity as profile. Hood pins explicitly deferred by user; no pin work in this revision.

## Evidence and rejected assumption

G01/G03 show I007 wrapping forward onto both sides behind the rear wheels. G04 establishes +native x from driver to passenger across the rear and +native y down the rear-facing part. That does not mean all of I007 should receive downward streaks: its corners are side returns. G05/G06 establish I129 deck tail and independent I040/I041 lips; D07/D08 establish independently oriented I021 inset and separate lip faces. User's r006 review Screenshot2026-10-19 185054.png is archived unchanged in rendered/review-r006-rear-20261019/.

r006 used a uniform vertical projection over the whole shell and sampled source-u.82 at the rear top while the deck ended around.917. The user correctly observed both wrong corner direction and detached rear streaks with no corresponding deck remnants. Reject that as a continuous wrap; retain rear-center downward flow only.

## r007 provisional fit

Aft quarter/deck native x>=1800 uses shared source progression from u.81 to.86 at x2038, retaining the existing texture's primary remnants at the tail. I040/I041 carry the same lateral source span through a small phase advance to rear topu.866. Center rear fitted native x1280..1792 samples v.65..35 with u increasing downward. Side-return zones x978..1280 and1792..2041 smoothly turn from independently oriented quarter-contour sampling into the rear projection. Source/body anchor ranges and equations are in the livery's placement.json and renderer, not promoted to a generic verified car transform.

These ranges follow readable 128px grid regions, native contours and established adjacency, but **are not exact paired physical boundary coordinates**. I021 has a separate provisional virtual projection x1344..1664/y88..281 fitted to the native inset area and D07/D08. Lip fore/aft interpretation is based on saved inverted rear-view orientation; exact crop/edge pairing remains uncalibrated. Do not put these fitted ranges into seams.json as confirmed paired points.

## Checks and limits

Both driver exports checked. Aft deck contains1,145 primary-remnant mask pixels. Source-coordinate constraints verify opposed corner flows, downward center flow and the shared deck/lip/rear phase relationship. Paint/spec unchanged at3,779,568 pixels outside the rear work;252,882 rear paint pixels changed. These are file/design-field checks; ordinary in-car review must judge the actual corner angle and seam fit. No broad mapping diagnostic requested or installed.
