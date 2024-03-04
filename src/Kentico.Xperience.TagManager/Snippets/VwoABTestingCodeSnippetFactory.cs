namespace Kentico.Xperience.TagManager.Snippets;

public class VwoABTestingCodeSnippetFactory : AbstractSnippetFactory
{
    private const string TAG_TYPE_NAME = "VWO";
    private const string TAG_DISPLAY_NAME = "Google Tag Manager";

    public override CodeSnippetSettings CreateCodeSnippetSettings() =>
        new(TAG_TYPE_NAME, TAG_DISPLAY_NAME);

    public override IEnumerable<CodeSnippet> CreateCodeSnippets(string thirdPartyIdentifier) =>
      new List<CodeSnippet>
      {
            new (GenerateScript(thirdPartyIdentifier), CodeSnippetLocations.HeadBottom),
      };

    private static string GenerateScript(string identifier) =>
    $$"""
    <script type='text/javascript'>
        (function(){
        var settings = {
            async: true,
            attr: { },
            url: "//dev.visualwebsiteoptimizer.com/j.php",
            a: "{{identifier}}"
        };
        window._vwo_code = window._vwo_code || (function(){
        var account_id=settings.a, settings_tolerance=2000, library_tolerance=2500, use_existing_jquery=false, 
        /* DO NOT EDIT BELOW THIS LINE */
        f=false,d=document;return{use_existing_jquery:function(){return use_existing_jquery;}, library_tolerance:function(){return library_tolerance;},
        settings_tolerance:function(){return settings_tolerance;},account_id:function(){return account_id;},end:function(){if(!f){f=true;var a=d.getElementById('_vis_opt_path_hides');if(a)a.parentNode.removeChild(a);{{'}'}}},init:function(){
        var a=d.createElement('script');a.type='text/javascript';a.setAttribute('async','true');a.setAttribute('src',settings.url+'?a='+settings.a+'&url='+encodeURIComponent(d.URL)+'&random='+Math.random()+'&f='+settings.attr.f+(settings.attr.u?'&u='+settings.attr.u:''));d.getElementsByTagName('head')[0].appendChild(a);},init_again:function(e){if(!e)return;var t=d.createElement('script');t.type='text/javascript';t.async=!f;t.setAttribute('src',settings.url+'a='+settings.a+'&url='+encodeURIComponent(settings.url)+'&random='+Math.random()+'&f='+e.f+(e.u?'&u='+e.u:''));d.getElementsByTagName('head')[0].appendChild(t);}{{'}'}};})();
        window._vwo_settings_timer = window._vwo_settings_timer || setTimeout(function(){window._vwo_code.end();}, settings.settings_tolerance);
        window._vwo_code.init();
        })();
    </script>
    """;
}
