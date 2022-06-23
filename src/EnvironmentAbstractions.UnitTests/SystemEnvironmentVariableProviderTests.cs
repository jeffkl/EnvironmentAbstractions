// Copyright (c) Jeff Kluge.
//
// Licensed under the MIT license.

using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace EnvironmentAbstractions.UnitTests
{
    public class SystemEnvironmentVariableProviderTests : TestBase
    {
        private const string EnvironentVariablePrefix = "__TEST_ENVVAR_FROM_";

        private readonly IEnvironmentVariableProvider _environmentVariableProvider = SystemEnvironmentVariableProvider.Instance;

        public SystemEnvironmentVariableProviderTests(ITestOutputHelper testOutput)
            : base(testOutput)
        {
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

        [Fact]
        public void GetEnvironmentVariableTest()
        {
            string environmentVariableName = $"{EnvironentVariablePrefix}{nameof(GetEnvironmentVariableTest)}";
            string environmentVariableValue = nameof(GetEnvironmentVariableTest);

            Environment.SetEnvironmentVariable(environmentVariableName, environmentVariableValue);

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

        [Fact]
        public void SetEnvironmentVariableTest()
        {
            string environmentVariableName = $"{EnvironentVariablePrefix}{nameof(SetEnvironmentVariableTest)}";
            string environmentVariableValue = nameof(SetEnvironmentVariableTest);

            _environmentVariableProvider.SetEnvironmentVariable(environmentVariableName, environmentVariableValue);

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