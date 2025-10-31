using CMS.DataEngine;
using CMS.Membership;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.TagManager.Admin;

using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

[assembly: UIPage(
    parentType: typeof(ChannelCodeSnippetSectionPage),
    slug: "edit",
    uiPageType: typeof(CodeSnippetEditPage),
    name: "Edit tag",
    templateName: TemplateNames.EDIT,
    order: UIPageOrder.First)]

namespace Kentico.Xperience.TagManager.Admin;

[UIEvaluatePermission(SystemPermissions.UPDATE)]
internal class CodeSnippetEditPage : ModelEditPage<CodeSnippetConfigurationModel>
{
    private CodeSnippetConfigurationModel? model;

    protected override CodeSnippetConfigurationModel Model
    {
        get
        {
            if (model != null)
            {
                return model;
            }

            var info = channelCodeSnippetInfoProvider
                .Get()
                .WhereEquals(nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemID), ObjectID)
                .GetEnumerableTypedResult()
                .Single();

            if (info == null)
            {
                return new CodeSnippetConfigurationModel();
            }

            // Load content type bindings
            var contentTypeIds = contentTypeBindingProvider
                .Get()
                .WhereEquals(nameof(ChannelCodeSnippetItemContentTypeInfo.ChannelCodeSnippetItemID), info.ChannelCodeSnippetItemID)
                .Select(b => b.ContentTypeID)
                .ToList();

            model = new CodeSnippetConfigurationModel()
            {
                ChannelIDs = [info.ChannelCodeSnippetItemChannelId],
                Name = info.ChannelCodeSnippetItemName,
                Code = info.ChannelCodeSnippetItemCode,
                DisplayMode = info.ChannelCodeSnippetAdministrationDisplayMode,
                TagType = info.ChannelCodeSnippetItemType,
                ConsentIDs = info.ChannelCodeSnippetItemConsentId == 0 ? [] : [info.ChannelCodeSnippetItemConsentId],
                ContentTypeIDs = contentTypeIds,
                TagIdentifier = info.ChannelCodeSnippetItemIdentifier,
                Location = info.ChannelCodeSnippetItemLocation,
                Enable = info.ChannelCodeSnippetItemEnable
            };

            return model;
        }
    }

    private readonly IInfoProvider<ChannelCodeSnippetItemInfo> channelCodeSnippetInfoProvider;
    private readonly IInfoProvider<ChannelCodeSnippetItemContentTypeInfo> contentTypeBindingProvider;
    private readonly IWebsiteChannelPermissionService websiteChannelPermissionService;

    [PageParameter(typeof(IntPageModelBinder))]
    public int ObjectID { get; set; }

    public CodeSnippetEditPage(
        IFormItemCollectionProvider formItemCollectionProvider,
        IFormDataBinder formDataBinder,
        IInfoProvider<ChannelCodeSnippetItemInfo> channelCodeSnippetInfoProvider,
        IInfoProvider<ChannelCodeSnippetItemContentTypeInfo> contentTypeBindingProvider,
        IWebsiteChannelPermissionService websiteChannelPermissionService)
        : base(formItemCollectionProvider, formDataBinder)
    {
        this.channelCodeSnippetInfoProvider = channelCodeSnippetInfoProvider;
        this.contentTypeBindingProvider = contentTypeBindingProvider;
        this.websiteChannelPermissionService = websiteChannelPermissionService;
    }

    protected override async Task<ICommandResponse> SubmitInternal(
        FormSubmissionCommandArguments args,
        ICollection<IFormItem> items,
        IFormFieldValueProvider formFieldValueProvider)
    {
        //Validates Update permission for selected channel.

        int channelId = Model.ChannelIDs.FirstOrDefault();
        if (channelId == 0)
        {
            return await base.SubmitInternal(args, items, formFieldValueProvider);
        }

        bool isAllowed = await websiteChannelPermissionService.IsAllowed(channelId, SystemPermissions.UPDATE);

        return isAllowed
            ? await base.SubmitInternal(args, items, formFieldValueProvider)
            : ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
                .AddErrorMessage("You do not have permission");

    }

    protected override async Task<ICommandResponse> ProcessFormData(
        CodeSnippetConfigurationModel model,
        ICollection<IFormItem> formItems)
    {
        var info = (await channelCodeSnippetInfoProvider
            .Get()
            .WhereEquals(nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemID), ObjectID)
            .GetEnumerableTypedResultAsync())
            .Single();

        model.MapToChannelCodeSnippetInfo(info);

        await channelCodeSnippetInfoProvider.SetAsync(info);

        // Update content type bindings
        // Delete existing bindings
        var existingBindings = await contentTypeBindingProvider
            .Get()
            .WhereEquals(nameof(ChannelCodeSnippetItemContentTypeInfo.ChannelCodeSnippetItemID), info.ChannelCodeSnippetItemID)
            .GetEnumerableTypedResultAsync();

        foreach (var binding in existingBindings)
        {
            await contentTypeBindingProvider.DeleteAsync(binding);
        }

        // Create new bindings
        foreach (var contentTypeId in model.ContentTypeIDs)
        {
            var binding = new ChannelCodeSnippetItemContentTypeInfo
            {
                ChannelCodeSnippetItemID = info.ChannelCodeSnippetItemID,
                ContentTypeID = contentTypeId
            };
            await contentTypeBindingProvider.SetAsync(binding);
        }

        return await base.ProcessFormData(model, formItems);
    }
}
