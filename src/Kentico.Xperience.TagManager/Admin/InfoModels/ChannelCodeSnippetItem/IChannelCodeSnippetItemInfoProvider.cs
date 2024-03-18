using CMS.DataEngine;

namespace Kentico.Xperience.TagManager.Admin;

/// <summary>
/// Declares members for <see cref="ChannelCodeSnippetItemInfo"/> management.
/// </summary>
public partial interface IChannelCodeSnippetItemInfoProvider
{
    void BulkDelete(IWhereCondition where, BulkDeleteSettings? settings = null);
}
