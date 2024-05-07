using CMS;
using CMS.Core;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.AzureSearch.Admin;

[assembly: RegisterModule(typeof(TagManagerAdminModule))]

namespace Kentico.Xperience.AzureSearch.Admin;

/// <summary>
/// Manages administration features and integration.
/// </summary>
internal class TagManagerAdminModule : AdminModule
{
    public TagManagerAdminModule() : base(nameof(TagManagerAdminModule)) { }

    protected override void OnInit(ModuleInitParameters parameters)
    {
        base.OnInit(parameters);

        RegisterClientModule("kentico", "xperience-integrations-tagmanager");
    }
}
