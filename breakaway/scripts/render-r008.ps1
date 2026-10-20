$ErrorActionPreference='Stop'
$project=Split-Path -Parent $PSScriptRoot
$workspace=Split-Path -Parent $project
$output=Join-Path $project 'graphics/r008'
if(Test-Path -LiteralPath (Join-Path $output 'placement.json')){throw 'Preserve existing r008 revision'}
New-Item -ItemType Directory -Path $output -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $project 'graphics/r007/breakaway-source.png') -Destination (Join-Path $output 'breakaway-source.png')
$parent=Get-Content -Raw -LiteralPath (Join-Path $project 'graphics/r007/placement.json') | ConvertFrom-Json -AsHashtable
$profile=Get-Content -Raw -LiteralPath (Join-Path $workspace 'shared/cars/porsche718gt4/mapping/panels.json') | ConvertFrom-Json
$panels=@($profile.panels|Where-Object {$_.status -eq 'confirmed'})
if($panels.Count -ne 28){throw 'Confirmed inventory changed'}
Add-Type -AssemblyName System.Drawing.Common
$refs=@([System.Drawing.Bitmap].Assembly.Location,[System.Drawing.PointF].Assembly.Location)
$refs+=@(Get-ChildItem -LiteralPath (Join-Path $PSHOME 'ref') -Filter '*.dll'|Select-Object -ExpandProperty FullName)
Add-Type -Path (Join-Path $PSScriptRoot 'PatternRenderer.cs') -ReferencedAssemblies $refs
$renderer=[HexPatternRenderer]::new((Join-Path $workspace 'shared/cars/porsche718gt4/mapping/template/base-reference.png'),(Join-Path $workspace 'shared/cars/porsche718gt4/mapping/template/original-mandatory-raster.png'),$output)
try{
    foreach($p in $panels){$b=$p.bbox_px;$renderer.AddPanel($p.id,$b[0],$b[1],$b[2],$b[3])}
    $renderer.ResolveOwnership()
    $renderer.ResolveFrontRear((Join-Path $workspace 'shared/cars/porsche718gt4/mapping/masks/I009.png'),(Join-Path $workspace 'shared/cars/porsche718gt4/mapping/masks/I007.png'),(Join-Path $workspace 'shared/cars/porsche718gt4/mapping/wire/wire-source.png'))
    $renderer.LoadPaintMasks((Join-Path $project 'graphics/r007'))
    $renderer.RearReturnsOnly((Join-Path $output 'breakaway-source.png'))
    $renderer.Save();$renderer.Report
    $parent.revision='r008';$parent.parent='r007';$parent.design='Continuous rear field: quarter returns turn into rear face, deck and lips supply rear-face pattern'
    $parent.texture_placements=@($parent.texture_placements|Where-Object {$_.island -notin @('I007','I021','I040','I041')})
    $parent['rear_wrap']['r008_change']='Only I007 side returns modified. Preserve quarter, deck, lips, center rear and inset from r007. Driver x<=1216 and passenger x>=1856 retain fore/aft source field; blend into unchanged rear face over x1216..1344 and1856..1728.'
    $parent['rear_wrap']['side_return_fit']='Driver virtual bodyX=rearX+768, bodyY=1792+.8*rearY; passenger bodyX=3840-rearX, bodyY=768-.8*rearY. G01/G03 row/column orientation fit; exact phase provisional.'
    $parent.design='r007 preserved with local fore/aft side-return correction; delayed turn onto rear face'
    $parent['hood_pins']='User explicitly deferred; no changes to pin fittings or their mapping'
    $parent['number_box']='Still deferred; options before installation'
    $parent | ConvertTo-Json -Depth 12 | Set-Content -LiteralPath (Join-Path $output 'placement.json') -Encoding utf8
    $samples=@()
    foreach($point in @(@('I007',1024,100),@('I007',1152,100),@('I007',1152,160),@('I007',1536,12),@('I007',1536,120),@('I007',1920,100),@('I007',2000,100),@('I007',1920,160),@('I129',2038,1269),@('I040',1823,394),@('I041',1369,348))){$uv=if($point[0] -eq 'I007'){$renderer.RearReturnUV([double]$point[1],[double]$point[2])}else{$renderer.RearUV($point[0],[double]$point[1],[double]$point[2])};$samples+=@{panel=$point[0];native_xy=@($point[1],$point[2]);source_uv=$uv}}
    $samples | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $output 'rear-field-samples.json') -Encoding utf8
}finally{$renderer.Dispose()}
