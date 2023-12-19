using GTM;
using Microsoft.AspNetCore.Mvc;
using CMS.Websites.Routing;
using CMS.Websites;
using Kentico.Xperience.TagManager.Helpers;
using Kentico.Xperience.TagManager.Enums;
using Kentico.Xperience.TagManager.Models;
using Kentico.Xperience.TagManager.Services;

namespace Kentico.Xperience.TagManager.Controllers
{
    [Route("/gtm/[action]")]
    public class GTMController : Controller
    {
        private readonly IChannelCodeSnippetsContext channelCodeSnippetsContext;
        private readonly IChannelCodeSnippetInfoProvider channelCodeSnippetInfoProvider;
        private readonly IWebsiteChannelContext websiteChannelContext;

        public GTMController(IChannelCodeSnippetsContext channelCodeSnippetsContext, IChannelCodeSnippetInfoProvider channelCodeSnippetInfoProvider, IWebsiteChannelContext websiteChannelContext)
        {
            this.channelCodeSnippetsContext = channelCodeSnippetsContext;
            this.channelCodeSnippetInfoProvider = channelCodeSnippetInfoProvider;
            this.websiteChannelContext = websiteChannelContext;
        }

        [HttpPost]
        public async Task<UpdateCodeSnippetsResultModel> UpdateCodeSnippets([FromBody] RequestModel requestModel)
        {
            var codeSnippets = await channelCodeSnippetsContext.GetCodeSnippets();
            var codeSnippetsIds = codeSnippets.Select(c => c.ID).ToArray();
            var newCodeSnippetModels = CreateCodeSnippetModels(codeSnippets.Where(c => !requestModel.Ids.Contains(c.ID)));

            var shouldBeRemoved = requestModel.Ids.Where(initializedId => !codeSnippetsIds.Contains(initializedId)).ToArray();
            var model = new UpdateCodeSnippetsResultModel()
            {
                CodeSnippetsIDs = codeSnippetsIds,
                NewCodeSnippets = newCodeSnippetModels,
                CodeSnippetsToRemove = GetCodesSnippetsToRemove(shouldBeRemoved)
            };
            return model;
        }

        private IList<CodeSnippetToRemove> GetCodesSnippetsToRemove(int[] ids)
        {
            var channelId = WebsiteChannelInfoProvider.ProviderObject.Get(websiteChannelContext.WebsiteChannelID).WebsiteChannelChannelID;
            var resultList = new List<CodeSnippetToRemove>();
            var snippets = channelCodeSnippetInfoProvider.Get()
                .WhereIn(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetID), ids)
                .WhereEquals(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetChannelID), channelId)
                .Columns(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetCode),
                    nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetID),
                    nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetGTMID),
                    nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetLocation))
                .ToList();
            foreach (var snippet in snippets)
            {
                if (!string.IsNullOrEmpty(snippet.ChannelCodeSnippetGTMID))
                {
                    resultList.Add(new CodeSnippetToRemove
                    {
                        Location = nameof(CodeSnippetLocations.HeadBottom),
                        Code = CodeSnippetHelper.GenerateGTMHeadScript(snippet.ChannelCodeSnippetGTMID).GetInnerHtml()
                    });
                    resultList.Add(new CodeSnippetToRemove
                    {
                        Location = nameof(CodeSnippetLocations.BodyTop),
                        WrapperID = CodeSnippetHelper.GetWrapperID(snippet.ChannelCodeSnippetID)
                    });
                }
                else
                {
                    resultList.Add(new CodeSnippetToRemove
                    {
                        Location = snippet.ChannelCodeSnippetLocation,
                        Code = snippet.ChannelCodeSnippetCode,
                        WrapperID = CodeSnippetHelper.GetWrapperID(snippet.ChannelCodeSnippetID)
                    });
                }
            }
            return resultList;

        }

        private IList<ChannelCodeSnippetModel> CreateCodeSnippetModels(IEnumerable<ChannelCodeSnippetDto> codeSnippets)
        {
            var resultList = new List<ChannelCodeSnippetModel>();
            foreach (var dto in codeSnippets)
            {
                if (!string.IsNullOrEmpty(dto.GTMId))
                {
                    resultList.Add(new ChannelCodeSnippetModel
                    {
                        ID = dto.ID,
                        Location = nameof(CodeSnippetLocations.HeadBottom),
                        Code = CodeSnippetHelper.GenerateGTMHeadScript(dto.GTMId).GetInnerHtml()
                    });
                    resultList.Add(new ChannelCodeSnippetModel
                    {
                        ID = dto.ID,
                        Location = nameof(CodeSnippetLocations.BodyTop),
                        Code = CodeSnippetHelper.WrapCodeSnippet(CodeSnippetHelper.GenerateGTMBodyScript(dto.GTMId), dto.ID).GetInnerHtml()
                    });
                }
                else if (!string.IsNullOrEmpty(dto.Code) && dto.Location.HasValue)
                {
                    var code = dto.Code;
                    if (dto.Location.Value == CodeSnippetLocations.BodyTop || dto.Location.Value == CodeSnippetLocations.BodyBottom)
                    {
                        code = CodeSnippetHelper.WrapCodeSnippet(code, dto.ID).GetInnerHtml();
                    }
                    resultList.Add(new ChannelCodeSnippetModel
                    {
                        ID = dto.ID,
                        Location = dto.Location.Value.ToString(),
                        Code = code
                    });
                }
            }
            return resultList;
        }
    }

}
