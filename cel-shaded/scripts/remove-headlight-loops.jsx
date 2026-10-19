#target photoshop
(function(){
 var root=File($.fileName).parent.parent,old=app.displayDialogs,doc;app.displayDialogs=DialogModes.NO;
 try{
  doc=app.open(File(root.fsName+'/source/rick-cel-shaded.psd'));
  var area=doc.layerSets.getByName('Paintable Area');
  var art=area.layerSets.getByName('[SHARED] Cel-shaded artwork');
  var loops=art.layerSets.getByName('Headlights - heavy sketch loops');
  var satin=doc.layerSets.getByName('Custom Spec Map').artLayers.getByName('[SHARED] Satin headlight sketch loops');
  loops.remove();satin.remove();
  doc.activeLayer=area;doc.save();
  return 'Removed heavy headlight loops and their matching spec layer from the layered master.';
 }finally{if(doc)doc.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;}
}());
