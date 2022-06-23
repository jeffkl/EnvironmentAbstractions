// Copyright (c) Jeff Kluge.
//
// Licensed under the MIT license.

using System;
using Xunit.Abstractions;

namespace EnvironmentAbstractions.UnitTests
{
    public abstract class TestBase : IDisposable
    {
        public TestBase(ITestOutputHelper testOutput)
        {
            TestOutput = testOutput;
        }

        public ITestOutputHelper TestOutput { get; }

        public virtual void Dispose()
        {
        }
    }
}