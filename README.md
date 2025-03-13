# Token Testing Blazor

This application is designed to work as a help-mate to the Instructure Canvas Web Applicaton. It does require authentication through Canvas and Microsoft (Azure.) The functionality is currently extremely limited but eventually should include a number of helpful features to aid Canvas instructors at Davis Tech.

## Design Notes

This project has two parts:

+ The Blazor Server
+ The Blazor Client

The server component provides access to Canvas API's (since client-side requests are blocked) through an internal API which can be called from the Client component.

It uses an Azure CosmosDB instance to store credentials, meaning it also uses Microsoft Entra ID to Authenticate.

This branch of the project includes code to refresh the access tokens, as well as tell when they expire.

It also implements an actual logout feature, completing the authentication user experience.

## Auth Flow

When initializing the authorization flow by hitting "Login" the client directs the user to the Microsoft Login Page 
where they login using Microsoft Entra ID. From there they are redirected back to the WASM which exchanges the auth code for an Azure Active Directory Access Token.
After that the user is once again redirected to the Canvas login, where they grant permission for the application to access their account, redirecting to the WASM with an authentication code.
The client then makes a call to the Blazor server with the auth code, while the server accesses the CosmosDB instance to get the Canvas OAuth Client Secret which is used to exchange said auth code for
a Canvas Access Token.


## Refresh Token

This branch actually contains the logic in order to refresh the Access Tokens. It also includes an `<Auth />` component which can be wrapped around a web page to have it redirect the user if they aren't logged in.

If a page doesn't use the `<Auth />` component, make sure to use the `<LoginHeader />` component so that the page still has the header with the login button.

## Setup

To get this project working see [Azure Setup](AzureSetup.md) and [Project Configuration](Configuration.md)

## Public Website
https://canvas-assistant-blazor-wasm.azurewebsites.net/

## Contribution

See [Contribution.md](Contribution.md) about the current state of the project and how to add on to it.
