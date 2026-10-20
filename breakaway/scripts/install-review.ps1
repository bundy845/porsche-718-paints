param([Parameter(Mandatory)][ValidateSet('r001','r002','r003','r004','r005','r006','r007','r008')][string]$Revision,
    [string]$ExpectedInstalledPaintHash)
$ErrorActionPreference='Stop'
function Save-InstallState($Value,[string]$Path){
    $json=$Value | ConvertTo-Json -Depth 8
    for($attempt=0;$attempt -lt 8;$attempt++){
        try {[IO.File]::WriteAllText($Path,$json,[Text.UTF8Encoding]::new($false));return}
        catch [IO.IOException] {if($attempt -eq 7){throw};Start-Sleep -Milliseconds 250}
    }
}
$project=Split-Path -Parent $PSScriptRoot
$preflightText=& python (Join-Path $PSScriptRoot 'preflight.py') --action install
if($LASTEXITCODE -ne 0){throw "Workflow preflight failed: $preflightText"}
$preflight=$preflightText | ConvertFrom-Json
$workflowStatePath=Join-Path $project 'docs/workflow-state.json'
$workflowState=Get-Content -Raw -LiteralPath $workflowStatePath | ConvertFrom-Json
if($Revision -ne $workflowState.revision){throw 'Historical installation blocked: active-state revision differs'}
$folder=Join-Path $project $workflowState.exports
$target='C:\Users\bundy\Documents\iRacing\paint\porsche718gt4'
$manifest=Join-Path $folder 'export-manifest.json'
$validation=Get-Content -Raw -LiteralPath $(if(Test-Path -LiteralPath $manifest){$manifest}else{Join-Path $folder 'validation.json'}) | ConvertFrom-Json
if($validation.status -ne 'PASS' -or $validation.revision -ne $Revision){throw 'Revision validation has not passed'}
if(Test-Path -LiteralPath $manifest){
    if([IO.Path]::GetFullPath($validation.source) -ne $preflight.source.path -or $validation.source_sha256 -ne $preflight.source.sha256){throw 'Export manifest does not match current saved master'}
}
if(-not(Test-Path -LiteralPath $target -PathType Container)){throw 'Verified iRacing car folder missing'}
$names=@('car_844824.tga','car_spec_844824.tga')
# Check both destination files before creating state or replacing either file.
foreach($name in $names){
    $observed=$preflight.installed.files.PSObject.Properties[$name].Value
    $destination=Join-Path $target $name
    $current=if(Test-Path -LiteralPath $destination){(Get-FileHash -LiteralPath $destination -Algorithm SHA256).Hash.ToLowerInvariant()}else{$null}
    if($current -ne $observed.actual_sha256){throw "Destination changed during preflight: $name"}
    if(-not $observed.matches_record){
        if($name -ne 'car_844824.tga' -or -not $ExpectedInstalledPaintHash -or $current -ne $ExpectedInstalledPaintHash.ToLowerInvariant()){
            throw "Unexpected installed file: $name. Preserve it; an explicit overwrite decision and matching ExpectedInstalledPaintHash are required for changed paint. Changed spec requires investigation."
        }
    }
}
foreach($name in $names){$source=Join-Path $folder $name;$expected=$validation.files.PSObject.Properties[$name].Value.sha256;if((Get-FileHash -LiteralPath $source -Algorithm SHA256).Hash.ToLowerInvariant() -ne $expected){throw "Validated source changed: $name"}}
$state=[ordered]@{status='installing';revision=$Revision;driver='Rick';customer_id='844824';paint_folder=$target;backup_created=$false;backup_policy='User explicitly waived backups';installed_at_utc=[DateTime]::UtcNow.ToString('o');files=@{};in_car_verified=$false;spec_mip_status='Awaiting simulator regeneration'}
$record=Join-Path $folder 'installed-session.json'
Save-InstallState $state $record
foreach($name in $names){
    $destination=Join-Path $target $name
    Copy-Item -LiteralPath (Join-Path $folder $name) -Destination $destination -Force
    [IO.File]::SetLastWriteTimeUtc($destination,[DateTime]::UtcNow)
    $actual=(Get-FileHash -LiteralPath $destination -Algorithm SHA256).Hash.ToLowerInvariant()
    if($actual -ne $validation.files.PSObject.Properties[$name].Value.sha256){throw "Installed hash mismatch: $name"}
    $state.files[$name]=@{sha256=$actual;bytes=(Get-Item -LiteralPath $destination).Length}
}
$mip=Join-Path $target 'car_spec_844824.mip'
if(Test-Path -LiteralPath $mip){$state.spec_mip_status=if((Get-Item -LiteralPath $mip).LastWriteTimeUtc -lt (Get-Item -LiteralPath (Join-Path $target 'car_spec_844824.tga')).LastWriteTimeUtc){'Existing MIP older than installed spec; regeneration pending'}else{'MIP timestamp current; finish review pending'}}
$state.status='installed';Save-InstallState $state $record
$workflowState | Add-Member -Force -NotePropertyName installed_record -NotePropertyValue ($workflowState.exports+'/installed-session.json')
Save-InstallState $workflowState $workflowStatePath
$state | ConvertTo-Json -Depth 5 -Compress
