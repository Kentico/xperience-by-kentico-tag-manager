using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Websites.UIPages;
using Kentico.Xperience.TagManager.Admin;
using Kentico.Xperience.TagManager.Constants;

[assembly: PageExtender(typeof(WebPageLayoutExtender))]
namespace Kentico.Xperience.TagManager.Admin
{
    [UIPermission(GTMConstants.PermissionConstants.CustomChannelSettingsPermission, "Channel settings")]
    public class WebPageLayoutExtender : PageExtender<WebPageLayout>
    {
        public override Task ConfigurePage()
        {
            return base.ConfigurePage();
        }
    }
}
