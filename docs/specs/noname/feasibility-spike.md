---
template_version: "2.0.0"
template_type: "research-brief"
template_source: "https://github.com/bankielewicz/Specs/blob/c36d38957cf8c90864dd02f18acd6e48f700ae12/docs/templates/discovery/research-brief.template.md"
template_revision: "c36d38957cf8c90864dd02f18acd6e48f700ae12"
provenance_register: "provenance.json"
provenance_links: ["LINK-NN-0031", "LINK-NN-0032", "LINK-NN-0033", "LINK-NN-0034", "LINK-NN-0035", "LINK-NN-0036", "LINK-NN-0037", "LINK-NN-0038", "LINK-NN-0039", "LINK-NN-0040"]
document_id: "RES-NN-001"
document_version: "0.1.0"
project_id: "NONAME"
status: "draft"
owner: "Bryan Ankielewicz (product owner)"
created: "2026-10-30"
updated: "2026-10-30"
source_of_truth: ["INPUT-NN-009", "INPUT-NN-010", "INPUT-NN-011", "INPUT-NN-012", "INPUT-NN-013", "INPUT-NN-014"]
related_documents: ["IDEA-NN-001"]
depends_on: ["IDEA-NN-001"]
supersedes: []
assumptions: ["ASM-NN-002", "ASM-NN-004"]
open_questions: ["Q-NN-011", "Q-NN-012", "Q-NN-013", "Q-NN-014"]
---

# Research brief: what an SSMS 22 extension can reach, found by a diagnostic extension

## Purpose and use

This brief runs EXP-NN-001 from the [brainstorm](brainstorming.md) through a diagnostic extension, NoName Diagnostics. The extension calls the operating-system, SQL and GUI APIs that NoName would need, and it logs every call, result, error and user action. The logs show what works, what fails, and what we didn't know to ask. The owner may keep the extension for future work (INPUT-NN-011). Planned probes below are not results; findings from running them are recorded only after a run.

## Inputs

### Confirmed owner input

These were recorded on 2026-10-30. Speaker: Bryan Ankielewicz, product owner.

<a id="input-nn-010"></a>**INPUT-NN-010:** "Yes - build a throwaway." This answers Q-NN-010: a test extension may be installed into SSMS 22 on the owner's PC.

<a id="input-nn-011"></a>**INPUT-NN-011:** "before you do, create a diagnostic throwaway - might not be a throwaway as it may prove useful in future enhancements. what you consider a throwaway is to test various apis whether OS, SQL and GUI capabilities. The throwaway is to contain output logs for various tests ensuring proper API calls are made and errors are captured. what we don't know, we don't know and the logs will enlighten us where our ignorance is present. the logs are to capture clicks and so forth."

<a id="input-nn-012"></a>**INPUT-NN-012:** "for those without copilot installed, the extension will allow end users the ability to copy & paste prompts that are crafted by the extension (using prompting best practices, etc...)."

<a id="input-nn-013"></a>**INPUT-NN-013:** "git init in NoName first, so the code has history is perfectly fine." This answers Q-NN-007.

<a id="input-nn-014"></a>**INPUT-NN-014:** "we will use /design to provide mockup ideas to ensure what we produce looks amazing and draws people to want (no - need) to use this extension with its bells/whistles and features/functionality". This applies to the product's design stage. The diagnostic extension is a test bench with a plain interface.

### Sources

| Input | Version / date | Relevant claim | Access restrictions |
| --- | --- | --- | --- |
| [Brainstorm](brainstorming.md) | IDEA-NN-001 v0.2.0 | EXP-NN-001, E-11 to E-20, C-06 to C-08 | None |
| Local SSMS 22 install, read-only inspection | 22.11.12328.437 on F:, instance `dd3b7a98`, 2026-10-30 | Classic APIs are present outside the Copilot component: `IObjectExplorerService`, `GetSelectedNodes`, `ScriptFactory` and `CurrentlyActiveWndConnectionInfo` in `SqlWorkbench.Interfaces.dll`; `SQLResultsControl`, `SelectedCells` and `ScriptExecutionCompleted` in `SQLEditors.dll`; `SelectedCells`, `BlockOfCellsCollection` and `GetCellDataAsString` in `Microsoft.SqlServer.GridControl.dll` | Names only; nothing was called |
| SSMSAgent manifest | `Microsoft.SqlServer.Management.Copilot.SSMSAgent` 22.200.11.171 | Microsoft's Copilot extension is a VisualStudio.Extensibility extension installed for all users, targeting `Microsoft.VisualStudio.Community` 18.10 or later | A different extension model from ours |
| [SSMS Schema Folders manifest](https://github.com/nicholas-ross/SSMS-Schema-Folders) | v1.7.1, 2026-10-30 | Targets `Microsoft.VisualStudio.Ssms` [21.0,23.0) on amd64 | Third-party |
| [StatisticsParser](https://github.com/BrentOzarULTD/StatisticsParserExtension) (MIT) | v1.1.0, 2026-10-30 | Targets `Microsoft.VisualStudio.Ssms` [22.6,] on amd64 and arm64; uses .NET Framework 4.8, Visual Studio SDK 17.14.40265 and VSSDK build tools 17.14.2120; places commands in SSMS's query-window context menu `{33F13AC3-80BB-4ECB-85BC-225435603A5E}`:`0x0050`; loads the brokered contracts from SSMS's main folder by reflection; notes that SSMS 22.6 added an editor-moniker parameter to every results-service method | Third-party; read for patterns, no code copied |
| vswhere | 2026-10-30 | SSMS 22 instance ID `dd3b7a98`; SSMS 21 instance ID `d0065fac` | None |

## Decision scope

- **Decision and owner:** whether NoName can be built as an in-process SSMS 22 extension that works with and without the AI component (C-08), and which API route serves each feature. Owner: Bryan.
- **In scope:** operating-system facilities inside the SSMS process; SSMS and Visual Studio shell services; the SQL editor, connection, results grid, messages, plan and Object Explorer; ScriptDom; the clipboard; DPAPI; the brokered services; user actions visible through shell events.
- **Out of scope:**
  - redaction quality (EXP-NN-003)
  - the product's visual design, which belongs to the design stage (INPUT-NN-014)
  - ARM64, which is blocked on this PC (E-18)
  - SSMS 21
- **Deadline and effort budget:** not set (Q-NN-006).
- **Impact of unresolved uncertainty:** the PRD and design can't commit to features whose route is unknown.

## Questions and hypotheses

| Research question ID | Question | Current hypothesis | Evidence that would contradict it | Decision affected | Priority and rationale |
| --- | --- | --- | --- | --- | --- |
| RQ-01 | Does a VSIX targeting `Microsoft.VisualStudio.Ssms` [22.0,23.0) install for the current user with SSMS 22's `VSIXInstaller.exe` and load? | Yes; community extensions do this | Installer rejects the target, or package load fails in `ActivityLog.xml` | Whole extension route (OPT-04) | Highest; gates everything |
| RQ-02 | Which menus can host our commands: query-window context menu, results-grid context menu, Object Explorer context menu? | Query-window and Tools menus work; others unknown | Command never appears, or the command-bar dump lacks those menus | Where users trigger NoName | High |
| RQ-03 | Can the extension read the editor selection and caret? | Yes, through standard editor services | No active text view, or empty selection when text is selected | Core flow step 1 | High |
| RQ-04 | Can it read the current connection through the classic API and through the brokered service? | Both work with the AI component; the classic API works without it | Types missing or calls failing on either install | Context gathering | High |
| RQ-05 | Can it read results-grid data and the user's selected cells? | Data through either route; selected cells only through the internal grid control | Reflection path fails, or `SelectedCells` is empty when cells are selected | INPUT-NN-005 | Highest; least documented |
| RQ-06 | Can it read the actual execution plan XML and the Messages text? | Yes through the brokered service | Proxy unavailable, or the method signature changed | Plan and error prompts | High |
| RQ-07 | Can it read the Object Explorer selection? | Yes through `IObjectExplorerService.GetSelectedNodes` | Service unavailable or the call fails | Object prompts | Medium |
| RQ-08 | Can it parse T-SQL with SSMS's ScriptDom in-process? | Yes (E-14) | Load or parse error | Redaction engine in SSMS (ASM-NN-002) | High |
| RQ-09 | Do clipboard write/read and DPAPI round trips work in-process? | Yes | Exception, or read-back differs | Copy-and-paste prompts (INPUT-NN-012); mapping protection | High |
| RQ-10 | Which routes work without the AI component? | The classic APIs do; the brokered services are unknown (E-20) | Classic routes fail without the AI component | C-08 | Highest |
| RQ-11 | Which user actions can be seen through shell events, without keyboard or mouse hooks? | Commands, window and document changes, selection changes and clicks in our own window | Events don't fire or lack identifiers | Usage logging (INPUT-NN-011) | Medium |
| RQ-12 | What does the environment contain that we haven't asked about? | Unknown by definition | Not applicable; answered by inventories | Later questions | Medium |

## Method and sampling

| Activity | Method | Participants / sources | Inclusion and exclusion criteria | Sample size rationale | Bias and limits | Owner |
| --- | --- | --- | --- | --- | --- | --- |
| Static inspection | Read file names, strings, manifests and versions from WSL | The local SSMS 22 install | SSMS 22 only | One install is what's available | Names don't prove behaviour | Claude |
| Community source study | Read manifests and source on GitHub | Schema Folders, StatisticsParser | SSMS 21/22 extensions | The two extensions found that state SSMS 22 support | Their choices may not suit our features | Claude |
| Diagnostic run with the AI component | Install NoName Diagnostics, follow the script below | Owner's PC, SSMS 22.11.3 with the AI workload (E-19) | Test server and database only (Q-NN-013) | One run per SSMS build | One machine and build | Bryan runs; Claude analyses |
| Diagnostic run without the AI component | The same script | An SSMS 22 install without the AI workload (Q-NN-011) | As above | Needed for C-08 | Environment not yet chosen | Bryan runs; Claude analyses |

### Build design

- **`src/NoName.Diagnostics.Core`** (netstandard2.0): the session log (JSON Lines), value sanitizer, probe runner and result model. It has no SSMS dependency and is tested in WSL.
- **`src/NoName.Diagnostics.Vsix`** (.NET Framework 4.8; Visual Studio SDK 17.14.40265; VSSDK build tools 17.14.2120): an in-process package that loads in the background when the shell starts, plus a Tools-menu tool window and context-menu commands.
- **Late binding:** every SSMS and Copilot type is loaded by name at run time and is never a compile-time reference. The same VSIX then runs with or without the AI component, and logs "type not found" instead of failing to load.
- **Manifest:** `InstallationTarget Id="Microsoft.VisualStudio.Ssms" Version="[22.0,23.0)"` on amd64, per user (not `AllUsers`).
- **Build and install:** Windows MSBuild 18.11 builds against the `\\wsl.localhost` path, restoring NuGet packages on the Windows side. Install uses SSMS 22's own `VSIXInstaller.exe` with `/instanceIds:dd3b7a98`.
- **Safety:** every probe runs on a button click, inside a try/catch with a time limit. UI-thread work goes through the JoinableTaskFactory. Loading the package only records the environment and subscribes to events.
- **Proposal (recorded 2026-10-30):** keep the extension as an SSMS-update canary. Every log records the SSMS build and internal DLL versions, and each run ends with a one-line pass/broken summary per capability, so runs before and after an SSMS update can be compared. This follows a reviewer note that the brokered methods' signatures changed in SSMS 22.6.

### Probe catalog

| Probe ID | Area | What it calls | What it logs | Pass oracle |
| --- | --- | --- | --- | --- |
| PRB-OS-01 | OS | OS version, process architecture, CLR and .NET Framework release, culture, DPI | Values (not sensitive) | All fields read |
| PRB-OS-02 | OS | Clipboard write, read-back and restore on the UI thread | Lengths; whether it matched; whether the previous text was restored | Read-back equals written; previous clipboard text restored |
| PRB-OS-03 | OS | DPAPI `ProtectedData.Protect`/`Unprotect` for the current user | Lengths; whether it matched | Round trip equal |
| PRB-HOST-01 | Host | DTE version and edition, `SSMS.exe` file version, IDE folder | Values | Version read |
| PRB-HOST-02 | Host | Loaded assemblies | Name, version, whether loaded from the SSMS folder | List produced |
| PRB-HOST-03 | Host | Shell services: UI shell, text manager, selection monitor, brokered container, settings, output window, component model, activity log | Available or not, per service | Each lookup recorded |
| PRB-HOST-04 | Host | All command bars and their controls | Bar names; control captions and command IDs | Dump produced; shows which context menus exist (RQ-02) |
| PRB-HOST-05 | Host | AI component presence: `SSMSAgent` folder and manifest; loaded Copilot assemblies | Present or absent, with version | Recorded, to correlate with RQ-10 |
| PRB-ED-01 | Editor | Active document and window | Kind and language; window caption as a hash | Recorded |
| PRB-ED-02 | Editor | Active text view selection and caret | Selection length, line count and a SHA-256 prefix; the text only in verbose mode | Matches what the owner selected |
| PRB-SQL-01 | SQL | Classic API `CurrentlyActiveWndConnectionInfo` (type found by name) | Type path found; field names; server, database and login as hashes; auth type; password field present yes/no, never its value | Fields read on a connected window |
| PRB-SQL-02 | SQL | Brokered `GetCurrentConnectionAsync` | Property names; editor moniker; connection string with secrets removed and other values hashed | Details returned |
| PRB-SQL-03 | SQL | Read-only metadata query, behind its own button: product version, edition, principal counts by type, sensitivity-classification and masked-column counts | Counts and version values | Query returns, against a test server only |
| PRB-GRID-01 | Grid | Internal grid control by reflection: grids in the active results pane, row and column counts, `SelectedCells` blocks | Type chain; counts; selected cell coordinates; header names hashed; cell text only in verbose mode | Coordinates match what the owner selected |
| PRB-GRID-02 | Grid | Brokered `GetAvailablePanesAsync` and `GetGridResultsSegmentAsync` | Method signatures as found; returned shape | Data returned |
| PRB-MSG-01 | Messages | Brokered `GetMessagesTabSegmentAsync` | Total length; hash | Text returned |
| PRB-PLAN-01 | Plan | Brokered `GetQueryPlanXmlSegmentAsync` | Total length; root element names; hash | XML returned |
| PRB-OE-01 | Object Explorer | `IObjectExplorerService.GetSelectedNodes` | Node count; node types; names as hashes | Nodes returned |
| PRB-PARSE-01 | Parser | ScriptDom loaded by name; TSql170 parse of the selection, or of a built-in synthetic sample when nothing is selected | Assembly version; parse error count; counts of string and number literals, identifiers and comments | Parse completes |
| PRB-BRK-01 | Brokered | Proxies for `ISqlEditorServiceBrokered` and `IQueryEditorTabDataServiceBrokered` | Contracts DLL path and version; success, null or exception per service; AI component present | Recorded for both installs (RQ-10) |
| PRB-E2E-01 | Prompt | Selection → fixed sample template → clipboard (INPUT-NN-012) | Template ID; prompt length; clipboard verified | Prompt on the clipboard |

### Event capture

- **EVT-CMD:** every command run through menus, toolbars or shortcuts, captured before and after execution (`CommandEvents`). Logs the command GUID, ID and name.
- **EVT-WIN** and **EVT-DOC:** window activation, and document open, close and save. Captions and paths are logged as hashes.
- **EVT-SEL:** shell selection and UI context changes, logged as element IDs.
- **EVT-UI:** clicks in the diagnostic tool window.
- **No keyboard or mouse hooks.** No low-level hooks are installed and no keystroke content is captured.

## Procedure and measurement

1. **Preparation:** build with Windows MSBuild. The owner closes SSMS. Then install:

   ```text
   "F:\Program Files\Microsoft SQL Server Management Studio 22\Release\Common7\IDE\VSIXInstaller.exe" /instanceIds:dd3b7a98 NoName.Diagnostics.vsix
   ```
2. **Tasks:**
   1. Start SSMS with `/log`, then open **Tools > NoName Diagnostics**.
   2. Select **Run all probes** before connecting.
   3. Connect to a test server, open a query window, and select part of a query. Run all probes again.
   4. Run the query with the actual plan included. Select a cell, then a column header, and run the grid probes.
   5. Select an Object Explorer node and run the Object Explorer probe.
   6. Right-click in the query window and use the NoName command.
   7. Close SSMS.
3. **Measurement:** each probe records a status (`pass`, `fail`, `error`, `timeout`, `skipped` or `not_applicable`), its duration in milliseconds, each API step with its outcome, and any exception's type, message and HRESULT.
4. **Analysis:** Claude reads `%LOCALAPPDATA%\NoName\Diagnostics\logs\*.jsonl` and `%LOCALAPPDATA%\Microsoft\SSMS\22.0_dd3b7a98\ActivityLog.xml` from WSL, then answers each RQ as yes, no or partial in Findings.
5. **Decision rule:**
   - If RQ-01 or RQ-03 fails, stop and reconsider OPT-03.
   - If selected cells can't be read (RQ-05), column and cell picking moves into the preview window.
   - Each feature needs a route that works without the AI component (RQ-10); otherwise it is offered only when the component is present.
6. **Stop condition:** if SSMS crashes or hangs, uninstall right away:

   ```text
   VSIXInstaller.exe /instanceIds:dd3b7a98 /uninstall:NoName.Diagnostics.6B1E5C8A-3F7D-4E2B-9A61-0C4D2E8F7B13
   ```

   If that isn't enough, start SSMS with `/safemode`.

## Information handling

| Information collected | Purpose | Permission / consent basis | Storage and access | Retention / deletion | Safe reporting form |
| --- | --- | --- | --- | --- | --- |
| Environment facts (versions, architecture, services, command bars) | Answer RQ-01, RQ-02 and RQ-12 | INPUT-NN-011 | Owner's PC, `%LOCALAPPDATA%\NoName\Diagnostics\logs` | The extension keeps the last 20 session files; the owner deletes the rest | Shared as logged |
| Query text, selections, grid values, plan and messages | Prove each route works | INPUT-NN-011 | As above | As above | Length and SHA-256 prefix by default. Values only in verbose mode, which the owner turns on per session (Q-NN-012) |
| Connection details (server, database, login) | RQ-04 | INPUT-NN-011 | As above | As above | Hashes; secrets are never logged, even in verbose mode |
| Window captions and file paths | Event context | INPUT-NN-011 | As above | As above | Hashes |
| Command events | RQ-11 | INPUT-NN-011 | As above | As above | Command IDs and names |

The tool window has a **Copy redacted log** button that copies a values-free version of the session. Review a log before sharing it.

## Schedule and dependencies

| Milestone | Dependency | Owner | Planned date | Actual date / status | Consequence if missed |
| --- | --- | --- | --- | --- | --- |
| This brief | INPUT-NN-010 to INPUT-NN-014 | Claude | 2026-10-30 | Drafted 2026-10-30 | The run has no script |
| Core library and tests | WSL .NET SDK | Claude | 2026-10-30 | `not_started` | No reliable logging |
| VSIX builds | Windows MSBuild and NuGet | Claude | 2026-10-30 | `not_started` | No installable package |
| Run with the AI component | SSMS closed for install; a test server (Q-NN-013) | Bryan | Not set (Q-NN-006) | `not_run` | RQ-01 to RQ-12 stay open |
| Run without the AI component | Q-NN-011 | Bryan | Not set | `not_run` | C-08 stays unproven |

## Findings and limitations

The diagnostic hasn't run, so every RQ is `not_run`. Static inspection supplied the names and versions listed in Inputs; those show what exists, not what works.

## Recommendation and traceability

| Decision | Recommendation | Supporting / conflicting findings | Requirement or assumption affected | Owner and actual decision | Follow-up |
| --- | --- | --- | --- | --- | --- |
| Extension model | In-process Visual Studio SDK package with late-bound access to SSMS types | Static inspection; StatisticsParser pattern | ASM-NN-002, C-08 | Bryan; pending | Confirm through RQ-01 and RQ-10 |
| Keep the diagnostic | Keep it as an update canary (proposal above) | Signature change in SSMS 22.6 (StatisticsParser comment) | ASM-NN-004 | Bryan; pending (Q-NN-014) | Rerun after each SSMS update |

**ASM-NN-004** (assumption): internal SSMS APIs change between SSMS 22 updates, so features built on them need a check on every update. Evidence: the StatisticsParser comment and E-02. If it's wrong, the canary costs little.

## Optional enterprise extension

`not_applicable`: this is a single-owner spike. Owner: Bryan.

## Provenance

| Subject record/document | Register link IDs | Origin or upstream record | Derivation / verification gaps |
| --- | --- | --- | --- |
| RES-NN-001 | LINK-NN-0031 | Research-brief template 2.0.0 at `c36d389` | The bundled copy's hash matched the template map |
| INPUT-NN-010 to INPUT-NN-014 | LINK-NN-0032 to LINK-NN-0036 | Owner statements, 2026-10-30 | Recorded from the conversation |
| Inputs and build design | LINK-NN-0037 to LINK-NN-0040 | IDEA-NN-001 v0.2.0; local inspection; community manifests; StatisticsParser source | Third-party repositories are pinned to the commits read on 2026-10-30 |

## Open questions

| ID | Question | Owner | Due | Blocking scope |
| --- | --- | --- | --- | --- |
| Q-NN-011 | Which SSMS 22 install without the AI component is used: a VM or Windows Sandbox with a fresh core-only install, or temporarily removing the AI workload? | Bryan | Before the second run | RQ-10 |
| Q-NN-012 | Is verbose mode (logging values, never secrets) off by default? Proposed: off, turned on per session | Bryan | Before the first run | Log content |
| Q-NN-013 | Which test SQL Server and database are used for the run? It should not be production | Bryan | Before the first run | PRB-SQL-03; the grid, plan and messages probes |
| Q-NN-014 | Is the diagnostic kept as a separate extension, or later folded into NoName as a diagnostics mode? | Bryan | Before PRD | Packaging |

## Exit criteria

- [ ] Each research question has evidence or an explicit unresolved status.
- [x] Methods, conditions, sample limitations, and contrary findings are recorded.
- [ ] Sensitive information has an agreed handling and retention plan. (Proposed above; agreement waits on Q-NN-012.)
- [ ] Recommendations identify their evidential limits and accountable decision owner.
- [ ] Affected assumptions, requirements, and dependent work have been updated or flagged.
