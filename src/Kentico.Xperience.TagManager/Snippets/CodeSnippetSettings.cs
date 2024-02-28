namespace Kentico.Xperience.TagManager.Snippets;

public class CodeSnippetSettings
{
    public string TagTypeName { get; private set; }
    public string TagDisplayName { get; private set; }
    public bool HasIdentifier { get; private set; }
    public bool HasCustomCode { get; private set; }
    public CodeSnippetSettings(string tagTypeName, string tagDisplayName, bool hasCustomCode = false, bool hasIdentifier = true)
    {
        TagTypeName = tagTypeName;
        TagDisplayName = tagDisplayName;
        HasIdentifier = hasIdentifier;
        HasCustomCode = hasCustomCode;
    }
}
