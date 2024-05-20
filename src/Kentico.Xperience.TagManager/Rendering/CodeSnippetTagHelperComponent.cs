using CMS.ContactManagement;

using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
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
    private readonly IHttpContextAccessor httpContextAccessor;

    public CodeSnippetTagHelperComponent(
        IChannelCodeSnippetsService codeSnippetsContext,
        IUrlHelperFactory urlHelperFactory,
        IFileVersionProvider fileVersionProvider,
        IHttpContextAccessor httpContextAccessor)
    {
        this.codeSnippetsContext = codeSnippetsContext;
        this.urlHelperFactory = urlHelperFactory;
        this.httpContextAccessor = httpContextAccessor;
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

        var codeSnippets = await codeSnippetsContext.GetConsentedCodeSnippets(contact);

        if (string.Equals(context.TagName, HeadTag, StringComparison.OrdinalIgnoreCase))
        {
            ProcessHead(output, codeSnippets, httpContextAccessor.HttpContext);
        }

        if (string.Equals(context.TagName, BodyTag, StringComparison.OrdinalIgnoreCase))
        {
            ProcessBody(output, codeSnippets, httpContextAccessor.HttpContext);
        }
    }

    private static void ProcessHead(
        TagHelperOutput output,
        ILookup<CodeSnippetLocations, CodeSnippetDto> codeSnippets,
        HttpContext? httpContext)
    {
        bool isEditMode = httpContext.Kentico().PageBuilder().EditMode;
        bool isPreviewMode = httpContext.Kentico().Preview().Enabled;

        var headTopSnippets = codeSnippets[CodeSnippetLocations.HeadTop];
        var headBottomSnippets = codeSnippets[CodeSnippetLocations.HeadBottom];

        if (isEditMode)
        {
            headTopSnippets = headTopSnippets.Where(x => x.DisplayMode is CodeSnippetAdministrationDisplayMode.Both or
                CodeSnippetAdministrationDisplayMode.PageBuilderOnly);

            headBottomSnippets = headBottomSnippets.Where(x => x.DisplayMode is CodeSnippetAdministrationDisplayMode.Both or
                CodeSnippetAdministrationDisplayMode.PageBuilderOnly);
        }
        else if (isPreviewMode)
        {
            headTopSnippets = headTopSnippets.Where(x => x.DisplayMode is CodeSnippetAdministrationDisplayMode.Both or
               CodeSnippetAdministrationDisplayMode.PreviewOnly);

            headBottomSnippets = headBottomSnippets.Where(x => x.DisplayMode is CodeSnippetAdministrationDisplayMode.Both or
                CodeSnippetAdministrationDisplayMode.PreviewOnly);
        }

        foreach (var codeSnippet in headTopSnippets)
        {
            output.PreContent.AppendHtml(codeSnippet.Code);
        }

        foreach (var codeSnippet in headBottomSnippets)
        {
            output.PostContent.AppendHtml(codeSnippet.Code);
        }
    }

    private void ProcessBody(
        TagHelperOutput output,
        ILookup<CodeSnippetLocations, CodeSnippetDto> codeSnippets,
        HttpContext? httpContext)
    {
        bool isEditMode = httpContext.Kentico().PageBuilder().EditMode;
        bool isPreviewMode = httpContext.Kentico().Preview().Enabled;

        var bodyTopSnippets = codeSnippets[CodeSnippetLocations.BodyTop];
        var bodyBottomSnippets = codeSnippets[CodeSnippetLocations.BodyBottom];

        if (isEditMode)
        {
            bodyTopSnippets = bodyTopSnippets.Where(x => x.DisplayMode is CodeSnippetAdministrationDisplayMode.Both or
                CodeSnippetAdministrationDisplayMode.PageBuilderOnly);

            bodyBottomSnippets = bodyBottomSnippets.Where(x => x.DisplayMode is CodeSnippetAdministrationDisplayMode.Both or
                CodeSnippetAdministrationDisplayMode.PageBuilderOnly);
        }
        else if (isPreviewMode)
        {
            bodyTopSnippets = bodyTopSnippets.Where(x => x.DisplayMode is CodeSnippetAdministrationDisplayMode.Both or
                CodeSnippetAdministrationDisplayMode.PreviewOnly);
            
            bodyBottomSnippets = bodyBottomSnippets.Where(x => x.DisplayMode is CodeSnippetAdministrationDisplayMode.Both or
                CodeSnippetAdministrationDisplayMode.PreviewOnly);
        }

        foreach (var codeSnippet in bodyTopSnippets)
        {
            output.PreContent.AppendHtml(codeSnippet.Code);
        }

        foreach (var codeSnippet in bodyBottomSnippets)
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
                    urlHelper.Content("~/_content/Kentico.Xperience.TagManager/js/ktc-tagmanager.js"))
            }
        };

        return scriptTag;
    }
}
