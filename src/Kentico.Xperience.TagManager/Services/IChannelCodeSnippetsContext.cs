using Kentico.Xperience.TagManager.Models;

namespace Kentico.Xperience.TagManager.Services
{
    public interface IChannelCodeSnippetsContext
    {
        Task<IList<ChannelCodeSnippetDto>> GetCodeSnippets();
    }
}
