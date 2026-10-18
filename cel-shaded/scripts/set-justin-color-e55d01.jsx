#target photoshop
(function(){
 var root=File($.fileName).parent.parent,source=File(root.fsName+'/source/rick-cel-shaded.psd'),old=app.displayDialogs;app.displayDialogs=DialogModes.NO;var doc;
 try{
  for(var i=0;i<app.documents.length;i++)if(app.documents[i].name==source.name&&!app.documents[i].saved)throw Error('Save manual PSD changes before updating Justin’s swatch.');
  doc=app.open(source);
  var colors=doc.layerSets.getByName('Paintable Area').layerSets.getByName('[SHARED] Cel-shaded artwork').layerSets.getByName('01 DRIVER COLORS - show ONE fill; double-click swatch');
  var previous=colors.artLayers.getByName('JUSTIN - Bright orange - edit swatch');
  var color=new ActionDescriptor();color.putDouble(charIDToTypeID('Rd  '),229);color.putDouble(charIDToTypeID('Grn '),93);color.putDouble(charIDToTypeID('Bl  '),1);
  var solid=new ActionDescriptor();solid.putObject(charIDToTypeID('Clr '),charIDToTypeID('RGBC'),color);
  var content=new ActionDescriptor();content.putObject(charIDToTypeID('Type'),stringIDToTypeID('solidColorLayer'),solid);
  var make=new ActionDescriptor(),ref=new ActionReference();ref.putClass(stringIDToTypeID('contentLayer'));make.putReference(charIDToTypeID('null'),ref);make.putObject(charIDToTypeID('Usng'),stringIDToTypeID('contentLayer'),content);
  executeAction(charIDToTypeID('Mk  '),make,DialogModes.NO);
  var trial=doc.activeLayer;trial.move(colors,ElementPlacement.PLACEATBEGINNING);trial.name='JUSTIN - trial E55D01 - edit swatch';trial.grouped=true;
  var checkRef=new ActionReference();checkRef.putIdentifier(charIDToTypeID('Lyr '),trial.id);
  var adj=executeActionGet(checkRef).getList(stringIDToTypeID('adjustment')).getObjectValue(0),actual=adj.getObjectValue(stringIDToTypeID('color'));
  var r=actual.getDouble(stringIDToTypeID('red')),g=actual.getDouble(stringIDToTypeID('grain')),b=actual.getDouble(stringIDToTypeID('blue'));
  if(Math.round(r)!=229||Math.round(g)!=93||Math.round(b)!=1)throw Error('Justin swatch verification failed: '+r+','+g+','+b);
  trial.visible=false;previous.name='JUSTIN - prior orange - edit swatch';previous.visible=false;
  doc.save();return 'Added and verified clipped Justin Solid Color Fill #E55D01; Rick swatch untouched.';
 }finally{if(doc)doc.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;}
}());
