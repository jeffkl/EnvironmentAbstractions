# `System.Environment` Abstractions for .NET

![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/EnvironmentAbstractions?label=EnvironmentAbstractions)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/EnvironmentAbstractions.TestHelpers?label=EnvironmentAbstractions.TestHelpers)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/EnvironmentAbstractions.BannedApiAnalyzer?label=EnvironmentAbstractions.BannedApiAnalyzer)

[![Official Build](https://github.com/jeffkl/EnvironmentAbstractions/actions/workflows/Official.yml/badge.svg)](https://github.com/jeffkl/EnvironmentAbstractions/actions/workflows/Official.yml)

EnvironmentAbstractions is an interface abstraction for accessing environment variables in .NET to make testing components easier.  A lot of repositories have code that accesses environment variables
and corresponding logic to mock the functionality in a unit test.  Some people set environment variables during tests while others have their own interfaces.  However,
setting environment variables applies to the entire process which can limit the parallelism of test execution.  This API is similar to [System.IO.Abstractions](https://github.com/TestableIO/System.IO.Abstractions)
but intended for environment variable access.

## Usage
Add a `<PackageReference />` to the [EnvironmentAbstractions](https://nuget.org/packages/EnvironmentAbstractions) package:

```xml
<PackageReference Include="EnvironmentAbstractions" Version="1.0.0" />
```

Use the `IEnvironmentVariableProvider` interface when accessing environment variables in your code:

```c#
public static void PrintSortedEnvironmentVariables(IEnvironmentVariableProvider environmentVariableProvider)
{
    foreach (KeyValuePair<string, string> item in environmentVariableProvider.GetEnvironmentVariables()
                                                      .OrderBy(i => i.Key))
    {
        Console.WriteLine("{0}={1}", item.Key, item.Value);
    }
}
```

Then you can call that code with the default environment variable provider `SystemEnvironmentVariableProvider` and its singleton `SystemEnvironmentVariableProvider.Instance`:

```c#
public static void Main(string[] args)
{
    PrintSortedEnvironmentVariables(SystemEnvironmentVariableProvider.Instance);
}

public static void PrintSortedEnvironmentVariables(IEnvironmentVariableProvider environmentVariableProvider)
{
    foreach (KeyValuePair<string, string> item in environmentVariableProvider.GetEnvironmentVariables()
                                                      .OrderBy(i => i.Key))
    {
        Console.WriteLine("{0}={1}", item.Key, item.Value);
    }
}
```

Unit tests can use the `MockEnvironmentVariableProvider` class from the [EnvironmentAbstractions.TestHelpers package](https://nuget.org/packages/EnvironmentAbstractions.TestHelpers)
the values that the method accesses:

```c#
using EnvironmentAbstractions.TestHelpers;
using Xunit;

[Fact]
public void Test1()
{
    IEnvironmentVariableProvider environmentVariableProvider = new MockEnvironmentVariableProvider
    {
        ["Variable1"] = "Value1",
        ["Variable2"] = "Value2",
    }

    Program.PrintSortedEnvironmentVariables(environmentVariableProvider);
}
```

Unit tests can also use any system capable of mocking interfaces like [Moq](https://nuget.org/packages/Moq)
```c#
using Moq;

[Fact]
public void GetEnvironmentVariableMoqTest()
{
    Mock<IEnvironmentVariableProvider> environmentVariableProviderMock = new Mock<IEnvironmentVariableProvider>();

    environmentVariableProviderMock.Setup(i => i.GetEnvironmentVariable(It.Is<string>(i => i.Equals("Var1"))))
        .Returns("Value1");

    environmentVariableProviderMock.Setup(i => i.GetEnvironmentVariable(It.Is<string>(i => i.Equals("Var2"))))
        .Returns("Value2");

    IEnvironmentVariableProvider environmentVariableProvider = environmentVariableProviderMock.Object;

    Program.PrintSortedEnvironmentVariables(environmentVariableProvider);
}
```

## Preventing usage of `System.Environment`

If you want to use `IEnvironmentVariableProvider` exclusively in your repository, you can reference the
[EnvironmentAbstractions.BannedApiAnalyzer](https://nuget.org/packages/EnvironmentAbstractions.BannedApiAnalyzer) package which uses the
[Microsoft.CodeAnalysis.BannedApiAnalyzers](https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.BannedApiAnalyzers/BannedApiAnalyzers.Help.md)
Roslyn analyzer to prevent code from accessing the `System.Environment` APIs.

Sample project
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

Sample source code
```c#
public static void Main(string[] args)
{
    Console.WriteLine("Hello, {0}!", System.Environment.GetEnvironmentVariable("USERNAME"));
}
```

Sample error
```
warning RS0030: The symbol 'Environment.GetEnvironmentVariable(string)' is banned in this project: Use IEnvironmentVariableProvider.GetEnvironmentVariable(string) instead.
