namespace Kentico.Xperience.TagManager.Rendering;

public class CodeSnippetDto
{
    public int ID { get; init; }
    public string? Code { get; init; }
    public CodeSnippetLocations Location { get; init; }
    public CodeSnippetAdministrationDisplayMode DisplayMode { get; set; }
}
