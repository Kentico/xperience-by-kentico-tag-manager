using CMS.DataEngine;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.TagManager.Admin;

[assembly: UIPage(
    parentType: typeof(CodeSnippetListingPage),
    slug: PageParameterConstants.PARAMETERIZED_SLUG,
    uiPageType: typeof(ChannelCodeSnippetSectionPage),
    name: "Edit",
    templateName: TemplateNames.SECTION_LAYOUT,
    order: 0)]

namespace Kentico.Xperience.TagManager.Admin;

internal class ChannelCodeSnippetSectionPage : EditSectionPage<ChannelCodeSnippetItemInfo>
{
    protected override async Task<string> GetObjectDisplayName(BaseInfo infoObject)
    {
        if (infoObject is not ChannelCodeSnippetItemInfo snippet)
        {
            return await base.GetObjectDisplayName(infoObject);
        }

        return string.IsNullOrWhiteSpace(snippet.ChannelCodeSnippetItemName)
            ? await base.GetObjectDisplayName(infoObject)
            : snippet.ChannelCodeSnippetItemName;
    }
}
