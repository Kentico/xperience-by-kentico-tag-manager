using CMS.ContentEngine;
using CMS.DataEngine;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.TagManager.Constants;
using Kentico.Xperience.TagManager.Services;

namespace Kentico.Xperience.TagManager.Admin.UIFormComponents
{
    public class ChannelSelectorWhereConditionProvider : IObjectSelectorWhereConditionProvider
    {
        private readonly IWebsiteChannelPermissionService websiteChannelPermissionService;

        public ChannelSelectorWhereConditionProvider(IWebsiteChannelPermissionService websiteChannelPermissionService)
        {
            this.websiteChannelPermissionService = websiteChannelPermissionService;
        }

        public WhereCondition Get()
        {
            var channelIDs = websiteChannelPermissionService
                .GetChannelIDsWithGrantedPermission(GTMConstants.PermissionConstants.CustomChannelSettingsPermission)
                .GetAwaiter().GetResult().ToList();
            return new WhereCondition()
                .WhereIn(nameof(ChannelInfo.ChannelID), channelIDs);
        }
    }
}
