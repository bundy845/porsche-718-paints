param(
    [ValidateSet('Install','Restore')][string]$Action = 'Install',
    [ValidateSet('d003-small-parts','d004-headlight-joins','d005-remaining-parts','d006-exterior-codes')][string]$Diagnostic = 'd003-small-parts'
)
$ErrorActionPreference = 'Stop'
$mappingRoot = Split-Path $PSScriptRoot -Parent
$paintFolder = 'C:\Users\bundy\Documents\iRacing\paint\porsche718gt4'
$target = Join-Path $paintFolder 'car_844824.tga'
$stateFile = Join-Path $mappingRoot 'diagnostics/installed-session.json'
function Hash([string]$file) { (Get-FileHash -LiteralPath $file -Algorithm SHA256).Hash.ToLowerInvariant() }
if (-not (Test-Path -LiteralPath $target -PathType Leaf)) { throw "Verified destination missing: $target" }
$state = if(Test-Path -LiteralPath $stateFile) { Get-Content -LiteralPath $stateFile -Raw | ConvertFrom-Json } else { $null }
if($Action -eq 'Restore') {
    if(-not $state -or $state.status -ne 'installed') { throw 'No active diagnostic session to restore.' }
    if((Hash $target) -ne $state.diagnostic_sha256) { throw 'Installed paint changed since the diagnostic. Preserve that change and resolve manually.' }
    $backup = Join-Path $mappingRoot $state.backup_paint
    if((Hash $backup) -ne $state.original_sha256) { throw 'Backup hash mismatch.' }
    Copy-Item -LiteralPath $backup -Destination $target
    if((Hash $target) -ne $state.original_sha256) { throw 'Restoration hash mismatch.' }
    $state.status = 'restored'
    $state | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $stateFile -Encoding utf8
    Write-Output 'Original installed paint restored and hash verified. Spec TGA and MIP were never modified.'
    exit
}
$source = Join-Path $mappingRoot "diagnostics/$Diagnostic.tga"
$bytes = [IO.File]::ReadAllBytes($source)
if($bytes.Length -ne 12582930 -or $bytes[2] -ne 2 -or $bytes[13] -ne 8 -or $bytes[15] -ne 8 -or $bytes[16] -ne 24) { throw 'Unexpected diagnostic format.' }
if($state -and $state.status -eq 'installed') {
    if((Hash $target) -ne $state.diagnostic_sha256) { throw 'Installed paint changed. Resolve before switching diagnostics.' }
} else {
    $relativeBackup = 'diagnostics/backups/' + [Guid]::NewGuid().ToString('N')
    $backupFolder = Join-Path $mappingRoot $relativeBackup
    New-Item -ItemType Directory -Path $backupFolder | Out-Null
    $backupHashes = @{}
    foreach($name in @('car_844824.tga','car_spec_844824.tga','car_spec_844824.mip')) {
        $file = Join-Path $paintFolder $name
        if(Test-Path -LiteralPath $file) {
            Copy-Item -LiteralPath $file -Destination $backupFolder
            $backupHashes[$name] = Hash $file
            if((Hash (Join-Path $backupFolder $name)) -ne $backupHashes[$name]) { throw 'Backup verification failed.' }
        }
    }
    $state = [pscustomobject]@{status='prepared'; target=$target; backup_paint="$relativeBackup/car_844824.tga";original_sha256=(Hash $target);backup_hashes=$backupHashes;diagnostic=$Diagnostic;diagnostic_sha256=''}
}
$state.diagnostic = $Diagnostic
$state.diagnostic_sha256 = Hash $source
# Persist recovery details before changing the destination.
$state.status = 'installed'
$state | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $stateFile -Encoding utf8
Copy-Item -LiteralPath $source -Destination $target
if((Hash $target) -ne $state.diagnostic_sha256) { throw 'Installed hash mismatch; recovery information retained.' }
Write-Output "Installed $Diagnostic for Rick (844824), hash verified. Original paint backed up. Spec files unchanged."
