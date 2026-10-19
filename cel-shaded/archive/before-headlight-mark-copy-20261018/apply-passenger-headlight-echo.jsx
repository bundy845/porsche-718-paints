#target photoshop
(function(){
 var root=File($.fileName).parent.parent,old=app.displayDialogs,doc,asset;app.displayDialogs=DialogModes.NO;
 function add(file,name,parent){
  asset=app.open(File(root.fsName+'/graphics/current/'+file));
  var layer=asset.activeLayer.duplicate(doc,ElementPlacement.PLACEATBEGINNING);
  asset.close(SaveOptions.DONOTSAVECHANGES);asset=null;app.activeDocument=doc;
  layer.move(parent,ElementPlacement.PLACEATBEGINNING);layer.name=name;layer.visible=true;
 }
 try{
  doc=app.open(File(root.fsName+'/source/rick-cel-shaded.psd'));
  var art=doc.layerSets.getByName('Paintable Area').layerSets.getByName('[SHARED] Cel-shaded artwork');
  var spec=doc.layerSets.getByName('Custom Spec Map');
  for(var i=0;i<art.artLayers.length;i++)if(art.artLayers[i].name=='Passenger - small headlight echo marks')throw Error('Passenger echo layer already exists.');
  add('passenger-headlight-small-echo.png','Passenger - small headlight echo marks',art);
  add('passenger-headlight-small-echo-spec.png','[SHARED] Satin passenger headlight echo marks',spec);
  spec.visible=false;doc.activeLayer=art;doc.save();
  return 'Added only the small passenger headlight echo marks and their satin spec layer.';
 }finally{if(asset)asset.close(SaveOptions.DONOTSAVECHANGES);if(doc)doc.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;}
}());
