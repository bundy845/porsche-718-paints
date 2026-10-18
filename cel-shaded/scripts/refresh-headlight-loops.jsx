#target photoshop
(function(){
 var root=File($.fileName).parent.parent,old=app.displayDialogs,doc,asset;app.displayDialogs=DialogModes.NO;
 function replace(file,name,parent){asset=app.open(File(root.fsName+'/graphics/current/'+file));var l=asset.activeLayer.duplicate(doc,ElementPlacement.PLACEATBEGINNING);asset.close(SaveOptions.DONOTSAVECHANGES);asset=null;app.activeDocument=doc;l.move(parent,ElementPlacement.PLACEATBEGINNING);var previous=parent.artLayers.getByName(name);l.name=name+' updated';previous.remove();l.name=name;l.visible=true;}
 try{
  doc=app.open(File(root.fsName+'/source/rick-cel-shaded.psd'));var art=doc.layerSets.getByName('Paintable Area').layerSets.getByName('[SHARED] Cel-shaded artwork'),group=art.layerSets.getByName('Headlights - heavy sketch loops'),spec=doc.layerSets.getByName('Custom Spec Map');
  replace('headlight-loops-passenger.png','Passenger - three sweeps across fender and bumper',group);replace('headlight-loops-driver.png','Driver - three sweeps across fender and bumper',group);replace('headlight-loops-spec.png','[SHARED] Satin headlight sketch loops',spec);
  spec.visible=false;doc.activeLayer=group;doc.save();return 'Refreshed headlight layers using only their connected paint islands.';
 }finally{if(asset)asset.close(SaveOptions.DONOTSAVECHANGES);if(doc)doc.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;}
}());
