# Progress

## 2026-10-18 - replaced invented accents with exact flipped driver pixels

- The user rejected the newly drawn marks and clarified that matching means copying the original artwork with flips/rotations. Removed the invented passenger paint/spec layers and replaced the passenger portion of the old echo pattern with actual donor pixels from the driver's `03 Short echoes and corner hatching` layer.
- Copied six complete raster components using `x'=x, y'=2536-y`, without scaling, resampling, clipping, or drawing. The saved PSD's 774 destination pixels exactly match their source RGBA values. All fit the painted passenger island. The target spec patch restores the original passenger primary-seam spec plus the copied marks; driver crop remains pixel identical.
- Source/previous exports and rejected generator archived in `archive/before-headlight-mark-copy-20261018/`; prior installed pair is in its `installed/` subfolder. Rick paint/spec passed 2048×2048 24-bit RLE checks and were installed with matching hashes. Justin's flat preview was refreshed. In-car appearance of the flipped marks remains to be reviewed.
- Copy evidence and source selection: `design/diagnostics/headlight-copy-source/copy-report.txt` and `source-and-copy.png`. The active copy layer is `Passenger headlight - flipped driver marks`.

## 2026-10-18 - passenger headlight short accents matched to driver

- The user liked the surviving short marks around the driver lamp and requested similar detail on the passenger lamp. Kept the rejected heavy rings removed. Checkpointed the prior source and exports in `archive/before-passenger-headlight-echo-20261018/`.
- Added four short strokes near the two tapered ends of the passenger lamp on a separate shared layer, plus matching satin spec. Rick and Justin exports were refreshed from the PSD. Flat proof: `design/diagnostics/passenger-headlight-small-echo-proof.png`.
- The driver's Rick crop is pixel identical to the pre-edit crop. The new paint/spec alpha matches on all 488 added pixels, with no pixels outside the passenger paint island. Both current TGAs passed 2048×2048 24-bit RLE checks and were installed for Rick with matching hashes; prior installed files are in `archive/installed-before-passenger-headlight-echo-20261018/`. In-car projection remains to be reviewed.

## 2026-10-18 - heavy headlight loops removed

- The user rejected the latest surround treatment and asked to take it off. Checkpointed the immediately preceding PSD and current exports in `archive/before-headlight-loop-removal-20261018/`.
- Removed `Headlights - heavy sketch loops` and `[SHARED] Satin headlight sketch loops` from the layered master. Refreshed Rick paint/spec exports and Justin's flat preview. The primary traced panel outline around each lamp remains.
- Verified both exports are 2048×2048, 24-bit RLE TGAs; inspected flat crops of both headlights without the added rings. Installed the new Rick pair under 844824 with matching hashes. Backed up the prior installed files in `archive/installed-before-headlight-loop-removal-20261018/`. The proposed temporary numbered guide was not installed.

## 2026-10-18 - headlight edge and lower-return curve revision

- The user supplied a car close-up and PSD crops showing loose top endpoints, breaks between fender/bumper panels, and lower returns curving incorrectly or remaining straight. Started from the newer saved PSD after Photoshop confirmed the master was closed. Checkpoint: `archive/iteration-headlights-20261018-153048/`.
- Updated only `RenderHeadlightLoops.cs` and refreshed the isolated headlight paint/spec layers. The new renderer clips to exact painted pixels, preserves the fender endpoints within their island, and shapes both lower bumper returns with a smooth inward bow. The original rejected wheelward paths remain excluded.
- Refreshed Rick's current paint/spec TGAs and Justin's flat preview; checked both 2048×2048 24-bit exports, matching headlight paint/spec alpha, and zero artwork outside the allowed headlight UV region. Installed Rick's pair with matching SHA-256 hashes. Prior installed files are in `archive/installed-before-headlight-curve-20261018-1531/`.
- Flat crops: `design/diagnostics/headlight-loops-{passenger,driver}-{wire,rick,justin}.png`. The exact cross-panel join still needs close-up car views from both front corners.

## 2026-10-18 - headlight bumper mapping corrected after in-car rejection

- The user supplied close-ups of both headlights: the three fender sweeps follow the upper lamp shape, but the added bumper strokes run toward the wheels and stop beside the lamps. The lower lamp edges remain untreated. The user correctly pointed out that the earlier color/number mapping should have been used before choosing those bumper segments.
- Revisited `coordinate-grid.png`, the earlier numbered car views in the conversation, and both saved Clyde parts references. The headlight return is farther toward the nose on the bumper strip (near grid column 2, passenger row 7 / driver row 12). The former passenger y730-850 and driver y1683-1800 paths were wheelward segments, **now rejected by in-car evidence**. They must not remain documented as possible headlight returns.
- Preserved the open master, rejected renderer source, rejected loop PNG, and prior flat preview in `archive/current-before-headlight-bumper-remap-20261018/`. Updated `RenderHeadlightLoops.cs` to use passenger anchors (333,844), (345,930), (350,999), and driver (350,1540), (345,1600), (333,1690). Each side follows its own wire boundary; fender sweeps are unchanged. Exact new endpoint correspondence remains provisional until viewed on the car.
- Verified that the regenerated fender pixels are identical and that no new loop ink remains in the rejected wheelward sections. Verified 2,811 opaque pixels of the corrected bumper paths appear in paint and matching spec. Refreshed Rick exports and Justin previews; installed Rick's 2048x2048, 24-bit paint/spec under 844824 with matching hashes. Prior installed pair is retained with this checkpoint.
- Process correction: use the saved numbered grid and car-viewer evidence to identify the physical region first, then use the white/green wire to place paths inside that region. A similar-looking nearby edge is insufficient evidence of a cross-panel continuation. Mark the exact rejected coordinates as rejected, and keep new physical joins provisional until observed.

## 2026-10-18 - heavier complete headlight surround attempt

- **Superseded by the bumper remap above:** the initially inferred bumper correspondence in this entry failed the in-car check. Retain this entry only as revision history.
- User approved the hood logo, then supplied a headlight close-up showing that the earlier lamp accents were too thin, too restrained, and incomplete compared with the 911's irregular concentric rings. This is a coverage and stroke-weight correction, not another general edge reinforcement.
- The previous `OffsetWire` lamp calls covered only fender ROIs and used 2.5px/1.2px strokes, with the outer one broken. They did not supply the complementary bumper segment. `scripts/RenderHeadlightLoops.cs` now traces each fender C-shaped green edge independently and adds the corresponding bumper-end boundary segment. Three continuous sweeps use approximately 5.1px, 3.6px, and 2.5px widths, varied along the path; their nominal offsets are 6px, 14.5px, and 23px with independent uneven spacing.
- Wire white-mesh intersections interrupt green pixels, so the path search bridges gaps up to two pixels before smoothing. End strokes continue into the UV join areas. Output is limited to the connected painted fender and bumper islands with 2px bleed; an initial small spill onto nearby trim was caught in the flat review and removed. The bumper-to-fender correspondence and exact 3D joining remain inferred from this template and need the next in-car close-ups.
- Checkpointed the current open master at `archive/current-before-headlight-loops-20261018/rick-cel-shaded-open-state.psd`. `scripts/apply-headlight-loops.jsx` removes only the obsolete partial lamp accents from the secondary contour layer and matching spec, then adds `Headlights - heavy sketch loops` with one editable raster layer per side. Primary seams, colors, number panels, and hood logo are preserved. A matching `[SHARED] Satin headlight sketch loops` uses the actual new alpha. `refresh-headlight-loops.jsx` updates these isolated layers after renderer changes.
- Checked both sides in Rick and Justin flat previews and against the wire. Verified 7,779 opaque headlight-ink pixels appear in the paint with the matching spec treatment. All four Rick/Justin paint/spec TGAs are 2048x2048, 24-bit. Installed Rick's pair under customer ID 844824 with matching hashes; prior installed files are stored in the checkpoint. Justin's preview pair is refreshed. Review images are `design/diagnostics/headlight-loops-{passenger,driver}-{rick,justin,wire}.png`.
- Outstanding: close-up views of both headlights to confirm the physical loop continuity across fender/bumper joins and the intended heavy sketch appearance. A flat UV check does not prove a complete oval on the car.

## 2026-10-18 - saved team logo on the hood

- User approved the number panels and requested one saved general-library door logo on the hood, correctly oriented and fully within the hood boundary.
- Copied the original upright driver-door logo PSD and PNG from `Paints/general-logos/2-old-2-slow/` into `graphics/logos/`. Duplicated its embedded smart object into `Paintable Area > [SHARED] Cel-shaded artwork > Hood team logo - 2 Old 2 Slow`. The shared original library and recovered 992 source were not edited.
- Checkpoint: `archive/current-before-hood-logo-20261018/rick-cel-shaded-open-state.psd`. `scripts/apply-hood-team-logo.jsx` preserves the current open master state, rotates the logo 90 degrees clockwise, scales uniformly, and centers it near UV (550,1270). Final pixel bounds are x438-663, y1150-1390. The top points toward the windshield, and the wordmark points toward the nose.
- Inspected front-reading Rick/Justin previews and the original green-wire overlay. The entire logo bounding rectangle stays inside the hood with at least 62px lateral UV clearance over its x span. The logo sits between the fuel filler and Porsche crest with visible gaps. Crops: `design/diagnostics/hood-logo-rick-front-reading.png`, `hood-logo-justin-front-reading.png`, and `hood-logo-wire-proof.png`.
- Added a matching transparency-derived `[SHARED] Satin hood team logo` spec layer using the established RGB 0,135,0 treatment. Refreshed Rick paint/spec and Justin preview exports. Installed Rick's 2048x2048, 24-bit pair under customer ID 844824; hashes match. Prior installed TGAs are retained with the checkpoint. The new hood artwork still needs an in-car view to confirm projected appearance.

## 2026-10-18 - shared 2Old / 2Slow door number panels

- User requested the 911 reference's loose ink frame, off-white text bands, and gray number field, with `2Old` above and `2Slow` below. Both doors use this treatment; no race number is painted because iRacing supplies it.
- Inspected the untouched 718 template's `Paintable Area > Car Decal` and `Turn Off Before Exporting TGA > Number Blocks` independently. The guide's inclusive door-number bounds are x942-1050, y1852-1924 (driver) and x942-1050, y615-687 (passenger). The original passenger decal is rotated 180 degrees, including its lettering.
- Preserved the unsaved open Photoshop master in `archive/current-before-number-panels-20261018/rick-cel-shaded-open-state.psd`, then added `Door number panels - 2Old / 2Slow` to the shared artwork. Both layers are instances of the same embedded vector smart object, based on `graphics/current/2old-2slow-number-panel.svg`. The passenger instance is rotated 180 degrees. Custom lettering consists of editable vector paths. Current pixel bounds: driver x919-1073, y1809-1967; passenger x920-1074, y572-730.
- Added `[SHARED] Satin number panels` to the existing spec group using the exact artwork transparency and the established RGB 0,135,0 finish. Existing personal colors and unrelated open edits were retained. Guides are hidden in exports.
- Refreshed Rick's current paint/spec pair and Justin's orange preview pair. Installed Rick's pair under customer ID 844824; installed hashes match. Previous installed TGAs are in the checkpoint's `installed/` directory. Checked 2048x2048, 24-bit TGA output and confirmed every pixel of both number-guide rectangles remains in the gray field. Both spec-center samples are RGB 0,135,0.
- Flat visual checks are `design/diagnostics/number-panel-rick-driver.png`, `number-panel-rick-passenger.png`, and corresponding Justin crops. Final wrapping and the sim-generated number still require side screenshots in the viewer.

## 2026-10-17 - BUNDY banner fit correction

- The first in-car screenshot confirmed that the exterior-facing side and outlined style were correct, but the bottom of the letters was clipped by the visible lower edge of the windshield strip.
- Checkpointed the master at `archive/current-before-bundy-fit-correction-20261017/rick-cel-shaded.psd`. `scripts/inset-bundy-wordmark.jsx` narrows and centers the existing vector smart object within the black exterior banner. Its UV bounds changed from x1012-1081, y1096-1439 to x1026-1076, y1095-1439; the word length and artwork style are unchanged.
- Refreshed Rick's paint/spec exports and Justin's #E55D01 preview pair, then installed Rick's pair under customer ID 844824 with matching source and installed hashes. `design/diagnostics/bundy-windshield-readable.png` shows clear flat-preview margin around the full outlines. The car-viewer fit still needs a new screenshot to confirm.

## 2026-10-17 — windshield lettering corrected to the 911 reference

- The user supplied a close crop of the reference windshield and rejected the Impact trial: its solid, rounded letterforms did not match the thin outlined, angular, forward-leaning capitals in the sample.
- Drew BUNDY as custom vector outlines with dark centers and an offset cool-gray echo. `scripts/RenderBundyOutline.cs` and `scripts/render-bundy-outline.ps1` create the editable `graphics/current/bundy-angular-outline.svg`, transparent PNG, and black-background preview. `scripts/replace-bundy-lettering.jsx` embeds the SVG as a Photoshop smart object and removes the three rejected Impact layers. The black exterior-strip layer and UV position are unchanged.
- Checkpointed the pre-correction master at `archive/current-before-bundy-font-correction-20261017/rick-cel-shaded.psd`. Refreshed Rick's current paint/spec and Justin's #E55D01 preview TGA pair, installed Rick's corrected pair under 844824, and verified both installed hashes. The corrected flat reading preview is `design/diagnostics/bundy-windshield-readable.png`. A front car-viewer image remains the projection check.

## 2026-10-17 — BUNDY exterior windshield banner

- User requested a black windshield banner with blocky cel-shaded BUNDY lettering inspired by the yellow 911 reference, specifically on the outside-facing strip rather than the strip visible from inside.
- In the original 718 template, the blue Michelin/Pilot Challenge strip is the exterior-branded UV island (about x1004–1090, y998–1541); the adjacent dark strip is the inside-facing candidate. `scripts/render-windshield-banner.ps1` derives a curved black mask from the branded strip's original pixels. `scripts/apply-bundy-windshield-banner.jsx` added it as a new shared group with three editable Impact text layers for white block letters and stepped blue-gray cel shadows. The neighboring strip was left untouched.
- Checkpointed the preceding authored master at `archive/current-before-bundy-banner-20261017/rick-cel-shaded.psd`. Refreshed the current Rick paint/spec exports and Justin #E55D01 preview pair from the updated shared master. Installed Rick's pair under customer ID 844824 and verified both installed hashes match their exports.
- Inspected the flat UV banner in `design/diagnostics/bundy-windshield-uv.png` and its rotated reading check in `design/diagnostics/bundy-windshield-readable.png`. The template evidence supports the chosen exterior strip, but the final car-viewer projection and reading direction still need a front view.

## Reusable artwork and next-livery notes

- Extracted two `2 Old 2 Slow` door-logo smart objects, two editable bumper-text layers, and the `Me outlined` smart object from the untouched `MWM Season 12/Porsche 992_2 Cup-Recovered.psd`. Saved individual transparent PNGs and editable PSDs, plus a combined original-placement PSD, under `Paints/general-logos/2-old-2-slow/` with a source manifest. Those UV positions and orientations belong only to the 992 Cup template.
- Updated `shared/cars/porsche718gt4/mapping/notes.md` with a short start sequence for the next 718 GT4 livery. The detailed mapping and automation lessons below remain the reference for this project; provisional contours still require car-viewer confirmation.

## Current: shared colors and contour/shading refinement

- User confirmed the fresh trace was a major improvement and requested stronger ducts, handle/body contours, roof detail, and additional shading. Their yellow 911 references supplied the style direction.
- Updated the current PSD, preserving its eight traced regional primary layers. Added independently placed handle frames and shoulder marks on each side, curved lower-door shadow facets, sill shadows, roof-edge shading/hatch ticks, and heavier duct strokes derived from local green-wire edges. New secondary paths remain provisional until seen on the car.
- Replaced the baked body color with two actual Solid Color Fill layers clipped to one shared white surface. Rick later selected a personal color directly in Photoshop; Justin selected bright orange. Shared Multiply shadows and Screen highlights adapt to either color. No duplicated art groups.
- Added `docs/color-controls.md`. `RefineSharedArt.cs` creates the new detail layers. `refine-shared-current.jsx` is a one-time conversion; do not rerun over an already converted master. `export-shared-current.jsx` is the reusable exporter from the current master.
- Verified actual SOLIDFILL kinds/clipping and saved hierarchy. Detected stale composites in direct sequential Photoshop saveAs exports, then switched to refreshed merged snapshots and checked actual base/shadow colors. The current Rick paint/spec exports and Justin preview are generated from this master. New detail placement still needs viewer review.
- Initial output checks confirmed body gloss at RGB 0,37,0, with the ink mask increasing roughness. Both TGAs are 2048×2048, 24-bit RLE. Only the current Rick export pair is retained; Justin is a PSD color control and preview at this stage.
- User then chose a personal color directly in the Rick PSD. Preserved that swatch, renamed its layer without changing fill pixels, exported from the user's swatch, and installed the matching Rick paint/spec pair. Documentation no longer embeds the earlier fixed Rick color. Project folder is now `cel-shaded/`.
- User clarified that “my number” means iRacing customer ID in the filename. Exported Justin's orange fill as `car_844824.tga` and shared spec as `car_spec_844824.tga`, without changing the layered master or canonical Rick pair. Installed this temporary preview under Rick's ID so it loads on his local car. Future cross-driver previews need the requesting driver's customer ID as the filename suffix.
- User selected Justin hue #E55D01. Added a separately editable, clipped fill for the new trial color while preserving Rick's personal swatch and Justin's previous orange fill hidden for comparison. Exported the new Justin trial under Rick customer ID 844824 and installed it for another in-car preview. The sampled base pixel is RGB 229,93,1; the canonical Rick export remains unchanged.

## Previous: fresh full-car trace

- User rejected all hand-fitted artwork and asked to start fresh, emphasizing that each side must use its own geometry. Opened the original template and exported its isolated Wire layer. Extracted the green boundaries directly at native 2048×2048 resolution; no side reflection or fitted primary seam paths are used.
- Created new green body/wing paint, eight regional primary-ink layers, and a separate layer of short offset echoes and corner hatching. All prior handmade ink, dark-wing fill, and shade bars are absent from the fresh PSD. The number/banner remain unpainted, and no sponsor artwork was added.
- Excluded the radiator/crest texture islands inside the rear-glass bay and rear-badging atlas from the trace. Some UV edges may still need localized removal after viewer review because UV boundaries do not always correspond to a visible panel gap.
- Source implementation: `scripts/TracePanelInk.cs`; one-time fresh build: `scripts/rebuild-traced-current.jsx`. Future user edits should start from the current master; do not blindly rerun the fresh-build script over manually edited layers.
- Confirmed the saved PSD has its regional layers and guides off. Checked both current TGAs are 2048×2048, RGB 24-bit RLE, body spec sample is RGB 0,37,0, and installed Rick TGA hashes match exports. Only the current export pair is retained. No installed-paint backup was made. Full-car viewer review remains outstanding.

**Current storage rule:** keep one editable PSD at `source/rick-cel-shaded.psd` and one paint/spec TGA pair under `exports/current/`. Earlier export directories named in the historical notes below were removed at the user's request; their references record what was tested, not files that still exist.

## r000 — project setup and palette proposal

- Parent: none.
- Sources: untouched `inputs/119_template_P718.zip` and extracted `inputs/Porsche 718 GT4.psd`.
- Changes: created the project folder and documented the proposed green palette and visual direction.
- Preview: `design/previews/palette-r000.svg`.
- Checks: ZIP contains one 2048×2048, 8-bit RGB PSD; Photoshop 2026 executable is installed; iRacing paint directory includes `porsche718gt4`.
- Photoshop automation: no connected Codex document session was found. Two attempts to connect through the Photoshop COM object stalled and were cancelled. Actual edit/save/TGA export route remains unverified.
- User review: Rick selected as first recipient; user confirmed `119_template_P718.zip` and chose no Git. Palette, finish, number, and banner treatment are pending.
- Next action completed in r001.

## r001 — first Rick paint draft

- Parent: r000. The unmodified original remains under `inputs/`.
- Source: `source/rick-cel-shaded.psd`; the editable artwork is in separate `[RICK]` body, olive shadow, lime highlight, and charcoal ink layers above the original template layers.
- Artwork sources: `graphics/body-r001.png`, `shadows-r001.png`, `highlights-r001.png`, and `ink-r001.png`, rendered from `scripts/RenderOverlays.cs` using `design/previews/template-clean-mask-r000.png`. The source artwork stays in the project for regeneration and editing.
- Paint export: `exports/r001/car_844824.tga`; spec export: `exports/r001/car_spec_844824.tga`. Flat views: `design/previews/paint-flat-r001.png` and `spec-flat-r001.png`.
- Local installation: the paint and spec TGA were copied into `C:\Users\bundy\Documents\iRacing\paint\porsche718gt4`. The replaced `car_844824.tga` and previous `car_spec_844824.mip` are backed up in `archive/preinstall-r001/`. Justin's files were not touched.
- Verification: Photoshop 2026 opened and edited the 2048×2048 RGB template, saved a layered PSD, and exported readable 24-bit RLE TGAs. The installed TGA hashes match the revisioned exports. Flat preview shows no added logos, number, or banner text and no template guides. The spec image uses the template's red metallic, green roughness, and blue clearcoat composition; a sampled body pixel is RGB 0,37,0, consistent with a glossy nonmetal finish.
- Limits: flat UV inspection does not verify seams or 3D appearance. The sim may stamp mandatory markings or a race number over the texture.
- Spec conversion: iRacing generated `car_spec_844824.mip` after the installed TGA; its timestamp is newer than the TGA. The prior MIP remains in the project backup.
- User review: pending front three-quarter, both side, rear three-quarter, and top screenshots from the iRacing Paint Shop or car viewer, with any desired changes to green, line density, or shading.
- Next: inspect those screenshots, correct the mapped artwork, and produce r002.

## r002 — unified color and contour rebuild

- Parent: r001; its layered master was checkpointed to `archive/r001/rick-cel-shaded.psd` before replacement.
- Feedback: user supplied side, top/front, opposite side, and rear screenshots. Broad olive areas landed on inconsistent body panels, the black lines did not follow the physical contours, and the result lacked the loose cel-shaded look of the 911 references. User said future server-downloaded paint files need no local backup.
- Source: `source/rick-cel-shaded.psd` rebuilt from the untouched original template. New editable `[RICK]` layers hold the uniform green body, charcoal contour/wing art, and selective satin roughness for the ink.
- Artwork sources: `graphics/body-r002.png`, `ink-r002.png`, and `ink-spec-r002.png`, generated by `scripts/RenderR002.cs` from the clean paintable-area mask. `scripts/render-r002.ps1` can regenerate them. The broad r001 olive shadows and lime highlight bands were removed. The r002 contour ink sits inside mapped UV boundaries and adds short offset sketch marks; its spec treatment raises roughness only under the ink and wing.
- Exports: `exports/r002/car_844824.tga` and `car_spec_844824.tga`; flat views `design/previews/paint-flat-r002.png` and `spec-flat-r002.png`.
- Local installation: both r002 TGAs replaced Rick's r001 review files in `C:\Users\bundy\Documents\iRacing\paint\porsche718gt4`. Per the user's direction, no new backup was made. Justin's files were not touched.
- Checks: Photoshop saved the layered master and both 2048×2048, 24-bit RLE TGAs. Installed TGA hashes match exports. Body spec sample is RGB 0,37,0; wing ink sample is RGB 0,133,0, giving the ink more roughness while the body stays glossy. iRacing generated a newer `car_spec_844824.mip` from the r002 TGA.
- Review still needed: the flat UV preview confirms that broad mismatched color blocks are gone but cannot establish final 3D seam alignment or comic line quality. Await refreshed side views from both sides and a front/top view. Correct remaining marks in r003.

## r003 — Clyde parts-map panel line draft

- Parent: r002; its authored layered master was checkpointed to `archive/r002/rick-cel-shaded.psd`. This is a design checkpoint, not a backup of a server-downloaded paint.
- Correction: the user's reference to Clyde was to [Porsche 718 GT4 Parts](https://www.tradingpaints.com/showroom/view/241344/Porsche-718-GT4-Parts), not the cel-shaded 911 artist. Clyde's two images are saved under `design/references/` for mapping, and show labeled UV islands plus their positions on the 3D car.
- Source: `source/rick-cel-shaded.psd` now has new editable `[RICK] Hand drawn panel ink r003` and paired satin spec layers. The r002 UV-edge ink layers remain in the PSD but are hidden. The uniform green body remains.
- Artwork: `scripts/RenderR003.cs` draws broken, offset charcoal strokes on Clyde's mapped door, side intake, sill, hood, roof, bumper, and wing islands. Source rasters are `graphics/ink-r003.png` and `graphics/ink-spec-r003.png`.
- Exports: `exports/r003/car_844824.tga` and `car_spec_844824.tga`; previews `design/previews/paint-flat-r003.png` and `spec-flat-r003.png`. Both TGAs are 2048×2048, 24-bit RLE.
- Installation: Rick's r003 paint and spec replaced the local r002 review files. No downloaded-paint backup was made, per the user's instruction. Installed files match project exports by SHA-256. Justin's files remain untouched.
- Visual review: the 718 template and Clyde's map establish the correct UV islands, but the new line alignment and visual weight still require an in-game side view from each side plus a front/top view. Do not treat the flat preview as 3D verification.

## r004 — screenshot-led ink cleanup

- Parent: r003; its authored layered master is at `archive/r003/rick-cel-shaded.psd`.
- Feedback: the user's r003 side and front/top screenshots showed meaningful panel placement progress, but long floating strokes above the rear wheels, a smooth arc across the hood, and overly regular roof marks still distracted from the sketch style.
- Changes: split or shortened those strokes, added a few shorter arch cues, and introduced one restrained green shadow tone on the mapped lower door and intake areas. Kept the established green body and dark wing. Source paths are `scripts/RenderR004.cs`; raster layers are `graphics/ink-r004.png` and `ink-spec-r004.png`.
- Source and exports: `source/rick-cel-shaded.psd` includes the new editable r004 layers; r003 layers remain hidden. Paint and spec are `exports/r004/car_844824.tga` and `car_spec_844824.tga`, with flat previews under `design/previews/`.
- Checks: both TGAs are 2048×2048, 24-bit RLE. They were installed for Rick only and their hashes match the exports. No server-downloaded paint backup was made. The r004 result awaits a new in-game visual check.

## r005 — hood and rear-arch correction

- Parent: r004; authored source checkpointed at `archive/r004/rick-cel-shaded.psd`.
- Feedback: user's r004 views from both sides and above show that the panel layout is closer, but the transverse double line on the hood reads as a stripe and the rear-wheel marks remain detached from the arch.
- Changes: removed the transverse hood pair, shifted the short rear-wheel marks toward the arches on both side islands, and added subtle irregularity to the Bezier stroke paths. Base green, green shade, spec intent, and other mapped panel lines remain.
- Art and source: `scripts/RenderR005.cs`, `graphics/ink-r005.png`, `graphics/ink-spec-r005.png`, and the layered `source/rick-cel-shaded.psd`. Prior r004 layers are hidden in the PSD.
- Export and install: `exports/r005/car_844824.tga` and `car_spec_844824.tga` are 2048×2048, 24-bit RLE; both were installed for Rick and hash-verified. Justin's paint was not changed. Await in-game review of the new placement.

## d001 — temporary coordinate mapping paint

- Feedback: user's r005 views show ink entering the driver-side brake duct, incorrect hood placement, and a passenger door treatment that appears reversed. A further art revision would guess at orientation.
- Temporary file: `design/diagnostics/car_844824-coordinate-diagnostic.tga`, generated from `scripts/RenderDiagnostic.cs` and `scripts/export-diagnostic.jsx`. It labels UV cells A (upper side), B (lower side), H (hood), R (roof), and F (front bumper), with increasing x and y indicated on each island.
- Installation: temporarily replaced Rick's local paint TGA only, hash-verified. `source/rick-cel-shaded.psd` and the r005 paint/spec exports are unchanged; r005 spec remains installed. This diagnostic is intentionally outside `exports/` and is not final livery artwork.
- Pending: collect readable viewer screenshots of both sides and front/top so the door orientation, hood edges, and brake-duct cells can be identified. Then restore Rick's design export after producing the corrected revision. The r005 export is available for immediate restoration if requested.

## Current PSD and export consolidation

- User required an editable PSD and one current set of exports rather than every TGA revision.
- Inspected the 2048×2048 RGB, 8-bit Photoshop master. It was already a layered PSD, not a flattened TGA, but hidden prior-version artwork cluttered its paint and spec groups.
- Reorganized current artwork into separate editable pixel layers: `[RICK] Body — electric green`, `[RICK] Green cel shade`, `[RICK] Dark wing`, and `[RICK] Hand drawn ink`. The selective finish remains in the `Custom Spec Map` group. Original Porsche template groups remain available.
- Exported the cleaned PSD to `exports/current/car_844824.tga` and `car_spec_844824.tga`; both are 2048×2048, 24-bit RLE. The flat preview visually matches the previous r005 preview.
- Deleted the old `exports/r001` through `exports/r005` directories after verifying their paths were inside this project's `exports` folder. The temporary coordinate diagnostic remains installed in iRacing while awaiting screenshots; it is outside `exports/current/` and did not enter the PSD.

## d002 — complete UV coordinate diagnostic

- Feedback: the first diagnostic left the wing, trunk/rear deck, rear bumper, brake ducts, front spoiler, and front side panels unlabeled. User screenshots confirmed side A appears upside down on the passenger side, side B upright on the driver side, and the hood grid rotates on the car.
- Replaced the partial overlay with a 16×16 coordinate grid over the complete 2048×2048 template. Every cell has a unique `x,y` label; quadrant colors and 64-pixel cross-lines make smaller pieces easier to locate.
- Re-exported the temporary diagnostic from the current PSD without saving the grid into the PSD or changing `exports/current/`. Installed the new TGA for Rick and verified it is 2048×2048, 24-bit RLE with a matching installed hash.
- Pending: front, rear, and overhead/rear viewer images with readable grid labels to locate the remaining small parts and seams. Once mapped, replace the diagnostic with corrected art from the editable PSD.

## Current mapped correction — full-grid screenshot review

- User supplied six readable views: both sides, front/top, rear, rear overhead, and whole-car overhead. These establish that both side islands run front to rear as UV x increases; the passenger island reverses vertical UV direction relative to the driver island. The hood runs from nose around x3 to windshield around x6. The grid also identifies the wing, roof/rear deck, front and rear bumpers, brake ducts, and lower spoiler regions.
- `scripts/RenderMappedCurrent.cs` generated new side and hood ink, restrained lower-door shade, and the matching selective spec layer under `graphics/current/`. The passenger art uses the opposite vertical orientation. Front bumper strokes and the ink cluster at the side duct were removed after UV overlay inspection because they crossed separate mapped parts. The dark wing and uniform green body remain separate editable layers.
- `scripts/update-mapped-current.jsx` updated the current master PSD without rebuilding it from the original template. It replaced only the editable ink, shade, and spec layers, then wrote the single current paint/spec export pair and flat previews.
- Verified the PSD in Photoshop: 2048×2048 RGB, 8-bit, with separate body, shade, wing, ink, and spec layers and the original template groups present. Both TGAs are 2048×2048, 24-bit RLE. Replaced Rick's temporary diagnostic in the local iRacing paint folder with the current design paint and spec, then verified matching SHA-256 hashes. No installed-file backup was made, per user direction. Justin's paint was not touched.
- Pending: viewer review of the corrected art. The flat UV preview and hashes establish export integrity but cannot confirm final 3D line placement. Remaining small parts are mapped by the diagnostic and are left without speculative contour strokes for this pass.

## Wire-aligned hood and driver-door correction

- User's in-game views showed the driver-door line climbing toward the windshield and a hood reduced to scattered strokes. The user pointed out that the PSD's Wire layer provides the green UV island and panel edges and the white mesh provides surface shape.
- Compared the artwork at native 2048×2048 pixel alignment against `design/previews/template-wire-r000.png`. The driver door front boundary begins near UV x910,y1805; the prior ink began at y1692 on the adjoining body. Moved its start to the mapped edge and kept the lower return on the door. Passenger-side vertical orientation was retained.
- Rebuilt both hood contour pairs to follow the curved green edges within the hood island. The pair now extends from near the nose to near the windshield with short offset echoes. Removed the isolated nose flicks.
- Updated the same layered master PSD and the single current paint/spec TGA pair. Verified both TGAs are 2048×2048, 24-bit RLE and their installed Rick copies match by SHA-256. The in-game result still needs a refreshed viewer check.

## Reference-led seam and panel revision

- User's next car views showed that the latest hood pair still lacked the 911 reference's seam-led design. The door-front vertical strokes did not describe a useful visible feature and were obscured by the automatic number; the lower-door shade read as a rectangular bar.
- Re-examined the original yellow 911 reference screenshots and the 718 wire preview. The revised main hood line traces the rounded nose and both green hood-side boundaries; shorter offset strokes add hand-drawn detail near those curves. The front vertical door strokes and the rectangular lower-door shade were removed. Only short rocker accents remain below the number area and before the side intake.
- Updated the layered Photoshop master and the single current paint/spec TGA pair, then installed them for Rick. Both TGAs passed 2048×2048 24-bit RLE header checks and installed SHA-256 hashes match the exports. The in-game appearance of this revision still needs review.

## 2026-10-17 — cap, handle, flap, windows, lamps and rear detail

- The user's blue-car screenshots confirmed that the previous passenger-handle move landed on the fuel cap. Corrected the passenger handle center to approximately (1372,750), based on its white mesh recess. Replaced the cap-crossing frame with four separated brackets outside the circular face.
- Removed the floating roof curves. Added parallel sketch strokes outside the escape flap, with a narrow neutral shadow along its edge. Moved rear-glass echoes onto the painted surround and added separate accents at both rear quarter-window curves.
- Added offset contours around front headlight pockets and rear lamp/body boundaries, plus rear-bumper relief lines and neutral shading. Added a trial shadow facet behind the GT4 MR embossed area on the separate front-spoiler surface; this does not redraw the individual letters.
- Updated only the generated shared contour, neutral shadow/highlight and matching spec layers in `source/rick-cel-shaded.psd`. The main seam layers and editable driver color fills are retained. Rick's sampled body color remains RGB 72,199,232; Justin's is RGB 229,93,1 (#E55D01). Source checkpoint: `archive/latest-feedback-checkpoint/rick-cel-shaded.psd`.
- Refreshed the current Rick paint/spec pair and Justin's preview pair. Installed the **Rick blue** pair under customer ID 844824 in the established iRacing paint folder. Both current TGAs are 2048×2048, 24-bit RLE, and the installed SHA-256 hashes match. Only current export filenames are maintained.
- Flat overlays were inspected against the native wire map. All new offsets still need a car-viewer check, especially the passenger handle, lamps and spoiler shading. The installed spec MIP was older than this export at verification; iRacing still needs to regenerate it.
- Photoshop automation is working via the approved elevated route. The previous repeated sandbox COM activation attempts were the cause of the apparent loop. Reusable mapping and automation corrections are recorded in the shared car notes.

## Fuel-cap circle refinement

- User confirmed the preceding pass is much better and requested circular cap accents instead of the four corner brackets.
- Replaced those brackets with a thin complete ring and a lighter partial outer ring, following the cap's measured UV ellipse (bounds x786–871, y751–811). Both rings remain outside the cap edge.
- Updated only the shared contour layer and matching spec layer in the layered master. Refreshed Rick's current exports and Justin's #E55D01 preview files. Installed Rick's blue paint/spec pair under 844824 and verified matching file hashes. Inspected the exported cap crop; the brackets are gone. Final projected circle shape remains subject to viewer review.
