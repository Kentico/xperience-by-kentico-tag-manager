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

You can also add image which is shown in Administration UI in the dropdown which is used to select type of the snippet.
All you need to do is paste svg code to the `CodeSnippetSettings` constructor in `CreateCodeSnippetSettings` method

You can extend your `ExampleSnippetFactory` like this:

```csharp

//...class definition

    private const string TAG_APPSETTINGS_NAME = "Custom.ExampleSnippet";
    private const string TAG_TYPE_NAME = "Example";
    private const string TAG_DISPLAY_NAME = "Example";

    private const string TAG_SVG_ICON = "<?xml version=\"1.0\" encoding=\"utf-8\"?><!-- Uploaded to: SVG Repo, www.svgrepo.com, Generator: SVG Repo Mixer Tools -->\r\n<svg width=\"40\" height=\"30\" viewBox=\"0 0 24 24\" id=\"code_snippet\" data-name=\"code snippet\" xmlns=\"http://www.w3.org/2000/svg\">\r\n  <rect id=\"Rectangle\" width=\"24\" height=\"24\" fill=\"none\"/>\r\n  <path id=\"Rectangle-2\" data-name=\"Rectangle\" d=\"M0,6.586V0H6.586\" transform=\"translate(2.343 12) rotate(-45)\" fill=\"none\" stroke=\"#000000\" stroke-miterlimit=\"10\" stroke-width=\"1.5\"/>\r\n  <path id=\"Line\" d=\"M4.659,0,0,17.387\" transform=\"translate(9.671 3.307)\" fill=\"none\" stroke=\"#000000\" stroke-linecap=\"square\" stroke-miterlimit=\"10\" stroke-width=\"1.5\"/>\r\n  <path id=\"Rectangle-3\" data-name=\"Rectangle\" d=\"M0,6.586V0H6.586\" transform=\"translate(21.657 12) rotate(135)\" fill=\"none\" stroke=\"#000000\" stroke-miterlimit=\"10\" stroke-width=\"1.5\"/>\r\n</svg>";


    public override CodeSnippetSettings CreateCodeSnippetSettings() =>
        new(TAG_TYPE_NAME, TAG_DISPLAY_NAME, TAG_APPSETTINGS_NAME, TAG_SVG_ICON);

//...other code...
```

Which will result in the following:

![Create Custom Snippet Type](/images/screenshots/create_snippet.png)

You can now add this snippet to `appsettings.json` as specified in the 

See [Using provided snippets](Usage-Guide.md)