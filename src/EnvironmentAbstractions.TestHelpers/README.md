# EnvironmentAbstractions.TestHelpers

The `EnvironmentAbstractions.TestHelpers` provides a helper implementation of `IEnvironmentVariableProvider` which can be passed to classes to mock environment variable values.

## Examples

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
    IEnvironmentVariableProvider environmentVariableProvider = new MockEnvironmentVariableProvider(addExistingEnvironmentVariables: true)
    {
        ["UserName"] = @"User1";
    };

    // Accessing environment variables returns actual values except %UserName% has been changed for just this test
}
```