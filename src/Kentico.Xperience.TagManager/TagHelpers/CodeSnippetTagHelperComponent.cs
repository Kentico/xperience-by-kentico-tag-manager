using Kentico.Xperience.TagManager.Enums;
using Kentico.Xperience.TagManager.Helpers;
using Kentico.Xperience.TagManager.Models;
using Kentico.Xperience.TagManager.Services;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Json;

namespace Kentico.Xperience.TagManager.TagHelpers
{
    public class CodeSnippetTagHelperComponent : TagHelperComponent
    {
        public override int Order => 1;

        private readonly IChannelCodeSnippetsContext codeSnippetsContext;

        public CodeSnippetTagHelperComponent(IChannelCodeSnippetsContext codeSnippetsContext)
        {
            this.codeSnippetsContext = codeSnippetsContext;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var codeSnippets = await codeSnippetsContext.GetCodeSnippets();
            if (string.Equals(context.TagName, "head",
                       StringComparison.OrdinalIgnoreCase))
            {
                foreach (var codeSnippet in FilterCustomSnippetsByLocation(codeSnippets, CodeSnippetLocations.HeadTop))
                {
                    output.PreContent.AppendHtml(codeSnippet.Code);
                }
                foreach (var codeSnippet in FilterCustomSnippetsByLocation(codeSnippets, CodeSnippetLocations.HeadBottom))
                {
                    output.PostContent.AppendHtml(codeSnippet.Code);
                }
                foreach (var gtm in FilterGTMSnippets(codeSnippets))
                {
                    output.PostContent.AppendHtml(CodeSnippetHelper.GenerateGTMHeadScript(gtm.GTMId));
                }
            }
            if (string.Equals(context.TagName, "body",
                               StringComparison.OrdinalIgnoreCase))
            {
                foreach (var codeSnippet in FilterCustomSnippetsByLocation(codeSnippets, CodeSnippetLocations.BodyTop))
                {
                    output.PreContent.AppendHtml(CodeSnippetHelper.WrapCodeSnippet(codeSnippet.Code, codeSnippet.ID));
                }
                foreach (var codeSnippet in FilterCustomSnippetsByLocation(codeSnippets, CodeSnippetLocations.BodyBottom))
                {
                    output.PostContent.AppendHtml(CodeSnippetHelper.WrapCodeSnippet(codeSnippet.Code, codeSnippet.ID));
                }
                foreach (var gtm in FilterGTMSnippets(codeSnippets))
                {
                    output.PreContent.AppendHtml(CodeSnippetHelper.WrapCodeSnippet(CodeSnippetHelper.GenerateGTMBodyScript(gtm.GTMId), gtm.ID));
                }

                output.PostContent.AppendHtml(CreateIdsWrapper(codeSnippets.Select(c => c.ID).ToArray()));
                output.PostContent.AppendHtml(GetScriptSrcTag());
            }

        }

        private IEnumerable<ChannelCodeSnippetDto> FilterCustomSnippetsByLocation(IEnumerable<ChannelCodeSnippetDto> codeSnippets, CodeSnippetLocations location) => codeSnippets
                .Where(x => x != null && x.Location == location);

        private IEnumerable<ChannelCodeSnippetDto> FilterGTMSnippets(IEnumerable<ChannelCodeSnippetDto> codeSnippets) => codeSnippets
                .Where(c => !string.IsNullOrEmpty(c.GTMId));

        private IHtmlContent CreateIdsWrapper(int[] ids)
        {
            var divTag = new TagBuilder("div");
            divTag.Attributes.Add("id", "codeSnippets_initIds");
            divTag.Attributes.Add("data-ids", JsonSerializer.Serialize(ids));
            return divTag;
        }

        private IHtmlContent GetScriptSrcTag()
        {
            var scriptTag = new TagBuilder("script");
            scriptTag.Attributes.Add("type", "text/javascript");
            scriptTag.Attributes.Add("src", "~/_content/Kentico.Xperience.TagManager/Scripts/main.js");
            return scriptTag;
        }
    }
}
