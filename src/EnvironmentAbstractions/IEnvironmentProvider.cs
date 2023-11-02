// Copyright (c) Jeff Kluge.
//
// Licensed under the MIT license.

namespace System
{
    /// <summary>
    /// Provides information about, and means to manipulate, the current environment and platform.
    /// </summary>
    public interface IEnvironmentProvider : IEnvironmentVariableProvider
    {
        /// <inheritdoc cref="Environment.CommandLine" />
        string CommandLine { get; }

        /// <inheritdoc cref="Environment.CurrentDirectory" />
        string CurrentDirectory { get; set; }

        /// <inheritdoc cref="Environment.CurrentManagedThreadId" />
        int CurrentManagedThreadId { get; }

        /// <inheritdoc cref="Environment.ExitCode" />
        int ExitCode { get; }

        /// <inheritdoc cref="Environment.HasShutdownStarted" />
        bool HasShutdownStarted { get; }

        /// <inheritdoc cref="Environment.Is64BitOperatingSystem" />
        bool Is64BitOperatingSystem { get; }

        /// <inheritdoc cref="Environment.Is64BitProcess" />
        bool Is64BitProcess { get; }

        /// <inheritdoc cref="Environment.MachineName" />
        string MachineName { get; }

        /// <inheritdoc cref="Environment.NewLine" />
        string NewLine { get; }

        /// <inheritdoc cref="Environment.OSVersion" />
        OperatingSystem OSVersion { get; }

#if NET5_0_OR_GREATER
        /// <inheritdoc cref="Environment.ProcessId" />
        int ProcessId { get; }
#endif

        /// <inheritdoc cref="Environment.ProcessorCount" />
        int ProcessorCount { get; }

#if NET6_0_OR_GREATER
        /// <inheritdoc cref="Environment.ProcessPath" />
        string? ProcessPath { get; }
#endif

        /// <inheritdoc cref="Environment.StackTrace" />
        string StackTrace { get; }

        /// <inheritdoc cref="Environment.SystemDirectory" />
        string SystemDirectory { get; }

        /// <inheritdoc cref="Environment.SystemPageSize" />
        int SystemPageSize { get; }

        /// <inheritdoc cref="Environment.TickCount" />
        int TickCount { get; }

#if NETCOREAPP3_1_OR_GREATER
        /// <inheritdoc cref="Environment.TickCount64" />
        long TickCount64 { get; }
#endif

        /// <inheritdoc cref="Environment.UserDomainName" />
        string UserDomainName { get; }

        /// <inheritdoc cref="Environment.UserInteractive" />
        bool UserInteractive { get; }

        /// <inheritdoc cref="Environment.UserName" />
        string UserName { get; }

        /// <inheritdoc cref="Environment.Version" />
        Version Version { get; }

        /// <inheritdoc cref="Environment.WorkingSet" />
        long WorkingSet { get; }

        /// <inheritdoc cref="Environment.Exit(int)" />
        void Exit(int exitCode);

        /// <inheritdoc cref="Environment.FailFast(string?)" />
        void FailFast(string? message);

        /// <inheritdoc cref="Environment.FailFast(string?, Exception?)" />
        void FailFast(string? message, Exception? exception);

        /// <inheritdoc cref="Environment.GetCommandLineArgs" />
        string[] GetCommandLineArgs();

        /// <inheritdoc cref="Environment.GetFolderPath(Environment.SpecialFolder)" />
        string GetFolderPath(Environment.SpecialFolder folder);

        /// <inheritdoc cref="System.Environment.GetFolderPath(Environment.SpecialFolder, Environment.SpecialFolderOption)" />
        string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option);

        /// <inheritdoc cref="Environment.GetLogicalDrives" />
        string[] GetLogicalDrives();
    }
}