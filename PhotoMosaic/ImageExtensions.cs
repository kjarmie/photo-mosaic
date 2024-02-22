using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PhotoMosaic;

public static class ImageExtensions
{
    public static void Process(this Image<Rgba32> image, Action<Rgba32> func)
    {
        for (int i = 0; i < image.Width; i++)
        {
            for (int j = 0; j < image.Height; j++)
            {
                func(image[i, j]);
            }
        }
    }


    public static Rgba32 AverageRgba(this Image<Rgba32> image)
    {
        double sumR = 0, sumG = 0, sumB = 0;
        double totalPixels = image.Width * (double)image.Height;

        image.Process(pixel =>
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

    /// <summary>
    /// Splits an image into a sequence of smaller sub-images.
    /// </summary>
    /// <param name="image">The image to split.</param>
    /// <param name="chunks">The desired number of chunks horizontally and vertically. Must be greater than 2.</param>
    /// <returns>An `IEnumerable` of sub-images.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the `chunks` parameter is less than or equal to 2.
    /// </exception>
    public static Dictionary<Coord, Image<Rgba32>> SplitInto(this Image<Rgba32> image, int chunks)
    {
        if (chunks < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(chunks), "Number of chunks must be greater than 2.");
        }

        var chunkWidth = Math.Max(1, image.Width / chunks);
        var chunkHeight = Math.Max(1, image.Height / chunks);

        Dictionary<Coord, Image<Rgba32>> dict = new Dictionary<Coord, Image<Rgba32>>();

        int row = 0;
        int col = 0;
        for (int i = 0; i < image.Width; i += chunkWidth)
        {
            row = 0;
            for (int j = 0; j < image.Height; j += chunkHeight)
            {
                int actualWidth = Math.Min(chunkWidth, image.Width - i);
                int actualHeight = Math.Min(chunkHeight, image.Height - j);

                Rectangle chunkRect = new Rectangle(i, j, actualWidth, actualHeight);
                var chunk = image.Clone(
                    ipc => ipc
                        .Crop(chunkRect)
                );

                dict.Add(new Coord(row, col), chunk);

                row++;
            }

            col++;
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
}