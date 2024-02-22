using FastEndpoints;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Razor;
using RazorSlices;
using Vite.AspNetCore.Extensions;
using Webapp.lib.Razor;

// Provides intellisense for Jetbrains IDE's. Can remove this and dependency on Jetbrains.Annotations since
// it is not required for functionality.
[assembly: AspMvcViewLocationFormat("/Features/{0}.cshtml")]
[assembly: AspMvcViewLocationFormat("/Features/Common/{0}.cshtml")]
[assembly: AspMvcViewLocationFormat("/Features/Common/Layout/{0}.cshtml")]

[assembly: AspMvcViewLocationFormat("/Features/{1}.cshtml")]
[assembly: AspMvcViewLocationFormat("/Features/Common/{1}.cshtml")]
[assembly: AspMvcViewLocationFormat("/Features/Common/Layout/{1}.cshtml")]


var builder = WebApplication.CreateBuilder();
var services = builder.Services;
var config = builder.Configuration;

// This service allows rendering the Razor views
services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

// I am using feature slices so I have set the root to '~/Features'
services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Clear();
    options.ViewLocationFormats.Add("/Features/{0}.cshtml");
    options.ViewLocationFormats.Add("/Features/Common/{0}.cshtml");
    options.ViewLocationFormats.Add("/Features/Common/Layout/{0}.cshtml");

    options.PageViewLocationFormats.Clear();
    options.PageViewLocationFormats.Add("/Features/{0}.cshtml");
    options.PageViewLocationFormats.Add("/Features/Common/{0}.cshtml");
    options.PageViewLocationFormats.Add("/Features/Common/Layout/{0}.cshtml");
});

// I prefer lower-case urls, but this is probably not necessary since we are not using the default Pages routing or MVC routes,
// all routes are specified manually.
services.AddRouting(options => { options.LowercaseUrls = true; });

services.AddFastEndpoints();

services.AddViteServices();

// We need the Razor pages service, but you will see below, we don't map the Razor Pages on the WebApplication
var mvcBuilder = builder.Services.AddRazorPages();
if (builder.Environment.IsDevelopment())
{
    // Allows hot reload
    mvcBuilder.AddRazorRuntimeCompilation();
}

var app = builder.Build();

// During development, we run the Vite dev server.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseViteDevelopmentServer(true);
}

app.UseFastEndpoints();
app.UseStaticFiles();

app.UseAuthentication(); // required by FastEndpoints
app.Run();