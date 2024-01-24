using CMS.ContentEngine;
using CMS.DataProtection;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Kentico.Xperience.TagManager.Admin;

internal class CodeSnippetEditModel
{
    [RequiredValidationRule]
    [TextInputComponent(Label = "Code name", Order = 0)]
    public string CodeName { get; set; } = "";

    [RequiredValidationRule]
    [ObjectIdSelectorComponent(objectType: ChannelInfo.OBJECT_TYPE, Label = "Channel", Order = 1, WhereConditionProviderType = typeof(ChannelSelectorWhereConditionProvider))]
    public IEnumerable<int> ChannelIDs { get; set; } = [];

    [RequiredValidationRule]
    [DropDownComponent(Label = "Code snippet type", Order = 2, DataProviderType = typeof(CodeSnippetTypesDropdownOptionsProvider))]
    public string? SnippetType { get; set; }

    [CodeEditorComponent(Label = "Code", Order = 3)]
    [VisibleIfEqualTo(nameof(SnippetType), nameof(CodeSnippetTypes.CustomCode))]
    public string? Code { get; set; }

    [RadioGroupComponent(Label = "Code snippet location", Order = 4, Options = CodeSnippetLocationsExtensions.FormComponentOptions)]
    [VisibleIfEqualTo(nameof(SnippetType), nameof(CodeSnippetTypes.CustomCode))]
    public string? Location { get; set; }

    [TextInputComponent(Label = "Google Tag Manager ID", Order = 3)]
    [VisibleIfEqualTo(nameof(SnippetType), nameof(CodeSnippetTypes.GTM))]
    public string? GTMID { get; set; }

    [ObjectIdSelectorComponent(objectType: ConsentInfo.OBJECT_TYPE, Label = "Consent", Order = 5, Placeholder = "{$customchannelsettings.codesnippets.noconsentneeded$}")]
    public IEnumerable<int> ConsentIDs { get; set; } = [];

    public void MapToChannelCodeSnippetInfo(ChannelCodeSnippetInfo info)
    {
        info.ChannelCodeSnippetChannelID = ChannelIDs.FirstOrDefault();
        info.ChannelCodeSnippetConsentID = ConsentIDs.FirstOrDefault();
        info.ChannelCodeSnippetLocation = Location;
        info.ChannelCodeSnippetType = SnippetType;
        info.ChannelCodeSnippetName = CodeName;

        switch (SnippetType)
        {
            case nameof(CodeSnippetTypes.GTM):
                info.ChannelCodeSnippetGTMID = GTMID;
                info.ChannelCodeSnippetCode = null;
                break;
            case nameof(CodeSnippetTypes.CustomCode):
                info.ChannelCodeSnippetGTMID = null;
                info.ChannelCodeSnippetCode = Code;
                break;
            default:
                break;
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
