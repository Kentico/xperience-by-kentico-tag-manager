using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;

namespace Kentico.Xperience.TagManager.Helpers
{
    public static class CodeSnippetHelper
    {
        public static IHtmlContent GenerateGTMHeadScript(string gtmID)
        {
            var tagBuilder = new TagBuilder("script");
            tagBuilder.InnerHtml.AppendHtml("""
                (function (w, d, s, l, i) {
                    w[l] = w[l] || [];
                    w[l].push({
                        'gtm.start':
                            new Date().getTime(),
                        event: 'gtm.js'
                    });
                    var f = d.getElementsByTagName(s)[0],
                        j = d.createElement(s),
                        dl = l != 'dataLayer' ? '&l=' + l : '';
                    j.async = true;
                    j.src =
                        'https://www.googletagmanager.com/gtm.js?id=' + i + dl;
                    var n = d.querySelector('[nonce]');
                    n && j.setAttribute('nonce', n.nonce || n.getAttribute('nonce'));
                    f.parentNode.insertBefore(j, f);
                })(window, document, 'script', 'dataLayer', 
                """ + gtmID + ");");
            return tagBuilder;
        }

        public static IHtmlContent GenerateGTMBodyScript(string gtmID)
        {
            var tagBuilder = new TagBuilder("noscript");
            tagBuilder.InnerHtml.AppendHtml(string.Format("""  
                    <iframe src="https://www.googletagmanager.com/ns.html?id={0}"
                        height="0" width="0" style="display:none;visibility:hidden" title="GTMNoScript">
                    </iframe>
                """, gtmID));
            return tagBuilder;
        }

        public static IHtmlContent WrapCodeSnippet(string content, int snippetId)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.Attributes.Add("id", GetWrapperID(snippetId));
            tagBuilder.InnerHtml.AppendHtml(content);
            return tagBuilder;
        }

        public static string GetWrapperID(int snippetId) => $"codeSnippet_{snippetId}";

        public static IHtmlContent WrapCodeSnippet(IHtmlContent content, int snippetId)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.Attributes.Add("id", $"codeSnippet_{snippetId}");
            tagBuilder.InnerHtml.AppendHtml(content);
            return tagBuilder;
        }

        public static string GetInnerHtml(this IHtmlContent content)
        {
            using (var writer = new StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}
