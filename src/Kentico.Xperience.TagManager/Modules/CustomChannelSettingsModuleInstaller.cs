using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Modules;
using GTM;
using static Kentico.Xperience.TagManager.Constants.GtmConstants;

namespace Kentico.Xperience.TagManager.Modules;

internal interface ICustomChannelSettingsModuleInstaller
{
    void Install();
}

internal class CustomChannelSettingsModuleInstaller : ICustomChannelSettingsModuleInstaller
{
    private readonly IResourceInfoProvider resourceInfoProvider;

    public CustomChannelSettingsModuleInstaller(IResourceInfoProvider resourceInfoProvider) =>
        this.resourceInfoProvider = resourceInfoProvider;

    public void Install()
    {
        var resourceInfo = InstallModule();
        InstallModuleClasses(resourceInfo);
    }

    private ResourceInfo InstallModule()
    {
        var resourceInfo = resourceInfoProvider.Get(ResourceConstants.ResourceName) ?? new ResourceInfo();

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
        var channelCodeSnippetClass = DataClassInfoProvider.GetDataClassInfo(ChannelCodeSnippetInfo.OBJECT_TYPE);
        if (channelCodeSnippetClass is not null)
        {
            return;
        }

        channelCodeSnippetClass = DataClassInfo.New(ChannelCodeSnippetInfo.OBJECT_TYPE);

        channelCodeSnippetClass.ClassName = ChannelCodeSnippetInfo.OBJECT_TYPE;
        channelCodeSnippetClass.ClassTableName = ChannelCodeSnippetInfo.OBJECT_TYPE.Replace(".", "_");
        channelCodeSnippetClass.ClassDisplayName = "Channel Code Snippet";
        channelCodeSnippetClass.ClassResourceID = resourceInfo.ResourceID;
        channelCodeSnippetClass.ClassType = ClassType.OTHER;
        var formInfo = FormHelper.GetBasicFormDefinition(nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetID));
        var formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetChannelID),
            Visible = false,
            DataType = FieldDataType.Integer,
            Enabled = true
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetConsentID),
            Visible = false,
            DataType = FieldDataType.Integer,
            Enabled = true,
            AllowEmpty = true
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
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(ChannelCodeSnippetInfo.ChannelCodeSnippetGTMID),
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

        channelCodeSnippetClass.ClassFormDefinition = formInfo.GetXmlDefinition();

        DataClassInfoProvider.SetDataClassInfo(channelCodeSnippetClass);
    }
}
