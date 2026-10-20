# Historical automation notes

Retired: source checkpoints and reconstruction/demo entry points described below were removed by explicit user direction during the Breakaway rename. These are historical records, not commands to run. Use automation.md for current operations.

These describe earlier revisions and failures. They are not active commands; use automation.md and workflow-state.json.

# Build and review route

## Active r003 route

Current source is source/breakaway-r003.psd, built from the verified r002 parent by revise-r003.jsx, which reuses the existing revise-breakaway.jsx engine. Both sources remain available; the engine refuses to overwrite an existing revision. It reconstructs only the isolated project art/finish groups using the proven clipped-fill creation routine, preserves template structure and clearcoat, exports both driver states, and reopens the saved result for inspection. Do not use the failed clipping-base replacement method.

render-r003.ps1 uses native components with exclusive overlap ownership and recorded independent affine source sampling. Small confirmed components take precedence where native candidate masks overlap. Front I009 is explicitly primary; shared texels stay shared. This fixes observed rear-to-front role contamination, but the native boundary/bleed and all exact part-to-part joins still require physical review. Per-part masks allow validators to check ownership and role colors directly.

Run validate-exports.py r003 for full pixels, palette reversal, materials, front-duct ownership, patterned separate parts and wing colors. On success, install-review.ps1 -Revision r003 installs Rick only through the elevated route with hashes checked, following the user's explicit no-backup preference. It updates TGA timestamps for iRacing MIP regeneration, never renames a TGA into a MIP, and does not touch Justin's installed paint.

The car mapping profile contains prepared d007-hood-fender-check plus a frozen plan. It is not installed; standard r003 has no markers. It tests the candidate longitudinal registration, not an already confirmed match. Use it for one front-overhead view when exact S01/S02 registration is needed, recording its installed hash and restoring normal project paint afterward. Unknown inner mirror/rim/stalk pieces remain off-limits until identified.

## Historical r002 paint-breakaway route

Active source is `source/breakaway-r002.psd`. Continue future work from it; r001 is a rejected historical parent. The one-time `revise-breakaway.jsx` built r002 from that intact parent, preserved template structure/clearcoat, replaced only this project's art/finish groups, exported both states, then reopened and inspected the saved result. It refuses to overwrite an existing r002 source. Do not rerun it over an edited master.

`render-breakaway.ps1` reuses PatternRenderer.cs, reading the shared confirmed-panel inventory and native template blue masks. It includes all 28 confirmed IDs, plain colors on small mapped parts and independently oriented texture sampling for sides, roof and deck. Texture source and prompt are in graphics/r002. `validate-exports.py r002` checks all export pixels and selected-mask coverage. Material roughness is 90/45/160 for primary/secondary/charcoal; metallic is zero on new artwork. Unknown islands are not recolored by inference.

An intermediate in-place mask replacement failed because deleting/replacing clipping bases lost the fill clipping relationships across groups. Saved-state/pixel checks caught the failure and those outputs were never installed. The refresh script was retired from the launcher and removed. Rebuilding the isolated art/finish groups through the already demonstrated creation routine produced a source that passed reopen, clipping, visibility and pixel verification. The malformed intermediate source was removed after successful validation. Avoid the failed mask-base replacement approach; preserve working source and editable colors during future targeted revisions. ASCII group prefixes are safer than full Unicode layer-name matching during traversal.

The JSX returns verification data through the known serializer; PowerShell writes the r002 build report. No use of JSON.stringify inside Photoshop. Rick's validated r002 exports were directly installed with explicit authorization and no backups, matching the user's preference. Check exports/r002/installed-session.json for current installed state; no MIP is manufactured by scripts.

## Historical r001 setup route

Reuse the proven Photoshop COM / JSX route described in the cel-shaded project's automation notes. This project's launcher is a scoped adaptation for its own input and output paths, not permission to run old cel-shaded modification scripts. Photoshop 27.9.0 and JSX execution were demonstrated on 2026-10-19; no documents were open at preflight.

1. `python scripts/make-pattern.py` writes deterministic polygon geometry (seed 718). `pwsh -NoProfile -File scripts/render-pattern.ps1` also runs that generator and renders six native-clipped role masks using System.Drawing. No external Python packages are required.
2. Through the approved elevated PowerShell route, run `scripts/invoke-photoshop.ps1 -Script build-r001.jsx`. The launcher checks the template hash and refuses to touch an open project input/master. The JSX refuses to overwrite an existing master. Use a checkpoint and targeted revision script after r001; never rerun a fresh build over edited source.
3. The master keeps six shared geometry masks with explicit clipped solid-color fills for Rick and Justin. Finish masks are derived from those same role masks. The original clearcoat group and other template content are preserved. Both driver paint and provisional spec exports are saved into r001, then Rick paint editing state is restored.
4. Inspect both 2048-square paint previews and exported TGAs; verify guide visibility, filename ownership, standard number reserves, matching geometry and template identity. These checks are not in-car verification.

No launcher installs or replaces iRacing paints. Review exports are staged inside the project. iRacing-generated spec MIPs do not exist for this revision until an authorized install/viewer session generates them. Existing installed TGAs/MIPs and mapping diagnostics remain untouched.

On 2026-10-19 the user explicitly requested installation and waived backups. A direct elevated PowerShell copy installed the validated Rick r001 paint/spec TGAs into the verified paint folder; destination hashes were checked and timestamps refreshed. This does not change the launcher's project-only behavior. See exports/r001/installed-session.json. The previous MIP remains older than the installed spec TGA and awaits iRacing regeneration. The prior diagnostic session record describes its historical restoration, not the current installed livery.

Finish values are draft starting points recorded in build-report.json. Their appearance, edge bleed and cross-panel transitions need in-car review. Exact seams are still provisional; see placement.md. Do not claim artificial depth or raised geometry from a spec map.

For a color edit, change the clearly named Rick/Justin SOLIDFILL swatches in each role group. Keep exactly one driver's fills visible across paint and spec groups. Layer names and recorded visibility are authoritative. Keep lime nonmetallic; the selected metallic role changes with the violet color between driver variants.

## Troubleshooting

The first local renderer compilation needed standard .NET reference assemblies in addition to Drawing; render-pattern.ps1 now includes PowerShell's ref directory. The first Git branch command failed to set a ref; the same explicitly authorized command with an explicit workspace succeeded. Neither failure changed existing livery assets.

Photoshop saved the layered source, both driver paint/spec TGAs and all previews, then failed while writing the build report because ExtendScript has no native JSON object here. Added json.jsxinc for serialization. Do not rerun build-r001 over the saved master: inspect-r001.jsx performs a read-only saved-layer/state inspection and recovers the build report through the same scoped launcher. No artifact rebuild is necessary for this reporting failure.

## r004 diagnostic/install reporting

A transient file sharing violation occurred while writing the final r004 installation record after both TGA copies/hash checks had succeeded. Verified installed hashes against validation.json, repaired the state without recopying paints, and added bounded IOException retries to state writes in install-review.ps1 and the dedicated diagnostic helper. This reporting recovery did not rebuild or alter the PSD/exports.

The first uninstalled d008 grid used DrawImageUnscaled, which applied 96-to-72 DPI scaling against Photoshop's PNG. Visual inspection rejected it before installation. Final renderer uses explicit source/destination pixel rectangles. Independent pixel validation now verifies no changes outside selected masks and identical PNG/TGA buffers. Rejected files remain in shared diagnostics with status rejected_before_install. No in-car evidence uses the rejected version.

## r005 export-only recovery

The new revision directory was missing at first export. The layered r005 master had saved before that failure; no incomplete paint was installed. Read Photoshop document state, then reused saved r005 with an explicit export-existing mode in the proven shared engine. This mode validates 18 saved clipped color/finish controls, exports both drivers and reopens/inspects saved state. The default build still refuses to overwrite an existing revision. Export folder creation now occurs before any document mutation. Recovery wrapper resume-r005.jsx is explicit and should not be used to replace reviewed source changes without checking them.

r006: native Base Color is still the original raster layer, filled per driver in the existing export-state function. Every paint export and final Rick restore sets its RGB explicitly. The first low-detail rear sample failed validation and was preserved as r006-first-rear-check before correction; native-basecoat pre-change source also preserved. In validation, metal-zero checks apply to covered artwork masks. Native Parts outside them retain their prior spec bytes even when the basecoat color changes; paint hue alone is not a valid material classification.
