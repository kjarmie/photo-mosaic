using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PhotoMosaic;

public static class ImageExtensions
{
    public static void Process(this Image<Rgba32> image, Action<Coord, Rgba32> func)
    {
        for (int c = 0; c < image.Width; c++)
        {
            for (int r = 0; r < image.Height; r++)
            {
                func(new Coord(r, c), image[c, r]);
            }
        }
    }

    public static Rgba32 AverageRgba(this Image<Rgba32> image)
    {
        double sumR = 0, sumG = 0, sumB = 0;
        double totalPixels = image.Width * (double)image.Height;

        image.Process((_, pixel) =>
        {
            sumR += pixel.R;
            sumG += pixel.G;
            sumB += pixel.B;
        });

        return new Rgba32(
            (byte)(sumR / totalPixels),
            (byte)(sumG / totalPixels),
            (byte)(sumB / totalPixels)
        );
    }

    public static Dictionary<Coord, Image<Rgba32>> SplitInto(this Image<Rgba32> image, int chunks)
    {
        var xS = (int)(image.Width / Math.Sqrt(chunks));
        var yS = (int)(image.Height / Math.Sqrt(chunks));

        // Console.WriteLine($"Rect size: {xS} (w) x {yS} (h)");

        var cols = image.Width / xS;
        var rows = image.Height / yS;
        // Console.WriteLine($"Dims: {cols} (w) x {rows} (h)");


        Dictionary<Coord, Image<Rgba32>> dict = new Dictionary<Coord, Image<Rgba32>>();

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Rectangle chunkRect = new Rectangle(c * xS, r * yS, xS, yS);
                var chunk = image.Clone(
                    ipc => ipc
                        .Crop(chunkRect)
                );
                dict.Add(new Coord(r, c), chunk);

            }
        }

        return dict;
    }

    public static CieXyz ConvertToXyz(this Rgba32 pixel)
    {
        CieXyz xyz;

        double r = pixel.R / 255.0;
        double g = pixel.G / 255.0;
        double b = pixel.B / 255.0;

        if (r > 0.04045)
            r = Math.Pow((r + 0.055) / 1.055, 2.4);
        else
            r /= 12.92;
        if (g > 0.04045)
            g = Math.Pow((g + 0.055) / 1.055, 2.4);
        else
            g /= 12.92;
        if (b > 0.04045)
            b = Math.Pow((b + 0.055) / 1.055, 2.4);
        else
            b /= 12.92;

        r *= 100;
        g *= 100;
        b *= 100;

        xyz = new CieXyz((float)(r * 0.4124 + g * 0.3576 + b * 0.1805),
            (float)(r * 0.2126 + g * 0.7152 + b * 0.0722),
            (float)(r * 0.0193 + g * 0.1192 + b * 0.9505)
        );

        return xyz;
    }

    public static CieLab ConvertToLab(this CieXyz pixel)
    {
        // D55	95.682	100.000	92.149	95.799	100.000	90.926	Mid-morning daylight
        var refX = 95.682;
        var refY = 100.000;
        var refZ = 92.149;

        CieLab lab;

        var x = pixel.X / refX;
        var y = pixel.Y / refY;
        var z = pixel.Z / refZ;

        if (x > 0.008856)
            x = Math.Pow(x, 1.0 / 3.0);
        else
            x = 7.787 * x + 16.0 / 116.0;
        if (y > 0.008856)
            y = Math.Pow(y, 1.0 / 3.0);
        else
            y = 7.787 * y + 16.0 / 116.0;
        if (z > 0.008856)
            z = Math.Pow(z, 1.0 / 3.0);
        else
            z = 7.787 * z + 16.0 / 116.0;

        lab = new CieLab((float)(116 * y - 16), (float)(500 * (x - y)), (float)(200 * (y - z)));

        return lab;
    }

    public static double DeltaECieDistance(this CieLab pixel, CieLab other)
    {
        return Math.Sqrt(
            Math.Pow(pixel.L - other.L, 2) +
            Math.Pow(pixel.A - other.A, 2) +
            Math.Pow(pixel.B - other.B, 2)
        );
    }

    public static void DrawImage(this Image<Rgba32> image, int startX, int startY, Image<Rgba32> overlay)
    {
        overlay.Process((coord, pixel) =>
        {
            var imageX = startX + coord.col;
            var imageY = startY + coord.row;
            image[imageX, imageY] = pixel;
        });
    }
}