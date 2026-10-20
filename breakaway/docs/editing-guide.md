# Editing Breakaway

Open `source/breakaway-master.psd`. The original `source/breakaway-r008.psd` is the rollback. The historical demo bundle and nine other PSD checkpoints were removed by user direction after the palette workflow was accepted; compact test/migration evidence remains under workflow-audit.

1. In Layers expand PALETTES, then RICK or JUSTIN. Keep the PALETTES library itself hidden.
2. Double-click the PRIMARY smart-object thumbnail. In its tab, double-click the COLOR Solid Color Fill swatch, choose the color, save the tab (Ctrl+S), and close it. Linked artwork and native fallback update together.
3. Repeat for SECONDARY. There are two color values per driver, four total. Neutral and finish controls remain separate.
4. Use Window > Layer Comps > PALETTE PREVIEW - Rick or Justin to inspect either driver. Save the master.
5. Preserve and close the master before automation. From the project root:

```powershell
python scripts/preflight.py
pwsh -NoProfile -File scripts/invoke-photoshop.ps1 -Script workflow.jsx -Operation export -OutputDirectory "$PWD/exports/new-review-name"
python scripts/validate-workflow.py
```

The output directory must be new. Export uses a temporary document and never saves or rebuilds the source. It writes both drivers' four paint/spec TGAs, four PNG previews and a validated source/output hash manifest. Installation is a separate authorized action.

Use ordinary duplicated smart-object instances to retain palette links. Do not rasterize them or use New Smart Object via Copy. Shared geometry remains editable under Paintable Area; separate saved material masks remain under Custom Spec Map. Geometry/logo changes that need a different finish must update the relevant material mask too. Preserve mandatory native art and the original Base Color raster beneath linked PRIMARY fallbacks.

Current outputs are exports/current. The renamed saved master was reexported and all four TGAs match the r008 baseline byte-for-byte. This does not establish rear seam acceptance; that remains separately scoped. Before each meaningful step review remaining referenced historical/test/redundant assets and ask for direction under project AGENTS.md.
