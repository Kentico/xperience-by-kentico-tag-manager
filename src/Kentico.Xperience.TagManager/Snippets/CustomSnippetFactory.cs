using System.Text.RegularExpressions;
using Kentico.Xperience.TagManager.Rendering;

namespace Kentico.Xperience.TagManager.Snippets;

public class CustomSnippetFactory : AbstractSnippetFactory
{
    private const string TAG_TYPE_NAME = "Custom";
    private const string TAG_DISPLAY_NAME = "Custom Snippet";

    public override CodeSnippetSettings CreateCodeSnippetSettings() =>
        new(TAG_TYPE_NAME, TAG_DISPLAY_NAME, true, false);

    public override CodeSnippetDto AdjustCodeSnippet(CodeSnippetDto codeSnippet) =>
        new()
        {
            Code = codeSnippet.Code != null ? AddSnippetIds(codeSnippet.ID, codeSnippet.Code!) : codeSnippet.Code,
            ID = codeSnippet.ID,
            Location = codeSnippet.Location
        };

    private static string AddSnippetIds(int codeSnippetId, string codeSnippet) =>
      Regex.Replace(codeSnippet, "<([^\\/]*?)>", $"""<$1 data-snippet-id="{codeSnippetId}">""");
}
