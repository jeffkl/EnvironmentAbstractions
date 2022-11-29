# `System.Environment` Abstractions for .NET

[![NuGet package EnvironmentAbstractions (with prereleases)](https://img.shields.io/nuget/vpre/EnvironmentAbstractions?label=EnvironmentAbstractions)](https://nuget.org/packages/EnvironmentAbstractions)
[![NuGet package EnvironmentAbstractions.TestHelpers (with prereleases)](https://img.shields.io/nuget/vpre/EnvironmentAbstractions?label=EnvironmentAbstractions.TestHelpers)](https://nuget.org/packages/EnvironmentAbstractions.TestHelpers)
[![NuGet package EnvironmentAbstractions.BannedApiAnalyzer (with prereleases)](https://img.shields.io/nuget/vpre/EnvironmentAbstractions?label=EnvironmentAbstractions.BannedApiAnalyzer)](https://nuget.org/packages/EnvironmentAbstractions.BannedApiAnalyzer)
[![Official Build](https://github.com/jeffkl/EnvironmentAbstractions/actions/workflows/Official.yml/badge.svg)](https://github.com/jeffkl/EnvironmentAbstractions/actions/workflows/Official.yml)

EnvironmentAbstractions is an interface abstraction for the `System.Environment` class in .NET to make testing components easier.  A lot of repositories have code that accesses environment variables
and corresponding logic to mock the functionality in a unit test.  Some people set environment variables during tests while others have their own interfaces.  However,
setting environment variables applies to the entire process which can limit the parallelism of test execution.  This API is similar to [System.IO.Abstractions](https://github.com/TestableIO/System.IO.Abstractions)
but intended for environment variable access.

There are two interfaces available, `IEnvironmentProvider` and `IEnvironmentVariableProvider`.  The `IEnvironmentProvider` abstracts away everything in `System.Environment` while `IEnvironmentVariableProvider` only provides abstractions for accessing environment variables.

## Getting Started

Add a `<PackageReference />` to the [EnvironmentAbstractions](https://nuget.org/packages/EnvironmentAbstractions) package:

```xml
<PackageReference Include="EnvironmentAbstractions" Version="1.0.0" />
```

## `IEnvironmentProvider`

The `IEnvironmentProvider` interfaces provides an abstraction for everything in the `System.Environment` class including getting system information, user information, or accessing environment variables.

Use the `IEnvironmentProvider` interface when accessing the environment your code:

```c#
public static void SayHello(IEnvironmentProvider environmentProvider)
{
    Console.WriteLine("Hello, {0}!", environmentProvider.UserName);
}

public static void PrintSortedEnvironmentVariables(IEnvironmentProvider environmentProvider)
{
    foreach (KeyValuePair<string, string> item in environmentProvider.GetEnvironmentVariables()
                                                      .OrderBy(i => i.Key))
    {
        Console.WriteLine("{0}={1}", item.Key, item.Value);
    }
}
```

Then you can call that code with the default environment variable provider `SystemEnvironmentProvider` and its singleton `SystemEnvironmentProvider.Instance`:

```c#
public static void Main(string[] args)
{
    IEnvironmentProvider environmentProvider = SystemEnvironmentProvider.Instance;

    SayHello(environmentProvider);

    PrintSortedEnvironmentVariables(environmentProvider);
}

public static void SayHello(IEnvironmentProvider environmentProvider)
{
    Console.WriteLine("Hello, {0}!", environmentProvider.UserName);
}

public static void PrintSortedEnvironmentVariables(IEnvironmentProvider environmentProvider)
{
    foreach (KeyValuePair<string, string> item in environmentProvider.GetEnvironmentVariables()
                                                      .OrderBy(i => i.Key))
    {
        Console.WriteLine("{0}={1}", item.Key, item.Value);
    }
}
```

Unit tests can use the `MockEnvironmentProvider` class from the [EnvironmentAbstractions.TestHelpers package](https://nuget.org/packages/EnvironmentAbstractions.TestHelpers)
to mock the values returned by environment access:

```c#
using EnvironmentAbstractions.TestHelpers;
using Xunit;

[Fact]
public void Test1()
{
    IEnvironmentProvider environmentProvider = new MockEnvironmentProvider();

    environmentVariableProvider["Variable1"] = "Value1";
    environmentVariableProvider["Variable2"] = "Value2";

    Program.PrintSortedEnvironmentVariables(environmentVariableProvider);
}

[Fact]
public void Test2()
{
    IEnvironmentProvider environmentProvider = new MockEnvironmentProvider();

    environmentProvider.UserName = "UserA";

    Program.SayHello(environmentVariableProvider);
}
```

Unit tests can also use any system capable of mocking interfaces like [Moq](https://nuget.org/packages/Moq)
```c#
using Moq;

[Fact]
public void GetEnvironmentVariableMoqTest()
{
    Mock<IEnvironmentProvider> environmentProviderMock = new Mock<IEnvironmentProvider>();

    environmentProviderMock.Setup(i => i.GetEnvironmentVariable(It.Is<string>(i => i.Equals("Var1"))))
        .Returns("Value1");

    environmentProviderMock.Setup(i => i.GetEnvironmentVariable(It.Is<string>(i => i.Equals("Var2"))))
        .Returns("Value2");

    IEnvironmentProvider environmentProvider = environmentProviderMock.Object;

    Program.PrintSortedEnvironmentVariables(environmentProvider);
}
```

## `IEnvironmentVariableProvider`

If your project only wants to abstract environment variable access, you can use only the `IEnvironmentVariableProvider` interface.

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
to mock environment variables:

```c#
using EnvironmentAbstractions.TestHelpers;
using Xunit;

[Fact]
public void Test1()
{
    IEnvironmentVariableProvider environmentVariableProvider = new MockEnvironmentVariableProvider();

    environmentVariableProvider["Variable1"] = "Value1";
    environmentVariableProvider["Variable2"] = "Value2";

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
warning RS0030: The symbol 'Environment.GetEnvironmentVariable(string)' is banned in this project: Use IEnvironmentProvider.GetEnvironmentVariable(string) instead.
