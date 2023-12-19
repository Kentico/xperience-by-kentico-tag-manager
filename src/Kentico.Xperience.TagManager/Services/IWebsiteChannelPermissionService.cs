namespace Kentico.Xperience.TagManager.Services
{
    public interface IWebsiteChannelPermissionService
    {
        Task<IEnumerable<int>> GetChannelIDsWithGrantedPermission(string permission);
    }
}
