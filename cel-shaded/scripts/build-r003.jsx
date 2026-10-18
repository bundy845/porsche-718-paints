#target photoshop
(function () {
    var root = File($.fileName).parent.parent;
    var input = File(root.fsName + "/source/rick-cel-shaded.psd");
    var output = File(root.fsName + "/source/rick-cel-shaded.psd");
    var paint = File(root.fsName + "/exports/r003/car_844824.tga");
    var preview = File(root.fsName + "/design/previews/paint-flat-r003.png");
    var oldDialogs = app.displayDialogs;
    app.displayDialogs = DialogModes.NO;
    var doc;
    try {
        doc = app.open(input);
        var area = doc.layerSets.getByName("Paintable Area");
        var specs = doc.layerSets.getByName("Custom Spec Map");
        area.artLayers.getByName("[RICK] Contour ink and wing r002").visible = false;
        specs.artLayers.getByName("[RICK] Satin roughness for comic ink r002").visible = false;
        function add(name, layerName, target) {
            var overlay = app.open(File(root.fsName + "/graphics/" + name));
            try {
                var layer = overlay.activeLayer.duplicate(doc, ElementPlacement.PLACEATBEGINNING);
                app.activeDocument = doc;
                layer.move(target, ElementPlacement.PLACEATBEGINNING);
                layer.name = layerName;
            } finally {
                overlay.close(SaveOptions.DONOTSAVECHANGES);
                app.activeDocument = doc;
            }
        }
        add("ink-r003.png", "[RICK] Hand drawn panel ink r003", area);
        add("ink-spec-r003.png", "[RICK] Satin panel ink spec r003", specs);
        doc.layerSets.getByName("Turn Off Before Exporting TGA").visible = false;
        doc.saveAs(output, new PhotoshopSaveOptions(), true, Extension.LOWERCASE);
        doc.saveAs(preview, new PNGSaveOptions(), true, Extension.LOWERCASE);
        var tga = new TargaSaveOptions();
        tga.resolution = TargaBitsPerPixels.TWENTYFOUR;
        tga.alphaChannels = false;
        tga.rleCompression = true;
        doc.saveAs(paint, tga, true, Extension.LOWERCASE);
        return "Saved r003 paint";
    } finally {
        if (doc) doc.close(SaveOptions.DONOTSAVECHANGES);
        app.displayDialogs = oldDialogs;
    }
}());
