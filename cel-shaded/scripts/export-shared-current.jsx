#target photoshop
(function(){
 var root=File($.fileName).parent.parent,old=app.displayDialogs;app.displayDialogs=DialogModes.NO;var doc;
 function output(pngName,tgaName){
  app.refresh();
  // Snapshot the rendered state before changing any layer visibility again.
  var out=doc.duplicate('Livery export snapshot',true);
  try{
   out.flatten();
   out.saveAs(File(root.fsName+'/design/previews/'+pngName),new PNGSaveOptions(),true,Extension.LOWERCASE);
   if(tgaName){var t=new TargaSaveOptions();t.resolution=TargaBitsPerPixels.TWENTYFOUR;t.alphaChannels=false;t.rleCompression=true;out.saveAs(File(root.fsName+'/exports/current/'+tgaName),t,true,Extension.LOWERCASE);}
  }finally{out.close(SaveOptions.DONOTSAVECHANGES);app.activeDocument=doc;}
 }
 try{
  for(var i=0;i<app.documents.length;i++)if(app.documents[i].name=='rick-cel-shaded.psd'&&!app.documents[i].saved)throw Error('Save manual changes to the current master before exporting.');
  doc=app.open(File(root.fsName+'/source/rick-cel-shaded.psd'));
  var area=doc.layerSets.getByName('Paintable Area'),spec=doc.layerSets.getByName('Custom Spec Map');
  var colors=area.layerSets.getByName('[SHARED] Cel-shaded artwork').layerSets.getByName('01 DRIVER COLORS - show ONE fill; double-click swatch');
  var rick=colors.artLayers.getByName('RICK - Your color - edit swatch'),justin=colors.artLayers.getByName('JUSTIN - trial E55D01 - edit swatch');
  doc.layerSets.getByName('Turn Off Before Exporting TGA').visible=false;
  area.visible=true;spec.visible=false;justin.visible=false;rick.visible=true;doc.activeLayer=rick;
  output('paint-flat-current.png','car_844824.tga');
  rick.visible=false;justin.visible=true;
  output('justin-orange-current.png',null);
  justin.visible=false;rick.visible=true;
  area.visible=false;spec.visible=true;
  spec.artLayers.getByName('[SHARED] Satin ink - traced artwork').visible=true;
  spec.artLayers.getByName('[SHARED] Satin ink - added contours').visible=true;
  output('spec-flat-current.png','car_spec_844824.tga');
  spec.visible=false;area.visible=true;doc.activeLayer=rick;app.refresh();doc.save();
  return 'Exported refreshed snapshots for Rick, Justin preview and shared spec. Saved master in Rick paint state.';
 }finally{if(doc)doc.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;}
}());
