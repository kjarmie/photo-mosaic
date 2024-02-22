using JetBrains.Annotations;

namespace Webapp.lib.Helpers;

public static class Htmx
{
    /// <summary>
    /// Very simple helper method to format text as html in Jetbrains IDE's or when using
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static string html([LanguageInjection(InjectedLanguage.HTML)] string html)
    {
        return html;
    }
}