#target photoshop
(function () {
    var root = File($.fileName).parent.parent;
    var input = File(root.fsName + "/source/rick-cel-shaded.psd");
    var paint = File(root.fsName + "/exports/r001/car_spec_844824.tga");
    var preview = File(root.fsName + "/design/previews/spec-flat-r001.png");
    var previousDialogs = app.displayDialogs;
    app.displayDialogs = DialogModes.NO;
    var doc;
    try {
        doc = app.open(input);
        doc.layerSets.getByName("Turn Off Before Exporting TGA").visible = false;
        doc.layerSets.getByName("Paintable Area").visible = false;
        doc.layerSets.getByName("Custom Spec Map").visible = true;
        doc.saveAs(preview, new PNGSaveOptions(), true, Extension.LOWERCASE);
        var tga = new TargaSaveOptions();
        tga.resolution = TargaBitsPerPixels.TWENTYFOUR;
        tga.alphaChannels = false;
        tga.rleCompression = true;
        doc.saveAs(paint, tga, true, Extension.LOWERCASE);
        return "Saved template-derived spec preview and TGA for Rick";
    } finally {
        if (doc) doc.close(SaveOptions.DONOTSAVECHANGES);
        app.displayDialogs = previousDialogs;
    }
}());
