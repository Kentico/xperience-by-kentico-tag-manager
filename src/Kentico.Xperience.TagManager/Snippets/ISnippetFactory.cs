using Kentico.Xperience.TagManager.Rendering;

namespace Kentico.Xperience.TagManager.Snippets;

public interface ISnippetFactory
{
    /// <summary>
    /// Creates settings for a Snippet Type
    /// </summary>
    /// <returns><see cref="CodeSnippetSettings"/></returns>
    CodeSnippetSettings CreateCodeSnippetSettings();

    /// <summary>
    /// Called when <see cref="CodeSnippetSettings.HasIdentifier"/> is set to True.
    /// </summary>
    /// <param name="thirdPartyIdentifier"></param>
    /// <returns><see cref="IEnumerable{CodeSnippet}"/></returns>
    IEnumerable<CodeSnippet> CreateCodeSnippets(string thirdPartyIdentifier);

    /// <summary>
    /// Called when <see cref="CodeSnippetSettings.HasCustomCode"/> is set to True.
    /// You can customize or validate custom code here.
    /// </summary>
    /// <param name="codeSnippet"></param>
    /// <returns> <see cref="CodeSnippetDto"/></returns>
    CodeSnippetDto AdjustCodeSnippet(CodeSnippetDto codeSnippet);
}

public abstract class AbstractSnippetFactory : ISnippetFactory
{
    public virtual CodeSnippetSettings CreateCodeSnippetSettings() => new("", "", false, false);

    public virtual IEnumerable<CodeSnippet> CreateCodeSnippets(string thirdPartyIdentifier) => new List<CodeSnippet>();
    public virtual CodeSnippetDto AdjustCodeSnippet(CodeSnippetDto codeSnippet) => codeSnippet;
}
