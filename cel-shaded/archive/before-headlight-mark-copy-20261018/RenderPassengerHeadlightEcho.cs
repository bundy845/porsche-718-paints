using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

// Small passenger accents corresponding to the driver lamp's surviving marks.
// The three heavy surround loops are intentionally absent.
public static class RenderPassengerHeadlightEcho {
 const int N=2048;
 static Bitmap New(){var b=new Bitmap(N,N,PixelFormat.Format32bppArgb);b.SetResolution(72,72);return b;}
 static void Stroke(Graphics g,float width,int alpha,PointF a,PointF b,PointF c,PointF d){
  using(var path=new GraphicsPath())using(var pen=new Pen(Color.FromArgb(alpha,18,24,18),width)){
   path.AddBezier(a,b,c,d);pen.StartCap=pen.EndCap=LineCap.Round;g.DrawPath(pen,path);
  }
 }
 public static void Run(string project){
  string dir=Path.Combine(project,"graphics","current");
  string diag=Path.Combine(project,"design","diagnostics");
  using(var mask=new Bitmap(Path.Combine(dir,"base-current.png")))
  using(var ink=New())using(var spec=New()){
   using(var g=Graphics.FromImage(ink)){
    g.SmoothingMode=SmoothingMode.AntiAlias;
    // Upper tapered end: broken echoes like the surviving driver marks.
    Stroke(g,1.9f,220,new PointF(486,735),new PointF(504,733),new PointF(525,741),new PointF(539,750));
    Stroke(g,1.25f,185,new PointF(508,751),new PointF(523,751),new PointF(540,758),new PointF(551,766));
    // Lower tapered end: two short strokes outside the lens opening.
    Stroke(g,1.8f,215,new PointF(403,919),new PointF(417,916),new PointF(437,905),new PointF(453,896));
    Stroke(g,1.15f,180,new PointF(417,924),new PointF(428,919),new PointF(439,914),new PointF(446,910));
   }
   for(int y=720;y<950;y++)for(int x=380;x<600;x++){
    var c=ink.GetPixel(x,y);if(c.A==0)continue;
    if(mask.GetPixel(x,y).A<128){ink.SetPixel(x,y,Color.Transparent);continue;}
    spec.SetPixel(x,y,Color.FromArgb(c.A,0,135,0));
   }
   ink.Save(Path.Combine(dir,"passenger-headlight-small-echo.png"),ImageFormat.Png);
   spec.Save(Path.Combine(dir,"passenger-headlight-small-echo-spec.png"),ImageFormat.Png);
   using(var proof=new Bitmap(Path.Combine(project,"design","previews","paint-flat-current.png"))){
    using(var g=Graphics.FromImage(proof))g.DrawImage(ink,new Rectangle(0,0,N,N),0,0,N,N,GraphicsUnit.Pixel);
    using(var crop=proof.Clone(new Rectangle(270,690,370,350),PixelFormat.Format32bppArgb))
    using(var enlarged=new Bitmap(1110,1050)){
     using(var g=Graphics.FromImage(enlarged)){g.InterpolationMode=InterpolationMode.NearestNeighbor;g.DrawImage(crop,new Rectangle(0,0,1110,1050),0,0,370,350,GraphicsUnit.Pixel);}
     enlarged.Save(Path.Combine(diag,"passenger-headlight-small-echo-proof.png"),ImageFormat.Png);
    }
   }
  }
 }
}
