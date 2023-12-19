using CMS.ContentEngine;
using CMS.DataProtection;
using GTM;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.TagManager.Admin;
using Kentico.Xperience.TagManager.Admin.UIPages;
using Kentico.Xperience.TagManager.Constants;
using Kentico.Xperience.TagManager.Services;

[assembly: UIPage(
    parentType: typeof(CustomChannelSettings),
    slug: "snippets",
    uiPageType: typeof(CodeSnippetListing),
    name: "Code snippets",
    templateName: TemplateNames.LISTING,
    order: UIPageOrder.First)]
namespace Kentico.Xperience.TagManager.Admin.UIPages
{
    public class CodeSnippetListing : ListingPage
    {
        private readonly IWebsiteChannelPermissionService websiteChannelPermissionService;
        private readonly IConsentInfoProvider consentInfoProvider;

        public CodeSnippetListing(IWebsiteChannelPermissionService websiteChannelPermissionService, IConsentInfoProvider consentInfoProvider)
        {
            this.websiteChannelPermissionService = websiteChannelPermissionService;
            this.consentInfoProvider = consentInfoProvider;
        }

        protected override string ObjectType => ChannelCodeSnippetInfo.OBJECT_TYPE;

        public async override Task ConfigurePage()
        {
            PageConfiguration.HeaderActions.AddLink<CodeSnippetModelCreate>("Add new");

            PageConfiguration.ColumnConfigurations.AddColumn(
                nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetChannelID),
                "Channel",
                formatter: (value, _) => ChannelInfoProvider.ProviderObject.Get((int)value).ChannelDisplayName
            );
            PageConfiguration.ColumnConfigurations.AddColumn(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetGTMID), "GTM ID");
            PageConfiguration.ColumnConfigurations.AddColumn(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetCode), "Code snippet");
            PageConfiguration.ColumnConfigurations.AddColumn(
                nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetConsentID),
                "Consent",
                formatter: (value, _) => consentInfoProvider.Get((int)value).ConsentDisplayName
            );

            PageConfiguration.AddEditRowAction<CodeSnippetModelEdit>();


            var channelsIDs = (await websiteChannelPermissionService
                .GetChannelIDsWithGrantedPermission(GTMConstants.PermissionConstants.CustomChannelSettingsPermission))
                .ToList();
            PageConfiguration.QueryModifiers
                .AddModifier((query, _) =>
                {
                    // Filters displayed objects by the 'SiteID' parameter
                    return query.WhereIn(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetChannelID), channelsIDs);
                });
        }
    }
}