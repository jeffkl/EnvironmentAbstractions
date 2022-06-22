// Copyright (c) Jeff Kluge.
//
// Licensed under the MIT license.

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System
{
    /// <summary>
    /// Represents a wrapper for the return value of <see cref="Environment.GetEnvironmentVariables()" /> as a <see cref="IReadOnlyDictionary{TKey, TValue}" />.
    /// </summary>
    internal class GetEnvironmentVariablesWrapper : IReadOnlyDictionary<string, string>
    {
        /// <summary>
        /// Stores the <see cref="Hashtable" /> which is being wrapped.
        /// </summary>
        private readonly Hashtable _hashtable;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEnvironmentVariablesWrapper" /> class with all current environment variables.
        /// </summary>
        public GetEnvironmentVariablesWrapper()
            : this(Environment.GetEnvironmentVariables())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEnvironmentVariablesWrapper" /> class with environment variables for the specified <see cref="EnvironmentVariableTarget" />.
        /// </summary>
        /// <param name="target">The <see cref="EnvironmentVariableTarget" /> to use when retrieving environent variables.</param>
        public GetEnvironmentVariablesWrapper(EnvironmentVariableTarget target)
            : this(Environment.GetEnvironmentVariables(target))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEnvironmentVariablesWrapper" /> class for the specified <see cref="IDictionary" /> object.
        /// </summary>
        /// <param name="environmentVariables">A <see cref="IDictionary" /> containing environment variables.</param>
        /// <exception cref="InvalidOperationException"><paramref name="environmentVariables" />'s underlying object is not of type <see cref="Hashtable" />.</exception>
        private GetEnvironmentVariablesWrapper(IDictionary environmentVariables)
        {
            // This should never throw unless something changes in the BCL
            if (environmentVariables is not Hashtable hashtable)
            {
                throw new InvalidOperationException("System.Environment.GetEnvironmentVariables(EnvironmentVariableTarget) returned an IDictionary that is not of type System.Collections.Hashtable");
            }

            _hashtable = hashtable!;
        }

        /// <inheritdoc cref="IReadOnlyCollection{T}.Count" />
        public int Count => _hashtable.Count;

        /// <inheritdoc cref="IReadOnlyDictionary{TKey, TValue}.Keys" />
        public IEnumerable<string> Keys => _hashtable.Keys.Cast<string>();

        /// <inheritdoc cref="IReadOnlyDictionary{TKey, TValue}.Values" />
        public IEnumerable<string> Values => _hashtable.Values.Cast<string>();

        /// <inheritdoc cref="P:System.Collections.Generic.IReadOnlyDictionary`2.Item(`0)" />
        public string this[string key] => (string)_hashtable[key];

        /// <inheritdoc cref="IReadOnlyDictionary{TKey, TValue}.ContainsKey(TKey)" />
        public bool ContainsKey(string key) => _hashtable.ContainsKey(key);

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()" />
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => new HashtableEnumeratorWrapper(_hashtable);

        /// <inheritdoc cref="IEnumerable.GetEnumerator()" />
        IEnumerator IEnumerable.GetEnumerator() => new HashtableEnumeratorWrapper(_hashtable);

        /// <inheritdoc cref="IReadOnlyDictionary{TKey, TValue}.TryGetValue(TKey, out TValue)" />
        public bool TryGetValue(string key, out string value)
        {
            value = null!;

            if (_hashtable.ContainsKey(key))
            {
                value = (string)_hashtable[key];

                return true;
            }

            return false;
        }

        /// <summary>
        /// Represents a wrapper that can enumerate a <see cref="Hashtable" /> as a <see cref="IEnumerator{T}" />.
        /// </summary>
        private class HashtableEnumeratorWrapper : IEnumerator<KeyValuePair<string, string>>
        {
            private readonly IDictionaryEnumerator _enumerator;

            /// <summary>
            /// Initializes a new instance of the <see cref="HashtableEnumeratorWrapper" /> class for the specified <see cref="Hashtable" />.
            /// </summary>
            /// <param name="hashtable">The <see cref="Hashtable" /> to enumerate.</param>
            public HashtableEnumeratorWrapper(Hashtable hashtable)
            {
                _enumerator = hashtable.GetEnumerator();
            }

            /// <inheritdoc cref="IEnumerator{T}.Current" />
            public KeyValuePair<string, string> Current
            {
                get
                {
                    if (_enumerator.Current is DictionaryEntry dictionaryEntry && dictionaryEntry.Key is string key && dictionaryEntry.Value is string value)
                    {
                        return new KeyValuePair<string, string>(key, value);
                    }

                    throw new InvalidOperationException("HashtableEnumeratorWrapper only supports enumerating a collection of DictionaryEntry objects");
                }
            }

            /// <inheritdoc cref="IEnumerator.Current" />
            object IEnumerator.Current => _enumerator.Current;

            /// <inheritdoc cref="IDisposable.Dispose" />
            public void Dispose()
            {
            }

            /// <inheritdoc cref="IEnumerator.MoveNext" />
            public bool MoveNext() => _enumerator.MoveNext();

            /// <inheritdoc cref="IEnumerator.Reset" />
            public void Reset() => _enumerator.Reset();
        }
    }
}