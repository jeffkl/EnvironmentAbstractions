// Copyright (c) Jeff Kluge.
//
// Licensed under the MIT license.

using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EnvironmentAbstractions.UnitTests
{
    public class SystemEnvironmentProviderTests : TestBase
    {
        private const string EnvironentVariablePrefix = "__TEST_ENVVAR_FROM_";

        private readonly IEnvironmentProvider _environmentVariableProvider = SystemEnvironmentProvider.Instance;

        public SystemEnvironmentProviderTests(ITestOutputHelper testOutput)
            : base(testOutput)
        {
        }

        public static IEnumerable<object[]> GetSpecialFolderValues()
        {
            return Enum.GetValues(typeof(Environment.SpecialFolder)).Cast<Environment.SpecialFolder>().Distinct().Select(i => new object[] { i });
        }

        [Fact]
        public void CommandLineTest()
        {
            _environmentVariableProvider.CommandLine.ShouldBe(Environment.CommandLine);
        }

        [Fact]
        public void CurrentDirectoryTest()
        {
            _environmentVariableProvider.CurrentDirectory.ShouldBe(Environment.CurrentDirectory);
        }

        [Fact]
        public void CurrentManagedThreadIdTest()
        {
            _environmentVariableProvider.CurrentManagedThreadId.ShouldBe(Environment.CurrentManagedThreadId);
        }

        [Fact]
        public void ExpandEnvironmentVariablesTest()
        {
            string environmentVariableName = $"{EnvironentVariablePrefix}{nameof(ExpandEnvironmentVariablesTest)}";
            string environmentVariableValue = nameof(ExpandEnvironmentVariablesTest);

            Environment.SetEnvironmentVariable(environmentVariableName, environmentVariableValue, EnvironmentVariableTarget.Process);

            try
            {
                _environmentVariableProvider.ExpandEnvironmentVariables($"leading text %{environmentVariableName}% middle text %{environmentVariableName}% trailing text")
                    .ShouldBe($"leading text {environmentVariableValue} middle text {environmentVariableValue} trailing text");
            }
            finally
            {
                Environment.SetEnvironmentVariable(environmentVariableName, value: null, EnvironmentVariableTarget.Process);
            }
        }

        [Fact]
        public void GetCommandLineArgsTest()
        {
            _environmentVariableProvider.GetCommandLineArgs().ShouldBe(Environment.GetCommandLineArgs());
        }

        [Theory]
        [InlineData(EnvironmentVariableTarget.Process)]
        [InlineData(EnvironmentVariableTarget.User)]
        [InlineData(null)]
        public void GetEnvironmentVariablesTest(EnvironmentVariableTarget? environmentVariableTarget)
        {
            IReadOnlyDictionary<string, string> environmentVariables = environmentVariableTarget == null
                ? _environmentVariableProvider.GetEnvironmentVariables()
                : _environmentVariableProvider.GetEnvironmentVariables(environmentVariableTarget.Value);

            foreach (KeyValuePair<string, string> item in environmentVariables)
            {
                item.Key.ShouldNotBeNull();
                item.Value.ShouldNotBeNull();
            }
        }

        [Theory]
        [MemberData(nameof(GetSpecialFolderValues))]
        public void GetFolderPathTest(Environment.SpecialFolder folder)
        {
            _environmentVariableProvider.GetFolderPath(folder).ShouldBe(Environment.GetFolderPath(folder));
        }

        [Fact]
        public void MachineNameTest()
        {
            _environmentVariableProvider.MachineName.ShouldBe(Environment.MachineName);
        }

        [Fact]
        public void SetEnvironmentVariableTest()
        {
            string environmentVariableName = $"{EnvironentVariablePrefix}{nameof(SetEnvironmentVariableTest)}";
            string environmentVariableValue = nameof(SetEnvironmentVariableTest);

            _environmentVariableProvider.SetEnvironmentVariable(environmentVariableName, environmentVariableValue, EnvironmentVariableTarget.Process);

            try
            {
                string? actual = _environmentVariableProvider.GetEnvironmentVariable(environmentVariableName);

                actual.ShouldBe(environmentVariableValue);
            }
            finally
            {
                Environment.SetEnvironmentVariable(environmentVariableName, value: null, EnvironmentVariableTarget.Process);
            }
        }
    }
}