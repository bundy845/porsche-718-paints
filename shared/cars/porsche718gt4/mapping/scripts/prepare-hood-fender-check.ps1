$ErrorActionPreference='Stop'
$mapping=Split-Path -Parent $PSScriptRoot
$workspace=[IO.Path]::GetFullPath((Join-Path $mapping '../../../../'))
$project=Join-Path $workspace 'breakaway'
$paint=Join-Path $project 'design/previews/rick-paint-flat-r003.png'
$masks=Join-Path $project 'graphics/r003/panel-masks'
$prefix=Join-Path $mapping 'diagnostics/d007-hood-fender-check'
if(Test-Path -LiteralPath ($prefix+'.tga')){throw 'Frozen diagnostic already exists; do not overwrite captured diagnostics'}
Add-Type -AssemblyName System.Drawing.Common
$references=@([System.Drawing.Bitmap].Assembly.Location,[System.Drawing.PointF].Assembly.Location)
$references+=@(Get-ChildItem -LiteralPath (Join-Path $PSHOME 'ref') -Filter '*.dll' | Select-Object -ExpandProperty FullName)
Add-Type -Path (Join-Path $PSScriptRoot 'SeamCheckRenderer.cs') -ReferencedAssemblies $references
[SeamCheckRenderer]::Run($paint,$masks,$prefix)
[ordered]@{diagnostic='d007-hood-fender-check';status='prepared_not_installed';based_on='breakaway r003 Rick paint';base_sha256=(Get-FileHash -LiteralPath $paint -Algorithm SHA256).Hash.ToLowerInvariant();tga_sha256=(Get-FileHash -LiteralPath ($prefix+'.tga') -Algorithm SHA256).Hash.ToLowerInvariant();panels=@('I122','I136','I081');seams=@('S01','S02');labels=@(@{number=1;x=640;color='cyan'},@{number=2;x=704;color='orange'},@{number=3;x=768;color='magenta'},@{number=4;x=832;color='white'},@{number=5;x=896;color='red'});purpose='Confirm or reject coarse shared longitudinal registration at hood/fender joins; same numbers are hypotheses, not confirmations';required_view='Front overhead showing hood and both fenders; then front three-quarter if a join is occluded';restore_from='breakaway/exports/r003/car_844824.tga';limits='No exact boundary pairing or uniform texel scale assumed; diagnostics excluded from livery exports'} | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $mapping 'diagnostics/d007-plan.json') -Encoding utf8
Write-Output 'Prepared numbered d007 hood/fender check; normal r003 paint remains separate. No installation performed.'
