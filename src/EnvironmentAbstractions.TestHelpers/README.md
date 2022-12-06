# EnvironmentAbstractions.TestHelpers
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/EnvironmentAbstractions.TestHelpers?label=EnvironmentAbstractions.TestHelpers)



## Examples of `MockEnvironmentProvider`

The `EnvironmentAbstractions.TestHelpers` provides a helper implementation of `IEnvironmentProvider` which can be passed to classes to mock access to the environment.

By default, all existing values are set from the current environment.
```c#
[Fact]
public void MethodReturnsExpectedValue()
{
    IEnvironmentProvider environmentProvider = new MockEnvironmentProvider();
    
    MyClass instance = new MyClass(environmentProvider);

    string value = instance.Method();
}
```

To use an instance of the environment without any values already set, specify the `useExistingEnvironmentValues` parameter to the `MockEnvironmentProvider` constructor:
```c#
[Fact]
public void MethodReturnsExpectedValue()
{
    IEnvironmentProvider environmentProvider = new MockEnvironmentProvider(useExistingEnvironmentValues: false);

    // The only Environment property with a value will be UserName
    environmentProvider.UserName = "UserA";
}
```

To use an instance of the environment with existing environment variable values,  specify the `addExistingEnvironmentVariables` parameter to the `MockEnvironmentProvider` constructor:
```c#
[Fact]
public void MethodReturnsExpectedValue()
{
    IEnvironmentProvider environmentProvider = new MockEnvironmentProvider(addExistingEnvironmentVariables: true);

    // All existing environment variables are set but UserName is overidden
    environmentProvider["UserName"] = "UserA";
}
```




## Examples of `MockEnvironmentVariableProvider`

The `EnvironmentAbstractions.TestHelpers` provides a helper implementation of `IEnvironmentVariableProvider` which can be passed to classes to mock environment variable values.

Use a clean set of environment variables where only what is added is available
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

To make all existing environment variables available, specify the `addExistingEnvironmentVariables` parameter to the `MockEnvironmentVariableProvider` constructor:
```c#
[Fact]
public void MethodReturnsExpectedValue()
{
    IEnvironmentVariableProvider environmentVariableProvider = new MockEnvironmentVariableProvider(addExistingEnvironmentVariables: true);

    environmentVariableProvider["UserName"] = @"User1";

    // Accessing environment variables returns actual values except %UserName% has been changed for just this test
}
```
