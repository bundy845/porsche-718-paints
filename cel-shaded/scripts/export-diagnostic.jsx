#target photoshop
(function () {
    var root=File($.fileName).parent.parent;
    var source=File(root.fsName+"/source/rick-cel-shaded.psd");
    var overlay=File(root.fsName+"/design/diagnostics/coordinate-grid.png");
    var tgaFile=File(root.fsName+"/design/diagnostics/car_844824-coordinate-diagnostic.tga");
    var preview=File(root.fsName+"/design/diagnostics/coordinate-flat.png");
    var oldDialogs=app.displayDialogs;
    app.displayDialogs=DialogModes.NO;
    var doc;
    try {
        doc=app.open(source);
        var area=doc.layerSets.getByName("Paintable Area");
        area.artLayers.getByName("[RICK] Hand drawn ink").visible=false;
        area.artLayers.getByName("[RICK] Dark wing").visible=false;
        area.artLayers.getByName("[RICK] Green cel shade").visible=false;
        var temp=app.open(overlay);
        var layer=temp.activeLayer.duplicate(doc,ElementPlacement.PLACEATBEGINNING);
        temp.close(SaveOptions.DONOTSAVECHANGES);
        app.activeDocument=doc;
        layer.move(area,ElementPlacement.PLACEATBEGINNING);
        layer.name="TEMP coordinate diagnostic — remove after screenshots";
        doc.layerSets.getByName("Turn Off Before Exporting TGA").visible=false;
        doc.saveAs(preview,new PNGSaveOptions(),true,Extension.LOWERCASE);
        var tga=new TargaSaveOptions();
        tga.resolution=TargaBitsPerPixels.TWENTYFOUR;
        tga.alphaChannels=false;
        tga.rleCompression=true;
        doc.saveAs(tgaFile,tga,true,Extension.LOWERCASE);
        return "Temporary diagnostic export saved; source PSD unchanged";
    } finally {
        if(doc)doc.close(SaveOptions.DONOTSAVECHANGES);
        app.displayDialogs=oldDialogs;
    }
}());
