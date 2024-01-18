using CMS;
using CMS.Core;
using CMS.DataEngine;
using Kentico.Xperience.TagManager.Modules;

[assembly: RegisterModule(type: typeof(CustomChannelSettingsModule))]

namespace Kentico.Xperience.TagManager.Modules;

internal class CustomChannelSettingsModule : Module
{
    public CustomChannelSettingsModule() : base(nameof(CustomChannelSettingsModule))
    {
    }

    protected override void OnInit()
    {
        base.OnInit();
        Service.Resolve<ICustomChannelSettingsModuleInstaller>().Install();
    }
}
