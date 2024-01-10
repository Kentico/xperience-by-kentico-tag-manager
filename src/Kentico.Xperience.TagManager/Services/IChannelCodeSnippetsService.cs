using Kentico.Xperience.TagManager.Enums;
using Kentico.Xperience.TagManager.Models;

namespace Kentico.Xperience.TagManager.Services
{
    public interface IChannelCodeSnippetsService
    {
        Task<ILookup<CodeSnippetLocations, ChannelCodeSnippetDto>> GetCodeSnippets();
    }
}
