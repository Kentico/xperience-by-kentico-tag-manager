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

public class TagManagerSnippetTypeDropdownComponentClientProperties : FormComponentClientProperties<TagManagerSnippet>
{
    public IEnumerable<TagManagerSnippet>? SnippetTypes { get; set; }
}

public sealed class TagManagerSnippetTypeDropdownComponentAttribute : FormComponentAttribute
{
}

[ComponentAttribute(typeof(TagManagerSnippetTypeDropdownComponentAttribute))]
public class TagManagerSnippetTypeDropdownComponent : FormComponent<TagManagerSnippetTypeDropdownComponentProperties, TagManagerSnippetTypeDropdownComponentClientProperties, TagManagerSnippet>
{
    public const string IDENTIFIER = "kentico.xperience-integrations-tag-manager.tag-manager-snippet-type-dropdown";
    public override string ClientComponentName => "@kentico/xperience-integrations-tagmanager/TagManagerSnippetTypeDropdown";

    public override TagManagerSnippet GetValue() => Value ?? new();
    public override void SetValue(TagManagerSnippet value) => Value = value;
    internal TagManagerSnippet? Value { get; set; }
    protected override async Task ConfigureClientProperties(TagManagerSnippetTypeDropdownComponentClientProperties properties)
    {
        properties.Value = Value ?? new();

        properties.SnippetTypes = SnippetFactoryStore.GetRegisteredSnippetFactories()
            .Select(x => x.CreateCodeSnippetSettings())
            .Where(x => x.TagTypeName != CustomSnippetFactory.TAG_TYPE_NAME)
            .Select(x => new TagManagerSnippet
            {
                DisplayName = x.TagDisplayName,
                TypeName = x.TagTypeName,
                Icon = x.TagSVGIconCode
            });

        await base.ConfigureClientProperties(properties);
    }
}

public class TagManagerSnippet
{
    public string DisplayName { get; set; } = "";
    public string TypeName { get; set; } = "";
    public string? Icon { get; set; }

    public static TagManagerSnippet FromSnippetStore(string typeName)
    {
        var factorySettings = SnippetFactoryStore.TryGetSnippetFactory(typeName)?.CreateCodeSnippetSettings();

        return new TagManagerSnippet
        {
            DisplayName = factorySettings?.TagDisplayName ?? "",
            TypeName = factorySettings?.TagTypeName ?? "",
            Icon = factorySettings?.TagSVGIconCode
        };
    }
}

