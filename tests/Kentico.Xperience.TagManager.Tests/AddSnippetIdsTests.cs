using System.Reflection;
using System.Text.RegularExpressions;

using NUnit.Framework;

namespace Kentico.Xperience.TagManager.Tests;

[TestFixture]
public class AddSnippetIdsTests
{
    // Using reflection to test the private static method
    private static string InvokeAddSnippetIds(int snippetId, string codeSnippet)
    {
        var assembly = Assembly.Load("Kentico.Xperience.TagManager");
        var type = assembly.GetType("Kentico.Xperience.TagManager.Rendering.DefaultChannelCodeSnippetsService");
        var method = type!.GetMethod("AddSnippetIds", BindingFlags.NonPublic | BindingFlags.Static);
        return (string)method!.Invoke(null, [snippetId, codeSnippet])!;
    }

    [Test]
    public void AddSnippetIds_ShouldNotModifyHtmlComments()
    {
        // Arrange
        int snippetId = 6;
        string input = "<!-- Google Tag Manager -->\n<!-- End Google Tag Manager -->";
        string expected = "<!-- Google Tag Manager -->\n<!-- End Google Tag Manager -->";

        // Act
        string result = InvokeAddSnippetIds(snippetId, input);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void AddSnippetIds_ShouldAddAttributeToRegularTags()
    {
        // Arrange
        int snippetId = 6;
        string input = "<script>alert('test');</script>";
        string expected = "<script data-snippet-id=\"6\">alert('test');</script>";

        // Act
        string result = InvokeAddSnippetIds(snippetId, input);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void AddSnippetIds_ShouldAddAttributeToDiv()
    {
        // Arrange
        int snippetId = 6;
        string input = "<div class=\"container\"></div>";
        string expected = "<div class=\"container\" data-snippet-id=\"6\"></div>";

        // Act
        string result = InvokeAddSnippetIds(snippetId, input);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void AddSnippetIds_ShouldHandleMixedContent()
    {
        // Arrange
        int snippetId = 6;
        string input = @"<!-- Google Tag Manager -->
<script>
(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':
new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],
j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src=
'https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);
})(window,document,'script','dataLayer','GTM-XXXX');
</script>
<!-- End Google Tag Manager -->";

        // Act
        string result = InvokeAddSnippetIds(snippetId, input);

        // Assert
        Assert.That(result, Does.Contain("<!-- Google Tag Manager -->"));
        Assert.That(result, Does.Contain("<!-- End Google Tag Manager -->"));
        Assert.That(result, Does.Not.Contain("<!-- Google Tag Manager -- data-snippet-id"));
        Assert.That(result, Does.Not.Contain("<!-- End Google Tag Manager -- data-snippet-id"));
        Assert.That(result, Does.Contain("<script data-snippet-id=\"6\">"));
    }

    [Test]
    public void AddSnippetIds_ShouldNotModifyClosingTags()
    {
        // Arrange
        int snippetId = 6;
        string input = "<div></div>";
        string expected = "<div data-snippet-id=\"6\"></div>";

        // Act
        string result = InvokeAddSnippetIds(snippetId, input);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void AddSnippetIds_ShouldHandleSelfClosingTags()
    {
        // Arrange
        int snippetId = 6;
        string input = "<img src=\"test.jpg\" />";
        string expected = "<img src=\"test.jpg\"  data-snippet-id=\"6\"/>";

        // Act
        string result = InvokeAddSnippetIds(snippetId, input);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void AddSnippetIds_ShouldHandleMultilineComments()
    {
        // Arrange
        int snippetId = 6;
        string input = @"<!-- This is a 
multiline comment
that should not be modified -->";
        string expected = @"<!-- This is a 
multiline comment
that should not be modified -->";

        // Act
        string result = InvokeAddSnippetIds(snippetId, input);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void AddSnippetIds_ShouldHandleNestedComments()
    {
        // Arrange
        int snippetId = 6;
        string input = "<!-- Comment 1 --><div><!-- Comment 2 --></div><!-- Comment 3 -->";
        string expected = "<!-- Comment 1 --><div data-snippet-id=\"6\"><!-- Comment 2 --></div><!-- Comment 3 -->";

        // Act
        string result = InvokeAddSnippetIds(snippetId, input);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void AddSnippetIds_ShouldHandleDoctype()
    {
        // Arrange
        int snippetId = 6;
        string input = "<!DOCTYPE html>";
        string expected = "<!DOCTYPE html>";

        // Act
        string result = InvokeAddSnippetIds(snippetId, input);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void AddSnippetIds_ShouldHandleConditionalComments()
    {
        // Arrange
        int snippetId = 6;
        string input = "<!--[if IE]><script src=\"ie.js\"></script><![endif]-->";
        // The comment wrapper is preserved but the script tag inside gets the attribute
        string expected = "<!--[if IE]><script src=\"ie.js\" data-snippet-id=\"6\"></script><![endif]-->";

        // Act
        string result = InvokeAddSnippetIds(snippetId, input);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
}
