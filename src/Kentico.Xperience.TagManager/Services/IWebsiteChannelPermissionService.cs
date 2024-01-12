using Kentico.Membership;

namespace Kentico.Xperience.TagManager.Services;

internal interface IWebsiteChannelPermissionService
{
    IEnumerable<int> GetChannelIDsWithGrantedPermission(AdminApplicationUser user, string permission);

    Task<bool> IsAllowed(int channelId, string permissionName);
}
