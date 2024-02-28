using CMS.ContactManagement;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Kentico.Xperience.TagManager.Rendering;

internal class CodeSnippetTagHelperComponent : TagHelperComponent
{
    private const string HeadTag = "head";
    private const string BodyTag = "body";

    public override int Order => 1;

    private readonly IChannelCodeSnippetsService codeSnippetsContext;
    private readonly IUrlHelperFactory urlHelperFactory;
    private readonly IFileVersionProvider fileVersionProvider;

    public CodeSnippetTagHelperComponent(
        IChannelCodeSnippetsService codeSnippetsContext,
        IUrlHelperFactory urlHelperFactory,
        IFileVersionProvider fileVersionProvider)
    {
        this.codeSnippetsContext = codeSnippetsContext;
        this.urlHelperFactory = urlHelperFactory;
        this.fileVersionProvider = fileVersionProvider;
    }

    /// <summary>
    /// The <see cref="ViewContext"/>.
    /// </summary>
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var contact = ContactManagementContext.CurrentContact;

        if (string.Equals(context.TagName, HeadTag, StringComparison.OrdinalIgnoreCase))
        {
            ProcessHead(output, await codeSnippetsContext.GetConsentedCodeSnippets(contact));
        }

        if (string.Equals(context.TagName, BodyTag, StringComparison.OrdinalIgnoreCase))
        {
            ProcessBody(output, await codeSnippetsContext.GetConsentedCodeSnippets(contact));
        }
    }

    private static void ProcessHead(
        TagHelperOutput output,
        ILookup<CodeSnippetLocations, CodeSnippetDto> codeSnippets)
    {
        foreach (var codeSnippet in codeSnippets[CodeSnippetLocations.HeadTop])
        {
            output.PreContent.AppendHtml(codeSnippet.Code);
        }

        foreach (var codeSnippet in codeSnippets[CodeSnippetLocations.HeadBottom])
        {
            output.PostContent.AppendHtml(codeSnippet.Code);
        }
    }

    private void ProcessBody(
        TagHelperOutput output,
        ILookup<CodeSnippetLocations, CodeSnippetDto> codeSnippets)
    {
        foreach (var codeSnippet in codeSnippets[CodeSnippetLocations.BodyTop])
        {
            output.PreContent.AppendHtml(codeSnippet.Code);
        }

        foreach (var codeSnippet in codeSnippets[CodeSnippetLocations.BodyBottom])
        {
            output.PostContent.AppendHtml(codeSnippet.Code);
        }

        output.PostContent.AppendHtml(GetScriptSrcTag());
    }

    private IHtmlContent GetScriptSrcTag()
    {
        var urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
        var scriptTag = new TagBuilder("script")
        {
            Attributes =
            {
                ["type"] = "module",
                ["src"] = fileVersionProvider.AddFileVersionToPath(
                    ViewContext.HttpContext.Request.PathBase,
                    urlHelper.Content("~/_content/Kentico.Xperience.TagManager/Scripts/ktc-tagmanager.js"))
            }
        };

        return scriptTag;
    }
}
