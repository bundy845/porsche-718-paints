$ErrorActionPreference='Stop'
Add-Type -AssemblyName System.Drawing
$refs=@('System.Drawing.Common','System.Drawing.Primitives','System.Runtime','System.Collections','System.IO','System.Console')
Add-Type -Path (Join-Path $PSScriptRoot 'CopyDriverHeadlightEcho.cs') -ReferencedAssemblies $refs
[CopyDriverHeadlightEcho]::Run((Split-Path -Parent $PSScriptRoot))
