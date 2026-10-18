#target photoshop
(function () {
    var src = File('C:/Dropbox/2 Old 2 Slow/Racing/iRacing/Paints/MWM Season 12/Porsche 992_2 Cup-Recovered.psd');
    var out = File('C:/Dropbox/2 Old 2 Slow/Racing/iRacing/Paints/porsche718gt4/cel-shaded/design/992-recovered-layer-inventory.txt');
    var prior = app.activeDocument;
    var doc = app.open(src);
    try {
        var rows = ['source=' + src.fsName, 'canvas=' + doc.width.as('px') + 'x' + doc.height.as('px') + ' mode=' + doc.mode + ' bits=' + doc.bitsPerChannel];
        function walk(group, path, depth) {
            for (var i = 0; i < group.layers.length; i++) {
                var layer = group.layers[i];
                var p = path ? path + '/' + layer.name : layer.name;
                var bounds = '';
                try { bounds = ' bounds=' + [layer.bounds[0].as('px'),layer.bounds[1].as('px'),layer.bounds[2].as('px'),layer.bounds[3].as('px')].join(','); } catch (e) {}
                var extra = '';
                if (layer.typename === 'ArtLayer') {
                    try { extra += ' kind=' + layer.kind; } catch (e2) {}
                    try { extra += ' clipped=' + layer.grouped; } catch (e3) {}
                }
                rows.push(Array(depth + 1).join('  ') + p + ' | ' + layer.typename + ' | ' + (layer.visible ? 'visible' : 'hidden') + extra + bounds);
                if (layer.typename === 'LayerSet') walk(layer, p, depth + 1);
            }
        }
        walk(doc, '', 0);
        out.open('w'); out.encoding = 'UTF8'; out.write(rows.join('\n')); out.close();
        return 'wrote ' + rows.length + ' rows to ' + out.fsName;
    } finally {
        doc.close(SaveOptions.DONOTSAVECHANGES);
        try { app.activeDocument = prior; } catch (ignore) {}
    }
}());
