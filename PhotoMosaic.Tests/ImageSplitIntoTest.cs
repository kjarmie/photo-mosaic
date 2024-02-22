using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PhotoMosaic.Tests;

public class ImageSplitIntoTest
{
    private readonly Image<Rgba32> _image;

    public ImageSplitIntoTest()
    {
        _image = new Image<Rgba32>(10, 10);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                _image[i, j] = new Rgba32(i, j, i + j);
            }
        }
    }

    // [Fact]
    // public void SplitInto_LessThan2Chunks_AssertArgumentException()
    // {
    //     var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _image.SplitInto(0));
    // }

    [Fact]
    public void SplitInto_NChunks_AssertListLengthNSquared()
    {
        var result = _image.SplitInto(2);

        Assert.Equal(4, result.Count());
    }


}

