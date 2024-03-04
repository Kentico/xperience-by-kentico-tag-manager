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
    uiPageType: typeof(CodeSnippetListing),
    name: "Code snippets",
    templateName: TemplateNames.LISTING,
    order: UIPageOrder.First)]

namespace Kentico.Xperience.TagManager.Admin;

internal class CodeSnippetListing : ListingPage
{
    private readonly IWebsiteChannelPermissionService websiteChannelPermissionService;
    private readonly IConsentInfoProvider consentInfoProvider;
    private readonly IAuthenticatedUserAccessor authenticatedUserAccessor;
    private readonly IInfoProvider<ChannelInfo> channelProvider;

    public CodeSnippetListing(
        IWebsiteChannelPermissionService websiteChannelPermissionService,
        IConsentInfoProvider consentInfoProvider,
        IAuthenticatedUserAccessor authenticatedUserAccessor,
        IInfoProvider<ChannelInfo> channelProvider)
    {
        this.websiteChannelPermissionService = websiteChannelPermissionService;
        this.consentInfoProvider = consentInfoProvider;
        this.authenticatedUserAccessor = authenticatedUserAccessor;
        this.channelProvider = channelProvider;
    }

    protected override string ObjectType => ChannelCodeSnippetInfo.OBJECT_TYPE;

    /// <inheritdoc />
    [PageCommand(Permission = SystemPermissions.DELETE)]
    public override Task<ICommandResponse<RowActionResult>> Delete(int id) => base.Delete(id);

    public override async Task ConfigurePage()
    {
        var allConsents = await consentInfoProvider.Get().GetEnumerableTypedResultAsync();
        var allChannels = await channelProvider.Get().GetEnumerableTypedResultAsync();

        PageConfiguration.HeaderActions.AddLink<CodeSnippetModelCreate>("Add new");
        PageConfiguration.TableActions.AddDeleteAction(nameof(Delete));
        PageConfiguration.AddEditRowAction<CodeSnippetModelEdit>();

        PageConfiguration.ColumnConfigurations
            .AddColumn(
                nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetID),
                "ID",
                maxWidth: 10)
            .AddColumn(
                nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetName),
                "Code Name",
                searchable: true)
            .AddColumn(
                nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetChannelID),
                "Channel",
                sortable: false,
                formatter: (value, _) => allChannels.FirstOrDefault(c => c.ChannelID == (int)value)?.ChannelDisplayName ?? ""
            )
            .AddColumn(
                nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetType),
                "Type",
                formatter: (_, container) => FormatSnippetType(container),
                sortable: true)
            .AddColumn(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetCode),
                "Code Snippet",
                sortable: false,
                formatter: (_, container) => FormatCodeSnippet(container))
            .AddColumn(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetIdentifier),
                "Identifier",
                formatter: (_, container) => FormatIdentifier(container))
            .AddColumn(
                nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetConsentID),
                "Consent",
                sortable: false,
                formatter: (value, _) =>
                    value is null or 0
                        ? LocalizationService.GetString("customchannelsettings.codesnippets.noconsentneeded")
                        : allConsents.FirstOrDefault(c => c.ConsentID == (int)value)?.ConsentDisplayName ?? "")
            .AddColumn(
                nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetLastModified),
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
                    .WhereIn(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetChannelID), channelsIDs)
                    .WhereIn(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetType), SnippetFactoryStore.GetRegisteredSnippetFactoryTypes().ToArray())
                    .AddColumns(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetIdentifier), nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetType)));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static string FormatCodeSnippet(IDataContainer container)
    {
        string codeSnippetType = (string)container[nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetType)];

        if (string.IsNullOrEmpty(codeSnippetType))
        {
            throw new ArgumentNullException(
                nameof(container),
                "Invalid ChannelCodeSnippetType!");
        }

        if (codeSnippetType == CustomSnippetFactory.TAG_TYPE_NAME)
        {
            return container[nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetCode)] as string ?? string.Empty;
        }

        return string.Empty;
    }

    private static string FormatIdentifier(IDataContainer container)
    {
        string codeSnippetType = (string)container[nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetType)];

        if (string.IsNullOrEmpty(codeSnippetType))
        {
            throw new ArgumentNullException(
                nameof(container),
                "Invalid ChannelCodeSnippetType!");
        }

        if (codeSnippetType != CustomSnippetFactory.TAG_TYPE_NAME)
        {
            return (string)container[nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetIdentifier)];
        }

        return string.Empty;
    }

    private static string FormatSnippetType(IDataContainer container)
    {
        string codeSnippetType = (string)container[nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetType)];

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

internal record struct UIPermissions
{
    public bool View { get; set; }
    public bool Update { get; set; }
    public bool Delete { get; set; }
    public bool Create { get; set; }
}
