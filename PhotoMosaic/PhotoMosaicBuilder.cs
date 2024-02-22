using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PhotoMosaic;

public class PhotoMosaicBuilder
{
    private readonly ILogger _logger;

    public PhotoMosaicBuilder(ILogger logger)
    {
        _logger = logger;
    }

    public Image<Rgba32> Generate(Image<Rgba32> src, Dictionary<string, Image<Rgba32>> dataset)
    {
        _logger.LogInformation("Generating Mosaic - Dataset average rgb...");
        var dataSetAveRGB = dataset
            .Select(x => new KeyValuePair<string, Rgba32>(x.Key, x.Value.AverageRgba()))
            .ToDictionary();

        _logger.LogInformation("Generating Mosaic - Splitting into chunks...");
        var chunks = src.SplitInto(1600);

        _logger.LogInformation("Generating Mosaic - Chunks average rgb...");
        var chunksAveRGB = chunks
            .Select(e => new KeyValuePair<Coord, Rgba32>(e.Key, e.Value.AverageRgba()))
            .ToDictionary();

        _logger.LogInformation("Generating Mosaic - Distance chunks to each dataset...");
        var chunkToImageDistances = chunksAveRGB
            .Select(e =>
                new KeyValuePair<Coord, Dictionary<string, double>>(e.Key, dataSetAveRGB
                    .Select(d => new KeyValuePair<string, double>(
                            d.Key,
                            e.Value.ConvertToXyz().ConvertToLab()
                                .DeltaECieDistance(d.Value.ConvertToXyz().ConvertToLab())
                        )
                    )
                    .ToDictionary()
                )
            )
            .ToDictionary();

        _logger.LogInformation("Generating Mosaic - Matching new chunks...");
        var chunksToDataset = chunkToImageDistances
            .Select(e => new KeyValuePair<Coord, Image<Rgba32>>(e.Key, dataset[
                    e.Value.MinBy(k => k.Value).Key
                ])
            )
            .ToDictionary();

        _logger.LogInformation("Generating Mosaic - Resizing new chunks...");
        var newChunks = chunksToDataset
            .Select(e => new KeyValuePair<Coord, Image<Rgba32>>(
                    e.Key, e.Value
                        .Clone(ipc =>
                            ipc.Resize(chunks[e.Key].Width, chunks[e.Key].Height)
                        )
                )
            )
            .ToDictionary();

        _logger.LogInformation("Generating Mosaic - Creating mosaic...");
        var mosaic = ChunksToMosaic(newChunks, src.Width, src.Height);

        return mosaic;
    }


    private Image<Rgba32> ChunksToMosaic(Dictionary<Coord, Image<Rgba32>> chunks, int width, int height)
    {
        var mosaic = new Image<Rgba32>(width, height);

        var xS = (int)(mosaic.Width / Math.Sqrt(chunks.Count));
        var yS = (int)(mosaic.Height / Math.Sqrt(chunks.Count));

        var cols = mosaic.Width / xS;
        var rows = mosaic.Height / yS;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                var section = new Coord(r, c);
                var chunk = chunks[section];

                mosaic.DrawImage(c * xS, r * yS, chunk);
            }
        }

        return mosaic;
    }
}