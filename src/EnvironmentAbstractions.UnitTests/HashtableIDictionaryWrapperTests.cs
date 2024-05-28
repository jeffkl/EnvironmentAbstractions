// Copyright (c) Jeff Kluge.
//
// Licensed under the MIT license.

using Shouldly;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EnvironmentAbstractions.UnitTests
{
    public class HashtableIDictionaryWrapperTests : TestBase
    {
        public HashtableIDictionaryWrapperTests(ITestOutputHelper testOutput)
            : base(testOutput)
        {
        }

        [Fact]
        public void GetEnvironmentVariablesWrapperContainsKeyReturnsSameValueAsGetEnvironmentVariable()
        {
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();

            GetEnvironmentVariablesWrapper wrapper = new GetEnvironmentVariablesWrapper(environmentVariables);

            foreach (KeyValuePair<string, string> environmentVariable in environmentVariables.Cast<DictionaryEntry>().Select(i => new KeyValuePair<string, string>((string)i.Key, (string)i.Value!)))
            {
                wrapper.ContainsKey(environmentVariable.Key).ShouldBeTrue();
            }
        }

        [Fact]
        public void GetEnvironmentVariablesWrapperEnumerateSeveralTimes()
        {
            GetEnvironmentVariablesWrapper wrapper = new GetEnvironmentVariablesWrapper();

            foreach (int iteration in Enumerable.Range(0, 3))
            {
                int count = 0;

                foreach (var item in wrapper)
                {
                    count++;

                    item.Key.ShouldNotBeNull();
                }

                count.ShouldNotBe(0);
            }
        }

        [Fact]
        public void GetEnvironmentVariablesWrapperGetEnumeratorSameAsGetEnvironmentVariables()
        {
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();

            GetEnvironmentVariablesWrapper wrapper = new GetEnvironmentVariablesWrapper(environmentVariables);

            Dictionary<string, string> expected = environmentVariables.Cast<DictionaryEntry>().ToDictionary(i => (string)i.Key, i => (string)i.Value!);

            wrapper.ToDictionary(i => i.Key, i => i.Value).ShouldBeSubsetOf(expected);
        }

        [Fact]
        public void GetEnvironmentVariablesWrapperIndexerReturnsSameValueAsGetEnvironmentVariable()
        {
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();

            GetEnvironmentVariablesWrapper wrapper = new GetEnvironmentVariablesWrapper(environmentVariables);

            foreach (KeyValuePair<string, string> environmentVariable in environmentVariables.Cast<DictionaryEntry>().Select(i => new KeyValuePair<string, string>((string)i.Key, (string)i.Value!)))
            {
                string actual = wrapper[environmentVariable.Key];

                actual.ShouldBe(environmentVariable.Value);
            }
        }

        [Fact]
        public void GetEnvironmentVariablesWrapperKeysSameAsGetEnvironmentVariables()
        {
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();

            GetEnvironmentVariablesWrapper wrapper = new GetEnvironmentVariablesWrapper(environmentVariables);

            IEnumerable<string> expected = environmentVariables.Cast<DictionaryEntry>().Select(i => (string)i.Key);

            wrapper.Keys.ShouldBe(expected);
        }

        [Fact]
        public void GetEnvironmentVariablesWrapperNonExistentValueSameAsGetEnvironmentVariable()
        {
            GetEnvironmentVariablesWrapper wrapper = new GetEnvironmentVariablesWrapper();

            wrapper.TryGetValue("SOMETHINGTHATDOESNOTEXIST", out string value).ShouldBeFalse();

            value.ShouldBeNull();
        }

        [Fact]
        public void GetEnvironmentVariablesWrapperTryGetValueReturnsSameValueAsGetEnvironmentVariable()
        {
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();

            GetEnvironmentVariablesWrapper wrapper = new GetEnvironmentVariablesWrapper(environmentVariables);

            foreach (KeyValuePair<string, string> environmentVariable in environmentVariables.Cast<DictionaryEntry>().Select(i => new KeyValuePair<string, string>((string)i.Key, (string)i.Value!)))
            {
                wrapper.TryGetValue(environmentVariable.Key, out string actual).ShouldBeTrue($"{environmentVariable.Key} should exist");

                actual.ShouldBe(environmentVariable.Value);
            }
        }

        [Fact]
        public void GetEnvironmentVariablesWrapperValuesSameAsGetEnvironmentVariables()
        {
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();

            GetEnvironmentVariablesWrapper wrapper = new GetEnvironmentVariablesWrapper(environmentVariables);

            IEnumerable<string> expected = environmentVariables.Cast<DictionaryEntry>().Select(i => (string)i.Value!);

            wrapper.Values.ShouldBe(expected);
        }
    }
}