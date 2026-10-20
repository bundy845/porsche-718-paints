using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

public static class SeamCheckRenderer {
    public static void Run(string paint,string masks,string output){
        using(var image=new Bitmap(paint)){
            string[] ids={"I122","I136","I081"};
            Color[] colors={Color.Cyan,Color.Orange,Color.Magenta,Color.White,Color.Red};
            foreach(string id in ids)using(var mask=new Bitmap(Path.Combine(masks,id+".png"))){
                for(int number=1;number<=5;number++){
                    int center=640+(number-1)*64,bestStart=0,bestLength=0,start=-1;
                    for(int y=0;y<2048;y++){
                        bool on=mask.GetPixel(center,y).A>200;
                        if(on&&start<0)start=y;
                        if((!on||y==2047)&&start>=0){int length=y-start;if(length>bestLength){bestStart=start;bestLength=length;}start=-1;}
                        for(int x=center-3;x<=center+3;x++)if(mask.GetPixel(x,y).A>200)image.SetPixel(x,y,colors[number-1]);
                    }
                    if(bestLength<20)continue;
                    using(var g=Graphics.FromImage(image))using(var font=new Font("Arial",22,FontStyle.Bold)){
                        g.TranslateTransform(center+9,bestStart+bestLength/2);
                        if(id=="I122")g.RotateTransform(90);else if(id=="I081")g.RotateTransform(180);
                        string text=number.ToString();g.DrawString(text,font,Brushes.Black,1,1);g.DrawString(text,font,new SolidBrush(colors[number-1]),0,0);
                    }
                }
            }
            image.Save(output+".png",ImageFormat.Png);
            using(var copy=image.Clone(new Rectangle(0,0,2048,2048),PixelFormat.Format32bppArgb)){
                var d=copy.LockBits(new Rectangle(0,0,2048,2048),ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);
                byte[] rgba=new byte[2048*2048*4];Marshal.Copy(d.Scan0,rgba,0,rgba.Length);copy.UnlockBits(d);
                using(var file=new BinaryWriter(File.Create(output+".tga"))){byte[] header=new byte[18];header[2]=2;header[13]=8;header[15]=8;header[16]=24;header[17]=32;file.Write(header);for(int p=0;p<rgba.Length;p+=4){file.Write(rgba[p]);file.Write(rgba[p+1]);file.Write(rgba[p+2]);}}
            }
        }
    }
}
