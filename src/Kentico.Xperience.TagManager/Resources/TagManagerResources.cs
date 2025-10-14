using CMS.Base;
using CMS.Localization;

using Kentico.Xperience.TagManager.Resources;

[assembly: RegisterLocalizationResource(typeof(TagManagerResources), SystemContext.SYSTEM_CULTURE_NAME)]
namespace Kentico.Xperience.TagManager.Resources;

internal class TagManagerResources
{
    public TagManagerResources()
    {
    }
}
