using CMS.DataEngine;

namespace GTM
{
    /// <summary>
    /// Class providing <see cref="ChannelCodeSnippetInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IChannelCodeSnippetInfoProvider))]
    public partial class ChannelCodeSnippetInfoProvider : AbstractInfoProvider<ChannelCodeSnippetInfo, ChannelCodeSnippetInfoProvider>, IChannelCodeSnippetInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCodeSnippetInfoProvider"/> class.
        /// </summary>
        public ChannelCodeSnippetInfoProvider()
            : base(ChannelCodeSnippetInfo.TYPEINFO)
        {
        }
    }
}