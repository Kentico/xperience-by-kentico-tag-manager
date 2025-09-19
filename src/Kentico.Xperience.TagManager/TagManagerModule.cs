using CMS;
using CMS.Base;
using CMS.Core;
using CMS.DataEngine;

using Kentico.Xperience.TagManager;
using Kentico.Xperience.TagManager.Admin;

using Microsoft.Extensions.DependencyInjection;

[assembly: RegisterModule(type: typeof(TagManagerModule))]

namespace Kentico.Xperience.TagManager;

internal class TagManagerModule : Module
{
    private ITagManagerModuleInstaller installer = null!;

    public TagManagerModule() : base(nameof(TagManagerModule))
    {
    }

    protected override void OnInit(ModuleInitParameters parameters)
    {
        base.OnInit(parameters);

        var services = parameters.Services;

        installer = services.GetRequiredService<ITagManagerModuleInstaller>();

        ApplicationEvents.Initialized.Execute += InitializeModule;
    }

    private void InitializeModule(object? sender, EventArgs e) => installer?.Install();
}
