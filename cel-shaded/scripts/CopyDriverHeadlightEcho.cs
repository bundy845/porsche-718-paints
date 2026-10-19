using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

// Copies actual PSD pixels. No drawing, resampling, or new stroke geometry.
public static class CopyDriverHeadlightEcho {
 const int N=2048,MirrorY=2536;
 static Bitmap New(){var b=new Bitmap(N,N,PixelFormat.Format32bppArgb);b.SetResolution(72,72);return b;}
 public static void Run(string project){
  string src=Path.Combine(project,"design","diagnostics","headlight-copy-source");
  string dir=Path.Combine(project,"graphics","current");
  var target=new Rectangle(380,710,220,240);
  using(var echoes=new Bitmap(Path.Combine(src,"echoes.png")))
  using(var seams=new Bitmap(Path.Combine(src,"passenger-seams.png")))
  using(var mask=new Bitmap(Path.Combine(dir,"base-current.png")))
  using(var copy=New())using(var patch=New())using(var donor=New()){
   // Whole connected source marks: the tiny tip dash, paired outer-seam
   // echoes, inner-tip echo, short middle dash, and lower-tip marks.
   Rectangle[] marks={new Rectangle(425,1595,6,4),new Rectangle(422,1608,54,15),
    new Rectangle(412,1625,43,17),new Rectangle(555,1690,10,12),
    new Rectangle(531,1765,30,16),new Rectangle(488,1787,45,16)};
   int pixels=0,outside=0;var log=new List<string>();
   foreach(var mark in marks){int count=0;
    for(int y=mark.Top;y<mark.Bottom;y++)for(int x=mark.Left;x<mark.Right;x++){
     var c=echoes.GetPixel(x-13,y-4);if(c.A==0)continue;
     int ty=MirrorY-y;
     if(!target.Contains(x,ty))throw new Exception("Copied mark exceeds target selection.");
     if(mask.GetPixel(x,ty).A<128)outside++;
     copy.SetPixel(x,ty,c);donor.SetPixel(x,y,c);pixels++;count++;
    }
    log.Add("Source "+mark+": "+count+" pixels; target x=x, y="+MirrorY+"-y");
   }
   // Restore the primary seam spec in the cleared target rectangle, and
   // include the copied marks using the same alpha maximum as the trace.
   for(int y=target.Top;y<target.Bottom;y++)for(int x=target.Left;x<target.Right;x++){
    int a=Math.Max(seams.GetPixel(x-377,y-497).A,copy.GetPixel(x,y).A);
    if(a>0)patch.SetPixel(x,y,Color.FromArgb(a,0,135,0));
   }
   copy.Save(Path.Combine(dir,"passenger-headlight-copied-marks.png"),ImageFormat.Png);
   patch.Save(Path.Combine(dir,"passenger-headlight-copied-spec.png"),ImageFormat.Png);
   donor.Save(Path.Combine(src,"driver-donor-marks.png"),ImageFormat.Png);
   log.Add("Copied pixels="+pixels+"; outside painted mask="+outside);
   File.WriteAllLines(Path.Combine(src,"copy-report.txt"),log);
   Console.WriteLine(String.Join("\n",log));
   // Source and destination shown in the same orientation for exact comparison.
   using(var proof=new Bitmap(660,390)){
    using(var g=Graphics.FromImage(proof)){g.Clear(Color.FromArgb(72,199,232));
     g.InterpolationMode=System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
     g.DrawImage(donor,new Rectangle(0,0,330,390),400,1585,220,260,GraphicsUnit.Pixel);
     using(var unflipped=New()){
      for(int y=1585;y<1845;y++)for(int x=400;x<620;x++)unflipped.SetPixel(x,y,copy.GetPixel(x,MirrorY-y));
      g.DrawImage(unflipped,new Rectangle(330,0,330,390),400,1585,220,260,GraphicsUnit.Pixel);
     }
    }
    proof.Save(Path.Combine(src,"source-and-copy.png"),ImageFormat.Png);
   }
   using(var wire=new Bitmap(Path.Combine(project,"design","previews","template-wire-r000.png"))){
    using(var g=Graphics.FromImage(wire))g.DrawImage(copy,new Rectangle(0,0,N,N),0,0,N,N,GraphicsUnit.Pixel);
    using(var crop=wire.Clone(new Rectangle(380,710,240,250),PixelFormat.Format32bppArgb))
     crop.Save(Path.Combine(src,"passenger-wire-copy.png"),ImageFormat.Png);
   }
  }
 }
}
