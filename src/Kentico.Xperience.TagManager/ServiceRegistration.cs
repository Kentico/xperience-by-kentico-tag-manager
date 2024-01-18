using CMS.Localization;
using Kentico.Xperience.TagManager.Modules;
using Kentico.Xperience.TagManager.Resources;
using Kentico.Xperience.TagManager.Services;
using Kentico.Xperience.TagManager.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

[assembly: RegisterLocalizationResource(markedType: typeof(Localization), cultureCodes: "en-us")]

namespace Kentico.Xperience.TagManager;

public static class ServiceRegistration
{
    public static IServiceCollection AddCustomChannelSettingsModule(this IServiceCollection services)
    {
        services.AddSingleton<ICustomChannelSettingsModuleInstaller, CustomChannelSettingsModuleInstaller>();
        services.AddSingleton<IChannelCodeSnippetsService, ChannelCodeSnippetsService>();
        services.AddScoped<IWebsiteChannelPermissionService, WebsiteChannelPermissionService>();
        services.AddTransient<ITagHelperComponent, CodeSnippetTagHelperComponent>();
        return services;
    }
}
