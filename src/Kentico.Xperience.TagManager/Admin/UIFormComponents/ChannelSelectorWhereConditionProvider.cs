using CMS.ContentEngine;
using CMS.DataEngine;
using Kentico.Xperience.Admin.Base.Authentication;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.TagManager.Constants;
using Kentico.Xperience.TagManager.Services;

namespace Kentico.Xperience.TagManager.Admin.UIFormComponents
{
    public class ChannelSelectorWhereConditionProvider : IObjectSelectorWhereConditionProvider
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

            var channelIDs = websiteChannelPermissionService
                .GetChannelIDsWithGrantedPermission(currentUser, GTMConstants.PermissionConstants.CustomChannelSettingsPermission)
                .ToArray();

            return new WhereCondition()
                .WhereIn(nameof(ChannelInfo.ChannelID), channelIDs);
        }
    }
}
