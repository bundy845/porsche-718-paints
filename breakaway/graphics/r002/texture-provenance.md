# Breakaway texture — r002

Generated using the built-in imagegen tool on 2026-10-19 for this project's paint masks. `breakaway-source.png` is the project-local original generated image. It supplies only the irregular paint breakup, not car geometry or UV mapping. It replaces the rejected hexagon/cube artwork. No reference livery artwork was copied.

The user selected the direction of their third original reference: paint breaking away from one color into another, with fine rearward distressed streaks. Retain the agreed violet/lime palette and reversed driver roles. Color choice remains editable. The original example was blue/black.

Prompt:

> Create a production source TEXTURE MASK for a racing livery: a single wide rectangular pure black-and-white grayscale bitmap, 2048 pixels wide by 1024 pixels tall if possible, fills the entire frame, absolutely NO car, no text, no border, no diagram, no labels, no logos, no shadows.
>
> Purpose: white is the second paint color appearing beneath a first paint color (black). It should resemble racing paint breaking away and eroding into a second color toward the REAR, from LEFT to RIGHT, like distressed horizontal dry-brushed scratched paint, NOT geometric shapes. This is an original abstract source texture. Crisp organic torn flecks and countless fine ragged mostly-horizontal brush scratches with varied lengths and thicknesses. Horizontal motion, delicate texture, quiet front and solid rear.
>
> Coverage progression is critical: leftmost 32 percent completely SOLID BLACK, clean, no texture. From x32% to x50%, begin sparse very small white elongated specks and horizontal short ragged chips. From x50% to x75%, increase white coverage dramatically through overlapping irregular flat shredded horizontal paint marks, a broad ragged transition, mostly fine 1-6 pixel thick scratches 5-70 px long, a few longer narrow streaks. From x75% to x88%, white dominant with thin broken black streaks and islands remaining. Last 12 percent completely SOLID WHITE. Transition boundary wanders slowly across image height rather than one straight vertical line. Strongly variable particle scale but no giant strokes, no lines that resemble a repeated grid. Keep enough fine texture to look like paint abrasion seen on a car, not a cartoon explosion.
>
> BLACK and WHITE only (minimal edge antialiasing okay). No grayscale gradient or blur. No repeated cells, hexagons, cubes, diamonds, triangles, confetti, round splashes, dripping paint, stars, checkerboard, checker texture, diagonal wedge, explosive radial center, smoke, fire, lightning. Flat orthographic source mask. This will be mapped as real artwork onto a known UV template, so the image must be solely the usable texture.

Renderer adaptation: masks are sampled by the established native-template renderer, with `sampleU = clamp((u - 0.22) / 0.78)` to keep the front quiet without cutting off texture at a hard vertical boundary. Main-side +x directions follow G01/G03; passenger v is inverted only for I081. Roof and rear-deck ranges are independently assigned from their confirmed orientations. No exact cross-island streak alignment is asserted. Matte/gloss materials use the same masks; metallic is zero throughout the new artwork.
