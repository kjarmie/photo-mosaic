using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using RazorSlices;
using Webapp.lib.Razor;
using static Webapp.lib.Helpers.Htmx;

namespace Webapp.Features.Home;

public class Home
{

}

[HttpGet(Url)]
[AllowAnonymous]
public class HomeEndpoint : EndpointWithoutRequest
{
    public const string Url = "/";
    private readonly IRazorViewToStringRenderer _renderer;

    public HomeEndpoint(IRazorViewToStringRenderer renderer)
    {
        _renderer = renderer;
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var content = await _renderer.GetViewForModel(new Home());
        await SendResultAsync(Results.Content(content, "text/html"));
    }
}

[HttpGet(Url)]
[AllowAnonymous]
public class CurrentTimeEndpoint : EndpointWithoutRequest
{
    public const string Url = "/api/time";

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendResultAsync(Results.Content(DateTime.Now.ToLongTimeString(), "text/html"));
    }
}

[HttpGet(Url)]
[AllowAnonymous]
public class PollingTimeEndpoint : EndpointWithoutRequest
{
    public const string Url = "/api/time-polling";

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendResultAsync(Results.Content(html(
            $"""
             <div id="time" hx-trigger="every 1s" hx-get="{Url}" hx-swap="outerHtml">{DateTime.Now.ToLongTimeString()}</div>
             """), "text/html"));
    }
}