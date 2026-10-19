$ErrorActionPreference='Stop'
Add-Type -AssemblyName System.Drawing
$refs=@('System.Drawing.Common','System.Drawing.Primitives','System.Runtime','System.Runtime.InteropServices','System.Collections','System.IO')
Add-Type -Path (Join-Path $PSScriptRoot 'RenderPassengerHeadlightEcho.cs') -ReferencedAssemblies $refs
[RenderPassengerHeadlightEcho]::Run((Split-Path -Parent $PSScriptRoot))
Write-Output 'Rendered passenger headlight accents, matching spec, and flat proof.'
