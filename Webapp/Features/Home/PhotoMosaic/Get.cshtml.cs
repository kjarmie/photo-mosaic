using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using RazorSlices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Webapp.lib.Razor;

namespace Webapp.Features.Home.PhotoMosaic;

public class GetMosaic
{
}

[HttpGet(Url)]
[AllowAnonymous]
public class GetMosaicEndpoint : Endpoint<GetMosaicRequest>
{
   public const string Url = "/photo-mosaic/get";

   private readonly IRazorViewToStringRenderer _renderer;

   public GetMosaicEndpoint(IRazorViewToStringRenderer renderer)
   {
      _renderer = renderer;
   }

   public override async Task HandleAsync(GetMosaicRequest req, CancellationToken ct)
   {
      var file = GetImage(req.imageId);
      if(file is null)
         await SendResultAsync(Results.NoContent());

      var content = await _renderer.GetViewForModel(new GetMosaic());
      await SendResultAsync(Results.Content(content, "text/html"));

   }

   private Image<Rgba32>?  GetImage(string id)
   {
      string assetsPath = Path.Combine(Environment.CurrentDirectory, "output");

      Image<Rgba32> image = new Image<Rgba32>(1, 1);

      var filePath = Path.Combine(assetsPath, $"{id}.png");
      try
      {
         // Load the image
         image = Image.Load<Rgba32>(filePath);
      }
      catch (Exception ex)
      {
         // Handle exceptions like invalid format or missing file
         Console.WriteLine($"Error loading image: {filePath} - {ex.Message}");
         return null;
      }

      return image;
   }
}

public record GetMosaicRequest(string imageId);