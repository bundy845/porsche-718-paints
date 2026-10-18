#target photoshop
(function(){
 var root=File($.fileName).parent.parent,source=File(root.fsName+'/source/rick-cel-shaded.psd'),old=app.displayDialogs,doc,checkpoint;
 app.displayDialogs=DialogModes.NO;
 function fit(l,cx,cy,w,h){var b=l.bounds;l.resize(w/(b[2].as('px')-b[0].as('px'))*100,h/(b[3].as('px')-b[1].as('px'))*100,AnchorPosition.MIDDLECENTER);b=l.bounds;l.translate(cx-(b[0].as('px')+b[2].as('px'))/2,cy-(b[1].as('px')+b[3].as('px'))/2);}
 function selectPixels(l){doc.activeLayer=l;var d=new ActionDescriptor(),r=new ActionReference();r.putProperty(charIDToTypeID('Chnl'),charIDToTypeID('fsel'));d.putReference(charIDToTypeID('null'),r);r=new ActionReference();r.putEnumerated(charIDToTypeID('Chnl'),charIDToTypeID('Chnl'),charIDToTypeID('Trsp'));d.putReference(charIDToTypeID('T   '),r);executeAction(charIDToTypeID('setd'),d,DialogModes.NO);}
 try{
  for(var i=0;i<app.documents.length;i++)if(app.documents[i].name==source.name){doc=app.documents[i];break;}
  if(!doc)doc=app.open(source);app.activeDocument=doc;
  var art=doc.layerSets.getByName('Paintable Area').layerSets.getByName('[SHARED] Cel-shaded artwork');
  for(var j=0;j<art.layerSets.length;j++)if(art.layerSets[j].name=='Door number panels - 2Old / 2Slow')throw Error('Number panel group already exists.');
  var folder=Folder(root.fsName+'/archive/current-before-number-panels-20261018');if(!folder.exists)folder.create();
  var archive=File(folder.fsName+'/rick-cel-shaded-open-state.psd');if(archive.exists)throw Error('Checkpoint exists; refusing to overwrite.');
  checkpoint=doc.duplicate('Before number panels - including open edits',false);checkpoint.saveAs(archive,new PhotoshopSaveOptions(),true);checkpoint.close(SaveOptions.DONOTSAVECHANGES);checkpoint=null;app.activeDocument=doc;
  var g=art.layerSets.add();g.name='Door number panels - 2Old / 2Slow';
  var place=new ActionDescriptor();place.putPath(charIDToTypeID('null'),File(root.fsName+'/graphics/current/2old-2slow-number-panel.svg'));executeAction(stringIDToTypeID('placeEvent'),place,DialogModes.NO);
  var driver=doc.activeLayer;driver.name='Driver number panel - shared embedded vector';driver.move(g,ElementPlacement.PLACEATBEGINNING);fit(driver,996.5,1888,154,158);
  var passenger=driver.duplicate();passenger.name='Passenger number panel - same vector - rotated 180';passenger.rotate(180,AnchorPosition.MIDDLECENTER);fit(passenger,996.5,651,154,158);
  var spec=doc.layerSets.getByName('Custom Spec Map'),finish=spec.artLayers.add();finish.name='[SHARED] Satin number panels';
  var c=new SolidColor();c.rgb.red=0;c.rgb.green=135;c.rgb.blue=0;
  selectPixels(driver);doc.activeLayer=finish;doc.selection.fill(c);doc.selection.deselect();
  selectPixels(passenger);doc.activeLayer=finish;doc.selection.fill(c);doc.selection.deselect();finish.visible=true;
  doc.layerSets.getByName('Turn Off Before Exporting TGA').visible=false;spec.visible=false;
  doc.activeLayer=driver;doc.save();
  return 'Saved shared number panels; open edits included in checkpoint and master. Driver '+driver.bounds+'; passenger '+passenger.bounds;
 }finally{if(checkpoint)checkpoint.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;}
}());
