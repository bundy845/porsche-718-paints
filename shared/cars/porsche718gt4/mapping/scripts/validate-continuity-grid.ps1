$ErrorActionPreference='Stop'
$mapping=Split-Path -Parent $PSScriptRoot
$workspace=[IO.Path]::GetFullPath((Join-Path $mapping '../../../../'))
Add-Type -AssemblyName System.Drawing.Common
function Read-Pixels([string]$Path){
    $bitmap=[Drawing.Bitmap]::new($Path)
    try {
        if($bitmap.Width -ne 2048 -or $bitmap.Height -ne 2048){throw 'Wrong dimensions'}
        $rectangle=[Drawing.Rectangle]::new(0,0,2048,2048)
        $copy=$bitmap.Clone($rectangle,[Drawing.Imaging.PixelFormat]::Format32bppArgb)
        try {$data=$copy.LockBits($rectangle,[Drawing.Imaging.ImageLockMode]::ReadOnly,[Drawing.Imaging.PixelFormat]::Format32bppArgb);$bytes=[byte[]]::new(2048*2048*4);[Runtime.InteropServices.Marshal]::Copy($data.Scan0,$bytes,0,$bytes.Length);$copy.UnlockBits($data);return ,$bytes} finally {$copy.Dispose()}
    } finally {$bitmap.Dispose()}
}
$plan=Get-Content -Raw -LiteralPath (Join-Path $mapping 'diagnostics/d008-plan.json') | ConvertFrom-Json
$base=Read-Pixels (Join-Path $workspace 'breakaway/design/previews/rick-paint-flat-r004.png')
$grid=Read-Pixels (Join-Path $mapping 'diagnostics/d008-continuity-grid.png')
$union=[byte[]]::new(2048*2048)
foreach($id in $plan.panels){$mask=Read-Pixels (Join-Path $workspace ('breakaway/graphics/r004/panel-masks/'+$id+'.png'));for($i=0;$i -lt $union.Length;$i++){if($mask[$i*4+3] -gt 0){$union[$i]=255}}}
$tga=[IO.File]::ReadAllBytes((Join-Path $mapping 'diagnostics/d008-continuity-grid.tga'))
if($tga.Length -ne 12582930 -or $tga[2] -ne 2 -or $tga[16] -ne 24 -or $tga[17] -ne 32 -or [BitConverter]::ToUInt16($tga,12) -ne 2048 -or [BitConverter]::ToUInt16($tga,14) -ne 2048){throw 'Invalid TGA header/length'}
$outside=0;$changed=0
for($i=0;$i -lt $union.Length;$i++){
    $p=$i*4;$q=18+$i*3
    for($c=0;$c -lt 3;$c++){
        if($grid[$p+$c] -ne $tga[$q+$c]){throw "PNG/TGA mismatch at $i"}
        if($union[$i] -eq 0 -and $base[$p+$c] -ne $grid[$p+$c]){throw "Diagnostic escaped selected masks at $i"}
    }
    if($union[$i] -eq 0){$outside++}elseif($base[$p] -ne $grid[$p] -or $base[$p+1] -ne $grid[$p+1] -or $base[$p+2] -ne $grid[$p+2]){$changed++}
}
if($changed -lt 2000000){throw 'Insufficient diagnostic coverage'}
if((Get-FileHash -LiteralPath (Join-Path $mapping 'diagnostics/d008-continuity-grid.tga') -Algorithm SHA256).Hash.ToLowerInvariant() -ne $plan.tga_sha256){throw 'Frozen plan hash mismatch'}
$result=[ordered]@{status='PASS';diagnostic=$plan.diagnostic;tga_sha256=$plan.tga_sha256;dimensions=@(2048,2048);png_tga_pixels_match=$true;unchanged_pixels_outside_selected_masks=$outside;changed_pixels_inside_selected_masks=$changed;in_car_verified=$false}
$result | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $mapping 'diagnostics/d008-validation.json') -Encoding utf8
$result | ConvertTo-Json -Compress
