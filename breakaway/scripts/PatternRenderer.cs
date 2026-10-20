using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

public sealed class HexPatternRenderer : IDisposable {
    const int N=2048;
    readonly Dictionary<string,byte[]> panels=new Dictionary<string,byte[]>();
    readonly Dictionary<string,Rectangle> panelBounds=new Dictionary<string,Rectangle>();
    readonly Dictionary<string,Bitmap> roles=new Dictionary<string,Bitmap>();
    readonly byte[] native;
    readonly byte[] mandatory;
    readonly string output;
    public readonly List<string> Report=new List<string>();
    public HexPatternRenderer(string clean,string protect,string destination) {
        output=destination; Directory.CreateDirectory(output);
        using(var b=new Bitmap(clean))native=Read(b);
        using(var b=new Bitmap(protect))mandatory=Read(b);
        foreach(var r in new[]{"base","ink","primary","accent","metal-primary","metal-accent"})roles[r]=new Bitmap(N,N,PixelFormat.Format32bppArgb);
    }
    static byte[] Read(Bitmap b) {
        using(var copy=b.Clone(new Rectangle(0,0,N,N),PixelFormat.Format32bppArgb)) {
            var d=copy.LockBits(new Rectangle(0,0,N,N),ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);
            var v=new byte[N*N*4];Marshal.Copy(d.Scan0,v,0,v.Length);copy.UnlockBits(d);return v;
        }
    }
    static void Write(Bitmap b,byte[] bytes) {
        var d=b.LockBits(new Rectangle(0,0,N,N),ImageLockMode.WriteOnly,PixelFormat.Format32bppArgb);
        Marshal.Copy(bytes,0,d.Scan0,bytes.Length);b.UnlockBits(d);
    }
    bool Blue(int index) {
        int p=index*4;return native[p]>native[p+2]+35 && native[p+1]>native[p+2]+25 && native[p]>native[p+1]+20;
    }
    public void AddPanel(string id,int x0,int y0,int x1,int y1) {
        // Largest connected native blue region inside the confirmed locator box.
        // Never derive a final boundary from the reference JPEG segmentation.
        var visited=new bool[N*N];var largest=new List<int>();
        for(int y=y0;y<y1;y++)for(int x=x0;x<x1;x++) {
            int first=y*N+x;if(visited[first]||!Blue(first))continue;
            var found=new List<int>();var q=new Queue<int>();q.Enqueue(first);visited[first]=true;
            while(q.Count>0) {
                int p=q.Dequeue();found.Add(p);int px=p%N,py=p/N;
                foreach(int z in new[]{p-1,p+1,p-N,p+N}) {
                    int zx=z%N,zy=z/N;
                    if(z<0||z>=N*N||zx<x0||zx>=x1||zy<y0||zy>=y1||Math.Abs(zx-px)+Math.Abs(zy-py)!=1)continue;
                    if(!visited[z]&&Blue(z)){visited[z]=true;q.Enqueue(z);}
                }
            }
            if(found.Count>largest.Count)largest=found;
        }
        if(largest.Count<100)throw new Exception("No useful native component: "+id);
        var mask=new byte[N*N];int count=0;
        foreach(int p in largest) { if(mandatory[p*4+3]<250){mask[p]=255;count++;} }
        panels[id]=mask;panelBounds[id]=Rectangle.FromLTRB(x0,y0,x1,y1);Report.Add(id+": "+count+" protected native-blue pixels");
        PaintMask("base",mask);
    }
    public void ResolveOwnership() {
        var ids=new List<string>(panels.Keys);
        ids.Sort((a,b)=>(panelBounds[a].Width*panelBounds[a].Height).CompareTo(panelBounds[b].Width*panelBounds[b].Height));
        var claimed=new bool[N*N];int removed=0;
        foreach(var id in ids){var m=panels[id];for(int p=0;p<m.Length;p++)if(m[p]>0){if(claimed[p]){m[p]=0;removed++;}else claimed[p]=true;}}
        Report.Add("Exclusive ownership: removed "+removed+" overlapping claims; smaller confirmed native components take precedence.");
    }
    public void MappedTexture(string panel,string textureFile,double[] transform) {
        var mask=panels[panel];var bytes=Read(roles["accent"]);var bounds=panelBounds[panel];
        using(var texture=new Bitmap(textureFile)){
            for(int y=bounds.Top;y<bounds.Bottom;y++)for(int x=bounds.Left;x<bounds.Right;x++){
                int index=y*N+x;if(mask[index]==0)continue;
                double nx=(x-bounds.Left)/Math.Max(1.0,bounds.Width-1),ny=(y-bounds.Top)/Math.Max(1.0,bounds.Height-1);
                double u=transform[0]*nx+transform[1]*ny+transform[2],v=transform[3]*nx+transform[4]*ny+transform[5];
                int sx=Math.Max(0,Math.Min(texture.Width-1,(int)(u*(texture.Width-1))));
                int sy=Math.Max(0,Math.Min(texture.Height-1,(int)(v*(texture.Height-1))));
                var color=texture.GetPixel(sx,sy);int p=index*4;
                bytes[p]=255;bytes[p+1]=255;bytes[p+2]=255;bytes[p+3]=(byte)((color.R*77+color.G*150+color.B*29)>>8);
            }
        }Write(roles["accent"],bytes);
    }
    public void PlainPrimary(string panel){
        var m=panels[panel];foreach(var role in new[]{"ink","accent"}){var bytes=Read(roles[role]);for(int i=0;i<m.Length;i++)if(m[i]>0)bytes[i*4+3]=0;Write(roles[role],bytes);}
    }
    readonly Dictionary<string,int[][]> sectionEdges=new Dictionary<string,int[][]>();
    int[][] Edges(string panel){
        if(sectionEdges.ContainsKey(panel))return sectionEdges[panel];
        var edges=new[]{new int[N],new int[N]};var m=panels[panel];
        for(int x=0;x<N;x++){edges[0][x]=-1;edges[1][x]=-1;for(int y=0;y<N;y++)if(m[y*N+x]>0){if(edges[0][x]<0)edges[0][x]=y;edges[1][x]=y;}}
        sectionEdges[panel]=edges;return edges;
    }
    double SurfaceV(string panel,double x,double y){
        var edge=Edges(panel);int ix=Math.Max(0,Math.Min(N-1,(int)Math.Round(x)));
        if(edge[0][ix]<0)throw new Exception("Missing registered surface column: "+panel+" "+ix);
        if(panel=="I136"||panel=="I081"){
            bool driver=panel=="I136";double baseV=driver?0.65+0.35*(y-1508)/524:0.35*(y-507)/524;
            int top=driver?edge[0][ix]:edge[1][ix];double distance=driver?y-top:top-y;
            double target=driver?0.65:0.35;
            if(distance<=0)return target;
            double oldTop=driver?0.65+0.35*(top-1508)/524:0.35*(top-507)/524;
            return baseV+(target-oldTop)*Math.Max(0,1-distance/96.0);
        }
        if(y<=edge[0][ix])return 0.35;if(y>=edge[1][ix])return 0.65;
        return 0.35+0.30*(y-edge[0][ix])/Math.Max(1,edge[1][ix]-edge[0][ix]);
    }
    // Existing G01/G02/G03/G05/G06 grids establish common fore/aft native x.
    // Boundary v is shared across adjacent large panels instead of independently
    // fitting their bounding boxes. Geometry is still provisional at pixel scale.
    public void RegisteredField(string panel,string textureFile){LinkedField(panel,panel,textureFile,null);}
    public void LinkedField(string panel,string body,string textureFile,double[] nativeTransform,bool smooth=false){
        var mask=panels[panel];var bytes=Read(roles["accent"]);var bounds=panelBounds[panel];
        using(var texture=new Bitmap(textureFile)){
            for(int y=bounds.Top;y<bounds.Bottom;y++)for(int x=bounds.Left;x<bounds.Right;x++){
                int index=y*N+x;if(mask[index]==0)continue;
                double bx=x,by=y;
                if(nativeTransform!=null){double nx=(x-bounds.Left)/(double)(bounds.Width-1),ny=(y-bounds.Top)/(double)(bounds.Height-1);bx=nativeTransform[0]*nx+nativeTransform[1]*ny+nativeTransform[2];by=nativeTransform[3]*nx+nativeTransform[4]*ny+nativeTransform[5];}
                double u=0.18+0.72*(bx-400)/1600.0,v=SurfaceV(body,bx,by);
                int sx=Math.Max(0,Math.Min(texture.Width-1,(int)(u*(texture.Width-1))));
                int sy=Math.Max(0,Math.Min(texture.Height-1,(int)(v*(texture.Height-1))));
                var color=texture.GetPixel(sx,sy);int p=index*4;
                double gray=(color.R*77+color.G*150+color.B*29)/256.0;
                if(smooth){
                    double fx=Math.Max(0,Math.Min(texture.Width-1,u*(texture.Width-1))),fy=Math.Max(0,Math.Min(texture.Height-1,v*(texture.Height-1)));
                    int x0=(int)fx,y0=(int)fy,x1=Math.Min(texture.Width-1,x0+1),y1=Math.Min(texture.Height-1,y0+1);double dx=fx-x0,dy=fy-y0;
                    var c00=texture.GetPixel(x0,y0);var c10=texture.GetPixel(x1,y0);var c01=texture.GetPixel(x0,y1);var c11=texture.GetPixel(x1,y1);
                    gray=((c00.R*77+c00.G*150+c00.B*29)*(1-dx)*(1-dy)+(c10.R*77+c10.G*150+c10.B*29)*dx*(1-dy)+(c01.R*77+c01.G*150+c01.B*29)*(1-dx)*dy+(c11.R*77+c11.G*150+c11.B*29)*dx*dy)/256.0;
                }
                bytes[p]=255;bytes[p+1]=255;bytes[p+2]=255;bytes[p+3]=(byte)gray;
            }
        }Write(roles["accent"],bytes);
    }
    public void LoadPaintMasks(string folder){foreach(string role in new[]{"base","ink","accent"})using(var b=new Bitmap(Path.Combine(folder,role+"-mask.png")))Write(roles[role],Read(b));}
    static double Clamp01(double v){return Math.Max(0,Math.Min(1,v));}
    public static double TailU(double x){return x<=1800?0.18+0.72*(x-400)/1600.0:0.81+(x-1800)*(0.86-0.81)/238.0;}
    static double Smooth(double t){t=Clamp01(t);return t*t*(3-2*t);}
    static double Mix(double a,double b,double t){return a+(b-a)*t;}
    // Reference-derived zones: G01/G03 side returns, G04 rear columns 10..13,
    // G05 deck span. Exact physical point correspondence remains provisional.
    public double[] RearUV(string panel,double x,double y){
        if(panel=="I129"||panel=="I136"||panel=="I081")return new[]{TailU(x),SurfaceV(panel,x,y)};
        var b=panelBounds[panel];double nx=(x-b.Left)/(b.Width-1),ny=(y-b.Top)/(b.Height-1);
        if(panel=="I040")return new[]{0.86+0.004*(1-ny),0.35+0.30*nx};
        if(panel=="I041")return new[]{0.864+0.002*(1-ny),0.35+0.30*nx};
        if(panel=="I021"){
            // Its separate atlas is fitted into the central shell's inset region;
            // D07/D08 establish adjacency/orientation, not exact pixel pairs.
            double virtualX=1344+320*nx,virtualY=88+193*ny;
            return new[]{0.866+0.065*(virtualY-12)/269.0,0.65-0.30*(virtualX-1280)/512.0};
        }
        if(panel!="I007")throw new Exception("Not a rear wrap panel: "+panel);
        var edge=Edges("I007");int ix=Math.Max(978,Math.Min(2041,(int)Math.Round(x)));
        double top=edge[0][ix]>=0?edge[0][ix]:12;
        double depth=Clamp01((y-top)/Math.Max(1,281-top));
        if(x>=1280&&x<=1792)return new[]{0.866+0.065*depth,0.65-0.30*(x-1280)/512.0};
        bool driver=x<1280;double t=driver?(x-978)/302.0:(2041-x)/249.0;t=Clamp01(t);
        double turn=Smooth(t),bodyX=1856+182*t;
        string body=driver?"I136":"I081";int sampleX=(int)Math.Min(2000,bodyX);
        var side=Edges(body);int sideY=driver?side[1][sampleX]:side[0][sampleX];
        double sideV=SurfaceV(body,sampleX,sideY)+(driver?0.22:-0.22)*depth;
        double v=Mix(sideV,driver?0.65:0.35,turn);
        double u=TailU(bodyX)+(0.006+0.065*depth)*turn;
        return new[]{u,v};
    }
    public void RearContinuation(string textureFile){
        var bytes=Read(roles["accent"]);
        using(var texture=new Bitmap(textureFile))foreach(string panel in new[]{"I136","I081","I129","I040","I041","I007","I021"}){
            var m=panels[panel];var b=panelBounds[panel];
            for(int y=b.Top;y<b.Bottom;y++)for(int x=b.Left;x<b.Right;x++){
                int i=y*N+x;if(m[i]==0)continue;
                if((panel=="I136"||panel=="I081"||panel=="I129")&&x<1800)continue;
                var uv=RearUV(panel,x,y);
                int sx=(int)(Clamp01(uv[0])*(texture.Width-1)),sy=(int)(Clamp01(uv[1])*(texture.Height-1));
                var c=texture.GetPixel(sx,sy);int p=i*4;
                bytes[p]=bytes[p+1]=bytes[p+2]=255;bytes[p+3]=(byte)((c.R*77+c.G*150+c.B*29)>>8);
            }
        }
        Write(roles["accent"],bytes);
        Report.Add("Rear continuation: common tail field on aft quarter/deck, across both deck lips, then a smooth side-return to rear-face turn. Reference-based fit; physical seam acceptance pending.");
    }
    public double[] RearReturnUV(double x,double y){
        var rear=RearUV("I007",x,y);
        if(x>=1344&&x<=1728)return rear;
        bool driver=x<1344;
        // G01/G03 show side-return rows following side height. Keep these
        // fore/aft rather than blending a radial field beside the wheel.
        double bodyX=driver?x+768:3840-x;
        double bodyY=driver?1792+0.8*y:768-0.8*y;
        double sideV=driver?0.65+0.35*(bodyY-1508)/524.0:0.35*(bodyY-507)/524.0;
        double turn=driver?Smooth((x-1216)/128.0):Smooth((1856-x)/128.0);
        return new[]{Mix(TailU(bodyX),rear[0],turn),Mix(sideV,rear[1],turn)};
    }
    public void RearReturnsOnly(string textureFile){
        var bytes=Read(roles["accent"]);var m=panels["I007"];var b=panelBounds["I007"];
        using(var texture=new Bitmap(textureFile))for(int y=b.Top;y<b.Bottom;y++)for(int x=b.Left;x<b.Right;x++){
            int i=y*N+x;if(m[i]==0||(x>=1344&&x<=1728))continue;
            var uv=RearReturnUV(x,y);int sx=(int)(Clamp01(uv[0])*(texture.Width-1)),sy=(int)(Clamp01(uv[1])*(texture.Height-1));
            var c=texture.GetPixel(sx,sy);int p=i*4;bytes[p]=bytes[p+1]=bytes[p+2]=255;bytes[p+3]=(byte)((c.R*77+c.G*150+c.B*29)>>8);
        }
        Write(roles["accent"],bytes);Report.Add("r008: side-return field stays fore/aft through driver x1216 and passenger x1856; turn confined farther around rear. Only I007 side-return paint/spec masks changed.");
    }
    public void NativeBasecoat(){
        var bytes=Read(roles["base"]);int added=0;
        for(int i=0;i<N*N;i++)if(Blue(i)&&mandatory[i*4+3]<250&&bytes[i*4+3]==0){int p=i*4;bytes[p]=bytes[p+1]=bytes[p+2]=bytes[p+3]=255;added++;}
        Write(roles["base"],bytes);Report.Add("Native primary basecoat fallback: "+added+" additional unprotected blue texels; no new detailed part mapping.");
    }
    public void ResolveFrontRear(string frontReference,string rearReference,string nativeWire){
        byte[] front,rear,wire;using(var b=new Bitmap(frontReference))front=Read(b);using(var b=new Bitmap(rearReference))rear=Read(b);using(var b=new Bitmap(nativeWire))wire=Read(b);
        const int x0=978,y0=58,x1=1036,y1=169;var labels=new byte[N*N];var barrier=new bool[N*N];var queue=new Queue<int>();
        for(int y=y0;y<y1;y++)for(int x=x0;x<x1;x++){
            int i=y*N+x;bool green=false;
            for(int yy=Math.Max(0,y-1);yy<=Math.Min(N-1,y+1);yy++)for(int xx=Math.Max(0,x-1);xx<=Math.Min(N-1,x+1);xx++){
                int p=(yy*N+xx)*4;if(wire[p+3]>40&&wire[p+1]>wire[p]+30&&wire[p+1]>wire[p+2]+30)green=true;
            }
            barrier[i]=green||!Blue(i)||mandatory[i*4+3]>=250;
            if(barrier[i])continue;
            bool f=true,r=true;
            for(int yy=y-2;yy<=y+2;yy++)for(int xx=x-2;xx<=x+2;xx++){int p=(yy*N+xx)*4+3;f&=front[p]>200;r&=rear[p]>200;}
            if(f!=r){labels[i]=(byte)(f?1:2);queue.Enqueue(i);}
        }
        while(queue.Count>0){int i=queue.Dequeue(),x=i%N,y=i/N;foreach(int z in new[]{i-1,i+1,i-N,i+N}){int xx=z%N,yy=z/N;if(xx<x0||xx>=x1||yy<y0||yy>=y1||Math.Abs(xx-x)+Math.Abs(yy-y)!=1||barrier[z]||labels[z]!=0)continue;labels[z]=labels[i];queue.Enqueue(z);}}
        int changed=0;
        for(int y=y0;y<y1;y++)for(int x=x0;x<x1;x++){
            int i=y*N+x;if(!Blue(i)||mandatory[i*4+3]>=250)continue;
            byte label=labels[i];if(label==0){int best=999;for(int dy=-4;dy<=4;dy++)for(int dx=-4;dx<=4;dx++){int xx=x+dx,yy=y+dy;if(xx<x0||xx>=x1||yy<y0||yy>=y1)continue;int z=yy*N+xx,d=dx*dx+dy*dy;if(labels[z]>0&&d<best){best=d;label=labels[z];}}}
            if(label==2&&panels["I009"][i]>0){panels["I009"][i]=0;panels["I007"][i]=255;changed++;}
        }
        Report.Add("Front/rear overlap: "+changed+" rear texels reclaimed using eroded reference cores and native green-wire barriers; boundary still subject to physical review.");
    }
    public void FillPanel(string panel,string role){PaintMask(role,panels[panel]);}
    public void Texture(string panel,string textureFile,int x0,int y0,int x1,int y1,double u0,double u1,bool flipY) {
        var mask=panels[panel];var bytes=Read(roles["accent"]);
        using(var texture=new Bitmap(textureFile)) {
            for(int y=y0;y<y1;y++)for(int x=x0;x<x1;x++) {
                int index=y*N+x;if(mask[index]==0)continue;
                double u=u0+(u1-u0)*(x-x0)/Math.Max(1.0,x1-x0-1);
                double v=(y-y0)/Math.Max(1.0,y1-y0-1);if(flipY)v=1.0-v;
                double sampleU=Math.Max(0.0,Math.Min(1.0,(u-0.22)/0.78));
                int sx=Math.Max(0,Math.Min(texture.Width-1,(int)(sampleU*(texture.Width-1))));
                int sy=Math.Max(0,Math.Min(texture.Height-1,(int)(v*(texture.Height-1))));
                var color=texture.GetPixel(sx,sy);
                int alpha=(color.R*77+color.G*150+color.B*29)>>8;
                // Sample the source's naturally quiet front. Never chop the
                // textured transition at a vertical threshold.
                int p=index*4;bytes[p]=255;bytes[p+1]=255;bytes[p+2]=255;bytes[p+3]=(byte)alpha;
            }
        }
        Write(roles["accent"],bytes);
    }
    void PaintMask(string role,byte[] mask) {
        var bytes=Read(roles[role]);for(int i=0;i<mask.Length;i++)if(mask[i]>0){int p=i*4;bytes[p]=255;bytes[p+1]=255;bytes[p+2]=255;bytes[p+3]=mask[i];}Write(roles[role],bytes);
    }
    public void Polygon(string panel,string role,float[] xy) {
        using(var temp=new Bitmap(N,N,PixelFormat.Format32bppArgb)) {
            using(var g=Graphics.FromImage(temp)) {
                g.SmoothingMode=SmoothingMode.AntiAlias;
                var points=new PointF[xy.Length/2];for(int i=0;i<points.Length;i++)points[i]=new PointF(xy[i*2],xy[i*2+1]);
                g.FillPolygon(Brushes.White,points);
            }
            var b=Read(temp);var m=panels[panel];
            for(int i=0;i<m.Length;i++)b[i*4+3]=(byte)(b[i*4+3]*m[i]/255);
            Write(temp,b);using(var g=Graphics.FromImage(roles[role]))g.DrawImageUnscaled(temp,0,0);
        }
    }
    public void Reserve(int x0,int y0,int x1,int y1) {
        foreach(var role in roles.Keys)if(role!="base")using(var g=Graphics.FromImage(roles[role])) {
            g.CompositingMode=CompositingMode.SourceCopy;g.FillRectangle(Brushes.Transparent,x0,y0,x1-x0,y1-y0);
        }
    }
    public void Save() {
        foreach(var kv in roles)kv.Value.Save(Path.Combine(output,kv.Key+"-mask.png"),ImageFormat.Png);
        string folder=Path.Combine(output,"panel-masks");Directory.CreateDirectory(folder);
        foreach(var kv in panels){using(var b=new Bitmap(N,N,PixelFormat.Format32bppArgb)){var bytes=new byte[N*N*4];for(int i=0;i<kv.Value.Length;i++)if(kv.Value[i]>0){int p=i*4;bytes[p]=255;bytes[p+1]=255;bytes[p+2]=255;bytes[p+3]=kv.Value[i];}Write(b,bytes);b.Save(Path.Combine(folder,kv.Key+".png"),ImageFormat.Png);}}
        File.WriteAllLines(Path.Combine(output,"native-mask-report.txt"),Report);
    }
    public void Dispose(){foreach(var b in roles.Values)b.Dispose();}
}
