All the configuration for this project. I really should put it in a central config file, but blazor is weird and that is a future step.
This is just a proof of concept.

Various things need to be configured in this project, including:
- Canvas OAuth Client ID
- Canvas OAuth Client Secret
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
