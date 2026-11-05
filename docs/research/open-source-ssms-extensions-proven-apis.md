# Open-source SSMS 21/22 extensions and the proven-API matrix

Research track 3 of 4 for NoName. Written 2026-11-04 by a Sonnet 5.5 research subagent at the owner's request. Evidence, not decisions. Third-party repositories were read for ideas and API names only; no code was copied.

## Scope and method

**Headline.** The gap list in the brief is mostly filled. SQLExtended (MIT, release 2026-11-01) ships an Execute intercept, Object Explorer locate, connected new-window open, results-grid reading and undoable edits, and its author records the Execute hook as confirmed in a running SSMS 22 on 2026-10-15. Several independent extensions (AxialSqlTools, MSSQLTOOL, SSMSPlus, ssms-helper, DBA Dash) use the same internal APIs. The true gaps are narrow: showing a plan or deadlock in SSMS's own viewer, Error List entries and squiggles, and any SSMS-minor-level compatibility record.

**What was read.**

| Source | How | Notes |
|---|---|---|
| Six local checkouts | Read in place | They are under `tmp/older/`, not `tmp/repos/`. Only `dbatools`, `sql-docs`, `sqlserver-kit` are in `tmp/repos/`; none was needed. |
| NoName's own VSIX and logs | Read in place | `src/NoName.Diagnostics.Vsix/`, two session logs under `%LOCALAPPDATA%\NoName\Diagnostics\logs\`. |
| Installed SSMS 22.11.12328.437 | Read-only listing and `strings` on shipped files | `Extensions\Microsoft\*`, the brokered-contracts DLL. First-party evidence. Nothing was copied. |
| 11 further public repositories | GitHub tarballs and API via `gh`, extracted under the session scratchpad, grep for API names | AxialSqlTools, MSSQLTOOL, ssms-helper, ssms-executor, SSMS-EnvTabs, Object Explorer Menu, Schema Folders, SSMSPlus, Flank, SQL Sage, Extension Manager, plus DBA Dash's extension folder. |
| Web searches | About 9 searches, plus the ErikEJ gallery page | Microsoft Learn page bodies could not be fetched by the fetch helper; Microsoft statements below come from search-result text and are marked **claimed**. |

**Conventions.** "Last confirmed" is the strongest SSMS version evidence in the source: a dated author statement, a release date, or a manifest range. A manifest range is not confirmation. **Claimed** means a statement I could not verify against code or an artifact. A rule applied throughout: an author's comment that something is "confirmed" is recorded as that author's claim, with its date.

**Licence rule.** MIT, Apache-2.0 and similar permissive code may be studied. GPL-2.0, GPL-3.0, AGPL-3.0 and unlicensed repositories (no licence means all rights reserved) give API names and ideas only. Ideas are reimplemented from scratch, never copied.

**Local-checkout gaps.** The SQLVersionToolsPublic and SqlPulse checkouts contain no source code, only documentation, scripts and screenshots. Everything about them is therefore claimed.

## Extension inventory

Ranges are the manifest `InstallationTarget` for `Microsoft.VisualStudio.Ssms` unless stated. All in-process VSSDK extensions run on .NET Framework (net472 or net48) because SSMS 22 is a .NET Framework 4.8 process. "SDK" is `Microsoft.VisualStudio.SDK`; "Build" is `Microsoft.VSSDK.BuildTools`.

| Extension | Author | Licence | SSMS range (manifest) | SDK / Build | .NET target | Last release | Source |
|---|---|---|---|---|---|---|---|
| StatisticsParser | Brent Ozar Unlimited | MIT | [22.6,] amd64 and arm64 | 17.14.40265 / 17.14.2120, plus Community.VisualStudio.Toolkit 17.0.533 | net48 (Core netstandard2.0) | v1.1.0, 2026-05-21 | local; github.com/BrentOzarULTD/StatisticsParserExtension |
| Performance Studio (SSMS VSIX) | Erik Darling, Darling Data | MIT | [21.0,23.0) amd64 and arm64 | 17.14.40265 / 17.14.2142 | net472 | v1.31.0, 2026-10-20 (nightly 2026-11-02) | local; github.com/erikdarlingdata/PerformanceStudio |
| SQLExtended | James Noonan (JamTheRadar) | MIT | [22.0,) amd64 | 17.14.40265 / 18.5.40034 | net48 | v2026.11.1.1915, 2026-11-01 | local; github.com/JamTheRadar/SQLExtended |
| AxialSqlTools | Alex Bochkov | Apache-2.0 | [22.0,) | 18.5.40034 build | net48 (also a net10.0 project) | 4.14, 2026-10-11 | github.com/Axial-SQL/AxialSqlTools |
| MSSQLTOOL | PuLuShen | Apache-2.0 | [22.0,23.0) | 18.5.40034 build | net472 | v5.407, 2026-11-04 | github.com/PuLuShen/MSSQLTOOL |
| ssms-helper | Emretor | MIT | [22.0,23.0) | 17.14.40265 / 18.5.40034 | net48 (Core netstandard2.0) | v0.13.27, 2026-10-29 | github.com/emretor/ssms-helper |
| SSMS Executor | Tom Willwerth (tkwj) | MIT | [21.0,23.0) | 17.14.40265 / 18.5.40034 | net472 | v3.1.41, 2026-10-10 | github.com/tkwj/ssms-executor |
| SSMS EnvTabs | Blake Becker | Apache-2.0 | [22.0,) | not captured | net472 | v2.3.1, 2026-10-19 | github.com/Blake-goofy/SSMS-EnvTabs |
| DBA Dash SSMS extension | DBA Dash (Trimble OSS) | MIT | [21.0,23.0) | 17.14.40265 / 18.5.40034 | net472 | DBA Dash 4.23.1, 2026-10-28 | github.com/trimble-oss/dba-dash |
| Flank SSMS extension | flank-project | Apache-2.0 | [22.0,) | 17.14.40265 / 18.9.820 | net472 (and net10.0 helpers) | pushed 2026-10-22 | github.com/flank-project/ssms-extension |
| Object Explorer Menu | Daniel Brink | **GPL-3.0** | [22.0,] | 17.14.40265 / 17.14.2094 | net472 | v4.1.2, 2026-03-31 | github.com/brink-daniel/ssms-object-explorer-menu |
| SSMS Schema Folders | Nicholas Ross | **GPL-2.0** | [21.0,23.0) and [15.0,21.0) | 15.0.1 / 17.12.2069 | net472 | v1.7.1-vsix, 2025-10-30 | github.com/nicholas-ross/SSMS-Schema-Folders |
| SSMSPlus (fork) | alint997 (upstream akarzazi, MishaT fork) | **none stated** | [21.0,) | not captured | net48 | v7.7, 2026-10-17 (upstream last release 6.0, 2024-07-13) | github.com/alint997/SSMSPlus |
| local-llm-chat-ssms | Markus Begerow | **GPL-2.0** | [22.0,23.0) amd64, all users | 17.0.32112.339 / 17.7.2196 | net472 | v1.0.0, 2025-11-18 | local |
| SqlPulse | Istvan Safar | **none**; repo has no source | README says SSMS 18 to 22 (claimed) | not public | net48 (README) | v0.1.305, 2026-08-22 | local; github.com/IstvanSafar/SqlPulse |
| SQL Sage | LUMASoftPL | **none**; README only | "made for SSMS 22" (claimed) | not public | not public | v1.0.0, 2026-10-20 | github.com/LUMASoftPL/sqlsage |
| SQLVersionTools (public) | SQLVersionTools Contributors | MIT (LICENSE.md, 2025) | SSMS 21 only (README) | not public | extension unknown; service .NET 8 | not stated | local; no source |
| SSMS Extension Manager | Blake-goofy | **AGPL-3.0** | standalone WPF app, not an extension | n/a | net10.0-windows | v1.0.2, 2026-10-04 | github.com/Blake-goofy/ssms-extension-manager |
| Right Way SQL Formatter | BadMonkeySoftware | **AGPL-3.0** | claimed SSMS 22 (gallery text) | not read | not read | repo pushed 2026-08-04 | github.com/BadMonkeySoftware/right-way-sql-formatter |
| Poor Man's T-SQL Formatter | Tao Klerks | **AGPL-3.0** | SSMS add-in last released for old SSMS | n/a | n/a | 1.6.16, 2019-08-04; last push 2023-07-14 | github.com/TaoK/PoorMansTSqlFormatter |
| SSMS Tools Pack | commercial vendor | closed | SSMS 22 status **not found** | n/a | n/a | n/a | listed as commercial in the ErikEJ gallery |
| Microsoft first-party (SQL Formatter, Presenter Mode, Schema Compare, CopilotUiTools) | Microsoft | closed, shipped in the SSMS install | [22.0,) | n/a | VSSDK plus VisualStudio.Extensibility hybrid | ships with 22.11.12328.437 | installed files |
| Microsoft Copilot SSMSAgent | Microsoft | closed | `Microsoft.VisualStudio.Community` [18.10,) (not the Ssms id) | n/a | net10.0, VisualStudio.Extensibility | 22.200.11.171 (installed) | installed files |

**Still works on SSMS 22?** Evidence is a release dated after SSMS 22 reached general availability (November 2025), plus either the author's statement or a manifest range. Exact SSMS minor versions are rarely stated.

| Extension | Evidence it works on 22.x |
|---|---|
| StatisticsParser | README and manifest require 22.6+; the brokered signature change in 22.6 is handled in code. Open issue 32 (2026-06-22) says the installer's SSMS 21 detection is confusing. |
| Performance Studio | Release 1.31.0 targets 21 and 22; README claims 18 to 22 (the 18 to 20 part is **claimed**, the manifest excludes it). |
| SQLExtended | Execute hook confirmed "in a running SSMS 22" on 2026-10-15 (author note, minor version not stated). Issues 5 to 8 closed 2026-10-23. |
| AxialSqlTools | Issue 21 (2025-11-13) asked for SSMS 22 support; closed; releases continue to 4.14. |
| MSSQLTOOL | Release notes cite SSMS 22 behaviour (client coordinates from `GetPointOfLineColumn`; Object Explorer root missing on "some SSMS 22 builds"). |
| SSMSPlus fork | README: version 7.7 tested on SSMS 22.10.1 (author claim). |
| Object Explorer Menu | README: tested on SSMS 22.11.3 (author claim). |
| Schema Folders | Changelog: v1.7.1 added 22 support (2025-10-30). README also carries a stale "last supported v1.6" sentence. |
| SqlPulse | README table lists 22.x as supported; no source to check (**claimed**). |
| local-llm-chat-ssms | Manifest targets 22 but with a 2023-era SDK (17.0); no evidence it was run on 22 (**claimed**). |
| Poor Man's / Right Way formatters | Not shown to run on 22. Microsoft now ships its own SQL Formatter, so the niche shrank. |

## What each extension touches

API names and patterns only. "Feature" is the user-visible job the API serves.

### NoName.Diagnostics.Vsix (our own, first-party proof)

| API or pattern | Feature | Status |
|---|---|---|
| `AsyncPackage`, `ProvideAutoLoad(ShellInitialized, BackgroundLoad)`, `ProvideMenuResource` | Background load at shell start; log opens and host is recorded | **Proven**, SSMS 22.11.12328.437, sessions 2026-10-30 and 2026-11-01 |
| `OleMenuCommandService`, command set from `.vsct` | Three commands (open window, probe window, copy prompt) | **Proven**: log lines show the commands firing on 2026-10-30 |
| Group parented to `{33F13AC3-80BB-4ECB-85BC-225435603A5E}` id `0x0050`, and to `IDM_VS_MENU_TOOLS` | Query-window context menu and Tools menu | **Proven** by the same log lines (the shell's `IDM_VS_CTXT_CODEWIN` parent is also declared; see the SQLExtended finding below that it never shows in a query window) |
| Host description: exe path, file version, CLR, 64-bit flag | Canary header | **Proven**: SSMS 22.11.12328.437, CLR 4.0.30319, 64-bit |

**Not yet proven.** The three commands only write a log line. They do not open a tool window, run a probe, read an editor, or touch any SSMS-internal type. `ProbeRunner` exists in the Core library and has never run inside SSMS. Everything in the matrix below that says A for NoName's own code is therefore a statement about other people's shipping code, not about NoName's.

### StatisticsParser (MIT, 22.6+)

| API or pattern | Feature | Recorded breakage or workaround |
|---|---|---|
| `SVsBrokeredServiceContainer` full-access broker; `ISqlEditorServiceBrokered.GetCurrentConnectionAsync` then `IQueryEditorTabDataServiceBrokered.GetMessagesTabSegmentAsync`, loaded by reflection from `...SqlEditor.BrokeredContracts.dll` in the IDE folder | Read the Messages pane text, paged in 64 KB segments | Signature changed in SSMS 22.6: every tab-data method gained a leading editor-moniker parameter. Code picks the overload by prefix-matching parameters so optional additions survive. The README says the Messages pane is "not an `IVsOutputWindow` pane". |
| `IVsMonitorSelection` current document frame, `IVsWindowFrame` `VSFPROPID_DocView`, then `SqlScriptEditorControl.TabPageHost` (property, with the private `tabPagesHost` field as fallback), reflection | Inject a "Parse Statistics" tab into the query window's results tab strip | The shell removes injected tabs when a query starts, so the tab is recreated after each run. A per-window supervisor held in a `ConditionalWeakTable`. |
| Event hooking by name pattern (Completed, Executed, Finished, Stopped, Done) on the doc view and `m_sqlResultsControl`, adapted with expression-tree delegates | Auto-refresh after a query finishes | A heuristic: the real event name is not known, so it hooks by pattern and logs which ones matched. |
| `.vsct`: `{33F13AC3-...}` id `0x0050` and `IDG_VS_TOOLS_EXT_TOOLS`, key binding Ctrl+K, Ctrl+G | Right-click and Tools entry | The Messages-pane right-click menu's id was "not statically discoverable" (PHASE8C notes); SQLExtended's `.vsct` supplies it: `0x0060`. |
| Unified Settings: `registration.json` plus `ISettingsReader` | Tools > Options page | The earlier `BaseOptionModel` settings store "never reached the read-sites" because it is a different store from the one Tools > Options binds to in SSMS 22. |
| `VSColorTheme.ThemeChanged`, `EnvironmentColors` brushes | Theme-following WPF grids | None |
| `IVsOutputWindow` pane | Diagnostics output pane | None |
| `IVsUIShell` | About dialog | `DTE.Version` is frozen at 18.0, so the product version is read from the executable instead (issue 24, closed 2026-05-20). |
| `AssemblyFileVersion` plus a JSON file on GitHub raw | Update notification (planned design) | The README says SSMS 22 has no Manage Extensions dialog. |

### SQLExtended (MIT, 2026-11-01)

This is the widest proof set. It references three SSMS assemblies at compile time from the install folder (`SqlWorkbench.Interfaces`, `SQLEditors`, `Microsoft.SqlServer.GridControl`) and marks them not-copied. NoName's rule is late binding, so for NoName these are proofs the API exists, not a pattern to copy. A second data point supports the late-binding rule: SqlPackageBase references Visual Studio 18 assemblies that conflict with the 17.x SDK at compile time (a CS1705 error), so its `ServiceCacheProxy` reaches `ServiceCache` by reflection only.

| API or pattern | Feature | Notes |
|---|---|---|
| DTE `Commands.Item("Query.Execute")` then `CommandEvents[guid,id].BeforeExecute`, `CancelDefault = true` | Confirm UPDATE or DELETE with no WHERE; cancels the run | Resolved by name at startup; the object is held in a static or the COM event source is collected and the handler stops firing "some minutes into the session". Confirmed in running SSMS 22, 2026-10-15 (author). Fails open. |
| `ServiceCache` (SqlPackageBase) by reflection: `ScriptFactory.CurrentlyActiveWndConnectionInfo.UIConnectionInfo`, `ServiceProvider.GetService` | Read the active query window's connection | Three fallback strategies. `ADMIN:` prefix is stripped so a DAC window does not pin the instance's single dedicated-admin slot. |
| `ScriptFactory.CreateNewBlankScript(ScriptType.Sql, UIConnectionInfo, null)` by reflection | Open Definition (F12): new query window already connected | Text inserted only if the factory left a new document active; fallback is a temp `.sql` via `dte.ItemOperations.OpenFile`. |
| `IObjectExplorerService`: `GetSelectedNodes`, `INodeInformation.UrnPath` and `NavigationContext`, `ConnectToServer` (by reflection) | Read the selected node; connect a server | `FindNode` and `SynchronizeTree` exist but it uses a tree walk instead. |
| Tree walk of the Object Explorer `TreeView` | Locate in Object Explorer (Ctrl+F12) | Server-side object resolution first; strict server match; English folder names only. |
| `TreeView.ContextMenuStripChanged` reflection hook | SQLExtended submenu on Object Explorer nodes | "SSMS builds those menus dynamically and there is no stable `IDM_OBJEXPL_*` GUID to place against." |
| `GridControl`, `IGridStorage.GetCellDataAsString`, `QEResultSet.IsCellDataNull` (reflection), `GetStorageColumnIndexByUIIndex` | Aggregates pane, find in grid, copy as IN filter, script as INSERT | Displayed text only; no typed path. Column 0 is the row number. Selection bounds mean different things per `SelectionType`. Notes say these were established by decompiling GridControl. |
| `.vsct` parents `guidSQLEditorGroup` `{33F13AC3-80BB-4ecb-85BC-225435603A5E}` ids `0x50` script, `0x60` messages, `0x70` grid | Context-menu entries | `IDM_VS_CTXT_CODEWIN` "merges without error and never appears in a query window". The ids are read from `SQLWorkbenchCommands` in SQLEditors by reflection. |
| `ProvideMenuResource` version number | Menu table refresh | Must be bumped with every `.vsct` change, then `Ssms.exe /updateconfiguration` once, because the shell keeps its own merged copy. |
| Key bindings in the SQL Query Editor scope, not only Global | Shortcuts | A Text Editor binding beats a Global one on the same chord, silently. Microsoft's own extensions win Global clashes (Copilot Ctrl+Alt+D, E, F, R; SqlFormatter Ctrl+K, Ctrl+Q; Presenter Mode Ctrl+Shift+F5). |
| DTE `TextDocument`, `EditPoint.ReplaceText`, `dte.UndoContext.Open/Close/SetAborted` | Format SQL as one undo step | None |
| MEF parts on `[ContentType("SQL")]`: completion source and commit manager, quick info, signature help, key processor, view tagger, `ClassificationTypeDefinition`; `ProvideBindingPath` | IntelliSense, hover info, rainbow brackets, comment colours | Proven. ComponentModelCache must be cleared on a stale MEF cache. |
| `ProvideToolWindow` with `MultiInstances` and `Transient`; `ToolWindowPane` hosting WPF | Four monitoring dashboards pinned per server; search, history, library windows | Panes are Transient because a pinned connection cannot be restored at startup. |
| `FileColorService.SetFileColorAsync` and `ColorByRegexConfig` file, by reflection | Environment tab colouring | "There is no API for tab colour." The shell reads a regex file, matches each document's full path, and derives the colour from a hash of the pattern text. |
| `IVsInfoBarUIFactory`, `IVsStatusbar` | Update banner, status text | None |
| ScriptDom 18.x in-process | Formatter, Qualify Columns, unsafe-DML analysis | SSMS provides it. |
| SMO, SQLite, SqlClient with its whole net48 closure bundled | Export, search index, cache | Bundles SqlClient because the host's binding redirects reach only so far. **Conflicts with ssms-helper**, which says do not bundle (see below). |
| Entra token handling for harvested connections (`EntraTokenBroker`) | Connections on Azure SQL | A harvested connection string has no password for Windows or Entra sign-ins. |
| Query history by polling DTE documents once a second, hashing text, debounce | SQL History window | **Polls document text, not executions**; it does not use Execute events. |
| Statistics capture vendored from StatisticsParser | Parse Statistics | Notes list unused brokered methods: `GetQueryPlanXmlSegmentAsync`, `GetClientStatisticsAsync`, `GetGridResultsSegmentAsync`. |
| Session log: 500-entry ring, optional file sink | Diagnostics | "Almost every failure in an SSMS extension has to be swallowed rather than thrown." `Debug.WriteLine` is stripped in Release; `ActivityLog.xml` is only written with `/log`. |

### Performance Studio SSMS VSIX (MIT)

| API or pattern | Feature | Notes |
|---|---|---|
| `.vsct` parent `{33F13AC3-...}` id `0x0080` | "Open in Performance Studio" on the execution-plan right-click menu | Comment: "confirmed same across SSMS 18-22" (author claim). |
| Walk the WinForms control tree from `GetFocus()` to find `Microsoft.SqlServer.Management.UI.VSIntegration.Editors.ShowPlan.ShowPlanControl`; call its non-public `GetShowPlanXml()` | Extract plan XML | Credits a Stack Overflow method by Martin Smith. Types found in already-loaded assemblies. |
| Temp `.sqlplan` file plus a named pipe to a desktop app | Open the plan in the external viewer | Reads the plan out; never shows a plan in SSMS. |
| `ProvideAutoLoad` for NoSolution and SolutionExists | Load | None |
| Installer app that finds both SSMS installs and calls each `VSIXInstaller.exe` | Install | SSMS 22 install is per-machine and needs `/admin` per its `install.cmd` (differs from NoName's per-user install; see open questions). |

### AxialSqlTools (Apache-2.0), MSSQLTOOL (Apache-2.0), SSMSPlus fork (unlicensed), ssms-helper (MIT)

Counts below are occurrences of each API name across each repository's source (names only), as a measure of how widely the pattern is used.

| API or pattern | Used by | Feature |
|---|---|---|
| `Query.Execute` with `CommandEvents.BeforeExecute` and `AfterExecute` | Axial, MSSQLTOOL, SSMSPlus (Before only), ssms-helper | Query history, execution guard, timing. ssms-helper stores each run's text in the view's `Properties` bag so the results step knows which text produced which grid. |
| `ServiceCache`, `ScriptFactory`, `UIConnectionInfo`, `CreateNewBlankScript` | Axial, MSSQLTOOL, SSMSPlus, ssms-helper | Connection read and new connected window. |
| `IObjectExplorerService` (`GetSelectedNodes`, `FindNode`), `INodeInformation` | Axial, MSSQLTOOL, SSMSPlus, Flank, OE Menu | Navigate and act on nodes. MSSQLTOOL notes `FindNode` can return a detached node context and `SynchronizeTree` can reconnect. |
| Native `SysTreeView32` read of Object Explorer roots | MSSQLTOOL | Fallback when the managed tree has no root "on some SSMS 22 builds". |
| `GridControl`, `IGridStorage`, `QEResultSet` | Axial, MSSQLTOOL, SSMSPlus, ssms-helper | Grid filter, export, copy as INSERT. ssms-helper path: doc view `.m_sqlResultsControl.m_gridResultsPage` then `FocusedGrid` then `GetResultSet(i)`; it says it decompiled SQLEditors and GridControl with `ilspycmd` and "re-check after SSMS updates". |
| `ScriptDom` with `TSqlParser` | all four | Parsing. |
| `IVsTextView`, `IWpfTextView`, `ITextBuffer`, `IAsyncCompletionSourceProvider` | ssms-helper, MSSQLTOOL | Completion popups, snippets. MSSQLTOOL notes SSMS 22's `GetPointOfLineColumn` returns client coordinates, not screen coordinates. |
| `ToolWindowPane`, `ProvideToolWindow`, `SettingsManager`/`ShellSettingsManager` | Axial, MSSQLTOOL, SSMSPlus | Dashboards, history windows, settings. |
| `ProvideOptionPage` / `DialogPage` | ssms-helper, SSMS Executor, OE Menu, Schema Folders | Tools > Options pages. |

### SSMS EnvTabs (Apache-2.0)

`IVsRunningDocumentTable`, `IVsMonitorSelection`, `IWpfTextView`, `IVsTextView`, `SqlScriptEditorControl`, `VSColorTheme`, `ToolWindowPane`. It writes the shell's regex colour config file. Issue 42 (closed 2026-10-07): writes fail with "file in use" when several tabs change at once. Axial's issue 129 reports resolving the tab's `IVsWindowFrame` and reading the doc view's `m_connection` (data source and database) per window, a method that contradicts SQLExtended's note that only the active window's connection can be read.

### DBA Dash SSMS extension (MIT)

| API or pattern | Feature | Notes |
|---|---|---|
| `Microsoft.SqlServer.Management.UI.VSIntegration.Editors.ShowPlan.ShowPlanControl.GetShowPlanXml()` by reflection | Open a plan externally | Comment: "confirmed directly against the SSMS 22 install". |
| `Microsoft.SqlServer.Management.SqlMgmt.Deadlock.DeadlockControl` (private parsed object model, re-serialized with `XmlSerializer`) | Read a deadlock graph out of SSMS's own viewer | The control keeps no XML string; the extension reconstructs it. Also: neither `DeadlockControl` nor its base overrides a context menu, so it assigns the control's `ContextMenuStrip` and polls for deadlock tabs because `WindowActivated` never fired for them. |
| `IVsTextManager` / `IVsTextView` | Read plan or deadlock XML from a plain XML results tab | Public, stable API; the XML-cell hyperlink opens a normal text editor. |
| `.vsct` parent `{33F13AC3-...}` ids `0x0070` (grid) and `0x0080` (plan) | Right-click entries | Comment cites `SQLWorkbenchCommands` in SQLEditors with `IDM_SQLWB_EXECPLAN_CONTEXT = 0x0080` and `IDM_SQLWB_SQLRESGRID_CONTEXT = 0x0070`. |
| `ScriptAndResultsEditorControl.m_connection` (a `Microsoft.Data.SqlClient` connection in SSMS 21 and 22, reached as `IDbConnection`) plus `UIConnectionInfo` | Reuse the query window's connection | Connection string lacks the password (Persist Security Info off); for a SQL login the password comes from `UIConnectionInfo.Password`; Entra is not supported. |

### Others (briefly)

| Extension | APIs and findings |
|---|---|
| Object Explorer Menu (GPL-3.0) | `IObjectExplorerService`, `INodeInformation`, `ContextMenuStripChanged`, `CreateNewBlankScript`, `Query.Execute`, `DialogPage`. Tested on 22.11.3 (author). |
| Schema Folders (GPL-2.0) | `SqlWorkbench.Interfaces` `IObjectExplorerService` and node extension; rewrites the Object Explorer tree into schema folders. Microsoft's SSMS 22.4.1 added a built-in "Group by schema" (per SqlPulse's WHATSNEW; **claimed**). |
| SSMS Executor (MIT) | DTE-only: executes the statement at the caret; ScriptDom; smallest example of a working 21/22 extension. |
| Flank (Apache-2.0) | `IObjectExplorerService`, `INodeInformation`, `UIConnectionInfo`, `ScriptFactory`, ScriptDom; also targets net10.0 helpers for Excel, Power BI and SSRS exports. |
| local-llm-chat-ssms (GPL-2.0) | `ToolWindowPane`, `OleMenuCommandService`, `DialogPage`, `SettingsManager`; parent `IDG_VS_WNDO_OTRWNDWS1` (View > Other Windows). Pure chat window; no editor, grid or connection access. API names only recorded. |
| SQL Sage (no licence, README only) | Claimed: runs on the user's own Claude Code or Codex CLI sign-in; a ScriptDom gate on every statement; "Mask names" replaces server, database, login, host and file names with placeholders; hash-chained local audit log; encrypted chat history (DPAPI). The closest product to NoName's redaction idea found. No source to verify. |
| SqlPulse (no licence, no source) | Claimed: query history, grid filter and conditional formatting, a profiler built on Extended Events, a "Transaction Guard" polling `sys.dm_exec_sessions` (needs `VIEW SERVER STATE`), a connection strip, session restore, Object Explorer grouping. WHATSNEW records an early bug where a grid storage proxy installed on every result grid broke scrollbars, now installed only on demand. Also: a `System.Windows.Forms.Timer` "silently fails in SSMS/WPF host", replaced by `DispatcherTimer`. |
| SQLVersionTools (no source in checkout) | A Git-sync tool for SSMS 21 with a separate Windows service; not usable as API evidence. |

## Proven-API matrix

**Class.** A = a shipping SSMS 22 extension does it (code read, or release dated after SSMS 22 reached GA). B = plausible via a documented VSSDK API or an SSMS-internal API, not shown by any shipping code. GAP = nobody does it. "Last confirmed" gives the strongest dated statement; NoName's own log is the only one with an exact SSMS build (22.11.12328.437, 2026-10-30), and it proves only rows 1 to 3.

| # | Capability | Extension(s) | Last confirmed | API or assembly | Class | NoName idea unblocked | Evidence |
|---|---|---|---|---|---|---|---|
| 1 | Package loads at shell start, background | NoName, all others | 22.11.12328.437, 2026-10-30 | `AsyncPackage`, `ProvideAutoLoad` | A | 05, 40 | `src/NoName.Diagnostics.Vsix/DiagnosticsPackage.cs`; log `session-20261030-193815-63d87e.jsonl` |
| 2 | Add a command to the Tools menu | NoName, StatisticsParser, DBA Dash | 22.11.12328.437, 2026-10-30 | `OleMenuCommandService`, `IDM_VS_MENU_TOOLS` | A | all | same log |
| 3 | Add a command to the query-window context menu | NoName, StatisticsParser, SQLExtended, Microsoft Copilot (its `extension.json` uses id 80, which is `0x50`) | 22.11.12328.437, 2026-10-30 | `{33F13AC3-80BB-4ECB-85BC-225435603A5E}` id `0x0050` | A | 01, 02, 49 | NoName log; `StatisticsParser.Vsix/VSCommandTable.vsct`; installed `SSMSAgent\.vsextension\extension.json` |
| 4 | Add commands to the Messages-pane, results-grid and execution-plan menus | SQLExtended (`0x60`, `0x70`), DBA Dash (`0x70`, `0x80`), ssms-helper (`0x70`), Performance Studio (`0x80`) | Releases 2026-10; "confirmed" per DBA Dash author | same GUID, ids `0x0060`, `0x0070`, `0x0080` | A (the `0x60` Messages menu: shipping code only, no explicit confirmation note) | 03, 19, 42, 44 | SQLExtended `SsmsSchemaViewerPackage.vsct`; DBA Dash `DBADashCommands.vsct` |
| 5 | Shell's `IDM_VS_CTXT_CODEWIN` shows in a query window | NoName declares it; SQLExtended says it does not | n/a | shell GUID | contradicted by SQLExtended | 01 | SQLExtended `CLAUDE.md`, VSCT section. NoName should not rely on it. |
| 6 | Read the active editor's selection and text | SQLExtended, SSMS Executor, ssms-helper, EnvTabs | 2026-10-29 (ssms-helper release) | DTE `TextSelection`/`TextDocument`; `IVsTextView`; `IWpfTextView` | A | 01, 07, 17 | SQLExtended `EditorHelper.cs`, `FormatCommand.cs` |
| 7 | Replace editor text as one undoable step | SQLExtended, ssms-helper | 2026-11-01 | DTE `UndoContext`, `EditPoint.ReplaceText`; `IWpfTextView` edit | A | 17, 25, 30 | SQLExtended `FormatCommand.cs` |
| 8 | Read the current connection | SQLExtended, Axial, MSSQLTOOL, SSMSPlus, ssms-helper, DBA Dash | 2026-11-04 (MSSQLTOOL release) | `ServiceCache.ScriptFactory.CurrentlyActiveWndConnectionInfo.UIConnectionInfo` (SqlPackageBase, SQLEditors; reflection) | A | 08, 49 | SQLExtended `ServiceCacheProxy.cs`; Axial `ScriptFactoryAccess.cs` |
| 9 | Read a specific tab's connection (not only the active one) | EnvTabs (per its design), Axial plan (issue 129); DBA Dash reads `m_connection` | contested | `IVsWindowFrame` then `DocView.m_connection` / `ScriptAndResultsEditorControl.m_connection` | B (SQLExtended says active-window only) | 38, 30 | EnvTabs sources; Axial issue 129; SQLExtended `EnvTabs/CLAUDE.md` |
| 10 | Read the current connection through the brokered service | StatisticsParser reads only `EditorMoniker` | SSMS 22.6+ | `ISqlEditorServiceBrokered.GetCurrentConnectionAsync` returning `SqlEditorConnectionDetails` with `EditorMoniker`, `EditorCaption`, `IsActive`, `ConnectionString` | B for the connection string (property exists in installed 22.11; nobody reads it; contents unverified) | 08 | installed `Microsoft.SqlServer.Management.UI.VSIntegration.SqlEditor.BrokeredContracts.dll` (`strings`) |
| 11 | Open a new query window already connected, with text | SQLExtended, Axial, MSSQLTOOL, OE Menu, ssms-helper, SSMSPlus, Flank | 2026-11-01 (SQLExtended) | `ScriptFactory.CreateNewBlankScript(ScriptType.Sql, UIConnectionInfo, null)` by reflection; fallback DTE `ItemOperations.OpenFile` of a temp `.sql` | A | 49, 17 | SQLExtended `QueryWindowOpener.cs`, `Definition/CLAUDE.md` |
| 12 | Intercept and cancel Execute | SQLExtended, Axial, MSSQLTOOL, ssms-helper, SSMSPlus | **22 (running SSMS), 2026-10-15** (SQLExtended author) | DTE `Commands.Item("Query.Execute")`, `CommandEvents.BeforeExecute`, `CancelDefault` | **A** (the brief expected a gap) | 16, 30 | SQLExtended `Safety/UnsafeDmlGuard.cs`, `Safety/CLAUDE.md` |
| 13 | Capture each executed batch | Axial (`AfterExecute`), MSSQLTOOL, SSMSPlus, ssms-helper | 2026-11-04 | `CommandEvents.BeforeExecute` / `AfterExecute` | A | 30 (query journal) | Axial `AxialSqlToolsPackage.QueryExecution.cs`. SQLExtended's history does **not** do this; it polls document text. |
| 14 | Know when a query finishes | StatisticsParser | 22.6+ | event-name pattern on the doc view (heuristic) | A, fragile | 19, 22 | `TabPageSupervisor.cs` |
| 15 | Read the Messages-tab text | StatisticsParser, SQLExtended | **22.6+** | brokered `GetMessagesTabSegmentAsync(editorMoniker, start, max, ct)` | A | 19 | StatisticsParser `Capture/`; `docs/TECHNICAL.md` §3 |
| 16 | Read grid results through the brokered contract | none | installed 22.11 only | `GetGridResultsSegmentAsync` returning `GridResultsSegment` with `ColumnNames`, `Rows`, `TotalRowCount`, `TotalColumnCount`, `StartRow`, `StartColumn`; `GetTextResultsSegmentAsync`; `GetAvailablePanesAsync` | B (names in the installed DLL; parameters not verified; no extension calls them) | 03, 19 | installed DLL; SQLExtended `Statistics/CLAUDE.md` notes the same names |
| 17 | Read results-grid cells and selection | SQLExtended, SSMSPlus, Axial, MSSQLTOOL, ssms-helper, DBA Dash | 2026-10-29 | `GridControl`, `IGridStorage.GetCellDataAsString`, `QEResultSet.IsCellDataNull`, `SelectedCells`, `GetStorageColumnIndexByUIIndex` | **A**; un-parks idea 03 | 03, 30, 49 | SQLExtended `ResultsGrid/ResultsGridReader.cs`, `ResultsGrid/CLAUDE.md`; ssms-helper `CLAUDE.md` |
| 18 | Read plan XML from the focused plan tab | Performance Studio, DBA Dash | v1.31.0, 2026-10-20 | `ShowPlanControl.GetShowPlanXml()` (reflection) | A | 42 (read side), 01 | `PlanViewer.Ssms/ShowPlanHelper.cs`; DBA Dash `ShowPlanXmlReader.cs` |
| 19 | Read a deadlock graph from SSMS's viewer | DBA Dash only | 4.23.1, 2026-10-28 | `SqlMgmt.Deadlock.DeadlockControl`, private parsed model re-serialized | A (single source) | 44 | DBA Dash `DeadlockXmlReader.cs` |
| 20 | Read plan or deadlock XML from an XML results tab | DBA Dash | 2026-10-28 | `IVsTextManager`, `IVsTextView` | A | 42, 44 | DBA Dash `ActiveEditorXmlReader.cs` |
| 21 | **Show a plan in SSMS's own viewer** | none | n/a | candidate: write a temp `.sqlplan` and `DTE.ItemOperations.OpenFile`; every extension that uses `OpenFile` opens `.sql` text views only | **GAP (B route)** | 42 | no extension found; grep across all local and downloaded sources |
| 22 | **Show a deadlock graph in SSMS's own viewer** | none | n/a | candidate: temp `.xdl` file the same way | **GAP (B route)** | 44 | same |
| 23 | Selected Object Explorer node | SQLExtended, Axial, MSSQLTOOL, SSMSPlus, OE Menu, Flank | 2026-11-01 | `IObjectExplorerService.GetSelectedNodes`, `INodeInformation.UrnPath`, `NavigationContext` | A | 26, 49 | SQLExtended `ObjectExplorerHelper.cs` |
| 24 | Locate and select a node in Object Explorer | SQLExtended (tree walk), MSSQLTOOL (`FindNode`), Axial | 2026-10-23 (SQLExtended issue 7 closed) | tree walk; `FindNode(urn)`; `SynchronizeTree` | **A** (brief expected a gap) | 26 | SQLExtended `ObjectExplorer/CLAUDE.md`; MSSQLTOOL notes |
| 25 | Connect a server in Object Explorer | SQLExtended | 2026-10 | `IObjectExplorerService.ConnectToServer` (reflection) | A | 26, 49 | `ObjectExplorerHelper.cs` |
| 26 | Add a command to Object Explorer node menus | SQLExtended, Axial, OE Menu | v4.1.2; tested 22.11.3 (author) | `TreeView.ContextMenuStripChanged` hook (no stable menu GUID) | A, fragile | 26, 01 | SQLExtended `ObjectExplorer/CLAUDE.md` |
| 27 | Show a WPF tool window | SQLExtended, Axial, MSSQLTOOL, SSMSPlus, local-llm-chat | 2026-11-04 | `ToolWindowPane`, `ProvideToolWindow`, `MultiInstances`, `Transient` | A | 12, 22, 38 | SQLExtended `SsmsSchemaViewerPackage.cs` |
| 28 | Add a tab to the results strip | StatisticsParser | 22.6+ | `SqlScriptEditorControl.TabPageHost` (reflection) | A | 07, 19 | `InPaneTab/TabPageSupervisor.cs` |
| 29 | Follow the SSMS theme live | StatisticsParser, SQLExtended | 2026-05-21 | `VSColorTheme.ThemeChanged`, `EnvironmentColors` | A | all UI | `StatisticsParserControl.xaml.cs` |
| 30 | Per-user settings in Tools > Options | StatisticsParser (Unified Settings), ssms-helper (`DialogPage`), Microsoft (`settingsRegistration.json`) | 22.6+ | `registration.json` plus `ISettingsReader`; `ProvideOptionPage` | A. Do not use `SettingsStore`/`BaseOptionModel` for Tools > Options pages. | 08, 40 | StatisticsParser `Options/StatisticsParserOptions.cs` |
| 31 | Per-user data files | SQLExtended (`%APPDATA%\SQLExtended`), StatisticsParser, NoName logs | 2026-11-01 | plain file I/O | A | 05, 28 | NoName log folder |
| 32 | MEF editor parts on the `SQL` content type | SQLExtended, ssms-helper | 2026-11-01 | completion, quick info, key processors, taggers, `ClassificationTypeDefinition`, `ProvideBindingPath` | A | 25, 30 | SQLExtended `IntelliSense/`, `Rainbow/` |
| 33 | Coloured tokens or text in the editor | SQLExtended (Rainbow, comment tags) | 2026-11-01 | `IViewTaggerProvider` with classifications | A | 25 | `Rainbow/RainbowTagger.cs` |
| 34 | **Error List entries and squiggles** | none | n/a | documented VS APIs (`ErrorListProvider`, `ITableDataSource`, an error tagger) | **GAP (B)**: grep for `ErrorListProvider`, `IVsErrorList`, `ITableDataSource`, `ErrorTag`, `SquiggleTag` finds nothing across all sources | 25 | all sources; SQLExtended `Rainbow/RainbowClassifications.cs` mentions squiggles only as something to avoid |
| 35 | Status bar text; info bar; output pane | SQLExtended, StatisticsParser | 2026-11-01 | `IVsStatusbar`, `IVsInfoBarUIFactory`, `IVsOutputWindow` | A | 05, 12 | respective sources |
| 36 | Colour query-window tabs | SQLExtended, EnvTabs, Axial | 2026-10-19 (EnvTabs) | `FileColorService.SetFileColorAsync`, shell regex colour config file | A | 38 | SQLExtended `EnvTabs/CLAUDE.md` |
| 37 | Parse T-SQL in process | SQLExtended, ssms-helper, Axial, MSSQLTOOL, SSMSPlus | 2026-11-04 | `Microsoft.SqlServer.TransactSql.ScriptDom` (host copy) | A | 08, 25 | NoName `CLAUDE.md` plus the extensions |
| 38 | Run DMV, Query Store, XE, error-log T-SQL from the extension | SQLExtended (dashboards), SqlPulse (claimed), SQL Sage (claimed) | 2026-11-01 | `Microsoft.Data.SqlClient` on a background thread with a harvested connection | A (SQLExtended code); SQL-side features need no SSMS-internal API | 22, 23, 24, 28, 41, 43 | SQLExtended `Monitoring/CLAUDE.md` |
| 39 | Launch an external app and pass data | Performance Studio (named pipe), DBA Dash | 2026-10-20 | `Process`, temp file, named pipe | A | 01 (optional channel) | `AppLauncher.cs` |
| 40 | Open-transaction watch | SqlPulse (claimed only) | 2026-08-22 | polls `sys.dm_exec_sessions`, needs `VIEW SERVER STATE`; floating banner | B (no source) | 30 | SqlPulse `README.md` |
| 41 | Extended Events capture UI | SqlPulse (claimed only); MSSQLTOOL "server health" | 2026-08-22 | XE sessions through T-SQL | B (no verifiable source for SqlPulse) | 23 | SqlPulse `README.md` |
| 42 | Insert text into SSMS's Copilot chat, or detect that Copilot is installed | none | n/a | Copilot is a separate component installed through the "AI Assistance" workload | **GAP** | 02, 40 | grep across all sources finds none |
| 43 | Third-party out-of-process (VisualStudio.Extensibility) extension | none (only Microsoft ships these) | n/a | `ExtensionType="VisualStudio.Extensibility"` manifest, `extension.json` | **GAP (B)** | none now | see Copilot section |
| 44 | Distribution without admin | NoName (per-user, `/instanceIds`), StatisticsParser (VSIXInstaller) | 22.11.12328.437, 2026-10-30 | SSMS's own `VSIXInstaller.exe`; `ProvideMenuResource` bump; `/updateconfiguration` | A | 40 | NoName `CLAUDE.md`; SQLExtended `CLAUDE.md` |

**Reading the gaps.** The brief's anticipated gaps (rows 12 and 24) are filled. Rows 21, 22 and 34 are the real gaps for NoName. Row 16 is a surprise in the other direction: the brokered contract in the installed SSMS carries more than any extension uses, including grid rows as data.

**First-party menu-id table.** All ids belong to the group GUID `{33F13AC3-80BB-4ECB-85BC-225435603A5E}`.

| Id | Symbol (from SQLEditors via SQLExtended and DBA Dash) | Seen in |
|---|---|---|
| `0x0050` | `IDM_SQLWB_SQLSCRIPT_CONTEXT` (query editor) | NoName, StatisticsParser, SQLExtended, Microsoft Copilot (as decimal 80) |
| `0x0060` | `IDM_SQLWB_SQLRESMSG_CONTEXT` (Messages pane) | SQLExtended |
| `0x0070` | `IDM_SQLWB_SQLRESGRID_CONTEXT` (results grid) | SQLExtended, DBA Dash, ssms-helper |
| `0x0080` | `IDM_SQLWB_EXECPLAN_CONTEXT` (execution plan) | Performance Studio, DBA Dash |

## What the Copilot extension shows about VisualStudio.Extensibility in SSMS 22

The source of Microsoft's Copilot extension is not public. What follows is first-party and read from files in the installed SSMS 22.11.12328.437 (read-only; names only). Anything about public documentation is **claimed**.

| Finding | Evidence |
|---|---|
| The SSMS agent is an out-of-process extension: `ExtensionType="VisualStudio.Extensibility"`, target .NET 10, `AllUsers`, marked Preview, version 22.200.11.171. | `Extensions\Microsoft\SSMSAgent\extension.vsixmanifest` |
| Its install target is the product id `Microsoft.VisualStudio.Community` with range [18.10,), not `Microsoft.VisualStudio.Ssms`. The Visual Studio shell version, not the SSMS product version, is the key. | same manifest |
| It declares everything in `.vsextension\extension.json`: command sets, control groups, placements, key bindings, a settings registration file, custom images, and a service hosted by `"host": "dotnetExtensibility"` with audience `Local | RemoteExclusiveClient`. | `.vsextension\extension.json` |
| Its context-menu group is placed with `legacyParentId` GUID `33f13ac3-80bb-4ecb-85bc-225435603a5e`, id 80, which is `0x0050`: the same query-editor menu classic extensions use. | same file |
| Four commands (Document, Explain, Fix, Optimize) with shortcuts Ctrl+Alt+D, E, F, R in the editor key scope `{5EFC7975-14BC-11CF-9B2B-00AA00573819}`; context providers "Editor,Shell". | same file |
| It ships a 93 MB `SqlCopilotServer.exe`, the `Microsoft.VisualStudio.Extensibility.*` assemblies, `GitHub.Copilot.SDK.dll`, a `Mcp` folder, and `Microsoft.VisualStudio.Copilot.ICopilotResponderProvider` parts. | directory listing |
| The bridge into the .NET Framework host is the brokered-contract assembly. Its own file comment reads: "Shared brokered service contracts for SqlEditor Service between SSMS (.NET Framework) and SSMSAgent (.NET 10)." The same assembly, version 22.11.171.48639, sits in the IDE folder, and StatisticsParser loads that copy. | both copies scanned with `strings`; sizes 26,464 and 25,952 bytes |
| The contract names include `ISqlEditorServiceBrokered`, `IQueryEditorTabDataServiceBrokered` and the methods listed in matrix rows 10 and 16, plus `GetOpenEditorsAsync` and `ChangeConnection`. Namespaces `...Copilot.SqlEditorFunctions` and `...Copilot.ObjectExplorerFunctions` appear as strings, which suggests the agent's tools read the editor and Object Explorer through these contracts (inference, not verified). | `strings` on the DLL |
| Other Microsoft extensions in the install are hybrids: `ExtensionType="VSSDK+VisualStudio.Extensibility"` with target `Microsoft.VisualStudio.Ssms` [22.0,): SQL Formatter, Presenter Mode, Schema Compare, CopilotUiTools. SQL Formatter ships a `registration.json` settings file and a MEF component. | installed manifests |
| Installation and requirements (claimed, from Microsoft Learn search text): GitHub Copilot in SSMS needs SSMS 22 and a GitHub account with Copilot access; it is added through the Visual Studio Installer's "AI Assistance" workload; "no plans to backport it to SSMS 21". | search result for Learn "installation state" page |
| Version history (claimed, from release notes in search text): agent mode first appears in preview in 22.7.0 (2026-06-09); 22.8.1 (2026-07-22) added agent skills such as wait category analysis and slow query investigation; 22.1.0 (2025-12-09) added bring-your-own-model. | learn.microsoft.com/ssms/release-notes-22 |

**What this means for NoName (evidence, not a decision).** Copilot is optional and installed as a separate workload, so a NoName package cannot assume it exists. No third-party extension found uses VisualStudio.Extensibility; all use in-process VSSDK. The brokered contracts are the first-party seam between in-process SSMS and out-of-process code, and they are what NoName's own late-bound reads would use.

## Candidate ideas for NoName that these extensions suggest

Each idea lists the evidence and the licence consequence. These are options, not decisions.

| # | Idea | Evidence | Licence note | Related NoName idea |
|---|---|---|---|---|
| 1 | **Prefer the brokered services for reads that the shell now exposes**, with reflection fallback: current connection (including `EditorCaption` and `IsActive`), messages text, grid rows, plan XML | Matrix rows 10, 15, 16; installed DLL names. They already changed once (22.6). | StatisticsParser MIT, SQLExtended MIT: studying is fine. Contract is Microsoft's, loaded from the install, never redistributed. | 03, 19, 42 |
| 2 | **Execute intercept as the armed-run confirmation** (cancel with `CancelDefault`) | Row 12; five extensions; confirmed 2026-10-15. Must resolve the command by name, keep the event source alive, fail open. | SQLExtended and ssms-helper MIT, Axial Apache-2.0: studying is fine. | 16 |
| 3 | **Grid cell and header reading for "redact this cell"** | Row 17; every note says displayed text only, NULL needs `IsCellDataNull`, over-cap selections should refuse rather than truncate. | MIT sources. | 03, 49 |
| 4 | **Row to prompt through a connected new window** | Rows 11, 17. | MIT sources. | 49 |
| 5 | **Plan and deadlock to prompt: read the XML** | Rows 18 to 20: plan via `GetShowPlanXml`, deadlock via re-serialized private model (DBA Dash only), XML tabs via `IVsTextView`. The viewer-side display remains a gap. | Performance Studio and DBA Dash MIT. | 42, 44, 01 |
| 6 | **Open-transaction watch from `sys.dm_exec_sessions`** | Row 40 (SqlPulse; claimed). Plain T-SQL needing `VIEW SERVER STATE`. | SqlPulse has no licence and no source: take the idea only. | 30 |
| 7 | **Locate an object in Object Explorer from the prompt** | Row 24; resolve the name on the server first, strict server match. | SQLExtended MIT. | 26 |
| 8 | **Safe-by-default AI wrapper design: mask names, ScriptDom gate, audit log, "what leaves your computer" chip** | SQL Sage README (claimed). Same problem space as the hub, different channel (it drives the user's CLI; NoName pastes). | No licence, no source: ideas only; the product name and wording are not reusable. | 01, 07, 08 |
| 9 | **Run the diagnostic probes inside SSMS before building** | Our VSIX proves only load and menu firing. Rows 12, 15 to 19, 24 and the brokered methods are the probe list that other extensions have already survived. | n/a | 05 |
| 10 | **Treat internal-API drift as normal**: log the resolved member names, fail soft, prefix-match parameters, record the SSMS build per proof | StatisticsParser (22.6), SSMS 22.3 breakages (below), SQLExtended Diagnostics notes. | MIT sources. | 05 |
| 11 | **Do not bundle assemblies the host ships** (SqlClient, ScriptDom, SMO) | ssms-helper: "SSMS 22.3 broke extensions that bundled their own versions". SQLExtended bundles SqlClient's net48 closure and says SqlClient binds to exact versions reachable only through the host's binding redirects. The two positions conflict. | MIT sources. | 40 |
| 12 | **Install per user; document the elevated-install trap** | NoName installs per user. SqlPulse WHATSNEW: installing as a different admin account registers the extension under that account's profile, invisible to the original user. | SqlPulse: idea only. | 40 |
| 13 | **Keybindings in the query-editor scope, not only Global** | SQLExtended: a Text Editor binding silently beats a Global one; Qualify Columns and Format SQL shipped broken for this. | MIT. | 30 |
| 14 | **Menu table versioning**: bump `ProvideMenuResource` and run `/updateconfiguration` after any `.vsct` change | SQLExtended notes; ssms-helper notes the same. | MIT. | 40 |

## Open questions and unverifiable claims

| Item | Status |
|---|---|
| Parameter lists for `GetGridResultsSegmentAsync`, `GetTextResultsSegmentAsync`, `GetOpenEditorsAsync`, `ChangeConnection`, `GetQueryPlanXmlSegmentAsync` | Only names and property names were read from the installed DLL (`strings`). Signatures need a metadata reader or a probe. |
| What `SqlEditorConnectionDetails.ConnectionString` contains (password? token?) | Unknown. If NoName reads it, it must be treated as a secret and never logged. |
| Why two copies of the brokered-contracts DLL differ in size (26,464 and 25,952 bytes) with the same version string | Unknown. The IDE-folder copy is the one StatisticsParser loads. |
| Whether `0x0060` (Messages menu) is confirmed on 22.11 | Shipping code only; no explicit confirmation note. |
| Whether per-document connection can be read for background tabs | SQLExtended says no; EnvTabs and Axial's plan say yes via `m_connection`. Needs a probe. |
| Exact SSMS minor for SQLExtended's 2026-10-15 confirmation | Not stated by the author. |
| Whether a plan or deadlock can be shown in SSMS's own viewer by opening a temp `.sqlplan` or `.xdl` through DTE | No extension tried it. Probe needed. |
| Whether Error List entries and squiggles work in SSMS 22 | No extension tried it. Probe needed. |
| SSMS Tools Pack and Poor Man's formatter on SSMS 22 | Not found. Not important: both are formatters or commercial. |
| SqlPulse, SQL Sage, SQLVersionTools features | Documentation only, no source. All **claimed**. |
| Microsoft statements about extension support and SSMS 22.3 | From search snippets of a Microsoft tech-community post and a Brent Ozar post: Microsoft "doesn't officially support SSMS extensions" and SSMS 22.3 "does not work when certain third-party extensions are installed". Learn pages were not fetched. **Claimed.** Redgate's own documentation separately says SSMS 22.3 changed internal components, builds fixed for 22.3 no longer work on 22.2 and below, and some versions crashed at startup or at first Object Explorer use (read from search text). |
| Whether SSMS 22 has a built-in Manage Extensions dialog | Conflicting: StatisticsParser's README and Brent Ozar's post say none; SQLExtended's docs mention one; the ErikEJ gallery lists a separate "SSMS Extension Manager" app (AGPL-3.0). Looks like a third-party tool, but this was not settled. |
| Per-user or all-users install | Performance Studio's `install.cmd` says SSMS 22 needs `/admin`; NoName installed per user with `/quiet /instanceIds`; SqlPulse says per-user under an elevated account is invisible to another user. Behaviour depends on the manifest `AllUsers` flag and the installer account; unresolved. |
| Whether the ScriptDom copy SSMS loads is 18.0.115 | Stated in NoName's `CLAUDE.md`; not re-checked here. |
| Whether the fetch helper's pages for ErikEJ's gallery blog and Microsoft Learn were read | They were not delivered; only GitHub API and search text were used for those. |

## Sources

All read on 2026-11-04 unless noted.

| Path or URL | What it supports |
|---|---|
| `<repo>/tmp/older/StatisticsParserExtension-main/` (manifest, `Capture/`, `InPaneTab/`, `Options/`, `docs/TECHNICAL.md`, `CHANGELOG.md`) | Brokered capture, TabPageHost, 22.6 signature change, Unified Settings |
| `<repo>/tmp/older/PerformanceStudio-main/src/PlanViewer.Ssms/` | ShowPlanControl, menu id 0x80, installer |
| `<repo>/tmp/older/SQLExtended-main/` (`CLAUDE.md`, `Safety/`, `Definition/`, `ObjectExplorer/`, `ResultsGrid/`, `EnvTabs/`, `Diagnostics/`, `Monitoring/`, `QueryWindowOpener.cs`, `ServiceCacheProxy.cs`, `.vsct`, csproj) | Execute hook, new window, locate, grid, tab colour, menu ids |
| `<repo>/tmp/older/SqlPulse-main/` (README, WHATSNEW) | Claimed features; no source |
| `<repo>/tmp/older/SQLVersionToolsPublic-main/` | Docs only |
| `<repo>/tmp/older/local-llm-chat-ssms-main/` | GPL-2.0, API names |
| `<repo>/src/NoName.Diagnostics.Vsix/DiagnosticsPackage.cs`, `VSCommandTable.vsct` | NoName's proven surface |
| `%LOCALAPPDATA%\NoName\Diagnostics\logs\session-20261030-193815-63d87e.jsonl`, `session-20261101-040515-8fa372.jsonl` (read via `/mnt/c/...`) | Host build 22.11.12328.437; commands fired |
| `F:\Program Files\Microsoft SQL Server Management Studio 22\Release\Common7\IDE\Extensions\Microsoft\SSMSAgent\` (`extension.vsixmanifest`, `.vsextension\extension.json`, `settingsRegistration.json`, brokered-contracts DLL) and the IDE-folder copy of the DLL | Copilot extension structure; brokered contract names |
| `...\Extensions\Microsoft\SSMS.SqlFormatter`, `SSMS.PresenterMode`, `SSMS.SchemaCompare`, `SSMS.CopilotUiTools`, `Copilot` manifests | Hybrid extension types |
| github.com/ErikEJ/SsmsExtensions (index page, 52 entries, 18 open source) and brink-daniel's `SSMSExtensionList.md` | Extension catalogue |
| github.com/Axial-SQL/AxialSqlTools (tarball, issues 21 and 129) | APIs, compatibility history |
| github.com/PuLuShen/MSSQLTOOL (tarball, `RELEASE_NOTES.md`) | APIs, SSMS 22 behaviour notes |
| github.com/emretor/ssms-helper (tarball, `CLAUDE.md`, `docs/FEATURES.md`) | 22.3 bundling note, grid reading |
| github.com/tkwj/ssms-executor, github.com/Blake-goofy/SSMS-EnvTabs (issues 38, 42), github.com/flank-project/ssms-extension, github.com/brink-daniel/ssms-object-explorer-menu, github.com/nicholas-ross/SSMS-Schema-Folders, github.com/alint997/SSMSPlus | Manifests, APIs, licences, release dates |
| github.com/trimble-oss/dba-dash (`DBADash.SSMSExtension/`) | Deadlock and plan reading, menu ids |
| github.com/LUMASoftPL/sqlsage README | SQL Sage claims |
| GitHub REST API (`gh api`) for licences, release dates, pushed dates of 20 repositories | Inventory columns |
| Web search: StatisticsParser launch post (brentozar.com, 2026-05); learn.microsoft.com SSMS 22 release notes and "installation state" page; techcommunity.microsoft.com SSMS 22 GA post; Redgate SSMS 22.3 compatibility notes (documentation.red-gate.com, productsupport.red-gate.com) | Claimed Microsoft statements, SSMS 22.3 breakage, Copilot requirements |
