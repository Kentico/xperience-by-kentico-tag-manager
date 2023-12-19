using CMS.Membership;
using Kentico.Membership;
using Kentico.Xperience.Admin.Base.Authentication;
using CMS.Websites;

namespace Kentico.Xperience.TagManager.Services
{
    public class WebsiteChannelPermissionService : IWebsiteChannelPermissionService
    {
        private readonly IAuthenticatedUserAccessor authenticatedUserAccessor;
        private readonly IUserRoleInfoProvider roleInfoProvider;

        public WebsiteChannelPermissionService(IAuthenticatedUserAccessor authenticatedUserAccessor, IUserRoleInfoProvider roleInfoProvider)
        {
            this.authenticatedUserAccessor = authenticatedUserAccessor;
            this.roleInfoProvider = roleInfoProvider;
        }


        public async Task<IEnumerable<int>> GetChannelIDsWithGrantedPermission(string permission)
        {
            var user = await authenticatedUserAccessor.Get();
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
                .Select(g => Guid.Parse(g.Substring(g.LastIndexOf("_"))))
                .ToList();
            var websiteChannelIDs = WebsiteChannelInfoProvider.ProviderObject.Get()
                    .Columns(nameof(WebsiteChannelInfo.WebsiteChannelChannelID))
                    .WhereIn(nameof(WebsiteChannelInfo.WebsiteChannelGUID), websiteChannelGuids)
                    .GetListResult<int>();

            return websiteChannelIDs;
        }
    }
}
