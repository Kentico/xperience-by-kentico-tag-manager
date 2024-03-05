using Kentico.Xperience.TagManager.Rendering;
using Kentico.Xperience.TagManager.Snippets;

namespace DancingGoat.TagManager;

public class DancingGoatSnippetFactory : AbstractSnippetFactory
{
    private const string TAG_APPSETTINGS_NAME = "Custom.DancingGoatSnippet";
    private const string TAG_TYPE_NAME = "DancingGoatCustomScript";
    private const string TAG_DISPLAY_NAME = "Dancing Goat Custom Script";

    public override CodeSnippetSettings CreateCodeSnippetSettings() =>
        new(TAG_TYPE_NAME, TAG_DISPLAY_NAME, TAG_APPSETTINGS_NAME);

    public override IEnumerable<CodeSnippet> CreateCodeSnippets(string thirdPartyIdentifier) =>
        new List<CodeSnippet>
        {
            new (GenerateScript(), CodeSnippetLocations.HeadTop),
        };

    private static string GenerateScript() =>
        $"""
            <script>
                alert("Custom script message!")
            </script>
        """;
}
