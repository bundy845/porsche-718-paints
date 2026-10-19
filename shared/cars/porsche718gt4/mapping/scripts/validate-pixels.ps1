$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing
$mappingRoot = Split-Path $PSScriptRoot -Parent
Add-Type -TypeDefinition @'
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
public static class ValidateMappingPixels718 {
  const int W=2048;
  static byte[] Read(string path) {
    using(var input=new Bitmap(path)) using(var b=input.Clone(new Rectangle(0,0,W,W),PixelFormat.Format32bppArgb)) {
      if(input.Width!=W || input.Height!=W) throw new Exception("Image dimensions: "+path);
      var d=b.LockBits(new Rectangle(0,0,W,W),ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);
      var a=new byte[W*W*4];Marshal.Copy(d.Scan0,a,0,a.Length);b.UnlockBits(d);return a;
    }
  }
  public static void Run(string root) {
    var rows=File.ReadAllLines(Path.Combine(root,"islands.csv"));int masks=0;
    for(int i=1;i<rows.Length;i++) {
      var r=rows[i].Split(',');var a=Read(Path.Combine(root,r[6]));int count=0,x0=W,y0=W,x1=0,y1=0;
      for(int p=0;p<W*W;p++)if(a[p*4+3]!=0) {
        if(a[p*4]!=255 || a[p*4+1]!=255 || a[p*4+2]!=255 || a[p*4+3]!=255) throw new Exception("Mask is not binary: "+r[0]);
        count++;int x=p%W,y=p/W;x0=Math.Min(x0,x);y0=Math.Min(y0,y);x1=Math.Max(x1,x+1);y1=Math.Max(y1,y+1);
      }
      if(x0!=int.Parse(r[1]) || y0!=int.Parse(r[2]) || x1!=int.Parse(r[3]) || y1!=int.Parse(r[4]) || count!=int.Parse(r[5])) throw new Exception("Mask geometry mismatch: "+r[0]);masks++;
    }
    foreach(var name in new[]{"d003-small-parts","d004-headlight-joins","d005-remaining-parts","d006-exterior-codes"}) {
      var png=Read(Path.Combine(root,"diagnostics/"+name+".png"));var tga=File.ReadAllBytes(Path.Combine(root,"diagnostics/"+name+".tga"));
      for(int p=0;p<W*W;p++)for(int c=0;c<3;c++)if(png[p*4+c]!=tga[18+p*3+c]) throw new Exception("PNG/TGA mismatch: "+name+" pixel "+p);
    }
    File.WriteAllText(Path.Combine(root,"pixel-validation.json"),"{\"masks_checked\":"+masks+",\"binary_mask_bounds_and_pixel_counts\":\"passed\",\"diagnostic_png_tga_pixel_equality\":\"passed\"}");
    Console.WriteLine(masks+" binary masks: bounds/counts verified. All four diagnostic TGAs match PNG pixels exactly.");
  }
}
'@ -ReferencedAssemblies System.Drawing.Common,System.Drawing.Primitives,System.Runtime.InteropServices,System.Collections,System.Console
[ValidateMappingPixels718]::Run($mappingRoot)
