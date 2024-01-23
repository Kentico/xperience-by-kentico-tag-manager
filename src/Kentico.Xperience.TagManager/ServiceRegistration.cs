using CMS.Localization;
using Kentico.Xperience.TagManager.Modules;
using Kentico.Xperience.TagManager.Resources;
using Kentico.Xperience.TagManager.Services;
using Kentico.Xperience.TagManager.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

[assembly: RegisterLocalizationResource(markedType: typeof(Localization), cultureCodes: "en-us")]

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddTagManager(this IServiceCollection services)
    {
        services.AddSingleton<ICustomChannelSettingsModuleInstaller, CustomChannelSettingsModuleInstaller>();
        services.AddSingleton<IChannelCodeSnippetsService, ChannelCodeSnippetsService>();
        services.AddScoped<IWebsiteChannelPermissionService, WebsiteChannelPermissionService>();
        services.AddTransient<ITagHelperComponent, CodeSnippetTagHelperComponent>();
        return services;
    }
}
