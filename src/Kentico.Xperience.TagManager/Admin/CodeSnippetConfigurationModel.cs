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
    [TextInputComponent(Label = "Code name", Order = 0)]
    public string CodeName { get; set; } = "";

    [RequiredValidationRule]
    [ObjectIdSelectorComponent(objectType: ChannelInfo.OBJECT_TYPE, Label = "Channel", Order = 1, WhereConditionProviderType = typeof(ChannelSelectorWhereConditionProvider))]
    public IEnumerable<int> ChannelIDs { get; set; } = [];

    [CheckBoxComponent(Order = 2, Label = "Is Custom Code Snippet")]
    public bool IsCustomCode { get; set; }

    [RequiredValidationRule]
    [TagManagerSnippetTypeDropdownComponent(Label = "Code snippet type", Order = 3)]
    [VisibleIfFalse(nameof(IsCustomCode))]
    public TagManagerSnippet? SnippetType { get; set; }

    [CodeEditorComponent(Label = "Code", Order = 4)]
    [VisibleIfTrue(nameof(IsCustomCode))]
    public string? Code { get; set; }

    [RadioGroupComponent(Label = "Code snippet location", Order = 5, Options = CodeSnippetLocationsExtensions.FormComponentOptions)]
    [VisibleIfTrue(nameof(IsCustomCode))]
    public string? Location { get; set; }

    [TextInputComponent(Label = "Tag ID", Order = 4)]
    [VisibleIfFalse(nameof(IsCustomCode))]
    public string? TagIdentifier { get; set; }

    [ObjectIdSelectorComponent(objectType: ConsentInfo.OBJECT_TYPE, Label = "Consent", Order = 6, Placeholder = "No consent needed")]
    public IEnumerable<int> ConsentIDs { get; set; } = [];

    public void MapToChannelCodeSnippetInfo(ChannelCodeSnippetItemInfo info)
    {
        info.ChannelCodeSnippetItemChannelId = ChannelIDs.FirstOrDefault();
        info.ChannelCodeSnippetItemConsentId = ConsentIDs.FirstOrDefault();
        info.ChannelCodeSnippetItemLocation = Location;
        info.ChannelCodeSnippetItemType = SnippetType?.TypeName;
        info.ChannelCodeSnippetItemName = CodeName;
        info.ChannelCodeSnippetItemIdentifier = TagIdentifier;
        info.ChannelCodeSnippetItemCode = Code;

        if (IsCustomCode)
        {
            info.ChannelCodeSnippetItemType = CustomSnippetFactory.TAG_TYPE_NAME;
        }
    }
}

internal static class CodeSnippetLocationsExtensions
{
    public const string FormComponentOptions = $"{nameof(CodeSnippetLocations.HeadTop)};Insert at the top of the head\r\n" +
                                               $"{nameof(CodeSnippetLocations.HeadBottom)};Insert at the bottom of the head\r\n" +
                                               $"{nameof(CodeSnippetLocations.BodyTop)};Insert at the top of the body\r\n" +
                                               $"{nameof(CodeSnippetLocations.BodyBottom)};Insert at the bottom of the body\r\n";
}
