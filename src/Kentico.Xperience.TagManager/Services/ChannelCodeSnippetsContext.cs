using CMS.ContentEngine;
using CMS.Websites;
using CMS.Websites.Routing;
using CMS.DataProtection;
using CMS.ContactManagement;
using Kentico.Xperience.TagManager.Enums;
using GTM;
using Kentico.Xperience.TagManager.Models;

namespace Kentico.Xperience.TagManager.Services
{
    internal class ChannelCodeSnippetsContext : IChannelCodeSnippetsContext
    {
        private readonly IConsentAgreementService consentAgreementService;
        private readonly IConsentInfoProvider consentInfoProvider;
        private readonly IWebsiteChannelContext channelContext;
        private readonly IChannelCodeSnippetInfoProvider codeSnippetInfoProvider;

        public ChannelCodeSnippetsContext(IConsentAgreementService consentAgreementService, IConsentInfoProvider consentInfoProvider, IWebsiteChannelContext channelContext, IChannelCodeSnippetInfoProvider codeSnippetInfoProvider)
        {
            this.consentAgreementService = consentAgreementService;
            this.consentInfoProvider = consentInfoProvider;
            this.channelContext = channelContext;
            this.codeSnippetInfoProvider = codeSnippetInfoProvider;
        }

        public async Task<IList<ChannelCodeSnippetDto>> GetCodeSnippets()
        {
            var allCodeSnippets = GetChannelCodeSnippets();
            var contact = ContactManagementContext.CurrentContact;
            var agreedConsentIDs = consentInfoProvider.Get()
                .WhereIn(nameof(ConsentInfo.ConsentID), allCodeSnippets.Select(x => x.ChannelCodeSnippetConsentID).ToArray())
                .GetEnumerableTypedResult()
                .Where(c => consentAgreementService.IsAgreed(contact, c))
                .Select(c => c.ConsentID)
                .ToList();
            var filteredCodeSnippets = allCodeSnippets
                .Where(c => agreedConsentIDs.Contains(c.ChannelCodeSnippetConsentID))
                .Select(c => new ChannelCodeSnippetDto
                {
                    GTMId = c.ChannelCodeSnippetGTMID ?? string.Empty,
                    Code = c.ChannelCodeSnippetCode ?? string.Empty,
                    Location = !string.IsNullOrEmpty(c.ChannelCodeSnippetLocation) ?
                        Enum.Parse<CodeSnippetLocations>(c.ChannelCodeSnippetLocation)
                        : null,
                    ID = c.ChannelCodeSnippetID,
                })
                .ToList();
            return filteredCodeSnippets;

        }

        private IList<ChannelCodeSnippetInfo> GetChannelCodeSnippets()
        {
            return codeSnippetInfoProvider.Get()
            .Source(x =>
                x.InnerJoin<ChannelInfo>(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetChannelID), nameof(ChannelInfo.ChannelID))
            ).Source(x =>
                x.InnerJoin<WebsiteChannelInfo>(nameof(ChannelInfo.ChannelID), nameof(WebsiteChannelInfo.WebsiteChannelChannelID))
            ).WhereEquals(nameof(WebsiteChannelInfo.WebsiteChannelID), channelContext.WebsiteChannelID)
            .Columns(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetLocation),
                nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetCode),
                nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetConsentID),
                nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetGTMID),
                nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetID))
            .ToList();
        }

    }
}
