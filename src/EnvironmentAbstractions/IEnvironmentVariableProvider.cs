// Copyright (c) Jeff Kluge.
//
// Licensed under the MIT license.

using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// Provides a mechanism for manipulating environent variables on the current system.
    /// </summary>
    public interface IEnvironmentVariableProvider
    {
        /// <inheritdoc cref="Environment.ExpandEnvironmentVariables(string)" />
        string? ExpandEnvironmentVariables(string name);

        /// <inheritdoc cref="Environment.GetEnvironmentVariable(string, EnvironmentVariableTarget)" />
        string? GetEnvironmentVariable(string name, EnvironmentVariableTarget target);

        /// <inheritdoc cref="Environment.GetEnvironmentVariable(string)" />
        string? GetEnvironmentVariable(string name);

        /// <inheritdoc cref="Environment.GetEnvironmentVariables()" />
        IReadOnlyDictionary<string, string> GetEnvironmentVariables();

        /// <inheritdoc cref="Environment.GetEnvironmentVariables(EnvironmentVariableTarget)" />
        IReadOnlyDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target);

        /// <inheritdoc cref="Environment.SetEnvironmentVariable(string, string)" />
        void SetEnvironmentVariable(string name, string? value);

        /// <inheritdoc cref="Environment.SetEnvironmentVariable(string, string, EnvironmentVariableTarget)" />
        void SetEnvironmentVariable(string name, string? value, EnvironmentVariableTarget target);
    }
}