using CMS.Localization;
using Kentico.Xperience.TagManager.Admin;
using Kentico.Xperience.TagManager.Rendering;
using Kentico.Xperience.TagManager.Resources;
using Microsoft.AspNetCore.Razor.TagHelpers;

[assembly: RegisterLocalizationResource(markedType: typeof(Localization), cultureCodes: "en-us")]

namespace Microsoft.Extensions.DependencyInjection;

public static class TagManagerServiceCollectionExtensions
{
    public static IServiceCollection AddTagManager(this IServiceCollection services)
    {
        services.AddSingleton<ITagManagerModuleInstaller, TagManagerModuleInstaller>();
        services.AddSingleton<IChannelCodeSnippetsService, ChannelCodeSnippetsService>();
        services.AddScoped<IWebsiteChannelPermissionService, WebsiteChannelPermissionService>();
        services.AddTransient<ITagHelperComponent, CodeSnippetTagHelperComponent>();
        return services;
    }
}
