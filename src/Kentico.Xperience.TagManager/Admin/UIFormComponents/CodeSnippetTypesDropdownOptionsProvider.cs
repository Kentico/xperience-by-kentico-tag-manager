using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.TagManager.Snippets;

namespace Kentico.Xperience.TagManager.Admin;

internal class CodeSnippetTypesDropdownOptionsProvider : IDropDownOptionsProvider
{
    public Task<IEnumerable<DropDownOptionItem>> GetOptionItems()
    {
        var factories = SnippetFactoryStore.GetSnippetFactories();
        var options = new List<DropDownOptionItem>();

        foreach (var factory in factories)
        {
            var settings = factory.CreateCodeSnippetSettings();

            options.Add(new DropDownOptionItem
            {
                Value = settings.TagTypeName,
                Text = settings.TagDisplayName
            });
        }

        return Task.FromResult<IEnumerable<DropDownOptionItem>>(options);
    }
}
