using System.Text.Json;
using System.Text.Json.Serialization;
using Kentico.Xperience.TagManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kentico.Xperience.TagManager.Controllers;

[Route("/kentico.tagmanager/gtm/[action]")]
public sealed class GtmController : Controller
{
    private readonly IChannelCodeSnippetsService channelCodeSnippetsContext;

    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    public GtmController(IChannelCodeSnippetsService channelCodeSnippetsContext) => this.channelCodeSnippetsContext = channelCodeSnippetsContext;


    [HttpPost]
    public async Task<IActionResult> UpdateCodeSnippets()
    {
        var codeSnippets = await channelCodeSnippetsContext.GetCodeSnippets();
        return Json(codeSnippets.SelectMany(s => s).ToArray(), jsonSerializerOptions);
    }
}
