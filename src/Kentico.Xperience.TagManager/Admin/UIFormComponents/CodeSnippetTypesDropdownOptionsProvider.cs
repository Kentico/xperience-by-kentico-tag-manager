using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Kentico.Xperience.TagManager.Admin;

internal class CodeSnippetTypesDropdownOptionsProvider : IDropDownOptionsProvider
{
    public Task<IEnumerable<DropDownOptionItem>> GetOptionItems() => Task.FromResult<IEnumerable<DropDownOptionItem>>(
    [
        new DropDownOptionItem
        {
            Value = nameof(CodeSnippetTypes.CustomCode),
            Text = "Custom code snippet"
        },
        new DropDownOptionItem
        {
            Value = nameof(CodeSnippetTypes.GTM),
            Text = "Google Tag Manager"
        }
    ]);
}
