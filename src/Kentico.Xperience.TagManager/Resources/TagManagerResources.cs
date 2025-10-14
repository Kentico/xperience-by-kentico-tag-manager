using CMS.Base;
using CMS.Localization;

using Kentico.Xperience.TagManager.Resources;

[assembly: RegisterLocalizationResource(typeof(TagManagerResources), SystemContext.SYSTEM_CULTURE_NAME)]
[assembly: RegisterLocalizationResource(typeof(TagManagerResources), "en-US")]
namespace Kentico.Xperience.TagManager.Resources;

internal class TagManagerResources
{
    public TagManagerResources()
    {
    }
}
