// Copyright (c) Jeff Kluge.
//
// Licensed under the MIT license.

using Moq;
using Shouldly;
using System;
using Xunit;

namespace EnvironmentAbstractions.UnitTests
{
    public class MoqTests
    {
        [Fact]
        public void GetEnvironmentVariableMoqTest()
        {
            Mock<IEnvironmentVariableProvider> environmentVariableProviderMock = new Mock<IEnvironmentVariableProvider>();

            environmentVariableProviderMock.Setup(i => i.GetEnvironmentVariable(It.Is<string>(i => i.Equals("Var1"))))
                .Returns("Value1");

            environmentVariableProviderMock.Setup(i => i.GetEnvironmentVariable(It.Is<string>(i => i.Equals("Var2"))))
                .Returns("Value2");

            IEnvironmentVariableProvider environmentVariableProvider = environmentVariableProviderMock.Object;

            environmentVariableProvider.GetEnvironmentVariable("Var1")
                .ShouldBe("Value1");

            environmentVariableProvider.GetEnvironmentVariable("Var2")
                .ShouldBe("Value2");
        }

        [Theory]
        [InlineData(EnvironmentVariableTarget.Machine)]
        [InlineData(EnvironmentVariableTarget.Process)]
        [InlineData(EnvironmentVariableTarget.User)]
        public void GetEnvironmentVariableWithEnvironmentVariableTargetMoqTest(EnvironmentVariableTarget environmentVariableTarget)
        {
            Mock<IEnvironmentVariableProvider> environmentVariableProviderMock = new Mock<IEnvironmentVariableProvider>();

            environmentVariableProviderMock.Setup(i => i.GetEnvironmentVariable(It.IsAny<string>(), It.Is<EnvironmentVariableTarget>(i => i.Equals(environmentVariableTarget))))
                .Returns("Value1");

            IEnvironmentVariableProvider environmentVariableProvider = environmentVariableProviderMock.Object;

            environmentVariableProvider.GetEnvironmentVariable("Anything", environmentVariableTarget)
                .ShouldBe("Value1");
        }
    }
}