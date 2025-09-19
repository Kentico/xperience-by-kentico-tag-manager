using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.TagManager.Admin.Components;
using Kentico.Xperience.TagManager.Snippets;

[assembly: RegisterFormComponent(
    identifier: TagManagerSnippetTypeDropdownComponent.IDENTIFIER,
    componentType: typeof(TagManagerSnippetTypeDropdownComponent),
    name: "Tag Manager Snippet Type Dropdown")]

namespace Kentico.Xperience.TagManager.Admin.Components;

#pragma warning disable S2094 // intentionally empty class
public class TagManagerSnippetTypeDropdownComponentProperties : FormComponentProperties
{
}
#pragma warning restore

public class TagManagerSnippetTypeDropdownComponentClientProperties : FormComponentClientProperties<string>
{
    public IEnumerable<TagManagerSnippetDto>? SnippetTypes { get; set; }
}

public sealed class TagManagerSnippetTypeDropdownComponentAttribute : FormComponentAttribute
{
}

[ComponentAttribute(typeof(TagManagerSnippetTypeDropdownComponentAttribute))]
public class TagManagerSnippetTypeDropdownComponent : FormComponent<TagManagerSnippetTypeDropdownComponentProperties, TagManagerSnippetTypeDropdownComponentClientProperties, string>
{
    public const string IDENTIFIER = "kentico.xperience-integrations-tag-manager.tag-manager-snippet-type-dropdown";
    public override string ClientComponentName => "@kentico/xperience-integrations-tagmanager/TagManagerSnippetTypeDropdown";

    public override string GetValue() => Value ?? "";
    public override void SetValue(string value) => Value = value;
    internal string? Value { get; set; }
    protected override async Task ConfigureClientProperties(TagManagerSnippetTypeDropdownComponentClientProperties properties)
    {
        properties.Value = Value ?? "";

        properties.SnippetTypes = SnippetFactoryStore.GetRegisteredSnippetFactories()
            .Select(x => x.CreateCodeSnippetSettings())
            .Select(x => new TagManagerSnippetDto
            {
                DisplayName = x.TagDisplayName,
                TypeName = x.TagTypeName,
                Icon = x.TagSVGIconCode
            });

        await base.ConfigureClientProperties(properties);
    }
}

public class TagManagerSnippetDto
{
    public string DisplayName { get; set; } = "";
    public string TypeName { get; set; } = "";
    public string? Icon { get; set; }
}

