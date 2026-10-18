param(
    [Parameter(Mandatory)][ValidateSet('headlights')][string]$Region,
    [Parameter(Mandatory)][ValidateSet('Rick')][string]$Driver,
    [Parameter(Mandatory)][ValidateSet('Preview', 'Apply', 'ExportAndValidate')][string]$Stage
)

$ErrorActionPreference = 'Stop'
$project = Split-Path -Parent $PSScriptRoot
$master = Join-Path $project 'source/rick-cel-shaded.psd'
$diagnostics = Join-Path $project 'design/diagnostics'
$graphics = Join-Path $project 'graphics/current'
$psdBefore = if (Test-Path -LiteralPath $master) { (Get-FileHash -LiteralPath $master -Algorithm SHA256).Hash } else { throw "Missing master: $master" }

if ($Stage -eq 'Preview') {
    & (Join-Path $PSScriptRoot 'render-headlight-loops.ps1') | Out-Null
    & (Join-Path $PSScriptRoot 'preview-headlight-loops.ps1')
    $files = @(
        'graphics/current/headlight-loops-passenger.png',
        'graphics/current/headlight-loops-driver.png',
        'graphics/current/headlight-loops-spec.png',
        'design/diagnostics/headlight-loops-passenger-wire.png',
        'design/diagnostics/headlight-loops-driver-wire.png'
    )
    foreach ($file in $files) { if (-not (Test-Path -LiteralPath (Join-Path $project $file))) { throw "Missing preview output: $file" } }
    if ((Get-FileHash -LiteralPath $master -Algorithm SHA256).Hash -ne $psdBefore) { throw 'Unexpected PSD change during preview.' }
    [pscustomobject]@{ status = 'PASS'; region = $Region; driver = $Driver; stage = $Stage; files = $files; note = 'Rick and Justin paint crops are from the previous export; use wire crops to assess proposed paths. In-car projection remains unverified.' } | ConvertTo-Json -Compress
    exit 0
}

$launcher = Join-Path $PSScriptRoot 'invoke-photoshop.ps1'
if ($Stage -eq 'Apply') {
    & $launcher -Script 'refresh-headlight-loops.jsx' -CheckOnly | Out-Null
    foreach ($name in @('headlight-loops-passenger.png', 'headlight-loops-driver.png', 'headlight-loops-spec.png')) {
        if (-not (Test-Path -LiteralPath (Join-Path $graphics $name))) { throw "Missing rendered asset: $name" }
    }
    $checkpoint = Join-Path $project ('archive/iteration-headlights-' + (Get-Date -Format 'yyyyMMdd-HHmmss'))
    New-Item -ItemType Directory -Path $checkpoint -ErrorAction Stop | Out-Null
    Copy-Item -LiteralPath $master -Destination (Join-Path $checkpoint 'rick-cel-shaded.psd') -ErrorAction Stop
    & $launcher -Script 'refresh-headlight-loops.jsx'
    [pscustomobject]@{ status = 'PASS'; region = $Region; driver = $Driver; stage = $Stage; checkpoint = $checkpoint; note = 'Layer refresh complete; export and in-car review still required.' } | ConvertTo-Json -Compress
    exit 0
}

& $launcher -Script 'export-shared-current.jsx' -CheckOnly | Out-Null
$exports = Join-Path $project 'exports/current'
$backup = Join-Path $project ('archive/exports-before-headlights-' + (Get-Date -Format 'yyyyMMdd-HHmmss'))
New-Item -ItemType Directory -Path $backup -ErrorAction Stop | Out-Null
foreach ($name in @('car_844824.tga', 'car_spec_844824.tga')) {
    $file = Join-Path $exports $name
    if (Test-Path -LiteralPath $file) { Copy-Item -LiteralPath $file -Destination (Join-Path $backup $name) -ErrorAction Stop }
}
& $launcher -Script 'export-shared-current.jsx'
& (Join-Path $PSScriptRoot 'preview-headlight-loops.ps1')
& (Join-Path $PSScriptRoot 'validate-headlights.ps1')
