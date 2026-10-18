using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

// Angular outlined capitals drawn specifically for the windshield wordmark.
// Coordinates are font-independent and scale cleanly for another car's banner.
public static class RenderBundyOutline
{
    const float Height = 70;
    const float Shear = 0.13f;
    static readonly float[][][][] Glyphs = new float[][][][]
    {
        new [] {
            new [] { new[]{0f,0f},new[]{42f,0f},new[]{58f,10f},new[]{58f,27f},new[]{51f,35f},new[]{58f,43f},new[]{58f,60f},new[]{47f,70f},new[]{0f,70f} },
            new [] { new[]{13f,12f},new[]{37f,12f},new[]{45f,17f},new[]{45f,27f},new[]{39f,31f},new[]{13f,31f} },
            new [] { new[]{13f,42f},new[]{39f,42f},new[]{45f,47f},new[]{45f,57f},new[]{39f,59f},new[]{13f,59f} }
        },
        new [] {
            new [] { new[]{0f,0f},new[]{13f,0f},new[]{13f,52f},new[]{18f,57f},new[]{40f,57f},new[]{45f,52f},new[]{45f,0f},new[]{58f,0f},new[]{58f,58f},new[]{47f,70f},new[]{11f,70f},new[]{0f,58f} }
        },
        new [] {
            new [] { new[]{0f,70f},new[]{0f,0f},new[]{13f,0f},new[]{45f,45f},new[]{45f,0f},new[]{58f,0f},new[]{58f,70f},new[]{45f,70f},new[]{13f,24f},new[]{13f,70f} }
        },
        new [] {
            new [] { new[]{0f,0f},new[]{38f,0f},new[]{58f,14f},new[]{58f,56f},new[]{38f,70f},new[]{0f,70f} },
            new [] { new[]{13f,13f},new[]{34f,13f},new[]{45f,22f},new[]{45f,48f},new[]{34f,57f},new[]{13f,57f} }
        },
        new [] {
            new [] { new[]{0f,0f},new[]{14f,0f},new[]{29f,26f},new[]{44f,0f},new[]{58f,0f},new[]{36f,42f},new[]{36f,70f},new[]{22f,70f},new[]{22f,42f} }
        }
    };

    static PointF[] Points(float[][] source, float origin)
    {
        var result = new PointF[source.Length];
        for (int i = 0; i < source.Length; i++)
            result[i] = new PointF(origin + source[i][0] + Shear * (Height - source[i][1]), source[i][1]);
        return result;
    }

    static GraphicsPath WordPath()
    {
        var path = new GraphicsPath(FillMode.Alternate);
        for (int i = 0; i < Glyphs.Length; i++)
            foreach (var polygon in Glyphs[i]) path.AddPolygon(Points(polygon, i * 70));
        return path;
    }

    static void Draw(Graphics g, bool blackBackground)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.CompositingQuality = CompositingQuality.HighQuality;
        g.Clear(blackBackground ? Color.FromArgb(13, 15, 18) : Color.Transparent);
        g.TranslateTransform(13, 8);
        using (var path = WordPath())
        using (var ink = new Pen(Color.FromArgb(242, 245, 239), 3.0f))
        using (var echo = new Pen(Color.FromArgb(170, 122, 154, 164), 3.4f))
        {
            ink.LineJoin = LineJoin.Bevel;
            echo.LineJoin = LineJoin.Bevel;
            g.TranslateTransform(2.6f, 2.8f);
            g.DrawPath(echo, path);
            g.TranslateTransform(-2.6f, -2.8f);
            g.DrawPath(ink, path);
        }
    }

    public static string Run(string pngPath, string svgPath, string previewPath)
    {
        using (var bitmap = new Bitmap(375, 88, PixelFormat.Format32bppArgb))
        using (var g = Graphics.FromImage(bitmap))
        {
            Draw(g, false);
            bitmap.Save(pngPath, ImageFormat.Png);
        }
        using (var bitmap = new Bitmap(375, 88, PixelFormat.Format32bppArgb))
        using (var g = Graphics.FromImage(bitmap))
        {
            Draw(g, true);
            bitmap.Save(previewPath, ImageFormat.Png);
        }
        var sb = new StringBuilder();
        sb.AppendLine("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"375\" height=\"88\" viewBox=\"0 0 375 88\">");
        sb.AppendLine("<g transform=\"translate(13 8)\" fill=\"none\" stroke-linejoin=\"bevel\">");
        var commands = new StringBuilder();
        for (int i = 0; i < Glyphs.Length; i++)
            foreach (var polygon in Glyphs[i])
            {
                var pts = Points(polygon, i * 70);
                commands.Append('M');
                for (int j = 0; j < pts.Length; j++)
                {
                    if (j > 0) commands.Append('L');
                    commands.Append(pts[j].X.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));
                    commands.Append(' ');
                    commands.Append(pts[j].Y.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));
                }
                commands.Append('Z');
            }
        string d = commands.ToString();
        sb.AppendLine("<path transform=\"translate(2.6 2.8)\" d=\"" + d + "\" stroke=\"#7a9aa4\" stroke-opacity=\"0.67\" stroke-width=\"3.4\"/>");
        sb.AppendLine("<path d=\"" + d + "\" stroke=\"#f2f5ef\" stroke-width=\"3\"/>");
        sb.AppendLine("</g></svg>");
        File.WriteAllText(svgPath, sb.ToString());
        return "Saved outlined BUNDY PNG, SVG, and black-background preview.";
    }
}
