using Microsoft.Extensions.Configuration;

namespace Kentico.Xperience.TagManager.Snippets;

internal static class SnippetFactoryStore
{
    private static readonly Dictionary<string, ISnippetFactory> snippetFactories = [];
    public static void AddSnippetFactory<TSnippetFactory>(IConfiguration configuration) where TSnippetFactory : ISnippetFactory, new()
    {
        const string section = "xbyk.tagmanager.modules";

        var usedTagModuleSection = configuration.GetSection(section).GetChildren();
        var snippetFactory = new TSnippetFactory();
        var settings = snippetFactory.CreateCodeSnippetSettings();
        string configurationString = settings.TagAppSettingsName;
        string tagType = settings.TagTypeName;

        if (usedTagModuleSection.Any() && !usedTagModuleSection.Any(x => x.Value == configurationString))
        {
            return;
        }

        if (!snippetFactories.TryAdd(tagType, snippetFactory))
        {
            throw new InvalidOperationException($"Snippet Factory with name {tagType} is already registered.");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="channelCodeSnippetType"></param>
    /// <returns>true if the store contains the specified snippet factory; otherwise, false.</returns>
    public static ISnippetFactory? TryGetSnippetFactory(string channelCodeSnippetType)
    {
        if (snippetFactories.TryGetValue(channelCodeSnippetType, out var factory))
        {
            return factory;
        }

        return null;
    }
    public static IEnumerable<ISnippetFactory> GetRegisteredSnippetFactories() => snippetFactories.Values;
    public static IEnumerable<string> GetRegisteredSnippetFactoryTypes() => snippetFactories.Keys;
}
