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

internal class TagManagerModuleInstaller : ITagManagerModuleInstaller
{
    private readonly IResourceInfoProvider resourceInfoProvider;

    public TagManagerModuleInstaller(IResourceInfoProvider resourceInfoProvider) => this.resourceInfoProvider = resourceInfoProvider;

    public void Install()
    {
        var resourceInfo = InstallModule();
        InstallModuleClasses(resourceInfo);
    }

    private ResourceInfo InstallModule()
    {
        var resourceInfo = resourceInfoProvider.Get(ResourceConstants.ResourceName)
            // Handle v1.0.0 resource name
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

    private static void InstallModuleClasses(ResourceInfo resourceInfo) => InstallChannelCodeSnippetClass(resourceInfo);

    private static void InstallChannelCodeSnippetClass(ResourceInfo resourceInfo)
    {
        var info = DataClassInfoProvider.GetDataClassInfo(ChannelCodeSnippetInfo.TYPEINFO.ObjectClassName) ??
                                      DataClassInfo.New(ChannelCodeSnippetInfo.OBJECT_TYPE);

        info.ClassName = ChannelCodeSnippetInfo.TYPEINFO.ObjectClassName;
        info.ClassTableName = ChannelCodeSnippetInfo.TYPEINFO.ObjectClassName.Replace(".", "_");
        info.ClassDisplayName = "Channel Code Snippet";
        info.ClassResourceID = resourceInfo.ResourceID;
        info.ClassType = ClassType.OTHER;
        var formInfo = FormHelper.GetBasicFormDefinition(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetID));
        var formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetChannelID),
            Visible = false,
            DataType = FieldDataType.Integer,
            Enabled = true,
            ReferenceToObjectType = ChannelInfo.OBJECT_TYPE,
            ReferenceType = ObjectDependencyEnum.Required
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetConsentID),
            Visible = false,
            DataType = FieldDataType.Integer,
            Enabled = true,
            AllowEmpty = true,
            ReferenceToObjectType = ConsentInfo.OBJECT_TYPE,
            ReferenceType = ObjectDependencyEnum.Required,
        };
        formItem.SetComponentName(ObjectIdSelectorComponent.IDENTIFIER);
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetGuid),
            Visible = false,
            DataType = FieldDataType.Guid,
            Enabled = true,
            AllowEmpty = false,
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetName),
            Visible = true,
            DataType = FieldDataType.Text,
            Enabled = true,
            AllowEmpty = false,
            Settings = new()
            {
                { nameof(TextInputProperties.Label), "Code name" },
            }
        };
        formItem.SetComponentName(TextInputComponent.IDENTIFIER);
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetLastModified),
            Visible = false,
            DataType = FieldDataType.DateTime,
            Enabled = true,
            AllowEmpty = false,
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetCode),
            Visible = false,
            Precision = 0,
            Size = 5000,
            DataType = FieldDataType.LongText,
            Enabled = true,
            AllowEmpty = true
        };
        formItem.SetComponentName(CodeEditorComponent.IDENTIFIER);
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetIdentifier),
            Visible = false,
            Precision = 0,
            Size = 25,
            DataType = FieldDataType.Text,
            Enabled = true,
            AllowEmpty = true
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetType),
            Visible = false,
            Precision = 0,
            Size = 25,
            DataType = FieldDataType.Text,
            Enabled = true,
            AllowEmpty = true
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetLocation),
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
