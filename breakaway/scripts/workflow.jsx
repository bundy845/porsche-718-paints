#target photoshop
#include "json.jsxinc"
#include "../docs/workflow-audit/request.jsxinc"
(function(){
    if(typeof liveryWorkflow=='undefined')throw Error('Use invoke-photoshop.ps1; workflow configuration required');
    var config=liveryWorkflow;liveryWorkflow=undefined;
    var doc=null,prior=app.documents.length?app.activeDocument:null;
    var working=null;
    var dialogs=app.displayDialogs,units=app.preferences.rulerUnits;
    function color(layer){
        doc.activeLayer=layer;
        var ref=new ActionReference();ref.putEnumerated(charIDToTypeID('Lyr '),charIDToTypeID('Ordn'),charIDToTypeID('Trgt'));
        var descriptor=executeActionGet(ref).getList(stringIDToTypeID('adjustment')).getObjectValue(0).getObjectValue(charIDToTypeID('Clr '));
        return [descriptor.getDouble(charIDToTypeID('Rd  ')),descriptor.getDouble(charIDToTypeID('Grn ')),descriptor.getDouble(charIDToTypeID('Bl  '))];
    }
    function tree(group){
        var result=[];
        for(var i=0;i<group.layers.length;i++){
            var l=group.layers[i],r={name:l.name,type:l.typename,visible:l.visible,blendMode:String(l.blendMode),opacity:l.opacity};
            if(l.typename=='LayerSet')r.layers=tree(l);
            else{r.kind=String(l.kind);r.clipped=l.grouped;if(l.kind==LayerKind.SOLIDFILL)r.rgb=color(l);}
            result.push(r);
        }
        return result;
    }
    function find(group,prefix){for(var i=0;i<group.layers.length;i++)if(group.layers[i].name.indexOf(prefix)==0)return group.layers[i];throw Error('Missing layer '+prefix);}
    function solid(rgb){var c=new SolidColor();c.rgb.red=rgb[0];c.rgb.green=rgb[1];c.rgb.blue=rgb[2];return c;}
    function paintGroup(){return find(doc.layerSets.getByName('Paintable Area'),'[SHARED] Paint breakaway');}
    function paletteExists(){try{doc.layerSets.getByName('PALETTES');return true;}catch(e){return false;}}
    function validatePalette(){
        var palettes=doc.layerSets.getByName('PALETTES'),area=doc.layerSets.getByName('Paintable Area');
        for(var i=0;i<2;i++){
            var driver=i?'JUSTIN':'RICK',group=palettes.layerSets.getByName(driver);
            for(var j=0;j<2;j++)if(find(group,j?'SECONDARY':'PRIMARY').kind!=LayerKind.SMARTOBJECT)throw Error('Palette control must be an embedded linked smart object');
            if(find(area,'['+driver+'] PRIMARY native fallback').kind!=LayerKind.SMARTOBJECT)throw Error('Missing linked native fallback');
        }
    }
    function setState(driver,paint){
        var palette=paletteExists();
        if(!palette&&paint){
            // Legacy input is hash-gated by the launcher. Read its real saved fill, never script RGB.
            var fill=find(paintGroup().layerSets.getByName('1 base'),'['+driver.toUpperCase()+']');
            var rgb=color(fill),base=doc.layerSets.getByName('Paintable Area').artLayers.getByName('Base Color');
            var locked=base.allLocked;base.allLocked=false;doc.activeLayer=base;
            doc.selection.selectAll();doc.selection.fill(solid(rgb));doc.selection.deselect();base.allLocked=locked;
        }
        function toggle(group){for(var i=0;i<group.layers.length;i++){
            var l=group.layers[i];
            if(l.name=='PALETTES'||l.name.indexOf('HOW TO EDIT')==0){l.visible=false;continue;}
            if(l.name.indexOf('[RICK]')==0)l.visible=driver=='Rick';
            else if(l.name.indexOf('[JUSTIN]')==0)l.visible=driver=='Justin';
            if(l.typename=='LayerSet')toggle(l);
        }}
        toggle(doc);
        doc.layerSets.getByName('Paintable Area').visible=paint;
        doc.layerSets.getByName('Custom Spec Map').visible=!paint;
        doc.layerSets.getByName('Turn Off Before Exporting TGA').visible=false;
    }
    function exportSet(folder){
        if(!folder.exists&&!folder.create())throw Error('Cannot create export directory');
        var tga=new TargaSaveOptions();tga.resolution=TargaBitsPerPixels.TWENTYFOUR;tga.alphaChannels=false;tga.rleCompression=true;
        var drivers=['Rick','Justin'],ids=['844824','845005'];
        for(var d=0;d<2;d++)for(var s=0;s<2;s++){
            setState(drivers[d],s==0);
            var name='car_'+(s?'spec_':'')+ids[d];
            doc.saveAs(File(folder.fsName+'/'+name+'.tga'),tga,true,Extension.LOWERCASE);
            doc.saveAs(File(folder.fsName+'/'+name+'.png'),new PNGSaveOptions(),true,Extension.LOWERCASE);
        }
    }
    function duplicateExport(folder){
        var saved=doc;working=doc.duplicate('Temporary export - never save source',false);doc=working;
        try{exportSet(folder);}finally{working.close(SaveOptions.DONOTSAVECHANGES);working=null;doc=saved;app.activeDocument=doc;}
    }
    try{
        app.displayDialogs=DialogModes.NO;app.preferences.rulerUnits=Units.PIXELS;
        if(config.operation=='preview'){
            for(var n=0;n<app.documents.length;n++){try{if(app.documents[n].fullName.fsName==File(config.source).fsName)doc=app.documents[n];}catch(e){}}
        }
        if(!doc)doc=app.open(File(config.source));app.activeDocument=doc;
        if(config.operation=='export'){
            if(!paletteExists()&&!config.legacyUnchanged)throw Error('Changed legacy source has unlinked native Base Color: explicit migration/merge decision required');
            if(paletteExists())validatePalette();
            var folder=Folder(config.output);if(folder.exists||!folder.create())throw Error('Export output must be new');
            duplicateExport(folder);
            return hexJSON({status:'PASS',operation:'export',source:config.source,output:folder.fsName,sourceSaved:false,temporaryDocument:true});
        }
        if(config.operation=='preview'){
            if(!paletteExists())throw Error('Preview requires a linked-palette PSD');
            validatePalette();
            doc.activeLayer=doc.layerSets.getByName('PALETTES').layerSets.getByName('RICK').artLayers.getByName('PRIMARY - double click thumbnail');
            try{app.bringToFront();}catch(e){}
            return hexJSON({status:'PASS',operation:'preview',source:config.source,paletteControls:4,sourceSaved:false,leftOpenForUserReview:true});
        }
        if(config.operation!='inspect')throw Error('Unsupported workflow operation');
        var result={status:'PASS',operation:'inspect',photoshopVersion:app.version,source:config.source,width:doc.width.as('px'),height:doc.height.as('px'),mode:String(doc.mode),bits:String(doc.bitsPerChannel),layers:tree(doc),sourceSaved:false};
        return hexJSON(result);
    }finally{
        if(working)working.close(SaveOptions.DONOTSAVECHANGES);
        if(doc&&config.operation!='preview')doc.close(SaveOptions.DONOTSAVECHANGES);
        app.displayDialogs=dialogs;app.preferences.rulerUnits=units;
        if(prior&&config.operation!='preview')app.activeDocument=prior;
    }
}());
