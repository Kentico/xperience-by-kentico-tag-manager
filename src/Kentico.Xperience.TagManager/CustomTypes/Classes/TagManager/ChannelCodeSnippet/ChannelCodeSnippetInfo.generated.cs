using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using GTM;

[assembly: RegisterObjectType(typeof(ChannelCodeSnippetInfo), ChannelCodeSnippetInfo.OBJECT_TYPE)]
namespace GTM
{
    /// <summary>
    /// Data container class for <see cref="ChannelCodeSnippetInfo"/>.
    /// </summary>
    [Serializable]
    public partial class ChannelCodeSnippetInfo : AbstractInfo<ChannelCodeSnippetInfo, IChannelCodeSnippetInfoProvider>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "kenticotagmanager.channelcodesnippet";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(ChannelCodeSnippetInfoProvider), OBJECT_TYPE, "KenticoTagManager.ChannelCodeSnippet", "ChannelCodeSnippetID", null, null, null, null, null, null, null)
        {
            TouchCacheDependencies = true,
            ContinuousIntegrationSettings =
            {
                Enabled = true
            },
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency("ChannelCodeSnippetChannelID", "cms.channel", ObjectDependencyEnum.Required),
                new ObjectDependency("ChannelCodeSnippetConsentID", "cms.consent", ObjectDependencyEnum.Required),
            },
        };


        /// <summary>
        /// Channel code snippet ID.
        /// </summary>
        [DatabaseField]
        public virtual int ChannelCodeSnippetID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(ChannelCodeSnippetID)), 0);
            set => SetValue(nameof(ChannelCodeSnippetID), value);
        }


        /// <summary>
        /// Channel code snippet channel ID.
        /// </summary>
        [DatabaseField]
        public virtual int ChannelCodeSnippetChannelID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(ChannelCodeSnippetChannelID)), 0);
            set => SetValue(nameof(ChannelCodeSnippetChannelID), value);
        }


        /// <summary>
        /// Channel code snippet type.
        /// </summary>
        [DatabaseField]
        public virtual string ChannelCodeSnippetType
        {
            get => ValidationHelper.GetString(GetValue(nameof(ChannelCodeSnippetType)), String.Empty);
            set => SetValue(nameof(ChannelCodeSnippetType), value, String.Empty);
        }

        /// <summary>
        /// Channel code snippet location.
        /// </summary>
        [DatabaseField]
        public virtual string ChannelCodeSnippetLocation
        {
            get => ValidationHelper.GetString(GetValue(nameof(ChannelCodeSnippetLocation)), String.Empty);
            set => SetValue(nameof(ChannelCodeSnippetLocation), value, String.Empty);
        }


        /// <summary>
        /// Channel code snippet GTMID.
        /// </summary>
        [DatabaseField]
        public virtual string ChannelCodeSnippetGTMID
        {
            get => ValidationHelper.GetString(GetValue(nameof(ChannelCodeSnippetGTMID)), String.Empty);
            set => SetValue(nameof(ChannelCodeSnippetGTMID), value, String.Empty);
        }


        /// <summary>
        /// Channel code snippet code.
        /// </summary>
        [DatabaseField]
        public virtual string ChannelCodeSnippetCode
        {
            get => ValidationHelper.GetString(GetValue(nameof(ChannelCodeSnippetCode)), String.Empty);
            set => SetValue(nameof(ChannelCodeSnippetCode), value, String.Empty);
        }


        /// <summary>
        /// Channel code snippet consent ID.
        /// </summary>
        [DatabaseField]
        public virtual int ChannelCodeSnippetConsentID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(ChannelCodeSnippetConsentID)), 0);
            set => SetValue(nameof(ChannelCodeSnippetConsentID), value);
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
        protected ChannelCodeSnippetInfo(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="ChannelCodeSnippetInfo"/> class.
        /// </summary>
        public ChannelCodeSnippetInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="ChannelCodeSnippetInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public ChannelCodeSnippetInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}
