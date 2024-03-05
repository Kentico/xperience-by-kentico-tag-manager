using Kentico.Xperience.TagManager.Rendering;

namespace Kentico.Xperience.TagManager.Snippets;

internal class GoogleAnalyticsSnippetFactory : AbstractSnippetFactory
{
    private const string TAG_TYPE_NAME = "GoogleAnalytics4";
    private const string TAG_DISPLAY_NAME = "Google Analytics 4";

    public override CodeSnippetSettings CreateCodeSnippetSettings() =>
        new(TAG_TYPE_NAME, TAG_DISPLAY_NAME);

    public override IEnumerable<CodeSnippet> CreateCodeSnippets(string thirdPartyIdentifier) =>
        new List<CodeSnippet>
        {
            new (GenerateScript(thirdPartyIdentifier), CodeSnippetLocations.HeadBottom),
        };

    private static string GenerateScript(string identifier) =>
      $$"""
        <!-- Google tag (gtag.js) -->
        <script async src="https://www.googletagmanager.com/gtag/js?id=G-WT7J3QS9E8"></script>
        <script>
            window.dataLayer = window.dataLayer || [];
            function gtag(){dataLayer.push(arguments);}
            gtag('js', new Date());

            gtag('config', '{{identifier}}');
        </script>
      """;
}
