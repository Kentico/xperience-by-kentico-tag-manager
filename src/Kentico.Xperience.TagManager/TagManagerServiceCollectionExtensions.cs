using Kentico.Xperience.TagManager.Admin;
using Kentico.Xperience.TagManager.Rendering;
using Kentico.Xperience.TagManager.Snippets;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class TagManagerServiceCollectionExtensions
{
    /// <summary>
    /// Adds Tag Manager and custom module to application.
    /// The <see cref="CustomSnippetFactory"/>
    /// , <see cref="GoogleTagManagerSnippetFactory"/>
    /// , <see cref="GoogleAnalyticsSnippetFactory"/>
    /// , <see cref="MicrosoftClaritySnippetFactory"/>
    /// , <see cref="VwoABTestingCodeSnippetFactory"/>
    /// , <see cref="IntercomSnippetFactory"/>
    /// will be available as an explicitly selectable tag manager modules
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddKenticoTagManager(this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddKenticoTagManagerServicesInternal();

        SnippetFactoryStore.AddSnippetFactory<CustomSnippetFactory>(configuration);
        SnippetFactoryStore.AddSnippetFactory<GoogleTagManagerSnippetFactory>(configuration);
        SnippetFactoryStore.AddSnippetFactory<GoogleAnalyticsSnippetFactory>(configuration);
        SnippetFactoryStore.AddSnippetFactory<MicrosoftClaritySnippetFactory>(configuration);
        SnippetFactoryStore.AddSnippetFactory<VwoABTestingCodeSnippetFactory>(configuration);
        SnippetFactoryStore.AddSnippetFactory<IntercomSnippetFactory>(configuration);
        return services;
    }

    /// <summary>
    /// Adds Tag Manager and custom module to application with customized options provided by the <see cref="ITagManagerBuilder"/>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IServiceCollection AddKenticoTagManager(this IServiceCollection services,
        IConfiguration configuration,
        Action<ITagManagerBuilder> configure
    )
    {
        services.AddKenticoTagManagerServicesInternal();

        var builder = new TagManagerBuilder(configuration);
        configure(builder);

        if (builder.IncludeDefaultSnippetModules)
        {
            SnippetFactoryStore.AddSnippetFactory<CustomSnippetFactory>(configuration);
            SnippetFactoryStore.AddSnippetFactory<GoogleTagManagerSnippetFactory>(configuration);
            SnippetFactoryStore.AddSnippetFactory<GoogleAnalyticsSnippetFactory>(configuration);
            SnippetFactoryStore.AddSnippetFactory<MicrosoftClaritySnippetFactory>(configuration);
            SnippetFactoryStore.AddSnippetFactory<VwoABTestingCodeSnippetFactory>(configuration);
            SnippetFactoryStore.AddSnippetFactory<IntercomSnippetFactory>(configuration);
        }

        return services;
    }

    private static IServiceCollection AddKenticoTagManagerServicesInternal(this IServiceCollection services) =>
        services
            .AddSingleton<ITagManagerModuleInstaller, TagManagerModuleInstaller>()
            .AddSingleton<IChannelCodeSnippetsService, DefaultChannelCodeSnippetsService>()
            .AddScoped<IWebsiteChannelPermissionService, WebsiteChannelPermissionService>()
            .AddTransient<ITagHelperComponent, CodeSnippetTagHelperComponent>();
}

public interface ITagManagerBuilder
{
    /// <summary>
    /// Registers the given <typeparamref name="TSnippetFactory"/> as a static resource under <see cref="CodeSnippetSettings.TagTypeName"/> 
    /// returned by <see cref="ISnippetFactory.CreateCodeSnippetSettings"/>
    /// </summary>
    /// <typeparam name="TSnippetFactory"></typeparam>
    /// <returns><see cref="ITagManagerBuilder"/></returns>
    ITagManagerBuilder AddSnippetFactory<TSnippetFactory>() where TSnippetFactory : class, ISnippetFactory, new();
}

internal class TagManagerBuilder : ITagManagerBuilder
{
    private readonly IConfiguration configuration;

    /// <summary>
    /// If true, the <see cref="CustomSnippetFactory"/>
    /// , <see cref="GoogleTagManagerSnippetFactory"/>
    /// , <see cref="GoogleAnalyticsSnippetFactory"/>
    /// , <see cref="MicrosoftClaritySnippetFactory"/>
    /// , <see cref="VwoABTestingCodeSnippetFactory"/>
    /// , <see cref="IntercomSnippetFactory"/>
    /// will be available as an explicitly selectable tag manager modules
    /// within the Admin UI. Defaults to <c>true</c>
    /// </summary>
    public bool IncludeDefaultSnippetModules { get; set; } = true;
    public TagManagerBuilder(IConfiguration configuration) => this.configuration = configuration;

    /// <inheritdoc />
    public ITagManagerBuilder AddSnippetFactory<TSnippetFactory>() where TSnippetFactory : class, ISnippetFactory, new()
    {
        SnippetFactoryStore.AddSnippetFactory<TSnippetFactory>(configuration);
        return this;
    }
}
