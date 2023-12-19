using Kentico.Xperience.TagManager.Modules;
using Kentico.Xperience.TagManager.Services;
using Kentico.Xperience.TagManager.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Kentico.Xperience.TagManager
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCustomChannelSettingsModule(this IServiceCollection services)
        {
            services.AddSingleton<ICustomChannelSettingsModuleInstaller, CustomChannelSettingsModuleInstaller>();
            services.AddSingleton<IChannelCodeSnippetsContext, ChannelCodeSnippetsContext>();
            services.AddScoped<IWebsiteChannelPermissionService, WebsiteChannelPermissionService>();
            services.AddTransient<ITagHelperComponent, CodeSnippetTagHelperComponent>();
            return services;
        }
    }
}
