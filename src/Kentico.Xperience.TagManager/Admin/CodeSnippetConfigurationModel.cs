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

    [DropDownComponent(Label = "Kentico administration Display Mode", Options = CodeSnippetExtensions.DisplayModeFormComponentOptions, Order = 7)]
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
    }
}

internal static class CodeSnippetExtensions
{
    public const string LocationFormComponentOptions = $"{nameof(CodeSnippetLocations.HeadTop)};Insert at the top of the head\r\n" +
                                               $"{nameof(CodeSnippetLocations.HeadBottom)};Insert at the bottom of the head\r\n" +
                                               $"{nameof(CodeSnippetLocations.BodyTop)};Insert at the top of the body\r\n" +
                                               $"{nameof(CodeSnippetLocations.BodyBottom)};Insert at the bottom of the body\r\n";

    public const string DisplayModeFormComponentOptions = $"{nameof(CodeSnippetAdministrationDisplayMode.None)};Do not display in Administration\r\n" +
                                                $"{nameof(CodeSnippetAdministrationDisplayMode.PreviewOnly)};Display in the Preview only\r\n" +
                                                $"{nameof(CodeSnippetAdministrationDisplayMode.PageBuilderOnly)};Display in the PageBuilder only\r\n" +
                                                $"{nameof(CodeSnippetAdministrationDisplayMode.Both)};Display in Preview and Pagebuilder";
}
