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
    slug: "snippets",
    uiPageType: typeof(CodeSnippetListingPage),
    name: "Code snippets",
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
        var allConsents = await consentInfoProvider.Get().GetEnumerableTypedResultAsync();
        var allChannels = await channelProvider.Get().GetEnumerableTypedResultAsync();

        PageConfiguration.HeaderActions.AddLink<CodeSnippetCreatePage>("Add new");
        PageConfiguration.TableActions.AddDeleteAction(nameof(Delete));
        PageConfiguration.AddEditRowAction<CodeSnippetEditPage>();

        PageConfiguration.ColumnConfigurations
            .AddColumn(
                nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemID),
                "ID",
                maxWidth: 10)
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
            .AddColumn(nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemCode),
                "Code Snippet",
                sortable: false,
                formatter: (_, container) => FormatCodeSnippet(container))
            .AddColumn(nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemIdentifier),
                "Identifier",
                formatter: (_, container) => FormatIdentifier(container))
            .AddColumn(
                nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemConsentId),
                "Consent",
                sortable: false,
                formatter: (value, _) =>
                    value is null or 0
                        ? LocalizationService.GetString("No consent needed")
                        : allConsents.FirstOrDefault(c => c.ConsentID == (int)value)?.ConsentDisplayName ?? "")
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static string FormatCodeSnippet(IDataContainer container)
    {
        string codeSnippetType = (string)container[nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemType)];

        if (string.IsNullOrEmpty(codeSnippetType))
        {
            throw new ArgumentNullException(
                nameof(container),
                "Invalid ChannelCodeSnippetType!");
        }

        if (codeSnippetType == CustomSnippetFactory.TAG_TYPE_NAME)
        {
            return container[nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemCode)] as string ?? string.Empty;
        }

        return string.Empty;
    }

    private static string FormatIdentifier(IDataContainer container)
    {
        string codeSnippetType = (string)container[nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemType)];

        if (string.IsNullOrEmpty(codeSnippetType))
        {
            throw new ArgumentNullException(
                nameof(container),
                "Invalid ChannelCodeSnippetType!");
        }

        if (codeSnippetType != CustomSnippetFactory.TAG_TYPE_NAME)
        {
            return (string)container[nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemIdentifier)];
        }

        return string.Empty;
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
            throw new InvalidOperationException("Specified snippet is not registered.");
    }
}
