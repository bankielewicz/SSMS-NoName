# NoName

NoName (a working name) is a planned SQL Server Management Studio (SSMS) 22 extension suite for one SQL Server instance: tools built on the instance's Query Store, Extended Events, error log, PerfMon counters and DMVs, with optional history kept on the instance, around a prompt builder that turns SQL Server context into an AI prompt, with identities, secrets, infrastructure names and selected data redacted locally first. It is for DBAs and developers whose organizations don't let AI tools connect to databases directly. It is meant to work whether or not SSMS's Copilot/AI component is installed.

Unreleased. Today the repository holds only **NoName Diagnostics**, a test-bench extension that will probe which SSMS APIs the suite can use and log every call, result, error and click. Microsoft doesn't support third-party SSMS extensions, so the probes are meant to be rerun after every SSMS update.

## Prerequisites

- Windows x64 with SSMS 22. The extension targets SSMS 22 only (`Microsoft.VisualStudio.Ssms` [22.0,23.0), amd64) and was last run on 22.11.3.
- To build the extension: Visual Studio 2026 with the Visual Studio extension development workload, which provides MSBuild and the VS SDK.
- To run the Core library's tests: the .NET 8 SDK.

## Quick start

1. Run the Core library's tests from the repository root. On Windows:

   ```bash
   dotnet test tests/NoName.Diagnostics.Core.Tests/NoName.Diagnostics.Core.Tests.csproj
   ```

   From WSL, use the wrapper, which keeps NuGet and CLI state in the repository's ignored `.cache/` folder:

   ```bash
   scripts/wsl-dotnet.sh test tests/NoName.Diagnostics.Core.Tests/NoName.Diagnostics.Core.Tests.csproj
   ```

2. Build the extension with Visual Studio 2026's MSBuild, from a Developer Command Prompt in the repository root:

   ```bat
   MSBuild.exe src\NoName.Diagnostics.Vsix\NoName.Diagnostics.Vsix.csproj -restore -p:Configuration=Debug
   ```

   The package is written to `src\NoName.Diagnostics.Vsix\bin.win\Debug\NoName.Diagnostics.vsix`. Windows builds use `obj.win\` and `bin.win\` so they don't collide with WSL builds.

3. Close SSMS, then install the package with SSMS 22's own installer, so it can't land in another SSMS or Visual Studio:

   ```bat
   "<SSMS 22 folder>\Release\Common7\IDE\VSIXInstaller.exe" /instanceIds:<instance-id> src\NoName.Diagnostics.Vsix\bin.win\Debug\NoName.Diagnostics.vsix
   ```

   `<SSMS 22 folder>` is where SSMS 22 is installed, and `<instance-id>` is your SSMS 22 instance ID, which `vswhere.exe` reports. To remove the extension, run the same installer with `/instanceIds:<instance-id> /uninstall:NoName.Diagnostics.6B1E5C8A-3F7D-4E2B-9A61-0C4D2E8F7B13`.

4. Start SSMS 22 and choose **Tools > NoName Diagnostics**, or right-click in a query window. Each SSMS session writes one log file, `%LOCALAPPDATA%\NoName\Diagnostics\logs\session-*.jsonl`, with a line per command click.

## Status

| Part | State |
| --- | --- |
| `NoName.Diagnostics.Core` (session log, sanitizer, probe runner; netstandard2.0) | Implemented, with unit tests |
| `NoName.Diagnostics.Vsix` 0.1.0 | Loads in SSMS 22.11.3 in the background and logs clicks on its three commands; no probes yet |
| Diagnostic probes (OS, SQL, editor, Object Explorer, results grid) | Specified in the feasibility spike; not built |
| The suite: prompt builder and single-instance DBA tools | Brainstormed (BRN-001 version 2); the design, PRD and architecture are next |

## Documentation

- [BRN-001, the suite brainstorm](docs/specs/brainstorm/BRN-001.md): the problems, the ideas with their dispositions, and the assumptions each tool depends on.
- [IDEA-NN-001, the first brainstorm](docs/specs/noname/brainstorming.md): owner inputs, evidence about SSMS 22, constraints and open questions.
- [RES-NN-001, the feasibility spike](docs/specs/noname/feasibility-spike.md): the diagnostic extension's research questions, probe catalog and log-privacy rules.
