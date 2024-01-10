using Kentico.Xperience.TagManager.Enums;

namespace Kentico.Xperience.TagManager.Models
{
    public class ChannelCodeSnippetDto
    {
        public required int ID { get; init; }
        public required string Code { get; init; }
        public required CodeSnippetLocations Location { get; init; }
        public required int Consent { get; init; }
    }
}
