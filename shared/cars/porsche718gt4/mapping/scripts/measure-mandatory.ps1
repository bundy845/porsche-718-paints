$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing
$mappingRoot = Split-Path $PSScriptRoot -Parent
Add-Type -TypeDefinition @'
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
public static class MandatoryCoverage718 {
 const int W=2048;
 static byte[] Read(string path) { using(var input=new Bitmap(path)) using(var b=input.Clone(new Rectangle(0,0,W,W),PixelFormat.Format32bppArgb)) {var d=b.LockBits(new Rectangle(0,0,W,W),ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);var a=new byte[W*W*4];Marshal.Copy(d.Scan0,a,0,a.Length);b.UnlockBits(d);return a;} }
 public static void Run(string root) {
  var mandatory=Read(Path.Combine(root,"template/original-mandatory-raster.png"));
  var rows=File.ReadAllLines(Path.Combine(root,"islands.csv"));
  using(var writer=new StreamWriter(Path.Combine(root,"template/mandatory-coverage.csv"))) {
   writer.WriteLine("id,pixels,mandatory_opaque_pixels,mandatory_any_alpha_pixels");
   for(int i=1;i<rows.Length;i++) {
    var r=rows[i].Split(',');var a=Read(Path.Combine(root,r[6]));int count=0,opaque=0,any=0;
    for(int p=0;p<W*W;p++)if(a[p*4+3]>0) {count++;if(mandatory[p*4+3]>=250)opaque++;if(mandatory[p*4+3]>0)any++;}
    writer.WriteLine(r[0]+","+count+","+opaque+","+any);
   }
  }
  using(var view=new Bitmap(W,W,PixelFormat.Format32bppArgb)) {
   var a=new byte[W*W*4];
   for(int p=0;p<W*W;p++){bool m=mandatory[p*4+3]>=250;a[p*4]=(byte)(m?50:240);a[p*4+1]=(byte)(m?50:240);a[p*4+2]=(byte)(m?220:240);a[p*4+3]=255;}
   var d=view.LockBits(new Rectangle(0,0,W,W),ImageLockMode.WriteOnly,PixelFormat.Format32bppArgb);Marshal.Copy(a,0,d.Scan0,a.Length);view.UnlockBits(d);view.Save(Path.Combine(root,"template/mandatory-protection-preview.png"));
  }
  Console.WriteLine("Measured native Car Mandatory alpha coverage for every reference component.");
 }
}
'@ -ReferencedAssemblies System.Drawing.Common,System.Drawing.Primitives,System.Runtime.InteropServices,System.Collections,System.Console
[MandatoryCoverage718]::Run($mappingRoot)
