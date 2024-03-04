namespace Kentico.Xperience.TagManager.Snippets;

internal class GoogleTagManagerSnippetFactory : AbstractSnippetFactory
{
    private const string TAG_TYPE_NAME = "GoogleTagManager";
    private const string TAG_DISPLAY_NAME = "Google Tag Manager";

    public override CodeSnippetSettings CreateCodeSnippetSettings() =>
        new(TAG_TYPE_NAME, TAG_DISPLAY_NAME);

    public override IEnumerable<CodeSnippet> CreateCodeSnippets(string thirdPartyIdentifier) =>
        new List<CodeSnippet>
        {
            new (GenerateGtmHeadScript(thirdPartyIdentifier), CodeSnippetLocations.HeadBottom),
            new (GenerateGtmBodyScript(thirdPartyIdentifier), CodeSnippetLocations.BodyTop)
        };

    private static string GenerateGtmHeadScript(string gtmId) =>
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

    private static string GenerateGtmBodyScript(string gtmId) =>
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
