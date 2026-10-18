using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

// r006: screenshot-mapped linework. B is the driver side as laid out in UV;
// A uses the same longitudinal direction but reverses the vertical UV axis.
public static class RenderMappedCurrent
{
    const int Size=2048;
    static readonly Color Ink=Color.FromArgb(235,20,29,18);
    static readonly Color Echo=Color.FromArgb(140,20,29,18);
    static readonly Color Shade=Color.FromArgb(145,79,129,0);

    public static void Run(string maskFile,string wingFile,string outputFolder)
    {
        using(var mask=new Bitmap(maskFile))
        using(var wing=new Bitmap(wingFile))
        using(var ink=new Bitmap(Size,Size,PixelFormat.Format32bppArgb))
        using(var shade=new Bitmap(Size,Size,PixelFormat.Format32bppArgb))
        using(var spec=new Bitmap(Size,Size,PixelFormat.Format32bppArgb))
        {
            using(var g=Graphics.FromImage(ink))
            {
                g.SmoothingMode=SmoothingMode.AntiAlias;
                DrawSideInk(g,false);
                DrawSideInk(g,true);
                DrawHoodInk(g);
            }
            using(var g=Graphics.FromImage(shade))
            {
                g.SmoothingMode=SmoothingMode.AntiAlias;
                // Reserve the shade layer for facets confirmed in the viewer.
            }
            Clip(ink,mask);Clip(shade,mask);
            BuildSpec(ink,wing,spec);
            ink.Save(System.IO.Path.Combine(outputFolder,"ink-current.png"),ImageFormat.Png);
            shade.Save(System.IO.Path.Combine(outputFolder,"shade-current.png"),ImageFormat.Png);
            spec.Save(System.IO.Path.Combine(outputFolder,"spec-current.png"),ImageFormat.Png);
        }
    }

    static void DrawSideInk(Graphics g,bool passenger)
    {
        var state=g.Save();
        if(passenger)g.Transform=new Matrix(1,0,0,-1,0,2560);
        using(var main=new Pen(Ink,4.3f))
        using(var fine=new Pen(Echo,2.2f))
        {
            main.StartCap=LineCap.Round;main.EndCap=LineCap.Round;
            fine.StartCap=LineCap.Round;fine.EndCap=LineCap.Round;
            // No vertical door strokes: they do not describe a visible body
            // break and are obscured by iRacing's number. The rocker crease
            // is below the number block and stops before the intake island.
            Stroke(g,main,1120,1982, 1175,1983, 1240,1984, 1286,1980);
            Stroke(g,fine,1162,1971, 1205,1974, 1250,1975, 1274,1973);
        }
        g.Restore(state);
    }

    static void DrawHoodInk(Graphics g)
    {
        using(var main=new Pen(Ink,5.2f))
        using(var fine=new Pen(Echo,2.0f))
        {
            main.StartCap=LineCap.Round;main.EndCap=LineCap.Round;
            fine.StartCap=LineCap.Round;fine.EndCap=LineCap.Round;
            // Main ink follows the green hood-panel perimeter. The front
            // curve is a real seam; short offset marks supply the sketch feel.
            Stroke(g,main,399,1144, 459,1078, 705,1017, 898,985);
            Stroke(g,main,397,1148, 382,1190, 378,1298, 397,1352);
            Stroke(g,fine,428,1137, 472,1102, 532,1081, 594,1062);
            Stroke(g,fine,737,1023, 782,1008, 832,996, 866,992);
            var state=g.Save();
            g.Transform=new Matrix(1,0,0,-1,0,2500);
            Stroke(g,main,399,1144, 459,1078, 705,1017, 898,985);
            Stroke(g,fine,428,1137, 472,1102, 532,1081, 594,1062);
            Stroke(g,fine,737,1023, 782,1008, 832,996, 866,992);
            g.Restore(state);
        }
    }

    static void Stroke(Graphics g,Pen pen,params float[] c)
    {
        var p=new PointF[33];
        double phase=(c[0]*.021+c[1]*.013)%(Math.PI*2);
        for(int i=0;i<p.Length;i++)
        {
            double t=i/32.0,u=1-t;
            double x=u*u*u*c[0]+3*u*u*t*c[2]+3*u*t*t*c[4]+t*t*t*c[6];
            double y=u*u*u*c[1]+3*u*u*t*c[3]+3*u*t*t*c[5]+t*t*t*c[7];
            double dx=3*u*u*(c[2]-c[0])+6*u*t*(c[4]-c[2])+3*t*t*(c[6]-c[4]);
            double dy=3*u*u*(c[3]-c[1])+6*u*t*(c[5]-c[3])+3*t*t*(c[7]-c[5]);
            double n=Math.Sqrt(dx*dx+dy*dy);
            double wobble=Math.Sin(Math.PI*t)*(Math.Sin(17*t+phase)*1.15+Math.Sin(39*t+phase*.7)*.35);
            if(n>.01){x-=dy/n*wobble;y+=dx/n*wobble;}
            p[i]=new PointF((float)x,(float)y);
        }
        g.DrawLines(pen,p);
    }
    static byte[] Read(Bitmap b)
    {
        var r=new Rectangle(0,0,Size,Size);
        using(var c=b.Clone(r,PixelFormat.Format32bppArgb))
        {
            var d=c.LockBits(r,ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);
            var a=new byte[d.Stride*Size];Marshal.Copy(d.Scan0,a,0,a.Length);c.UnlockBits(d);return a;
        }
    }
    static void Write(Bitmap b,byte[] a)
    {
        var r=new Rectangle(0,0,Size,Size);
        var d=b.LockBits(r,ImageLockMode.WriteOnly,PixelFormat.Format32bppArgb);
        Marshal.Copy(a,0,d.Scan0,a.Length);b.UnlockBits(d);
    }
    static void Clip(Bitmap art,Bitmap mask)
    {
        var a=Read(art);var m=Read(mask);
        for(int p=0;p<a.Length;p+=4)
        {
            int blue=Math.Min(m[p]-m[p+2]-8,m[p]-m[p+1]-5);
            int alpha=(blue<=0||m[p]<65||m[p+1]<70)?0:Math.Min(255,blue*6);
            a[p+3]=(byte)(a[p+3]*alpha/255);
        }
        Write(art,a);
    }
    static void BuildSpec(Bitmap ink,Bitmap wing,Bitmap spec)
    {
        var a=Read(ink);var w=Read(wing);var s=new byte[a.Length];
        for(int p=0;p<s.Length;p+=4)
        {
            s[p]=0;s[p+1]=135;s[p+2]=0;
            s[p+3]=Math.Max(a[p+3],w[p+3]);
        }
        Write(spec,s);
    }
}
