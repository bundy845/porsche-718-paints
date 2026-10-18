using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

public static class RenderR002
{
    const int Size = 2048;
    static readonly Color Ink = Color.FromArgb(222, 23, 29, 24);
    static readonly Color FineInk = Color.FromArgb(145, 23, 29, 24);

    public static void Run(string cleanMask, string outputFolder)
    {
        using (var source = new Bitmap(cleanMask))
        using (var body = new Bitmap(Size, Size, PixelFormat.Format32bppArgb))
        using (var ink = new Bitmap(Size, Size, PixelFormat.Format32bppArgb))
        using (var inkSpec = new Bitmap(Size, Size, PixelFormat.Format32bppArgb))
        {
            var mask = SourcePixels(source);
            BuildBody(mask, body);
            BuildInk(mask, ink);
            BuildInkSpec(ink, inkSpec);
            body.Save(System.IO.Path.Combine(outputFolder, "body-r002.png"), ImageFormat.Png);
            ink.Save(System.IO.Path.Combine(outputFolder, "ink-r002.png"), ImageFormat.Png);
            inkSpec.Save(System.IO.Path.Combine(outputFolder, "ink-spec-r002.png"), ImageFormat.Png);
        }
    }

    static Rectangle Rect { get { return new Rectangle(0, 0, Size, Size); } }
    static byte[] Read(Bitmap bitmap)
    {
        var data = bitmap.LockBits(Rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        var bytes = new byte[data.Stride * Size];
        Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
        bitmap.UnlockBits(data);
        return bytes;
    }
    static void Write(Bitmap bitmap, byte[] bytes)
    {
        var data = bitmap.LockBits(Rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);
        bitmap.UnlockBits(data);
    }
    static byte[] SourcePixels(Bitmap bitmap)
    {
        using (var copy = bitmap.Clone(Rect, PixelFormat.Format32bppArgb)) return Read(copy);
    }
    static int BlueAlpha(byte[] data, int x, int y)
    {
        if (x < 0 || x >= Size || y < 0 || y >= Size) return 0;
        int p = (y * Size + x) * 4;
        int blue = Math.Min((int)data[p] - (int)data[p + 2] - 8,
                            (int)data[p] - (int)data[p + 1] - 5);
        if (blue <= 0 || data[p] < 65 || data[p + 1] < 70) return 0;
        return Math.Min(255, blue * 6);
    }
    static bool Inside(byte[] data, int x, int y) { return BlueAlpha(data, x, y) >= 220; }
    static bool MarkedRegion(int x, int y)
    {
        // Major mapped panels only. Small UV fittings are left clean.
        return (x > 450 && x < 1580 && y > 505 && y < 1020) ||
               (x > 410 && x < 1580 && y > 1510 && y < 2040) ||
               (x > 260 && x < 940 && y > 1040 && y < 1570) ||
               (x > 1070 && x < 2030 && y > 1040 && y < 1520) ||
               (x > 930 && x < 2030 && y > 15 && y < 495);
    }
    static bool ContourBand(byte[] mask, int x, int y)
    {
        if (!MarkedRegion(x, y) || !Inside(mask, x, y)) return false;
        int inner = 3, outer = 10;
        bool away = Inside(mask, x - inner, y) && Inside(mask, x + inner, y) &&
                    Inside(mask, x, y - inner) && Inside(mask, x, y + inner);
        bool near = !Inside(mask, x - outer, y) || !Inside(mask, x + outer, y) ||
                    !Inside(mask, x, y - outer) || !Inside(mask, x, y + outer);
        return away && near;
    }

    static void BuildBody(byte[] mask, Bitmap body)
    {
        var pixels = new byte[mask.Length];
        for (int y = 0; y < Size; y++)
        for (int x = 0; x < Size; x++)
        {
            int p = (y * Size + x) * 4;
            pixels[p] = 0;
            pixels[p + 1] = 245;
            pixels[p + 2] = 184;
            pixels[p + 3] = (byte)BlueAlpha(mask, x, y);
        }
        Write(body, pixels);
    }

    static void BuildInk(byte[] mask, Bitmap ink)
    {
        var pixels = new byte[mask.Length];
        for (int y = 0; y < Size; y++)
        for (int x = 0; x < Size; x++)
        {
            if (!ContourBand(mask, x, y)) continue;
            int p = (y * Size + x) * 4;
            pixels[p] = 24;
            pixels[p + 1] = 29;
            pixels[p + 2] = 23;
            pixels[p + 3] = 228;
        }
        Write(ink, pixels);
        using (var g = Graphics.FromImage(ink))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var wing = new SolidBrush(Color.FromArgb(248, 23, 29, 24)))
                g.FillRectangle(wing, 0, 0, 635, 445);
            DrawSmallSketchMarks(g, mask);
        }
        // Clip every mark to the paintable UV surface.
        pixels = Read(ink);
        for (int y = 0; y < Size; y++)
        for (int x = 0; x < Size; x++)
        {
            int p = (y * Size + x) * 4;
            pixels[p + 3] = (byte)(pixels[p + 3] * BlueAlpha(mask, x, y) / 255);
        }
        Write(ink, pixels);
    }

    static void BuildInkSpec(Bitmap ink, Bitmap spec)
    {
        var artwork = Read(ink);
        var pixels = new byte[artwork.Length];
        for (int p = 0; p < pixels.Length; p += 4)
        {
            pixels[p] = 0;
            pixels[p + 1] = 135;
            pixels[p + 2] = 0;
            pixels[p + 3] = artwork[p + 3];
        }
        Write(spec, pixels);
    }

    static void DrawSmallSketchMarks(Graphics g, byte[] mask)
    {
        using (var pen = new Pen(Ink, 2.5f))
        using (var light = new Pen(FineInk, 1.3f))
        {
            pen.StartCap = LineCap.Round; pen.EndCap = LineCap.Round;
            light.StartCap = LineCap.Round; light.EndCap = LineCap.Round;
            // Broken, slightly offset marks are tangent to real UV edges.
            for (int y = 22; y < Size - 22; y += 13)
            for (int x = 22; x < Size - 22; x += 13)
            {
                if (!ContourBand(mask, x, y) || ((x / 13 + y / 13 * 7) % 11) != 0) continue;
                float gx = (Inside(mask, x + 14, y) ? 1 : 0) - (Inside(mask, x - 14, y) ? 1 : 0);
                float gy = (Inside(mask, x, y + 14) ? 1 : 0) - (Inside(mask, x, y - 14) ? 1 : 0);
                float length = (float)Math.Sqrt(gx * gx + gy * gy);
                if (length < 0.1f) continue;
                float tx = -gy / length, ty = gx / length;
                float nx = gx / length, ny = gy / length;
                int span = 10 + ((x * 3 + y) % 13);
                g.DrawLine(pen, x - tx * span, y - ty * span, x + tx * span * .5f, y + ty * span * .5f);
                g.DrawLine(light, x - tx * span * .7f + nx * 5, y - ty * span * .7f + ny * 5,
                                  x + tx * span * .65f + nx * 5, y + ty * span * .65f + ny * 5);
            }
            // A few short panel accents, with stops before UV joins.
            Line(g, pen, 735, 710, 750, 741, 765, 766);
            Line(g, light, 742, 708, 757, 738, 772, 755);
            Line(g, pen, 1420, 686, 1434, 723, 1450, 748);
            Line(g, light, 1427, 682, 1441, 710, 1455, 730);
            Line(g, pen, 810, 1640, 824, 1673, 833, 1695);
            Line(g, light, 816, 1638, 829, 1663, 840, 1686);
            Line(g, pen, 1420, 1675, 1432, 1708, 1443, 1736);
            Line(g, light, 1426, 1672, 1439, 1700, 1451, 1724);
            Line(g, pen, 520, 1210, 530, 1195, 545, 1188);
            Line(g, light, 525, 1216, 539, 1198, 552, 1193);
            Line(g, pen, 720, 1450, 746, 1462, 773, 1468);
        }
    }
    static void Line(Graphics g, Pen pen, float x1, float y1, float x2, float y2, float x3, float y3)
    {
        g.DrawLines(pen, new[] { new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3) });
    }
}
