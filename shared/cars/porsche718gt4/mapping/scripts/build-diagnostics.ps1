$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing
$mappingRoot = Split-Path $PSScriptRoot -Parent
Add-Type -TypeDefinition @'
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
public static class MappingDiagnostics718 {
  const int W=2048;
  static byte[] Read(Bitmap b) { var d=b.LockBits(new Rectangle(0,0,W,W),ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);var a=new byte[W*W*4];Marshal.Copy(d.Scan0,a,0,a.Length);b.UnlockBits(d);return a; }
  static void Write(Bitmap b,byte[] a) { var d=b.LockBits(new Rectangle(0,0,W,W),ImageLockMode.WriteOnly,PixelFormat.Format32bppArgb);Marshal.Copy(a,0,d.Scan0,a.Length);b.UnlockBits(d); }
  static void Save(Bitmap b,string path) {
    b.Save(path+".png"); var a=Read(b);
    using(var f=new BinaryWriter(File.Create(path+".tga"))) {
      byte[] h=new byte[18];h[2]=2;h[12]=0;h[13]=8;h[14]=0;h[15]=8;h[16]=24;h[17]=32;f.Write(h);
      for(int p=0;p<W*W;p++) {f.Write(a[p*4]);f.Write(a[p*4+1]);f.Write(a[p*4+2]);}
    }
  }
  public static void Remaining(string root, string activeIds, bool focus=false) {
    var rows=File.ReadAllLines(Path.Combine(root,"islands.csv"));
    using(var original=new Bitmap(Path.Combine(root,"template/base-reference.png")))
    using(var output=original.Clone(new Rectangle(0,0,W,W),PixelFormat.Format32bppArgb)) {
      var a=Read(output);
      Color[] palette={Color.FromArgb(255,220,70),Color.FromArgb(80,240,245),Color.FromArgb(255,120,210),Color.FromArgb(150,255,80),Color.FromArgb(255,145,60),Color.FromArgb(170,170,255)};
      int[] focusIds={8,10,11,12,17,48,83,84,86,121,130,137,139,141,149,151,2};
      string[] focusCodes={"A","B","C","D","E","F","G","H","J","K","L","M","N","P","Q","S","R"};
      for(int i=1;i<rows.Length;i++) {
        var f=rows[i].Split(',');int id=int.Parse(f[0].Substring(1));if(id==1)continue;
        int x0=int.Parse(f[1]),y0=int.Parse(f[2]),x1=int.Parse(f[3]),y1=int.Parse(f[4]);
        bool active=activeIds.Contains("|"+id+"|");
        using(var mask=new Bitmap(Path.Combine(root,f[6]))) using(var pat=new Bitmap(W,W,PixelFormat.Format32bppArgb)) {
          var m=Read(mask);
          // Find the widest interior location for a single readable centered ID.
          int bw=x1-x0,bh=y1-y0;var dist=new int[bw*bh];int best=0,bx=x0,by=y0;
          for(int y=0;y<bh;y++)for(int x=0;x<bw;x++) {
            int p=y*bw+x;if(m[((y+y0)*W+x+x0)*4+3]==0){dist[p]=0;continue;}
            dist[p]=1+Math.Min(x>0?dist[p-1]:0,y>0?dist[p-bw]:0);
          }
          for(int y=bh-1;y>=0;y--)for(int x=bw-1;x>=0;x--) {
            int p=y*bw+x;if(dist[p]==0)continue;
            dist[p]=Math.Min(dist[p],1+Math.Min(x<bw-1?dist[p+1]:0,y<bh-1?dist[p+bw]:0));
            if(dist[p]>best){best=dist[p];bx=x+x0;by=y+y0;}
          }
          using(var g=Graphics.FromImage(pat)) {
            g.Clear(active?palette[id%6]:Color.FromArgb(185,185,185));
            if(active) {
              string label=focus?focusCodes[Array.IndexOf(focusIds,id)]:(id==2?"2":id.ToString("000"));
              float size=Math.Max(7,Math.Min(44,best*((focus || id==2)?1.5f:.8f)));
              using(var font=new Font("Arial",size,FontStyle.Bold,GraphicsUnit.Pixel)) {
                var sz=g.MeasureString(label,font);g.DrawString(label,font,Brushes.Black,bx-sz.Width/2,by-sz.Height/2);
              }
              if(focus && bw<=8)for(int yy=y0;yy<y1;yy+=32)g.FillRectangle(Brushes.White,x0,yy,bw,8);
              if(bw>160 || bh>160)using(var font=new Font("Arial",13,FontStyle.Bold,GraphicsUnit.Pixel))
                for(int y=y0+8;y<y1;y+=55)for(int x=x0+3;x<x1;x+=65) {
                  if(Math.Abs(x-bx)<50 && Math.Abs(y-by)<35)continue;
                  g.DrawString(label,font,Brushes.Black,x,y);g.DrawLine(Pens.Black,x,y+22,x+26,y+22);g.DrawLine(Pens.Black,x+26,y+22,x+22,y+18);
                }
            }
          }
          var ptn=Read(pat);
          for(int y=y0;y<y1;y++)for(int x=x0;x<x1;x++){int p=y*W+x;if(m[p*4+3]==0)continue;for(int c=0;c<3;c++)a[p*4+c]=ptn[p*4+c];a[p*4+3]=255;}
        }
      }
      Write(output,a);Save(output,Path.Combine(root,focus?"diagnostics/d006-exterior-codes":"diagnostics/d005-remaining-parts"));
    }
  }
  public static void Run(string root) {
    var rows=File.ReadAllLines(Path.Combine(root,"islands.csv"));
    using(var original=new Bitmap(Path.Combine(root,"template/base-reference.png")))
    using(var small=original.Clone(new Rectangle(0,0,W,W),PixelFormat.Format32bppArgb)) {
      var output=Read(small);var occupied=new bool[W*W];
      var neutralIds="|7|81|116|119|122|125|126|128|129|136|";
      Color[] palette={Color.FromArgb(255,220,70),Color.FromArgb(80,240,245),Color.FromArgb(255,120,210),Color.FromArgb(150,255,80),Color.FromArgb(255,145,60),Color.FromArgb(170,170,255)};
      for(int i=1;i<rows.Length;i++) {
        var fields=rows[i].Split(',');int id=int.Parse(fields[0].Substring(1));if(id==1)continue;
        int x0=int.Parse(fields[1]),y0=int.Parse(fields[2]),x1=int.Parse(fields[3]),y1=int.Parse(fields[4]);
        using(var mask=new Bitmap(Path.Combine(root,fields[6])))
        using(var pattern=new Bitmap(W,W,PixelFormat.Format32bppArgb)) {
          var m=Read(mask);bool neutral=neutralIds.Contains("|"+id+"|");
          using(var g=Graphics.FromImage(pattern)) using(var font=new Font("Arial",11,FontStyle.Bold,GraphicsUnit.Pixel)) {
            g.Clear(neutral?Color.FromArgb(185,185,185):palette[id%palette.Length]);
            if(!neutral) {
              for(int y=y0;y<y1;y+=40)for(int x=x0;x<x1;x+=48) {
                g.DrawLine(Pens.Black,x+3,y+33,x+35,y+33);g.DrawLine(Pens.Black,x+35,y+33,x+30,y+29);
                g.DrawString(id.ToString("000"),font,Brushes.Black,x+4,y+6);
              }
              int cx=(x0+x1)/2,cy=(y0+y1)/2;
              float size=Math.Max(6,Math.Min(12,Math.Min((x1-x0)/2.2f,(y1-y0)/1.4f)));
              using(var centerFont=new Font("Arial",size,FontStyle.Bold,GraphicsUnit.Pixel)) {
                var txt=id.ToString("000");var s=g.MeasureString(txt,centerFont);g.DrawString(txt,centerFont,Brushes.Black,cx-s.Width/2,cy-s.Height/2);
              }
            }
          }
          var ptn=Read(pattern);
          for(int y=y0;y<y1;y++)for(int x=x0;x<x1;x++) {int p=y*W+x;if(m[p*4+3]==0)continue;occupied[p]=true;for(int c=0;c<3;c++) output[p*4+c]=ptn[p*4+c];output[p*4+3]=255;}
        }
      }
      Write(small,output);Save(small,Path.Combine(root,"diagnostics/d003-small-parts"));
      // A separate local coordinate test. All main surfaces are neutral, with labels
      // only around the two headlight/fender/bumper regions. x,y labels are cell indices.
      using(var joins=original.Clone(new Rectangle(0,0,W,W),PixelFormat.Format32bppArgb))
      using(var pattern=new Bitmap(W,W,PixelFormat.Format32bppArgb)) {
        using(var g=Graphics.FromImage(pattern)) using(var font=new Font("Arial",9,FontStyle.Bold,GraphicsUnit.Pixel)) {
          g.Clear(Color.FromArgb(185,185,185));
          foreach(var rect in new[]{new Rectangle(288,704,384,352),new Rectangle(288,1472,384,384)})
          for(int y=rect.Top;y<rect.Bottom;y+=32)for(int x=rect.Left;x<rect.Right;x+=32) {
            using(var brush=new SolidBrush(palette[((x/32)+(y/32))%palette.Length]))g.FillRectangle(brush,x,y,32,32);
            g.DrawRectangle(Pens.Black,x,y,32,32);g.DrawString((x/32)+","+(y/32),font,Brushes.Black,x+1,y+3);
            g.DrawLine(Pens.Black,x+5,y+25,x+26,y+25);g.DrawLine(Pens.Black,x+26,y+25,x+22,y+21);
          }
        }
        var a=Read(joins);var b=Read(pattern);
        for(int p=0;p<W*W;p++) if(occupied[p]) {for(int c=0;c<3;c++)a[p*4+c]=b[p*4+c];a[p*4+3]=255;}
        Write(joins,a);Save(joins,Path.Combine(root,"diagnostics/d004-headlight-joins"));
      }
    }
    Console.WriteLine("Prepared d003 small-part IDs and d004 headlight 32px coordinate cells. No source PSD or installed paint changed.");
  }
}
'@ -ReferencedAssemblies System.Drawing.Common,System.Drawing.Primitives,System.Runtime.InteropServices,System.Collections,System.Console
[MappingDiagnostics718]::Run($mappingRoot)
$remaining = (Get-Content -LiteralPath (Join-Path $mappingRoot 'diagnostics/d005-plan.json') -Raw | ConvertFrom-Json).active_component_numbers
[MappingDiagnostics718]::Remaining($mappingRoot, ('|' + ($remaining -join '|') + '|'))
[MappingDiagnostics718]::Remaining($mappingRoot, '|8|10|11|12|17|48|83|84|86|121|130|137|139|141|149|151|2|', $true)
