# EnvironmentAbstractions.BannedApiAnalyzer
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/EnvironmentAbstractions.BannedApiAnalyzer?label=EnvironmentAbstractions.BannedApiAnalyzer)

The `EnvironmentAbstractions.BannedApiAnalyzer` package uses the
[Microsoft.CodeAnalysis.BannedApiAnalyzers](https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.BannedApiAnalyzers/BannedApiAnalyzers.Help.md)
package to prevent usages of `System.Environment` to access environment variables.

Adding a `<PackageReference />` to the package in your project is all you need:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="EnvironmentAbstractions.BannedApiAnalyzer" Version="1.0.0" />
  </ItemGroup>
</Project>
```

Then when anyone attempts to call the built-in methods for read environment variables, they'll receive a build warning indicating that they should use `IEnvironmentVariableProvider` instead:

```c#
public static void Main(string[] args)
{
    Console.WriteLine("Hello, {0}!", System.Environment.GetEnvironmentVariable("USERNAME"));
}
```

```
warning RS0030: The symbol 'Environment.GetEnvironmentVariable(string)' is banned in this project: Use IEnvironmentVariableProvider.GetEnvironmentVariable(string) instead.
```
