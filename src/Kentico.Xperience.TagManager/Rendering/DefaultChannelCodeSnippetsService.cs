﻿using System.Text.RegularExpressions;

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
    private readonly IProgressiveCache cache;

    public DefaultChannelCodeSnippetsService(
        IConsentAgreementService consentAgreementService,
        IWebsiteChannelContext channelContext,
        IInfoProvider<ChannelCodeSnippetItemInfo> codeSnippetInfoProvider,
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
                    .WhereIn(nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemType), SnippetFactoryStore.GetRegisteredSnippetFactoryTypes().ToArray())
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
