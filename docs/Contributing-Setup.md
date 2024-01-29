# Contributing Setup

---This documents the steps a maintainer or developer would follow to work on the library in their development environment---
---Update the details for this project, replacing "repotemplate" and anything else that needs changed---

## Required Software

The requirements to setup, develop, and build this project are listed below.

### .NET Runtime

.NET SDK 8.0 or newer

- <https://dotnet.microsoft.com/en-us/download/dotnet/8.0>
- See `global.json` file for specific SDK requirements

### Node.js Runtime

- [Node.js](https://nodejs.org/en/download) 20.10.0 or newer
- [NVM for Windows](https://github.com/coreybutler/nvm-windows) to manage multiple installed versions of Node.js
- See `engines` in the solution `package.json` for specific version requirements

### C# Editor

- VS Code
- Visual Studio
- Rider

### Database

SQL Server 2019 or newer compatible database

- [SQL Server Linux](https://learn.microsoft.com/en-us/sql/linux/sql-server-linux-setup?view=sql-server-ver15)
- [Azure SQL Edge](https://learn.microsoft.com/en-us/azure/azure-sql-edge/disconnected-deployment)

### SQL Editor

- MS SQL Server Management Studio
- Azure Data Studio

## Sample Project

### Database Setup

Running the sample project requires creating a new Xperience by Kentico database using the included template.

Change directory in your console to `./examples/DancingGoat` and follow the instructions in the Xperience
documentation on [creating a new database](https://docs.xperience.io/xp26/developers-and-admins/installation#Installation-CreatetheprojectdatabaseCreateProjectDatabase).

### Admin Customization

To run the Sample app Admin customization in development mode, add the following to your [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows#secret-manager) for the application.

```json
"CMSAdminClientModuleSettings": {
  "kentico-xperience-integrations-tagmanager": {
    "Mode": "Proxy",
    "Port": 3009
  }
}
```

## Development Workflow

### Prepare your Git branch and commits

1. Create a new branch with one of the following prefixes

   - `feat/` - for new functionality
   - `refactor/` - for restructuring of existing features
   - `fix/` - for bugfixes

1. Run `dotnet format` against the `src/Kentico.Xperience.TagManager` project

   > use `dotnet: format` VS Code task.

1. Commit changes, with a commit message preferably following the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/#summary) convention.

### Test the package locally using the following commands

1. Create a local packages folder

   `mkdir ./test-packages`

1. Add a local NuGet package config

   `dotnet nuget add source ./test-packages`

1. Generate a local package

   `dotnet pack .\src\Kentico.Xperience.TagManager\ -c Release -o .\test-packages\ -p:SIGN_FILE=false`

1. Use the NuGet package in the DancingGoat project instead of the project reference

   1. Modify `examples\DancingGoat.csproj` replacing the `<ProjectReference Include="..\..\src\Kentico.Xperience.TagManager\Kentico.Xperience.TagManager.csproj" />` with `<PackageReference Include="Kentico.Xperience.TagManager" />`
   1. Update the `Directory.Packages.props` with a reference to the `Kentico.Xperience.TagManager` package `<PackageVersion Include="Kentico.Xperience.TagManager" Verison="" />` with a version matching the value in the project's `Directory.Build.props`

1. Run the `DancingGoat` application and ensure all functionality is correct.

1. Ensure these changes are not committed to the repository

### Create a PR

Once ready, create a PR on GitHub. The PR will need to have all comments resolved and all tests passing before it will be merged.

- The PR should have a helpful description of the scope of changes being contributed.
- Include screenshots or video to reflect UX or UI updates
- Indicate if new settings need to be applied when the changes are merged - locally or in other environments
