$ErrorActionPreference = 'Stop'
$project = Split-Path -Parent $PSScriptRoot
$graphics = Join-Path $project 'graphics/current'
$exports = Join-Path $project 'exports/current'
$master = Join-Path $project 'source/rick-cel-shaded.psd'
$checks = @()
if (-not (Test-Path -LiteralPath $master -PathType Leaf)) { throw 'Missing master PSD.' }
$masterTime = (Get-Item -LiteralPath $master).LastWriteTimeUtc

foreach ($name in @('car_844824.tga', 'car_spec_844824.tga')) {
    $path = Join-Path $exports $name
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) { throw "Missing export: $name" }
    # The established exporter saves the PSD after writing TGAs, so allow its short save interval.
    if ((Get-Item -LiteralPath $path).LastWriteTimeUtc -lt $masterTime.AddMinutes(-1)) { throw "Export older than master: $name. Re-export before claiming validation." }
    $stream = [IO.File]::OpenRead($path)
    try {
        $header = New-Object byte[] 18
        if ($stream.Read($header, 0, 18) -ne 18) { throw "Short TGA: $name" }
        $width = [BitConverter]::ToUInt16($header, 12)
        $height = [BitConverter]::ToUInt16($header, 14)
        if ($width -ne 2048 -or $height -ne 2048 -or $header[16] -ne 24 -or $header[2] -ne 10) { throw "Unexpected TGA format: $name ($width x $height, $($header[16]) bpp, type $($header[2]))" }
        $checks += [pscustomobject]@{ file = $name; dimensions = '2048x2048'; bits = 24; sha256 = (Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash }
    } finally { $stream.Dispose() }
}

Add-Type -AssemblyName System.Drawing
$paintPaths = @('headlight-loops-passenger.png', 'headlight-loops-driver.png') | ForEach-Object { Join-Path $graphics $_ }
$specPath = Join-Path $graphics 'headlight-loops-spec.png'
$paint = @($paintPaths | ForEach-Object { [Drawing.Bitmap]::new($_) })
$spec = [Drawing.Bitmap]::new($specPath)
try {
    if ($paint[0].Width -ne 2048 -or $paint[0].Height -ne 2048 -or $paint[1].Width -ne 2048 -or $paint[1].Height -ne 2048 -or $spec.Width -ne 2048 -or $spec.Height -ne 2048) { throw 'Headlight PNG dimensions are not 2048x2048.' }
    $opaque = 0
    for ($y = 0; $y -lt 2048; $y++) {
        for ($x = 0; $x -lt 2048; $x++) {
            $a = [Math]::Max($paint[0].GetPixel($x, $y).A, $paint[1].GetPixel($x, $y).A)
            $s = $spec.GetPixel($x, $y).A
            if ($a -ne $s) { throw "Paint/spec alpha differs at ($x,$y)." }
            if ($a -gt 0) {
                if ($x -lt 270 -or $x -ge 640 -or $y -lt 680 -or $y -ge 1840) { throw "Headlight pixel outside allowed UV region at ($x,$y)." }
                $opaque++
            }
        }
    }
    if ($opaque -eq 0) { throw 'No headlight artwork pixels found.' }
} finally { foreach ($p in $paint) { $p.Dispose() }; $spec.Dispose() }

[pscustomobject]@{ status = 'PASS'; inCar = 'NOT VERIFIED IN-CAR'; exports = $checks; headlightPixels = $opaque; paintSpecAlpha = 'MATCH'; outsideRegion = 0 } | ConvertTo-Json -Compress -Depth 4
