$ErrorActionPreference='Stop'
Add-Type -AssemblyName System.Drawing.Common
$refs=@([System.Drawing.Bitmap].Assembly.Location,[System.Drawing.PointF].Assembly.Location,(Join-Path $PSHOME 'ref\System.Collections.dll'))
Add-Type -Path (Join-Path $PSScriptRoot 'RenderHeadlightLoops.cs') -ReferencedAssemblies $refs
[RenderHeadlightLoops]::Run((Split-Path -Parent $PSScriptRoot))
