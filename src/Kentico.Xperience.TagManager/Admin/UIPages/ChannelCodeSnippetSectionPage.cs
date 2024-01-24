using CMS.DataEngine;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.TagManager.Admin;

[assembly: UIPage(
    parentType: typeof(CodeSnippetListing),
    slug: PageParameterConstants.PARAMETERIZED_SLUG,
    uiPageType: typeof(ChannelCodeSnippetSectionPage),
    name: "Edit",
    templateName: TemplateNames.SECTION_LAYOUT,
    order: 0)]

namespace Kentico.Xperience.TagManager.Admin;

internal class ChannelCodeSnippetSectionPage : EditSectionPage<ChannelCodeSnippetInfo>
{
    protected override async Task<string> GetObjectDisplayName(BaseInfo infoObject)
    {
        if (infoObject is not ChannelCodeSnippetInfo snippet)
        {
            return await base.GetObjectDisplayName(infoObject);
        }

        return string.IsNullOrWhiteSpace(snippet.ChannelCodeSnippetName)
            ? await base.GetObjectDisplayName(infoObject)
            : snippet.ChannelCodeSnippetName;
    }
}
