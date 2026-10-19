#target photoshop
(function(){
 var root=File($.fileName).parent.parent,old=app.displayDialogs,units=app.preferences.rulerUnits,doc,asset;
 app.displayDialogs=DialogModes.NO;app.preferences.rulerUnits=Units.PIXELS;
 function clearTarget(layer){doc.activeLayer=layer;doc.selection.select([[380,710],[600,710],[600,950],[380,950]]);doc.selection.clear();doc.selection.deselect();}
 function add(file,name,parent){asset=app.open(File(root.fsName+'/graphics/current/'+file));var l=asset.activeLayer.duplicate(doc,ElementPlacement.PLACEATBEGINNING);asset.close(SaveOptions.DONOTSAVECHANGES);asset=null;app.activeDocument=doc;l.move(parent,ElementPlacement.PLACEATBEGINNING);l.name=name;l.visible=true;}
 try{
  doc=app.open(File(root.fsName+'/source/rick-cel-shaded.psd'));
  var art=doc.layerSets.getByName('Paintable Area').layerSets.getByName('[SHARED] Cel-shaded artwork');
  var spec=doc.layerSets.getByName('Custom Spec Map');
  var rejected=art.artLayers.getByName('Passenger - small headlight echo marks');
  var rejectedSpec=spec.artLayers.getByName('[SHARED] Satin passenger headlight echo marks');
  var echoes=art.artLayers.getByName('03 Short echoes and corner hatching');
  var tracedSpec=spec.artLayers.getByName('[SHARED] Satin ink - traced artwork');
  rejected.remove();rejectedSpec.remove();
  clearTarget(echoes);clearTarget(tracedSpec);
  add('passenger-headlight-copied-marks.png','Passenger headlight - flipped driver marks',art);
  add('passenger-headlight-copied-spec.png','[SHARED] Satin passenger headlight - copied marks and original seams',spec);
  spec.visible=false;doc.activeLayer=art;doc.save();
  return 'Removed invented marks; replaced passenger headlight echoes with exact vertically flipped driver pixels and matching spec.';
 }finally{if(asset)asset.close(SaveOptions.DONOTSAVECHANGES);if(doc)doc.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;app.preferences.rulerUnits=units;}
}());
