// Copyright (c) Jeff Kluge.
//
// Licensed under the MIT license.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnvironmentAbstractions.TestHelpers
{
    /// <summary>
    /// Represents an implementation of <see cref="IEnvironmentVariableProvider" /> that stores values in memory.
    /// </summary>
    public class MockEnvironmentVariableProvider : IEnvironmentVariableProvider
    {
        private readonly ConcurrentDictionary<string, string>[] _dictionaries = new[]
        {
            new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase),
            new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase),
            new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase),
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="MockEnvironmentVariableProvider" /> class.
        /// </summary>
        /// <param name="addExistingEnvironmentVariables"><c>true</c> to have all existing environment variables added, otherwise <c>false</c> (the default).</param>
        public MockEnvironmentVariableProvider(bool addExistingEnvironmentVariables = false)
        {
            if (addExistingEnvironmentVariables)
            {
                for (int i = (int)EnvironmentVariableTarget.Machine; i >= 0; i--)
                {
                    EnvironmentVariableTarget environmentVariableTarget = (EnvironmentVariableTarget)i;

                    foreach (KeyValuePair<string, string> item in SystemEnvironmentVariableProvider.Instance.GetEnvironmentVariables(environmentVariableTarget))
                    {
                        Add(item.Key, item.Value, environmentVariableTarget);
                    }
                }
            }
        }

        /// <inheritdoc cref="SetEnvironmentVariable(string, string?)" />
        public string? this[string name]
        {
            get
            {
                return GetEnvironmentVariable(name);
            }

            set
            {
                SetEnvironmentVariable(name, value);
            }
        }

        /// <inheritdoc cref="SetEnvironmentVariable(string, string?)" />
        public void Add(string name, string? value)
        {
            SetEnvironmentVariable(name, value);
        }

        /// <inheritdoc cref="SetEnvironmentVariable(string, string?, EnvironmentVariableTarget)" />
        public void Add(string name, string? value, EnvironmentVariableTarget environmentVariableTarget)
        {
            SetEnvironmentVariable(name, value, environmentVariableTarget);
        }

        /// <inheritdoc cref="IEnvironmentVariableProvider.ExpandEnvironmentVariables(string)" />
        public string? ExpandEnvironmentVariables(string name)
        {
            const char EnvironmentVariableCharacter = '%';

            StringBuilder result = new StringBuilder();

            int lastPosition = 0;
            int currentPosition;

            while (lastPosition < name.Length && (currentPosition = name.IndexOf(EnvironmentVariableCharacter, lastPosition + 1)) >= 0)
            {
                if (name[lastPosition] == EnvironmentVariableCharacter)
                {
                    string key = name.Substring(lastPosition + 1, currentPosition - lastPosition - 1);

                    string? value = GetEnvironmentVariable(key);

                    if (value != null)
                    {
                        result.Append(value);

                        lastPosition = currentPosition + 1;

                        continue;
                    }
                }

                result.Append(name.Substring(lastPosition, currentPosition - lastPosition));

                lastPosition = currentPosition;
            }

            result.Append(name.Substring(lastPosition));

            return result.ToString();
        }

        /// <inheritdoc cref="IEnvironmentVariableProvider.GetEnvironmentVariable(string, EnvironmentVariableTarget)" />
        public string? GetEnvironmentVariable(string name, EnvironmentVariableTarget target)
        {
            ConcurrentDictionary<string, string> dictionary = _dictionaries[(int)target];

            return dictionary.TryGetValue(name, out string? value) ? value : default;
        }

        /// <inheritdoc cref="IEnvironmentVariableProvider.GetEnvironmentVariable(string)" />
        public string? GetEnvironmentVariable(string name)
        {
            return GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }

        /// <inheritdoc cref="IEnvironmentVariableProvider.GetEnvironmentVariables()" />
        public IReadOnlyDictionary<string, string> GetEnvironmentVariables()
        {
            return GetEnvironmentVariables(EnvironmentVariableTarget.Process);
        }

        /// <inheritdoc cref="IEnvironmentVariableProvider.GetEnvironmentVariables(EnvironmentVariableTarget)" />
        public IReadOnlyDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target)
        {
            Dictionary<string, string> environmentVariables = new Dictionary<string, string>(capacity: _dictionaries.Sum(i => i.Count), StringComparer.OrdinalIgnoreCase);

            for (int i = (int)EnvironmentVariableTarget.Machine; i >= 0; i--)
            {
                ConcurrentDictionary<string, string> dictionary = _dictionaries[i];

                foreach (var item in dictionary)
                {
                    environmentVariables[item.Key] = item.Value;
                }
            }

            return environmentVariables;
        }

        /// <inheritdoc cref="IEnvironmentVariableProvider.SetEnvironmentVariable(string, string?)" />
        public void SetEnvironmentVariable(string name, string? value)
        {
            SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Process);
        }

        /// <inheritdoc cref="IEnvironmentVariableProvider.SetEnvironmentVariable(string, string?, EnvironmentVariableTarget)" />
        public void SetEnvironmentVariable(string name, string? value, EnvironmentVariableTarget target)
        {
            ConcurrentDictionary<string, string> dictionary = _dictionaries[(int)target];

            if (value != null)
            {
                dictionary[name] = value;
            }
            else
            {
                dictionary.TryRemove(name, out _);
            }
        }
    }
}