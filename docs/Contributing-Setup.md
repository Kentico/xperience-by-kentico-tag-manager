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

1. Generate a local package using the VS Code `.NET: pack (TagManager)` task or execute its command and arguments at the command line
   This will generate a new `Kentico.Xperience.TagManager` package in the `nuget-local` folder with a version matching the version in your `Directory.Build.props`

1. Use the NuGet package in the DancingGoat project instead of the project reference

   1. Modify `examples\DancingGoat\DancingGoat.csproj` replacing the project reference with a package reference

      ```xml
      -<ProjectReference Include="..\..\src\Kentico.Xperience.TagManager\Kentico.Xperience.TagManager.csproj" />
      +<PackageReference Include="Kentico.Xperience.TagManager" />
      ```

   1. Update the `Directory.Packages.props` with a reference to the `Kentico.Xperience.TagManager` package

      ```xml
      +<PackageVersion Include="Kentico.Xperience.TagManager" Verison="" />
      ```

      Populate `Version=""` with the matching the value in the project's `Directory.Build.props`

1. Rebuild the solution using the VS Code `.NET: rebuild (Solution)` task or run `dotnet build --no-incremental` at the command line

1. Make sure the `Kentico.Xperience.TagManager.dll` version in the `examples\DancingGoat\bin\Debug\net6.0\` folder is the right version

1. Run the `DancingGoat` application and ensure all functionality is correct

1. Undo these changes to ensure they are not committed to the repository

### Create a PR

Once ready, create a PR on GitHub. The PR will need to have all comments resolved and all tests passing before it will be merged.

- The PR should have a helpful description of the scope of changes being contributed.
- Include screenshots or video to reflect UX or UI updates
- Indicate if new settings need to be applied when the changes are merged - locally or in other environments
