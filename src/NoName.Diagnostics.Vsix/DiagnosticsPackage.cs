using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using NoName.Diagnostics.Core;
using Task = System.Threading.Tasks.Task;

namespace NoName.Diagnostics.Vsix
{
    /// <summary>
    /// Loads in the background when the SSMS shell starts, so command and window events are logged
    /// from the beginning. Loading only opens the session log and records the host; every probe
    /// runs on an explicit click.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "0.1.0")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.ShellInitialized_string, PackageAutoLoadFlags.BackgroundLoad)]
    [Guid(PackageIds.PackageGuidString)]
    public sealed class DiagnosticsPackage : AsyncPackage
    {
        public const int KeepSessions = 20;

        public static DiagnosticsPackage Instance { get; private set; }

        public static string LogDirectory { get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NoName", "Diagnostics", "logs");

        public DiagnosticLog Log { get; private set; }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            Exception logFailure = null;
            try
            {
                DiagnosticLog.Prune(LogDirectory, KeepSessions - 1);
                Log = new DiagnosticLog(LogDirectory);
                Log.Write("host", "package.initialize", DescribeHost());
            }
            catch (Exception ex)
            {
                logFailure = ex;
            }

            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            if (logFailure != null)
                ActivityLog.LogError("NoName.Diagnostics", "Could not open the diagnostic log: " + logFailure);

            Instance = this;
            if (await GetServiceAsync(typeof(IMenuCommandService)) is OleMenuCommandService commands)
            {
                AddCommand(commands, PackageIds.OpenWindow, "OpenWindow");
                AddCommand(commands, PackageIds.ProbeWindow, "ProbeWindow");
                AddCommand(commands, PackageIds.CopyPrompt, "CopyPrompt");
            }
            else
            {
                Log?.Write("host", "commands.unavailable");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Log?.Dispose();
                Log = null;
            }
            base.Dispose(disposing);
        }

        private void AddCommand(OleMenuCommandService commands, int id, string name)
        {
            var command = new MenuCommand((s, e) => Log?.Write("ui", "command", new Dictionary<string, object> { ["command"] = name }),
                new CommandID(PackageIds.CommandSet, id));
            commands.AddCommand(command);
        }

        private static IDictionary<string, object> DescribeHost()
        {
            Process process = Process.GetCurrentProcess();
            string exe = process.MainModule?.FileName;
            return new Dictionary<string, object>
            {
                ["extensionVersion"] = typeof(DiagnosticsPackage).Assembly.GetName().Version.ToString(),
                ["processId"] = process.Id,
                ["exe"] = exe,
                ["exeFileVersion"] = exe == null ? null : FileVersionInfo.GetVersionInfo(exe).FileVersion,
                ["ideDirectory"] = AppDomain.CurrentDomain.BaseDirectory,
                ["clr"] = Environment.Version.ToString(),
                ["is64BitProcess"] = Environment.Is64BitProcess,
            };
        }
    }
}
