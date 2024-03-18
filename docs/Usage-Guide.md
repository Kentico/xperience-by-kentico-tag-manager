# Usage Guide

## Using provided snippets

You can use any of our provided snippets by chosing type of the snippet in the Admin UI application

In the `appsettings.json` you may chose which tag manager types should be selectable in the ui application.

For backwards compatibility all snippet types are allowed by default and user does not need to change anything in the `appsettings.json`.

If you need to customize this you should configure the following section:

``` json
"xbyk.tagmanager.modules": [
  "Kentico.VWO",
  "Kentico.GoogleTagManager",
  "Kentico.GoogleAnalytics4",
  "Kentico.MicrosoftClarity",
  "Kentico.Intercom",
  "Kentico.CustomTag"
]
```

If any module is specified only the specified modules are included. If no module is specified or the section is not present all modules are included.

If you wish to add your own custom tag manager module See [Creating custom Tag Manager module](Creating-custom-module.md)