# Creative brief

## Outcome

Create a cel-shaded iRacing Porsche 718 GT4 livery. The user wants this directory to serve as the common root, with each livery in its own subfolder. This project's folder is `cel-shaded/`.

## Visual direction

- Reference: the user's yellow Porsche 911 screenshots in the conversation, especially the near-solid vivid body color, loose black multi-stroke contour lines, broad shadow blocks around recesses and body edges, and restrained race graphics. These screenshots are inspiration, not a 718 UV map.
- The base color is controlled by a personal Solid Color Fill in the shared PSD. Keep the ink and shading compatible with the active swatch.
- Keep visual weight on the silhouette, wheel arches, side intake, door edge, bumpers, hood and rear wing. Use a few offset, broken pen strokes; avoid outlining every seam at equal weight.
- No real sponsor or brand logos in the added artwork yet. Avoid copying the reference's text and number 64.
- The current body finish is gloss and nonmetal. The texture adds a black exterior windshield banner with BUNDY lettering and shared sketch-style door number panels labeled `2Old` and `2Slow`. The gray centers are blank for iRacing's numbers. iRacing may still stamp mandatory markings in the simulator.

## Inputs and deliverables

- Starting artifact: `inputs/119_template_P718.zip`, copied unchanged from `C:\Users\bundy\Downloads\119_template_P718.zip` on 2026-10-16. It contains `Porsche 718 GT4.psd`, also extracted unchanged to `inputs/`.
- Intended editable deliverable: one layered Photoshop master in `source/`, preserving the supplied template structure.
- Review deliverable: current visual previews followed by mapped iRacing Paint Shop screenshots before finalizing the paint.
- First TGA recipient is Rick (customer ID 844824). The project keeps one current paint/spec export pair. The temporary coordinate diagnostic has been replaced by the current design in iRacing.

## Rough plan

1. Use the [shared 718 mapping notes](../../shared/cars/porsche718gt4/mapping/notes.md), the PSD Wire layer, and the viewer grid screenshots before drawing across any panel.
2. Review current contours, hood, ducts and door details on the car; refine any remaining offsets in the same layered PSD.
3. Keep only the current Rick paint/spec export set. Add a second recipient only if requested.

## Review criteria

- Reads as comic or cel-shaded from all major angles, with the chosen body color visible.
- Black contours follow 718 body features and wrap cleanly across UV seams.
- Editable PSD remains layered; exported paint has no template guides or unintended reference graphics.
- No added real logos until requested.

## Open choices

- Rick (844824) and Justin (845005) share artwork with separate color fills.
- No Git for now; keep one current editable PSD and one current export set.
- Banner text is BUNDY. Door number treatment is a loose ink frame with off-white `2Old` and `2Slow` bands around a blank gray field for the sim-generated number.
- Whether the initial gloss should gain selective matte ink or shadow treatment after in-game review.
