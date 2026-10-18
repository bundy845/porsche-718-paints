#target photoshop
(function () {
    var root = File($.fileName).parent.parent;
    var input = File(root.fsName + "/inputs/Porsche 718 GT4.psd");
    var output = Folder(root.fsName + "/design/previews");
    var previousDialogs = app.displayDialogs;
    app.displayDialogs = DialogModes.NO;
    var doc;
    try {
        doc = app.open(input);
        var guides = doc.layerSets.getByName("Turn Off Before Exporting TGA");
        var options = new PNGSaveOptions();
        doc.saveAs(File(output.fsName + "/template-visible-r000.png"), options, true, Extension.LOWERCASE);
        guides.artLayers.getByName("Wire").visible = true;
        guides.artLayers.getByName("Number Blocks").visible = true;
        doc.saveAs(File(output.fsName + "/template-wire-r000.png"), options, true, Extension.LOWERCASE);
        return "Saved template-visible-r000.png and template-wire-r000.png";
    } finally {
        if (doc) doc.close(SaveOptions.DONOTSAVECHANGES);
        app.displayDialogs = previousDialogs;
    }
}());
