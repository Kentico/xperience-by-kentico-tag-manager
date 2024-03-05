namespace Kentico.Xperience.TagManager.Snippets;

public class CodeSnippetSettings
{
    public string TagAppSettingsName { get; private set; }
    public string TagTypeName { get; private set; }
    public string TagDisplayName { get; private set; }
    public CodeSnippetSettings(string tagTypeName, string tagDisplayName, string tagAppSettingsName)
    {
        TagTypeName = tagTypeName;
        TagDisplayName = tagDisplayName;
        TagAppSettingsName = tagAppSettingsName;
    }
}
