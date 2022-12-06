// Copyright (c) Jeff Kluge.
//
// Licensed under the MIT license.

using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// Represents an implementation of <see cref="IEnvironmentVariableProvider" /> that manipulates environment variables on the current system.
    /// </summary>
    public sealed class SystemEnvironmentVariableProvider : IEnvironmentVariableProvider
    {
        private SystemEnvironmentVariableProvider()
        {
        }

        /// <summary>
        /// Gets an <see cref="IEnvironmentVariableProvider" /> that manipulates environment variables on the current system.
        /// </summary>
        public static IEnvironmentVariableProvider Instance { get; } = new SystemEnvironmentVariableProvider();

        /// <inheritdoc />
        public string? ExpandEnvironmentVariables(string name) => Environment.ExpandEnvironmentVariables(name);

        /// <inheritdoc />
        public string? GetEnvironmentVariable(string name, EnvironmentVariableTarget target) => Environment.GetEnvironmentVariable(name, target);

        /// <inheritdoc />
        public string? GetEnvironmentVariable(string name) => Environment.GetEnvironmentVariable(name);

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> GetEnvironmentVariables() => new GetEnvironmentVariablesWrapper();

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target) => new GetEnvironmentVariablesWrapper(target);

        /// <inheritdoc />
        public void SetEnvironmentVariable(string name, string? value) => Environment.SetEnvironmentVariable(name, value);

        /// <inheritdoc />
        public void SetEnvironmentVariable(string name, string? value, EnvironmentVariableTarget target) => Environment.SetEnvironmentVariable(name, value, target);
    }
}