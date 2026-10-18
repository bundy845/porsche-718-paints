#target photoshop
(function(){
 var root=File($.fileName).parent.parent,source=File(root.fsName+'/source/rick-cel-shaded.psd');
 var old=app.displayDialogs,units=app.preferences.rulerUnits,doc,checkpoint,asset,isolated;
 app.displayDialogs=DialogModes.NO;app.preferences.rulerUnits=Units.PIXELS;
 function selectPixels(l){doc.activeLayer=l;var d=new ActionDescriptor(),r=new ActionReference();r.putProperty(charIDToTypeID('Chnl'),charIDToTypeID('fsel'));d.putReference(charIDToTypeID('null'),r);r=new ActionReference();r.putEnumerated(charIDToTypeID('Chnl'),charIDToTypeID('Chnl'),charIDToTypeID('Trsp'));d.putReference(charIDToTypeID('T   '),r);executeAction(charIDToTypeID('setd'),d,DialogModes.NO);}
 try{
  for(var i=0;i<app.documents.length;i++)if(app.documents[i].name==source.name){doc=app.documents[i];break;}
  if(!doc)doc=app.open(source);app.activeDocument=doc;
  var art=doc.layerSets.getByName('Paintable Area').layerSets.getByName('[SHARED] Cel-shaded artwork');
  for(var j=0;j<art.layerSets.length;j++)if(art.layerSets[j].name=='Hood team logo - 2 Old 2 Slow')throw Error('Hood team logo already exists.');
  var folder=Folder(root.fsName+'/archive/current-before-hood-logo-20261018');if(!folder.exists)folder.create();
  var archive=File(folder.fsName+'/rick-cel-shaded-open-state.psd');if(archive.exists)throw Error('Checkpoint exists; refusing to overwrite.');
  checkpoint=doc.duplicate('Before hood logo - including open edits',false);var options=new PhotoshopSaveOptions();options.layers=true;options.embedColorProfile=true;checkpoint.saveAs(archive,options,true);checkpoint.close(SaveOptions.DONOTSAVECHANGES);checkpoint=null;
  app.activeDocument=doc;var group=art.layerSets.add();group.name='Hood team logo - 2 Old 2 Slow';
  asset=app.open(File(root.fsName+'/graphics/logos/2-old-2-slow-driver-door.psd'));
  var logo=asset.layers[0].duplicate(doc,ElementPlacement.PLACEATBEGINNING);asset.close(SaveOptions.DONOTSAVECHANGES);asset=null;app.activeDocument=doc;
  logo.move(group,ElementPlacement.PLACEATBEGINNING);logo.name='2 Old 2 Slow - hood - front reading - embedded original';logo.visible=true;
  // Source is upright. On the 718 hood, +UV x points toward the windshield.
  // Clockwise 90 puts the top of the logo toward the windshield and its baseline toward the nose.
  logo.rotate(90,AnchorPosition.MIDDLECENTER);var b=logo.bounds;
  var scale=240/(b[3].as('px')-b[1].as('px'))*100;logo.resize(scale,scale,AnchorPosition.MIDDLECENTER);
  b=logo.bounds;logo.translate(550-(b[0].as('px')+b[2].as('px'))/2,1270-(b[1].as('px')+b[3].as('px'))/2);
  b=logo.bounds;if(b[0].as('px')<433||b[2].as('px')>667||b[1].as('px')<1147||b[3].as('px')>1393)throw Error('Hood logo outside planned safe rectangle: '+b);
  var spec=doc.layerSets.getByName('Custom Spec Map'),finish=spec.artLayers.add();finish.name='[SHARED] Satin hood team logo';
  var c=new SolidColor();c.rgb.red=0;c.rgb.green=135;c.rgb.blue=0;selectPixels(logo);doc.activeLayer=finish;doc.selection.fill(c);doc.selection.deselect();finish.visible=true;
  // Preserve a native-size placement image for wire-overlay and bounds inspection.
  isolated=app.documents.add(2048,2048,doc.resolution,'Hood logo placement',NewDocumentMode.RGB,DocumentFill.TRANSPARENT);app.activeDocument=doc;logo.duplicate(isolated,ElementPlacement.PLACEATBEGINNING);app.activeDocument=isolated;
  isolated.saveAs(File(root.fsName+'/design/diagnostics/hood-logo-placement.png'),new PNGSaveOptions(),true);isolated.close(SaveOptions.DONOTSAVECHANGES);isolated=null;app.activeDocument=doc;
  doc.layerSets.getByName('Turn Off Before Exporting TGA').visible=false;spec.visible=false;doc.activeLayer=logo;doc.save();
  return 'Saved original embedded smart-object hood logo, uniformly scaled and rotated +90 degrees. Bounds='+logo.bounds+'; kind='+logo.kind;
 }finally{if(checkpoint)checkpoint.close(SaveOptions.DONOTSAVECHANGES);if(asset)asset.close(SaveOptions.DONOTSAVECHANGES);if(isolated)isolated.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;app.preferences.rulerUnits=units;}
}());
