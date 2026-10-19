# Photoshop automation and session handoff

## Verified route and preflight

On 2026-10-18, elevated PowerShell created `Photoshop.Application` (version 27.9.0), read its open documents, and returned `2` from `DoJavaScript('1+1')`. This verifies COM and JSX execution through the elevated route. The sandbox route previously failed with `CO_E_SERVER_EXEC_FAILURE`; do not retry it. If the elevated route is unavailable, report a blocker instead of inventing another bridge.

Run `scripts/iterate.ps1` from an **approved elevated PowerShell execution route** for Apply or ExportAndValidate. The agent's command tool uses `sandbox_permissions: require_escalated`; a human terminal must also have Photoshop COM access. Preview needs only the normal local PowerShell route. A launcher call is:

```powershell
pwsh -NoProfile -File .\cel-shaded\scripts\invoke-photoshop.ps1 -Script refresh-headlight-loops.jsx -CheckOnly
```

The launcher checks that the on-disk master exists, requires it to be closed in Photoshop, runs only allowlisted project JSX scripts, and returns compact JSON or a short stage error. This protects the user's open editing session because the existing JSX opens and closes the master. The master was observed open with unsaved edits early on 2026-10-18; a later check found no open documents. Recheck each session. Save or resolve manual changes and close the master before automation. Do not automatically save or close the user's open document.

## Headlight iteration

The heavy-loop workflow below is historical: the user rejected and removed those layers. Current passenger details are flipped driver pixels, documented in `current-state.md`. Do not rerun the heavy-loop Apply stage or the one-time `copy-driver-headlight-echo.jsx` against the completed current master.

From the repository root:

```powershell
pwsh -NoProfile -File .\cel-shaded\scripts\iterate.ps1 -Region headlights -Driver Rick -Stage Preview
pwsh -NoProfile -File .\cel-shaded\scripts\iterate.ps1 -Region headlights -Driver Rick -Stage Apply
pwsh -NoProfile -File .\cel-shaded\scripts\iterate.ps1 -Region headlights -Driver Rick -Stage ExportAndValidate
```

- **Preview** reuses `render-headlight-loops.ps1` and `preview-headlight-loops.ps1`. Review the new wire crops and rendered layer PNGs. Rick/Justin paint crops still show the *previous* exports until Apply and ExportAndValidate run. Confirm the intended bumper island before applying.
- **Apply** checks Photoshop's saved state, copies the on-disk master to a uniquely named `archive/iteration-headlights-*` checkpoint, then runs the existing `refresh-headlight-loops.jsx`. Never run the one-time `apply-headlight-loops.jsx` again.
- **ExportAndValidate** backs up the existing current Rick TGAs, runs `export-shared-current.jsx`, refreshes regional crops, then checks TGA headers and the generated headlight paint/spec alpha and UV bounds. The exporter also refreshes Justin's flat preview. No file is installed in iRacing by these commands. The report says `NOT VERIFIED IN-CAR` until the user reviews both front corners.

Existing Photoshop JSX is unchanged. The launcher is the only COM bridge. When a script fails, retain the checkpoint and backup, inspect the short error and current PSD state, and stop before install. Do not rerun a failed stage blindly.

## Starting and ending a conversation

Start: read `cel-shaded/README.md` and `cel-shaded/docs/current-state.md`. Read only the relevant mapping heading and script for the requested region. Verify Photoshop's saved state before any Apply/Export stage. Ask for one specific in-car observation and cropped views when mapping is uncertain.

End after an accepted revision: update `current-state.md` with the master revision/date, saved state, exports, confirmed/rejected/provisional findings, and exactly one next action. Append a short entry to `progress.md` only for a meaningful change, linking its checkpoint and checks. Keep full UV discoveries in the shared mapping notes and technical failures here. A new conversation can resume from the two short start files without loading the prior conversation or full progress history.

Use Astra for ambiguous screenshots, unknown UV island correspondence, and design judgment. Use Sol for bounded script edits, execution, export checks, and documentation. These are task choices, not installed model-routing configuration; no automatic routing is assumed.
