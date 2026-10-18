#target photoshop
(function () {
    var root = File($.fileName).parent.parent;
    var source = File(root.fsName + '/source/rick-cel-shaded.psd');
    var vector = File(root.fsName + '/graphics/current/bundy-angular-outline.svg');
    var oldDialogs = app.displayDialogs;
    var doc = null;
    app.displayDialogs = DialogModes.NO;
    function fit(layer, cx, cy, wantedW, wantedH) {
        var b = layer.bounds;
        var w = b[2].as('px') - b[0].as('px');
        var h = b[3].as('px') - b[1].as('px');
        if (w < 1 || h < 1) throw Error('New BUNDY wordmark has empty bounds.');
        layer.resize(wantedW / w * 100, wantedH / h * 100, AnchorPosition.MIDDLECENTER);
        b = layer.bounds;
        layer.translate(cx - (b[0].as('px') + b[2].as('px')) / 2,
                        cy - (b[1].as('px') + b[3].as('px')) / 2);
    }
    try {
        for (var i = 0; i < app.documents.length; i++) {
            if (app.documents[i].name === source.name && !app.documents[i].saved)
                throw Error('Save manual edits to the master before replacing the lettering.');
        }
        doc = app.open(source);
        var group = doc.layerSets.getByName('Paintable Area')
            .layerSets.getByName('[SHARED] Cel-shaded artwork')
            .layerSets.getByName('Exterior windshield banner - BUNDY');
        var placed = new ActionDescriptor();
        placed.putPath(charIDToTypeID('null'), vector);
        executeAction(stringIDToTypeID('placeEvent'), placed, DialogModes.NO);
        var wordmark = doc.activeLayer;
        wordmark.move(group, ElementPlacement.PLACEATBEGINNING);
        wordmark.name = 'BUNDY - angular outlined vector (embedded SVG)';
        wordmark.rotate(90, AnchorPosition.MIDDLECENTER);
        fit(wordmark, 1046, 1267, 68, 344);
        group.artLayers.getByName('BUNDY - editable block lettering').remove();
        group.artLayers.getByName('BUNDY - editable pale cel edge').remove();
        group.artLayers.getByName('BUNDY - editable block shadow').remove();
        doc.activeLayer = wordmark;
        doc.save();
        return 'Replaced Impact with an embedded SVG angular outline wordmark on the same exterior strip. Layer kind: ' + wordmark.kind;
    } finally {
        if (doc) { app.activeDocument = doc; doc.close(SaveOptions.DONOTSAVECHANGES); }
        app.displayDialogs = oldDialogs;
    }
}());
