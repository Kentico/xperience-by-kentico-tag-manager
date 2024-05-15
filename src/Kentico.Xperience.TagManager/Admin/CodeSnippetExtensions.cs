using Kentico.Xperience.TagManager.Rendering;

namespace Kentico.Xperience.TagManager.Admin;

internal static class CodeSnippetExtensions
{
    public const string LocationFormComponentOptions = $"{nameof(CodeSnippetLocations.HeadTop)};Insert at the top of the head\r\n" +
                                               $"{nameof(CodeSnippetLocations.HeadBottom)};Insert at the bottom of the head\r\n" +
                                               $"{nameof(CodeSnippetLocations.BodyTop)};Insert at the top of the body\r\n" +
                                               $"{nameof(CodeSnippetLocations.BodyBottom)};Insert at the bottom of the body\r\n";

    public const string DisplayModeFormComponentOptions = $"{nameof(CodeSnippetAdministrationDisplayMode.None)};Do not display in Administration\r\n" +
                                                $"{nameof(CodeSnippetAdministrationDisplayMode.PreviewOnly)};Display in the Preview view mode only\r\n" +
                                                $"{nameof(CodeSnippetAdministrationDisplayMode.PageBuilderOnly)};Display in the Page Builder only\r\n" +
                                                $"{nameof(CodeSnippetAdministrationDisplayMode.Both)}Display in the Preview view mode and the Page Builder";
}
