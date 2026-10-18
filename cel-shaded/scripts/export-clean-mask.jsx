#target photoshop
(function () {
    var root = File($.fileName).parent.parent;
    var input = File(root.fsName + "/inputs/Porsche 718 GT4.psd");
    var output = File(root.fsName + "/design/previews/template-clean-mask-r000.png");
    var previousDialogs = app.displayDialogs;
    app.displayDialogs = DialogModes.NO;
    var doc;
    try {
        doc = app.open(input);
        var guides = doc.layerSets.getByName("Turn Off Before Exporting TGA");
        for (var i = 0; i < guides.artLayers.length; i++)
            guides.artLayers[i].visible = guides.artLayers[i].name === "Mask";
        var area = doc.layerSets.getByName("Paintable Area");
        area.artLayers.getByName("Car Decal").visible = false;
        area.artLayers.getByName("Pitbox").visible = false;
        doc.saveAs(output, new PNGSaveOptions(), true, Extension.LOWERCASE);
        return "Saved clean UV mask reference";
    } finally {
        if (doc) doc.close(SaveOptions.DONOTSAVECHANGES);
        app.displayDialogs = previousDialogs;
    }
}());
