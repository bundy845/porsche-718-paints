param(
    [Parameter(Mandatory)][ValidateSet('refresh-headlight-loops.jsx', 'remove-headlight-loops.jsx', 'copy-driver-headlight-echo.jsx', 'export-shared-current.jsx')][string]$Script,
    [switch]$CheckOnly
)

$ErrorActionPreference = 'Stop'
$project = Split-Path -Parent $PSScriptRoot
$master = [IO.Path]::GetFullPath((Join-Path $project 'source/rick-cel-shaded.psd'))
$jsx = Join-Path $PSScriptRoot $Script
if (-not (Test-Path -LiteralPath $master -PathType Leaf)) { throw "Missing master: $master" }
if (-not (Test-Path -LiteralPath $jsx -PathType Leaf)) { throw "Missing JSX: $jsx" }

try { $app = New-Object -ComObject Photoshop.Application }
catch { throw "Photoshop COM unavailable through this route: $($_.Exception.Message)" }

$openMaster = @($app.Documents | Where-Object {
    try { [IO.Path]::GetFullPath([string]$_.FullName) -ieq $master } catch { $false }
})
if ($openMaster.Count -gt 0 -and -not [bool]$openMaster[0].Saved) {
    throw 'The master is open in Photoshop with unsaved edits. Save or resolve those edits before automation.'
}
if ($openMaster.Count -gt 0) {
    throw 'The master is open in Photoshop. Close it after saving so the existing JSX cannot close the editing session.'
}

if ($CheckOnly) {
    [pscustomobject]@{ status = 'PASS'; route = 'elevated Photoshop COM'; version = [string]$app.Version; script = $Script; masterClosed = $true } | ConvertTo-Json -Compress
    exit 0
}

try {
    $result = $app.DoJavaScriptFile([IO.Path]::GetFullPath($jsx))
    [pscustomobject]@{ status = 'PASS'; script = $Script; result = [string]$result } | ConvertTo-Json -Compress
}
catch {
    throw "Photoshop JSX failed ($Script): $($_.Exception.Message)"
}
