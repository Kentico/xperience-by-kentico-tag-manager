using Kentico.Xperience.TagManager.Rendering;

namespace Kentico.Xperience.TagManager.Snippets;
public class CodeSnippet
{
    public string? Code { get; init; }
    public CodeSnippetLocations Location { get; init; }
    public CodeSnippet(string? code, CodeSnippetLocations location)
    {
        Code = code;
        Location = location;
    }
}
