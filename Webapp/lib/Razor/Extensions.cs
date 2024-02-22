using System.Reflection;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Razor.Hosting;
using RazorSlices;

namespace Webapp.lib.Razor;

public static class Extensions
{
    public static async Task<string> GetViewForModel<T>(this IRazorViewToStringRenderer renderer, T model)
    {
        // string path = Path.Combine(Path.GetFullPath("."), $"{typeof(Extensions).Namespace}.dll");
        // var assembly = Assembly.LoadFrom(path);

        var assembly = Assembly.GetExecutingAssembly();

        var identifier = assembly
            .GetTypes()
            .First(e => e.IsSubclassOf(typeof(RazorPage<T>)))
            .GetCustomAttribute<RazorSourceChecksumAttribute>()
            .Identifier;

        if (identifier is null)
            throw new Exception("The model provided is not associated with any view.");

        var result = await renderer.RenderViewToStringAsync(identifier, model);

        return result;
    }
}