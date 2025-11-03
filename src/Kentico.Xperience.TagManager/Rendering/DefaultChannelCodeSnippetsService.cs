using System.Text.RegularExpressions;

using CMS.Base;
using CMS.ContactManagement;
using CMS.ContentEngine;
using CMS.DataEngine;
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

    private readonly IInfoProvider<ChannelCodeSnippetItemInfo> codeSnippetInfoProvider;
    private readonly IInfoProvider<ChannelCodeSnippetItemContentTypeInfo> contentTypeBindingProvider;
    private readonly IProgressiveCache cache;

    public DefaultChannelCodeSnippetsService(
        IConsentAgreementService consentAgreementService,
        IWebsiteChannelContext channelContext,
        IInfoProvider<ChannelCodeSnippetItemInfo> codeSnippetInfoProvider,
        IInfoProvider<ChannelCodeSnippetItemContentTypeInfo> contentTypeBindingProvider,
        IProgressiveCache cache)
    {
        this.consentAgreementService = consentAgreementService;
        this.channelContext = channelContext;
        this.codeSnippetInfoProvider = codeSnippetInfoProvider;
        this.contentTypeBindingProvider = contentTypeBindingProvider;
        this.cache = cache;
    }

    public Task<ILookup<CodeSnippetLocations, CodeSnippetDto>> GetConsentedCodeSnippets(ContactInfo? contact, int? contentTypeId = null)
    {
        return cache.LoadAsync(s =>
        {
            s.GetCacheDependency = () =>
                CacheHelper.GetCacheDependency(
                    [
                        $"{ChannelCodeSnippetItemInfo.OBJECT_TYPE}|all",
                        $"{ChannelCodeSnippetItemContentTypeInfo.OBJECT_TYPE}|all",
                        $"{ChannelInfo.OBJECT_TYPE}|all",
                        $"{WebsiteChannelInfo.OBJECT_TYPE}|byid|{channelContext.WebsiteChannelID}",
                        $"{ContactInfo.OBJECT_TYPE}|byid|{contact?.ContactID}|children|{ConsentAgreementInfo.OBJECT_TYPE}",
                    ]);
            return GetCodeSnippetsInternal();
        }, new CacheSettings(CacheHelper.CacheMinutes(), $"{nameof(DefaultChannelCodeSnippetsService)}.{nameof(GetConsentedCodeSnippets)}|{contact?.ContactID}|{channelContext.WebsiteChannelID}|{contentTypeId}"));

        async Task<ILookup<CodeSnippetLocations, CodeSnippetDto>> GetCodeSnippetsInternal()
        {
            // Get snippet IDs to include based on content type filtering
            IEnumerable<int>? allowedSnippetIds = null;

            if (contentTypeId.HasValue)
            {
                // Get all snippets that have NO content type bindings (show on all pages)
                var snippetsWithNoBindings = await codeSnippetInfoProvider.Get()
                    .Source(x => x.LeftJoin<ChannelCodeSnippetItemContentTypeInfo>(
                        nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemID),
                        nameof(ChannelCodeSnippetItemContentTypeInfo.ChannelCodeSnippetItemID)))
                    .WhereNull(nameof(ChannelCodeSnippetItemContentTypeInfo.ChannelCodeSnippetItemContentTypeID))
                    .Columns($"{ChannelCodeSnippetItemInfo.TYPEINFO.ObjectClassName.Replace(".", "_")}.{nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemID)}")
                    .GetEnumerableTypedResultAsync();

                // Get snippets bound to the current content type
                var snippetsBoundToContentType = await contentTypeBindingProvider.Get()
                    .WhereEquals(nameof(ChannelCodeSnippetItemContentTypeInfo.ContentTypeID), contentTypeId.Value)
                    .GetEnumerableTypedResultAsync();

                allowedSnippetIds = snippetsWithNoBindings
                    .Select(s => s.ChannelCodeSnippetItemID)
                    .Union(snippetsBoundToContentType.Select(b => b.ChannelCodeSnippetItemID))
                    .ToList();
            }

            var query = codeSnippetInfoProvider.Get()
                    .Source(x =>
                    {
                        x.InnerJoin<ChannelInfo>(
                            nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemChannelId),
                            nameof(ChannelInfo.ChannelID));

                        x.InnerJoin<WebsiteChannelInfo>(
                            nameof(ChannelInfo.ChannelID),
                            nameof(WebsiteChannelInfo.WebsiteChannelChannelID));

                        x.LeftJoin<ConsentInfo>(
                            $"{ChannelCodeSnippetItemInfo.OBJECT_TYPE.Replace('.', '_')}.{nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemConsentId)}",
                            nameof(ConsentInfo.ConsentID));
                    })
                    .WhereTrue(nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemEnable))
                    .WhereEquals(nameof(WebsiteChannelInfo.WebsiteChannelID), channelContext.WebsiteChannelID)
                    .WhereIn(nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemType), SnippetFactoryStore.GetRegisteredSnippetFactoryTypes().ToArray());

            // Apply content type filtering if we have allowed snippet IDs
            if (allowedSnippetIds != null && allowedSnippetIds.Any())
            {
                query = query.WhereIn(nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemID), allowedSnippetIds.ToArray());
            }
            else if (contentTypeId.HasValue)
            {
                // If we have a content type but no allowed snippets, return empty
                return Enumerable.Empty<CodeSnippetDto>().ToLookup(r => r.Location);
            }

            var snippets = (await query
                    .Columns(nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemLocation),
                        nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemCode),
                        nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemConsentId),
                        nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemEnable),
                        nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetAdministrationDisplayMode),
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
           throw new InvalidOperationException("Specified tag is not registered.");

        var snippetSettings = snippetFactory.CreateCodeSnippetSettings();

        var tags = new List<CodeSnippetDto>();

        if (!Enum.TryParse(snippetInfo.ChannelCodeSnippetAdministrationDisplayMode, out CodeSnippetAdministrationDisplayMode displayMode))
        {
            displayMode = CodeSnippetAdministrationDisplayMode.None;
        }

        if (snippetSettings.TagTypeName != CustomSnippetFactory.TAG_TYPE_NAME)
        {
            tags.AddRange(snippetFactory.CreateCodeSnippets(snippetInfo.ChannelCodeSnippetItemIdentifier).Select(x => new CodeSnippetDto
            {
                Location = x.Location,
                Code = x.Code,
                ID = snippetInfo.ChannelCodeSnippetItemID,
                DisplayMode = displayMode
            }));
        }
        else
        {
            var tag = AdjustCustomCodeSnippet(new CodeSnippetDto
            {
                ID = snippetInfo.ChannelCodeSnippetItemID,
                Code = snippetInfo.ChannelCodeSnippetItemCode,
                Location = Enum.TryParse(snippetInfo.ChannelCodeSnippetItemLocation, out CodeSnippetLocations location)
                   ? location
                   : throw new InvalidOperationException("Invalid Channel Tag Location."),
                DisplayMode = displayMode
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
          Location = codeSnippet.Location,
          DisplayMode = codeSnippet.DisplayMode
      };

    private static string AddSnippetIds(int codeSnippetId, string codeSnippet) =>
      Regex.Replace(codeSnippet, "<([^\\/]*?)>", $"""<$1 data-snippet-id="{codeSnippetId}">""");
}
