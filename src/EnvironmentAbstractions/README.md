# EnvironmentAbstractions
 ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/EnvironmentAbstractions?label=EnvironmentAbstractions)

The `EnvironmentAbstractions` package provides an interface, `IEnvironmentVariableProvider`, that can be used as a layer of abstraction for code to be easier to mock for unit testing.

## IEnvironmentVariableProvider

The `IEnvironmentVariableProvider` interface supports the following methods:

```c#
public interface IEnvironmentVariableProvider
{
    string? ExpandEnvironmentVariables(string name);

    string? GetEnvironmentVariable(string name, EnvironmentVariableTarget target);

    string? GetEnvironmentVariable(string name);

    IReadOnlyDictionary<string, string> GetEnvironmentVariables();

    IReadOnlyDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target);

    void SetEnvironmentVariable(string name, string? value);

    void SetEnvironmentVariable(string name, string? value, EnvironmentVariableTarget target);
}
```


## Examples
The example below shows how to use `IEnvironmentVariableProvider` in a class to read environment variables.  You should have two constructors: one that does default logic
and another that accepts an `IEnvironmentVariableProvider` instance. The constructor that accepts an `IEnvironmentVariableProvider` can be internal so it can only be
called by unit tests.  

```C#
public class MyClass
{
    private readonly IEnvironmentVariableProvider environmentVariableProvider;

    /// <summary>
    /// Internal constructor only for unit tests which reads environment variables from the specified IEnvironmentVariableProvider.
    /// </summary>
    internal MyClass(IEnvironmentVariableProvider environmentVariableProvider)
    {
        this.environmentVariableProvider = environmentVariableProvider
    }

    /// <summary>
    /// Initializes a new instance of the MyClass class which reads environment variables directly from the system.
    /// </summary>
    public MyClass()
      : this(SystemEnvironmentVariableProvider.Instance)
    {
    }

    public CustomConfig GetConfiguration()
    {
        // Check if the user has set an environment variable to override the default location
        string customConfigLocation = environmentVariableProvider.GetEnvironmentVariable("CUSTOM_CONFIG");

        if (!string.IsNullOrWhitespace(environmentVariableProvider))
        {
            return LoadConfigFromLocation(customConfigLocation);
        }

        // Load the configuration from the default location, %UserProfile%\configuration.xml
        return LoadConfiguationFromLocation(Path.Combine(environmentVariableProvider.GetEnvironmentVariable("USERPROFILE"), "configuration.xml"));
    }
}
```

Unit tests can then use the `MockEnvironmentVariableProvider` class from the
[EnvironmentAbstractions.TestHelpers package](https://nuget.org/packages/EnvironmentAbstractions.TestHelpers), an existing mocking framework like
[Moq](https://nuget.org/packages/Moq), or any custom implementation.

```c#
[Fact]
public void MethodReturnsExpectedValue()
{
    IEnvironmentVariableProvider environmentVariableProvider = new MockEnvironmentVariableProvider
    {
        ["Variable1"] = "Value1";
    };

    MyClass instance = new MyClass(environmentVariableProvider);

    string value = instance.Method();
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
```
