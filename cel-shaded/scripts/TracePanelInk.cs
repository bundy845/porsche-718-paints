using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

// Fresh artwork: every primary seam comes from the template's green Wire
// pixels at native resolution. No mirrored side artwork or fitted seam curves.
public static class TracePanelInk
{
 const int N=2048, Count=N*N;
 static readonly string[] Names={"hood","driver","passenger","roof-deck","rear-bumper","wing","front-parts","small-parts"};
 static bool[] edges,body;
 static byte[][] layers;
 static byte[] accents;
 static int Region(int x,int y){
  if(x>=355&&x<960&&y>=965&&y<1560)return 0;
  if(x>=380&&y>=1510)return 1;
  if(x>=380&&y>=500&&y<1020)return 2;
  if(x>=1080&&y>=1020&&y<1510)return 3;
  if(x>=975&&y<400)return 4;
  if(x<635&&y<460)return 5;
  if(x<380&&y>=700)return 6;
  return 7;
 }
 static byte[] Pixels(Bitmap b){
  using(var c=b.Clone(new Rectangle(0,0,N,N),PixelFormat.Format32bppArgb)){
   var d=c.LockBits(new Rectangle(0,0,N,N),ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);
   var a=new byte[Count*4];Marshal.Copy(d.Scan0,a,0,a.Length);c.UnlockBits(d);return a;
  }
 }
 static void Save(byte[] a,string file){
  using(var b=new Bitmap(N,N,PixelFormat.Format32bppArgb)){
   b.SetResolution(72,72);
   var d=b.LockBits(new Rectangle(0,0,N,N),ImageLockMode.WriteOnly,PixelFormat.Format32bppArgb);
   Marshal.Copy(a,0,d.Scan0,a.Length);b.UnlockBits(d);b.Save(file,ImageFormat.Png);
  }
 }
 static void Dot(byte[] a,double cx,double cy,double radius,int opacity){
  int minX=Math.Max(0,(int)Math.Floor(cx-radius-1)),maxX=Math.Min(N-1,(int)Math.Ceiling(cx+radius+1));
  int minY=Math.Max(0,(int)Math.Floor(cy-radius-1)),maxY=Math.Min(N-1,(int)Math.Ceiling(cy+radius+1));
  for(int y=minY;y<=maxY;y++)for(int x=minX;x<=maxX;x++){
   double dist=Math.Sqrt((x-cx)*(x-cx)+(y-cy)*(y-cy));
   int v=(int)(Math.Max(0,Math.Min(1,radius+.65-dist))*opacity);
   int p=(y*N+x)*4;
   if(v>a[p+3]){a[p]=18;a[p+1]=24;a[p+2]=18;a[p+3]=(byte)v;}
  }
 }
 static void Line(byte[] a,double x1,double y1,double x2,double y2,double radius,int opacity){
  int steps=Math.Max(1,(int)Math.Ceiling(Math.Sqrt((x2-x1)*(x2-x1)+(y2-y1)*(y2-y1))*2));
  for(int s=0;s<=steps;s++){double t=(double)s/steps;Dot(a,x1+(x2-x1)*t,y1+(y2-y1)*t,radius,opacity);}
 }
 static bool Frame(int x,int y,out double tx,out double ty){
  double sx=0,sy=0,sxx=0,syy=0,sxy=0;int count=0;
  for(int j=Math.Max(0,y-7);j<=Math.Min(N-1,y+7);j++)for(int i=Math.Max(0,x-7);i<=Math.Min(N-1,x+7);i++)if(edges[j*N+i]){
   double dx=i-x,dy=j-y;sx+=dx;sy+=dy;sxx+=dx*dx;syy+=dy*dy;sxy+=dx*dy;count++;
  }
  tx=1;ty=0;if(count<4)return false;
  sxx-=sx*sx/count;syy-=sy*sy/count;sxy-=sx*sy/count;
  double angle=.5*Math.Atan2(2*sxy,sxx-syy);tx=Math.Cos(angle);ty=Math.Sin(angle);return true;
 }
 static bool NumberArea(int x,int y){return x>910&&x<1088&&((y>578&&y<735)||(y>1800&&y<1940));}
 static void Inward(int region,int x,int y,ref double nx,ref double ny){
  // Independent physical-region centers choose the echo's side. Primary
  // seams never move; offsets below apply only to short sketch accents.
  double ax=1200,ay=1250;
  if(region==0){ax=650;ay=1250;}
  if(region==1){ax=1250;ay=1825;}
  if(region==2){ax=1250;ay=730;}
  if(region==3){ax=1520;ay=1250;}
  if(region==4){ax=1500;ay=125;}
  if(region==5){ax=320;ay=y<220?105:340;}
  if(region==6){ax=240;ay=1250;}
  int px=(int)Math.Round(x+nx*13),py=(int)Math.Round(y+ny*13),mx=(int)Math.Round(x-nx*13),my=(int)Math.Round(y-ny*13);
  bool plus=px>=0&&px<N&&py>=0&&py<N&&body[py*N+px];
  bool minus=mx>=0&&mx<N&&my>=0&&my<N&&body[my*N+mx];
  if((!plus&&minus)||(plus==minus&&nx*(ax-x)+ny*(ay-y)<0)){nx=-nx;ny=-ny;}
 }
 static void Hatch(int px,int py){
  int best=-1;double bd=28*28;
  for(int y=Math.Max(0,py-28);y<Math.Min(N,py+29);y++)for(int x=Math.Max(0,px-28);x<Math.Min(N,px+29);x++)if(edges[y*N+x]){
   double ds=(x-px)*(x-px)+(y-py)*(y-py);if(ds<bd){bd=ds;best=y*N+x;}
  }
  if(best<0)return;int bx=best%N,by=best/N;if(NumberArea(bx,by))return;
  double tx,ty;if(!Frame(bx,by,out tx,out ty))return;
  double nx=-ty,ny=tx;Inward(Region(bx,by),bx,by,ref nx,ref ny);
  for(int k=0;k<3;k++){
   double cx=bx+tx*(k*5-5),cy=by+ty*(k*5-5),len=k==1?16:11;
   Line(accents,cx-nx*3,cy-ny*3,cx+nx*len+tx*2,cy+ny*len+ty*2,k==1?1.15:.65,k==1?228:175);
  }
 }
 public static string Run(string wireFile,string maskFile,string folder){
  byte[] wire,mask;using(var b=new Bitmap(wireFile))wire=Pixels(b);using(var b=new Bitmap(maskFile))mask=Pixels(b);
  edges=new bool[Count];body=new bool[Count];bool[] raw=new bool[Count],seen=new bool[Count];
  for(int i=0;i<Count;i++){
   int p=i*4;body[i]=mask[p]>mask[p+1]+5&&mask[p]>mask[p+2]+8&&mask[p]>65;
   raw[i]=wire[p+3]>50&&wire[p+1]>wire[p+2]+45&&wire[p+1]>wire[p]+45;
   // These islands in the rear-glass bay are texture/decal atlases, not
   // exterior body seams: radiator rectangle, crest and rear badging.
   int x=i%N,y=i/N;
   if((x>=1640&&x<=1850&&y>=1060&&y<=1460)||(x>=1890&&y>=1490&&y<=1565))raw[i]=false;
  }
  int retained=0,components=0;var queue=new int[Count];
  for(int seed=0;seed<Count;seed++)if(raw[seed]&&!seen[seed]){
   int head=0,tail=1;queue[0]=seed;seen[seed]=true;
   while(head<tail){int p=queue[head++];int x=p%N,y=p/N;
    for(int j=Math.Max(0,y-1);j<=Math.Min(N-1,y+1);j++)for(int i=Math.Max(0,x-1);i<=Math.Min(N-1,x+1);i++){
     int q=j*N+i;if(raw[q]&&!seen[q]){seen[q]=true;queue[tail++]=q;}
    }
   }
   if(tail>=28){components++;for(int k=0;k<tail;k++){edges[queue[k]]=true;retained++;}}
  }
  layers=new byte[Names.Length][];for(int k=0;k<layers.Length;k++)layers[k]=new byte[Count*4];accents=new byte[Count*4];
  for(int y=0;y<N;y++)for(int x=0;x<N;x++)if(edges[y*N+x]){
   int r=Region(x,y);double width=r==7?1.15:2.0+.4*Math.Sin(x*.027+y*.019);
   Dot(layers[r],x,y,width,245);
   // Broken echo lengths, derived from actual seam normals. Exclude number
   // footprint and the small mechanical texture islands.
   if(r==7||NumberArea(x,y)||Math.Sin(x*.033+y*.023)<.57)continue;
   double tx,ty;if(!Frame(x,y,out tx,out ty))continue;double nx=-ty,ny=tx;Inward(r,x,y,ref nx,ref ny);
   double offset=7.8+.9*Math.Sin(x*.065+y*.047);double ex=x+nx*offset,ey=y+ny*offset;
   int ix=(int)Math.Round(ex),iy=(int)Math.Round(ey);
   if(ix>0&&ix<N-1&&iy>0&&iy<N-1&&body[iy*N+ix])Dot(accents,ex,ey,.65,165);
  }
  // Independent anchors on the two sides; snapping is only to nearby green
  // wire boundaries, never to a guessed front/rear reflection.
  int[,] anchors={{396,1110},{397,1390},{1115,1058},{1445,1265},{1460,1460},{1542,1070},{1980,1430},
   {1368,1783},{1450,1900},{1655,1790},{1840,1790},{850,1940},{1370,732},{1480,660},{1655,740},{1840,740},
   {1240,210},{1740,230},{130,910},{140,1605},{95,350},{535,335}};
  for(int i=0;i<anchors.GetLength(0);i++)Hatch(anchors[i,0],anchors[i,1]);
  var basePaint=new byte[Count*4];var spec=new byte[Count*4];var preview=new byte[Count*4];
  for(int i=0;i<Count;i++){
   int p=i*4;basePaint[p]=0;basePaint[p+1]=245;basePaint[p+2]=184;basePaint[p+3]=body[i]?(byte)255:(byte)0;
   byte alpha=accents[p+3];for(int k=0;k<layers.Length;k++)alpha=Math.Max(alpha,layers[k][p+3]);
   spec[p]=0;spec[p+1]=135;spec[p+2]=0;spec[p+3]=alpha;
   // Opaque proof on green for artwork regions, dark grey in gutters.
   double a=alpha/255.0;int br=body[i]?184:55,bg=body[i]?245:55,bb=body[i]?0:55;
   preview[p]=(byte)(bb*(1-a)+18*a);preview[p+1]=(byte)(bg*(1-a)+24*a);preview[p+2]=(byte)(br*(1-a)+18*a);preview[p+3]=255;
  }
  Directory.CreateDirectory(folder);
  for(int k=0;k<layers.Length;k++)Save(layers[k],Path.Combine(folder,"seams-"+Names[k]+".png"));
  Save(accents,Path.Combine(folder,"accents-current.png"));Save(basePaint,Path.Combine(folder,"base-current.png"));
  Save(spec,Path.Combine(folder,"spec-current.png"));Save(preview,Path.Combine(folder,"trace-proof.png"));
  return "Retained "+retained+" green-edge pixels in "+components+" components; created "+layers.Length+" independent seam layers.";
 }
}
