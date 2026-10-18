using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

// Separates the current combined artwork by color and UV location so the
// Photoshop source exposes independently editable shade, ink, and wing layers.
public static class SplitCurrentArt
{
    public static void Run(string sourceFile, string outputFolder)
    {
        using(var source=new Bitmap(sourceFile))
        using(var copy=source.Clone(new Rectangle(0,0,source.Width,source.Height),PixelFormat.Format32bppArgb))
        using(var ink=new Bitmap(source.Width,source.Height,PixelFormat.Format32bppArgb))
        using(var shade=new Bitmap(source.Width,source.Height,PixelFormat.Format32bppArgb))
        using(var wing=new Bitmap(source.Width,source.Height,PixelFormat.Format32bppArgb))
        {
            int w=source.Width,h=source.Height;
            var src=Read(copy);
            var aInk=new byte[src.Length];
            var aShade=new byte[src.Length];
            var aWing=new byte[src.Length];
            for(int y=0;y<h;y++)for(int x=0;x<w;x++)
            {
                int p=(y*w+x)*4;
                if(src[p+3]==0)continue;
                byte[] dest;
                if(x<635&&y<450) dest=aWing;
                else if(src[p+1]>src[p+2]+15 && src[p+1]>src[p]+10) dest=aShade;
                else dest=aInk;
                dest[p]=src[p];dest[p+1]=src[p+1];dest[p+2]=src[p+2];dest[p+3]=src[p+3];
            }
            Write(ink,aInk);Write(shade,aShade);Write(wing,aWing);
            ink.Save(System.IO.Path.Combine(outputFolder,"ink-current.png"),ImageFormat.Png);
            shade.Save(System.IO.Path.Combine(outputFolder,"shade-current.png"),ImageFormat.Png);
            wing.Save(System.IO.Path.Combine(outputFolder,"wing-current.png"),ImageFormat.Png);
        }
    }
    static byte[] Read(Bitmap b)
    {
        var r=new Rectangle(0,0,b.Width,b.Height);
        var d=b.LockBits(r,ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);
        var a=new byte[d.Stride*b.Height];Marshal.Copy(d.Scan0,a,0,a.Length);b.UnlockBits(d);return a;
    }
    static void Write(Bitmap b,byte[] a)
    {
        var r=new Rectangle(0,0,b.Width,b.Height);
        var d=b.LockBits(r,ImageLockMode.WriteOnly,PixelFormat.Format32bppArgb);
        Marshal.Copy(a,0,d.Scan0,a.Length);b.UnlockBits(d);
    }
}
