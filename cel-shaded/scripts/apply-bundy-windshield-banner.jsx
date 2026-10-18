#target photoshop
(function () {
    var root = File($.fileName).parent.parent;
    var source = File(root.fsName + '/source/rick-cel-shaded.psd');
    var blackFile = File(root.fsName + '/graphics/current/windshield-banner-black.png');
    var oldDialogs = app.displayDialogs;
    var doc = null;
    app.displayDialogs = DialogModes.NO;
    function rgb(r,g,b) {
        var c = new SolidColor(); c.rgb.red = r; c.rgb.green = g; c.rgb.blue = b; return c;
    }
    function fit(layer, cx, cy, wantedW, wantedH) {
        var b = layer.bounds;
        var w = b[2].as('px') - b[0].as('px');
        var h = b[3].as('px') - b[1].as('px');
        if (w < 1 || h < 1) throw Error('BUNDY text has empty bounds.');
        layer.resize(wantedW / w * 100, wantedH / h * 100, AnchorPosition.MIDDLECENTER);
        b = layer.bounds;
        layer.translate(cx - (b[0].as('px') + b[2].as('px')) / 2,
                        cy - (b[1].as('px') + b[3].as('px')) / 2);
    }
    try {
        for (var i = 0; i < app.documents.length; i++) {
            if (app.documents[i].name === source.name && !app.documents[i].saved)
                throw Error('Save manual edits to the current master before adding the banner.');
        }
        doc = app.open(source);
        var art = doc.layerSets.getByName('Paintable Area').layerSets.getByName('[SHARED] Cel-shaded artwork');
        for (var j = 0; j < art.layerSets.length; j++) {
            if (art.layerSets[j].name === 'Exterior windshield banner - BUNDY')
                throw Error('The BUNDY banner group already exists; this is a one-time script.');
        }
        var group = art.layerSets.add();
        group.name = 'Exterior windshield banner - BUNDY';
        var imageDoc = app.open(blackFile);
        try {
            var base = imageDoc.activeLayer.duplicate(doc, ElementPlacement.PLACEATBEGINNING);
            app.activeDocument = doc;
            base.move(group, ElementPlacement.PLACEATBEGINNING);
            base.name = 'Exterior strip black - 718 UV';
        } finally {
            app.activeDocument = imageDoc;
            imageDoc.close(SaveOptions.DONOTSAVECHANGES);
            app.activeDocument = doc;
        }

        var main = group.artLayers.add();
        main.kind = LayerKind.TEXT;
        main.name = 'BUNDY - editable block lettering';
        var type = main.textItem;
        type.contents = 'BUNDY';
        type.font = 'Impact';
        type.size = UnitValue(110, 'pt');
        type.color = rgb(246, 247, 241);
        type.position = [UnitValue(200, 'px'), UnitValue(250, 'px')];
        main.rotate(90, AnchorPosition.MIDDLECENTER);
        fit(main, 1046, 1267, 68, 344);

        var light = main.duplicate();
        light.name = 'BUNDY - editable pale cel edge';
        light.textItem.color = rgb(111, 156, 171);
        light.translate(2.4, 2.2);
        light.move(main, ElementPlacement.PLACEAFTER);

        var dark = main.duplicate();
        dark.name = 'BUNDY - editable block shadow';
        dark.textItem.color = rgb(39, 77, 94);
        dark.translate(4.8, 4.5);
        dark.move(light, ElementPlacement.PLACEAFTER);

        doc.activeLayer = main;
        doc.save();
        return 'Added black exterior windshield strip and editable BUNDY lettering; the adjacent interior strip was left alone.';
    } finally {
        if (doc) { app.activeDocument = doc; doc.close(SaveOptions.DONOTSAVECHANGES); }
        app.displayDialogs = oldDialogs;
    }
}());
