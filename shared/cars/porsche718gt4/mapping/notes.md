# Porsche 718 GT4 paint mapping

Applies to the 2048 × 2048 `Porsche 718 GT4.psd` in `cel-shaded/inputs/`, from `119_template_P718.zip`. Verify the wire layer before reusing coordinates with another template version. Coordinates below are approximate region locations, origin at the upper left. The user rejected the hand-fitted primary contour drafts and requested a fresh start. Their numerical stroke paths must not be reused as verified seams. The user subsequently confirmed the direct trace looks much better in the viewer. New secondary contours and shading still need viewer review.

## Fast start for the next 718 GT4 livery

1. Start with the exact 718 template and its `Turn Off Before Exporting TGA > Wire` layer. Compare its size and wire layout with this project's template before reusing any coordinates. Use the [wire preview](wire/template-wire-r000.png), [coordinate diagnostic](diagnostics/coordinate-grid.png), and the mapped-region table below to identify every paint island.
2. Use the numbered-grid car views to identify the physical region before selecting a wire edge. Trace main dark contours from real panel seams and body-plane changes. Green wire edges identify UV islands and some panel edges, but some enclose decal atlases rather than exterior seams. White mesh shows curvature and small recesses such as door handles. A nearby similar-looking edge is not proof of a cross-panel continuation. Draw each side independently: the passenger island is vertically reversed relative to the driver island.
3. Keep short, uneven secondary echoes around actual features: hood nose and sides, handles, intakes, lights, roof flap, windows, ducts, spoiler, and rear bumper. Place shading only where a body plane or recess supports it. Keep ink and shadows on separate layers from the primary seam trace.
4. Check number-box coverage and the car viewer early. Review both doors, hood/front, roof, and rear before filling gaps with extra lines. A pixel-aligned flat overlay confirms only UV placement. The current fuel-cap ring, passenger handle, roof flap, lamp, rear-bumper, and spoiler accents are still provisional until a fresh viewer check.
5. Use one layered master with shared art, separate clipped driver color fills, and a matching shared ink spec mask. Export refreshed merged snapshots to avoid Photoshop's stale sequential composites. Verify both driver colors, TGA dimensions, spec alignment, and installed filename/customer ID.

Shared logos from the 992 Cup source live in `Paints/general-logos/2-old-2-slow/`. Their saved coordinates and rotations are **992-specific provenance**, not 718 placement. Position them against the destination template and car viewer.

## Sources and evidence

- The original PSD has a `Turn Off Before Exporting TGA > Wire` layer. [Wire preview](wire/template-wire-r000.png): green boundaries show the paint island and panel edges; the white mesh shows local surface shape. Keep this guide hidden in exported paint.
- [Clyde Coman, Porsche 718 GT4 Parts](https://www.tradingpaints.com/showroom/view/241344/Porsche-718-GT4-Parts) identifies separate painted parts. Local copies: [image 1](references/clyde-718-parts-1.jpg), [image 2](references/clyde-718-parts-2.jpg).
- The [complete coordinate diagnostic](diagnostics/coordinate-grid.png) and user-supplied car-viewer screenshots establish island orientation and part placement. The diagnostic is a reference, not livery artwork. See [verification.md](verification.md) for evidence gaps.
- The current direct tracing implementation is [TracePanelInk.cs](../../../../cel-shaded/scripts/TracePanelInk.cs). It uses the [isolated Wire layer](wire/wire-source.png) at native resolution, extracting green edges and excluding the white mesh. `RenderMappedCurrent.cs` belongs to the rejected hand-fitted drafts.

## Mapped regions and orientation

| Region | UV placement and orientation | Evidence / status |
| --- | --- | --- |
| Driver side, `B` | Lower side island, roughly x 820–2020 and y 1530–2048. UV x increases front to rear. Door upper edge is near y 1805 at x 910; lower door edge is near y 1975. | Grid in car viewer plus PSD wire. A prior stroke beginning at y 1692 climbed onto the adjacent body. The subsequent vertical door stroke was removed because it did not improve the visible design and conflicted with the number area. |
| Passenger side, `A` | Upper side island, roughly x 820–2020 and y 500–990. UV x increases front to rear, while its vertical UV direction is reversed relative to `B`. | Grid labels appear upside down on passenger side. A vertical mirror about y 1280 (`A_y ≈ 2560 − B_y`) oriented the door artwork correctly in the viewer. This is an approximate art transform, not a guarantee that every feature aligns. |
| Hood | Main island roughly x 370–950, y 980–1540. Nose is at low x; windshield is at high x. Upper and lower green edges curve inward near the nose. | Grid and wire. The fresh trace uses each actual edge independently; earlier mirrored approximations were rejected. |
| Front bumper and lower spoiler | Separate islands from the hood around the left/lower part of the flat template; viewer labels include `F` and low x cells. Brake-duct openings and lower splitter have their own surfaces. | Full-grid front view. Earlier guessed bumper strokes crossed ducts; no new bumper contour should be drawn without tracing its own wire island and checking in the viewer. |
| Side intake | Separate narrow island beside each door, close to the lower rear door return in the flat template. | Wire and side views. Earlier ink crossed it; stop a door stroke before this island and draw any intake treatment on its own UV part. |
| Roof and rear deck | Mid/right template islands. Roof/rear deck labels appear around x 8–15, y 8–13 in the diagnostic grid. | Overhead and rear views. Exact decorative paths are not yet calibrated. |
| Windshield banner | Two narrow, tall islands between the hood and roof on the flat template. The exterior candidate is the blue Michelin/Pilot Challenge strip, about x1004–1090, y998–1541; the adjacent dark strip to its left is the inside-facing candidate. Banner lettering runs down the UV strip to read across the windshield on the car. | The template's existing exterior series branding distinguishes the strips. The new black BUNDY art is restricted to the blue/branded island. A front car-viewer screenshot is still needed to confirm projection and reading direction. |
| Rear wing and rear bumper | Wing top/endplates show diagnostic cells around x 1–4, y 2–3; rear bumper shows cells around x 10–15, y 0–2. These are distinct from the rear deck. | Rear and overhead views. The fresh base colors the paintable wing green with traced outlines. On-car ink placement remains unverified. |

## Reusable drawing method

### Complete headlight sketch surrounds (2026-10-18)

- The previous narrow lamp accent ROIs covered only the fender's C-shaped edge; sparse 2.5px/1.2px accents failed the user's request for several heavy, imperfect loops all the way around the light. Increasing the density of generic seam tracing does not complete a split surround.
- Current fender paths independently follow the green boundaries through passenger anchors approximately (490,752), (573,805), (418,905), and driver anchors (418,1634), (573,1727), (490,1784). The user's subsequent close-ups show these sweeps around the upper lamp contours.
- **Rejected bumper paths, confirmed in-car:** passenger (325,730)-(326,811)-(340,850), and driver (340,1683)-(326,1740)-(325,1800). These draw trails toward the wheels, not the missing lower lamp perimeter. Do not reuse them as headlight returns.
- The numbered grid and parts reference place the lower return farther toward the nose on the bumper strip, near column 2 / rows 7 and 12. Revised anchors are passenger (333,844)-(345,930)-(350,999), and driver (350,1540)-(345,1600)-(333,1690). These now replace the rejected segments. **Exact new 3D endpoint alignment and complete ring continuity still need an in-car check.** Distinguish the known rejected region from the corrected placement awaiting confirmation.
- `RenderHeadlightLoops.cs` draws three continuous variable-width sweeps around the cavity, with irregular independent offsets into the paint. It bridges small gaps where white wire-mesh lines cover the green edge. Clip to the connected intended body islands rather than all blue/paintable pixels: global clipping allowed a stray stroke to land on adjacent trim in the first proof.
- Keep the new lamp artwork and matching spec isolated for adjustments after viewing both lights. Current nominal stroke widths are 5.1/3.6/2.5px at 2048 resolution, with offsets 6/14.5/23px. Old partial lamp echoes were removed from the secondary contour/spec layers, while the primary seam layer remains. Wire and flat previews establish atlas coverage; use close-up front-corner views to establish physical continuity.

### Hood logo orientation and available space (2026-10-18)

- On this template, the hood nose is at low UV x and the windshield at high UV x. An upright source logo must rotate 90 degrees clockwise: its top then points toward +UV x and its reading direction runs toward +UV y. Rotating the hood crop 270 degrees produces a front-reading review view, with windshield at the top and nose at the bottom.
- The saved upright 992 driver-door logo was copied as an embedded smart object and scaled uniformly to UV bounds x438-663/y1150-1390. This places it in the central space between the fuel-filler fixture and Porsche crest. Its bounding rectangle remains at least 62px inside the outer hood sides over that x span, measured from the original green wire edges. Inspect fixtures as well as the outer boundary; a logo can fit inside the hood yet overlap a fixture.
- Original 992 placement is not reusable placement data. The reusable artwork itself lives in `Paints/general-logos/2-old-2-slow/`; this project embeds a copy from `cel-shaded/graphics/logos/`. Front-reading and wire-overlay previews pass. The user subsequently approved the result ("looks great").

### Door number panels (718 original template, 2026-10-18)

- Use `Number Blocks` to reserve the sim's number footprint; `Car Decal` supplies the original plate location and reading orientation. At 2048px, inclusive number bounds are driver x942-1050/y1852-1924 and passenger x942-1050/y615-687. These coordinates belong only to this template.
- The passenger plate in the original decal is rotated **180 degrees**, including text. A vertical reflection would mirror lettering. Driver and passenger number-guide centers are (996,1888) and (996,651).
- Current shared `2Old` / `2Slow` panels use a gray center around each guide with off-white header/footer bands, rough ink outlines, and no baked race number. Two instances of the same embedded SVG smart object share edits. The external vector source is `cel-shaded/graphics/current/2old-2slow-number-panel.svg`; each instance retains its own placement and rotation.
- Verified all pixels of both guide rectangles fall within the exported gray center. Flat crops show readable labels after the passenger crop is rotated 180 degrees. The user subsequently reported that the panels look great; no new side screenshot was supplied for a separate visual audit.

### General placement procedure

1. Open the exact template PSD and show its Wire layer. Use Clyde’s parts guide and the full-grid viewer images to name each UV island and determine direction on the car.
2. At native 2048 × 2048 pixels, trace primary ink directly from the green boundaries. Do not approximate the primary seams with inset decorative curves. Keep each side's geometry independent. Short secondary marks may be offset inward, checking the part mask around ducts, lights, bumpers, and windows.
3. Check a raster overlay with an explicit 2048 × 2048 destination rectangle. `System.Drawing.Graphics.DrawImage(image, 0, 0)` can scale a PNG when its DPI differs from the wire preview; that produced a misleading overlay here. Pixel-aligned inspection requires `DrawImage(image, new Rectangle(0,0,2048,2048))` or an equivalent exact-pixel composite.
4. Export from an editable layered PSD and inspect the car in the viewer from both sides, front/top, and rear when those parts have artwork. A flat UV preview proves placement on the template, not placement on the rendered car.
5. Record which coordinates were confirmed by the viewer and which are still inferred. If an island is uncertain, use the complete coordinate diagnostic for that part before adding ink across it.

## Style findings from the 911 reference and 718 reviews

- The sparse incremental drafts lost the reference's overall coverage. The fresh version establishes the complete set of main panel outlines before tuning short accent marks.
- Green Wire edges also surround decal and texture-atlas islands. They are not all exterior body seams. Exclude the radiator/crest islands inside the rear-glass bay and the rear-badging atlas. Review remaining small-part outlines in the viewer before calling them final.

- The reference uses its heaviest sketch lines to reinforce real panel seams and body-plane changes. Short secondary strokes sit near curves, openings, and corners. A long line placed merely to fill an empty panel reads as a stripe.
- The reference windshield lettering is thin outlined, angular, and forward-leaning with dark centers. A solid Impact face failed to match it. For a future name or car, start from the vector outline source `cel-shaded/graphics/current/bundy-angular-outline.svg` and redraw the letters needed; adapt its placement using that car's own windshield UV map.
- The 718's exterior windshield strip hides the lower part of letters that appear close to its edge on the flat UV. An on-car BUNDY screenshot showed clipping at the original x1012-1081 bounds. The current narrower, centered smart object sits at x1026-1076, y1095-1439; confirm that correction in the viewer. Leave ample margin inside the visible banner on future cars, and determine which UV direction becomes its lower edge before fitting text.
- The 718's automatic door number overlaps the forward door area. Vertical ink there may be hidden or appear unrelated to a physical seam; keep major artwork out of the number's footprint unless a painted number panel is deliberately designed.
- The lower-door shade previously appeared as a rectangular bar in the viewer. It was removed. Add future shadow facets only where a body plane or recess supports their shape.
- On the hood, follow the rounded nose and the two curved side seams visible in the Wire layer. Broken echo strokes can give hand-drawn character, but the main contour must first read as the real hood perimeter. Treat the latest perimeter trace as a draft until viewed on the car.

## Refinement and color lessons

- Preserve confirmed primary traces while adding secondary contours on separate layers. Reference style includes handle frames, short body-crease echoes, and shaped shadow facets, beyond the main outline alone.
- Door-handle recess mesh centers are approximately (1372,1787) on the lower driver island and (1372,750) on the upper passenger island. The earlier passenger y771 placement sat too high in the rendered view. White mesh loops locate the handle recess even though it has no separate green boundary. The revised frame still needs viewer confirmation.
- **The oval near (828.5,781) is the passenger-front fuel cap, not a door handle.** This is confirmed by the user's blue-car screenshot. Its native green-wire bounds are x786–871, y751–811. A previous update incorrectly moved the passenger handle treatment there. Fuel-cap accents must remain outside the circular face. The user subsequently preferred one or two loose circular outlines over corner brackets; use UV ellipses following the measured cap shape, which becomes circular on the car.
- Rear glass opening: approximately x1595–1900, y1070–1470. The previous curves near x1872 were inside the glass opening and disappeared. Put echoes on the painted ring outside it (roughly x1580/x1915 and y1054/y1485). Exclude the radiator/crest atlas inside the opening. Side quarter-window curves are separate and require their own upper/lower-island accents.
- Roof escape flap: approximately x1217–1466, y1263–1461. Place short echoes just outside its four edges. Free curves in the middle of the roof or cutting into the flap were rejected in the viewer.
- The headlight pockets are concave boundaries near the front tips of the two large side islands; green edges plus the paint mask can direct echoes onto the painted fenders. Rear lamps meet both side-body and rear-bumper UV pieces. Check all adjoining pieces in the viewer.
- The front spoiler's GT4 MR lettering is model relief, not a text layer in this PSD. A neutral facet behind the approximate embossed area is a draft; it does not individually recolor or outline the letters. Check the new spoiler shade in the viewer before treating those coordinates as confirmed.
- The user found duct lines too close to edges or too thin. The refinement uses heavier strokes shifted toward local painted samples. Verify the inset in the viewer, especially small separate front duct islands; a flat UV border may hide along an edge when rendered.
- Use one shared white/transparent paintable surface with separate clipped Solid Color fills for driver colors. Use neutral Multiply shadows and Screen highlights above it so color changes do not require duplicated art. Keep ink-spec masks shared.
- Photoshop may export stale composites when several visibility states are saved in quick succession. Refresh, take a merged duplicate for each export, and verify output pixels against the intended swatches. The original master remains layered.

## Project-specific production preferences

The green livery keeps one editable PSD and one current Rick paint/spec TGA pair. The user does not need backups of installed paints that are downloaded from the server. These are project preferences, not intrinsic 718 mapping rules.

## Automation lessons

- Photoshop COM returned `CO_E_SERVER_EXEC_FAILURE` inside the execution sandbox. The approved elevated execution route worked. Do not repeat long sandbox COM activation attempts after this failure; use the working route.
- Setting an imported layer's opacity to 100 in the hidden spec group returned Photoshop's "Set is not currently available" error. A new layer already has opacity 100; preserve non-default opacity only when needed. The updater logs the failing stage and discards an unsuccessful unsaved update.
- Keep neutral shadows/highlights below the primary ink. Preserve existing Solid Color swatches and layer opacity when replacing generated art. Refresh and export merged temporary snapshots for each driver.
