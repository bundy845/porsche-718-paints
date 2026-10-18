#target photoshop
(function () {
    var root = File($.fileName).parent.parent;
    var input = File(root.fsName + "/inputs/Porsche 718 GT4.psd");
    var source = File(root.fsName + "/source/rick-cel-shaded.psd");
    var paint = File(root.fsName + "/exports/r001/car_844824.tga");
    var preview = File(root.fsName + "/design/previews/paint-flat-r001.png");
    var previousDialogs = app.displayDialogs;
    app.displayDialogs = DialogModes.NO;
    var doc;
    try {
        doc = app.open(input);
        doc.layerSets.getByName("Turn Off Before Exporting TGA").visible = false;
        var area = doc.layerSets.getByName("Paintable Area");
        area.artLayers.getByName("Car Decal").visible = false;
        area.artLayers.getByName("Pitbox").visible = false;
        function add(fileName, layerName) {
            var overlay = app.open(File(root.fsName + "/graphics/" + fileName));
            try {
                var layer = overlay.activeLayer.duplicate(doc, ElementPlacement.PLACEATBEGINNING);
                app.activeDocument = doc;
                layer.move(area, ElementPlacement.PLACEATBEGINNING);
                layer.name = layerName;
                return layer;
            } finally {
                overlay.close(SaveOptions.DONOTSAVECHANGES);
                app.activeDocument = doc;
            }
        }
        add("body-r001.png", "[RICK] green body mask #B8F500");
        add("shadows-r001.png", "[RICK] Olive cel shadows");
        add("highlights-r001.png", "[RICK] Sunlit lime highlights");
        add("ink-r001.png", "[RICK] Charcoal comic ink");
        var psd = new PhotoshopSaveOptions();
        psd.layers = true;
        doc.saveAs(source, psd, true, Extension.LOWERCASE);
        doc.saveAs(preview, new PNGSaveOptions(), true, Extension.LOWERCASE);
        var tga = new TargaSaveOptions();
        tga.resolution = TargaBitsPerPixels.TWENTYFOUR;
        tga.alphaChannels = false;
        tga.rleCompression = true;
        doc.saveAs(paint, tga, true, Extension.LOWERCASE);
        return "Saved r001 layered PSD, paint preview and Rick TGA";
    } finally {
        if (doc) doc.close(SaveOptions.DONOTSAVECHANGES);
        app.displayDialogs = previousDialogs;
    }
}());
