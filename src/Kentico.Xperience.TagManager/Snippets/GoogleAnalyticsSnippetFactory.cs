using Kentico.Xperience.TagManager.Rendering;

namespace Kentico.Xperience.TagManager.Snippets;

internal class GoogleAnalyticsSnippetFactory : AbstractSnippetFactory
{
    private const string TAG_APPSETTINGS_NAME = "Kentico.GoogleAnalytics4";
    private const string TAG_TYPE_NAME = "GoogleAnalytics4";
    private const string TAG_DISPLAY_NAME = "Google Analytics 4";
    private const string SVG_ICON_CODE = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"40\" height=\"30\" viewBox=\"0 0 64 64\"><g transform=\"matrix(.363638 0 0 .363636 -3.272763 -2.909091)\"><path d=\"M130 29v132c0 14.77 10.2 23 21 23 10 0 21-7 21-23V30c0-13.54-10-22-21-22s-21 9.33-21 21z\" fill=\"#f9ab00\"/><g fill=\"#e37400\"><path d=\"M75 96v65c0 14.77 10.2 23 21 23 10 0 21-7 21-23V97c0-13.54-10-22-21-22s-21 9.33-21 21z\"/><circle cx=\"41\" cy=\"163\" r=\"21\"/></g></g></svg>\r\n";

    public override CodeSnippetSettings CreateCodeSnippetSettings() =>
        new(TAG_TYPE_NAME, TAG_DISPLAY_NAME, TAG_APPSETTINGS_NAME, SVG_ICON_CODE);

    public override IEnumerable<CodeSnippet> CreateCodeSnippets(string thirdPartyIdentifier) =>
        [
            new (GenerateScript(thirdPartyIdentifier), CodeSnippetLocations.HeadBottom),
        ];

    private static string GenerateScript(string identifier) =>
      $$"""
        <!-- Google tag (gtag.js) -->
        <script async src="https://www.googletagmanager.com/gtag/js?id={{identifier}}"></script>
        <script>
            window.dataLayer = window.dataLayer || [];
            function gtag(){dataLayer.push(arguments);}
            gtag('js', new Date());

            gtag('config', '{{identifier}}');
        </script>
      """;
}
