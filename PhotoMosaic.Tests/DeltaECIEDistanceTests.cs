using SixLabors.ImageSharp.ColorSpaces;

namespace PhotoMosaic.Tests;

public class DeltaECieDistanceTests
{
    [Theory]
    [InlineData(15.487, 37.811, -52.431, 18.475, 21.887, -48.513, 16.6689125)]
    [InlineData(15.487, 37.811, -52.431, 18.475, 23.675, -42.513, 17.5248784)]
    public void DeltaECieDistance_VariousValues_AssertOutcomes(float l1, float a1, float b1, float l2, float a2,
        float b2, double outcome)
    {
        var lab1 = new CieLab(l1, a1, b1);
        var lab2 = new CieLab(l2, a2, b2);

        var result = lab1.DeltaECieDistance(lab2);

        Assert.Equal(Math.Truncate(outcome * 100.0) / 100.0, Math.Truncate(result * 100.0) / 100.0);
    }
}