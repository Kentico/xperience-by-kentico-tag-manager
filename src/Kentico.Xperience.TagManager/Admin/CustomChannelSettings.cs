using CMS.Membership;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.UIPages;
using Kentico.Xperience.TagManager.Admin;

[assembly: UIApplication(
    identifier: CustomChannelSettings.IDENTIFIER,
    type: typeof(CustomChannelSettings),
    slug: "custom-channel-settings",
    name: "Custom channel settings",
    category: BaseApplicationCategories.CONFIGURATION,
    icon: Icons.DialogWindowCogwheel,
    templateName: TemplateNames.SECTION_LAYOUT)]

namespace Kentico.Xperience.TagManager.Admin;

[UIPermission(SystemPermissions.VIEW)]
[UIPermission(SystemPermissions.CREATE)]
[UIPermission(SystemPermissions.UPDATE)]
[UIPermission(SystemPermissions.DELETE)]
internal class CustomChannelSettings : ApplicationPage
{
    public const string IDENTIFIER = "custom-channel-settings";
}
