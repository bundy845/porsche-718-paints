#target photoshop
(function(){
 var root=File($.fileName).parent.parent, old=app.displayDialogs;app.displayDialogs=DialogModes.NO;
 var rows=[],doc,keepOpen=false;
 function walk(p,prefix){for(var i=0;i<p.layers.length;i++){var l=p.layers[i];rows.push(prefix+l.name+' | '+l.visible+' | '+l.bounds);if(l.typename=='LayerSet')walk(l,prefix+'  ');}}
 function find(p,n){for(var i=0;i<p.layers.length;i++){var l=p.layers[i];if(l.name==n)return l;if(l.typename=='LayerSet'){var r=find(l,n);if(r)return r;}}}
 function isolated(l,name){var out=app.documents.add(2048,2048,72,name,NewDocumentMode.RGB,DocumentFill.TRANSPARENT);app.activeDocument=doc;l.duplicate(out,ElementPlacement.PLACEATBEGINNING);app.activeDocument=out;out.layers[0].visible=true;out.saveAs(File(root.fsName+'/design/diagnostics/'+name+'.png'),new PNGSaveOptions(),true);out.close(SaveOptions.DONOTSAVECHANGES);app.activeDocument=doc;}
 try{
  doc=app.open(File(root.fsName+'/inputs/Porsche 718 GT4.psd'));walk(doc,'');
  isolated(find(doc,'Number Blocks'),'template-number-blocks');
  var decal=find(doc,'Car Decal');if(!decal)decal=find(doc,'Car Decals');if(decal)isolated(decal,'template-car-decals');
  doc.close(SaveOptions.DONOTSAVECHANGES);doc=null;
  for(var i=0;i<app.documents.length;i++)if(app.documents[i].name=='rick-cel-shaded.psd'){doc=app.documents[i];keepOpen=true;break;}
  if(!doc)doc=app.open(File(root.fsName+'/source/rick-cel-shaded.psd'));rows.push('CURRENT MASTER saved='+doc.saved);walk(doc,'');
  var f=File(root.fsName+'/design/diagnostics/number-panel-inventory.txt');f.open('w');f.write(rows.join('\n'));f.close();return rows.join('\n');
 }finally{if(doc&&!keepOpen)doc.close(SaveOptions.DONOTSAVECHANGES);app.displayDialogs=old;}
}());
