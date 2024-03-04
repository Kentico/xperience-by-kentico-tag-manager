namespace Kentico.Xperience.TagManager.Snippets;

public class CodeSnippetSettings
{
    public string TagTypeName { get; private set; }
    public string TagDisplayName { get; private set; }
    public CodeSnippetSettings(string tagTypeName, string tagDisplayName)
    {
        TagTypeName = tagTypeName;
        TagDisplayName = tagDisplayName;
    }
}
