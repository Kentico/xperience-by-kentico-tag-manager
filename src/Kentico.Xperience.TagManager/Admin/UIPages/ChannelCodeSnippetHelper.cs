using GTM;
using Kentico.Xperience.TagManager.Admin.UIPages.Models;
using Kentico.Xperience.TagManager.Enums;

namespace Kentico.Xperience.TagManager.Admin.UIPages;

internal static class ChannelCodeSnippetHelper
{
    public static void SetChannelCodeSnippetInfo(CodeSnippetEditModel model, ChannelCodeSnippetInfo info)
    {
        info.ChannelCodeSnippetChannelID = model.ChannelIDs.FirstOrDefault();
        info.ChannelCodeSnippetConsentID = model.ConsentIDs.FirstOrDefault();
        info.ChannelCodeSnippetLocation = model.Location;
        info.ChannelCodeSnippetType = model.SnippetType;

        switch (model.SnippetType)
        {
            case nameof(CodeSnippetTypes.GTM):
                info.ChannelCodeSnippetGTMID = model.GTMID;
                info.ChannelCodeSnippetCode = null;
                break;
            case nameof(CodeSnippetTypes.CustomCode):
                info.ChannelCodeSnippetGTMID = null;
                info.ChannelCodeSnippetCode = model.Code;
                break;
            default:
                break;
        }
    }
}
