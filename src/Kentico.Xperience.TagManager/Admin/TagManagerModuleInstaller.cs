using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.DataProtection;
using CMS.FormEngine;
using CMS.Modules;

using Kentico.Xperience.Admin.Base.Forms;

using static Kentico.Xperience.TagManager.Constants.TagManagerConstants;

namespace Kentico.Xperience.TagManager.Admin;

internal interface ITagManagerModuleInstaller
{
    void Install();
}

internal class TagManagerModuleInstaller(IInfoProvider<ResourceInfo> resourceInfoProvider) : ITagManagerModuleInstaller
{
    private readonly IInfoProvider<ResourceInfo> resourceInfoProvider = resourceInfoProvider;

    public void Install()
    {
        var resourceInfo = InstallModule();
        InstallModuleClasses(resourceInfo);
    }

    private ResourceInfo InstallModule()
    {
        var resourceInfo = resourceInfoProvider.Get(ResourceConstants.ResourceName)
            ?? resourceInfoProvider.Get("Kentico.Xperience.TagManager")
            ?? new ResourceInfo();

        resourceInfo.ResourceDisplayName = ResourceConstants.ResourceDisplayName;
        resourceInfo.ResourceName = ResourceConstants.ResourceName;
        resourceInfo.ResourceDescription = ResourceConstants.ResourceDescription;
        resourceInfo.ResourceIsInDevelopment = ResourceConstants.ResourceIsInDevelopment;
        if (resourceInfo.HasChanged)
        {
            resourceInfoProvider.Set(resourceInfo);
        }

        return resourceInfo;
    }

    private static void InstallModuleClasses(ResourceInfo resourceInfo)
    {
        InstallChannelCodeSnippetClass(resourceInfo);
        InstallChannelCodeSnippetItemContentTypeBindingClass(resourceInfo);
    }

    private static void InstallChannelCodeSnippetClass(ResourceInfo resourceInfo)
    {
        var info = DataClassInfoProvider.GetDataClassInfo(ChannelCodeSnippetItemInfo.OBJECT_TYPE) ??
                                      DataClassInfo.New(ChannelCodeSnippetItemInfo.OBJECT_TYPE);

        info.ClassName = ChannelCodeSnippetItemInfo.TYPEINFO.ObjectClassName;
        info.ClassTableName = ChannelCodeSnippetItemInfo.TYPEINFO.ObjectClassName.Replace(".", "_");
        info.ClassDisplayName = "Channel Code Snippet Item";
        info.ClassResourceID = resourceInfo.ResourceID;
        info.ClassType = ClassType.OTHER;
        var formInfo = FormHelper.GetBasicFormDefinition(nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemID));
        var formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemChannelId),
            Visible = false,
            DataType = FieldDataType.Integer,
            Enabled = true,
            ReferenceToObjectType = ChannelInfo.OBJECT_TYPE,
            ReferenceType = ObjectDependencyEnum.Required
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemConsentId),
            Visible = false,
            DataType = FieldDataType.Integer,
            Enabled = true,
            AllowEmpty = true,
            ReferenceToObjectType = ConsentInfo.OBJECT_TYPE,
            ReferenceType = ObjectDependencyEnum.Required,
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemEnable),
            Visible = false,
            DataType = FieldDataType.Boolean,
            Enabled = true,
            AllowEmpty = false,
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemGuid),
            Visible = false,
            DataType = FieldDataType.Guid,
            Enabled = true,
            AllowEmpty = false,
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemName),
            Visible = true,
            DataType = FieldDataType.Text,
            Enabled = true,
            AllowEmpty = false,
            Settings = new()
            {
                { nameof(TextInputProperties.Label), "Code name" },
            }
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemLastModified),
            Visible = false,
            DataType = FieldDataType.DateTime,
            Enabled = true,
            AllowEmpty = false,
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemCode),
            Visible = false,
            Precision = 0,
            Size = 5000,
            DataType = FieldDataType.LongText,
            Enabled = true,
            AllowEmpty = true
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemIdentifier),
            Visible = false,
            Precision = 0,
            Size = 200,
            DataType = FieldDataType.Text,
            Enabled = true,
            AllowEmpty = true
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetAdministrationDisplayMode),
            Visible = false,
            Precision = 0,
            Size = 200,
            DataType = FieldDataType.Text,
            Enabled = true,
            AllowEmpty = true
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemType),
            Visible = false,
            Precision = 0,
            Size = 200,
            DataType = FieldDataType.Text,
            Enabled = true,
            AllowEmpty = true
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetItemInfo.ChannelCodeSnippetItemLocation),
            Visible = false,
            Precision = 0,
            Size = 25,
            DataType = FieldDataType.Text,
            Enabled = true,
            AllowEmpty = true
        };
        formInfo.AddFormItem(formItem);

        SetFormDefinition(info, formInfo);

        if (info.HasChanged)
        {
            DataClassInfoProvider.SetDataClassInfo(info);
        }
    }

    private static void InstallChannelCodeSnippetItemContentTypeBindingClass(ResourceInfo resourceInfo)
    {
        var info = DataClassInfoProvider.GetDataClassInfo(ChannelCodeSnippetItemContentTypeInfo.OBJECT_TYPE) ??
                                      DataClassInfo.New(ChannelCodeSnippetItemContentTypeInfo.OBJECT_TYPE);

        info.ClassName = ChannelCodeSnippetItemContentTypeInfo.TYPEINFO.ObjectClassName;
        info.ClassTableName = ChannelCodeSnippetItemContentTypeInfo.TYPEINFO.ObjectClassName.Replace(".", "_");
        info.ClassDisplayName = "Channel Code Snippet Item to Content Type Binding";
        info.ClassResourceID = resourceInfo.ResourceID;
        info.ClassType = ClassType.OTHER;

        var formInfo = FormHelper.GetBasicFormDefinition(nameof(ChannelCodeSnippetItemContentTypeInfo.ChannelCodeSnippetItemContentTypeID));

        var formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetItemContentTypeInfo.ChannelCodeSnippetItemID),
            Visible = false,
            DataType = FieldDataType.Integer,
            Enabled = true,
            AllowEmpty = false,
            ReferenceToObjectType = ChannelCodeSnippetItemInfo.OBJECT_TYPE,
            ReferenceType = ObjectDependencyEnum.Binding
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetItemContentTypeInfo.ContentTypeID),
            Visible = false,
            DataType = FieldDataType.Integer,
            Enabled = true,
            AllowEmpty = false,
            ReferenceToObjectType = DataClassInfo.OBJECT_TYPE,
            ReferenceType = ObjectDependencyEnum.Binding
        };
        formInfo.AddFormItem(formItem);

        SetFormDefinition(info, formInfo);

        if (info.HasChanged)
        {
            DataClassInfoProvider.SetDataClassInfo(info);
        }
    }

    /// <summary>
    /// Ensure that the form is not upserted with any existing form
    /// </summary>
    /// <param name="info"></param>
    /// <param name="form"></param>
    private static void SetFormDefinition(DataClassInfo info, FormInfo form)
    {
        if (info.ClassID > 0)
        {
            var existingForm = new FormInfo(info.ClassFormDefinition);
            existingForm.CombineWithForm(form, new());
            info.ClassFormDefinition = existingForm.GetXmlDefinition();
        }
        else
        {
            info.ClassFormDefinition = form.GetXmlDefinition();
        }
    }
}
