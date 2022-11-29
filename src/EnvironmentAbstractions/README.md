# EnvironmentAbstractions
 ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/EnvironmentAbstractions?label=EnvironmentAbstractions)

The `EnvironmentAbstractions` package provides an interface, `IEnvironmentVariableProvider`, that can be used as a layer of abstraction for code to be easier to mock for unit testing.


## IEnvironmentProvider
The `IEnvironmentProvider` interfaces supports the following members:

```c#
public interface IEnvironmentProvider : IEnvironmentVariableProvider
{
    string CommandLine { get; }

    string CurrentDirectory { get; set; }

    int CurrentManagedThreadId { get; }

    int ExitCode { get; }

    bool HasShutdownStarted { get; }

    bool Is64BitOperatingSystem { get; }

    bool Is64BitProcess { get; }

    string MachineName { get; }

    string NewLine { get; }

    OperatingSystem OSVersion { get; }

#if NET5_0_OR_GREATER
    int ProcessId { get; }
#endif

    int ProcessorCount { get; }

#if NET6_0_OR_GREATER
    string? ProcessPath { get; }
#endif

    string StackTrace { get; }

    string SystemDirectory { get; }

    int SystemPageSize { get; }

    int TickCount { get; }

#if NETCOREAPP3_1_OR_GREATER
    long TickCount64 { get; }
#endif

    string UserDomainName { get; }

    bool UserInteractive { get; }

    string UserName { get; }

    Version Version { get; }

    long WorkingSet { get; }

    void Exit(int exitCode);

    void FailFast(string? message);

    void FailFast(string? message, Exception? exception);

    string? ExpandEnvironmentVariables(string name);

    string? GetEnvironmentVariable(string name, EnvironmentVariableTarget target);

    string? GetEnvironmentVariable(string name);

    IReadOnlyDictionary<string, string> GetEnvironmentVariables();

    IReadOnlyDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target);

    string[] GetCommandLineArgs();

    string GetFolderPath(Environment.SpecialFolder folder);

    string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option);

    string[] GetLogicalDrives();

    void SetEnvironmentVariable(string name, string? value);

    void SetEnvironmentVariable(string name, string? value, EnvironmentVariableTarget target);
}
```

## IEnvironmentVariableProvider

The `IEnvironmentVariableProvider` interface is just for abstracting away access to environment variables and supports the following methods:

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
The example below shows how to use `IEnvironmentProvider` in a class to access the environment.  You should have two constructors: one that does default logic
and another that accepts an `IEnvironmentProvider` instance. The constructor that accepts an `IEnvironmentProvider` can be internal so it can only be
called by unit tests.  
```C#
public class MyClass
{
    private readonly IEnvironmentProvider environmentProvider;

    /// <summary>
    /// Internal constructor only for unit tests which accesses the environment from the specified IEnvironmentProvider.
    /// </summary>
    internal MyClass(IEnvironmentProvider environmentProvider)
    {
        this.environmentProvider = environmentProvider
    }

    /// <summary>
    /// Initializes a new instance of the MyClass class which accesses the environment directly from the system.
    /// </summary>
    public MyClass()
      : this(SystemEnvironmentProvider.Instance)
    {
    }

    public void SayHello()
    {
        // Get the current username from IEnvironmentProvider so that a test can mock the value
        Console.WriteLine("Hello {0}!", environmentProvider.UserName);
    }

    public CustomConfig GetConfiguration()
    {
        // Check if the user has set an environment variable to override the default location
        string customConfigLocation = environmentProvider.GetEnvironmentVariable("CUSTOM_CONFIG");

        if (!string.IsNullOrWhitespace(environmentProvider))
        {
            return LoadConfigFromLocation(customConfigLocation);
        }

        // Load the configuration from the default location, %UserProfile%\configuration.xml
        return LoadConfiguationFromLocation(Path.Combine(environmentProvider.GetEnvironmentVariable("USERPROFILE"), "configuration.xml"));
    }
}
```

Alternatively, you can use the `IEnvironmentVariableProvider` interface in a class to only abstract away access to environment variables.  Like the example above, you should have two constructors: one that does default logic
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

Unit tests can then use the `MockEnvironmentProvider` or `MockEnvironmentVariableProvider` class from the
[EnvironmentAbstractions.TestHelpers package](https://nuget.org/packages/EnvironmentAbstractions.TestHelpers), an existing mocking framework like
[Moq](https://nuget.org/packages/Moq), or any custom implementation.

`MockEnvironmentProvider`
```c#
[Fact]
public void MethodReturnsExpectedValue()
{
    IEnvironmentProvider environmentProvider = new MockEnvironmentProvider
    {
        // Initializes this instance to have the UserName property return "UserA"
        UserName = "UserA",
    };

    // Sets an environment variable value
    environmentProvider["Variable1"] = "Value1";

    MyClass instance = new MyClass(environmentVariableProvider);

    string actualUsername = instance.GetUserName();

    Assert.Equal(actualUsername, "UserA");
}
```


`MockEnvironmentVariableProvider`
```c#
[Fact]
public void MethodReturnsExpectedValue()
{
    IEnvironmentVariableProvider environmentVariableProvider = new MockEnvironmentVariableProvider

    environmentVariableProvider["Variable1"] = "Value1";

    MyClass instance = new MyClass(environmentVariableProvider);

    string value = instance.Method();
}
```

## Preventing usage of `System.Environment`

If you want to use `IEnvironmentProvider` or `IEnvironmentVariableProvider` exclusively in your repository, you can reference the
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
