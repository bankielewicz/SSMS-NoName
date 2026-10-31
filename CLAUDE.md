# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

NoName is a slug, not the product name (that's still open, Q-NN-001). It is an SSMS 22 extension suite for DBAs working on **one SQL Server instance**: tools built on the instance's own data (Query Store, Extended Events, the SQL error log, PerfMon counters, DMVs), with an optional history database on the instance filled by Agent jobs, around a hub that builds AI prompts locally with per-user redaction of identities, secrets, infrastructure names and selected data. Users copy the prompt into whatever AI their organization allows. It must work whether or not SSMS's Copilot/AI component is installed, stay light on the user's PC (no lag in SSMS), and look polished. An MCP channel is optional and comes later. It is for the owner first, then an open-source release.

The code today is the **diagnostic extension** (NoName Diagnostics). It probes the OS, SQL and GUI APIs the product needs inside SSMS and logs every call, result, error and user action. It doubles as an SSMS-update canary: rerun it after each SSMS update to see what broke. The product itself isn't built yet.

Read before changing direction or scope:
- `docs/specs/brainstorm/BRN-001.md`: the current brainstorm (version 2, converged). Its version 2 records the owner's single-instance redirection in his words; its promoted ideas are what the design and the PRD build on. Every capability is classed A (proven in SSMS), B (waits on a named diagnostic probe) or C (outside SSMS).
- `docs/specs/noname/brainstorming.md` (IDEA-NN-001): confirmed owner inputs (`INPUT-NN-*`), evidence, constraints (`C-*`) and open questions (`Q-NN-*`).
- `docs/specs/noname/feasibility-spike.md` (RES-NN-001): research questions, probe catalog (`PRB-*`), install/run procedure and log-privacy rules.

## Where the planning stands (as of 2026-10-31)

Check each item against the documents' versions and `gh pr list` before relying on it, and update this section when a planning step completes.

- Done: BRN-001 version 2. The first design canvas ("SQL Server Health Dashboards") was judged too much of an enterprise monitoring platform; the owner asked for a new single-instance canvas, iterated in Claude Design until it is impressive.
- Next: `/devforgeai:ui BRN-001` (a create run: it proposes flows and briefs for the owner to confirm, then draws the new canvas and records it as DSN-001), then `/devforgeai:prd BRN-001`.
- In parallel: the diagnostic probes (`PRB-*` in RES-NN-001 and the class B probes in BRN-001), which decide which tools are buildable. Only P1 is proven: the VSIX loads and its menu commands fire.
- Open: GitHub issue #4 (the sanitizer logs connection-string key names verbatim). Not yet done: recording the owner's newer inputs as `INPUT-NN-015` onward in IDEA-NN-001; BRN-001 quotes them meanwhile.
- Research for the design lives outside the repository: third-party repositories in the git-ignored `tmp/` are read for ideas only, never copied (two of them aren't MIT: `local-llm-chat-ssms` is GPL-2.0 and SqlPulse has no licence).

## Workflow: DevForgeAI, TDD, then QA with /code-review

This project follows **bankielewicz/DevForgeAI**, the `devforgeai@devforgeai` plugin (0.31.x), which the owner develops. `devforgeai@devforgeai-local` (bankielewicz/Specs) is installed only for comparison. Its skills share the `devforgeai:` prefix, so for this project use only the skills named below.

- **Planning chain:** `devforgeai:brainstorm` → `ui` → `prd` → `architecture` → `context` → `epic`. They write:
  - `docs/specs/brainstorm/BRN-NNN.md`
  - `docs/specs/design/DSN-NNN.md`, where `ui` proposes the flows and a brief for each, draws the canvas in Claude Design through the Artifact tool once the owner confirms them, and records the boards the owner iterated on; that's the mockup step in INPUT-NN-014
  - `docs/specs/prd/PRD-NNN.md`
  - `docs/specs/arch/ARCH-NNN.md`, with ADRs in `docs/specs/adr/`
  - `docs/specs/context/`
  - `docs/specs/epic/EPIC-NNN.md`
- **Check before deciding:** before proposing or changing behaviour, run `devforgeai:spec-lookup` to cite what's already decided. Anything it doesn't find is the owner's decision to ask about.
- **Earlier documents:** `docs/specs/noname/brainstorming.md` (IDEA-NN-001) and `feasibility-spike.md` (RES-NN-001) were written with the Specs templates before the switch. They feed the DevForgeAI chain and aren't replaced. When either changes, bump `document_version`, add a superseding entity to their register, and validate:
  ```sh
  python3 "$(ls -d ~/.claude/plugins/cache/devforgeai-local/devforgeai/*/skills/workflow/scripts/validate_templates.py | head -1)" --provenance docs/specs/noname/provenance.json
  ```
- **TDD** (DevForgeAI 0.31 has no implement skill yet, so this is the convention):
  1. Write the test first, against a stub that compiles and throws `NotImplementedException`.
  2. The red run must fail on behaviour, not on build or setup errors.
  3. Implement until it's green.

  Tests added for code that already exists are characterization tests and pass first time; say so in the commit message.
- **QA and merging:**
  - The working session never labels its own pull request.
  - An independent QA session, or the owner, runs `/code-review` on the pull request (`/code-review <PR#>`) and records the verdict as a PR comment plus the `merge-approved` or `qa-failed` label. A `qa-failed` verdict sends the PR back; fix its findings test-first.
  - `devforgeai:git` commits, pushes and opens the pull request. It merges only when the label is `merge-approved`, CI is green and the owner confirms.
- **Other skills:** `devforgeai:documents-updater` refreshes the README and CHANGELOG; run `devforgeai:precompact` before `/compact`.
- **The VSIX can't be unit-tested.** Its evidence is a diagnostic run inside SSMS 22, read back from the logs.

## Commands

Run from the repo root. `scripts/wsl-dotnet.sh` wraps `dotnet` so NuGet and CLI state stay in the repo's ignored `.cache/` and `$TMPDIR` rather than `$HOME`.

```sh
# Core library tests (net8.0 test project, netstandard2.0 library)
scripts/wsl-dotnet.sh test tests/NoName.Diagnostics.Core.Tests/NoName.Diagnostics.Core.Tests.csproj

# One test, or one class
scripts/wsl-dotnet.sh test tests/NoName.Diagnostics.Core.Tests/NoName.Diagnostics.Core.Tests.csproj --filter "FullyQualifiedName~ProbeRunnerTests.Probe_that_hangs_times_out"

# Coverage plus the same 95% line-coverage gate CI uses (pick a new folder each run; don't rm first)
scripts/wsl-dotnet.sh test tests/NoName.Diagnostics.Core.Tests/NoName.Diagnostics.Core.Tests.csproj --settings tests/coverage.runsettings --collect "XPlat Code Coverage" --results-directory .cache/coverage-run-3
python3 scripts/coverage_gate.py .cache/coverage-run-3 --minimum 95

# The coverage gate's own tests (stdlib unittest, run first in CI)
python3 -m unittest discover -s tests/scripts
```

The commands above run net8.0 only. Any run with Windows dotnet (the CI Windows job, or `dotnet.exe` from WSL) runs net8.0 and net48, because the test project adds net48 only when `$(OS)` is Windows. From WSL, restore from the repo's cache so no packages land in the Windows profile:

```sh
"/mnt/c/Program Files/dotnet/dotnet.exe" test '\\wsl.localhost\Ubuntu\home\bryan\Projects\NoName\tests\NoName.Diagnostics.Core.Tests\NoName.Diagnostics.Core.Tests.csproj' -f net8.0 '-p:RestorePackagesPath=\\wsl.localhost\Ubuntu\home\bryan\Projects\NoName\.cache\nuget\packages' -- RunConfiguration.TreatNoTestsAsError=true
```

.NET Framework won't load assemblies from a UNC path, so the net48 test host finds no tests there: `dotnet test` prints "No test is available" and still exits 0 unless `RunConfiguration.TreatNoTestsAsError=true` is set (CI sets it). To run net48 from WSL, build it with `-f net48`, copy `tests/NoName.Diagnostics.Core.Tests/bin.win/Debug/net48/` to a folder on a local Windows drive, and run `dotnet.exe test <that folder>\NoName.Diagnostics.Core.Tests.dll -- RunConfiguration.TreatNoTestsAsError=true`.

The VSIX builds only with Windows MSBuild (Visual Studio 2026 Community, which has the extension workload). It runs fine from WSL against the UNC path, and restores NuGet packages on the Windows side:

```sh
"/mnt/c/Program Files/Microsoft Visual Studio/18/Community/MSBuild/Current/Bin/MSBuild.exe" '\\wsl.localhost\Ubuntu\home\bryan\Projects\NoName\src\NoName.Diagnostics.Vsix\NoName.Diagnostics.Vsix.csproj' -restore -p:Configuration=Debug -nologo -v:minimal
```

The output is `src/NoName.Diagnostics.Vsix/bin.win/Debug/NoName.Diagnostics.vsix`. To install it, SSMS must be closed; always use SSMS 22's own installer and its instance ID, so the package can't land in SSMS 21 or Visual Studio 2026:

```sh
"/mnt/f/Program Files/Microsoft SQL Server Management Studio 22/Release/Common7/IDE/VSIXInstaller.exe" /quiet /instanceIds:dd3b7a98 '\\wsl.localhost\Ubuntu\home\bryan\Projects\NoName\src\NoName.Diagnostics.Vsix\bin.win\Debug\NoName.Diagnostics.vsix'
# uninstall: same installer, /instanceIds:dd3b7a98 /uninstall:NoName.Diagnostics.6B1E5C8A-3F7D-4E2B-9A61-0C4D2E8F7B13
```

The installer log is the newest `/mnt/c/Users/bryan/AppData/Local/Temp/dd_VSIXInstaller_*.log`. Diagnostic session logs are in `/mnt/c/Users/bryan/AppData/Local/NoName/Diagnostics/logs/*.jsonl`, and SSMS's load errors are in `/mnt/c/Users/bryan/AppData/Roaming/Microsoft/SSMS/22.0_dd3b7a98/ActivityLog.xml` when the owner starts SSMS with `/log`.

## Architecture

**`src/NoName.Diagnostics.Core`** (netstandard2.0, no SSMS references, `TreatWarningsAsErrors`):
- `DiagnosticLog` writes one JSON Lines file per session, flushed per line so a host crash keeps everything before it. It writes a `session/start` and `session/end` line, prunes to the newest sessions, and its `LineWritten` event feeds live viewers.
- `Sanitizer` is the privacy boundary. By default values are logged as their shape (length and a SHA-256 prefix); verbose mode logs values. Connection-string secrets are never logged, verbose or not.
- `ProbeRunner` runs an `IProbe`; each API call goes through `ProbeContext.Step`/`StepAsync`, which records the call and outcome, then lets the probe continue. Exceptions, hangs (time limit) and cancellation become `ProbeStatus` results. `RunAllAsync` logs a `run.summary` line with each probe's status (the canary view).
- `Json` is a dependency-free writer, because SSMS loads its own System.Text.Json.

**`src/NoName.Diagnostics.Vsix`** is a classic VSSDK project (.NET Framework 4.8, `Microsoft.VisualStudio.SDK` 17.14.40265, `Microsoft.VSSDK.BuildTools` 17.14.2120). This is the same setup as the shipping SSMS 22 extension StatisticsParser (MIT), which the spike used as a reference.
- The manifest targets `Microsoft.VisualStudio.Ssms` [22.0,23.0) on amd64, installed per user.
- The package loads in the background on `ShellInitialized`. Loading only opens the log and records the host; probes run on explicit clicks.
- Commands sit in the Tools menu and in SSMS's query-window context menu (`{33F13AC3-80BB-4ECB-85BC-225435603A5E}`:`0x0050`, plus `IDM_VS_CTXT_CODEWIN`). `PackageIds.cs` must match the symbols in `VSCommandTable.vsct`.
- **Late binding is a rule.** Every SSMS or Copilot type (SqlWorkbench.Interfaces, SQLEditors, GridControl, the brokered `SqlEditor.BrokeredContracts`) is loaded by name at runtime, never as a compile-time reference. The same VSIX then runs with or without the AI component (C-08) and logs "type not found" instead of failing to load. These internal APIs change between SSMS 22 updates; 22.6 changed the brokered methods' signatures.
- SSMS redirects every `Microsoft.SqlServer.TransactSql.ScriptDom` version up to 18.0 to its bundled 18.0.115, so in-process parsing always uses SSMS's copy.

**Build plumbing:** `Directory.Build.props` gives Windows builds `obj.win/` and `bin.win/` so WSL `dotnet` and Windows MSBuild don't overwrite each other's restore output. It excludes both OS's output folders from compile items, and disables SourceLink.

**CI** (`.github/workflows/ci.yml`) tests and measures only the Core library and the coverage gate, on Ubuntu with .NET 8, against GitHub's merge commit for pull requests. `scripts/coverage_gate.py` enforces 95% line coverage from coverlet's exact line counts. A second job, "Core tests (Windows)", runs the same tests on windows-latest without coverage, on both net8.0 and net48 (SSMS loads the library on .NET Framework 4.8), so Windows-only paths (such as `Prune`'s file-in-use handling) run in CI; it isn't a required check. Linux and WSL run net8.0 only. There's no coverage upload: GitHub Code Quality isn't available for this personal-account repository. Actions are pinned to full commit SHAs.

## Repository rules

- GitHub `bankielewicz/SSMS-NoName` is public. Its `main` ruleset has no bypass for anyone: pull requests only, no force-push or deletion, and the CI check "Core tests and coverage" must pass. Its code-quality and 95% coverage rules receive no data while Code Quality is unavailable, so the CI check is what enforces coverage. Never weaken a gate to get a pull request through: no coverage exclusions, no skips that hide a test that didn't run, no mocks added only to hit lines, no lowered minimum. A test the environment can't exercise (for example one that needs a non-root user, run as root) is reported as skipped with its reason, never as passed. Branch from `origin/main` and open a pull request; never plan a force-push to `main`.
- Commit as `Bryan Ankielewicz <121247296+bankielewicz@users.noreply.github.com>`, which is set in the repo-local git config. The machine's global identity ("DevForgeAI CI/CD Engineer") is wrong here, and other addresses may be credited to the wrong GitHub account. Check `git config user.email` before committing.
- `Specs-main/` is a checkout of the DevForgeAI framework (`bankielewicz/Specs`) and is git-ignored. Framework changes belong there, never in this repo. `github.com/bankielewicz/SSMS` holds an earlier Codex attempt that the owner rejected; treat it as background reading only.
- When Claude Code's sandbox is on, it mounts placeholder dotfiles at the repo root (`.bashrc`, `.gitmodules`, `.mcp.json` and others), and `.git/config` is read-only. Stage explicit paths, never `git add -A`. Use absolute paths instead of `cd` into subfolders.
- The Write tool turns `\u2028`/`\u2029` escapes into raw characters, which breaks C# string literals. Write them as `(char)0x2028`.
