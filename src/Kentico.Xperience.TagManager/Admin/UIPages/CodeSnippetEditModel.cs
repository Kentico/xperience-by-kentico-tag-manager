using CMS.ContentEngine;
using CMS.DataProtection;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.TagManager.Snippets;

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
    [VisibleIfTrue(nameof(ShowCodeField))]
    public string? Code { get; set; }

    [RadioGroupComponent(Label = "Code snippet location", Order = 4, Options = CodeSnippetLocationsExtensions.FormComponentOptions)]
    [VisibleIfTrue(nameof(ShowCodeField))]
    public string? Location { get; set; }

    [TextInputComponent(Label = "Tag ID", Order = 3)]
    [VisibleIfTrue(nameof(ShowIdentifierField))]
    public string? TagIdentifier { get; set; }

    [ObjectIdSelectorComponent(objectType: ConsentInfo.OBJECT_TYPE, Label = "Consent", Order = 5, Placeholder = "{$customchannelsettings.codesnippets.noconsentneeded$}")]
    public IEnumerable<int> ConsentIDs { get; set; } = [];

    private bool ShowIdentifierField { get; set; }
    private bool ShowCodeField { get; set; }

    public void MapToChannelCodeSnippetInfo(ChannelCodeSnippetInfo info)
    {
        info.ChannelCodeSnippetChannelID = ChannelIDs.FirstOrDefault();
        info.ChannelCodeSnippetConsentID = ConsentIDs.FirstOrDefault();
        info.ChannelCodeSnippetLocation = Location;
        info.ChannelCodeSnippetType = SnippetType;
        info.ChannelCodeSnippetName = CodeName;

        var settings = SnippetFactoryStore.GetSnippetFactory(info.ChannelCodeSnippetType!).CreateCodeSnippetSettings();

        ShowCodeField = settings.HasCustomCode;
        ShowIdentifierField = settings.HasIdentifier;
    }
}

internal static class CodeSnippetLocationsExtensions
{
    public const string FormComponentOptions = $"{nameof(CodeSnippetLocations.HeadTop)};Insert at the top of the head\r\n" +
                                               $"{nameof(CodeSnippetLocations.HeadBottom)};Insert at the bottom of the head\r\n" +
                                               $"{nameof(CodeSnippetLocations.BodyTop)};Insert at the top of the body\r\n" +
                                               $"{nameof(CodeSnippetLocations.BodyBottom)};Insert at the bottom of the body\r\n";
}
