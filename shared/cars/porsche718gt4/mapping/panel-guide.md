# Panel and feature lookup

Use this with [overlay.png](overlay.png), [panels.json](panels.json), and [seams.json](seams.json). IDs label reference components, not an asserted one-to-one inventory of physical panels. All masks are provisional JPEG-derived selections. Confirmed below means the named physical surface is established, not that every boundary or orientation is calibrated.

Bounds are native pixels `[x0,y0,x1,y1)`; x points right and y down. Both sides are named from the seated driver, not the camera.

| ID | Physical region | Source bounds | Identity | Direction / limit |
| --- | --- | --- | --- | --- |
| I002 | Shared vertical front louver surround ahead of both front wheels | [679, 6, 795, 131] | confirmed | Large 2 appears on both sides: shared source. Driver-side digit upright; passenger-side digit reversed/inverted. An asymmetric letter/axis check is needed to distinguish reflection from rotation exactly. Separate small fins have separate UV pieces. |
| I004 | Rear wing underside and wrapped rear lip | [6, 9, 636, 227] | confirmed | Underside uses 004, including the orange lip visible above. Arrows point toward driver/left across span when viewed from rear; exact top/bottom edge correspondence remains uncalibrated. |
| I007 | Rear bumper outer shell | [978, 10, 2042, 282] | confirmed | +x travels from driver side toward passenger side around rear; +y goes down the rear face. Corners distort. |
| I009 | Shared front brake-duct outer surround, both sides | [801, 58, 1036, 169] | confirmed | The same 009 appears on both front openings. Driver-side digits upright; passenger-side digits rotated 180 degrees. +x points outward toward each front wheel. Independent left/right graphics are impossible on these shared texels. |
| I020 | Shared front brake-duct inner return | [590, 154, 846, 277] | confirmed | 020 appears on the vertical inner return behind the 009 surround in both openings. Duct mesh stays separate. Complete interior extent and rotation need more sampling. |
| I021 | Rear bumper center inset | [1304, 175, 1715, 309] | confirmed | 021 reads upright from behind; +x goes toward passenger/right, +y down. The tow strap and circular tow opening remain separate. |
| I030 | Rear wing top | [12, 234, 631, 445] | confirmed | +x runs passenger/right to driver/left across wing; digits upside down from rear, so +y runs toward vehicle front on the visible upper face. |
| I040 | Rear deck trailing lip, upper face | [1604, 308, 2042, 395] | confirmed | 040 is the orange horizontal ledge above the tail lamp/badge strip; distinct from the elevated rear wing. +x points toward driver/left; digits upside down when viewed from rear. |
| I041 | Rear deck trailing lip, rear-facing face | [1126, 311, 1613, 349] | confirmed | 041 is the narrow face just above PORSCHE. +x points toward driver/left; lettering upside down from rear. This is not the wing underside. |
| I044 | Driver/left wing outer endplate | [317, 387, 467, 459] | confirmed | 044 upright from outside driver side; +x points toward rear, +y down. |
| I045 | Passenger/right wing outer endplate | [151, 388, 299, 458] | confirmed | 045 upright from outside passenger side; +x points toward front, +y down. Do not copy driver text by reflection. |
| I063 | Passenger/right wing inner endplate | [153, 465, 301, 533] | confirmed | 063 upright on inner face. Exact fore/aft arrow direction is not readable in these views. |
| I064 | Driver/left wing inner endplate | [315, 465, 463, 533] | confirmed | 064 upright on inner face. Exact fore/aft arrow direction is not readable in this view. |
| I078 | Passenger mirror outer shell | [1813, 499, 1986, 657] | confirmed | 078 on outer/top shell, locally upright from passenger side; arrows go toward vehicle front. Underside black insert and stalk are separate; far face not yet observed. |
| I080 | Passenger side intake outer surround | [668, 504, 817, 688] | confirmed | 080 upright from passenger side; +x points rearward, +y down. This differs from the upside-down passenger main-body texture. |
| I081 | Passenger main body: fender, door, sill, pillars and rear quarter | [394, 507, 2009, 1032] | confirmed | +x front to rear; +y up the physical side. Rotate upright text 180 degrees relative to the driver side, never reflect lettering. |
| I116 | Front bumper main skin | [144, 730, 400, 1810] | confirmed | +y goes from passenger/right across nose toward driver/left. At the central face +x goes upward toward hood. Lamp joins pending. |
| I119 | Front lower splitter | [9, 787, 156, 1753] | confirmed | Narrow left-edge strip. Cells 0,9 and 0,10 visible across nose; +y goes toward driver/left. Lip thickness and returns pending. |
| I120 | Passenger door handle outer face | [802, 887, 913, 926] | confirmed | 120 upright from passenger side. The handle recess is on main body; the physical handle face is this separate island. Text follows a curved horizontal face. |
| I121 | Passenger side-intake inner return candidate | [849, 930, 919, 950] | provisional | Cyan diagnostic appears behind the gray intake outer shell; full 121 ID is not readable. Paired source geometry supports this candidate only. |
| I122 | Hood | [365, 977, 947, 1563] | confirmed | +x nose to windshield; +y passenger/right to driver/left. Front-readable upright logo rotates 90 degrees clockwise in UV. |
| I125 | Exterior windshield banner | [1012, 1005, 1085, 1534] | confirmed | Runs across car along UV y. Readable front-view artwork uses top toward +x and reading direction +y. Visible crop limits remain unmeasured. |
| I126 | Interior windshield banner | [932, 1006, 1003, 1534] | provisional | INSIDE is printed on the guide. Cockpit projection, reading direction and visible crop need an interior view. |
| I128 | Roof | [1091, 1042, 1520, 1497] | confirmed | +x windshield to rear glass; +y passenger/right to driver/left. Escape flap is a feature within this island. |
| I129 | Rear deck and rear-glass painted surround | [1522, 1046, 2039, 1495] | confirmed | +x roof to tail; +y passenger/right to driver/left. The center is not body paint; atlas components sit inside its UV opening. |
| I131 | Grille/radiator texture atlas inside rear-glass UV opening | [1655, 1072, 1829, 1245] | unknown | Template content identifies an atlas texture. Physical use or repeated instances unknown; not rear-glass paint. |
| I132 | Porsche crest texture atlas inside rear-glass UV opening | [1667, 1250, 1824, 1451] | unknown | Template content identifies a crest atlas. Physical instances and mirroring unknown; not rear-glass paint. |
| I134 | PORSCHE rear lettering texture atlas | [1906, 1500, 2019, 1537] | confirmed | Rear letters pick up diagnostic pink from this atlas. Each glyph uses a small texture area: do not draw a body stripe through the atlas. Smaller badging lines remain unassigned. |
| I136 | Driver main body: fender, door, sill, pillars and rear quarter | [394, 1508, 2011, 2033] | confirmed | +x front to rear; +y down the physical side. Text upright in UV reads upright in the side view. |
| I137 | Rear badging atlas: lower lines | [1907, 1532, 1987, 1558] | unknown | Texture atlas; not a body panel. |
| I139 | Rear badging atlas: bottom line | [1907, 1559, 1978, 1564] | unknown | Texture atlas; not a body panel. |
| I141 | Driver side-intake inner return candidate | [848, 1590, 920, 1611] | provisional | Green diagnostic appears behind the gray outer shell; digit fragments are insufficient for an exact-face assignment. Confirm full 141 or use one isolated patch. |
| I143 | Driver door handle outer face | [802, 1613, 913, 1654] | confirmed | 143 upside down from driver side; handle face separate from main-body recess. Use a 180-degree text rotation relative to the diagnostic. |
| I153 | Driver side intake outer surround | [667, 1852, 817, 2036] | confirmed | 153 upright from driver side; +x points rearward, +y down. Separate from side body. |
| I156 | Driver mirror outer shell | [1813, 1881, 1986, 2040] | confirmed | 156 locally upside down from driver side, arrows point toward rear. Underside black insert/stalk separate; far face not yet observed. |

## Features within islands

| ID | Feature | Source coordinates | Status | Limit |
| --- | --- | --- | --- | --- |
| F01 | Passenger fuel-cap face (I081) | [786, 751, 872, 812] | confirmed | Oval source becomes circular on car. This is not the door handle. Native green-wire bounds; accent safe area is outside face. |
| F02 | Passenger handle recess (I081) | [1372, 750] | provisional | White mesh recess; outer handle may use a separate atlas piece. Earlier y771 treatment misplaced. Do not confuse with fuel cap. |
| F03 | Driver handle recess (I136) | [1372, 1787] | provisional | White mesh recess; exact projected stroke bounds unverified. |
| F04 | Roof escape flap (I128) | [1217, 1263, 1467, 1462] | provisional | Physical flap confirmed; approximate source bounds retained from wire review. Same roof texture, not a separate paint island. |
| F05 | Driver door number reserve (I136) | [942, 1852, 1051, 1925] | confirmed | Template number-guide footprint, half-open bounds; text upright in UV. Reserve additional header/footer margin if adding panels. |
| F06 | Passenger door number reserve (I081) | [942, 615, 1051, 688] | confirmed | Template number-guide footprint; text must rotate 180 degrees, not mirror. |
| F07 | Rear bumper number reserve (I007) | [1248, 71, 1357, 144] | confirmed | Template footprint, independently measured from magenta pixels. Text upright from rear; keep body graphics clear of the sim-number area. |

## Protected and unresolved components

The original Car Mandatory layer covers at least 95% of each component marked protected_template_overlay in panels.json. These selections are reserved from livery placement even when their exact physical fixture is unnamed. Use the native mandatory alpha, not JPEG fringes, for exact protection. This is a measured template classification, not a claim that every hidden fixture has been viewed on the car.

The unresolved components without that protection are listed below. Do not guess their physical surface or transform. A repeating guide color is not proof of shared UVs.

| ID | Candidate / unknown | Bounds |
| --- | --- | --- |
| I008 | Unidentified reference component I008 | [640, 31, 678, 66] |
| I010 | Unidentified reference component I010 | [643, 69, 682, 111] |
| I011 | Unidentified reference component I011 | [703, 84, 728, 115] |
| I012 | Unidentified reference component I012 | [731, 86, 753, 112] |
| I017 | Unidentified reference component I017 | [779, 136, 1048, 224] |
| I048 | Unidentified reference component I048 | [1084, 404, 1223, 497] |
| I083 | Unidentified reference component I083 | [1965, 513, 2021, 703] |
| I084 | Unidentified reference component I084 | [2036, 513, 2039, 728] |
| I086 | Unidentified reference component I086 | [111, 528, 506, 587] |
| I121 | Passenger side-intake inner return candidate | [849, 930, 919, 950] |
| I126 | Interior windshield banner | [932, 1006, 1003, 1534] |
| I130 | Unidentified reference component I130 | [92, 1050, 168, 1489] |
| I137 | Rear badging atlas: lower lines | [1907, 1532, 1987, 1558] |
| I139 | Rear badging atlas: bottom line | [1907, 1559, 1978, 1564] |
| I141 | Driver side-intake inner return candidate | [848, 1590, 920, 1611] |
| I149 | Unidentified reference component I149 | [2036, 1812, 2039, 2027] |
| I151 | Unidentified reference component I151 | [1964, 1836, 2021, 2026] |

Contact sheets: [sheet 1](previews/atlas-01.jpg), [sheet 2](previews/atlas-02.jpg), [sheet 3](previews/atlas-03.jpg), [sheet 4](previews/atlas-04.jpg), [sheet 5](previews/atlas-05.jpg), [sheet 6](previews/atlas-06.jpg), [sheet 7](previews/atlas-07.jpg).
