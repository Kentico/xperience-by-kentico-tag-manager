using CMS.ContactManagement;

namespace Kentico.Xperience.TagManager.Rendering;

public interface IChannelCodeSnippetsService
{
    Task<ILookup<CodeSnippetLocations, CodeSnippetDto>> GetConsentedCodeSnippets(ContactInfo? contact, int? contentTypeId = null);
}
