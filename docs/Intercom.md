# Intercom Messenger

Intercom offers a suite of tools to promising they are "The only AI customer service solution you need". The focus of this integration is their Messenger allowing you to communicate with customers live through your website.

## Locating the Tag ID

You use your Intercom Workspace ID as the Tag ID in the Tag Manager application.

To find your Workspace ID, log into your [Intercom Dashboard](https://app.intercom.com/) and go into Installation settings. You can find this settings by hovering over your avatar in the bottom left in the menu panel and navigating to "Settings". Here in the left menu, click on Installation. You will arrive at the following screen.

There, select "Install with a no-code integration" banner, select Google Tag Manager tab and click on a button "Copy workspace ID". This is the easiest way to access your Workspace ID which you then just paste into the Tag Manager application when creating new Intercom tag.

![Intercom step 1](/images/docs/intercom-id.png)

Down on this page, you can also verify if the installation was successful.

## Limitations

This integration does not send any data to Intercom. That means any conversation is private and you don't need the consent of the user, but Itercom does not benefit of any knowledge of the customer that might reside in Xperience by Kentico.

If you want to share user data with Intercom, [create a custom module](Creating-custom-module.md) and set up the widget to meet your needs. 