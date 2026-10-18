#target photoshop
(function(){
 var root=File($.fileName).parent.parent,source=File(root.fsName+'/source/rick-cel-shaded.psd');
 var old=app.displayDialogs,units=app.preferences.rulerUnits,doc,checkpoint,asset;
 app.displayDialogs=DialogModes.NO;app.preferences.rulerUnits=Units.PIXELS;
 function clearRect(layer,x,y,w,h){doc.activeLayer=layer;doc.selection.select([[x,y],[x+w,y],[x+w,y+h],[x,y+h]]);doc.selection.clear();doc.selection.deselect();}
 function importLayer(file,name,parent){asset=app.open(File(root.fsName+'/graphics/current/'+file));var layer=asset.activeLayer.duplicate(doc,ElementPlacement.PLACEATBEGINNING);asset.close(SaveOptions.DONOTSAVECHANGES);asset=null;app.activeDocument=doc;layer.name=name;layer.move(parent,ElementPlacement.PLACEATBEGINNING);layer.visible=true;return layer;}
 try{
  for(var i=0;i<app.documents.length;i++)if(app.documents[i].name==source.name){doc=app.documents[i];break;}
  if(!doc)doc=app.open(source);app.activeDocument=doc;
  var art=doc.layerSets.getByName('Paintable Area').layerSets.getByName('[SHARED] Cel-shaded artwork'),spec=doc.layerSets.getByName('Custom Spec Map');
  for(var j=0;j<art.layerSets.length;j++)if(art.layerSets[j].name=='Headlights - heavy sketch loops')throw Error('Headlight-loop group already exists.');
  var folder=Folder(root.fsName+'/archive/current-before-headlight-loops-20261018');if(!folder.exists)folder.create();var archive=File(folder.fsName+'/rick-cel-shaded-open-state.psd');if(archive.exists)throw Error('Checkpoint exists; refusing to overwrite.');
  checkpoint=doc.duplicate('Before full headlight surrounds - including open edits',false);var psd=new PhotoshopSaveOptions();psd.layers=true;psd.embedColorProfile=true;checkpoint.saveAs(archive,psd,true);checkpoint.close(SaveOptions.DONOTSAVECHANGES);checkpoint=null;app.activeDocument=doc;
  // Remove only the obsolete partial fender echoes in the secondary contour
  // layer and its matching spec. Primary seams and other artwork are retained.
  var contour=art.artLayers.getByName('04 Body contours, handle frames and inset ducts');
  var priorSpec=spec.artLayers.getByName('[SHARED] Satin ink - added contours');
  clearRect(contour,360,710,260,250);clearRect(contour,360,1585,260,240);
  clearRect(priorSpec,360,710,260,250);clearRect(priorSpec,360,1585,260,240);
  var group=art.layerSets.add();group.name='Headlights - heavy sketch loops';
  importLayer('headlight-loops-passenger.png','Passenger - three sweeps across fender and bumper',group);
  importLayer('headlight-loops-driver.png','Driver - three sweeps across fender and bumper',group);
  importLayer('headlight-loops-spec.png','[SHARED] Satin headlight sketch loops',spec);
  doc.layerSets.getByName('Turn Off Before Exporting TGA').visible=false;spec.visible=false;doc.activeLayer=group;doc.save();
  return 'Saved three heavier headlight sweeps on each side, with bumper returns; removed the old partial fender accents and updated the matching satin spec.';
 }finally{if(checkpoint)checkpoint.close(SaveOptions.DONOTSAVECHANGES);if(asset)asset.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;app.preferences.rulerUnits=units;}
}());
