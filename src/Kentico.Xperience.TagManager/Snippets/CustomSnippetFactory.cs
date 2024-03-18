namespace Kentico.Xperience.TagManager.Snippets;

internal class CustomSnippetFactory : AbstractSnippetFactory
{
    private const string TAG_APPSETTINGS_NAME = "Kentico.Custom";
    public const string TAG_TYPE_NAME = "CustomTag";
    private const string TAG_DISPLAY_NAME = "Custom Snippet";
    private const string TAG_SVG_ICON = "<?xml version=\"1.0\" encoding=\"utf-8\"?><!-- Uploaded to: SVG Repo, www.svgrepo.com, Generator: SVG Repo Mixer Tools -->\r\n<svg width=\"40\" height=\"30\" viewBox=\"0 0 24 24\" id=\"code_snippet\" data-name=\"code snippet\" xmlns=\"http://www.w3.org/2000/svg\">\r\n  <rect id=\"Rectangle\" width=\"24\" height=\"24\" fill=\"none\"/>\r\n  <path id=\"Rectangle-2\" data-name=\"Rectangle\" d=\"M0,6.586V0H6.586\" transform=\"translate(2.343 12) rotate(-45)\" fill=\"none\" stroke=\"#000000\" stroke-miterlimit=\"10\" stroke-width=\"1.5\"/>\r\n  <path id=\"Line\" d=\"M4.659,0,0,17.387\" transform=\"translate(9.671 3.307)\" fill=\"none\" stroke=\"#000000\" stroke-linecap=\"square\" stroke-miterlimit=\"10\" stroke-width=\"1.5\"/>\r\n  <path id=\"Rectangle-3\" data-name=\"Rectangle\" d=\"M0,6.586V0H6.586\" transform=\"translate(21.657 12) rotate(135)\" fill=\"none\" stroke=\"#000000\" stroke-miterlimit=\"10\" stroke-width=\"1.5\"/>\r\n</svg>";
    public override CodeSnippetSettings CreateCodeSnippetSettings() =>
        new(TAG_TYPE_NAME, TAG_DISPLAY_NAME, TAG_APPSETTINGS_NAME, TAG_SVG_ICON);
}
