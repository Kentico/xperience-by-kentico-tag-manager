using CMS.DataEngine;

using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Kentico.Xperience.TagManager.Admin.Components;

/// <summary>
/// Filters content types to only show page content types (web page types).
/// </summary>
internal class ContentTypeSelectorWhereConditionProvider : IObjectSelectorWhereConditionProvider
{
    /// <summary>
    /// Provides a where condition that filters to only page content types.
    /// </summary>
    public WhereCondition Get() => new WhereCondition()
        .WhereEquals(nameof(DataClassInfo.ClassContentTypeType), ClassContentTypeType.WEBSITE);
}
