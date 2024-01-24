using CMS.ContactManagement;

namespace Kentico.Xperience.TagManager.Rendering;

public interface IChannelCodeSnippetsService
{
    Task<ILookup<CodeSnippetLocations, ChannelCodeSnippetDto>> GetConsentedCodeSnippets(ContactInfo? contact);
}
