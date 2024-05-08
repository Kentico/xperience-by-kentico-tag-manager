using CMS.Membership;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.UIPages;
using Kentico.Xperience.TagManager.Admin;

[assembly: UIApplication(
    identifier: TagManagerApplicationPage.IDENTIFIER,
    type: typeof(TagManagerApplicationPage),
    slug: "tag-management",
    name: "Tag Management",
    category: BaseApplicationCategories.CONFIGURATION,
    icon: Icons.BracesOctothorpe,
    templateName: TemplateNames.SECTION_LAYOUT)]

namespace Kentico.Xperience.TagManager.Admin;

/// <summary>
/// The root application page for Tag Managment integration.
/// </summary>
[UIPermission(SystemPermissions.VIEW)]
[UIPermission(SystemPermissions.CREATE)]
[UIPermission(SystemPermissions.UPDATE)]
[UIPermission(SystemPermissions.DELETE)]
internal class TagManagerApplicationPage : ApplicationPage
{
    public const string IDENTIFIER = "Kentico.Xperience.Integrations.TagManager";
}
