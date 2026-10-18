#target photoshop
(function(){
 var root=File($.fileName).parent.parent,source=File(root.fsName+'/source/rick-cel-shaded.psd');
 var old=app.displayDialogs;app.displayDialogs=DialogModes.NO;var doc;
 try{
  for(var i=0;i<app.documents.length;i++)if(app.documents[i].name==source.name&&!app.documents[i].saved)throw Error('Save any open PSD edits before updating the color fill label.');
  doc=app.open(source);
  var colors=doc.layerSets.getByName('Paintable Area').layerSets.getByName('[SHARED] Cel-shaded artwork').layerSets.getByName('01 DRIVER COLORS - show ONE fill; double-click swatch');
  var rick=null,justin=null;for(var i=0;i<colors.artLayers.length;i++){var l=colors.artLayers[i];if(l.name.indexOf('RICK -')===0)rick=l;if(l.name.indexOf('JUSTIN -')===0)justin=l;}
  if(!rick||!justin)throw Error('Expected both existing color fills.');
  rick.name='RICK - Your color - edit swatch';justin.name='JUSTIN - Bright orange - edit swatch';
  // Rename only; leave fill swatches, visibility and artwork pixels untouched.
  doc.save();return 'Renamed color fill labels; existing swatches and visibility were preserved.';
 }finally{if(doc)doc.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;}
}());
