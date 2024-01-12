using CMS.ContentEngine;
using CMS.DataProtection;
using CMS.Membership;
using GTM;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Authentication;
using Kentico.Xperience.TagManager.Admin;
using Kentico.Xperience.TagManager.Admin.UIPages;
using Kentico.Xperience.TagManager.Services;

[assembly: UIPage(
    parentType: typeof(CustomChannelSettings),
    slug: "snippets",
    uiPageType: typeof(CodeSnippetListing),
    name: "Code snippets",
    templateName: TemplateNames.LISTING,
    order: UIPageOrder.First)]

namespace Kentico.Xperience.TagManager.Admin.UIPages;

internal class CodeSnippetListing : ListingPage
{
    private readonly IWebsiteChannelPermissionService websiteChannelPermissionService;
    private readonly IConsentInfoProvider consentInfoProvider;
    private readonly IAuthenticatedUserAccessor authenticatedUserAccessor;

    public CodeSnippetListing(IWebsiteChannelPermissionService websiteChannelPermissionService,
        IConsentInfoProvider consentInfoProvider, IAuthenticatedUserAccessor authenticatedUserAccessor)
    {
        this.websiteChannelPermissionService = websiteChannelPermissionService;
        this.consentInfoProvider = consentInfoProvider;
        this.authenticatedUserAccessor = authenticatedUserAccessor;
    }

    protected override string ObjectType => ChannelCodeSnippetInfo.OBJECT_TYPE;

    /// <inheritdoc />
    [PageCommand(Permission = "Delete")]
    public override Task<ICommandResponse<RowActionResult>> Delete(int id) => base.Delete(id);

    public override async Task ConfigurePage()
    {
        PageConfiguration.HeaderActions.AddLink<CodeSnippetModelCreate>("Add new");
        PageConfiguration.TableActions.AddDeleteAction("Delete");
        PageConfiguration.ColumnConfigurations.AddColumn(
            nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetChannelID),
            "Channel",
            formatter: (value, _) => ChannelInfoProvider.ProviderObject.Get((int)value).ChannelDisplayName
        );

        PageConfiguration.QueryModifiers.AddModifier(q =>
            q.AddColumn(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetGTMID)));
        PageConfiguration.ColumnConfigurations.AddColumn(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetCode),
            "Code Snippet",
            formatter: (_, container) =>
                container[nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetCode)] as string ??
                $"Google Tag Manager ID: {container[nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetGTMID)]}");
        PageConfiguration.ColumnConfigurations.AddColumn(
            nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetConsentID),
            "Consent",
            formatter: (value, _) =>
                value is null or 0
                    ? LocalizationService.GetString("customchannelsettings.codesnippets.noconsentneeded")
                    : consentInfoProvider.Get((int)value).ConsentDisplayName
        );

        PageConfiguration.AddEditRowAction<CodeSnippetModelEdit>();


        var channelsIDs = websiteChannelPermissionService.GetChannelIDsWithGrantedPermission(
                await authenticatedUserAccessor.Get(),
                SystemPermissions.VIEW)
            .ToArray();

        PageConfiguration.QueryModifiers
            .AddModifier((query, _) =>
                query.WhereIn(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetChannelID), channelsIDs));
    }
}
