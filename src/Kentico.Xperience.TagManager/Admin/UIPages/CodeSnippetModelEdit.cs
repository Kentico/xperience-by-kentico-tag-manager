using GTM;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.TagManager.Admin.UIPages;
using Kentico.Xperience.TagManager.Models;

[assembly: UIPage(
    parentType: typeof(CodeSnippetListing),
    PageParameterConstants.PARAMETERIZED_SLUG,
    uiPageType: typeof(CodeSnippetModelEdit),
    name: "Edit a code snippet",
    templateName: TemplateNames.EDIT,
    order: UIPageOrder.First)]
namespace Kentico.Xperience.TagManager.Admin.UIPages
{
    public class CodeSnippetModelEdit : ModelEditPage<CodeSnippetEditModel>
    {
        private CodeSnippetEditModel? model;
        protected override CodeSnippetEditModel Model
        {
            get
            {
                if (model == null)
                {
                    var info = channelCodeSnippetInfoProvider.Get(ObjectID);
                    if (info == null)
                    {
                        return new CodeSnippetEditModel();
                    }
                    model = new CodeSnippetEditModel()
                    {
                        ChannelID = new int[] { info.ChannelCodeSnippetChannelID },
                        Code = info.ChannelCodeSnippetCode,
                        SnippetType = info.ChannelCodeSnippetType,
                        ConsentID = new int[] { info.ChannelCodeSnippetConsentID },
                        GTMID = info.ChannelCodeSnippetGTMID,
                        Location = info.ChannelCodeSnippetLocation,
                    };
                }
                return model;
            }
        }

        private readonly IChannelCodeSnippetInfoProvider channelCodeSnippetInfoProvider;

        [PageParameter(typeof(IntPageModelBinder))]
        public int ObjectID { get; set; }

        public CodeSnippetModelEdit(Xperience.Admin.Base.Forms.Internal.IFormItemCollectionProvider formItemCollectionProvider, IFormDataBinder formDataBinder, IChannelCodeSnippetInfoProvider channelCodeSnippetInfoProvider) : base(formItemCollectionProvider, formDataBinder)
        {
            this.channelCodeSnippetInfoProvider = channelCodeSnippetInfoProvider;
        }

        protected override async Task<ICommandResponse> ProcessFormData(CodeSnippetEditModel model, ICollection<IFormItem> formItems)
        {
            var info = channelCodeSnippetInfoProvider.Get(ObjectID);
            info.ChannelCodeSnippetChannelID = model.ChannelID.FirstOrDefault();
            info.ChannelCodeSnippetConsentID = model.ConsentID.FirstOrDefault();
            info.ChannelCodeSnippetCode = model.Code;
            info.ChannelCodeSnippetGTMID = model.GTMID;
            info.ChannelCodeSnippetLocation = model.Location;
            info.ChannelCodeSnippetType = model.SnippetType;
            channelCodeSnippetInfoProvider.Set(info);
            return await base.ProcessFormData(model, formItems);
        }
    }
}
