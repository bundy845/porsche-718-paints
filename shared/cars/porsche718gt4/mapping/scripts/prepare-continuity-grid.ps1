$ErrorActionPreference='Stop'
$mapping=Split-Path -Parent $PSScriptRoot
$workspace=[IO.Path]::GetFullPath((Join-Path $mapping '../../../../'))
$project=Join-Path $workspace 'breakaway'
$paint=Join-Path $project 'design/previews/rick-paint-flat-r004.png'
$prefix=Join-Path $mapping 'diagnostics/d008-continuity-grid'
if((Test-Path -LiteralPath ($prefix+'.tga')) -or (Test-Path -LiteralPath ($prefix+'.png'))){throw 'Frozen diagnostic already exists'}
Add-Type -AssemblyName System.Drawing.Common
$references=@([System.Drawing.Bitmap].Assembly.Location,[System.Drawing.PointF].Assembly.Location)
$references+=@(Get-ChildItem -LiteralPath (Join-Path $PSHOME 'ref') -Filter '*.dll' | Select-Object -ExpandProperty FullName)
Add-Type -Path (Join-Path $PSScriptRoot 'ContinuityGridRenderer.cs') -ReferencedAssemblies $references
[ContinuityGridRenderer]::Run($paint,(Join-Path $project 'graphics/r004/panel-masks'),$prefix)
[ordered]@{
    diagnostic='d008-continuity-grid';status='prepared_not_installed';based_on='breakaway r004 Rick'
    base_sha256=(Get-FileHash -LiteralPath $paint -Algorithm SHA256).Hash.ToLowerInvariant()
    tga_sha256=(Get-FileHash -LiteralPath ($prefix+'.tga') -Algorithm SHA256).Hash.ToLowerInvariant()
    panels=[ContinuityGridRenderer]::Panels
    cell_size_px=64;small_part_cell_size_px=32
    small_parts=@('I120','I143','I078','I156','I040','I041','I009','I002','I020')
    labels='column,row in native UV space. Cell c,r bounds are [c*step,r*step,(c+1)*step,(r+1)*step). Different parts can use different steps; do not equate their labels.'
    purpose='Measure corresponding boundary points and local orientation/scale at hood-fender, handle-recess, intake-body, roof-deck and rear quarter-bumper joins. Existing guessed transforms are not confirmed.'
    required_views=@('Front overhead with hood and both fenders','Driver side close enough to read handle/intake/mirror cells','Passenger side close enough to read handle/intake/mirror cells','Driver rear three-quarter with deck and bumper corner','Passenger rear three-quarter with deck and bumper corner','Straight rear with both corners and center inset')
    restore_from='breakaway/exports/r004/car_844824.tga'
    limits='Native mask ownership remains provisional at exact boundaries. No new unknown island identities asserted. Spec reflections are not mapping signals. Diagnostic never enters normal livery exports.'
} | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $mapping 'diagnostics/d008-plan.json') -Encoding utf8
Write-Output 'Prepared frozen d008 continuity grid; no installation performed.'
