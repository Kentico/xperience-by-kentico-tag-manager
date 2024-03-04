namespace Kentico.Xperience.TagManager.Snippets;

public class CustomSnippetFactory : AbstractSnippetFactory
{
    public const string TAG_TYPE_NAME = "CustomTag";
    private const string TAG_DISPLAY_NAME = "Custom Snippet";

    public override CodeSnippetSettings CreateCodeSnippetSettings() =>
        new(TAG_TYPE_NAME, TAG_DISPLAY_NAME);
}
