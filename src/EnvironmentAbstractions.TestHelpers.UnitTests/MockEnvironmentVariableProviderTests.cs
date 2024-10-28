// Copyright (c) Jeff Kluge.
//
// Licensed under the MIT license.

using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace EnvironmentAbstractions.TestHelpers.UnitTests
{
    public class MockEnvironmentVariableProviderTests
    {
        private const string EnvironmentVariable1Name = "Var1";
        private const string EnvironmentVariable1Value = "Value1";
        private const string EnvironmentVariable2Name = "Var2";
        private const string EnvironmentVariable2Value = "Value2";
        private const string EnvironmentVariable3Name = "Var3";
        private const string EnvironmentVariable3Value = "Value3";

        [Fact]
        public void AddsExistingEnvironmentVariablesTest()
        {
            IEnvironmentVariableProvider environmentVariableProvider = new MockEnvironmentVariableProvider(addExistingEnvironmentVariables: true)
            {
                ["COMPUTERNAME"] = "MyComputer01",
            };

            environmentVariableProvider.GetEnvironmentVariable("COMPUTERNAME")
                .ShouldBe("MyComputer01");

            environmentVariableProvider.GetEnvironmentVariable("USERNAME")
                .ShouldBe(SystemEnvironmentVariableProvider.Instance.GetEnvironmentVariable("USERNAME"));
        }

        [Fact]
        public void AddTest()
        {
            MockEnvironmentVariableProvider mockEnvironmentVariableProvider = new MockEnvironmentVariableProvider();

            mockEnvironmentVariableProvider[EnvironmentVariable1Name].ShouldBeNull();

            mockEnvironmentVariableProvider.Add(EnvironmentVariable1Name, EnvironmentVariable1Value);

            mockEnvironmentVariableProvider[EnvironmentVariable1Name].ShouldBe(EnvironmentVariable1Value);
        }

        [Fact]
        public void ExpandEnvironmentVariablesTest()
        {
            IEnvironmentVariableProvider environmentVariableProvider = new MockEnvironmentVariableProvider
            {
                [EnvironmentVariable1Name] = EnvironmentVariable1Value,
                [EnvironmentVariable2Name] = EnvironmentVariable2Value,
            };

            environmentVariableProvider.ExpandEnvironmentVariables("Leading text %VAR1% middle text %VAR2% trailing text")
                .ShouldBe("Leading text Value1 middle text Value2 trailing text");
        }

        [Theory]
        [InlineData(EnvironmentVariableTarget.Machine)]
        [InlineData(EnvironmentVariableTarget.Process)]
        [InlineData(EnvironmentVariableTarget.User)]
        [InlineData(null)]
        public void GetEnvironmentVariablesTest(EnvironmentVariableTarget? environmentVariableTarget)
        {
            IEnvironmentVariableProvider environmentVariableProvider = new MockEnvironmentVariableProvider
            {
                [EnvironmentVariable1Name] = EnvironmentVariable1Value,
                [EnvironmentVariable2Name] = EnvironmentVariable2Value,
            };

            IReadOnlyDictionary<string, string> actual = environmentVariableTarget == null
                ? environmentVariableProvider.GetEnvironmentVariables()
                : environmentVariableProvider.GetEnvironmentVariables(environmentVariableTarget.Value);

            actual
                .ShouldBe(
                    new Dictionary<string, string>
                    {
                        [EnvironmentVariable1Name] = EnvironmentVariable1Value,
                        [EnvironmentVariable2Name] = EnvironmentVariable2Value,
                    },
                    ignoreOrder: true);
        }

        [Theory]
        [InlineData(EnvironmentVariableTarget.Machine)]
        [InlineData(EnvironmentVariableTarget.Process)]
        [InlineData(EnvironmentVariableTarget.User)]
        [InlineData(null)]
        public void GetEnvironmentVariableWithEnvironmentVariableTarget(EnvironmentVariableTarget? environmentVariableTarget)
        {
            IEnvironmentVariableProvider environmentVariableProvider = new MockEnvironmentVariableProvider
            {
                [EnvironmentVariable1Name] = EnvironmentVariable1Value,
            };

            environmentVariableProvider.SetEnvironmentVariable(EnvironmentVariable2Name, EnvironmentVariable2Value, EnvironmentVariableTarget.User);
            environmentVariableProvider.SetEnvironmentVariable(EnvironmentVariable3Name, EnvironmentVariable3Value, EnvironmentVariableTarget.Machine);

            switch (environmentVariableTarget)
            {
                case EnvironmentVariableTarget.Machine:
                    environmentVariableProvider.GetEnvironmentVariable(EnvironmentVariable3Name, environmentVariableTarget.Value)
                        .ShouldBe(EnvironmentVariable3Value);

                    break;
                case EnvironmentVariableTarget.User:
                    environmentVariableProvider.GetEnvironmentVariable(EnvironmentVariable2Name, environmentVariableTarget.Value)
                        .ShouldBe(EnvironmentVariable2Value);
                    break;
                default:
                    string? actual = environmentVariableTarget == null
                        ? environmentVariableProvider.GetEnvironmentVariable(EnvironmentVariable1Name)
                        : environmentVariableProvider.GetEnvironmentVariable(EnvironmentVariable1Name, environmentVariableTarget.Value);
                    actual.ShouldBe(EnvironmentVariable1Value);
                    break;
            }
        }

        [Fact]
        public void IndexerTest()
        {
            MockEnvironmentVariableProvider mockEnvironmentVariableProvider = new MockEnvironmentVariableProvider();

            mockEnvironmentVariableProvider[EnvironmentVariable1Name].ShouldBeNull();

            mockEnvironmentVariableProvider[EnvironmentVariable1Name] = EnvironmentVariable1Value;

            mockEnvironmentVariableProvider[EnvironmentVariable1Name].ShouldBe(EnvironmentVariable1Value);

            mockEnvironmentVariableProvider[EnvironmentVariable2Name] = EnvironmentVariable2Value;

            mockEnvironmentVariableProvider[EnvironmentVariable2Name].ShouldBe(EnvironmentVariable2Value);
        }

        [Theory]
        [InlineData(EnvironmentVariableTarget.Machine)]
        [InlineData(EnvironmentVariableTarget.Process)]
        [InlineData(EnvironmentVariableTarget.User)]
        [InlineData(null)]
        public void SetEnvironmentVariableTest(EnvironmentVariableTarget? environmentVariableTarget)
        {
            IEnvironmentVariableProvider environmentVariableProvider = new MockEnvironmentVariableProvider();

            if (environmentVariableTarget == null)
            {
                environmentVariableProvider.SetEnvironmentVariable(EnvironmentVariable1Name, EnvironmentVariable2Value);
            }
            else
            {
                environmentVariableProvider.SetEnvironmentVariable(EnvironmentVariable1Name, EnvironmentVariable2Value, environmentVariableTarget.Value);
            }

            string? actual = environmentVariableTarget == null
                ? environmentVariableProvider.GetEnvironmentVariable(EnvironmentVariable1Name)
                : environmentVariableProvider.GetEnvironmentVariable(EnvironmentVariable1Name, environmentVariableTarget.Value);

            actual.ShouldBe(EnvironmentVariable2Value);

            if (environmentVariableTarget == null)
            {
                environmentVariableProvider.SetEnvironmentVariable(EnvironmentVariable1Name, null);
            }
            else
            {
                environmentVariableProvider.SetEnvironmentVariable(EnvironmentVariable1Name, null, environmentVariableTarget.Value);
            }

            environmentVariableProvider.GetEnvironmentVariable(EnvironmentVariable1Name)
                .ShouldBeNull();
        }
    }
}