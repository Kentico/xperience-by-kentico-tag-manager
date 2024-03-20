using Kentico.Xperience.TagManager.Rendering;

namespace Kentico.Xperience.TagManager.Snippets;

internal class VwoABTestingCodeSnippetFactory : AbstractSnippetFactory
{
    private const string TAG_APPSETTINGS_NAME = "Kentico.VWO";
    private const string TAG_TYPE_NAME = "VWO";
    private const string TAG_DISPLAY_NAME = "VWO";
    private const string TAG_SVG_ICON = "<svg width=\"40\" height=\"30\" viewBox=\"0 0 279 96\" xmlns=\"http://www.w3.org/2000/svg\">\r\n    <g fill=\"none\" fill-rule=\"evenodd\">\r\n        <path d=\"m229.76 0c27.19 0 49.24 21.49 49.24 48 0 26.51-22.04 48-49.24 48-22.903 0-42.15-15.245-47.656-35.895l4.365 2.763 2.264-3.398c5.108 17.397 21.543 30.13 41.03 30.13 23.566 0 42.671-18.625 42.671-41.6 0-22.975-19.1-41.6-42.671-41.6-2.02 0-4 .136-5.944.4l4.126-6.195-.847-.536c.882-.046 1.771-.069 2.664-.069\" fill=\"#BF3078\"/>\r\n        <path d=\"m180.53 0h49.24l-65.65 96-24.618-36-24.618 36-24.895-36.406 3.893-5.845 21 30.713 19.16-28.01 5.462-7.988 5.462 7.988 19.16 28.01 53.38-78.06h-36.969-55.11l4.263-6.4h50.845\" fill=\"#802050\"/>\r\n        <path d=\"m65.65 96l-65.65-96h131.29l-65.65 96m0-11.538l53.38-78.06h-106.76l53.38 78.06\" fill=\"#26134D\"/>\r\n    </g>\r\n</svg>";

    public override CodeSnippetSettings CreateCodeSnippetSettings() =>
        new(TAG_TYPE_NAME, TAG_DISPLAY_NAME, TAG_APPSETTINGS_NAME, TAG_SVG_ICON);

    public override IEnumerable<CodeSnippet> CreateCodeSnippets(string thirdPartyIdentifier) =>
      new List<CodeSnippet>
      {
            new (GenerateScript(thirdPartyIdentifier), CodeSnippetLocations.HeadBottom),
      };

    private static string GenerateScript(string identifier) =>
    $$"""
    <!-- Start VWO Async SmartCode -->
        <link rel="preconnect" href="https://dev.visualwebsiteoptimizer.com"/>
        <script type='text/javascript' id='vwoCode'>
            window._vwo_code || (function() {
            var account_id={{identifier}},
            version=2.0,
            settings_tolerance=2000,
            hide_element='body',
            hide_element_style = 'opacity:0 !important;filter:alpha(opacity=0) !important;background:none !important',
            /* DO NOT EDIT BELOW THIS LINE */
            f=false,w=window,d=document,v=d.querySelector('#vwoCode'),cK='_vwo_'+account_id+'_settings',cc={};try{var c=JSON.parse(localStorage.getItem('_vwo_'+account_id+'_config'));cc=c&&typeof c==='object'?c:{{{'}'}}}catch(e){}var stT=cc.stT==='session'?w.sessionStorage:w.localStorage;code={use_existing_jquery:function(){return typeof use_existing_jquery!=='undefined'?use_existing_jquery:undefined},library_tolerance:function(){return typeof library_tolerance!=='undefined'?library_tolerance:undefined},settings_tolerance:function(){return cc.sT||settings_tolerance},hide_element_style:function(){return'{'+(cc.hES||hide_element_style)+'}'},hide_element:function(){return typeof cc.hE==='string'?cc.hE:hide_element},getVersion:function(){return version},finish:function(){if(!f){f=true;var e=d.getElementById('_vis_opt_path_hides');if(e)e.parentNode.removeChild(e)}{{'}'}},finished:function(){return f},load:function(e){var t=this.getSettings(),n=d.createElement('script'),i=this;if(t){n.textContent=t;d.getElementsByTagName('head')[0].appendChild(n);if(!w.VWO||VWO.caE){stT.removeItem(cK);i.load(e){{'}'}}}else{n.fetchPriority='high';n.src=e;n.type='text/javascript';n.onerror=function(){_vwo_code.finish()};d.getElementsByTagName('head')[0].appendChild(n){{'}'}}},getSettings:function(){try{var e=stT.getItem(cK);if(!e){return}e=JSON.parse(e);if(Date.now()>e.e){stT.removeItem(cK);return}return e.s}catch(e){return{{'}'}}},init:function(){if(d.URL.indexOf('__vwo_disable__')>-1)return;var e=this.settings_tolerance();w._vwo_settings_timer=setTimeout(function(){_vwo_code.finish();stT.removeItem(cK)},e);var t=d.currentScript,n=d.createElement('style'),i=this.hide_element(),r=t&&!t.async&&i?i+this.hide_element_style():'',c=d.getElementsByTagName('head')[0];n.setAttribute('id','_vis_opt_path_hides');v&&n.setAttribute('nonce',v.nonce);n.setAttribute('type','text/css');if(n.styleSheet)n.styleSheet.cssText=r;else n.appendChild(d.createTextNode(r));c.appendChild(n);this.load('https://dev.visualwebsiteoptimizer.com/j.php?a='+account_id+'&u='+encodeURIComponent(d.URL)+'&vn='+version){{'}'}}};w._vwo_code=code;code.init();})();
        </script>
    <!-- End VWO Async SmartCode -->
    """;
}
