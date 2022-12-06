// Copyright (c) Jeff Kluge.
//
// Licensed under the MIT license.

using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// Represents an implementation of <see cref="IEnvironmentProvider" /> that manipulates environment variables on the current system.
    /// </summary>
    public sealed class SystemEnvironmentProvider : IEnvironmentProvider
    {
        /// <summary>
        /// Gets an <see cref="IEnvironmentProvider" /> that manipulates environment variables on the current system.
        /// </summary>
        public static IEnvironmentProvider Instance { get; } = new SystemEnvironmentProvider();

        /// <inheritdoc />
        public string CommandLine => Environment.CommandLine;

        /// <inheritdoc />
        public string CurrentDirectory
        {
            get => Environment.CurrentDirectory;
            set => Environment.CurrentDirectory = value;
        }

        /// <inheritdoc />
        public int CurrentManagedThreadId => Environment.CurrentManagedThreadId;

        /// <inheritdoc />
        public int ExitCode
        {
            get => Environment.ExitCode;
            set => Environment.ExitCode = value;
        }

        /// <inheritdoc />
        public bool HasShutdownStarted => Environment.HasShutdownStarted;

        /// <inheritdoc />
        public bool Is64BitOperatingSystem => Environment.Is64BitOperatingSystem;

        /// <inheritdoc />
        public bool Is64BitProcess => Environment.Is64BitProcess;

        /// <inheritdoc />
        public string MachineName => Environment.MachineName;

        /// <inheritdoc />
        public string NewLine => Environment.NewLine;

        /// <inheritdoc />
        public OperatingSystem OSVersion => Environment.OSVersion;

#if NET5_0_OR_GREATER
        /// <inheritdoc />
        public int ProcessId => Environment.ProcessId;
#endif

        /// <inheritdoc />
        public int ProcessorCount => Environment.ProcessorCount;

#if NET6_0_OR_GREATER
        /// <inheritdoc />
        public string? ProcessPath => Environment.ProcessPath;
#endif

        /// <inheritdoc />
        public string StackTrace => Environment.StackTrace;

        /// <inheritdoc />
        public string SystemDirectory => Environment.SystemDirectory;

        /// <inheritdoc />
        public int SystemPageSize => Environment.SystemPageSize;

        /// <inheritdoc />
        public int TickCount => Environment.TickCount;

#if NETCOREAPP3_1_OR_GREATER
        /// <inheritdoc />
        public long TickCount64 => Environment.TickCount64;
#endif

        /// <inheritdoc />
        public string UserDomainName => Environment.UserDomainName;

        /// <inheritdoc />
        public bool UserInteractive => Environment.UserInteractive;

        /// <inheritdoc />
        public string UserName => Environment.UserName;

        /// <inheritdoc />
        public Version Version => Environment.Version;

        /// <inheritdoc />
        public long WorkingSet => Environment.WorkingSet;

        /// <inheritdoc />
        public void Exit(int exitCode) => Environment.Exit(exitCode);

        /// <inheritdoc />
        public string? ExpandEnvironmentVariables(string name) => Environment.ExpandEnvironmentVariables(name);

        /// <inheritdoc />
        public void FailFast(string? message) => Environment.FailFast(message);

        /// <inheritdoc />
        public void FailFast(string? message, Exception? exception) => Environment.FailFast(message, exception);

        /// <inheritdoc />
        public string[] GetCommandLineArgs() => Environment.GetCommandLineArgs();

        /// <inheritdoc />
        public string? GetEnvironmentVariable(string name, EnvironmentVariableTarget target) => Environment.GetEnvironmentVariable(name, target);

        /// <inheritdoc />
        public string? GetEnvironmentVariable(string name) => Environment.GetEnvironmentVariable(name);

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> GetEnvironmentVariables() => new GetEnvironmentVariablesWrapper();

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target) => new GetEnvironmentVariablesWrapper(target);

        /// <inheritdoc />
        public string GetFolderPath(Environment.SpecialFolder folder) => Environment.GetFolderPath(folder);

        /// <inheritdoc />
        public string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option) => Environment.GetFolderPath(folder, option);

        /// <inheritdoc />
        public string[] GetLogicalDrives() => Environment.GetLogicalDrives();

        /// <inheritdoc />
        public void SetEnvironmentVariable(string name, string? value) => Environment.SetEnvironmentVariable(name, value);

        /// <inheritdoc />
        public void SetEnvironmentVariable(string name, string? value, EnvironmentVariableTarget target) => Environment.SetEnvironmentVariable(name, value, target);
    }
}