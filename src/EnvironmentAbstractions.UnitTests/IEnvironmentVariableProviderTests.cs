// Copyright (c) Jeff Kluge.
//
// Licensed under the MIT license.

using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace EnvironmentAbstractions.UnitTests
{
    public class IEnvironmentVariableProviderTests
    {
        [Fact]
        public void GetEnvironmentVariableTest()
        {
            IEnvironmentVariableProvider mock = new ImplementationOfIIEnvironmentVariableProvider();

            mock.GetEnvironmentVariable("Test").ShouldBe("Test");
        }

        private class ImplementationOfIIEnvironmentVariableProvider : IEnvironmentVariableProvider
        {
            public string? ExpandEnvironmentVariables(string name)
            {
                throw new NotImplementedException();
            }

            public string? GetEnvironmentVariable(string name, EnvironmentVariableTarget target)
            {
                return name;
            }

            public string? GetEnvironmentVariable(string name)
            {
                return name;
            }

            public IReadOnlyDictionary<string, string> GetEnvironmentVariables()
            {
                throw new NotImplementedException();
            }

            public IReadOnlyDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target)
            {
                throw new NotImplementedException();
            }

            public void SetEnvironmentVariable(string name, string? value)
            {
            }

            public void SetEnvironmentVariable(string name, string? value, EnvironmentVariableTarget target)
            {
            }
        }
    }
}