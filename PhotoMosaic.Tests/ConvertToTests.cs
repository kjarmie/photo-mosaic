using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;

namespace PhotoMosaic.Tests;

public class ConvertToTests
{
    [Theory]
    [InlineData(0.04706, 0.04706, 0.04706, 0.349, 0.368, 0.400)]
    [InlineData(0.07059, 0.46275, 0.48235, 10.301, 14.514, 20.994)]
    [InlineData(0.00392, 0.00392, 0.00392, 0.029, 0.030, 0.03)]
    public void ConvertToXyz_VariousValues_AssertOutcomes(float r, float g, float b, float x, float y, float z)
    {
        var pixel = new Rgba32(r, g, b, 255);

        var result = pixel.ConvertToXyz();

        Assert.Equal(Math.Truncate(x * 100) / 100, Math.Truncate(result.X * 100) / 100);
        Assert.Equal(Math.Truncate(y * 100) / 100, Math.Truncate(result.Y * 100) / 100);
        Assert.Equal(Math.Truncate(z * 100) / 100, Math.Truncate(result.Z * 100) / 100);

        Assert.Equal(
            new CieXyz(
                (float)(Math.Truncate(x * 100) / 100),
                (float)(Math.Truncate(y * 100) / 100),
                (float)(Math.Truncate(z * 100) / 100)
            ),
            new CieXyz(
                (float)(Math.Truncate(result.X * 100) / 100),
                (float)(Math.Truncate(result.Y * 100) / 100),
                (float)(Math.Truncate(result.Z * 100) / 100)
            )
        );
    }

    [Theory]
    // [InlineData(0.349, 0.368, 0.400, 3.321, 0.000, -0.000)]
    // [InlineData(10.301, 14.514, 20.994, 44.962, -24.374, -10.436)]
    // [InlineData(0.029, 0.030, 0.03, 0.274, 0.000, -0.000)]
    [InlineData(3.793, 2.625, 14.491, 18.475, 21.887, -48.513)]
    [InlineData(4, 2, 14, 15.487, 37.811, -52.431)]
    public void ConvertToLab_VariousValues_AssertOutcomes(float x, float y, float z, float l, float a, float b)
    {
        var pixel = new CieXyz(x, y, z);

        var result = pixel.ConvertToLab();

        Assert.Equal(
            new CieLab(
                (float)(Math.Truncate(l * 100.0) / 100.0),
                (float)(Math.Truncate(a * 100.0) / 100.0),
                (float)(Math.Truncate(b * 100.0) / 100.0)
            ),
            new CieLab(
                (float)(Math.Truncate(result.L * 100.0) / 100.0),
                (float)(Math.Truncate(result.A * 100.0) / 100.0),
                (float)(Math.Truncate(result.B * 100.0) / 100.0)
            )
        );
    }
}