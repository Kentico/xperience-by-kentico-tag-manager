using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Kentico.Xperience.TagManager.Services;

namespace Kentico.Xperience.TagManager.Controllers
{
    [Route("/gtm/[action]")]
    public class GTMController : Controller
    {
        private readonly IChannelCodeSnippetsService channelCodeSnippetsContext;

        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };

        public GTMController(
            IChannelCodeSnippetsService channelCodeSnippetsContext)
        {
            this.channelCodeSnippetsContext = channelCodeSnippetsContext;
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCodeSnippets()
        {
            var codeSnippets = await channelCodeSnippetsContext.GetCodeSnippets();
            return Json(codeSnippets.SelectMany(s => s).ToArray(), jsonSerializerOptions);
        }
    }
}
