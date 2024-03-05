namespace Kentico.Xperience.TagManager.Snippets;

internal class CustomSnippetFactory : AbstractSnippetFactory
{
    private const string TAG_APPSETTINGS_NAME = "Kentico.Custom";
    public const string TAG_TYPE_NAME = "CustomTag";
    private const string TAG_DISPLAY_NAME = "Custom Snippet";

    public override CodeSnippetSettings CreateCodeSnippetSettings() =>
        new(TAG_TYPE_NAME, TAG_DISPLAY_NAME, TAG_APPSETTINGS_NAME);
}
