// Copyright (c) Jeff Kluge.
//
// Licensed under the MIT license.

using EnvironmentAbstractions.TestHelpers;
using System.Collections.Generic;
using System.Linq;

namespace System
{
    /// <summary>
    /// Represents an implementation of <see cref="IEnvironmentProvider" /> that stores values in memory.
    /// </summary>
    public class MockEnvironmentProvider : MockEnvironmentVariableProvider, IEnvironmentProvider
    {
        private static readonly OperatingSystem NullOperatingSystem = new OperatingSystem(0, new Version(1, 0));

        private static readonly Version Version1dot0 = NullOperatingSystem.Version;

        private string[] _commandLineArgs = Array.Empty<string>();
        private string[] _logicalDrives = Array.Empty<string>();

        private Dictionary<Environment.SpecialFolder, string> _specialFolders = new Dictionary<Environment.SpecialFolder, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MockEnvironmentProvider"/> class.
        /// </summary>
        /// <param name="useExistingEnvironmentValues"><c>true</c> (default) to have all existing environment properties set from the current system, otherwise <c>false</c>.</param>
        /// <param name="addExistingEnvironmentVariables"><c>true</c> to have all existing environment variables added, otherwise <c>false</c> (default).</param>
        public MockEnvironmentProvider(bool useExistingEnvironmentValues = true, bool addExistingEnvironmentVariables = false)
            : base(addExistingEnvironmentVariables)
        {
            if (useExistingEnvironmentValues)
            {
                CommandLine = Environment.CommandLine;
#if NET9_0_OR_GREATER
                CpuUsage = Environment.CpuUsage;
#endif
                CurrentDirectory = Environment.CurrentDirectory;
                CurrentManagedThreadId = Environment.CurrentManagedThreadId;
                ExitCode = Environment.ExitCode;
                HasShutdownStarted = Environment.HasShutdownStarted;
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem;
                Is64BitProcess = Environment.Is64BitProcess;
#if NET9_0_OR_GREATER
                IsPrivilegedProcess = Environment.IsPrivilegedProcess;
#endif
                MachineName = Environment.MachineName;
                NewLine = Environment.NewLine;
                OSVersion = Environment.OSVersion;
#if NET5_0_OR_GREATER
                ProcessId = Environment.ProcessId;
#endif
                ProcessorCount = Environment.ProcessorCount;
#if NET6_0_OR_GREATER
                ProcessPath = Environment.ProcessPath;
#endif
                StackTrace = Environment.StackTrace;
                SystemDirectory = Environment.SystemDirectory;
                SystemPageSize = Environment.SystemPageSize;
                TickCount = Environment.TickCount;
#if NETCOREAPP3_1_OR_GREATER
                TickCount64 = Environment.TickCount64;
#endif
                UserDomainName = Environment.UserDomainName;
                UserInteractive = Environment.UserInteractive;
                UserName = Environment.UserName;
                Version = Environment.Version;
                WorkingSet = Environment.WorkingSet;

                _commandLineArgs = Environment.GetCommandLineArgs();
                _logicalDrives = Environment.GetLogicalDrives();

                foreach (Environment.SpecialFolder folder in Enum.GetValues(typeof(Environment.SpecialFolder)).Cast<Environment.SpecialFolder>())
                {
                    _specialFolders[folder] = Environment.GetFolderPath(folder);
                }
            }
            else
            {
                CommandLine = string.Empty;
                CurrentDirectory = string.Empty;
                MachineName = string.Empty;
                NewLine = string.Empty;
                OSVersion = NullOperatingSystem;
                StackTrace = string.Empty;
                SystemDirectory = string.Empty;
                UserDomainName = string.Empty;
                UserName = string.Empty;
                Version = Version1dot0;
            }
        }

        /// <inheritdoc />
        public string CommandLine { get; set; }

#if NET9_0_OR_GREATER
        /// <inheritdoc />
        public Environment.ProcessCpuUsage CpuUsage { get; set; }
#endif

        /// <inheritdoc />
        public string CurrentDirectory { get; set; }

        /// <inheritdoc />
        public int CurrentManagedThreadId { get; set; }

        /// <inheritdoc />
        public int ExitCode { get; set; }

        /// <inheritdoc />
        public bool HasShutdownStarted { get; set; }

        /// <inheritdoc />
        public bool Is64BitOperatingSystem { get; set; }

        /// <inheritdoc />
        public bool Is64BitProcess { get; set; }

#if NET9_0_OR_GREATER
        /// <inheritdoc />
        public bool IsPrivilegedProcess { get; }
#endif

        /// <inheritdoc />
        public string MachineName { get; set; }

        /// <inheritdoc />
        public string NewLine { get; set; }

        /// <inheritdoc />
        public OperatingSystem OSVersion { get; set; }

#if NET5_0_OR_GREATER
        /// <inheritdoc />
        public int ProcessId { get; set; }
#endif

        /// <inheritdoc />
        public int ProcessorCount { get; set; }

#if NET6_0_OR_GREATER
        /// <inheritdoc />
        public string? ProcessPath { get; set; }
#endif

        /// <inheritdoc />
        public string StackTrace { get; set; }

        /// <inheritdoc />
        public string SystemDirectory { get; set; }

        /// <inheritdoc />
        public int SystemPageSize { get; set; }

        /// <inheritdoc />
        public int TickCount { get; set; }

#if NETCOREAPP3_1_OR_GREATER
        /// <inheritdoc />
        public long TickCount64 { get; set; }
#endif

        /// <inheritdoc />
        public string UserDomainName { get; set; }

        /// <inheritdoc />
        public bool UserInteractive { get; set; }

        /// <inheritdoc />
        public string UserName { get; set; }

        /// <inheritdoc />
        public Version Version { get; set; }

        /// <inheritdoc />
        public long WorkingSet { get; set; }

        /// <inheritdoc />
        public void Exit(int exitCode) => ExitCode = exitCode;

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">The <see cref="MockEnvironmentProvider" /> does not currently support calling the <see cref="FailFast(string?)" /> method.</exception>
        public void FailFast(string? message) => throw new NotSupportedException($"The {nameof(MockEnvironmentProvider)} does not currently support calling the {nameof(FailFast)} method.");

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">The <see cref="MockEnvironmentProvider" /> does not currently support calling the <see cref="FailFast(string?, Exception?)" /> method.</exception>
        public void FailFast(string? message, Exception? exception) => throw new NotSupportedException($"The {nameof(MockEnvironmentProvider)} does not currently support calling the {nameof(FailFast)} method.");

        /// <inheritdoc />
        public string[] GetCommandLineArgs() => _commandLineArgs;

        /// <inheritdoc />
        public string GetFolderPath(Environment.SpecialFolder folder) => _specialFolders.TryGetValue(folder, out string? value) ? value : string.Empty;

        /// <inheritdoc />
        public string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option) => _specialFolders.TryGetValue(folder, out string? value) ? value : string.Empty;

        /// <inheritdoc />
        public string[] GetLogicalDrives() => _logicalDrives;

        /// <summary>
        /// Sets the command-line arguments returned by <see cref="GetCommandLineArgs()" />.
        /// </summary>
        /// <param name="args">The command-line arguments to set.</param>
        public void SetCommandLineArgs(string[] args) => _commandLineArgs = args;

        /// <summary>
        /// Sets a folder path returned by <see cref="GetFolderPath(Environment.SpecialFolder)" />.
        /// </summary>
        /// <param name="folder">The <see cref="Environment.SpecialFolder" /> to set the path of.</param>
        /// <param name="path">The path to set.</param>
        public void SetFolderPath(Environment.SpecialFolder folder, string path) => _specialFolders[folder] = path;

        /// <summary>
        /// Sets the logical drives returned by <see cref="GetLogicalDrives()" />.
        /// </summary>
        /// <param name="drives">The drives to set.</param>
        public void SetLogicalDrives(string[] drives) => _logicalDrives = drives;
    }
}