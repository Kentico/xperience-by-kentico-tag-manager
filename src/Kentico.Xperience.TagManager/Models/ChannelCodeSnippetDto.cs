using Kentico.Xperience.TagManager.Enums;

namespace Kentico.Xperience.TagManager.Models;

public class ChannelCodeSnippetDto
{
    public int ID { get; init; }
    public string? Code { get; init; }
    public CodeSnippetLocations Location { get; init; }
}
