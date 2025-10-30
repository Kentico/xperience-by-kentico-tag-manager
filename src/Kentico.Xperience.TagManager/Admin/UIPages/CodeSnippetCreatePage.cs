using CMS.DataEngine;
using CMS.Membership;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.TagManager.Admin;

using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

[assembly: UIPage(
    parentType: typeof(CodeSnippetListingPage),
    slug: "add",
    uiPageType: typeof(CodeSnippetCreatePage),
    name: "Create a tag",
    templateName: TemplateNames.EDIT,
    order: UIPageOrder.First)]

namespace Kentico.Xperience.TagManager.Admin;

[UIEvaluatePermission(SystemPermissions.CREATE)]
internal class CodeSnippetCreatePage : ModelEditPage<CodeSnippetConfigurationModel>
{
    private CodeSnippetConfigurationModel? model;
    protected override CodeSnippetConfigurationModel Model => model ??= new CodeSnippetConfigurationModel();
    private readonly IInfoProvider<ChannelCodeSnippetItemInfo> channelCodeSnippetInfoProvider;
    private readonly IInfoProvider<ChannelCodeSnippetItemContentTypeInfo> contentTypeBindingProvider;
    private readonly IPageLinkGenerator pageLinkGenerator;
    private readonly IWebsiteChannelPermissionService websiteChannelPermissionService;

    public CodeSnippetCreatePage(
        IFormItemCollectionProvider formItemCollectionProvider,
        IFormDataBinder formDataBinder,
        IInfoProvider<ChannelCodeSnippetItemInfo> channelCodeSnippetInfoProvider,
        IInfoProvider<ChannelCodeSnippetItemContentTypeInfo> contentTypeBindingProvider,
        IPageLinkGenerator pageLinkGenerator,
        IWebsiteChannelPermissionService websiteChannelPermissionService)
        : base(formItemCollectionProvider, formDataBinder)
    {
        this.channelCodeSnippetInfoProvider = channelCodeSnippetInfoProvider;
        this.contentTypeBindingProvider = contentTypeBindingProvider;
        this.pageLinkGenerator = pageLinkGenerator;
        this.websiteChannelPermissionService = websiteChannelPermissionService;
    }

    protected override async Task<ICommandResponse> ProcessFormData(CodeSnippetConfigurationModel model,
        ICollection<IFormItem> formItems)
    {
        CreateCodeSnippetInfo(model);

        var navigateResponse = await NavigateToEditPage(model, formItems);

        return navigateResponse;
    }

    private async Task<INavigateResponse> NavigateToEditPage(CodeSnippetConfigurationModel model,
        ICollection<IFormItem> formItems)
    {
        var baseResult = await base.ProcessFormData(model, formItems);

        var navigateResponse = NavigateTo(
            pageLinkGenerator.GetPath<CodeSnippetListingPage>());

        foreach (var message in baseResult.Messages)
        {
            navigateResponse.Messages.Add(message);
        }

        return navigateResponse;
    }

    protected override async Task<ICommandResponse> SubmitInternal(
        FormSubmissionCommandArguments args,
        ICollection<IFormItem> items,
        IFormFieldValueProvider formFieldValueProvider)
    {
        //Validates Create permission for selected channel.

        int channelId = Model.ChannelIDs.FirstOrDefault();
        if (channelId == 0)
        {
            return await base.SubmitInternal(args, items, formFieldValueProvider);
        }

        bool isAllowed = await websiteChannelPermissionService.IsAllowed(channelId, SystemPermissions.VIEW);

        return isAllowed
            ? await base.SubmitInternal(args, items, formFieldValueProvider)
            : ResponseFrom(new FormSubmissionResult(FormSubmissionStatus.ValidationFailure))
                .AddErrorMessage(
                    LocalizationService.GetString("customchannelsettings.codesnippets.permissionerror"));
    }

    private void CreateCodeSnippetInfo(CodeSnippetConfigurationModel model)
    {
        var infoObject = new ChannelCodeSnippetItemInfo();

        model.MapToChannelCodeSnippetInfo(infoObject);

        channelCodeSnippetInfoProvider.Set(infoObject);

        // Create content type bindings
        foreach (var contentTypeId in model.ContentTypeIDs)
        {
            var binding = new ChannelCodeSnippetItemContentTypeInfo
            {
                ChannelCodeSnippetItemID = infoObject.ChannelCodeSnippetItemID,
                ContentTypeID = contentTypeId
            };
            contentTypeBindingProvider.Set(binding);
        }
    }
}
