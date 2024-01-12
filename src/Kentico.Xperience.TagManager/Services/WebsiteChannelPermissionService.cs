using CMS.Membership;
using CMS.Websites;
using Kentico.Membership;
using Kentico.Xperience.Admin.Base.Authentication;

namespace Kentico.Xperience.TagManager.Services;

internal class WebsiteChannelPermissionService : IWebsiteChannelPermissionService
{
    private readonly IUserRoleInfoProvider roleInfoProvider;
    private readonly IAuthenticatedUserAccessor authenticatedUserAccessor;

    public WebsiteChannelPermissionService(IUserRoleInfoProvider roleInfoProvider, IAuthenticatedUserAccessor authenticatedUserAccessor)
    {
        this.roleInfoProvider = roleInfoProvider;
        this.authenticatedUserAccessor = authenticatedUserAccessor;
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

    public async Task<bool> IsAllowed(int channelId, string permissionName)
    {
        var currentUser = await authenticatedUserAccessor.Get();

        if (currentUser.IsAdministrator())
        {
            return true;
        }

        int[] channelsIDs = GetChannelIDsWithGrantedPermission(
                await authenticatedUserAccessor.Get(),
                SystemPermissions.VIEW)
            .ToArray();

        return channelsIDs.Contains(channelId);
    }
}
