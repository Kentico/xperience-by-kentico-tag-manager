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
    public class CodeSnippetModelCreate : ModelEditPage<CodeSnippetEditModel>
    {
        private CodeSnippetEditModel? model;
        protected override CodeSnippetEditModel Model => model ??= new CodeSnippetEditModel();
        private readonly IChannelCodeSnippetInfoProvider channelCodeSnippetInfoProvider;

        public CodeSnippetModelCreate(Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider formItemCollectionProvider, IFormDataBinder formDataBinder, IChannelCodeSnippetInfoProvider channelCodeSnippetInfoProvider) : base(formItemCollectionProvider, formDataBinder)
        {
            this.channelCodeSnippetInfoProvider = channelCodeSnippetInfoProvider;
        }

        protected override async Task<ICommandResponse> ProcessFormData(CodeSnippetEditModel model, ICollection<IFormItem> formItems)
        {
            var infoObject = new ChannelCodeSnippetInfo()
            {
                ChannelCodeSnippetChannelID = model.ChannelID.FirstOrDefault(),
                ChannelCodeSnippetConsentID = model.ConsentID.FirstOrDefault(),
                ChannelCodeSnippetCode = model.Code,
                ChannelCodeSnippetGTMID = model.GTMID,
                ChannelCodeSnippetLocation = model.Location,
                ChannelCodeSnippetType = model.SnippetType
            };
            channelCodeSnippetInfoProvider.Set(infoObject);
            return await base.ProcessFormData(model, formItems);
        }
    }
}
