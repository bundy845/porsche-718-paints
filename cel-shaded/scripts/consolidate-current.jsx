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
        function remove(group,names) {
            for(var i=0;i<names.length;i++)group.artLayers.getByName(names[i]).remove();
        }
        remove(area,[
            "[RICK] Sketched panel ink r005",
            "[RICK] Panel ink and shade r004",
            "[RICK] Hand drawn panel ink r003",
            "[RICK] Contour ink and wing r002"
        ]);
        remove(specs,[
            "[RICK] Panel ink spec r004",
            "[RICK] Satin panel ink spec r003",
            "[RICK] Satin roughness for comic ink r002"
        ]);
        area.artLayers.getByName("[RICK] Uniform green body #B8F500 r002").name="[RICK] Body — electric green";
        specs.artLayers.getByName("[RICK] Panel ink spec r005").name="[RICK] Ink and wing satin finish";
        function add(name,label) {
            var overlay=app.open(File(folder+name));
            try {
                var layer=overlay.activeLayer.duplicate(doc,ElementPlacement.PLACEATBEGINNING);
                app.activeDocument=doc;
                layer.move(area,ElementPlacement.PLACEATBEGINNING);
                layer.name=label;
            } finally {
                overlay.close(SaveOptions.DONOTSAVECHANGES);
                app.activeDocument=doc;
            }
        }
        add("shade-current.png","[RICK] Green cel shade");
        add("wing-current.png","[RICK] Dark wing");
        add("ink-current.png","[RICK] Hand drawn ink");
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
        return "Clean layered PSD and current paint/spec exports saved";
    } finally {
        if(doc)doc.close(SaveOptions.DONOTSAVECHANGES);
        app.displayDialogs=oldDialogs;
    }
}());
