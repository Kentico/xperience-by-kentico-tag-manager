using System.Text.Json;
using System.Text.Json.Serialization;

using CMS.ContactManagement;

using Microsoft.AspNetCore.Mvc;

namespace Kentico.Xperience.TagManager.Rendering;

[ApiController]
[Route("/kentico.tagmanager/[action]")]
public sealed class TagManagerController : Controller
{
    private readonly IChannelCodeSnippetsService channelCodeSnippetsContext;

    public TagManagerController(IChannelCodeSnippetsService channelCodeSnippetsContext) => this.channelCodeSnippetsContext = channelCodeSnippetsContext;

    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>
    /// Returns the current collection of consented tags
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetConsentedTags()
    {
        var codeSnippets = await channelCodeSnippetsContext.GetConsentedCodeSnippets(ContactManagementContext.CurrentContact);

        return Json(codeSnippets.SelectMany(s => s).ToArray(), jsonSerializerOptions);
    }
}
