$ErrorActionPreference='Stop'
Add-Type -AssemblyName System.Drawing
$project=Split-Path -Parent $PSScriptRoot
$out=Join-Path $project 'design\diagnostics'
$rects=@(@('passenger',270,690,370,350),@('driver',270,1500,370,350))
foreach($item in @(@('template-wire-r000.png','wire'),@('paint-flat-current.png','rick'),@('justin-orange-current.png','justin'))){
 $src=[Drawing.Bitmap]::new((Join-Path $project ('design\previews\'+$item[0])))
 if($item[1] -eq 'wire'){
  $ink=[Drawing.Bitmap]::new((Join-Path $out 'headlight-loops-only.png'))
  $g=[Drawing.Graphics]::FromImage($src);$g.DrawImage($ink,[Drawing.Rectangle]::new(0,0,2048,2048));$g.Dispose();$ink.Dispose()
 }
 foreach($s in $rects){
  $crop=$src.Clone([Drawing.Rectangle]::new($s[1],$s[2],$s[3],$s[4]),$src.PixelFormat)
  $b=[Drawing.Bitmap]::new($s[3]*3,$s[4]*3);$g=[Drawing.Graphics]::FromImage($b)
  $g.DrawImage($crop,[Drawing.Rectangle]::new(0,0,$b.Width,$b.Height))
  $b.Save((Join-Path $out ('headlight-loops-'+$s[0]+'-'+$item[1]+'.png')),[Drawing.Imaging.ImageFormat]::Png)
  $g.Dispose();$b.Dispose();$crop.Dispose()
 }
 $src.Dispose()
}
