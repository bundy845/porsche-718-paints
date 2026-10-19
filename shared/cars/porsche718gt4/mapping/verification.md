# Mapping verification and remaining work

Updated 2026-10-19. Original full-grid screenshots are recovered and saved, together with new small-part close-ups. **The full no-guessing mapping is not yet complete.**

| Area | Evidence | Established / limit |
| --- | --- | --- |
| Main sides, hood, roof, rear deck | G01–G06 | Identity and broad orientation; revised side bounds include fenders/pillars omitted by old coarse table. |
| Front bumper / splitter | G01–G03, C01/C02 | Separate islands, across-nose direction. Lamp joins and wrap endpoints pending. |
| Rear bumper / center insert | G04, D07/D08 | Separate I007/I021; center upright, +x toward passenger. Exact join pending. |
| Mirrors / handles / side intakes | D01/D05 | Passenger 078/120/080, driver 156/143/153; independent transforms. Mirror far faces/inserts/stalks pending. |
| Front ducts | D02/D03 | Shared outer I009 and inner I020; I009 reverses 180 degrees. Inner-return extent pending. |
| Wheelward front vertical louver | D02/D04 | Diagnostic color visible, complete ID unreadable. Do not assign from color alone. |
| Wing | D06–D11 | Top 030, underside/lip 004; driver outer/inner 044/064; passenger outer/inner 045/063. Inner-face fore/aft direction pending. |
| Rear-deck lip | D06–D09 | Upper 040 and rear-facing 041 separate from elevated wing; lettering inverted from rear. |
| Rear lettering | D06–D09, T01 | PORSCHE uses I134 atlas color; other badging lines/glyph extents pending. |
| Exterior banner | C01/C02, E02/E07 | Identity and reading direction; earlier clipping preserved. Pixel-safe bounds unmeasured; interior needs cockpit view. |
| Numbers / fuel cap | E01/E05/E06, T02/T03 | Template number reserves/rotations; passenger fender oval is fuel cap. |

Evidence IDs resolve through [evidence.json](evidence.json), with saved files and hashes. New confirmations cite inspectable images.

### Later d004/d005 findings

- H01/H02: both headlight surrounds now have saved 32px grid views. Lower returns are on I116, distinct from I081/I136 fender portions. Observed cell regions are recorded under S04/S05; these are not exact paired endpoints.
- K01/K02 supersede the unresolved vertical-louver row above: the large `2` confirms shared I002 on both front corners. Passenger glyph is reversed/inverted relative to driver; an asymmetric-letter check is needed for its exact transform. Small fin faces remain unassigned.
- K03/K06 expose separate inner intake returns. I141/I121 are candidates from paired source geometry/color, still provisional because full IDs are unreadable.
- K04/K05 show colored mirror rims. Do not assign a narrow source strip from color alone. d006 is installed for the letter/stripe pass; X01 confirms code R is I002 on the front louver surround. Other d006 codes still need readable in-car evidence.
- User review of d006: R is on the passenger-side I002 louver. L/I130, G/I083, and J/I086 were visible in separate views, but the pieces are too small or occluded for confident physical names; a lower-right mark and yellow/orange driver/passenger strips were also unreadable. Keep these IDs, plus hidden/inside pieces, classified unresolved and off-limits for livery artwork. The verbal review is recorded as a limitation, not a screenshot-backed part identification.
- T04/T05: native PSD inventory and mandatory-overlay alpha resolve placement policy for 111 components. These are protected from livery artwork; physical names remain separate from that classification. Only 17 source components still have unresolved placement policy, including the inside banner.
- Interior review is deferred: the user reports cockpit access is difficult in the viewer and launching the game is a large interruption. No cockpit view or interior confirmation is claimed.

## Remaining completion gates

1. **Joins:** the 20 [seams.json](seams.json) entries establish adjacency but lack matched source-edge coordinates. d004 targets the difficult headlight joins first. A 32px cell bounds a region, not a precise vertex; refine with endpoint ticks where needed.
2. **Exterior pieces:** resolve vertical louvers, mirror far faces/inserts/stalks, wing supports/inner directions, grille/lip/duct returns, rear lamp trims, diffuser/tow parts and visible fasteners. Distinguish paint response from material/model surfaces and shared atlases.
3. **Technical atlas:** classify remaining unknown records as exterior, interior, shared texture, fixed/overridden, or unused with evidence. Absence from an exterior view does not establish unused/unpaintable.
4. **Hidden faces:** obtain inside-banner/cockpit views if exposed by the viewer. INSIDE wording alone does not establish projection/direction.
5. **Geometry:** replace provisional JPEG masks with registered native-wire boundaries, bleed/hole limits and distortion checks before declaring them production masks. Store paired seam points and tolerances beside evidence.
6. **Final audit:** every component has supported disposition; each text-bearing face a transform; required continuous joins matching edge samples. Check all links/hashes; restore original paint after the last diagnostic.

## Rejections to retain

Fuel cap is not handle (E01). Earlier lamp bumper paths run wheelward (E03/E04); exact rejected points are in seams.json. No car-wide reflection aligns every separate part, and reflecting text reverses it. Radiator/crest shapes inside rear-glass UV space are atlases, not body paint (T01). Flat previews, adjacent wire edges, repeated colors and unreadable labels are not physical correspondence proof.

## Next action

Review the installed diagnostic named in [installed-session.json](diagnostics/installed-session.json), collect targeted car views, and record only readable findings. Livery masters and current exports remain untouched.
