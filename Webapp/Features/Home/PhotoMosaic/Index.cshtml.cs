using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using PhotoMosaic;
using RazorSlices;
using Webapp.lib.Razor;

namespace Webapp.Features.Home.PhotoMosaic;

[HttpGet(Url)]
[AllowAnonymous]
public class PhotoMosaicEndpoint : EndpointWithoutRequest
{
    public const string Url = "/photo-mosaic";

    private readonly IRazorViewToStringRenderer _renderer;

    public PhotoMosaicEndpoint(IRazorViewToStringRenderer renderer)
    {
        _renderer = renderer;
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var content = await _renderer.GetViewForModel(new Index());
        await SendResultAsync(Results.Content(content, "text/html"));
    }
}

public class Index
{
}