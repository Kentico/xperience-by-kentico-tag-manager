using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Membership;
using Kentico.Xperience.Admin.Base.Authentication;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Kentico.Xperience.TagManager.Admin;

internal class ChannelSelectorWhereConditionProvider : IObjectSelectorWhereConditionProvider
{
    private readonly IWebsiteChannelPermissionService websiteChannelPermissionService;
    private readonly IAuthenticatedUserAccessor authenticatedUserAccessor;

    public ChannelSelectorWhereConditionProvider(
        IWebsiteChannelPermissionService websiteChannelPermissionService,
        IAuthenticatedUserAccessor authenticatedUserAccessor)
    {
        this.websiteChannelPermissionService = websiteChannelPermissionService;
        this.authenticatedUserAccessor = authenticatedUserAccessor;
    }

    public WhereCondition Get()
    {
        var currentUser = authenticatedUserAccessor.Get().GetAwaiter().GetResult();

        int[] channelIDs = websiteChannelPermissionService
            .GetChannelIDsWithGrantedPermission(currentUser, SystemPermissions.VIEW)
            .ToArray();

        return new WhereCondition()
            .WhereIn(nameof(ChannelInfo.ChannelID), channelIDs);
    }
}
