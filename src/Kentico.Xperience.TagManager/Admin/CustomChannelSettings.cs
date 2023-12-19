using CMS.Membership;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.UIPages;
using Kentico.Xperience.TagManager.Admin;

[assembly: UIApplication(CustomChannelSettings.IDENTIFIER, typeof(CustomChannelSettings), "custom-channel-settings", "Custom channel settings", BaseApplicationCategories.CONFIGURATION, Icons.BrandGoogle, TemplateNames.SECTION_LAYOUT)]
namespace Kentico.Xperience.TagManager.Admin
{
    [UIPermission(SystemPermissions.VIEW)]
    internal class CustomChannelSettings : ApplicationPage
    {
        public const string IDENTIFIER = "custom-channel-settings";
    }
}