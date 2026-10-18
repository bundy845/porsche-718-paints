$ErrorActionPreference = 'Stop'
$project = Split-Path -Parent $PSScriptRoot
$mask = Join-Path $project 'design\previews\template-clean-mask-r000.png'
$graphics = Join-Path $project 'graphics'

Add-Type -AssemblyName System.Drawing.Common
$references = @([System.Drawing.Bitmap].Assembly.Location, [System.Drawing.PointF].Assembly.Location)
Add-Type -Path (Join-Path $PSScriptRoot 'RenderOverlays.cs') -ReferencedAssemblies $references
[RenderOverlays]::Run($mask, $graphics)
