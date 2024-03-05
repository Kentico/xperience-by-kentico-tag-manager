using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.ContentEngine;
using CMS.DataProtection;
using Kentico.Xperience.TagManager.Admin;

[assembly: RegisterObjectType(typeof(ChannelCodeSnippetItemInfo), ChannelCodeSnippetItemInfo.OBJECT_TYPE)]

namespace Kentico.Xperience.TagManager.Admin;

/// <summary>
/// Data container class for <see cref="ChannelCodeSnippetItemInfo"/>.
/// </summary>
[Serializable]
public partial class ChannelCodeSnippetItemInfo : AbstractInfo<ChannelCodeSnippetItemInfo, IChannelCodeSnippetItemInfoProvider>
{
    /// <summary>
    /// Object type.
    /// </summary>
    public const string OBJECT_TYPE = "kenticotagmanager.channelcodesnippetitem";


    /// <summary>
    /// Type information.
    /// </summary>
    public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(ChannelCodeSnippetItemInfoProvider), OBJECT_TYPE, "KenticoTagManager.ChannelCodeSnippetItem", nameof(ChannelCodeSnippetItemID), nameof(ChannelCodeSnippetItemLastModified), nameof(ChannelCodeSnippetItemGuid), nameof(ChannelCodeSnippetItemName), null, null, null, null)
    {
        TouchCacheDependencies = true,
        ContinuousIntegrationSettings =
        {
            Enabled = true
        },
        DependsOn = new List<ObjectDependency>()
        {
            new ObjectDependency(nameof(ChannelCodeSnippetItemChannelID), ChannelInfo.OBJECT_TYPE, ObjectDependencyEnum.Required),
            new ObjectDependency(nameof(ChannelCodeSnippetItemConsentID), ConsentInfo.OBJECT_TYPE, ObjectDependencyEnum.Required),
        },
    };


    /// <summary>
    /// Channel code snippet ID.
    /// </summary>
    [DatabaseField]
    public virtual int ChannelCodeSnippetItemID
    {
        get => ValidationHelper.GetInteger(GetValue(nameof(ChannelCodeSnippetItemID)), 0);
        set => SetValue(nameof(ChannelCodeSnippetItemID), value);
    }


    /// <summary>
    /// Channel code snippet guid.
    /// </summary>
    [DatabaseField]
    public virtual Guid ChannelCodeSnippetItemGuid
    {
        get => ValidationHelper.GetGuid(GetValue(nameof(ChannelCodeSnippetItemGuid)), default);
        set => SetValue(nameof(ChannelCodeSnippetItemGuid), value);
    }



    /// <summary>
    /// Channel code snippet name.
    /// </summary>
    [DatabaseField]
    public virtual string ChannelCodeSnippetItemName
    {
        get => ValidationHelper.GetString(GetValue(nameof(ChannelCodeSnippetItemName)), "");
        set => SetValue(nameof(ChannelCodeSnippetItemName), value);
    }


    /// <summary>
    /// Channel code snippet date modified.
    /// </summary>
    [DatabaseField]
    public virtual DateTime ChannelCodeSnippetItemLastModified
    {
        get => ValidationHelper.GetDateTime(GetValue(nameof(ChannelCodeSnippetItemLastModified)), DateTime.MinValue);
        set => SetValue(nameof(ChannelCodeSnippetItemLastModified), value);
    }


    /// <summary>
    /// Channel code snippet channel ID.
    /// </summary>
    [DatabaseField]
    public virtual int ChannelCodeSnippetItemChannelID
    {
        get => ValidationHelper.GetInteger(GetValue(nameof(ChannelCodeSnippetItemChannelID)), 0);
        set => SetValue(nameof(ChannelCodeSnippetItemChannelID), value);
    }


    /// <summary>
    /// Channel code snippet type.
    /// </summary>
    [DatabaseField]
    public virtual string ChannelCodeSnippetItemType
    {
        get => ValidationHelper.GetString(GetValue(nameof(ChannelCodeSnippetItemType)), String.Empty);
        set => SetValue(nameof(ChannelCodeSnippetItemType), value, String.Empty);
    }

    /// <summary>
    /// Channel code snippet location.
    /// </summary>
    [DatabaseField]
    public virtual string ChannelCodeSnippetItemLocation
    {
        get => ValidationHelper.GetString(GetValue(nameof(ChannelCodeSnippetItemLocation)), String.Empty);
        set => SetValue(nameof(ChannelCodeSnippetItemLocation), value, String.Empty);
    }


    /// <summary>
    /// Channel code snippet GTMID.
    /// </summary>
    [DatabaseField]
    public virtual string ChannelCodeSnippetItemIdentifier
    {
        get => ValidationHelper.GetString(GetValue(nameof(ChannelCodeSnippetItemIdentifier)), String.Empty);
        set => SetValue(nameof(ChannelCodeSnippetItemIdentifier), value, String.Empty);
    }


    /// <summary>
    /// Channel code snippet code.
    /// </summary>
    [DatabaseField]
    public virtual string ChannelCodeSnippetItemCode
    {
        get => ValidationHelper.GetString(GetValue(nameof(ChannelCodeSnippetItemCode)), String.Empty);
        set => SetValue(nameof(ChannelCodeSnippetItemCode), value, String.Empty);
    }


    /// <summary>
    /// Channel code snippet consent ID.
    /// </summary>
    [DatabaseField]
    public virtual int ChannelCodeSnippetItemConsentID
    {
        get => ValidationHelper.GetInteger(GetValue(nameof(ChannelCodeSnippetItemConsentID)), 0);
        set => SetValue(nameof(ChannelCodeSnippetItemConsentID), value);
    }


    /// <summary>
    /// Deletes the object using appropriate provider.
    /// </summary>
    protected override void DeleteObject()
    {
        Provider.Delete(this);
    }


    /// <summary>
    /// Updates the object using appropriate provider.
    /// </summary>
    protected override void SetObject()
    {
        Provider.Set(this);
    }


    /// <summary>
    /// Constructor for de-serialization.
    /// </summary>
    /// <param name="info">Serialization info.</param>
    /// <param name="context">Streaming context.</param>
    protected ChannelCodeSnippetItemInfo(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }


    /// <summary>
    /// Creates an empty instance of the <see cref="ChannelCodeSnippetItemInfo"/> class.
    /// </summary>
    public ChannelCodeSnippetItemInfo()
        : base(TYPEINFO)
    {
    }


    /// <summary>
    /// Creates a new instances of the <see cref="ChannelCodeSnippetItemInfo"/> class from the given <see cref="DataRow"/>.
    /// </summary>
    /// <param name="dr">DataRow with the object data.</param>
    public ChannelCodeSnippetItemInfo(DataRow dr)
        : base(TYPEINFO, dr)
    {
    }
}
