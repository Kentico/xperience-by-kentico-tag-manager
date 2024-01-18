using CMS.Membership;
using GTM;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.TagManager.Admin.UIPages;
using Kentico.Xperience.TagManager.Admin.UIPages.Models;
using Kentico.Xperience.TagManager.Services;
using IFormItemCollectionProvider = Kentico.Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider;

[assembly: UIPage(
    parentType: typeof(CodeSnippetListing),
    PageParameterConstants.PARAMETERIZED_SLUG,
    uiPageType: typeof(CodeSnippetModelEdit),
    name: "Edit a code snippet",
    templateName: TemplateNames.EDIT,
    order: UIPageOrder.First)]

namespace Kentico.Xperience.TagManager.Admin.UIPages;

internal class CodeSnippetModelEdit : ModelEditPage<CodeSnippetEditModel>
{
    private CodeSnippetEditModel? model;

    protected override CodeSnippetEditModel Model
    {
        get
        {
            if (model != null)
            {
                return model;
            }

            var info = channelCodeSnippetInfoProvider.Get(ObjectID);
            if (info == null)
            {
                return new CodeSnippetEditModel();
            }

            model = new CodeSnippetEditModel()
            {
                ChannelIDs = [info.ChannelCodeSnippetChannelID],
                Code = info.ChannelCodeSnippetCode,
                SnippetType = info.ChannelCodeSnippetType,
                ConsentIDs = info.ChannelCodeSnippetConsentID == 0 ? [] : [info.ChannelCodeSnippetConsentID],
                GTMID = info.ChannelCodeSnippetGTMID,
                Location = info.ChannelCodeSnippetLocation,
            };

            return model;
        }
    }

    private readonly IChannelCodeSnippetInfoProvider channelCodeSnippetInfoProvider;
    private readonly IWebsiteChannelPermissionService websiteChannelPermissionService;

    [PageParameter(typeof(IntPageModelBinder))]
    public int ObjectID { get; set; }

    public CodeSnippetModelEdit(
        IFormItemCollectionProvider formItemCollectionProvider,
        IFormDataBinder formDataBinder,
        IChannelCodeSnippetInfoProvider channelCodeSnippetInfoProvider,
        IWebsiteChannelPermissionService websiteChannelPermissionService)
        : base(formItemCollectionProvider, formDataBinder)
    {
        this.channelCodeSnippetInfoProvider = channelCodeSnippetInfoProvider;
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
                .AddErrorMessage(
                    LocalizationService.GetString("customchannelsettings.codesnippets.permissionerror"));

    }

    protected override async Task<ICommandResponse> ProcessFormData(
        CodeSnippetEditModel model,
        ICollection<IFormItem> formItems)
    {
        var info = channelCodeSnippetInfoProvider.Get(ObjectID);
        info.ChannelCodeSnippetChannelID = model.ChannelIDs.FirstOrDefault();
        info.ChannelCodeSnippetConsentID = model.ConsentIDs.FirstOrDefault();
        info.ChannelCodeSnippetCode = model.Code;
        info.ChannelCodeSnippetGTMID = model.GTMID;
        info.ChannelCodeSnippetLocation = model.Location;
        info.ChannelCodeSnippetType = model.SnippetType;
        channelCodeSnippetInfoProvider.Set(info);

        return await base.ProcessFormData(model, formItems);
    }
}
