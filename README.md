# trainline

## Project Overview

This project contains the AddressProcessing library and its unit tests. The project has been upgraded to the latest .NET 10.0 with modern SDK-style project format.

## Upgrade Summary (Latest Change)

### Date: 2026-06-02
This project has been successfully upgraded from .NET Framework 4.6.1 to **.NET 10.0** with full modernization:

**Key Changes:**
- **Target Framework**: .NET Framework 4.6.1 → .NET 10.0
- **Project Format**: Legacy format → SDK-style projects
- **Visual Studio**: VS 2013 → VS 2022 compatible (latest MSBuild)
- **Test Framework**: NUnit 2.6.4 → NUnit 4.0.1
- **Test Infrastructure**: Updated to modern test runners (NUnit3TestAdapter 4.5.0, Microsoft.NET.Test.Sdk 17.9.0)

**Modernizations Applied:**
1. Converted `.csproj` files to SDK-style format (`<Project Sdk="Microsoft.NET.Sdk">`)
2. Removed obsolete MSBuild properties:
   - `TargetFrameworkVersion` → `TargetFramework`
   - `FileUpgradeFlags`, `UpgradeBackupLocation`, `OldToolsVersion`, `TargetFrameworkProfile`
3. Cleaned up legacy assembly info attributes (SDK projects auto-generate these)
4. Updated test assertions from legacy NUnit to modern constraint-based syntax:
   - `Assert.NotNull()` → `Assert.That(x, Is.Not.Null)`
   - `Assert.AreEqual()` → `Assert.That(x, Is.EqualTo(y))`
   - `Assert.DoesNotThrow()` → `Assert.That(() => ..., Throws.Nothing)`
   - `Assert.Throws<T>()` → `Assert.That(() => ..., Throws.TypeOf<T>())`
5. Removed unused code (CsvDataRecordMapper, CsvDataRecord) that had external dependencies
6. Added automatic test data copying with `<None Update="test_data\**" CopyToOutputDirectory="Always" />`

**Build & Test Results:**
- ✅ Solution builds successfully with no errors
- ✅ All 8 unit tests pass
- ✅ Full .NET 10.0 compatibility verified

**Migration Path Benefits:**
- Improved performance with latest .NET runtime
- Access to latest C# language features (nullable reference types, records, etc.)
- Better tooling and diagnostics
- Long-term support and security updates

## Building the Project

### Prerequisites
- .NET 10.0 SDK or later

### Build
```bash
cd src
dotnet build Exercise.sln
```

### Run Tests
```bash
cd src
dotnet test Exercise.sln
```

### Release Build
```bash
cd src
dotnet build Exercise.sln -c Release
```

## Project Structure

- **AddressProcessing**: Main library with address processing functionality
- **AddressProcessing.Tests**: Unit tests using NUnit framework
- **test_data**: Test data files (contacts.csv)

## Troubleshooting

If you encounter build issues after this upgrade:

1. **Clear NuGet cache**: `dotnet nuget locals all --clear`
2. **Clean build**: `dotnet clean && dotnet build`
3. **Restore dependencies**: `dotnet restore`

For compatibility issues, verify you have the correct .NET 10.0 SDK installed:
```bash
dotnet --version
```
