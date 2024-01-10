using Kentico.Membership;

namespace Kentico.Xperience.TagManager.Services
{
    public interface IWebsiteChannelPermissionService
    {
        IEnumerable<int> GetChannelIDsWithGrantedPermission(AdminApplicationUser user, string permission);
    }
}
