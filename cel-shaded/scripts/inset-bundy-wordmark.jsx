#target photoshop
(function () {
    var root = File($.fileName).parent.parent;
    var source = File(root.fsName + '/source/rick-cel-shaded.psd');
    var oldDialogs = app.displayDialogs;
    var doc = null;
    app.displayDialogs = DialogModes.NO;
    try {
        for (var i = 0; i < app.documents.length; i++) {
            if (app.documents[i].name === source.name && !app.documents[i].saved)
                throw Error('Save manual edits to the master before adjusting BUNDY.');
        }
        doc = app.open(source);
        var group = doc.layerSets.getByName('Paintable Area')
            .layerSets.getByName('[SHARED] Cel-shaded artwork')
            .layerSets.getByName('Exterior windshield banner - BUNDY');
        var wordmark = group.artLayers.getByName('BUNDY - angular outlined vector (embedded SVG)');
        var before = wordmark.bounds;
        var beforeW = before[2].as('px') - before[0].as('px');
        var beforeH = before[3].as('px') - before[1].as('px');
        if (beforeW < 1 || beforeH < 1) throw Error('BUNDY smart object has empty bounds.');
        // In the car view the lower letter strokes crossed the lower glass edge.
        // Reduce the glyph depth across the narrow UV strip and shift it inward.
        wordmark.resize(50 / beforeW * 100, 344 / beforeH * 100, AnchorPosition.MIDDLECENTER);
        var after = wordmark.bounds;
        wordmark.translate(1051 - (after[0].as('px') + after[2].as('px')) / 2,
                           1267 - (after[1].as('px') + after[3].as('px')) / 2);
        after = wordmark.bounds;
        doc.activeLayer = wordmark;
        doc.save();
        return 'BUNDY bounds before=' + [before[0].as('px'),before[1].as('px'),before[2].as('px'),before[3].as('px')].join(',')
            + ' after=' + [after[0].as('px'),after[1].as('px'),after[2].as('px'),after[3].as('px')].join(',');
    } finally {
        if (doc) { app.activeDocument = doc; doc.close(SaveOptions.DONOTSAVECHANGES); }
        app.displayDialogs = oldDialogs;
    }
}());
