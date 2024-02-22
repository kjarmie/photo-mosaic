using System.ComponentModel.DataAnnotations;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using PhotoMosaic;
using RazorSlices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using Webapp.lib.Razor;

namespace Webapp.Features.Home.PhotoMosaic;

public class Upload
{
    public string requestId;

    public Upload(string requestId)
    {
        this.requestId = requestId;
    }
}

public class PhotoMosaicUploadEndpoint : Endpoint<PhotoMosaicUploadRequest>
{
    public const string Url = "/photo-mosaic/submit";

    private readonly IRazorViewToStringRenderer _renderer;

    public PhotoMosaicUploadEndpoint(IRazorViewToStringRenderer renderer)
    {
        _renderer = renderer;
    }

    public override void Configure()
    {
        Post(Url);
        AllowFileUploads();
        AllowFormData();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PhotoMosaicUploadRequest req, CancellationToken ct)
    {
        // Generate new Id for the request
        var id = Guid.NewGuid().ToString();

        // Send back loading state
        var content = await _renderer.GetViewForModel(new Upload(id));
        await SendResultAsync(Results.Content(content, "text/html"));


        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("Showcase", LogLevel.Debug)
                .AddConsole();
        });
        ILogger logger = loggerFactory.CreateLogger<Program>();


        PhotoMosaicBuilder builder = new PhotoMosaicBuilder(logger);

        var source = await Image.LoadAsync<Rgba32>(req.image.OpenReadStream(), ct);

        var dataset = LoadImages();

        var mosaic = builder.Generate(source, dataset);

        SaveImageToPng(mosaic, "output", id);
    }

    private Dictionary<string, Image<Rgba32>> LoadImages()
    {
        string assetsPath = Path.Combine(Environment.CurrentDirectory, "assets");

        Dictionary<string, Image<Rgba32>> images = new Dictionary<string, Image<Rgba32>>();

        foreach (string filePath in Directory.EnumerateFiles(assetsPath, "*.jpg", SearchOption.AllDirectories))
        {
            try
            {
                var paths = filePath.Split("\\");
                var filename = paths.Last();
                var folder = paths.Reverse().Skip(1).Reverse().Last();
                var title = $"{folder}/{filename}".Replace(".jpg", "");
                var image = Image.Load<Rgba32>(filePath);

                images.Add(title, image);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {filePath} - {ex.Message}");
            }
        }

        return images;
    }

    void SaveImageToPng(Image image, string outputDirectory, string fileName)
    {
        Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, outputDirectory));

        string filePath = Path.Combine(Environment.CurrentDirectory, outputDirectory, fileName + ".png");

        try
        {
            var encoder = new PngEncoder();
            image.Save(filePath, encoder);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving image: {filePath} - {ex.Message}");
        }
    }
}

public record PhotoMosaicUploadRequest(IFormFile image);