using Microsoft.Extensions.Logging;
using PhotoMosaic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;

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

// Console.WriteLine($"{DateTime.Now.ToLongTimeString()} - Loading images...");
logger.LogInformation("Loading dataset...");
var dataset = LoadImages();

logger.LogInformation("Loading source...");
var source = LoadSource();

logger.LogInformation("Generating Mosaic...");
var mosaic = builder.Generate(source, dataset);

logger.LogInformation("Saving Mosaic...");
SaveImageToJpg(mosaic, "outputs", "output");

void SaveImageToJpg(Image image, string outputDirectory, string fileName)
{
    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, outputDirectory));

    // Build the full output path
    string filePath = Path.Combine(Environment.CurrentDirectory, "../../../", outputDirectory, fileName + ".jpg");

    try
    {
        var encoder = new JpegEncoder { Quality = 100 }; // Set desired quality if needed
        image.Save(filePath, encoder);
    }
    catch (Exception ex)
    {
        // Handle saving error
        Console.WriteLine($"Error saving image: {filePath} - {ex.Message}");
    }
}

Image<Rgba32> LoadSource()
{
    string assetsPath = Path.Combine(Environment.CurrentDirectory, "../../../", "inputs");

    Image<Rgba32> image = new Image<Rgba32>(1, 1);

    var filePath = Path.Combine(assetsPath, "source.jpg");
    try
    {
        // Load the image
        image = Image.Load<Rgba32>(filePath);
    }
    catch (Exception ex)
    {
        // Handle exceptions like invalid format or missing file
        Console.WriteLine($"Error loading image: {filePath} - {ex.Message}");
    }

    return image;
}


Dictionary<string, Image<Rgba32>> LoadImages()
{
    string assetsPath = Path.Combine(Environment.CurrentDirectory, "../../../", "assets");

    Dictionary<string, Image<Rgba32>> images = new Dictionary<string, Image<Rgba32>>();

    foreach (string filePath in Directory.EnumerateFiles(assetsPath, "*.jpg", SearchOption.AllDirectories))
    {
        try
        {
            var title = filePath.Split("/").Last().Replace("assets\\", "").Replace(".jpg", "");
            // Load the image
            var image = Image.Load<Rgba32>(filePath);

            // Add the image to the list
            images.Add(title, image);
        }
        catch (Exception ex)
        {
            // Handle exceptions like invalid format or missing file
            Console.WriteLine($"Error loading image: {filePath} - {ex.Message}");
        }
    }

    return images;
}