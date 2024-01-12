namespace Kentico.Xperience.TagManager.Enums;

public enum CodeSnippetLocations
{
    HeadTop = 0,
    HeadBottom,
    BodyTop,
    BodyBottom
}

internal static class CodeSnippetLocationsExtensions
{
    public const string FormComponentOptions = $"{nameof(CodeSnippetLocations.HeadTop)};Insert at the top of the head\r\n" +
                                               $"{nameof(CodeSnippetLocations.HeadBottom)};Insert at the bottom of the head\r\n" +
                                               $"{nameof(CodeSnippetLocations.BodyTop)};Insert at the top of the body\r\n" +
                                               $"{nameof(CodeSnippetLocations.BodyBottom)};Insert at the bottom of the body\r\n";
}
