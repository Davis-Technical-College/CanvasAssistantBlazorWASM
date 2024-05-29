Documentation to access CosmosDB from a client side BlazorWASM

Steps to setup in Azure - 

I. CosmosDB


1. Create a CosmosDB instance (NoSQL)
2. Copy the instance URI and configure that in `appsettings.json` (See [Configuration](Configuration.md))
3. In the CosmosDB instance under Settings -> CORS add the domain of the site to the list of allowed origins (e.g. "http://localhost:3000")
4. Configure Permissions
	
	This is a mess, and needs to be done from the Azure CLI.
	Bash files and role JSONs will be at the bottom of this file.
	
	A. Open a new Azure Command Line
	
	B. Run `code .` to open a text editor (If asked to open a classic shell, choose 'Yes')
	
	C. In a new file create `role-definition-rw.json` (see below)

	Note there is a hidden menu in the top right of the text editor to save a the file
	
	D. In another file create the `createRole.sh` script using your resource group and CosmosDB account name
	
	E. Run the script using `bash createRole.sh`
	
	F. Get the **roleDefinitionId** for the role you just created.
	
	This should be in the name property of the JSON object outputted by running the previous script, but if not it can be found by running 
		`az cosmosdb sql role definition list --account-name yourCosmosAccount -g yourResourceGroup`
	
	G. Note down the **roleDefinitionId** which can be found in the name property.
	
	H. Create the `applyRole.sh` script as seen below, configuring the values as seen below. The object ID you wish to assign to can be accessed via Microsoft Entra ID -> Users -> (YourUser) -> Overview

	I. Run the script using `bash applyRole.sh`



II. App Registration
This is needed for the Authorization using Microsoft EntraID.

1. In the Microsoft EntraID portal -> App registrations hit NEW REGISTRATION and enter an application name and hit register.
2. Note down the Application (client) ID and the Directory (tenant) ID to configure in `appsettings.json`.
3. Under the app registration -> Manage -> Authentication hit **Add a platform** and choose **Single-page application**, giving it the redirect URL (e.g. `http://localhost:3000/callback`) you want to use - then hit **Configure**.
4. Under the app registration -> Manage -> API permissions choose **Add a permission** and choose Azure Cosmos DB. Select that it is a delegated permission, and you specifically want `user_impersonation`, then hit add permission.
5. Configure `appsettings.json` with the tenant ID, client ID and the redirect URI


III. Finish Configuring `appsettings.json` with all the canvas information, on both client and server.

**Important!** Make sure the visual studio project is configured to use http not https


External Files for configuring permissions:

role-definition-rw.json:

```json
{
    "RoleName": "CosmosDBReadWriteRole",
    "Type": "CustomRole",
    "AssignableScopes": ["/"],
    "Permissions": [{
        "DataActions": [
            "Microsoft.DocumentDB/databaseAccounts/readMetadata",
            "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/items/*",
            "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/*"
        ]
    }]
}
```

createRole.sh:

```sh
resourceGroupName='YourResourceGroupName'
accountName='CosmosDBAccountName'
az cosmosdb sql role definition create -a $accountName -g $resourceGroupName -b @role-definition-rw.json
```

applyRole.sh:

```sh
resourceGroupName='YourResourceGroupName'
accountName='CosmosDBAccountName'
roleDefinitionId='GET THIS USING THE COMMAND MENTIONED ABOVE'
principalId='YourUserObjectId'
az cosmosdb sql role assignment create -a $accountName -g $resourceGroupName -s "/" -p $principalId -d $roleDefinitionId
```
