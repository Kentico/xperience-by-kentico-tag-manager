using CMS.Base;
using CMS.ContentEngine;
using CMS.Websites;
using CMS.Websites.Routing;
using CMS.DataProtection;
using CMS.ContactManagement;
using CMS.Helpers;
using Kentico.Xperience.TagManager.Enums;
using GTM;
using Kentico.Xperience.TagManager.Models;
using Kentico.Xperience.TagManager.Helpers;

namespace Kentico.Xperience.TagManager.Services
{
    internal class ChannelCodeSnippetsService : IChannelCodeSnippetsService
    {
        private readonly IConsentAgreementService consentAgreementService;
        private readonly IWebsiteChannelContext channelContext;
        private readonly IChannelCodeSnippetInfoProvider codeSnippetInfoProvider;
        private readonly IProgressiveCache cache;

        public ChannelCodeSnippetsService(
            IConsentAgreementService consentAgreementService,
            IWebsiteChannelContext channelContext,
            IChannelCodeSnippetInfoProvider codeSnippetInfoProvider,
            IProgressiveCache cache)
        {
            this.consentAgreementService = consentAgreementService;
            this.channelContext = channelContext;
            this.codeSnippetInfoProvider = codeSnippetInfoProvider;
            this.cache = cache;
        }

        public Task<ILookup<CodeSnippetLocations, ChannelCodeSnippetDto>> GetCodeSnippets()
        {
            var currentContact = ContactManagementContext.CurrentContact;
            return cache.LoadAsync(s =>
            {
                s.CacheDependency =
                    CacheHelper.GetCacheDependency(
                        $"om.contact|byid|{currentContact.ContactID}|children|cms.consentagreement");

                return GetCodeSnippetsInternal();
            }, new CacheSettings(CacheHelper.CacheMinutes()));

            async Task<ILookup<CodeSnippetLocations, ChannelCodeSnippetDto>> GetCodeSnippetsInternal()
            {
                var snippets = (await codeSnippetInfoProvider.Get()
                        .Source(x =>
                        {
                            x.InnerJoin<ChannelInfo>(
                                nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetChannelID),
                                nameof(ChannelInfo.ChannelID));

                            x.InnerJoin<WebsiteChannelInfo>(
                                nameof(ChannelInfo.ChannelID),
                                nameof(WebsiteChannelInfo.WebsiteChannelChannelID));

                            x.InnerJoin<ConsentInfo>(
                                $"cms_channelcodesnippet.{nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetConsentID)}",
                                nameof(ConsentInfo.ConsentID));
                        })
                        .WhereEquals(nameof(WebsiteChannelInfo.WebsiteChannelID), channelContext.WebsiteChannelID)
                        //.Columns(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetLocation),
                        //    nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetCode),
                        //    nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetConsentID),
                        //    nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetGTMID),
                        //    nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetID))
                        .GetEnumerableTypedResultAsync(r =>
                        {
                            var dataContainer = new DataRecordContainer(r);
                            var consent = ConsentInfo.New(dataContainer);
                            var snippet = ChannelCodeSnippetInfo.New(dataContainer);

                            return (snippet, consent);
                        }))
                    .Where(r => consentAgreementService.IsAgreed(currentContact, r.consent))
                    .SelectMany(r => CreateCodeSnipped(r.snippet))
                    .ToLookup(r => r.Location);

                return snippets;
            }
        }

        private static IEnumerable<ChannelCodeSnippetDto> CreateCodeSnipped(ChannelCodeSnippetInfo c)
        {
            if (string.IsNullOrEmpty(c.ChannelCodeSnippetGTMID))
            {
                yield return new ChannelCodeSnippetDto
                {
                    ID = c.ChannelCodeSnippetID,
                    Code = CodeSnippetHelper.AddSnippetIds(
                        c.ChannelCodeSnippetID,
                        c.ChannelCodeSnippetConsentID,
                        c.ChannelCodeSnippetCode),
                    Location = Enum.TryParse(c.ChannelCodeSnippetLocation, out CodeSnippetLocations location)
                        ? location
                        : throw new InvalidOperationException(),
                    Consent = c.ChannelCodeSnippetConsentID
                };

                yield break;
            }

            yield return new ChannelCodeSnippetDto
            {
                ID = c.ChannelCodeSnippetID,
                Code = CodeSnippetHelper.AddSnippetIds(
                    c.ChannelCodeSnippetID,
                    c.ChannelCodeSnippetConsentID,
                    CodeSnippetHelper.GenerateGtmHeadScript(c.ChannelCodeSnippetGTMID)),
                Location = CodeSnippetLocations.HeadBottom,
                Consent = c.ChannelCodeSnippetConsentID
            };

            yield return new ChannelCodeSnippetDto
            {
                ID = c.ChannelCodeSnippetID,
                Code = CodeSnippetHelper.AddSnippetIds(
                    c.ChannelCodeSnippetID,
                    c.ChannelCodeSnippetConsentID,
                    CodeSnippetHelper.GenerateGtmBodyScript(c.ChannelCodeSnippetGTMID)),
                Location = CodeSnippetLocations.BodyTop,
                Consent = c.ChannelCodeSnippetConsentID
            };
        }
    }
}
