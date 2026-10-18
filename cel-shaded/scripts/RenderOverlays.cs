using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

public static class RenderOverlays
{
    const int Size = 2048;
    static readonly Color Ink = Color.FromArgb(245, 23, 29, 24);
    static readonly Color FineInk = Color.FromArgb(180, 23, 29, 24);
    static readonly Color Shadow = Color.FromArgb(225, 76, 113, 16);
    static readonly Color DeepShadow = Color.FromArgb(195, 53, 85, 20);
    static readonly Color Highlight = Color.FromArgb(190, 231, 255, 102);

    public static void Run(string template, string outputFolder)
    {
        using (var source = new Bitmap(template))
        using (var body = new Bitmap(Size, Size, PixelFormat.Format32bppArgb))
        using (var shade = NewBitmap())
        using (var light = NewBitmap())
        using (var ink = NewBitmap())
        {
            MakeBody(source, body);
            DrawShadows(shade);
            DrawHighlights(light);
            DrawInk(ink);
            MaskToBlue(source, shade);
            MaskToBlue(source, light);
            MaskToBlue(source, ink);
            body.Save(System.IO.Path.Combine(outputFolder, "body-r001.png"), ImageFormat.Png);
            shade.Save(System.IO.Path.Combine(outputFolder, "shadows-r001.png"), ImageFormat.Png);
            light.Save(System.IO.Path.Combine(outputFolder, "highlights-r001.png"), ImageFormat.Png);
            ink.Save(System.IO.Path.Combine(outputFolder, "ink-r001.png"), ImageFormat.Png);
        }
    }

    static Bitmap NewBitmap() { return new Bitmap(Size, Size, PixelFormat.Format32bppArgb); }
    static Rectangle Bounds { get { return new Rectangle(0, 0, Size, Size); } }

    static byte[] Pixels(Bitmap image)
    {
        var data = image.LockBits(Bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        var bytes = new byte[data.Stride * Size];
        Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
        image.UnlockBits(data);
        return bytes;
    }

    static void PutPixels(Bitmap image, byte[] bytes)
    {
        var data = image.LockBits(Bounds, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);
        image.UnlockBits(data);
    }

    static byte[] SourcePixels(Bitmap source)
    {
        using (var copy = source.Clone(Bounds, PixelFormat.Format32bppArgb)) return Pixels(copy);
    }

    static int BodyAlpha(byte b, byte g, byte r)
    {
        // The untouched template uses RGB 62,137,205 on the main body.
        // Intermediate blue edge pixels receive partial alpha to avoid a hard fringe.
        int blue = Math.Min((int)b - (int)r - 8, (int)b - (int)g - 5);
        if (blue <= 0 || b < 65 || g < 70) return 0;
        return Math.Min(255, blue * 6);
    }

    static void MakeBody(Bitmap source, Bitmap body)
    {
        var sourceData = SourcePixels(source);
        var target = new byte[sourceData.Length];
        for (int p = 0; p < target.Length; p += 4)
        {
            target[p] = 0; target[p + 1] = 245; target[p + 2] = 184;
            target[p + 3] = (byte)BodyAlpha(sourceData[p], sourceData[p + 1], sourceData[p + 2]);
        }
        PutPixels(body, target);
    }

    static void MaskToBlue(Bitmap source, Bitmap artwork)
    {
        var sourceData = SourcePixels(source);
        var target = Pixels(artwork);
        for (int p = 0; p < target.Length; p += 4)
        {
            int alpha = BodyAlpha(sourceData[p], sourceData[p + 1], sourceData[p + 2]);
            target[p + 3] = (byte)(target[p + 3] * alpha / 255);
        }
        PutPixels(artwork, target);
    }

    static Graphics Canvas(Bitmap image)
    {
        var g = Graphics.FromImage(image);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.CompositingMode = CompositingMode.SourceOver;
        return g;
    }

    static void Stroke(Graphics g, Color color, float width, params PointF[] points)
    {
        using (var pen = new Pen(color, width))
        {
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            pen.LineJoin = LineJoin.Round;
            if (points.Length == 4) g.DrawBezier(pen, points[0], points[1], points[2], points[3]);
            else g.DrawLines(pen, points);
        }
    }

    static PointF P(float x, float y) { return new PointF(x, y); }

    static void DrawShadows(Bitmap image)
    {
        using (var g = Canvas(image))
        using (var brush = new SolidBrush(Shadow))
        using (var deep = new SolidBrush(DeepShadow))
        using (var wing = new SolidBrush(Color.FromArgb(250, 23, 29, 24)))
        {
            // Both mapped faces of the rear wing blade.
            g.FillRectangle(wing, 0, 0, 635, 445);
            // Side 1: a long, uneven lower shadow with a cut around the door.
            g.FillPolygon(brush, new[] { P(470, 895), P(695, 920), P(880, 892), P(1070, 914), P(1230, 892), P(1420, 911), P(1595, 876), P(1580, 1030), P(430, 1030) });
            g.FillPolygon(deep, new[] { P(760, 835), P(930, 860), P(1120, 846), P(1300, 865), P(1390, 895), P(1280, 915), P(1010, 892), P(850, 902) });
            // Side 2 mirrors the visual rhythm, without flipping its shape.
            g.FillPolygon(brush, new[] { P(440, 1764), P(670, 1798), P(860, 1779), P(1040, 1804), P(1250, 1781), P(1440, 1810), P(1588, 1760), P(1600, 2048), P(425, 2048) });
            g.FillPolygon(deep, new[] { P(780, 1730), P(965, 1750), P(1140, 1726), P(1320, 1747), P(1410, 1780), P(1300, 1800), P(1030, 1768), P(850, 1795) });
            // Hood and roof: broad graphic shadow shapes.
            g.FillPolygon(brush, new[] { P(400, 1270), P(800, 1190), P(910, 1220), P(815, 1430), P(670, 1510), P(470, 1450) });
            g.FillPolygon(brush, new[] { P(1080, 1395), P(1270, 1370), P(1430, 1405), P(1550, 1380), P(1555, 1500), P(1080, 1520) });
            // Bumper and fender edge blocks.
            g.FillPolygon(brush, new[] { P(960, 127), P(1060, 152), P(1190, 180), P(1400, 140), P(1600, 160), P(1850, 120), P(2015, 140), P(2000, 250), P(950, 270) });
            g.FillPolygon(brush, new[] { P(1610, 358), P(1770, 340), P(1945, 354), P(2020, 390), P(2010, 510), P(1585, 500) });
        }
    }

    static void DrawHighlights(Bitmap image)
    {
        using (var g = Canvas(image))
        {
            Stroke(g, Highlight, 38, P(560, 590), P(740, 535), P(1020, 555), P(1230, 580));
            Stroke(g, Highlight, 27, P(930, 630), P(1080, 600), P(1280, 612), P(1470, 650));
            Stroke(g, Highlight, 34, P(535, 1590), P(735, 1545), P(1025, 1575), P(1210, 1600));
            Stroke(g, Highlight, 25, P(1130, 1650), P(1280, 1630), P(1460, 1660), P(1540, 1700));
            Stroke(g, Highlight, 31, P(310, 1080), P(460, 1050), P(680, 1080), P(835, 1120));
            Stroke(g, Highlight, 24, P(1100, 1055), P(1350, 1050), P(1650, 1070), P(1810, 1110));
            Stroke(g, Highlight, 20, P(1110, 62), P(1390, 38), P(1780, 55), P(1970, 90));
        }
    }

    static void DrawInk(Bitmap image)
    {
        using (var g = Canvas(image))
        {
            // Side 1, silhouette and deliberate duplicate pen passes.
            Stroke(g, Ink, 8, P(520, 570), P(710, 535), P(840, 540), P(1040, 562));
            Stroke(g, FineInk, 3, P(525, 581), P(700, 548), P(838, 548), P(1030, 570));
            Stroke(g, Ink, 7, P(520, 868), P(690, 915), P(860, 904), P(1015, 895));
            Stroke(g, FineInk, 3, P(540, 881), P(720, 922), P(900, 914), P(1080, 905));
            Stroke(g, Ink, 6, P(1110, 870), P(1250, 905), P(1410, 902), P(1570, 865));
            Stroke(g, FineInk, 3, P(1124, 883), P(1270, 912), P(1430, 913), P(1560, 879));
            Stroke(g, Ink, 7, P(785, 715), P(830, 790), P(840, 858), P(925, 900));
            Stroke(g, FineInk, 3, P(800, 713), P(843, 780), P(858, 843), P(942, 884));
            Stroke(g, Ink, 7, P(1370, 658), P(1390, 750), P(1420, 825), P(1500, 866));
            Stroke(g, FineInk, 3, P(1385, 670), P(1403, 748), P(1437, 821), P(1509, 849));
            // Side 2.
            Stroke(g, Ink, 8, P(530, 1605), P(720, 1565), P(900, 1570), P(1060, 1595));
            Stroke(g, FineInk, 3, P(540, 1619), P(736, 1579), P(916, 1581), P(1080, 1608));
            Stroke(g, Ink, 8, P(485, 1750), P(650, 1795), P(825, 1800), P(1010, 1788));
            Stroke(g, FineInk, 3, P(500, 1767), P(666, 1811), P(852, 1812), P(1030, 1801));
            Stroke(g, Ink, 7, P(1090, 1774), P(1260, 1804), P(1430, 1800), P(1580, 1750));
            Stroke(g, FineInk, 3, P(1100, 1787), P(1270, 1815), P(1440, 1814), P(1580, 1763));
            Stroke(g, Ink, 7, P(830, 1650), P(855, 1720), P(865, 1768), P(930, 1800));
            Stroke(g, FineInk, 3, P(842, 1658), P(867, 1724), P(882, 1774), P(945, 1795));
            Stroke(g, Ink, 7, P(1370, 1640), P(1395, 1710), P(1418, 1772), P(1490, 1800));
            // Hood, roof, bumpers and quick sketch hatches.
            Stroke(g, Ink, 8, P(390, 1200), P(550, 1160), P(710, 1170), P(850, 1210));
            Stroke(g, FineInk, 3, P(415, 1216), P(560, 1175), P(720, 1182), P(846, 1223));
            Stroke(g, Ink, 7, P(460, 1440), P(590, 1500), P(760, 1525), P(875, 1495));
            Stroke(g, Ink, 8, P(1090, 1400), P(1260, 1380), P(1440, 1410), P(1560, 1390));
            Stroke(g, FineInk, 3, P(1100, 1417), P(1270, 1394), P(1445, 1421), P(1550, 1406));
            Stroke(g, Ink, 7, P(1010, 180), P(1190, 210), P(1410, 210), P(1585, 182));
            Stroke(g, FineInk, 3, P(1030, 193), P(1200, 220), P(1420, 224), P(1600, 194));
            Stroke(g, Ink, 7, P(1620, 380), P(1765, 345), P(1910, 355), P(2020, 398));
            Stroke(g, FineInk, 3, P(1632, 390), P(1770, 359), P(1910, 368), P(2010, 410));
            Stroke(g, Ink, 5, P(930, 744), P(950, 752), P(970, 759), P(990, 764));
            Stroke(g, Ink, 5, P(1190, 759), P(1215, 751), P(1240, 750), P(1260, 755));
            Stroke(g, FineInk, 3, P(930, 751), P(943, 744), P(954, 742), P(970, 741));
            Stroke(g, Ink, 5, P(940, 1680), P(960, 1685), P(985, 1692), P(1010, 1690));
            Stroke(g, Ink, 5, P(1190, 1690), P(1210, 1680), P(1240, 1679), P(1260, 1685));
        }
    }
}
