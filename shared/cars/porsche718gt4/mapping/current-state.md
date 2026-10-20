# Mapping session state

Updated 2026-10-20 for workflow audit. This profile remains incomplete. No new mapping, diagnostics, screenshots or geometry calibration were created by the workflow cleanup.

- Exact template: 2048-square 8-bit RGB Porsche 718 GT4; SHA-256 in template/identity.json.
- Reuse panels.json, seams.json and evidence.json, independently checking identity, orientation, geometry and seam registration. There are 28 confirmed identities, not 28 fully calibrated surfaces.
- G01-G06 grids, E01-E07 livery views, D01-D11 small-parts views, H01/H02 lamp views, K01-K06 remaining parts and X01 d006 louver evidence remain available. Newer r007/r008 findings also remain in the shared mapping folder.
- Diagnostic session records restored d008-continuity-grid. d006 was an earlier restored diagnostic. No diagnostic is currently recorded installed; do not request new mapping merely because a thread starts.
- Last normal session record points to r008, but actual Rick paint hash at audit is ad29044d077240c692ffdc6fa96c75bd4542a5df02930af945863d011ca4d962, differing from recorded r008. The spec matches. Preserve current installed paint; run the livery preflight to obtain current bytes. Restoration records are historical, not live state.
- Native mandatory protection covers 111 reference components; 17 remain unresolved for placement. Keep unknown/inside pieces off-limits. Cockpit review remains deferred.
- Seam records establish adjacency but have no confirmed paired boundary points. In particular S14/S15 and the complete deck/lip/bumper registration chain remain unregistered. Artwork-specific virtual coordinates are not geometry.
- The 770 hood/fender sample agreements are livery-file checks; structured S01/S02 remain provisional. No in-car seam acceptance is inferred.
- Existing validation.json is the historical mapping audit. Current-state hashes must come from preflight, not its old diagnostic labels or backup checks.
- Do not create installed-paint backups by default. The backup-generating historical installer now refuses Install; Restore remains guarded for its historical sessions. A new diagnostic requires a scoped gap plan with existing evidence checked, smallest target and shared persistence path, plus a current matching recoverable normal source.

Next action: reuse this profile for authorized livery work; report specific gaps before any diagnostic. Rear calibration belongs to the separately scoped Task 2.
