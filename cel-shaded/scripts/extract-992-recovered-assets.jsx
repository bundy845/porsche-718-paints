#target photoshop
(function () {
    var src = File('C:/Dropbox/2 Old 2 Slow/Racing/iRacing/Paints/MWM Season 12/Porsche 992_2 Cup-Recovered.psd');
    var outDir = Folder('C:/Dropbox/2 Old 2 Slow/Racing/iRacing/Paints/porsche718gt4/cel-shaded/design/extracted-992-assets');
    if (!outDir.exists) outDir.create();
    var prior = null;
    try { prior = app.activeDocument; } catch (noOpenDocument) {}
    var source = app.open(src);
    var created = [];
    var entries = [
        ['2old2slow logo - Passenger Door', '2-old-2-slow-passenger-door'],
        ['2old2slow logo - Driver Door', '2-old-2-slow-driver-door'],
        ["You don't have to get ready if you stay ready", 'stay-ready-bumper-text'],
        ['Nothing echoes louder than the truth', 'truth-bumper-text'],
        ['Me outlined', 'me-outlined']
    ];
    var rows = [];
    var originalUnits = app.preferences.rulerUnits;
    app.preferences.rulerUnits = Units.PIXELS;
    function savePSD(doc, file) {
        var opts = new PhotoshopSaveOptions();
        opts.layers = true;
        opts.embedColorProfile = true;
        doc.saveAs(file, opts, true, Extension.LOWERCASE);
    }
    function savePNG(doc, file) {
        var opts = new PNGSaveOptions();
        opts.compression = 9;
        opts.interlaced = false;
        doc.saveAs(file, opts, true, Extension.LOWERCASE);
    }
    try {
        var paint = source.layerSets.getByName('Paintable Area');
        var full = app.documents.add(2048, 2300, source.resolution, '992 reusable artwork - original UV positions', NewDocumentMode.RGB, DocumentFill.TRANSPARENT);
        created.push(full);
        for (var i = entries.length - 1; i >= 0; i--) {
            var item = paint.artLayers.getByName(entries[i][0]);
            app.activeDocument = source;
            var dup = item.duplicate(full, ElementPlacement.PLACEATBEGINNING);
            app.activeDocument = full;
            dup.visible = true;
        }
        app.activeDocument = full;
        savePSD(full, File(outDir.fsName + '/992-recovered-artwork-original-positions.psd'));
        full.close(SaveOptions.DONOTSAVECHANGES);
        created.pop();
        for (var j = 0; j < entries.length; j++) {
            var layer = paint.artLayers.getByName(entries[j][0]);
            var origBounds = [layer.bounds[0].as('px'),layer.bounds[1].as('px'),layer.bounds[2].as('px'),layer.bounds[3].as('px')];
            var doc = app.documents.add(2048, 2300, source.resolution, entries[j][1], NewDocumentMode.RGB, DocumentFill.TRANSPARENT);
            created.push(doc);
            app.activeDocument = source;
            var copy = layer.duplicate(doc, ElementPlacement.PLACEATBEGINNING);
            app.activeDocument = doc;
            copy.visible = true;
            doc.trim(TrimType.TRANSPARENT, true, true, true, true);
            savePSD(doc, File(outDir.fsName + '/' + entries[j][1] + '.psd'));
            savePNG(doc, File(outDir.fsName + '/' + entries[j][1] + '.png'));
            rows.push(entries[j][1] + ' | source=' + entries[j][0] + ' | kind=' + layer.kind + ' | sourceBounds=' + origBounds.join(',') + ' | exportSize=' + doc.width.as('px') + 'x' + doc.height.as('px'));
            doc.close(SaveOptions.DONOTSAVECHANGES);
            created.pop();
        }
        var report = File(outDir.fsName + '/extraction-report.txt');
        report.open('w'); report.encoding = 'UTF8'; report.write(rows.join('\n')); report.close();
        return rows.join('\n');
    } finally {
        for (var k = created.length - 1; k >= 0; k--) try { created[k].close(SaveOptions.DONOTSAVECHANGES); } catch (ignoreCreated) {}
        source.close(SaveOptions.DONOTSAVECHANGES);
        app.preferences.rulerUnits = originalUnits;
        if (prior) try { app.activeDocument = prior; } catch (ignore) {}
    }
}());
