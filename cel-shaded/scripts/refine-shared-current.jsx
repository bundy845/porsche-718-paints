#target photoshop
(function(){
 var root=File($.fileName).parent.parent, source=File(root.fsName+'/source/rick-cel-shaded.psd');
 var old=app.displayDialogs;app.displayDialogs=DialogModes.NO;var doc;
 function add(file,label,parent){
  var im=app.open(File(root.fsName+'/graphics/current/'+file));
  try{var l=im.activeLayer.duplicate(doc,ElementPlacement.PLACEATBEGINNING);app.activeDocument=doc;l.move(parent,ElementPlacement.PLACEATBEGINNING);l.name=label;l.visible=true;return l;}
  finally{im.close(SaveOptions.DONOTSAVECHANGES);app.activeDocument=doc;}
 }
 function fill(name,r,g,b,parent){
  var d=new ActionDescriptor(),ref=new ActionReference();ref.putClass(stringIDToTypeID('contentLayer'));d.putReference(charIDToTypeID('null'),ref);
  var content=new ActionDescriptor(),solid=new ActionDescriptor(),color=new ActionDescriptor();
  color.putDouble(charIDToTypeID('Rd  '),r);color.putDouble(charIDToTypeID('Grn '),g);color.putDouble(charIDToTypeID('Bl  '),b);
  solid.putObject(charIDToTypeID('Clr '),charIDToTypeID('RGBC'),color);
  content.putObject(charIDToTypeID('Type'),stringIDToTypeID('solidColorLayer'),solid);
  d.putObject(charIDToTypeID('Usng'),stringIDToTypeID('contentLayer'),content);executeAction(charIDToTypeID('Mk  '),d,DialogModes.NO);
  var l=doc.activeLayer;l.move(parent,ElementPlacement.PLACEATBEGINNING);l.name=name;l.grouped=true;return l;
 }
 try{
  // Do not replace an existing unsaved document with an on-disk copy.
  for(var i=0;i<app.documents.length;i++)if(app.documents[i].name==source.name&&!app.documents[i].saved)throw Error('Current master has unsaved edits; preserve them before applying this update.');
  doc=app.open(source);
  var area=doc.layerSets.getByName('Paintable Area'),spec=doc.layerSets.getByName('Custom Spec Map');
  var art=area.layerSets.getByName('[RICK] Fresh cel-shaded artwork');
  art.name='[SHARED] Cel-shaded artwork';
  var base=art.artLayers.getByName('01 Electric green base');
  var colors=art.layerSets.add();colors.name='01 DRIVER COLORS - show ONE fill; double-click swatch';colors.move(base,ElementPlacement.PLACEAFTER);colors.blendMode=BlendMode.NORMAL;
  var surface=add('surface-mask.png','Paintable surface - shared clipping mask',colors);
  doc.activeLayer=surface;
  var rick=fill('RICK - Your color - edit swatch',184,245,0,colors);
  var justin=fill('JUSTIN - Bright orange - edit swatch',255,121,0,colors);justin.visible=false;rick.visible=true;
  base.remove();
  area.artLayers.getByName('Base Color').name='Template blue fallback - color controls are in SHARED';
  var lines=art.layerSets.getByName('02 Main ink - traced panel edges');
  var shade=add('cel-shadows.png','Shared cel shadows - Multiply - adjust opacity',art);shade.blendMode=BlendMode.MULTIPLY;shade.move(lines,ElementPlacement.PLACEAFTER);
  var highlight=add('cel-highlights.png','Shared highlights - Screen - adjust opacity',art);highlight.blendMode=BlendMode.SCREEN;highlight.move(lines,ElementPlacement.PLACEAFTER);
  add('refined-contours.png','04 Body contours, handle frames and inset ducts',art);
  spec.artLayers.getByName('[RICK] Satin ink - matches traced artwork').name='[SHARED] Satin ink - traced artwork';
  add('refined-spec.png','[SHARED] Satin ink - added contours',spec);
  area.visible=true;spec.visible=false;doc.layerSets.getByName('Turn Off Before Exporting TGA').visible=false;
  if(rick.kind!=LayerKind.SOLIDFILL||justin.kind!=LayerKind.SOLIDFILL||!rick.grouped||!justin.grouped)throw Error('Color fill or clipping verification failed');
  doc.activeLayer=rick;doc.save();
  var png=new PNGSaveOptions(),tga=new TargaSaveOptions();tga.resolution=TargaBitsPerPixels.TWENTYFOUR;tga.alphaChannels=false;tga.rleCompression=true;
  doc.saveAs(File(root.fsName+'/design/previews/paint-flat-current.png'),png,true,Extension.LOWERCASE);
  doc.saveAs(File(root.fsName+'/exports/current/car_844824.tga'),tga,true,Extension.LOWERCASE);
  rick.visible=false;justin.visible=true;
  doc.saveAs(File(root.fsName+'/design/previews/justin-orange-current.png'),png,true,Extension.LOWERCASE);
  justin.visible=false;rick.visible=true;
  area.visible=false;spec.visible=true;
  doc.saveAs(File(root.fsName+'/design/previews/spec-flat-current.png'),png,true,Extension.LOWERCASE);
  doc.saveAs(File(root.fsName+'/exports/current/car_spec_844824.tga'),tga,true,Extension.LOWERCASE);
  area.visible=true;spec.visible=false;doc.activeLayer=rick;doc.save();
  return 'Verified two clipped SOLIDFILL color controls; shared artwork preserved. Saved Rick master/export and Justin orange preview.';
 }finally{if(doc)doc.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;}
}());
