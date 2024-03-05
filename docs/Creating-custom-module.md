# Create a custom Snippet Factory

If you wish to add your own custom snippet type there are two options.
You can either Chose `Custom Snippet` Code snippet type in the UI edit or create page.

Or you can create your own type which will be displayed in the Administration Snippet type dropdown.

If you wish to create your own type:

Create `ISnippetFactory`. You can inherit prepared `AbstractSnippetFactory` and override it's methods which specify the type, displayed name and appsettings snippet name and used script injected into each application.

Create `ExampleSnippetFactory`

```csharp
public class ExampleSnippetFactory : AbstractSnippetFactory
{
    private const string TAG_APPSETTINGS_NAME = "Custom.ExampleSnippet";
    private const string TAG_TYPE_NAME = "Example";
    private const string TAG_DISPLAY_NAME = "Example";

    public override CodeSnippetSettings CreateCodeSnippetSettings() =>
        new(TAG_TYPE_NAME, TAG_DISPLAY_NAME, TAG_APPSETTINGS_NAME);

    public override IEnumerable<CodeSnippet> CreateCodeSnippets(string thirdPartyIdentifier) =>
        new List<CodeSnippet>
        {
            new (GenerateScript(), CodeSnippetLocations.HeadBottom),
        };

    private static string GenerateScript() =>
     $"""
         <script>
             alert("Custom script message!")
         </script>
     """;
}

```

You can create an `IEnumerable<CodeSnippet>` of multiple `CodeSnippet`s. You specify the location where the snippet should be added in the html file.

You can use the identifier which you specify in the administration.

Now you add it to startup.

```csharp
builder.Services.AddKenticoTagManager(builder.Configuration, builder =>
{
    builder.AddSnippetFactory<ExampleSnippetFactory>();
});
```

This registers all default snippet types and adds our `ExampleSnippetFactory`.

You can now add this snippet to `appsettings.json` as specified in the 

See [Using provided snippets](Usage-Guide.md)