using System.Text.RegularExpressions;
using CMS.Base;
using CMS.ContactManagement;
using CMS.ContentEngine;
using CMS.DataProtection;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;

namespace Kentico.Xperience.TagManager.Rendering;

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

    public Task<ILookup<CodeSnippetLocations, ChannelCodeSnippetDto>> GetConsentedCodeSnippets(ContactInfo? contact)
    {
        return cache.LoadAsync(s =>
        {
            s.GetCacheDependency = () =>
                CacheHelper.GetCacheDependency(
                    [
                        $"{ChannelCodeSnippetInfo.OBJECT_TYPE}|all",
                        $"{ChannelInfo.OBJECT_TYPE}|all",
                        $"{WebsiteChannelInfo.OBJECT_TYPE}|all",

                        $"{ContactInfo.OBJECT_TYPE}|byid|{contact?.ContactID}|children|{ConsentAgreementInfo.OBJECT_TYPE}",
                    ]);

            return GetCodeSnippetsInternal();
        }, new CacheSettings(CacheHelper.CacheMinutes(), $"{nameof(ChannelCodeSnippetsService)}.{nameof(GetConsentedCodeSnippets)}|{contact?.ContactID}"));

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

                        x.LeftJoin<ConsentInfo>(
                            $"kenticotagmanager_channelcodesnippet.{nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetConsentID)}",
                            nameof(ConsentInfo.ConsentID));
                    })
                    .WhereEquals(nameof(WebsiteChannelInfo.WebsiteChannelID), channelContext.WebsiteChannelID)
                    .Columns(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetLocation),
                        nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetCode),
                        nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetConsentID),
                        nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetGTMID),
                        nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetID),
                        nameof(ConsentInfo.ConsentID))
                    .GetEnumerableTypedResultAsync(r =>
                    {
                        var dataContainer = new DataRecordContainer(r);
                        var consent = dataContainer[nameof(ConsentInfo.ConsentID)] is > 0 ? ConsentInfo.New(dataContainer) : null;
                        var snippet = ChannelCodeSnippetInfo.New(dataContainer);

                        return (snippet, consent);
                    }))
                .Where(r => r.consent is null || (contact is not null && consentAgreementService.IsAgreed(contact, r.consent)))
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
                Code = AddSnippetIds(
                    c.ChannelCodeSnippetID,
                    c.ChannelCodeSnippetCode),
                Location = Enum.TryParse(c.ChannelCodeSnippetLocation, out CodeSnippetLocations location)
                    ? location
                    : throw new InvalidOperationException(),
            };

            yield break;
        }

        yield return new ChannelCodeSnippetDto
        {
            ID = c.ChannelCodeSnippetID,
            Code = AddSnippetIds(
                c.ChannelCodeSnippetID,
                GenerateGtmHeadScript(c.ChannelCodeSnippetGTMID)),
            Location = CodeSnippetLocations.HeadBottom,
        };

        yield return new ChannelCodeSnippetDto
        {
            ID = c.ChannelCodeSnippetID,
            Code = AddSnippetIds(
                c.ChannelCodeSnippetID,
                GenerateGtmBodyScript(c.ChannelCodeSnippetGTMID)),
            Location = CodeSnippetLocations.BodyTop,
        };
    }

    public static string AddSnippetIds(int codeSnippetId, string codeSnippet) =>
        Regex.Replace(codeSnippet, "<([^\\/]*?)>", $"""<$1 data-snippet-id="{codeSnippetId}">""");

    public static string GenerateGtmHeadScript(string gtmId) =>
        $$"""
          <script>
              (function (w, d, s, l, i) {
                  w[l] = w[l] || [];
                  w[l].push({
                      'gtm.start':
                          new Date().getTime(),
                      event: 'gtm.js'
                  });
                  var f = d.getElementsByTagName(s)[0],
                      j = d.createElement(s),
                      dl = l != 'dataLayer' ? '&l=' + l : '';
                  j.async = true;
                  j.src =
                      'https://www.googletagmanager.com/gtm.js?id=' + i + dl;
                  var n = d.querySelector('[nonce]');
                  n && j.setAttribute('nonce', n.nonce || n.getAttribute('nonce'));
                  f.parentNode.insertBefore(j, f);
              })(window, document, 'script', 'dataLayer','{{gtmId}}');
          </script>
          """;

    public static string GenerateGtmBodyScript(string gtmId) =>
        $"""
         <noscript>
             <iframe src="https://www.googletagmanager.com/ns.html?id={gtmId}"
                     height="0"
                     width="0"
                     style="display:none;visibility:hidden"
                     title="GTMNoScript">
             </iframe>
         </noscript>
         """;
}
