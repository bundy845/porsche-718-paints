#target photoshop
(function () {
    var root=File($.fileName).parent.parent;
    var doc=app.open(File(root.fsName+"/source/rick-cel-shaded.psd"));
    try {
        var rows=[];
        function walk(container,prefix) {
            for (var i=0;i<container.layers.length;i++) {
                var layer=container.layers[i];
                rows.push(prefix+layer.name+" | "+(layer.visible?"visible":"hidden")+" | "+layer.typename);
                if (layer.typename=="LayerSet") walk(layer,prefix+"  ");
            }
        }
        rows.push("canvas="+doc.width.as("px")+"x"+doc.height.as("px")+" mode="+doc.mode+" bits="+doc.bitsPerChannel);
        walk(doc,"");
        return rows.join("\n");
    } finally {
        doc.close(SaveOptions.DONOTSAVECHANGES);
    }
}());
