using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.TagManager.Admin.UIFormComponents;
using Kentico.Xperience.TagManager.Enums;

namespace Kentico.Xperience.TagManager.Models;

internal class CodeSnippetEditModel
{
    [RequiredValidationRule]
    [ObjectIdSelectorComponent(objectType: "cms.channel", Label = "Channel", Order = 1, WhereConditionProviderType = typeof(ChannelSelectorWhereConditionProvider))]
    public IEnumerable<int> ChannelID { get; set; } = [];

    [RequiredValidationRule]
    [DropDownComponent(Label = "Code snippet type", Order = 2, DataProviderType = typeof(CodeSnippetTypesDropdownOptionsProvider))]
    public string? SnippetType { get; set; }

    [TextAreaComponent(Label = "Code", Order = 3, MinRowsNumber = 3)]
    [VisibleIfEqualTo(nameof(SnippetType), nameof(CodeSnippetTypes.CustomCode))]
    public string? Code { get; set; }

    [RadioGroupComponent(Label = "Code snippet location", Order = 4, Options = CodeSnippetLocationsExtensions.FormComponentOptions)]
    [VisibleIfEqualTo(nameof(SnippetType), nameof(CodeSnippetTypes.CustomCode))]
    public string? Location { get; set; }

    [TextInputComponent(Label = "Google Tag Manager ID", Order = 3)]
    [VisibleIfEqualTo(nameof(SnippetType), nameof(CodeSnippetTypes.GTM))]
    public string? GTMID { get; set; }

    [ObjectIdSelectorComponent(objectType: "cms.consent", Label = "Consent", Order = 5, Placeholder = "{$customchannelsettings.codesnippets.noconsentneeded$}")]
    public IEnumerable<int> ConsentID { get; set; } = [];
}
