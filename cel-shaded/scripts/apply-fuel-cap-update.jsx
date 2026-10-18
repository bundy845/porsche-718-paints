#target photoshop
(function(){
 var root=File($.fileName).parent.parent,source=File(root.fsName+'/source/rick-cel-shaded.psd');
 var old=app.displayDialogs,doc;app.displayDialogs=DialogModes.NO;
 try{
  for(var i=0;i<app.documents.length;i++)if(app.documents[i].name==source.name&&!app.documents[i].saved)throw Error('Save the current master edits before applying the cap update.');
  doc=app.open(source);
  var art=doc.layerSets.getByName('Paintable Area').layerSets.getByName('[SHARED] Cel-shaded artwork');
  var spec=doc.layerSets.getByName('Custom Spec Map');
  function replace(file,label,parent){
   var previous=parent.artLayers.getByName(label),opacity=previous.opacity,visible=previous.visible;
   var im=app.open(File(root.fsName+'/graphics/current/'+file));
   try{
    var added=im.activeLayer.duplicate(doc,ElementPlacement.PLACEATBEGINNING);app.activeDocument=doc;
    added.move(parent,ElementPlacement.PLACEATBEGINNING);added.name=label+' [new]';added.visible=true;
    if(opacity!=100)added.opacity=opacity;
    added.move(previous,ElementPlacement.PLACEBEFORE);previous.remove();added.name=label;added.visible=visible;
   }finally{app.activeDocument=im;im.close(SaveOptions.DONOTSAVECHANGES);app.activeDocument=doc;}
  }
  replace('refined-contours.png','04 Body contours, handle frames and inset ducts',art);
  replace('refined-spec.png','[SHARED] Satin ink - added contours',spec);
  doc.save();return 'Saved circular fuel-cap accents and matching spec in layered master.';
 }finally{if(doc){app.activeDocument=doc;doc.close(SaveOptions.DONOTSAVECHANGES);}app.displayDialogs=old;}
}());
