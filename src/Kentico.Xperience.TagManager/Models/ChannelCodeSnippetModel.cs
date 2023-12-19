namespace Kentico.Xperience.TagManager.Models
{
    public class UpdateCodeSnippetsResultModel
    {
        public IList<ChannelCodeSnippetModel> NewCodeSnippets { get; set; } = Array.Empty<ChannelCodeSnippetModel>();
        public IList<CodeSnippetToRemove> CodeSnippetsToRemove { get; set; } = Array.Empty<CodeSnippetToRemove>();
        public int[] CodeSnippetsIDs { get; set; } = Array.Empty<int>();
    }

    public class ChannelCodeSnippetModel
    {
        public int ID { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public class CodeSnippetToRemove
    {
        public string Location { set; get; } = string.Empty;
        public string WrapperID { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public class RequestModel
    {
        public int[] Ids { get; set; } = Array.Empty<int>();
    }
}
