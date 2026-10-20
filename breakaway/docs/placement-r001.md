# r001 placement and evidence

Read the shared mapping README, panel-guide, panels.json, evidence.json and seams.json before changing this placement. Native coordinates are 2048-square pixels. Segmentation masks from the reference JPEG are locator aids, not production clipping masks.

| Region | Island / direction | Evidence | r001 treatment |
| --- | --- | --- | --- |
| Driver main body | I136; +x front to rear, +y down | G01, G06 | Quiet front; burst across door/rear quarter. Preserve F05 number reserve. |
| Passenger main body | I081; +x front to rear, +y up | G03, G06 | Independently bounded burst with appropriate vertical direction. Preserve F06. No universal mirror transform for other islands. |
| Hood | I122; +x nose to windshield, +y passenger to driver | G02, G06 | Quiet primary color; no detailed cross-fender join. |
| Roof | I128; +x front to rear, +y passenger to driver | G02, G05, G06 | Smaller independent rearward burst. No exact roof-rail continuity claim. |
| Rear deck surround | I129; +x roof to tail, +y passenger to driver | G05, G06 | Primary color; do not paint the internal texture atlases. |
| Rear bumper shell | I007; +x driver to passenger, +y down | G04 | Secondary color, with F07 number reserve quiet. No matched quarter seam. |
| Wing top | I030; +x passenger to driver, +y toward front | G05 | Charcoal; no directional typography. |
| Wing underside | I004; arrows toward driver across span | G04, G05 | Charcoal; exact lip boundaries remain uncalibrated. |
| Wing outer endplates | I044 +x rear; I045 +x front; +y down | Shared panel-guide evidence | Charcoal, no detailed wrapping. |
| Side intake surrounds | I080 and I153; +x rear, +y down | G01, G03, D01 | Primary color, independent of side pattern. |
| Mirror outer shells | I078 locally upright; I156 locally inverted | D01 and shared panel-guide | Primary color, no directional detail. |
| Front bumper | I116; +y passenger to driver, central +x toward hood | G02 | Primary color; no headlight-loop tracing. |
| Front splitter | I119; +y toward driver | G02 | Charcoal. |

Clipping method: connected native blue regions from the untouched template-derived clean mask, constrained to confirmed panel bounds, protected by native mandatory pixels. Do not use JPEG-derived island masks as final clipping. Adjacent unresolved regions retain their original template appearance. These masks are a first-draft boundary treatment; edge bleed and physical coverage still require in-car review.

All exact cross-panel seams S01–S20 remain provisional. r001 deliberately does not depend on a continuous individual hexagon across separate UV islands. If a join becomes design-critical, collect a numbered/ticked diagnostic and an in-car view before calling it aligned. Keep reusable diagnostic evidence in the shared car mapping folder.
