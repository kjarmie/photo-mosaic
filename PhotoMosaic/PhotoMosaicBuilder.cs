using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PhotoMosaic;

public class PhotoMosaicBuilder
{
    public Image<Rgba32> Build(Image<Rgba32> src, Dictionary<string, Image<Rgba32>> dataset)
    {
        var dataSetAveRGB = dataset
            .Select(x => new KeyValuePair<string, Rgba32>(x.Key, x.Value.AverageRgba()))
            .ToDictionary();

        var chunks = src.SplitInto(20);
        var chunksAveRGB = chunks
            .Select(e => new KeyValuePair<Coord, Rgba32>(e.Key, e.Value.AverageRgba()))
            .ToDictionary();

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

        var chunksToDataset = chunkToImageDistances
            .Select(e => new KeyValuePair<Coord, Image<Rgba32>>(e.Key, dataset[
                    e.Value.MinBy(k => k.Value).Key
                ])
            )
            .ToDictionary();

        var newChunks = chunksToDataset
            .Select(e => new KeyValuePair<Coord, Image<Rgba32>>(e.Key, e.Value
                .Clone(ipc => ipc.Resize(e.Value.Width, e.Value.Height))))
            .ToDictionary();

        var mosaic = ChunksToMosaic(newChunks, src.Width, src.Height);

        return mosaic;
    }


    private Image<Rgba32> ChunksToMosaic(Dictionary<Coord, Image<Rgba32>> chunks, int width, int height)
    {
        var mosaic = new Image<Rgba32>(width, height);

        var cols = chunks.MaxBy(e => e.Key.col).Key.col;
        var rows = chunks.MaxBy(e => e.Key.row).Key.row;


        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                var coord = new Coord(j, i);
                var chunk = chunks[coord];
                AddChunkToMosaic(mosaic, coord, chunk);
            }
        }

        return mosaic;
    }

    private void AddChunkToMosaic(Image<Rgba32> mosaic, Coord section, Image<Rgba32> chunk)
    {
        int i = 0;
        int j = 0;

        int row = 0;
        int col = 0;
        try
        {
            for (i = section.col * chunk.Width; i < (section.row + 1) * chunk.Width; i++)
            {
                row = 0;
                for (j = section.row * chunk.Height; j < (section.row + 1) * chunk.Height; j++)
                {
                    var pixel =  chunk[col, row];
                    mosaic[i, j] = pixel;
                    row++;
                }

                col++;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Chunk: {chunk.Height} (H) x {chunk.Width} (W)");
            Console.WriteLine($"row: {row}");
            Console.WriteLine($"col: {col}");
            Console.WriteLine($"i: {i}");
            Console.WriteLine($"i-max: {(section.row + 1) * chunk.Width}");
            Console.WriteLine($"j: {j}");
            Console.WriteLine($"j-max: {(section.row + 1) * chunk.Height}");
            throw;
        }

        // for (int i = section.row * 20; i < (section.row + 1) * 20; i++)
        // {
        //     col = 0;
        //     for (int j = section.col * 20; j < (section.col + 1) * 20; j++)
        //     {
        //         mosaic[i, j] = chunk[row, col];
        //         col++;
        //     }
        //
        //     row++;
        // }
    }
}