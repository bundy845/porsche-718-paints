#target photoshop
(function () {
    var root=File($.fileName).parent.parent;
    var source=File(root.fsName+"/source/rick-cel-shaded.psd");
    var folder=root.fsName+"/graphics/current/";
    var paint=File(root.fsName+"/exports/current/car_844824.tga");
    var spec=File(root.fsName+"/exports/current/car_spec_844824.tga");
    var paintPreview=File(root.fsName+"/design/previews/paint-flat-current.png");
    var specPreview=File(root.fsName+"/design/previews/spec-flat-current.png");
    var oldDialogs=app.displayDialogs;
    app.displayDialogs=DialogModes.NO;
    var doc;
    try {
        doc=app.open(source);
        var area=doc.layerSets.getByName("Paintable Area");
        var specs=doc.layerSets.getByName("Custom Spec Map");
        area.artLayers.getByName("[RICK] Hand drawn ink").remove();
        area.artLayers.getByName("[RICK] Green cel shade").remove();
        specs.artLayers.getByName("[RICK] Ink and wing satin finish").remove();
        function add(name,group,label) {
            var overlay=app.open(File(folder+name));
            try {
                var layer=overlay.activeLayer.duplicate(doc,ElementPlacement.PLACEATBEGINNING);
                app.activeDocument=doc;
                layer.move(group,ElementPlacement.PLACEATBEGINNING);
                layer.name=label;
            } finally {
                overlay.close(SaveOptions.DONOTSAVECHANGES);
                app.activeDocument=doc;
            }
        }
        add("shade-current.png",area,"[RICK] Green cel shade");
        add("ink-current.png",area,"[RICK] Hand drawn ink");
        add("spec-current.png",specs,"[RICK] Ink and wing satin finish");
        doc.layerSets.getByName("Turn Off Before Exporting TGA").visible=false;
        area.visible=true;
        specs.visible=false;
        var psd=new PhotoshopSaveOptions();psd.layers=true;
        doc.saveAs(source,psd,true,Extension.LOWERCASE);
        doc.saveAs(paintPreview,new PNGSaveOptions(),true,Extension.LOWERCASE);
        var tga=new TargaSaveOptions();
        tga.resolution=TargaBitsPerPixels.TWENTYFOUR;
        tga.alphaChannels=false;
        tga.rleCompression=true;
        doc.saveAs(paint,tga,true,Extension.LOWERCASE);
        area.visible=false;
        specs.visible=true;
        doc.saveAs(specPreview,new PNGSaveOptions(),true,Extension.LOWERCASE);
        doc.saveAs(spec,tga,true,Extension.LOWERCASE);
        return "Mapped layered PSD and current paint/spec exports saved";
    } finally {
        if(doc)doc.close(SaveOptions.DONOTSAVECHANGES);
        app.displayDialogs=oldDialogs;
    }
}());
