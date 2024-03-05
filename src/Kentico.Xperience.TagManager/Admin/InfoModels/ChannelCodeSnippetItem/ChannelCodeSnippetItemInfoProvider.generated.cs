using CMS.DataEngine;

namespace Kentico.Xperience.TagManager.Admin;

/// <summary>
/// Class providing <see cref="ChannelCodeSnippetItemInfo"/> management.
/// </summary>
[ProviderInterface(typeof(IChannelCodeSnippetItemInfoProvider))]
public partial class ChannelCodeSnippetItemInfoProvider : AbstractInfoProvider<ChannelCodeSnippetItemInfo, ChannelCodeSnippetItemInfoProvider>, IChannelCodeSnippetItemInfoProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChannelCodeSnippetItemInfoProvider"/> class.
    /// </summary>
    public ChannelCodeSnippetItemInfoProvider()
        : base(ChannelCodeSnippetItemInfo.TYPEINFO)
    {
    }
}
