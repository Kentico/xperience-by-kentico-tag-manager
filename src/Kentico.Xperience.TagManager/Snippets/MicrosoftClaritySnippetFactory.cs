namespace Kentico.Xperience.TagManager.Snippets;

internal class MicrosoftClaritySnippetFactory : AbstractSnippetFactory
{
    private const string TAG_TYPE_NAME = "MicrosoftClarity";
    private const string TAG_DISPLAY_NAME = "Microsoft Clarity";

    public override CodeSnippetSettings CreateCodeSnippetSettings() =>
        new(TAG_TYPE_NAME, TAG_DISPLAY_NAME);

    public override IEnumerable<CodeSnippet> CreateCodeSnippets(string thirdPartyIdentifier) =>
      new List<CodeSnippet>
      {
            new (GenerateScript(thirdPartyIdentifier), CodeSnippetLocations.HeadBottom),
      };

    private static string GenerateScript(string identifier) =>
      $$"""
       <script type="text/javascript">
          (function(c,l,a,r,i,t,y){
              c[a]=c[a]||function(){(c[a].q=c[a].q||[]).push(arguments)};
              t=l.createElement(r);t.async=1;t.src="https://www.clarity.ms/tag/"+i;
              y=l.getElementsByTagName(r)[0];y.parentNode.insertBefore(t,y);
          })(window, document, "clarity", "script", "{{identifier}}");
      </script>
      """;
}
