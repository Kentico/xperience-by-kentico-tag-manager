namespace Kentico.Xperience.TagManager.Constants;

internal static class GtmConstants
{
    internal static class Permissions
    {
        public const string WebsiteChannelPermissionName = "Kentico.Xperience.Application.WebPages";
    }

    internal static class ResourceConstants
    {
        private const string ResourceNamePrefix = "Kentico.Xperience.TagManager";
        public const string ResourceDisplayName = "Custom channel settings";
        public const string ResourceName = $"{ResourceNamePrefix}.CustomChannelSettings";
        public const string ResourceDescription = "";
        public const bool ResourceIsInDevelopment = true;
    }
}
