using CMS;
using CMS.Core;
using CMS.DataEngine;
using Kentico.Xperience.TagManager.Modules;

[assembly: RegisterModule(typeof(CustomChannelSettingsModule))]
namespace Kentico.Xperience.TagManager.Modules
{
    /// <summary>
    /// Module with bizformitem event handlers for SalesForce Sales integration
    /// </summary>
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
}