using Kentico.Xperience.TagManager.Rendering;

namespace Kentico.Xperience.TagManager.Snippets;

internal class GoogleTagManagerSnippetFactory : AbstractSnippetFactory
{
    private const string TAG_APPSETTINGS_NAME = "Kentico.GoogleTagManager";
    private const string TAG_TYPE_NAME = "GoogleTagManager";
    private const string TAG_DISPLAY_NAME = "Google Tag Manager";
    private const string TAG_ICON_SVG = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<svg width=\"40px\" height=\"30px\" viewBox=\"0 0 256 256\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" preserveAspectRatio=\"xMidYMid\">\r\n    <g>\r\n        <polygon fill=\"#8AB4F8\" points=\"150.261818 245.516364 105.825455 202.185455 201.258182 104.730909 247.265455 149.821818\">\r\n\r\n</polygon>\r\n        <path d=\"M150.450909,53.9381818 L106.174545,8.73090909 L9.36,104.629091 C-3.12,117.109091 -3.12,137.341818 9.36,149.836364 L104.72,245.821818 L149.810909,203.64 L77.1563636,127.232727 L150.450909,53.9381818 Z\" fill=\"#4285F4\">\r\n\r\n</path>\r\n        <path d=\"M246.625455,105.370909 L150.625455,9.37090909 C138.130909,-3.12363636 117.869091,-3.12363636 105.374545,9.37090909 C92.88,21.8654545 92.88,42.1272727 105.374545,54.6218182 L201.374545,150.621818 C213.869091,163.116364 234.130909,163.116364 246.625455,150.621818 C259.12,138.127273 259.12,117.865455 246.625455,105.370909 Z\" fill=\"#8AB4F8\">\r\n\r\n</path>\r\n        <circle fill=\"#246FDB\" cx=\"127.265455\" cy=\"224.730909\" r=\"31.2727273\">\r\n\r\n</circle>\r\n    </g>\r\n</svg>";

    public override CodeSnippetSettings CreateCodeSnippetSettings() =>
        new(TAG_TYPE_NAME, TAG_DISPLAY_NAME, TAG_APPSETTINGS_NAME, TAG_ICON_SVG);

    public override IEnumerable<CodeSnippet> CreateCodeSnippets(string thirdPartyIdentifier) =>
        [
            new (GenerateGtmHeadScript(thirdPartyIdentifier), CodeSnippetLocations.HeadBottom),
            new (GenerateGtmBodyScript(thirdPartyIdentifier), CodeSnippetLocations.BodyTop)
        ];

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

    private static string GenerateGtmBodyScript(string gtmId)
    {
        string gtmCode = @"
             <noscript>
                 <iframe src='https://www.googletagmanager.com/ns.html?id={0}'
                         height='0'
                         width='0'
                         style='display:none;visibility:hidden'
                         title='GTMNoScript'>
                 </iframe>
             </noscript>
            ";

        return string.Format(gtmCode, gtmId);
    }
}
