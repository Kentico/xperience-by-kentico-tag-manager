namespace Kentico.Xperience.TagManager.Snippets;

internal static class SnippetFactoryStore
{
    private static readonly Dictionary<string, ISnippetFactory> snippetFactories;
    static SnippetFactoryStore() => snippetFactories = [];
    public static void AddSnippetFactory<TSnippetFactory>() where TSnippetFactory : ISnippetFactory, new()
    {
        var snippetFactory = new TSnippetFactory();
        string tagTypeName = snippetFactory.CreateCodeSnippetSettings().TagTypeName;
        snippetFactories.Add(tagTypeName, snippetFactory);
    }

    public static ISnippetFactory GetSnippetFactory(string channelCodeSnippetType) => snippetFactories[channelCodeSnippetType];
    public static IEnumerable<ISnippetFactory> GetSnippetFactories() => snippetFactories.Values;
}
