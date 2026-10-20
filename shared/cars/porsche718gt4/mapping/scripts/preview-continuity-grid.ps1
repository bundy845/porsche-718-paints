param([Parameter(Mandatory)][ValidateSet('Install','Restore')][string]$Action,[string]$GapPlan)
$ErrorActionPreference='Stop'
function Save-Session($Value,[string]$Path){
    $json=$Value | ConvertTo-Json -Depth 20
    for($attempt=0;$attempt -lt 8;$attempt++){
        try {[IO.File]::WriteAllText($Path,$json,[Text.UTF8Encoding]::new($false));return}
        catch [IO.IOException] {if($attempt -eq 7){throw};Start-Sleep -Milliseconds 250}
    }
}
$mapping=Split-Path -Parent $PSScriptRoot
$workspace=[IO.Path]::GetFullPath((Join-Path $mapping '../../../../'))
$project=Join-Path $workspace 'breakaway'
if($Action -eq 'Install'){
    if(-not $GapPlan){throw 'Diagnostic install blocked: supply a current documented gap plan; do not repeat historical d008 for a new thread'}
    $preflight=& python (Join-Path $project 'scripts/preflight.py') --action diagnostic --diagnostic-plan $GapPlan
    if($LASTEXITCODE -ne 0){throw "Diagnostic preflight failed: $preflight"}
}
$normal=Join-Path $project 'exports/r004/car_844824.tga'
$diagnostic=Join-Path $mapping 'diagnostics/d008-continuity-grid.tga'
$target='C:\Users\bundy\Documents\iRacing\paint\porsche718gt4\car_844824.tga'
$record=Join-Path $mapping 'diagnostics/installed-session.json'
$projectRecord=Join-Path $project 'exports/r004/installed-session.json'
$plan=Get-Content -Raw -LiteralPath (Join-Path $mapping 'diagnostics/d008-plan.json') | ConvertFrom-Json
$diagnosticValidation=Get-Content -Raw -LiteralPath (Join-Path $mapping 'diagnostics/d008-validation.json') | ConvertFrom-Json
if($diagnosticValidation.status -ne 'PASS' -or $diagnosticValidation.tga_sha256 -ne $plan.tga_sha256){throw 'Diagnostic validation has not passed'}
$validation=Get-Content -Raw -LiteralPath (Join-Path $project 'exports/r004/validation.json') | ConvertFrom-Json
function Get-Sha([string]$Path){(Get-FileHash -LiteralPath $Path -Algorithm SHA256).Hash.ToLowerInvariant()}
$normalHash=$validation.files.'car_844824.tga'.sha256
if($validation.status -ne 'PASS' -or (Get-Sha $normal) -ne $normalHash){throw 'Validated normal restore source changed'}
if((Get-Sha $diagnostic) -ne $plan.tga_sha256){throw 'Frozen diagnostic changed'}
$previous=Get-Content -Raw -LiteralPath $record | ConvertFrom-Json -AsHashtable
$projectState=Get-Content -Raw -LiteralPath $projectRecord | ConvertFrom-Json -AsHashtable
if($Action -eq 'Install'){
    if($previous.status -ne 'restored'){throw 'Previous diagnostic session is not restored'}
    if((Get-Sha $target) -ne $normalHash){throw 'Installed paint differs from validated r004; refusing to replace intervening edits'}
    $state=[ordered]@{status='installing';diagnostic='d008-continuity-grid';target=$target;diagnostic_sha256=$plan.tga_sha256;normal_sha256=$normalHash;restore_source=$normal;backup_created=$false;backup_policy='User explicitly waived backups; restore uses existing validated r004 export';installed_at_utc=[DateTime]::UtcNow.ToString('o');previous_session=$previous}
    $source=$diagnostic;$expected=$plan.tga_sha256
}else{
    if($previous.diagnostic -ne 'd008-continuity-grid' -or $previous.status -ne 'installed'){throw 'd008 is not recorded as installed'}
    if((Get-Sha $target) -ne $previous.diagnostic_sha256){throw 'Installed diagnostic changed; refusing to replace intervening edits'}
    $state=$previous;$state.status='restoring';$source=$normal;$expected=$normalHash
}
Save-Session $state $record
Copy-Item -LiteralPath $source -Destination $target -Force
[IO.File]::SetLastWriteTimeUtc($target,[DateTime]::UtcNow)
if((Get-Sha $target) -ne $expected){throw 'Installed paint hash mismatch'}
if($Action -eq 'Install'){
    $state.status='installed';$projectState.status='temporarily_replaced_by_diagnostic'
    $projectState['active_diagnostic']='shared/cars/porsche718gt4/mapping/diagnostics/installed-session.json'
}else{
    $state.status='restored';$state['restored_at_utc']=[DateTime]::UtcNow.ToString('o');$projectState.status='installed'
    $projectState.Remove('active_diagnostic')
}
$state['installed_paint_sha256']=$expected
Save-Session $state $record
Save-Session $projectState $projectRecord
[ordered]@{status=$state.status;action=$Action;diagnostic=$state.diagnostic;sha256=$expected;target=$target;spec_modified=$false;backup_created=$false} | ConvertTo-Json -Compress
