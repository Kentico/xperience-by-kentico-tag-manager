using CMS.Membership;
using GTM;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.TagManager.Admin.UIPages;
using Kentico.Xperience.TagManager.Models;

[assembly: UIPage(
    parentType: typeof(CodeSnippetListing),
    "add",
    uiPageType: typeof(CodeSnippetModelCreate),
    name: "Create a code snippet",
    templateName: TemplateNames.EDIT,
    order: UIPageOrder.First)]

namespace Kentico.Xperience.TagManager.Admin.UIPages
{
    [UIPermission(SystemPermissions.CREATE)]
    public class CodeSnippetModelCreate : ModelEditPage<CodeSnippetEditModel>
    {
        private CodeSnippetEditModel? model;
        protected override CodeSnippetEditModel Model => model ??= new CodeSnippetEditModel();
        private readonly IChannelCodeSnippetInfoProvider channelCodeSnippetInfoProvider;
        private readonly IPageUrlGenerator pageUrlGenerator;

        public CodeSnippetModelCreate(
            Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider formItemCollectionProvider,
            IFormDataBinder formDataBinder, IChannelCodeSnippetInfoProvider channelCodeSnippetInfoProvider,
            IPageUrlGenerator pageUrlGenerator)
            : base(formItemCollectionProvider, formDataBinder)
        {
            this.channelCodeSnippetInfoProvider = channelCodeSnippetInfoProvider;
            this.pageUrlGenerator = pageUrlGenerator;
        }

        protected override async Task<ICommandResponse> ProcessFormData(CodeSnippetEditModel model,
            ICollection<IFormItem> formItems)
        {
            CreateCodeSnippetInfo(model);

            var navigateResponse = await NavigateToEditPage(model, formItems);

            return navigateResponse;
        }

        private async Task<INavigateResponse> NavigateToEditPage(CodeSnippetEditModel model, ICollection<IFormItem> formItems)
        {
            var baseResult = await base.ProcessFormData(model, formItems);

            var navigateResponse = NavigateTo(
                pageUrlGenerator.GenerateUrl<CodeSnippetListing>());

            foreach (var message in baseResult.Messages)
            {
                navigateResponse.Messages.Add(message);
            }

            return navigateResponse;
        }

        private ChannelCodeSnippetInfo CreateCodeSnippetInfo(CodeSnippetEditModel model)
        {
            var infoObject = new ChannelCodeSnippetInfo
            {
                ChannelCodeSnippetChannelID = model.ChannelID.FirstOrDefault(),
                ChannelCodeSnippetConsentID = model.ConsentID.FirstOrDefault(),
                ChannelCodeSnippetCode = model.Code,
                ChannelCodeSnippetGTMID = model.GTMID,
                ChannelCodeSnippetLocation = model.Location,
                ChannelCodeSnippetType = model.SnippetType
            };
            channelCodeSnippetInfoProvider.Set(infoObject);
            return infoObject;
        }
    }
}
