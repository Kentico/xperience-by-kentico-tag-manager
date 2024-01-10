using CMS.Membership;
using CMS.Websites;
using Kentico.Membership;

namespace Kentico.Xperience.TagManager.Services
{
    public class WebsiteChannelPermissionService : IWebsiteChannelPermissionService
    {
        private readonly IUserRoleInfoProvider roleInfoProvider;

        public WebsiteChannelPermissionService(IUserRoleInfoProvider roleInfoProvider)
        {
            this.roleInfoProvider = roleInfoProvider;
        }

        public IEnumerable<int> GetChannelIDsWithGrantedPermission(AdminApplicationUser user, string permission)
        {
            if (user.IsAdministrator())
            {
                return WebsiteChannelInfoProvider.ProviderObject.Get()
                    .Columns(nameof(WebsiteChannelInfo.WebsiteChannelChannelID))
                    .GetListResult<int>();
            }
            var websiteChannelGuids = roleInfoProvider.Get()
                .Source(s => s.InnerJoin<ApplicationPermissionInfo>(nameof(UserRoleInfo.RoleID), nameof(ApplicationPermissionInfo.RoleID)))
                .WhereEquals(nameof(UserInfo.UserID), user.UserID)
                .WhereEquals(nameof(ApplicationPermissionInfo.PermissionName), permission)
                .Column(nameof(ApplicationPermissionInfo.ApplicationName))
                .GetListResult<string>()
                .Select(g => Guid.Parse(g[g.LastIndexOf('_')..]))
                .ToList();

            var websiteChannelIDs = WebsiteChannelInfoProvider.ProviderObject.Get()
                    .Columns(nameof(WebsiteChannelInfo.WebsiteChannelChannelID))
                    .WhereIn(nameof(WebsiteChannelInfo.WebsiteChannelGUID), websiteChannelGuids)
                    .GetListResult<int>();

            return websiteChannelIDs;
        }
    }
}
