using CMS.ContentEngine;
using CMS.DataProtection;

using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.TagManager.Admin.Components;
using Kentico.Xperience.TagManager.Rendering;
using Kentico.Xperience.TagManager.Snippets;

namespace Kentico.Xperience.TagManager.Admin;

internal class CodeSnippetConfigurationModel
{
    [RequiredValidationRule]
    [TextInputComponent(Label = "Name", Order = 0)]
    public string Name { get; set; } = "";

    [RequiredValidationRule]
    [ObjectIdSelectorComponent(objectType: ChannelInfo.OBJECT_TYPE, Label = "Channel", Order = 1, WhereConditionProviderType = typeof(ChannelSelectorWhereConditionProvider))]
    public IEnumerable<int> ChannelIDs { get; set; } = [];

    [RequiredValidationRule]
    [TagManagerSnippetTypeDropdownComponent(Label = "Tag type", Order = 3)]
    public string? TagType { get; set; }

    [CodeEditorComponent(Label = "Code", Order = 4, ExplanationTextAsHtml = true)]
    [VisibleIfEqualTo(nameof(TagType), CustomSnippetFactory.TAG_TYPE_NAME)]
    public string? Code { get; set; }

    [RadioGroupComponent(Label = "Tag location", Order = 5, Options = CodeSnippetExtensions.LocationFormComponentOptions)]
    [VisibleIfEqualTo(nameof(TagType), CustomSnippetFactory.TAG_TYPE_NAME)]
    public string? Location { get; set; }

    [TextInputComponent(Label = "Tag ID", Order = 4)]
    [VisibleIfNotEqualTo(nameof(TagType), CustomSnippetFactory.TAG_TYPE_NAME)]
    public string? TagIdentifier { get; set; }

    [ObjectIdSelectorComponent(objectType: ConsentInfo.OBJECT_TYPE, Label = "Consent", Order = 6, Placeholder = "No consent needed")]
    public IEnumerable<int> ConsentIDs { get; set; } = [];

    [CheckBoxComponent(Label = "Enable tag rendering", Order = 7)]
    public bool Enable { get; set; } = true;

    [DropDownComponent(Label = "Kentico administration Display Mode", Options = CodeSnippetExtensions.DisplayModeFormComponentOptions, Order = 8)]
    public string DisplayMode { get; set; } = "None";

    public void MapToChannelCodeSnippetInfo(ChannelCodeSnippetItemInfo info)
    {
        info.ChannelCodeSnippetItemChannelId = ChannelIDs.FirstOrDefault();
        info.ChannelCodeSnippetItemConsentId = ConsentIDs.FirstOrDefault();
        info.ChannelCodeSnippetItemLocation = Location;
        info.ChannelCodeSnippetItemType = TagType;
        info.ChannelCodeSnippetItemName = Name;
        info.ChannelCodeSnippetItemIdentifier = TagIdentifier;
        info.ChannelCodeSnippetItemCode = Code;
        info.ChannelCodeSnippetAdministrationDisplayMode = DisplayMode;
        info.ChannelCodeSnippetItemEnable = Enable;
    }
}
