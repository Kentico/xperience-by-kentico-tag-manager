using Kentico.Xperience.TagManager.Enums;

namespace Kentico.Xperience.TagManager.Models
{
    public class ChannelCodeSnippetDto
    {
        public CodeSnippetLocations? Location { get; set; }
        public string Code { get; set; } = string.Empty;
        public string GTMId { get; set; } = string.Empty;
        public int ID { get; set; }
    }
}
