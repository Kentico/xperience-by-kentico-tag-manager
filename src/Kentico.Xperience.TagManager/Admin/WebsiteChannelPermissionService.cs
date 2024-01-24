using CMS.Membership;
using CMS.Websites;
using Kentico.Membership;
using Kentico.Xperience.Admin.Base.Authentication;
using Kentico.Xperience.TagManager.Constants;

namespace Kentico.Xperience.TagManager.Admin;

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
            .WhereStartsWith(nameof(ApplicationPermissionInfo.ApplicationName), TagManagerConstants.Permissions.WebsiteChannelPermissionName)
            .Column(nameof(ApplicationPermissionInfo.ApplicationName))
            .GetListResult<string>()
            .Select(g => g.Split('_') is [_, var guid] ? Guid.Parse(guid) : Guid.Empty)
            .Where(g => g != Guid.Empty)
            .ToList();

        var websiteChannelIDs = WebsiteChannelInfoProvider.ProviderObject.Get()
            .Columns(nameof(WebsiteChannelInfo.WebsiteChannelChannelID))
            .WhereIn(nameof(WebsiteChannelInfo.WebsiteChannelGUID), websiteChannelGuids)
            .GetListResult<int>();

        return websiteChannelIDs;
    }

    public async Task<bool> IsAllowed(int channelId, string permissionName)
    {
        int[] channelsIDs = GetChannelIDsWithGrantedPermission(
                await authenticatedUserAccessor.Get(),
                SystemPermissions.VIEW)
            .ToArray();

        return channelsIDs.Contains(channelId);
    }
}
