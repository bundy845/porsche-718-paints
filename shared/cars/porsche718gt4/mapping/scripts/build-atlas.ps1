$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing
$mappingRoot = Split-Path $PSScriptRoot -Parent
Add-Type -TypeDefinition @'
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
public static class PartsAtlas718 {
  class Region { public List<int> Pixels=new List<int>(); public int X0=2048,Y0=2048,X1=0,Y1=0; }
  public static void Run(string root) {
    const int W=2048,N=W*W;
    Directory.CreateDirectory(Path.Combine(root,"masks"));
    Directory.CreateDirectory(Path.Combine(root,"previews"));
    using(var input=new Bitmap(Path.Combine(root,"references/clyde-718-parts-2.jpg")))
    using(var src=input.Clone(new Rectangle(0,0,W,W),PixelFormat.Format32bppArgb)) {
      if(input.Width!=W || input.Height!=W) throw new Exception("Unexpected reference dimensions");
      var data=src.LockBits(new Rectangle(0,0,W,W),ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);
      var bytes=new byte[N*4]; Marshal.Copy(data.Scan0,bytes,0,bytes.Length); src.UnlockBits(data);
      var background=src.GetPixel(1,1000);
      var foreground=new bool[N];
      for(int p=0;p<N;p++) {
        int b=bytes[p*4]-background.B,g=bytes[p*4+1]-background.G,r=bytes[p*4+2]-background.R;
        foreground[p]=(r*r+g*g+b*b)>2500;
      }
      var seen=new bool[N]; var queue=new int[N]; var regions=new List<Region>(); int discarded=0,discardedPixels=0;
      for(int seed=0;seed<N;seed++) {
        if(!foreground[seed] || seen[seed]) continue;
        var region=new Region(); int head=0,tail=0; queue[tail++]=seed;seen[seed]=true;
        while(head<tail) {
          int p=queue[head++],x=p%W,y=p/W;region.Pixels.Add(p);
          region.X0=Math.Min(region.X0,x);region.X1=Math.Max(region.X1,x+1);
          region.Y0=Math.Min(region.Y0,y);region.Y1=Math.Max(region.Y1,y+1);
          int[] near={x>0?p-1:-1,x<W-1?p+1:-1,y>0?p-W:-1,y<W-1?p+W:-1};
          foreach(int q in near) if(q>=0 && foreground[q] && !seen[q]) { seen[q]=true;queue[tail++]=q; }
        }
        if(region.Pixels.Count>=32) regions.Add(region); else {discarded++;discardedPixels+=region.Pixels.Count;}
      }
      regions.Sort((a,b)=>a.Y0!=b.Y0?a.Y0.CompareTo(b.Y0):a.X0.CompareTo(b.X0));
      var csv=new List<string>{"id,x0,y0,x1,y1,pixels,mask_file"};
      using(var overview=new Bitmap(Path.Combine(root,"wire/template-wire-r000.png"))) {
        using(var g=Graphics.FromImage(overview)) using(var font=new Font("Arial",15,FontStyle.Bold,GraphicsUnit.Pixel)) {
          int i=0;
          foreach(var region in regions) {
            string id="I"+(++i).ToString("000"),file="masks/"+id+".png";
            var rgba=new byte[N*4];
            foreach(int p in region.Pixels) {rgba[p*4]=255;rgba[p*4+1]=255;rgba[p*4+2]=255;rgba[p*4+3]=255;}
            using(var mask=new Bitmap(W,W,PixelFormat.Format32bppArgb)) {
              var bits=mask.LockBits(new Rectangle(0,0,W,W),ImageLockMode.WriteOnly,PixelFormat.Format32bppArgb);
              Marshal.Copy(rgba,0,bits.Scan0,rgba.Length);mask.UnlockBits(bits);mask.Save(Path.Combine(root,file));
            }
            int labelPoint=region.Pixels[region.Pixels.Count/2],lx=labelPoint%W,ly=labelPoint/W;
            lx=Math.Min(lx,W-46);ly=Math.Min(ly,W-20);
            g.DrawRectangle(Pens.OrangeRed,region.X0,region.Y0,region.X1-region.X0,region.Y1-region.Y0);
            g.FillRectangle(Brushes.Black,lx,ly,46,20);g.DrawString(id,font,Brushes.Yellow,lx,ly);
            csv.Add(String.Join(",",id,region.X0,region.Y0,region.X1,region.Y1,region.Pixels.Count,file));
          }
        }
        overview.Save(Path.Combine(root,"overlay.png"));
      }
      for(int start=0,page=1;start<regions.Count;start+=24,page++) {
        using(var sheet=new Bitmap(1200,1440)) using(var g=Graphics.FromImage(sheet)) using(var font=new Font("Arial",13,FontStyle.Bold)) {
          g.Clear(Color.FromArgb(235,235,235));
          for(int j=0;j<24 && start+j<regions.Count;j++) {
            var r=regions[start+j];int x=(j%4)*300,y=(j/4)*240;var rect=new Rectangle(r.X0,r.Y0,r.X1-r.X0,r.Y1-r.Y0);
            float scale=Math.Min(286f/rect.Width,195f/rect.Height);
            var dest=new Rectangle(x+7,y+5,(int)(rect.Width*scale),(int)(rect.Height*scale));
            g.DrawImage(src,dest,rect,GraphicsUnit.Pixel);
            g.DrawString("I"+(start+j+1).ToString("000")+"  "+r.X0+","+r.Y0+" - "+r.X1+","+r.Y1,font,Brushes.Black,x+5,y+204);
          }
          sheet.Save(Path.Combine(root,"previews/atlas-"+page.ToString("00")+".jpg"),ImageFormat.Jpeg);
        }
      }
      File.WriteAllLines(Path.Combine(root,"islands.csv"),csv);
      File.WriteAllText(Path.Combine(root,"segmentation.json"),"{\"background_rgb\":["+background.R+","+background.G+","+background.B+"],\"rgb_distance_threshold\":50,\"minimum_component_pixels\":32,\"connectivity\":4,\"islands\":"+regions.Count+",\"discarded_components\":"+discarded+",\"discarded_pixels\":"+discardedPixels+"}");
      Console.WriteLine("Reference components: "+regions.Count+"; below 32 pixels: "+discarded+" ("+discardedPixels+" pixels). Masks are reference-derived candidates, not verified paint boundaries.");
    }
  }
}
'@ -ReferencedAssemblies System.Drawing.Common,System.Drawing.Primitives,System.Runtime.InteropServices,System.Collections,System.Console
[PartsAtlas718]::Run($mappingRoot)
