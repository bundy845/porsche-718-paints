// Read-only inventory of the original Porsche 718 GT4 template.
#target photoshop
(function () {
    var root = File($.fileName).parent.parent;
    var input = File(root.fsName + "/inputs/Porsche 718 GT4.psd");
    var previousDialogs = app.displayDialogs;
    app.displayDialogs = DialogModes.NO;
    var doc;
    var lines = [];
    try {
        doc = app.open(input);
        lines.push("name=" + doc.name);
        lines.push("size=" + doc.width.as("px") + "x" + doc.height.as("px"));
        lines.push("mode=" + doc.mode + " depth=" + doc.bitsPerChannel);
        lines.push("channels=" + doc.channels.length + " guides=" + doc.guides.length);
        function visit(parent, prefix) {
            for (var i = 0; i < parent.layers.length; i++) {
                var layer = parent.layers[i];
                var kind = layer.typename === "LayerSet" ? "group" : "layer";
                lines.push(prefix + kind + " | " + layer.name + " | visible=" + layer.visible + " | blend=" + layer.blendMode + " | opacity=" + layer.opacity);
                if (kind === "group") visit(layer, prefix + "  ");
            }
        }
        visit(doc, "");
        return lines.join("\n");
    } finally {
        if (doc) doc.close(SaveOptions.DONOTSAVECHANGES);
        app.displayDialogs = previousDialogs;
    }
}());
