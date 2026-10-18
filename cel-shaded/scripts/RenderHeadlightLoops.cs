using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

// Trace continuous paths along the actual 718 green wire. Each side is
// sampled independently. Bumper returns follow the lower lens edge; their
// join alignment still needs car-viewer confirmation after the 2026-10-18 remap.
public static class RenderHeadlightLoops {
 const int N=2048;
 static bool[] green=new bool[N*N];
 static Bitmap mask;
 static bool[] allowed=new bool[N*N];
 static void IncludeIsland(int sx,int sy){var queue=new Queue<int>();int start=sy*N+sx;if(mask.GetPixel(sx,sy).A<128)throw new Exception("Island seed is outside paint");if(allowed[start])return;queue.Enqueue(start);allowed[start]=true;while(queue.Count>0){int p=queue.Dequeue(),x=p%N,y=p/N;foreach(int d in new[]{-1,1,-N,N}){int n=p+d,xx=n%N,yy=n/N;if(n<0||n>=N*N||Math.Abs(xx-x)+Math.Abs(yy-y)!=1||allowed[n])continue;if(mask.GetPixel(xx,yy).A>128){allowed[n]=true;queue.Enqueue(n);}}}}
 static int Snap(int x,int y){int best=-1;double d=99999;for(int yy=y-14;yy<=y+14;yy++)for(int xx=x-14;xx<=x+14;xx++){if(xx<0||yy<0||xx>=N||yy>=N||!green[yy*N+xx])continue;double v=(xx-x)*(xx-x)+(yy-y)*(yy-y);if(v<d){d=v;best=yy*N+xx;}}if(best<0)throw new Exception("No green edge near "+x+","+y);return best;}
 static List<PointF> Route(int start,int end,Rectangle box){
  var prev=new int[N*N];for(int i=0;i<prev.Length;i++)prev[i]=-2;
  var q=new Queue<int>();q.Enqueue(start);prev[start]=-1;
  while(q.Count>0&&prev[end]==-2){int p=q.Dequeue(),x=p%N,y=p/N;for(int dy=-1;dy<=1;dy++)for(int dx=-1;dx<=1;dx++){int xx=x+dx,yy=y+dy;if(!box.Contains(xx,yy))continue;int n=yy*N+xx;if(green[n]&&prev[n]==-2){prev[n]=p;q.Enqueue(n);}}}
  if(prev[end]==-2)throw new Exception("Disconnected headlight edge "+start%N+","+start/N+" -> "+end%N+","+end/N);
  var pts=new List<PointF>();for(int p=end;p!=-1;p=prev[p])pts.Add(new PointF(p%N,p/N));pts.Reverse();return pts;
 }
 static List<PointF> Trace(int[] anchors,Rectangle box){var all=new List<PointF>();for(int i=0;i<anchors.Length-2;i+=2){var part=Route(Snap(anchors[i],anchors[i+1]),Snap(anchors[i+2],anchors[i+3]),box);if(all.Count>0)part.RemoveAt(0);all.AddRange(part);}var smooth=new List<PointF>();for(int i=0;i<all.Count;i+=3){float x=0,y=0;int n=0;for(int j=Math.Max(0,i-4);j<=Math.Min(all.Count-1,i+4);j++){x+=all[j].X;y+=all[j].Y;n++;}smooth.Add(new PointF(x/n,y/n));}smooth.Add(all[all.Count-1]);return smooth;}
 static PointF Normal(List<PointF> p,int i,int sign){var a=p[Math.Max(0,i-3)];var b=p[Math.Min(p.Count-1,i+3)];double dx=b.X-a.X,dy=b.Y-a.Y,len=Math.Sqrt(dx*dx+dy*dy);return new PointF((float)(sign*dy/len),(float)(-sign*dx/len));}
 static void Loops(Graphics g,List<PointF> p,int sign,double phase,float bleed){
  for(int ring=0;ring<3;ring++){
   var pts=new List<PointF>();
   for(int i=0;i<p.Count;i++){double t=(double)i/(p.Count-1);var n=Normal(p,i,sign);double offset=6+ring*8.5+(ring==0?1:ring==1?2:3)*Math.Sin(10*t+phase+ring*.9)+.65*Math.Sin(29*t+ring);pts.Add(new PointF(p[i].X+n.X*(float)offset,p[i].Y+n.Y*(float)offset));}
   // Continue the ring strokes through the atlas endpoints, with short bleed.
   var first=pts[0];var second=pts[1];var last=pts[pts.Count-1];var before=pts[pts.Count-2];
   double d1=Math.Sqrt(Math.Pow(second.X-first.X,2)+Math.Pow(second.Y-first.Y,2)),d2=Math.Sqrt(Math.Pow(last.X-before.X,2)+Math.Pow(last.Y-before.Y,2));
   pts.Insert(0,new PointF(first.X-(float)((second.X-first.X)/d1*bleed),first.Y-(float)((second.Y-first.Y)/d1*bleed)));
   pts.Add(new PointF(last.X+(float)((last.X-before.X)/d2*bleed),last.Y+(float)((last.Y-before.Y)/d2*bleed)));
   for(int i=1;i<pts.Count;i++){double t=(double)i/(pts.Count-1);float w=(float)((ring==0?5.1:ring==1?3.6:2.5)+.55*Math.Sin(18*t+phase+ring)+.25*Math.Sin(47*t));using(var pen=new Pen(Color.FromArgb(ring==2?235:250,14,21,17),w)){pen.StartCap=pen.EndCap=LineCap.Round;g.DrawLine(pen,pts[i-1],pts[i]);}}
  }
 }
 static Bitmap New(){var b=new Bitmap(N,N,PixelFormat.Format32bppArgb);b.SetResolution(72,72);return b;}
 static void Clip(Bitmap b){for(int y=680;y<1840;y++)for(int x=270;x<640;x++){var c=b.GetPixel(x,y);if(c.A==0)continue;bool body=false;for(int dy=-2;dy<=2&&!body;dy++)for(int dx=-2;dx<=2;dx++)if(allowed[(y+dy)*N+x+dx]){body=true;break;}if(!body)b.SetPixel(x,y,Color.Transparent);}}
 public static string Run(string root){
  string dir=Path.Combine(root,"graphics","current"),diag=Path.Combine(root,"design","diagnostics");
  using(var wire=new Bitmap(Path.Combine(diag,"wire-source.png")))for(int y=0;y<N;y++)for(int x=0;x<N;x++){var c=wire.GetPixel(x,y);green[y*N+x]=c.A>50&&c.G>c.R+45&&c.G>c.B+45;}
  // White mesh intersections interrupt the rasterized green edge by a few pixels.
  // Bridge those tiny breaks so the contour is traced as a continuous path.
  var raw=(bool[])green.Clone();for(int y=690;y<1840;y++)for(int x=280;x<630;x++)if(raw[y*N+x])for(int dy=-2;dy<=2;dy++)for(int dx=-2;dx<=2;dx++)green[(y+dy)*N+x+dx]=true;
  mask=new Bitmap(Path.Combine(dir,"base-current.png"));
  IncludeIsland(630,800);IncludeIsland(630,1720);IncludeIsland(300,800);
  var paths=new List<PointF>[] {
   Trace(new[]{490,752,573,805,418,905},new Rectangle(380,720,230,230)),
   Trace(new[]{418,1634,573,1727,490,1784},new Rectangle(380,1590,230,235)),
   // The original y730..850 / y1683..1800 paths were REJECTED in-car:
   // they run toward the wheels. The grid's forward/lower-lens sections
   // lie toward row 7 / row 12, adjoining those rejected sections.
   Trace(new[]{333,844,345,930,350,999},new Rectangle(315,820,65,200)),
   Trace(new[]{350,1540,345,1600,333,1690},new Rectangle(315,1515,65,200))
  };
  var log=new List<string>();
  using(var all=New())using(var ga=Graphics.FromImage(all)){
   for(int side=0;side<2;side++)using(var layer=New()){
    using(var g=Graphics.FromImage(layer)){g.SmoothingMode=SmoothingMode.AntiAlias;Loops(g,paths[side],1,side*.8,25);Loops(g,paths[side+2],-1,side*.8+2,12);}
    Clip(layer);layer.Save(Path.Combine(dir,side==0?"headlight-loops-passenger.png":"headlight-loops-driver.png"),ImageFormat.Png);ga.DrawImage(layer,new Rectangle(0,0,N,N));
    log.Add((side==0?"Passenger":"Driver")+": fender samples="+paths[side].Count+", bumper samples="+paths[side+2].Count);
   }
   all.Save(Path.Combine(diag,"headlight-loops-only.png"),ImageFormat.Png);
   using(var spec=New()){for(int y=680;y<1840;y++)for(int x=270;x<640;x++){var c=all.GetPixel(x,y);if(c.A>0)spec.SetPixel(x,y,Color.FromArgb(c.A,0,135,0));}spec.Save(Path.Combine(dir,"headlight-loops-spec.png"),ImageFormat.Png);}
  }
  mask.Dispose();File.WriteAllLines(Path.Combine(diag,"headlight-path-report.txt"),log.ToArray());return String.Join("; ",log.ToArray());
 }
}
