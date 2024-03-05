using System.Text.RegularExpressions;
using CMS.Base;
using CMS.ContactManagement;
using CMS.ContentEngine;
using CMS.DataProtection;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;
using Kentico.Xperience.TagManager.Admin;
using Kentico.Xperience.TagManager.Snippets;

namespace Kentico.Xperience.TagManager.Rendering;

internal class DefaultChannelCodeSnippetsService : IChannelCodeSnippetsService
{
    private readonly IConsentAgreementService consentAgreementService;
    private readonly IWebsiteChannelContext channelContext;
    private readonly IChannelCodeSnippetItemInfoProvider codeSnippetInfoProvider;
    private readonly IProgressiveCache cache;

    public DefaultChannelCodeSnippetsService(
        IConsentAgreementService consentAgreementService,
        IWebsiteChannelContext channelContext,
        IChannelCodeSnippetItemInfoProvider codeSnippetInfoProvider,
        IProgressiveCache cache)
    {
        this.consentAgreementService = consentAgreementService;
        this.channelContext = channelContext;
        this.codeSnippetInfoProvider = codeSnippetInfoProvider;
        this.cache = cache;
    }

    public Task<ILookup<CodeSnippetLocations, CodeSnippetDto>> GetConsentedCodeSnippets(ContactInfo? contact)
    {
        return cache.LoadAsync(s =>
        {
            s.GetCacheDependency = () =>
                CacheHelper.GetCacheDependency(
                    [
                        $"{ChannelCodeSnippetItemInfo.OBJECT_TYPE}|all",
                        $"{ChannelInfo.OBJECT_TYPE}|all",
                        $"{WebsiteChannelInfo.OBJECT_TYPE}|all",
                        $"{ContactInfo.OBJECT_TYPE}|byid|{contact?.ContactID}|children|{ConsentAgreementInfo.OBJECT_TYPE}",
                    ]);

            return GetCodeSnippetsInternal();
        }, new CacheSettings(CacheHelper.CacheMinutes(), $"{nameof(DefaultChannelCodeSnippetsService)}.{nameof(GetConsentedCodeSnippets)}|{contact?.ContactID}"));

        async Task<ILookup<CodeSnippetLocations, CodeSnippetDto>> GetCodeSnippetsInternal()
        {
            var snippets = (await codeSnippetInfoProvider.Get()
                    .Source(x =>
                    {
                        x.InnerJoin<ChannelInfo>(
                            nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemChannelID),
                            nameof(ChannelInfo.ChannelID));

                        x.InnerJoin<WebsiteChannelInfo>(
                            nameof(ChannelInfo.ChannelID),
                            nameof(WebsiteChannelInfo.WebsiteChannelChannelID));

                        x.LeftJoin<ConsentInfo>(
                            $"kenticotagmanager_channelcodesnippet.{nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemConsentID)}",
                            nameof(ConsentInfo.ConsentID));
                    })
                    .WhereEquals(nameof(WebsiteChannelInfo.WebsiteChannelID), channelContext.WebsiteChannelID)
                    .WhereIn(nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemType), SnippetFactoryStore.GetRegisteredSnippetFactoryTypes().ToArray())
                    .Columns(nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemLocation),
                        nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemCode),
                        nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemConsentID),
                        nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemIdentifier),
                        nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemID),
                        nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemType),
                        nameof(ConsentInfo.ConsentID))
                    .GetEnumerableTypedResultAsync(r =>
                    {
                        var dataContainer = new DataRecordContainer(r);
                        var consent = dataContainer[nameof(ConsentInfo.ConsentID)] is > 0 ? ConsentInfo.New(dataContainer) : null;
                        var snippet = ChannelCodeSnippetItemInfo.New(dataContainer);

                        return (snippet, consent);
                    }))
                .Where(r => r.consent is null || (contact is not null && consentAgreementService.IsAgreed(contact, r.consent)))
                .SelectMany(r => CreateCodeSnippet(r.snippet))
                .ToLookup(r => r.Location);

            return snippets;
        }
    }

    private static IEnumerable<CodeSnippetDto> CreateCodeSnippet(ChannelCodeSnippetItemInfo snippetInfo)
    {
        var snippetFactory = SnippetFactoryStore.TryGetSnippetFactory(snippetInfo.ChannelCodeSnippetItemType) ??
           throw new InvalidOperationException("Specified snippet is not registered.");

        var snippetSettings = snippetFactory.CreateCodeSnippetSettings();

        var tags = new List<CodeSnippetDto>();

        if (!string.IsNullOrEmpty(snippetInfo.ChannelCodeSnippetItemIdentifier))
        {
            tags.AddRange(snippetFactory.CreateCodeSnippets(snippetInfo.ChannelCodeSnippetItemIdentifier).Select(x => new CodeSnippetDto
            {
                Location = x.Location,
                Code = x.Code,
                ID = snippetInfo.ChannelCodeSnippetItemID
            }));
        }

        if (!string.IsNullOrEmpty(snippetInfo.ChannelCodeSnippetItemCode) && snippetSettings.TagTypeName == "Custom")
        {
            var tag = AdjustCustomCodeSnippet(new CodeSnippetDto
            {
                ID = snippetInfo.ChannelCodeSnippetItemID,
                Code = snippetInfo.ChannelCodeSnippetItemCode,
                Location = Enum.TryParse(snippetInfo.ChannelCodeSnippetItemLocation, out CodeSnippetLocations location)
                   ? location
                   : throw new InvalidOperationException("Invalid Channel Code Snippet Location."),
            });

            tags.Add(tag);
        }

        return tags;
    }

    private static CodeSnippetDto AdjustCustomCodeSnippet(CodeSnippetDto codeSnippet) =>
      new()
      {
          Code = codeSnippet.Code != null ? AddSnippetIds(codeSnippet.ID, codeSnippet.Code!) : codeSnippet.Code,
          ID = codeSnippet.ID,
          Location = codeSnippet.Location
      };

    private static string AddSnippetIds(int codeSnippetId, string codeSnippet) =>
      Regex.Replace(codeSnippet, "<([^\\/]*?)>", $"""<$1 data-snippet-id="{codeSnippetId}">""");
}
