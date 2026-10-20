using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

public static class ContinuityGridRenderer {
    public static readonly string[] Panels={"I122","I136","I081","I128","I129","I007","I021","I040","I041","I120","I143","I080","I153","I078","I156","I116","I009","I002","I020"};
    public static void Run(string paint,string masks,string output){
        using(var image=new Bitmap(paint)){
            foreach(string id in Panels){
                bool small=id=="I120"||id=="I143"||id=="I078"||id=="I156"||id=="I040"||id=="I041"||id=="I009"||id=="I002"||id=="I020";
                int step=small?32:64;
                int rotation=(id=="I081"||id=="I143"||id=="I156"||id=="I040"||id=="I041")?180:((id=="I122"||id=="I116")?90:0);
                using(var mask=new Bitmap(Path.Combine(masks,id+".png")))
                using(var overlay=new Bitmap(2048,2048,PixelFormat.Format32bppArgb)){
                    using(var g=Graphics.FromImage(overlay))
                    using(var font=new Font("Arial",small?9:13,FontStyle.Bold,GraphicsUnit.Pixel))
                    using(var format=new StringFormat(){Alignment=StringAlignment.Center,LineAlignment=StringAlignment.Center}){
                        g.TextRenderingHint=System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                        Color[] colors={Color.FromArgb(255,220,125),Color.FromArgb(130,225,255),Color.FromArgb(225,165,255),Color.FromArgb(240,240,240)};
                        for(int y=0;y<2048;y+=step)for(int x=0;x<2048;x+=step){
                            using(var brush=new SolidBrush(colors[(x/step+2*(y/step))%4]))g.FillRectangle(brush,x,y,step,step);
                            g.DrawRectangle(Pens.Black,x,y,step-1,step-1);
                            var state=g.Save();g.TranslateTransform(x+step/2,y+step/2);g.RotateTransform(rotation);
                            g.DrawString((x/step)+","+(y/step),font,Brushes.Black,0,0,format);g.Restore(state);
                        }
                    }
                    var rect=new Rectangle(0,0,2048,2048);
                    using(var native=mask.Clone(rect,PixelFormat.Format32bppArgb)){
                        var md=native.LockBits(rect,ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);
                        var od=overlay.LockBits(rect,ImageLockMode.ReadWrite,PixelFormat.Format32bppArgb);
                        var mb=new byte[2048*2048*4];var ob=new byte[mb.Length];
                        Marshal.Copy(md.Scan0,mb,0,mb.Length);Marshal.Copy(od.Scan0,ob,0,ob.Length);
                        for(int p=0;p<ob.Length;p+=4)ob[p+3]=mb[p+3];
                        Marshal.Copy(ob,0,od.Scan0,ob.Length);native.UnlockBits(md);overlay.UnlockBits(od);
                    }
                    using(var g=Graphics.FromImage(image))g.DrawImage(overlay,rect,0,0,2048,2048,GraphicsUnit.Pixel);
                }
            }
            image.Save(output+".png",ImageFormat.Png);
            using(var copy=image.Clone(new Rectangle(0,0,2048,2048),PixelFormat.Format32bppArgb)){
                var d=copy.LockBits(new Rectangle(0,0,2048,2048),ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);
                byte[] bgra=new byte[2048*2048*4];Marshal.Copy(d.Scan0,bgra,0,bgra.Length);copy.UnlockBits(d);
                using(var file=new BinaryWriter(File.Create(output+".tga"))){
                    byte[] header=new byte[18];header[2]=2;header[13]=8;header[15]=8;header[16]=24;header[17]=32;file.Write(header);
                    byte[] bgr=new byte[2048*2048*3];for(int p=0,q=0;p<bgra.Length;p+=4){bgr[q++]=bgra[p];bgr[q++]=bgra[p+1];bgr[q++]=bgra[p+2];}file.Write(bgr);
                }
            }
        }
    }
}
