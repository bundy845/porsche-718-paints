#target photoshop
(function(){
 var root=File($.fileName).parent.parent,doc,snapshot,old=app.displayDialogs;app.displayDialogs=DialogModes.NO;
 try{
  doc=app.open(File(root.fsName+'/source/rick-cel-shaded.psd'));
  var dir=Folder(root.fsName+'/archive/current-before-headlight-bumper-remap-20261018');if(!dir.exists)dir.create();var f=File(dir.fsName+'/rick-cel-shaded-open-state.psd');if(f.exists)throw Error('Remap checkpoint already exists.');
  snapshot=doc.duplicate('Before headlight bumper remap - open state',false);var opts=new PhotoshopSaveOptions();opts.layers=true;opts.embedColorProfile=true;snapshot.saveAs(f,opts,true);snapshot.close(SaveOptions.DONOTSAVECHANGES);snapshot=null;app.activeDocument=doc;return 'Checkpointed current open master before correcting the rejected bumper paths.';
 }finally{if(snapshot)snapshot.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;}
}());
