# EnvironmentAbstractions.BannedApiAnalyzer
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/EnvironmentAbstractions.BannedApiAnalyzer?label=EnvironmentAbstractions.BannedApiAnalyzer)

The `EnvironmentAbstractions.BannedApiAnalyzer` package uses the
[Microsoft.CodeAnalysis.BannedApiAnalyzers](https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.BannedApiAnalyzers/BannedApiAnalyzers.Help.md)
package to prevent usages of `System.Environment` to access the environment.

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

Then when anyone attempts to call the built-in methods for accessing the environment, they'll receive a build warning indicating that they should use `IEnvironmentProvider` instead:

```c#
public static void Main(string[] args)
{
    Console.WriteLine("Hello, {0}!", System.Environment.UserName);
}
```

```
warning RS0030: The symbol 'Environment.UserName' is banned in this project: Use IEnvironmentProvider.UserName instead.
```

If you want to only ban access to environment variables, you'll need to first set the MSBuild property `BanSystemEnvironmentVariableAPIs` to `true` in your project:

```xml
<PropertyGroup>
  <!-- Only ban accessing environment variables in this project -->
  <BanSystemEnvironmentVariableAPIs>true</BanSystemEnvironmentVariableAPIs>
</PropertyGroup>
```

Then only accessing environment variables will emit a compile-time error telling developers to use `IEnvironmentVariableProvider` instead:

```c#
public static void Main(string[] args)
{
    Console.WriteLine("Hello, {0}!", System.Environment.GetEnvironmentVariable("USERNAME"));
}
```

```
warning RS0030: The symbol 'Environment.GetEnvironmentVariable(string)' is banned in this project: Use IEnvironmentVariableProvider.GetEnvironmentVariable(string) instead.
```
