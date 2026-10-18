#target photoshop
(function(){
 var root=File($.fileName).parent.parent,source=File(root.fsName+'/source/rick-cel-shaded.psd');
 var old=app.displayDialogs;app.displayDialogs=DialogModes.NO;var doc,stage='open';
 function replace(folder,file,label,parent,mode){
  stage='import '+label;var im=app.open(File(folder+'/'+file));
  try{
   var layer=im.activeLayer.duplicate(doc,ElementPlacement.PLACEATBEGINNING);
   app.activeDocument=doc;var previous=parent.artLayers.getByName(label+' [previous]');
   var opacity=previous.opacity,visible=previous.visible;
   layer.move(parent,ElementPlacement.PLACEATBEGINNING);layer.name=label;layer.visible=true;
   if(opacity!=100)layer.opacity=opacity;
   if(mode)layer.blendMode=mode;
   layer.visible=visible;
   previous.remove();
  }finally{app.activeDocument=im;im.close(SaveOptions.DONOTSAVECHANGES);app.activeDocument=doc;}
 }
 try{
  for(var i=0;i<app.documents.length;i++)if(app.documents[i].name==source.name&&!app.documents[i].saved)throw Error('Current master has unsaved edits; save them before applying this update.');
  doc=app.open(source);
  var area=doc.layerSets.getByName('Paintable Area'),art=area.layerSets.getByName('[SHARED] Cel-shaded artwork'),spec=doc.layerSets.getByName('Custom Spec Map');
  var folder=root.fsName+'/graphics/current';
  // Add each replacement before removing its previous layer, so an import failure
  // leaves the existing PSD artwork intact.
  art.artLayers.getByName('04 Body contours, handle frames and inset ducts').name='04 Body contours, handle frames and inset ducts [previous]';
  replace(folder,'refined-contours.png','04 Body contours, handle frames and inset ducts',art,BlendMode.NORMAL);
  art.artLayers.getByName('Shared cel shadows - Multiply - adjust opacity').name='Shared cel shadows - Multiply - adjust opacity [previous]';
  replace(folder,'cel-shadows.png','Shared cel shadows - Multiply - adjust opacity',art,BlendMode.MULTIPLY);
  art.artLayers.getByName('Shared highlights - Screen - adjust opacity').name='Shared highlights - Screen - adjust opacity [previous]';
  replace(folder,'cel-highlights.png','Shared highlights - Screen - adjust opacity',art,BlendMode.SCREEN);
  spec.artLayers.getByName('[SHARED] Satin ink - added contours').name='[SHARED] Satin ink - added contours [previous]';
  replace(folder,'refined-spec.png','[SHARED] Satin ink - added contours',spec,BlendMode.NORMAL);
  stage='order layers';var mainInk=art.layerSets.getByName('02 Main ink - traced panel edges');
  art.artLayers.getByName('Shared cel shadows - Multiply - adjust opacity').move(mainInk,ElementPlacement.PLACEAFTER);
  art.artLayers.getByName('Shared highlights - Screen - adjust opacity').move(mainInk,ElementPlacement.PLACEAFTER);
  art.artLayers.getByName('04 Body contours, handle frames and inset ducts').move(art,ElementPlacement.PLACEATBEGINNING);
  stage='restore colors';area.visible=true;spec.visible=false;doc.layerSets.getByName('Turn Off Before Exporting TGA').visible=false;
  var colors=art.layerSets.getByName('01 DRIVER COLORS - show ONE fill; double-click swatch');
  colors.artLayers.getByName('RICK - Your color - edit swatch').visible=true;
  colors.artLayers.getByName('JUSTIN - trial E55D01 - edit swatch').visible=false;
  colors.artLayers.getByName('JUSTIN - prior orange - edit swatch').visible=false;
  stage='save';doc.activeLayer=colors.artLayers.getByName('RICK - Your color - edit swatch');doc.save();
  return 'Updated shared contours, shadows, highlights and spec; preserved Rick color and Justin trial fill.';
 }catch(e){var log=File(root.fsName+'/design/diagnostics/update-error.txt');log.open('w');log.write(stage+'\n'+e.toString()+'\nline '+e.line);log.close();throw e;
 }finally{if(doc){app.activeDocument=doc;doc.close(SaveOptions.DONOTSAVECHANGES);}app.displayDialogs=old;}
}());
