#target photoshop
(function(){
 var root=File($.fileName).parent.parent;
 var source=File(root.fsName+'/source/rick-cel-shaded.psd');
 var old=app.displayDialogs;app.displayDialogs=DialogModes.NO;var doc;
 try {
  doc=app.open(File(root.fsName+'/inputs/Porsche 718 GT4.psd'));
  var area=doc.layerSets.getByName('Paintable Area');
  var spec=doc.layerSets.getByName('Custom Spec Map');
  doc.layerSets.getByName('Turn Off Before Exporting TGA').visible=false;
  area.artLayers.getByName('Car Decal').visible=false;
  area.artLayers.getByName('Pitbox').visible=false;
  area.layerSets.getByName('Car Patterns').visible=false;
  function add(file,label,parent){
   var im=app.open(File(root.fsName+'/graphics/current/'+file));
   try {
    var l=im.activeLayer.duplicate(doc,ElementPlacement.PLACEATBEGINNING);
    app.activeDocument=doc;l.move(parent,ElementPlacement.PLACEATBEGINNING);l.name=label;l.visible=true;
    return l;
   }finally{im.close(SaveOptions.DONOTSAVECHANGES);app.activeDocument=doc;}
  }
  var art=area.layerSets.add();art.name='[RICK] Fresh cel-shaded artwork';
  add('base-current.png','01 Electric green base',art);
  var lines=art.layerSets.add();lines.name='02 Main ink - traced panel edges';
  var files=['hood','driver','passenger','roof-deck','rear-bumper','wing','front-parts','small-parts'];
  var labels=['Hood','Driver side - lower UV island','Passenger side - upper UV island','Roof and rear deck','Rear bumper','Wing','Front bumper and splitter','Separate small parts'];
  for(var k=0;k<files.length;k++)add('seams-'+files[k]+'.png',labels[k],lines);
  add('accents-current.png','03 Short echoes and corner hatching',art);
  var finish=add('spec-current.png','[RICK] Satin ink - matches traced artwork',spec);
  area.visible=true;spec.visible=false;
  var psd=new PhotoshopSaveOptions();psd.layers=true;
  doc.saveAs(source,psd,true,Extension.LOWERCASE);
  doc.saveAs(File(root.fsName+'/design/previews/paint-flat-current.png'),new PNGSaveOptions(),true,Extension.LOWERCASE);
  var tga=new TargaSaveOptions();tga.resolution=TargaBitsPerPixels.TWENTYFOUR;tga.alphaChannels=false;tga.rleCompression=true;
  doc.saveAs(File(root.fsName+'/exports/current/car_844824.tga'),tga,true,Extension.LOWERCASE);
  area.visible=false;spec.visible=true;finish.visible=true;
  doc.saveAs(File(root.fsName+'/design/previews/spec-flat-current.png'),new PNGSaveOptions(),true,Extension.LOWERCASE);
  doc.saveAs(File(root.fsName+'/exports/current/car_spec_844824.tga'),tga,true,Extension.LOWERCASE);
  return 'Fresh layered master, paint preview and current paint/spec TGAs saved.';
 }finally{if(doc)doc.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;}
}());
