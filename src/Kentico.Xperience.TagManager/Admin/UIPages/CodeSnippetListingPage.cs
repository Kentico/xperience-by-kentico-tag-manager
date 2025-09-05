using CMS.Base;
using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.DataProtection;
using CMS.Membership;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Authentication;
using Kentico.Xperience.TagManager.Admin;
using Kentico.Xperience.TagManager.Snippets;

[assembly: UIPage(
    parentType: typeof(TagManagerApplicationPage),
    slug: "tags",
    uiPageType: typeof(CodeSnippetListingPage),
    name: "Tags",
    templateName: TemplateNames.LISTING,
    order: UIPageOrder.First)]

namespace Kentico.Xperience.TagManager.Admin;

/// <summary>
/// An admin UI page that displays information about the registered tags.
/// </summary>
[UIEvaluatePermission(SystemPermissions.VIEW)]
internal class CodeSnippetListingPage : ListingPage
{
    private readonly IWebsiteChannelPermissionService websiteChannelPermissionService;
    private readonly IAuthenticatedUserAccessor authenticatedUserAccessor;
    private readonly IInfoProvider<ConsentInfo> consentInfoProvider;
    private readonly IInfoProvider<ChannelInfo> channelProvider;

    public CodeSnippetListingPage(
        IWebsiteChannelPermissionService websiteChannelPermissionService,
        IInfoProvider<ConsentInfo> consentInfoProvider,
        IAuthenticatedUserAccessor authenticatedUserAccessor,
        IInfoProvider<ChannelInfo> channelProvider)
    {
        this.websiteChannelPermissionService = websiteChannelPermissionService;
        this.consentInfoProvider = consentInfoProvider;
        this.authenticatedUserAccessor = authenticatedUserAccessor;
        this.channelProvider = channelProvider;
    }

    protected override string ObjectType => ChannelCodeSnippetItemInfo.OBJECT_TYPE;

    /// <inheritdoc />
    [PageCommand(Permission = SystemPermissions.DELETE)]
    public override Task<ICommandResponse<RowActionResult>> Delete(int id) => base.Delete(id);

    public override async Task ConfigurePage()
    {
        await base.ConfigurePage();

        var allConsents = await consentInfoProvider.Get().GetEnumerableTypedResultAsync();
        var allChannels = await channelProvider.Get().GetEnumerableTypedResultAsync();

        PageConfiguration.HeaderActions.AddLink<CodeSnippetCreatePage>("Add new");
        PageConfiguration.TableActions.AddDeleteAction(nameof(Delete));
        PageConfiguration.AddEditRowAction<CodeSnippetEditPage>();

        PageConfiguration.ColumnConfigurations
            .AddColumn(
                nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemID),
                "ID",
                maxWidth: 4)
            .AddColumn(
                nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemName),
                "Code Name",
                searchable: true)
            .AddColumn(
                nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemChannelId),
                "Channel",
                sortable: false,
                formatter: (value, _) => allChannels.FirstOrDefault(c => c.ChannelID == (int)value)?.ChannelDisplayName ?? ""
            )
            .AddColumn(
                nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemType),
                "Type",
                formatter: (_, container) => FormatSnippetType(container),
                sortable: true)
            .AddColumn(
                nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemConsentId),
                "Consent",
                sortable: false,
                formatter: (value, _) =>
                    value is null or 0
                        ? LocalizationService.GetString("customchannelsettings.codesnippets.noconsentneeded")
                        : allConsents.FirstOrDefault(c => c.ConsentID == (int)value)?.ConsentDisplayName ?? "")
            .AddColumn(
                nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemEnable),
                "Enable",
                sortable: true)
            .AddColumn(
                nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemLastModified),
                "Last Modified",
                defaultSortDirection: SortTypeEnum.Desc,
                sortable: true);

        int[] channelsIDs = websiteChannelPermissionService.GetChannelIDsWithGrantedPermission(
                await authenticatedUserAccessor.Get(),
                SystemPermissions.VIEW)
            .ToArray();

        PageConfiguration.QueryModifiers
            .AddModifier((query, _) =>
                query
                    .WhereIn(nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemChannelId), channelsIDs)
                    .WhereIn(nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemType), SnippetFactoryStore.GetRegisteredSnippetFactoryTypes().ToArray())
                    .AddColumns(nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemIdentifier), nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemType)));
    }

    private static string FormatSnippetType(IDataContainer container)
    {
        string codeSnippetType = (string)container[nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemType)];

        if (string.IsNullOrEmpty(codeSnippetType))
        {
            throw new ArgumentNullException(
                nameof(container),
                "Invalid ChannelCodeSnippetType!");
        }

        return SnippetFactoryStore.TryGetSnippetFactory(codeSnippetType)?.CreateCodeSnippetSettings().TagDisplayName ??
            throw new InvalidOperationException("Specified tag is not registered.");
    }
}
