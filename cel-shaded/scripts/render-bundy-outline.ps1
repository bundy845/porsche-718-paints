$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing.Common
$references = @([System.Drawing.Bitmap].Assembly.Location, [System.Drawing.PointF].Assembly.Location)
Add-Type -Path (Join-Path $PSScriptRoot 'RenderBundyOutline.cs') -ReferencedAssemblies $references
$projectRoot = Split-Path -Parent $PSScriptRoot
$png = Join-Path $projectRoot 'graphics\current\bundy-angular-outline.png'
$svg = Join-Path $projectRoot 'graphics\current\bundy-angular-outline.svg'
$preview = Join-Path $projectRoot 'design\previews\bundy-angular-outline-preview.png'
[RenderBundyOutline]::Run($png, $svg, $preview)
