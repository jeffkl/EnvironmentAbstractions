// Copyright (c) Jeff Kluge.
//
// Licensed under the MIT license.

using System;
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

        /// <inheritdoc cref="IEnvironmentVariableProvider.ExpandEnvironmentVariables(string)" />
        public string? ExpandEnvironmentVariables(string name) => Environment.ExpandEnvironmentVariables(name);

        /// <inheritdoc cref="IEnvironmentVariableProvider.GetEnvironmentVariable(string, EnvironmentVariableTarget)" />
        public string? GetEnvironmentVariable(string name, EnvironmentVariableTarget target) => Environment.GetEnvironmentVariable(name, target);

        /// <inheritdoc cref="IEnvironmentVariableProvider.GetEnvironmentVariable(string)" />
        public string? GetEnvironmentVariable(string name) => Environment.GetEnvironmentVariable(name);

        /// <inheritdoc cref="IEnvironmentVariableProvider.GetEnvironmentVariables()" />
        public IReadOnlyDictionary<string, string> GetEnvironmentVariables() => new GetEnvironmentVariablesWrapper();

        /// <inheritdoc cref="IEnvironmentVariableProvider.GetEnvironmentVariables(EnvironmentVariableTarget)" />
        public IReadOnlyDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target) => new GetEnvironmentVariablesWrapper(target);

        /// <inheritdoc cref="IEnvironmentVariableProvider.SetEnvironmentVariable(string, string?)" />
        public void SetEnvironmentVariable(string name, string? value) => Environment.SetEnvironmentVariable(name, value);

        /// <inheritdoc cref="IEnvironmentVariableProvider.SetEnvironmentVariable(string, string?, EnvironmentVariableTarget)" />
        public void SetEnvironmentVariable(string name, string? value, EnvironmentVariableTarget target) => Environment.SetEnvironmentVariable(name, value, target);
    }
}