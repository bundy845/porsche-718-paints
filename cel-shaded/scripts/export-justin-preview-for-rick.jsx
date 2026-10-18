#target photoshop
(function(){
 var root=File($.fileName).parent.parent,old=app.displayDialogs;app.displayDialogs=DialogModes.NO;var doc;
 function snapshot(file,png){
  app.refresh();var out=doc.duplicate('Justin preview for Rick',true);
  try{out.flatten();var t=new TargaSaveOptions();t.resolution=TargaBitsPerPixels.TWENTYFOUR;t.alphaChannels=false;t.rleCompression=true;out.saveAs(file,t,true,Extension.LOWERCASE);
   if(png)out.saveAs(png,new PNGSaveOptions(),true,Extension.LOWERCASE);
  }finally{out.close(SaveOptions.DONOTSAVECHANGES);app.activeDocument=doc;}
 }
 try{
  for(var i=0;i<app.documents.length;i++)if(app.documents[i].name=='rick-cel-shaded.psd'&&!app.documents[i].saved)throw Error('Save manual changes to the current master before exporting.');
  doc=app.open(File(root.fsName+'/source/rick-cel-shaded.psd'));
  var area=doc.layerSets.getByName('Paintable Area'),spec=doc.layerSets.getByName('Custom Spec Map');
  var colors=area.layerSets.getByName('[SHARED] Cel-shaded artwork').layerSets.getByName('01 DRIVER COLORS - show ONE fill; double-click swatch');
  var rick=colors.artLayers.getByName('RICK - Your color - edit swatch'),justin=colors.artLayers.getByName('JUSTIN - trial E55D01 - edit swatch');
  var outDir=Folder(root.fsName+'/design/previews/justin-on-844824');if(!outDir.exists)outDir.create();
  doc.layerSets.getByName('Turn Off Before Exporting TGA').visible=false;area.visible=true;spec.visible=false;
  rick.visible=false;justin.visible=true;snapshot(File(outDir.fsName+'/car_844824.tga'),File(outDir.fsName+'/paint-flat.png'));
  area.visible=false;spec.visible=true;
  spec.artLayers.getByName('[SHARED] Satin ink - traced artwork').visible=true;
  spec.artLayers.getByName('[SHARED] Satin ink - added contours').visible=true;
  snapshot(File(outDir.fsName+'/car_spec_844824.tga'),File(outDir.fsName+'/spec-flat.png'));
  return 'Exported Justin fill as Rick customer ID 844824 for local preview; master left unchanged.';
 }finally{if(doc)doc.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;}
}());
