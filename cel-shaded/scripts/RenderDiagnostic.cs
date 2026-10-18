using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

// Temporary coordinate paint for screenshot-based UV orientation checks.
// This does not belong in the finished livery or revision exports.
public static class RenderDiagnostic
{
    const int Size=2048;
    public static void Run(string maskFile,string outputFile)
    {
        using(var mask=new Bitmap(maskFile))
        using(var art=new Bitmap(Size,Size,PixelFormat.Format32bppArgb))
        {
            using(var g=Graphics.FromImage(art))
            {
                g.SmoothingMode=SmoothingMode.AntiAlias;
                g.TextRenderingHint=System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                FullGrid(g);
            }
            Clip(art,mask);
            art.Save(outputFile,ImageFormat.Png);
        }
    }
    static void FullGrid(Graphics g)
    {
        var quadrants=new[] {
            Color.FromArgb(255,175,113), // top-left: wing, bumpers, small fittings
            Color.FromArgb(236,145,225), // top-right: side and rear fittings
            Color.FromArgb(255,225,104), // bottom-left: hood and front body
            Color.FromArgb(101,219,240)  // bottom-right: roof, rear deck, side
        };
        using(var major=new Pen(Color.FromArgb(220,20,25,25),2f))
        using(var minor=new Pen(Color.FromArgb(90,20,25,25),1f))
        using(var font=new Font("Arial",22f,FontStyle.Bold,GraphicsUnit.Pixel))
        using(var ink=new SolidBrush(Color.FromArgb(255,13,21,23)))
        {
            for(int row=0;row<16;row++)for(int col=0;col<16;col++)
            {
                int x=col*128,y=row*128;
                int q=(row>=8?2:0)+(col>=8?1:0);
                var baseColor=quadrants[q];
                int delta=((row+col)%2==0)?0:-23;
                using(var fill=new SolidBrush(Color.FromArgb(194,
                    Math.Max(0,baseColor.R+delta),
                    Math.Max(0,baseColor.G+delta),
                    Math.Max(0,baseColor.B+delta))))
                    g.FillRectangle(fill,x,y,128,128);
                g.DrawRectangle(major,x,y,128,128);
                g.DrawLine(minor,x+64,y,x+64,y+128);
                g.DrawLine(minor,x,y+64,x+128,y+64);
                g.DrawString(col+","+row,font,ink,x+6,y+6);
            }
        }
    }
    static byte[] Read(Bitmap b)
    {
        var r=new Rectangle(0,0,Size,Size);
        var d=b.LockBits(r,ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);
        var a=new byte[d.Stride*Size];Marshal.Copy(d.Scan0,a,0,a.Length);b.UnlockBits(d);return a;
    }
    static void Write(Bitmap b,byte[] a)
    {
        var r=new Rectangle(0,0,Size,Size);
        var d=b.LockBits(r,ImageLockMode.WriteOnly,PixelFormat.Format32bppArgb);
        Marshal.Copy(a,0,d.Scan0,a.Length);b.UnlockBits(d);
    }
    static void Clip(Bitmap art,Bitmap mask)
    {
        using(var copy=mask.Clone(new Rectangle(0,0,Size,Size),PixelFormat.Format32bppArgb))
        {
            var a=Read(art);var m=Read(copy);
            for(int p=0;p<a.Length;p+=4)
            {
                int blue=Math.Min(m[p]-m[p+2]-8,m[p]-m[p+1]-5);
                int alpha=(blue<=0||m[p]<65||m[p+1]<70)?0:Math.Min(255,blue*6);
                a[p+3]=(byte)(a[p+3]*alpha/255);
            }
            Write(art,a);
        }
    }
}
