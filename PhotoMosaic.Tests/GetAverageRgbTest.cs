using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PhotoMosaic.Tests;

public class GetAverageRgbTest
{
    [Fact]
    public void GetAverageRgb_SinglePixelImage_AssertPixelColor()
    {
        // Arrange
        var image = new Image<Rgba32>(1, 1);
        var pixel = new Rgba32(255, 255, 255);
        image[0, 0] = pixel;

        // Act
        var result = image.AverageRgba();

        // Assert
        Assert.Equal(pixel, result);
    }

    [Fact]
    public void GetAverageRgb_SingleColorImage_AssertPixelColor()
    {
        // Arrange
        var image = new Image<Rgba32>(10, 5);
        var pixel = new Rgba32(255, 255, 255);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                image[i, j] = pixel;
            }
        }

        // Act
        var result = image.AverageRgba();

        // Assert
        Assert.Equal(pixel, result);
    }

    [Fact]
    public void GetAverageRgb_MultiColorImage_AssertAccurateAverage()
    {
        // Arrange
        var image = new Image<Rgba32>(2, 2);
        var average = new Rgba32(184, 146, 131);

        image[0, 0] = new Rgba32(237, 28, 36);
        image[0, 1] = new Rgba32(63, 72, 204);
        image[1, 0] = new Rgba32(255, 255, 255);
        image[1, 1] = new Rgba32(181, 230, 29);

        // Act
        var result = image.AverageRgba();

        // Assert
        Assert.Equal(average, result);
    }
}