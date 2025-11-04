using System;
using System.Data;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Kentico.Xperience.TagManager.Admin;

[assembly: RegisterObjectType(typeof(ChannelCodeSnippetItemContentTypeInfo), ChannelCodeSnippetItemContentTypeInfo.OBJECT_TYPE)]

namespace Kentico.Xperience.TagManager.Admin
{
    /// <summary>
    /// Data container class for <see cref="ChannelCodeSnippetItemContentTypeInfo"/>.
    /// </summary>
    public class ChannelCodeSnippetItemContentTypeInfo : AbstractInfo<ChannelCodeSnippetItemContentTypeInfo, IInfoProvider<ChannelCodeSnippetItemContentTypeInfo>>, IInfoWithId
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "kenticotagmanager.channelcodesnippetitemcontenttype";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(IInfoProvider<ChannelCodeSnippetItemContentTypeInfo>), OBJECT_TYPE, "KenticoTagManager.ChannelCodeSnippetItemContentType", nameof(ChannelCodeSnippetItemContentTypeID), null, null, null, null, null, null, null)
        {
            TouchCacheDependencies = true,
            IsBinding = true,
            ContinuousIntegrationSettings =
            {
                Enabled = true
            },
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency(nameof(ChannelCodeSnippetItemID), ChannelCodeSnippetItemInfo.OBJECT_TYPE, ObjectDependencyEnum.Binding),
                new ObjectDependency(nameof(ContentTypeID), DataClassInfo.OBJECT_TYPE, ObjectDependencyEnum.Binding),
            },
        };


        /// <summary>
        /// Channel code snippet item content type ID
        /// </summary>
        [DatabaseField]
        public virtual int ChannelCodeSnippetItemContentTypeID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue(nameof(ChannelCodeSnippetItemContentTypeID)), 0);
            }
            set
            {
                SetValue(nameof(ChannelCodeSnippetItemContentTypeID), value);
            }
        }


        /// <summary>
        /// Channel code snippet item ID
        /// </summary>
        [DatabaseField]
        public virtual int ChannelCodeSnippetItemID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue(nameof(ChannelCodeSnippetItemID)), 0);
            }
            set
            {
                SetValue(nameof(ChannelCodeSnippetItemID), value);
            }
        }


        /// <summary>
        /// Content type ID
        /// </summary>
        [DatabaseField]
        public virtual int ContentTypeID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue(nameof(ContentTypeID)), 0);
            }
            set
            {
                SetValue(nameof(ContentTypeID), value);
            }
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
        /// Creates an empty instance of the <see cref="ChannelCodeSnippetItemContentTypeInfo"/> class.
        /// </summary>
        public ChannelCodeSnippetItemContentTypeInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instance of the <see cref="ChannelCodeSnippetItemContentTypeInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public ChannelCodeSnippetItemContentTypeInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}
