# Xperience by Kentico Tag Manager

**Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.**

Xperience by Kentico Tag Manager is a .NET 8 class library that integrates with Xperience by Kentico CMS to enable marketers to include prebuilt and custom tags into websites. The project includes a sample DancingGoat application demonstrating the integration.

## Working Effectively

### Environment Setup
- Install .NET SDK 8.0.411 exactly as specified in `global.json`:
  - `curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 8.0.411`
  - `export PATH="$HOME/.dotnet:$PATH"`
- Install Node.js 22.x for frontend builds (Client project requires >=22 <23):
  - `cd /tmp && wget https://nodejs.org/dist/v22.11.0/node-v22.11.0-linux-x64.tar.xz && tar -xf node-v22.11.0-linux-x64.tar.xz`
  - `export PATH="/tmp/node-v22.11.0-linux-x64/bin:$PATH"`
- **CRITICAL**: Set both paths in environment: `export PATH="/tmp/node-v22.11.0-linux-x64/bin:$HOME/.dotnet:$PATH"`

### Build Commands - TIMING VERIFIED
- **NEVER CANCEL BUILDS OR LONG-RUNNING COMMANDS** - Always use timeout of 600+ seconds.
- `dotnet restore --locked-mode` -- takes ~2 seconds, NEVER CANCEL, timeout 300+ seconds
- `dotnet build --configuration Release --no-restore` -- takes ~26 seconds with npm builds, NEVER CANCEL. Set timeout to 600+ seconds.
- `dotnet build --configuration Debug` -- takes ~7 seconds for incremental builds, timeout 300+ seconds
- `dotnet clean` -- takes ~1 second, quick cleanup command
- `dotnet format src/Kentico.Xperience.TagManager/Kentico.Xperience.TagManager.csproj` -- fix code formatting, required before commits

### Test Commands - TIMING VERIFIED
- `dotnet test --configuration Release --no-build --no-restore` -- takes ~2 seconds, but test project is currently empty
- No functional tests are currently implemented

### Package Testing - TIMING VERIFIED
To test local package changes:
1. `dotnet pack ./src/Kentico.Xperience.TagManager -c Release -o nuget-local -p:SIGN_FILE=false` -- takes ~22 seconds, creates local NuGet package
2. Update `Directory.Packages.props`: Set `Kentico.Xperience.TagManager` version to match the version in `Directory.Build.props` (currently 4.2.2)
3. Update `nuget.config`: Uncomment `<package pattern="Kentico.Xperience.TagManager" />` in LocalPackages section
4. `dotnet build -p:LOCAL_NUGET=true` -- takes ~7 seconds, builds solution using local package
5. Verify DLL version in `examples/DancingGoat/bin/Debug/net8.0/` matches expected version
6. **IMPORTANT**: Undo changes to `Directory.Packages.props` and `nuget.config` before committing

### Frontend Development - TIMING VERIFIED
- Admin UI has two Node.js projects with different requirements:
  - `src/Kentico.Xperience.TagManager/Admin/Client/` (requires Node.js >=22 <23)
  - `src/Kentico.Xperience.TagManager/Admin/FrontEnd/` (requires Node.js >=20.11.0 <21)
- Frontend builds are automatically triggered during .NET builds
- Manual frontend commands (tested with Node 22):
  - `npm install` -- takes ~4 seconds in Client directory
  - `npm run build` -- takes ~7 seconds in Client directory
  - Webpack builds are integrated into .NET build process

## Validation

### Required Pre-Commit Validation
- **CRITICAL**: `dotnet format src/Kentico.Xperience.TagManager/Kentico.Xperience.TagManager.csproj` -- MUST run before committing or CI will fail
- Build succeeds: `dotnet build --configuration Release --no-restore`
- Package creation works: `dotnet pack ./src/Kentico.Xperience.TagManager -c Release -o nuget-local -p:SIGN_FILE=false`

### Manual Testing Requirements
- **CRITICAL**: Always test actual functionality after making changes, not just build success
- Test scenario: Create a tag in the Tag Management UI, verify it renders on the DancingGoat sample site
- The DancingGoat sample application requires a SQL Server database (see Database Setup below)
- **VALIDATION SCENARIOS**: Test the tag manager by adding a Google Tag Manager snippet, saving it, and verifying it appears in page source

### Database Setup for DancingGoat Sample
**Note**: The DancingGoat sample requires a full Xperience by Kentico database setup which is complex and may not be practical in all environments.
- Requires SQL Server 2019+ compatible database
- Follow [Xperience documentation](https://docs.xperience.io/xp26/developers-and-admins/installation#Installation-CreatetheprojectdatabaseCreateProjectDatabase) for database creation
- Database setup is **not required** for library development and testing - only for full end-to-end validation

### Admin Development Mode
For admin UI development, add to DancingGoat User Secrets:
```json
"CMSAdminClientModuleSettings": {
  "kentico-xperience-integrations-tagmanager": {
    "Mode": "Proxy",
    "Port": 3009
  }
}
```

## Common Tasks

### Repository Structure
```
.
├── src/Kentico.Xperience.TagManager/     # Main library project
│   ├── Admin/Client/                     # React admin UI (Node 22+)
│   ├── Admin/FrontEnd/                   # Legacy admin frontend (Node 20+)
│   ├── Snippets/                         # Tag snippet factories
│   └── Rendering/                        # Tag rendering components
├── examples/DancingGoat/                 # Sample application
├── tests/Kentico.Xperience.TagManager.Tests/  # Test project (empty)
├── docs/                                 # Documentation
├── .github/workflows/ci.yml             # CI pipeline
├── global.json                          # .NET SDK version (8.0.411)
├── Directory.Build.props                # Project properties
├── Directory.Packages.props             # NuGet package versions
└── nuget.config                         # NuGet configuration
```

### Key Files
- `global.json` - Specifies exact .NET SDK version requirement (8.0.411)
- `Directory.Build.props` - Contains version (4.2.2) and package metadata
- `Directory.Packages.props` - Central package version management
- `nuget.config` - Local package testing configuration
- `.github/workflows/ci.yml` - Build and test automation
- `.vscode/tasks.json` - VS Code build tasks

### VS Code Tasks (Alternative Commands)
- `.NET: build (Solution)` - Standard build
- `.NET: build (Solution) - LOCAL_NUGET` - Build with local package
- `.NET: pack (TagManager)` - Create NuGet package
- `.NET: format (TagManager)` - Format code
- `npm: build - Admin/Client` - Build admin UI
- `npm: build - Admin/FrontEnd` - Build legacy frontend

### Code Generation Commands (DancingGoat Only)
If working with the DancingGoat sample content model:
```bash
cd examples/DancingGoat
dotnet run --no-build -- --kxp-codegen --location "./Models/Schema/" --type ReusableFieldSchemas --namespace "DancingGoat.Models"
dotnet run --no-build -- --kxp-codegen --location "./Models/Reusable/{name}/" --type ReusableContentTypes --include "DancingGoat.*" --namespace "DancingGoat.Models"
dotnet run --no-build -- --kxp-codegen --location "./Models/WebPage/{name}/" --type PageContentTypes --include "DancingGoat.*" --namespace "DancingGoat.Models"
```

### CI Pipeline Information
- **Triggers**: Changes to .cs, .cshtml, .tsx, .js, .json, .csproj, .props, .targets, .sln files
- **Commands**: 
  1. `dotnet restore --locked-mode`
  2. `dotnet build --configuration Release --no-restore`
  3. `dotnet test --configuration Release --no-build --no-restore`
- **Environment**: Ubuntu latest with PowerShell
- Build failures typically indicate formatting issues - run `dotnet format` to fix

### Complete Development Workflow Example
```bash
# Setup environment
export PATH="/tmp/node-v22.11.0-linux-x64/bin:$HOME/.dotnet:$PATH"
cd /path/to/repo

# Make changes to code
# ... edit files ...

# Format and build
dotnet format src/Kentico.Xperience.TagManager/Kentico.Xperience.TagManager.csproj
dotnet build --configuration Release --no-restore

# Test local package
dotnet pack ./src/Kentico.Xperience.TagManager -c Release -o nuget-local -p:SIGN_FILE=false

# Update Directory.Packages.props version to 4.2.2
# Uncomment package pattern in nuget.config
dotnet build -p:LOCAL_NUGET=true

# Test DancingGoat if database available
# ... manual testing ...

# Revert config changes before commit
# Reset Directory.Packages.props and nuget.config
```

### Important Notes
- **TIMING**: Set timeouts of 600+ seconds for builds, 300+ seconds for other operations
- **NODE VERSIONS**: Different frontend projects require different Node.js versions - ensure PATH is set correctly
- **FORMATTING**: Always run `dotnet format` before committing - CI enforces formatting rules
- **PACKAGE TESTING**: Local package testing requires manual configuration changes that must be reverted
- **DATABASE**: Full sample application testing requires SQL Server database setup
- **EMPTY TESTS**: Test project exists but contains no tests - manual validation is required

### Known Issues and Warnings (Non-Fatal)
- Node.js version warnings during build (engines mismatch between projects) - these are non-fatal
- Empty test project - no automated tests are implemented
- Browserslist outdated warnings - can be ignored or fixed with `npx update-browserslist-db@latest`
- Mixed line ending issues may require `dotnet format` to resolve
- npm audit warnings about low severity vulnerabilities - can be ignored for development