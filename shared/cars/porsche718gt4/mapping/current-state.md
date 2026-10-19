# Mapping session state

Updated 2026-10-19. Scope: complete the reusable car mapping for future liveries. Branch: `feature/complete-718-mapping`. Existing cel-shaded work and user edits were left in place with explicit authorization; no livery source/export files were changed by this audit.

## Sources and evidence

- Exact 2048-square template hashes match the original ZIP member: [identity](template/identity.json).
- Recovered original G01-G06 grid views, E01-E07 livery evidence, D01-D11 small-parts views, H01/H02 headlight views, K01-K06 remaining-part views, and X01 d006 front-louver confirmation: [evidence.json](evidence.json).
- Corrected old description of Clyde image 2: it is the flat parts atlas.
- Added 156 candidate component masks, measured bounds, seven source placement features, a Pitbox reservation, and 20 seam records. Components include atlas fragments, not 156 physical panels.
- Placement policies: 28 identified components; 111 protected by the original Car Mandatory overlay; 17 unresolved. Exact physical names for protected hardware remain unknown where not visible.

## Active diagnostic

- **Original paint is restored** for Rick 844824 after the d006 review. [installed-session.json](diagnostics/installed-session.json) records the verified restore and last diagnostic.
- Original installed paint was backed up and hash-verified. Spec TGA/MIP were not changed.
- d006-exterior-codes was used for the exterior review, then the original paint was restored. Its letters simplify the tiny labels and its thin strips have white ticks.
- Restore with `scripts/preview-diagnostic.ps1 -Action Restore` after the last review. It checks the currently installed hash before replacing anything.

## Limits and decisions

- No universal side mirror transform. Separate mirrors/handles/intakes have independent directions.
- Both front duct surrounds/inner returns share source texels; both front vertical louver surrounds share I002.
- Lamp lower returns are now located by saved 32px cells, but precise matched fender/bumper endpoints remain uncalibrated.
- Geometry masks remain provisional reference segmentation. Native mandatory-alpha protection is measured separately.
- Cockpit-only review is deferred because the current viewer makes it difficult and launching the game is a substantial interruption for the user. Do not claim the inside banner is verified.
- `mapping/image.png` appeared during the session and was left untouched; it duplicates the rear diagnostic view already saved as D08 evidence.

## Validation

See [validation.json](validation.json) and [pixel-validation.json](pixel-validation.json). Evidence/template/backup/install hashes, references, bounds, image dimensions, native mask counts, and diagnostic PNG/TGA equality are checked. Full mapping remains incomplete and is not ready for final sign-off.

## Next action

X01 confirms d006 code R on the passenger-side front louver surround (I002); K01/K02 show the feature on both sides. The user reports the remaining d006 marks are tiny or tucked under/inside parts; keep those unknown components off-limits for livery artwork rather than guessing. The original paint is restored. Cockpit-only review remains deferred.
