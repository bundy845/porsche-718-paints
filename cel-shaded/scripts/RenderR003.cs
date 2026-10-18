using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

// Panel paths are traced against the supplied 718 PSD wireframe, with Clyde
// Coman's labeled parts paint used to identify the physical body panels.
public static class RenderR003
{
    const int Size = 2048;
    static readonly Color Ink = Color.FromArgb(244, 20, 29, 18);
    static readonly Color Echo = Color.FromArgb(150, 20, 29, 18);
    static readonly Color Shadow = Color.FromArgb(170, 79, 129, 0);

    public static void Run(string maskFile, string outputFolder)
    {
        using (var mask = new Bitmap(maskFile))
        using (var artwork = new Bitmap(Size, Size, PixelFormat.Format32bppArgb))
        using (var spec = new Bitmap(Size, Size, PixelFormat.Format32bppArgb))
        {
            var g = Graphics.FromImage(artwork);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            DrawPanels(g);
            g.Dispose();
            ClipToMask(artwork, mask);
            BuildSpec(artwork, spec);
            artwork.Save(System.IO.Path.Combine(outputFolder, "ink-r003.png"), ImageFormat.Png);
            spec.Save(System.IO.Path.Combine(outputFolder, "ink-spec-r003.png"), ImageFormat.Png);
        }
    }

    static void DrawPanels(Graphics g)
    {
        using (var wing = new SolidBrush(Color.FromArgb(245, 20, 29, 18)))
        {
            g.FillRectangle(wing, 0, 0, 635, 220);
            g.FillRectangle(wing, 0, 225, 635, 225);
        }
        using (var main = new Pen(Ink, 4.2f))
        using (var thin = new Pen(Echo, 2.0f))
        using (var wide = new Pen(Ink, 6.0f))
        using (var shade = new SolidBrush(Shadow))
        {
            foreach (var p in new[] { main, thin, wide })
            {
                p.StartCap = LineCap.Round;
                p.EndCap = LineCap.Round;
                p.LineJoin = LineJoin.Round;
            }

            // Upper side: the door, intake and sill live in one maroon island
            // in Clyde's parts paint. These paths follow visible panel edges.
            Line(g, main, 918,570, 920,730, 936,801, 982,831);
            Line(g, thin, 925,572, 928,724, 944,795, 992,828);
            Line(g, main, 983,832, 1130,839, 1300,840, 1409,848);
            Line(g, main, 1437,846, 1451,792, 1442,750, 1404,721);
            Line(g, thin, 1448,849, 1462,791, 1453,744, 1412,715);
            Line(g, wide, 1384,593, 1410,630, 1446,694, 1479,731);
            Line(g, main, 1394,588, 1425,621, 1461,685, 1490,717);
            Line(g, main, 1491,737, 1511,753, 1535,752, 1565,748);
            Line(g, main, 1465,754, 1522,763, 1570,764, 1620,755);
            Line(g, main, 950,829, 1070,854, 1290,864, 1417,856);
            Line(g, thin, 972,840, 1100,861, 1280,872, 1395,866);
            Line(g, main, 863,586, 873,544, 893,532, 933,535);
            Line(g, main, 1620,757, 1680,744, 1740,729, 1785,737);
            Line(g, thin, 1618,767, 1680,753, 1744,740, 1777,747);
            // Sketch hatches near the intake and the door's lower return.
            Line(g, thin, 1377,654, 1390,665, 1404,681, 1410,696);
            Line(g, thin, 1390,649, 1405,664, 1418,681, 1425,693);
            Line(g, thin, 1030,852, 1060,861, 1090,863, 1110,865);
            Line(g, main, 1539,857, 1569,867, 1590,867, 1610,864);

            // Lower side: same physical cues, but the UV island is rotated
            // around its own wheel arches; do not simply mirror pixel art.
            Line(g, main, 918,1752, 917,1848, 926,1924, 952,1980);
            Line(g, thin, 926,1757, 925,1844, 935,1918, 959,1974);
            Line(g, main, 955,1981, 1100,1986, 1270,1988, 1414,1985);
            Line(g, main, 1431,1981, 1454,1945, 1459,1909, 1444,1875);
            Line(g, thin, 1440,1986, 1462,1945, 1468,1904, 1452,1869);
            Line(g, wide, 1464,1805, 1444,1848, 1410,1898, 1383,1939);
            Line(g, main, 1473,1800, 1456,1843, 1422,1893, 1395,1935);
            Line(g, main, 1381,1940, 1427,1960, 1484,1953, 1540,1913);
            Line(g, main, 1448,1767, 1512,1753, 1580,1750, 1635,1758);
            Line(g, thin, 1458,1777, 1520,1764, 1581,1760, 1622,1768);
            Line(g, main, 981,1972, 1098,1954, 1275,1954, 1371,1964);
            Line(g, main, 880,1749, 897,1718, 921,1709, 946,1712);
            Line(g, thin, 1393,1875, 1410,1863, 1424,1852, 1435,1837);
            Line(g, thin, 1404,1887, 1420,1874, 1438,1856, 1447,1840);
            Line(g, main, 1494,1968, 1535,1975, 1565,1973, 1595,1968);

            // Hood: the purple island in Clyde's map. Broken parallel strokes
            // sit inside its perimeter and follow the front-to-screen seam.
            Line(g, wide, 409,1043, 489,1015, 723,1005, 899,992);
            Line(g, main, 410,1056, 514,1029, 728,1022, 884,1005);
            Line(g, main, 371,1107, 357,1212, 361,1330, 402,1434);
            Line(g, thin, 381,1110, 368,1210, 371,1330, 410,1418);
            Line(g, wide, 405,1454, 541,1487, 733,1516, 880,1531);
            Line(g, main, 420,1441, 558,1478, 746,1501, 868,1516);
            Line(g, main, 822,1048, 757,1120, 744,1320, 821,1472);
            Line(g, thin, 832,1050, 772,1132, 758,1310, 833,1464);
            Line(g, main, 448,1068, 507,1060, 554,1050, 590,1042);
            Line(g, thin, 435,1082, 500,1073, 546,1064, 577,1056);
            Line(g, main, 444,1432, 520,1450, 581,1467, 620,1478);

            // Roof and rear deck: restrained outline around the roof hatch
            // and a few short registration strokes by the rear glass.
            Line(g, main, 1131,1064, 1250,1072, 1373,1077, 1490,1061);
            Line(g, thin, 1137,1074, 1255,1081, 1376,1085, 1484,1071);
            Line(g, main, 1132,1456, 1240,1470, 1370,1482, 1501,1454);
            Line(g, main, 1190,1267, 1270,1269, 1375,1269, 1450,1267);
            Line(g, thin, 1195,1278, 1270,1280, 1375,1279, 1444,1277);
            Line(g, main, 1189,1268, 1193,1340, 1195,1420, 1194,1450);
            Line(g, main, 1450,1268, 1450,1330, 1448,1410, 1444,1450);
            Line(g, thin, 1207,1445, 1241,1450, 1266,1454, 1290,1455);

            // Short bumper and wing-end marks: isolated pieces per Clyde.
            Line(g, main, 161,1053, 154,1172, 160,1300, 174,1360);
            Line(g, thin, 171,1056, 166,1172, 171,1300, 184,1353);
            Line(g, main, 292,1015, 309,1050, 311,1100, 301,1135);
            Line(g, main, 292,1444, 307,1478, 306,1520, 294,1560);
            Line(g, main, 33,225, 132,209, 256,209, 356,226);
            Line(g, thin, 48,238, 137,223, 246,222, 344,236);
            Line(g, main, 20,400, 138,392, 260,392, 375,404);
        }
    }

    static void Line(Graphics g, Pen pen, params float[] c)
    {
        g.DrawBezier(pen, c[0],c[1],c[2],c[3],c[4],c[5],c[6],c[7]);
    }

    static byte[] Read(Bitmap b)
    {
        var rect = new Rectangle(0,0,Size,Size);
        var d = b.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        var bytes = new byte[d.Stride*Size];
        Marshal.Copy(d.Scan0,bytes,0,bytes.Length);
        b.UnlockBits(d);
        return bytes;
    }
    static void Write(Bitmap b, byte[] bytes)
    {
        var rect = new Rectangle(0,0,Size,Size);
        var d = b.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        Marshal.Copy(bytes,0,d.Scan0,bytes.Length);
        b.UnlockBits(d);
    }
    static void ClipToMask(Bitmap art, Bitmap mask)
    {
        using (var copy = mask.Clone(new Rectangle(0,0,Size,Size), PixelFormat.Format32bppArgb))
        {
            var a=Read(art); var m=Read(copy);
            for (int p=0;p<a.Length;p+=4)
            {
                int blue=Math.Min(m[p]-m[p+2]-8,m[p]-m[p+1]-5);
                int alpha=(blue<=0 || m[p]<65 || m[p+1]<70)?0:Math.Min(255,blue*6);
                a[p+3]=(byte)(a[p+3]*alpha/255);
            }
            Write(art,a);
        }
    }
    static void BuildSpec(Bitmap art, Bitmap spec)
    {
        var a=Read(art); var s=new byte[a.Length];
        for (int p=0;p<s.Length;p+=4)
        { s[p]=0; s[p+1]=135; s[p+2]=0; s[p+3]=a[p+3]; }
        Write(spec,s);
    }
}
