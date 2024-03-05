using Kentico.Xperience.TagManager.Rendering;

namespace Kentico.Xperience.TagManager.Snippets;

internal class IntercomSnippetFactory : AbstractSnippetFactory
{
    private const string TAG_APPSETTINGS_NAME = "Kentico.Intercom";
    private const string TAG_TYPE_NAME = "Intercom";
    private const string TAG_DISPLAY_NAME = "Intercom";

    public override CodeSnippetSettings CreateCodeSnippetSettings() =>
        new(TAG_TYPE_NAME, TAG_DISPLAY_NAME, TAG_APPSETTINGS_NAME);

    public override IEnumerable<CodeSnippet> CreateCodeSnippets(string thirdPartyIdentifier) =>
        new List<CodeSnippet>
        {
            new (GenerateScript(thirdPartyIdentifier), CodeSnippetLocations.HeadBottom),
        };

    private static string GenerateScript(string identifier) =>
    $$"""
        <script>
            window.intercomSettings = {
                app_id: "{{identifier}}"
            };
            (function() {
                var w=window;var ic=w.Intercom;if(typeof ic==="function"){ic('reattach_activator');ic('update',w.intercomSettings);}else{var d=document;var i=function(){i.c(arguments);};i.q=[];i.c=function(args){i.q.push(args);};w.Intercom=i;var l=function(){var s=d.createElement('script');s.type='text/javascript';s.async=true;s.src='https://widget.intercom.io/widget/YOUR_APP_ID';var x=d.getElementsByTagName('script')[0];x.parentNode.insertBefore(s,x);};if(w.attachEvent){w.attachEvent('onload',l);}else{w.addEventListener('load',l,false);}{{'}'}}
            })();
        </script>
    """;
}
