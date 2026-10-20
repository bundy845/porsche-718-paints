param(
    [Parameter(Mandatory)][ValidateSet('workflow.jsx')][string]$Script,
    [ValidateSet('inspect','export','preview')][string]$Operation='inspect',
    [string]$Source,[string]$OutputDirectory,[switch]$CheckOnly
)
$ErrorActionPreference='Stop'
$env:PYTHONDONTWRITEBYTECODE='1'
$project=Split-Path -Parent $PSScriptRoot
$state=Get-Content -Raw -LiteralPath (Join-Path $project 'docs/workflow-state.json') | ConvertFrom-Json
$preflightText=& python (Join-Path $PSScriptRoot 'preflight.py') --action $(if($Operation -eq 'export'){'export'}else{'inspect'})
if($LASTEXITCODE -ne 0){throw "Workflow preflight failed: $preflightText"}
$preflight=$preflightText | ConvertFrom-Json
if(-not $Source){$Source=$preflight.source.path}
$Source=[IO.Path]::GetFullPath($Source)
if(-not $Source.StartsWith($project+[IO.Path]::DirectorySeparatorChar,[StringComparison]::OrdinalIgnoreCase) -or -not(Test-Path -LiteralPath $Source -PathType Leaf)){throw 'Source must be an existing project PSD'}
if([IO.Path]::GetExtension($Source) -ne '.psd' -or $Source -match '[\\/]inputs[\\/]'){throw 'Do not use an original template as working source'}
if($Operation -eq 'export'){
    if(-not $OutputDirectory){throw 'A new project output directory is required'}
    $OutputDirectory=[IO.Path]::GetFullPath($OutputDirectory)
    if(-not $OutputDirectory.StartsWith($project+[IO.Path]::DirectorySeparatorChar,[StringComparison]::OrdinalIgnoreCase) -or (Test-Path -LiteralPath $OutputDirectory)){throw 'Output must be a new project directory; overwrite prohibited'}
}
$sourceHash=(Get-FileHash -LiteralPath $Source -Algorithm SHA256).Hash.ToLowerInvariant()
$inputPsd=[IO.Path]::GetFullPath((Join-Path $project $state.template))
$rollback=[IO.Path]::GetFullPath((Join-Path $project $state.rollback))
$jsx=Join-Path $PSScriptRoot $Script
try{$app=New-Object -ComObject Photoshop.Application}catch{throw "Documented Photoshop COM route unavailable: $($_.Exception.Message)"}
foreach($document in @($app.Documents)){
    $opened=try{[IO.Path]::GetFullPath([string]$document.FullName)}catch{''}
    if($opened -eq $Source -and $Operation -eq 'preview'){continue}
    if($opened -in @($Source,$preflight.source.path,$inputPsd,$rollback)){throw 'Project source/template is open. Preserve and close it before automated export/inspection; unsaved edits must not be lost.'}
}
if($CheckOnly){[pscustomobject]@{status='PASS';version=[string]$app.Version;route='elevated Photoshop COM';script=$Script}|ConvertTo-Json -Compress;exit 0}
$legacyUnchanged=$sourceHash -eq $state.rollback_sha256 -or ($sourceHash -eq $state.recorded_source_sha256 -and -not $preflight.source.changed_since_record)
$config=@{operation=$Operation;source=$Source;output=$OutputDirectory;legacyUnchanged=$legacyUnchanged}|ConvertTo-Json -Compress
$audit=Join-Path $project 'docs/workflow-audit'
[IO.File]::WriteAllText((Join-Path $audit 'request.jsxinc'),('var liveryWorkflow='+$config+";`r`n"),[Text.UTF8Encoding]::new($false))
$result=$app.DoJavaScriptFile($jsx)
if((Get-FileHash -LiteralPath $Source -Algorithm SHA256).Hash.ToLowerInvariant() -ne $sourceHash){throw 'Workflow modified source unexpectedly'}
$report=[string]$result | ConvertFrom-Json
if($report.status -ne 'PASS'){throw 'Workflow verification failed'}
$report | Add-Member -NotePropertyName sourceSha256 -NotePropertyValue $sourceHash
$report | ConvertTo-Json -Depth 25 | Set-Content -LiteralPath (Join-Path $audit ($Operation+'-report.json')) -Encoding utf8
if($Operation -eq 'export'){
    $validated=& python (Join-Path $PSScriptRoot 'validate-workflow.py') --exports $OutputDirectory --source $Source --source-hash $sourceHash
    if($LASTEXITCODE -ne 0){throw "Saved-export validation failed: $validated"}
}
[pscustomobject]@{status='PASS';operation=$Operation;sourceUnchanged=$true;report=(Join-Path $audit ($Operation+'-report.json'))}|ConvertTo-Json -Compress
