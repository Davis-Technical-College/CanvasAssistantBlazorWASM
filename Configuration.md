This is a list of what must be configured during this project. At some point, this should be put into a config file in Blazor.

Various things need to be configured in this project, including:
- Canvas OAuth Client ID*
- Canvas OAuth Client Secret*
- Canvas Redirect URI
- Canvas Auth URI (If using different Canvas Domain)
- Azure tenant ID
- Azure Application Client ID
- Azure Application Redirect URI
- Azure CosmosDB URI

All of these values need to be configured in two location:

`TokenTestingBlazor.Client/wwwroot/appsettings.json` for the Client

`TokenTestingBlazor/appsettings.json` for the Server

Currently, there is an `appsettings.template.json` file at both locations, rename that to `appsettings.json`.
This is so your config file is ignored by git and won't be tracked.

*Both the Canvas OAuth Client ID and the Canvas OAuth Client Secret need to be given by DATC Instructors. These are secret keys and should be kept as secure as a password would be. 
