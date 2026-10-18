#target photoshop
(function(){
 var root=File($.fileName).parent.parent;
 var d=app.open(File(root.fsName+'/inputs/Porsche 718 GT4.psd'));
 var old=app.displayDialogs;app.displayDialogs=DialogModes.NO;
 try {
  for(var i=0;i<d.layers.length;i++)d.layers[i].visible=false;
  var guides=d.layerSets.getByName('Turn Off Before Exporting TGA');guides.visible=true;
  for(var j=0;j<guides.layers.length;j++)guides.layers[j].visible=guides.layers[j].name==='Wire';
  d.saveAs(File(root.fsName+'/design/diagnostics/wire-source.png'),new PNGSaveOptions(),true,Extension.LOWERCASE);
  return 'Exported isolated wire at '+d.width.as('px')+' x '+d.height.as('px');
 }finally{d.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;}
}());
