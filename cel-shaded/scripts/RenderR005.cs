using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

// Panel paths are traced against the supplied 718 PSD wireframe, with Clyde
// Coman's labeled parts paint used to identify the physical body panels.
public static class RenderR005
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
            artwork.Save(System.IO.Path.Combine(outputFolder, "ink-r005.png"), ImageFormat.Png);
            spec.Save(System.IO.Path.Combine(outputFolder, "ink-spec-r005.png"), ImageFormat.Png);
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

            // One restrained green shadow tone, kept inside the mapped door
            // and intake panels. It reads as a graphic plane in hard light.
            g.FillPolygon(shade, new[] {
                new PointF(964,813), new PointF(1110,827), new PointF(1325,832),
                new PointF(1412,820), new PointF(1399,845), new PointF(1120,847),
                new PointF(982,836) });
            g.FillPolygon(shade, new[] {
                new PointF(972,1953), new PointF(1100,1958), new PointF(1310,1955),
                new PointF(1401,1947), new PointF(1413,1977), new PointF(1100,1980),
                new PointF(981,1971) });
            g.FillPolygon(shade, new[] {
                new PointF(1397,640), new PointF(1440,682), new PointF(1490,730),
                new PointF(1472,738), new PointF(1423,699) });
            g.FillPolygon(shade, new[] {
                new PointF(1462,1820), new PointF(1438,1872), new PointF(1393,1930),
                new PointF(1411,1934), new PointF(1456,1884) });

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
            Line(g, main, 1644,788, 1687,779, 1725,774, 1747,781);
            Line(g, thin, 1656,796, 1687,789, 1722,785, 1733,790);
            Line(g, main, 1805,793, 1831,803, 1848,814, 1856,829);
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
            Line(g, main, 1468,1798, 1512,1789, 1550,1786, 1574,1792);
            Line(g, thin, 1480,1807, 1520,1800, 1556,1799, 1565,1803);
            Line(g, main, 1665,1811, 1691,1821, 1712,1835, 1720,1852);
            Line(g, main, 981,1972, 1098,1954, 1275,1954, 1371,1964);
            Line(g, main, 880,1749, 897,1718, 921,1709, 946,1712);
            Line(g, thin, 1393,1875, 1410,1863, 1424,1852, 1435,1837);
            Line(g, thin, 1404,1887, 1420,1874, 1438,1856, 1447,1840);
            Line(g, main, 1494,1968, 1535,1975, 1565,1973, 1595,1968);

            // Hood: the purple island in Clyde's map. Broken parallel strokes
            // sit inside its perimeter and follow the front-to-screen seam.
            Line(g, wide, 416,1043, 492,1021, 553,1015, 625,1010);
            Line(g, main, 432,1057, 508,1036, 568,1030, 606,1023);
            Line(g, main, 733,1012, 787,1008, 839,1000, 881,995);
            Line(g, main, 371,1107, 357,1212, 361,1330, 402,1434);
            Line(g, thin, 381,1110, 368,1210, 371,1330, 410,1418);
            Line(g, wide, 406,1455, 484,1477, 570,1492, 636,1504);
            Line(g, main, 422,1443, 499,1467, 575,1480, 618,1490);
            Line(g, main, 738,1513, 791,1522, 837,1527, 875,1530);
            // The transverse r004 hood pair looked like a decorative stripe
            // on the car, so the hood center is intentionally left open.
            Line(g, main, 448,1068, 507,1060, 554,1050, 590,1042);
            Line(g, thin, 435,1082, 500,1073, 546,1064, 577,1056);
            Line(g, main, 444,1432, 520,1450, 581,1467, 620,1478);

            // Roof and rear deck: restrained outline around the roof hatch
            // and a few short registration strokes by the rear glass.
            Line(g, main, 1133,1066, 1200,1071, 1257,1076, 1300,1077);
            Line(g, thin, 1140,1077, 1200,1082, 1248,1085, 1278,1087);
            Line(g, main, 1393,1072, 1440,1070, 1470,1064, 1490,1061);
            Line(g, main, 1134,1457, 1200,1467, 1255,1472, 1290,1474);
            Line(g, main, 1400,1472, 1450,1465, 1481,1459, 1501,1454);
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
        // Slight, repeatable displacement makes the contour feel drawn while
        // keeping its anchors on the measured 718 panel paths.
        var points = new PointF[33];
        double phase = (c[0] * .021 + c[1] * .013) % (Math.PI * 2);
        for (int i=0; i<points.Length; i++)
        {
            double t=i/32.0, u=1.0-t;
            double x=u*u*u*c[0]+3*u*u*t*c[2]+3*u*t*t*c[4]+t*t*t*c[6];
            double y=u*u*u*c[1]+3*u*u*t*c[3]+3*u*t*t*c[5]+t*t*t*c[7];
            double dx=3*u*u*(c[2]-c[0])+6*u*t*(c[4]-c[2])+3*t*t*(c[6]-c[4]);
            double dy=3*u*u*(c[3]-c[1])+6*u*t*(c[5]-c[3])+3*t*t*(c[7]-c[5]);
            double length=Math.Sqrt(dx*dx+dy*dy);
            double wobble=Math.Sin(Math.PI*t)*(Math.Sin(17*t+phase)*1.3+Math.Sin(39*t+phase*.7)*.45);
            if (length>0.01) { x+=-dy/length*wobble; y+=dx/length*wobble; }
            points[i]=new PointF((float)x,(float)y);
        }
        g.DrawLines(pen,points);
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
