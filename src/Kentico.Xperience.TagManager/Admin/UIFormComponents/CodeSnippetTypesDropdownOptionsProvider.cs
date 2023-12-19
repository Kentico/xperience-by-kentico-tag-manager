using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.TagManager.Enums;

namespace Kentico.Xperience.TagManager.Admin.UIFormComponents
{
    public class CodeSnippetTypesDropdownOptionsProvider : IDropDownOptionsProvider
    {
        public async Task<IEnumerable<DropDownOptionItem>> GetOptionItems()
        {
            return new DropDownOptionItem[]
            {
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
            };
        }
    }
}
