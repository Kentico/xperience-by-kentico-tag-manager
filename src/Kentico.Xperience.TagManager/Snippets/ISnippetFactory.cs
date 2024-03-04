namespace Kentico.Xperience.TagManager.Snippets;

public interface ISnippetFactory
{
    /// <summary>
    /// Creates settings for a Snippet Type
    /// </summary>
    /// <returns><see cref="CodeSnippetSettings"/></returns>
    CodeSnippetSettings CreateCodeSnippetSettings();

    /// <summary>
    /// </summary>
    /// <param name="thirdPartyIdentifier"></param>
    /// <returns><see cref="IEnumerable{CodeSnippet}"/></returns>
    IEnumerable<CodeSnippet> CreateCodeSnippets(string thirdPartyIdentifier);
}

public abstract class AbstractSnippetFactory : ISnippetFactory
{
    public virtual CodeSnippetSettings CreateCodeSnippetSettings() => new("", "");

    public virtual IEnumerable<CodeSnippet> CreateCodeSnippets(string thirdPartyIdentifier) => new List<CodeSnippet>();
}
