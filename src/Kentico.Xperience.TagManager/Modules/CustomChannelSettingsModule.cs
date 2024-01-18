using CMS;
using CMS.Core;
using CMS.DataEngine;
using Kentico.Xperience.TagManager.Modules;
using Microsoft.Extensions.DependencyInjection;

[assembly: RegisterModule(type: typeof(CustomChannelSettingsModule))]

namespace Kentico.Xperience.TagManager.Modules;

internal class CustomChannelSettingsModule : Module
{
    public CustomChannelSettingsModule() : base(nameof(CustomChannelSettingsModule))
    {
    }

    protected override void OnInit(ModuleInitParameters parameters)
    {
        base.OnInit();

        var services = parameters.Services;

        services.GetRequiredService<ICustomChannelSettingsModuleInstaller>().Install();
    }
}
