using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

// Secondary marks only. Confirmed primary green-wire traces are untouched.
public static class RefineSharedArt {
 const int N=2048;
 static Bitmap ink,shade,light,mask;
 static Graphics gi,gs,gl;
 static GraphicsPath Curve(float[] p) {var q=new GraphicsPath();var pts=Points(p);if((pts.Length-1)%3==0)q.AddBeziers(pts);else q.AddCurve(pts);return q;}
 static PointF[] Points(float[] p){var q=new PointF[p.Length/2];for(int i=0;i<q.Length;i++)q[i]=new PointF(p[i*2],p[i*2+1]);return q;}
 static void Stroke(float w,params float[] p){using(var q=Curve(p))using(var pen=new Pen(Color.FromArgb(225,18,24,18),w)){pen.StartCap=pen.EndCap=LineCap.Round;gi.DrawPath(pen,q);}}
 static void Shadow(int a,params float[] p){using(var q=Curve(p))using(var b=new SolidBrush(Color.FromArgb(a,0,0,0))){q.CloseFigure();gs.FillPath(b,q);}}
 static void Highlight(params float[] p){using(var q=Curve(p))using(var pen=new Pen(Color.FromArgb(70,255,255,255),2.2f)){pen.StartCap=pen.EndCap=LineCap.Round;gl.DrawPath(pen,q);}}
 static void Tick(float x,float y,float dx,float dy){using(var p=new Pen(Color.FromArgb(210,18,24,18),1.5f))gi.DrawLine(p,x,y,x+dx,y+dy);}
 static bool Green(Color c){return c.A>50&&c.G>c.R+45&&c.G>c.B+45;}
 // Offset only genuine island edges, onto their painted side. Local mesh
 // tangent makes the sketch echo follow the contour instead of crossing it.
 static void OffsetWire(Bitmap wire,int x0,int y0,int x1,int y1,float offset,float width,bool broken=true){
  using(var brush=new SolidBrush(Color.FromArgb(205,18,24,18)))
  for(int y=y0;y<y1;y++)for(int x=x0;x<x1;x++)if(Green(wire.GetPixel(x,y))){
   double sx=0,sy=0;
   for(int dy=-12;dy<=12;dy+=3)for(int dx=-12;dx<=12;dx+=3){int xx=x+dx,yy=y+dy;
    if(xx>=0&&yy>=0&&xx<N&&yy<N&&mask.GetPixel(xx,yy).A>128){sx+=dx;sy+=dy;}}
   double len=Math.Sqrt(sx*sx+sy*sy);if(len<8)continue;
   double d=offset+.8*Math.Sin(x*.06+y*.04),xx2=x+sx/len*d,yy2=y+sy/len*d;
   int px=(int)xx2,py=(int)yy2;if(px<0||py<0||px>=N||py>=N||mask.GetPixel(px,py).A<128)continue;
   if(broken&&Math.Sin(x*.029+y*.041)<-.35)continue;
   gi.FillEllipse(brush,(float)xx2-width/2,(float)yy2-width/2,width,width);
  }
 }
 static void Reinforce(Bitmap wire,int x0,int y0,int x1,int y1,float radius){
  x0=Math.Max(0,x0);y0=Math.Max(0,y0);x1=Math.Min(N,x1);y1=Math.Min(N,y1);
  for(int y=y0;y<y1;y++)for(int x=x0;x<x1;x++)if(Green(wire.GetPixel(x,y))){
   float w=radius+(float)(.45*Math.Sin(x*.21+y*.11));
   using(var p=new SolidBrush(Color.FromArgb(225,18,24,18)))gi.FillEllipse(p,x-w,y-w,w*2,w*2);
   // Broken offset echo on the actual wire, kept slightly irregular.
   int h=(x*17+y*31)%11;if(h<4){double dx=((x*7+y*3)%7)-3,dy=((x*11+y*5)%7)-3;
    using(var p=new SolidBrush(Color.FromArgb(135,18,24,18)))gi.FillEllipse(p,(float)(x+dx-1),(float)(y+dy-1),2,2);
   }
  }
 }
 static void WindowEdge(Bitmap wire){
  // The rear glass is a transparent opening surrounded by painted body.
  // Follow only green wire pixels where the shared paint mask changes sides;
  // this avoids the grille and crest UV atlas inside the opening.
  for(int y=1045;y<1470;y++)for(int x=1570;x<1910;x++)if(Green(wire.GetPixel(x,y))){
   bool inside=false,outside=false;
   for(int d=-8;d<=8;d+=4){if(x+d<0||x+d>=N||y+d<0||y+d>=N)continue;
    if(mask.GetPixel(x+d,y).A>128)inside=true;else outside=true;
    if(mask.GetPixel(x,y+d).A>128)inside=true;else outside=true;
   }
   if(inside&&outside){float w=2.35f+(float)(.4*Math.Sin(x*.15+y*.12));using(var p=new SolidBrush(Color.FromArgb(235,18,24,18)))gi.FillEllipse(p,x-w,y-w,w*2,w*2);}
  }
 }
 static void Handle(float x,float y){
  // Recess surrounds independently identified from the white mesh on each side.
  Stroke(2.4f,x-65,y-19,x-25,y-23,x+36,y-22,x+65,y-16);
  Stroke(2.1f,x-65,y+17,x-20,y+23,x+35,y+21,x+63,y+16);
  Tick(x-58,y-29,2,57);Tick(x+57,y-27,-2,54);
  Tick(x-72,y-12,17,0);Tick(x+51,y+12,22,0);
  Shadow(55,x-52,y+3,x-24,y+14,x+27,y+15,x+51,y+3,x+19,y+23,x-22,y+23,x-52,y+3);
  Highlight(x-45,y-12,x-20,y-15,x+24,y-15,x+41,y-10);
 }
 static Bitmap New(){var b=new Bitmap(N,N,PixelFormat.Format32bppArgb);b.SetResolution(72,72);return b;}
 static void FuelCapRings(){
  // Green-wire cap bounds: x786..871, y751..811. UV ellipse becomes
  // the circular cap on the car; both sketch rings stay outside that edge.
  for(int ring=0;ring<2;ring++){
   double start=ring==0?0:.35,end=ring==0?Math.PI*2:5.55;
   int steps=(int)Math.Ceiling((end-start)/.025);var pts=new PointF[steps+1];
   for(int i=0;i<=steps;i++){double t=start+(end-start)*i/steps;
    double wobble=.45*Math.Sin(5*t+ring)+.25*Math.Sin(9*t);
    pts[i]=new PointF((float)(828.5+(50+ring*6+wobble)*Math.Cos(t)),(float)(781+(37.5+ring*6+wobble)*Math.Sin(t)));
   }
   using(var p=new Pen(Color.FromArgb(ring==0?220:180,18,24,18),ring==0?1.9f:1.1f)){
    p.StartCap=p.EndCap=LineCap.Round;gi.DrawLines(p,pts);
   }
  }
 }
 static Graphics G(Bitmap b){var g=Graphics.FromImage(b);g.SmoothingMode=SmoothingMode.AntiAlias;return g;}
 public static string Run(string folder,string wirePath){
  ink=New();shade=New();light=New();gi=G(ink);gs=G(shade);gl=G(light);
  mask=new Bitmap(Path.Combine(folder,"base-current.png"));
  Handle(1372,1787);Handle(1372,750);
  // (827,775) is the fuel cap, confirmed by the user's car screenshot.
  FuelCapRings();
  // Keep the number panel clear. Main door and shoulder contours come from
  // the traced body seams; no floating strokes cross this area.
  // Tapered lower-door facets follow the mesh's curved return into the intake.
  Shadow(52,1095,1940,1210,1951,1328,1942,1386,1870,1368,1943,1321,1960,1240,1963,1170,1965,1110,1961,1095,1940);
  Shadow(52,1095,618,1210,606,1328,617,1385,688,1369,615,1321,596,1240,594,1170,592,1110,597,1095,618);
  Stroke(1.8f,1100,1935,1220,1946,1320,1935,1363,1889);
  Stroke(1.8f,1100,622,1220,611,1320,621,1363,668);
  Shadow(65,866,2008,1110,2012,1325,2010,1515,2008,1460,2030,1100,2032,865,2026,864,2020,865,2014,866,2008);
  Shadow(65,866,544,1110,541,1325,542,1515,546,1460,520,1100,521,865,525,864,531,865,537,866,544);
  // Roof facet along actual outside edge and hatch corner overrun marks.
  Shadow(38,1108,1054,1111,1180,1102,1350,1110,1470,1122,1420,1130,1210,1124,1080,1119,1070,1114,1060,1108,1054);
  Shadow(38,1504,1070,1500,1210,1503,1350,1500,1463,1482,1410,1486,1220,1485,1082,1492,1078,1498,1075,1504,1070);
  foreach(var p in new[]{new PointF(1220,1260),new PointF(1450,1260),new PointF(1222,1460),new PointF(1450,1450)}){Tick(p.X-14,p.Y,29,0);Tick(p.X,p.Y-14,0,29);}
  // Roof sketch detail follows the escape flap, outside its four edges.
  Stroke(2.2f,1205,1288,1203,1340,1206,1390,1206,1435);
  Stroke(2.1f,1231,1249,1300,1246,1380,1248,1440,1249);
  Stroke(2.2f,1480,1283,1482,1333,1479,1382,1480,1430);
  Stroke(2.0f,1245,1475,1310,1472,1380,1467,1440,1466);
  Stroke(1.1f,1285,1241,1320,1240,1360,1241,1397,1242);
  Stroke(1.1f,1487,1320,1488,1340,1486,1380,1487,1400);
  Shadow(35,1469,1280,1472,1340,1471,1400,1469,1449,1481,1420,1482,1340,1469,1280);
  // Rear-glass echoes must lie on the painted surround, outside the opening.
  Stroke(2.2f,1582,1120,1582,1080,1590,1055,1640,1054);
  Stroke(1.6f,1660,1054,1740,1055,1810,1058,1850,1068);
  Stroke(2.3f,1880,1082,1912,1112,1915,1170,1915,1250);
  Stroke(2.0f,1915,1290,1915,1390,1910,1444,1875,1465);
  Stroke(1.8f,1845,1480,1775,1491,1680,1494,1620,1487);
  Stroke(1.8f,1581,1438,1578,1380,1579,1310,1580,1260);
  // Rear bumper reliefs: follow the two molded horizontal recesses.
  Stroke(2.2f,1095,169,1160,158,1250,158,1317,161);
  Stroke(1.1f,1112,177,1175,169,1250,168,1290,170);
  Stroke(2.2f,1695,159,1760,157,1850,158,1924,170);
  Stroke(1.1f,1725,169,1800,168,1860,169,1910,178);
  Shadow(44,1092,171,1170,162,1260,162,1318,166,1260,177,1165,178,1092,171);
  Shadow(44,1693,164,1780,161,1860,163,1925,173,1860,179,1770,178,1693,164);
  // A neutral facet behind the GT4 MR emboss on the separate front spoiler.
  // The lettering is model relief, not editable text in the paint template.
  Shadow(48,20,1168,44,1162,64,1164,80,1170,88,1220,88,1315,79,1358,55,1364,31,1360,20,1352);
  Stroke(1.5f,22,1183,19,1220,20,1300,24,1336);
  Highlight(72,1180,78,1220,78,1300,72,1345);
  // Rear panel and wing get the same uneven, wire-following sketch emphasis.
  using(var wire=new Bitmap(wirePath)){
   Reinforce(wire,0,700,390,1770,1.85f); // front lamps and duct surrounds
   Reinforce(wire,330,965,970,1565,1.6f); // hood edges beside lamp pockets
   Reinforce(wire,970,0,2048,420,2.0f); // rear body, lamps and bumper
   Reinforce(wire,1080,1000,1540,1520,1.8f); // roof and rear deck
   Reinforce(wire,0,0,640,460,1.7f); // wing and end plates
   WindowEdge(wire);
   OffsetWire(wire,380,720,590,940,9,2.5f,false); // passenger headlight pocket
   OffsetWire(wire,380,1600,590,1830,9,2.5f,false); // driver headlight pocket
   OffsetWire(wire,380,720,590,940,16,1.2f);
   OffsetWire(wire,380,1600,590,1830,16,1.2f);
   OffsetWire(wire,1820,735,2020,960,10,2.5f,false); // passenger tail lamp/body return
   OffsetWire(wire,1820,1530,2020,1790,10,2.5f,false); // driver tail lamp/body return
   OffsetWire(wire,1820,735,2020,960,17,1.2f);
   OffsetWire(wire,1820,1530,2020,1790,17,1.2f);
   OffsetWire(wire,1000,15,1320,85,11,2.3f); // lamp edges on rear bumper
   OffsetWire(wire,1740,15,2010,90,11,2.3f);
   OffsetWire(wire,1430,855,1650,1005,9,2.4f); // rear quarter window curves
   OffsetWire(wire,1430,1520,1650,1690,9,2.4f);
  }
  // Strengthen the side duct lips and the separate front duct surround islands.
  // Green edges supply the geometry. Inset is selected toward the same island.
  using(var wire=new Bitmap(wirePath)){
   for(int y=0;y<N;y++)for(int x=0;x<N;x++){
    bool side=(x>1380&&x<1535&&((y>1795&&y<1960)||(y>575&&y<740)));
    bool front=(x>635&&x<1090&&y>270&&y<495);
    if(!side&&!front)continue;
    var c=wire.GetPixel(x,y);if(c.A<50||c.G<c.R+45||c.G<c.B+45)continue;
    double sx=0,sy=0;int n=0;
    // Local painted samples choose inward direction without reflecting sides.
    for(int oy=-9;oy<=9;oy+=3)for(int ox=-9;ox<=9;ox+=3){int xx=x+ox,yy=y+oy;if(xx<0||yy<0||xx>=N||yy>=N)continue;if(mask.GetPixel(xx,yy).A>128){sx+=ox;sy+=oy;n++;}}
    double len=Math.Sqrt(sx*sx+sy*sy);double dx=len>1?sx/len*4:0,dy=len>1?sy/len*4:0;
    using(var b=new SolidBrush(Color.FromArgb(240,18,24,18)))gi.FillEllipse(b,(float)(x+dx-2.4),(float)(y+dy-2.4),4.8f,4.8f);
    if(side&&len>1)using(var b=new SolidBrush(Color.FromArgb(38,0,0,0)))gs.FillEllipse(b,(float)(x+dx*2-5),(float)(y+dy*2-5),10,10);
   }
  }
  gi.Dispose();gs.Dispose();gl.Dispose();
  // The common white surface carries the original exact alpha, independent of color.
  using(var white=New())for(int y=0;y<N;y++){for(int x=0;x<N;x++){
   var a=mask.GetPixel(x,y).A;white.SetPixel(x,y,Color.FromArgb(a,255,255,255));
   bool spoiler=x>=15&&x<=95&&y>=1150&&y<=1375;
   foreach(var b in new[]{shade,light}){var c=b.GetPixel(x,y);if(c.A>0&&a==0&&!spoiler)b.SetPixel(x,y,Color.Transparent);}
  }if(y==N-1)white.Save(Path.Combine(folder,"surface-mask.png"),ImageFormat.Png);}
  ink.Save(Path.Combine(folder,"refined-contours.png"),ImageFormat.Png);shade.Save(Path.Combine(folder,"cel-shadows.png"),ImageFormat.Png);light.Save(Path.Combine(folder,"cel-highlights.png"),ImageFormat.Png);
  using(var spec=New())for(int y=0;y<N;y++){for(int x=0;x<N;x++)spec.SetPixel(x,y,Color.FromArgb(ink.GetPixel(x,y).A,0,135,0));if(y==N-1)spec.Save(Path.Combine(folder,"refined-spec.png"),ImageFormat.Png);}
  ink.Dispose();shade.Dispose();light.Dispose();mask.Dispose();return "Shared contours, neutral shadows/highlights, surface mask and supplemental spec created.";
 }
}
