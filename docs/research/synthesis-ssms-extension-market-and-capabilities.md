# Synthesis: what the SSMS extension market and the SSMS 22 surface tell NoName

Written 2026-11-04 by Claude (Fable 5.1) from the four research tracks in this folder, at the owner's request. Evidence, not decisions: nothing here changes BRN-001, DSN-001 or PRD-001 until the brainstorm or PRD step takes it up. Every claim below points at a track file; the track files carry the URLs and read dates.

## 1. The answer in brief

- **The suite's ideas are realistic.** Every in-SSMS mechanism NoName needs, except three, is shipping today in at least one SSMS 22 extension whose source was read: reading the editor selection and connection, opening a connected query window, intercepting Execute, reading grid cells and the Messages pane, reading plan and deadlock XML, Object Explorer nodes and menus, WPF tool windows, in-process ScriptDom, theme tokens, options pages ([track 3](open-source-ssms-extensions-proven-apis.md), matrix rows 1 to 44).
- **The three true gaps** are showing a plan or deadlock graph in SSMS's own viewer from an extension, Error List entries with squiggles (one unverified example), and any way to detect SSMS's Copilot or hand text to it ([track 3](open-source-ssms-extensions-proven-apis.md) rows 21, 22, 34, 42; [track 4](ssms22-extensibility-surface-and-native-features.md), "aspirational or blocked").
- **The hub is unique, with one near neighbour.** No vendor redacts anything before a prompt leaves the machine; Redgate documents that it transmits the query window, errors, plans and schema, and Devart's assistant sends metadata to its own service. The nearest product, SQL Sage (SSMS 22, source not public), masks server, database, login, host and file names, shows a "what leaves your computer" chip and gates statements with ScriptDom, but masks no table or column names, no secrets and no data ([track 1](commercial-ssms-addins-redgate-apexsql.md) matrix rows 1 and 6; [track 2](commercial-ssms-addins-devart-and-others.md) rows 22 to 27).
- **Copy-and-paste as the channel is unique.** Copilot in SSMS is optional (the "AI Assistance" workload, not installed by default), and no extension found detects it or feeds it ([track 4](ssms22-extensibility-surface-and-native-features.md) native-features table; [track 3](open-source-ssms-extensions-proven-apis.md) row 42).
- **The canary (IDEA-05) is the best-evidenced idea in the brainstorm.** Microsoft's FAQ says third-party extensions "are not supported" and feedback items are "closed without investigation". SSMS 22.3 changed internal components and crashed every older Redgate add-in at startup or when Object Explorer opened; 22.6 changed the brokered method signatures; 22.10 changed a SQLite binding redirect that broke one extension's history store ([track 4](ssms22-extensibility-surface-and-native-features.md), stance section; [track 1](commercial-ssms-addins-redgate-apexsql.md), break timeline; [track 3](open-source-ssms-extensions-proven-apis.md), SSMSPlus row).
- **SSMS 22 now ships things the suite must not rebuild**: a SQL formatter, schema compare, database projects with Git, a Query Hint Recommendation Tool, Copilot Agent mode with a bundled MCP server, group-by-schema in Object Explorer, and native tab rename and colour (section 4).
- **The crowded corners** are the editor toolkit (IDEA-30), object search (IDEA-26) and source control (IDEA-31): several free or cheap tools do each on SSMS 22. The uncrowded corners are the instance window's shared time brush (IDEA-46), history on the instance (IDEA-39), the error-log reader (IDEA-43), the deadlock list (IDEA-44), the server-side audit (IDEA-32) and everything that ends in a redacted prompt.

## 2. Evidence vocabulary

The four tracks used "class A" differently: tracks 1 and 2 wrote "A (vendor-proven)" for a vendor's own SSMS 22 statement, track 3 wrote A for shipping open-source code it read, and track 4 wrote A only for first-party or Microsoft evidence. BRN-001's class A means proven inside SSMS by NoName itself, so for this synthesis:

| Label | Meaning | Becomes BRN-001 class |
|---|---|---|
| **A** | NoName Diagnostics logged it on the owner's SSMS 22 | A |
| **B+** | A shipping SSMS 22 extension does it and its source was read (track 3), or a vendor states it for SSMS 22 on its own page (tracks 1 and 2); the probe is expected to pass | B, with a high-confidence probe |
| **B** | A documented VSSDK API or an SSMS-internal name exists; no shipping example | B |
| **GAP** | No extension found doing it; only a probe can tell | B, a true unknown |
| **C** | Outside SSMS: server objects, Agent jobs, a separate process | C |

Vendor statements are what the vendor says, not proof on the owner's host. Open-source code read in track 3 is the stronger evidence, because it names the exact API.

## 3. BRN-001 idea by idea

Verdicts: **confirmed** (a shipping SSMS 22 product or extension does the same job), **extends** (the market suggests a concrete addition), **unique** (no precedent found), **crowded** (free or cheap SSMS 22 tools already do it), **native** (SSMS 22 does it), **contradicts** (evidence against an assumption).

| Idea | Verdict | Strongest evidence | Label | What NoName must still prove | Track |
|---|---|---|---|---|---|
| 01 redacted AI prompts (hub) | unique; mechanics confirmed | Redgate Assistant's seven context sources (selection, errors, IntelliSense errors, schema, plan, formatting, analysis) are all read from an SSMS 22 package; no vendor redacts; SQL Sage masks names only | B+ | PRB-ED-02, PRB-SQL-01 on both installs | 1, 2 |
| 02 copy-and-paste prompts | unique | Copilot optional and not installed by default; no extension detects or feeds it | B+ (clipboard is OS) | PRB-OS-02 | 3, 4 |
| 03 redact a selected grid cell or header (parked) | confirmed mechanics | Six extensions read `SelectedCells` and `GetCellDataAsString`; the brokered `GetGridResultsSegmentAsync` exists in the installed contract but nobody calls it | B+ | PRB-GRID-01; row 03 can be un-parked when it passes on both installs | 3 |
| 04 per-user settings and presets | confirmed; one trap | Tools > Options in SSMS 22 binds to Unified Settings (`registration.json` plus `ISettingsReader`); the classic settings store "never reached" an Options page; `DialogPage` still works | B+ | A settings probe: write, restart SSMS, read | 3, 4 |
| 05 diagnostics canary | confirmed, strongly | Redgate's 22.0, 22.3, 22.4, 22.5, 22.6 break timeline; StatisticsParser's minimum raised to 22.6; SSMSBoost ships a build per SSMS update; Microsoft's FAQ | A (loads) | Nothing; add canary rows from section 5 | 1, 2, 4 |
| 06 MCP channel (parked) | confirmed feasible, stays parked | Copilot Agent mode GA in 22.10.2 with a bundled `sql-tools` MCP server; extensions may ship an `mcp.json` asset (Microsoft's do); Redgate's package starts and stops its own MCP server | B | Only when un-parked | 1, 4 |
| 07 preview with highlighted redactions | unique; one neighbour | SQL Sage's per-turn "what leaves" chip (claimed); Redgate gives prose only | B+ (tool window) | PRB-UI (tool window) | 1, 2 |
| 08 connection-aware redaction, ScriptDom | confirmed mechanics | `ServiceCache.ScriptFactory.CurrentlyActiveWndConnectionInfo` read by six extensions; ScriptDom used in process by five; SQL Sage's ScriptDom gate | B+ | PRB-SQL-01, PRB-PARSE-01; treat the brokered `ConnectionString` as a secret, contents unknown | 3 |
| 12 live diagnostics window | confirmed | WPF `ToolWindowPane` in five or more extensions; Redgate hosts WebView2 in one | B+ | The window itself | 1, 3 |
| 14 redaction receipt | unique | No product gives counts and a hash; SQL Sage's hash-chained audit log is a different thing | B+ | None beyond 07 | 1, 2 |
| 16 armed-run confirmation | confirmed, strongly | `Query.Execute` intercepted with `CommandEvents.BeforeExecute` and `CancelDefault` in five extensions, confirmed on a running SSMS 22 on 2026-10-15 (author's note); SSMSBoost, SSMS Tools Pack, SQL Guard, SQL Complete sell the same guard | B+ | PRB-ED-03; hold the event object, fail open; Copilot Agent mode runs queries through its own MCP server and will not pass this hook (inference) | 2, 3, 4 |
| 17 placeholder fill | mechanics confirmed; feature unique | Editor text replaced as one undo step (SQLExtended, ssms-helper); snippets with placeholders exist in SQL Prompt and SQL Complete, nothing fills from a per-user profile | B+ | PRB-ED-04 | 1, 2, 3 |
| 19 capture PRINT output | route confirmed, fragile | StatisticsParser reads the Messages pane through the brokered `GetMessagesTabSegmentAsync` (signature changed in 22.6); query-finished detection is a name-pattern heuristic | B+ | PRB-MSG-01; resolve methods by name and arity | 3 |
| 22 live monitor | confirmed and differentiated | dbForge Monitor is hosted, free, SSMS 22, one window per instance, real-time only, no blocking tree, no kill; SQL Sage shows a blocking chain with the idle open-transaction culprit and KILL as a script; SQLExtended polls DMVs from a tool window | B+ | PRB-LIVE-01 to 05; the audited kill needs IDEA-32 | 2, 3 |
| 23 Extended Events capture presets | unique in SSMS | dbForge Event Profiler is a separate app; SSMS's XEvent Profiler is native; SqlPulse claims one (no source) | C for the capture, B for the window | PRB-XE-01 to 05; hand-off to SSMS's XE viewer is a GAP | 2, 3, 4 |
| 24 index and statistics review | confirmed, partly | dbForge Index Manager does fragmentation only; SQLTreeo opens missing indexes in a connected window; SQL Prompt's index analysis sends the query to a hosted model | B+ | PRB-OE-04 | 1, 2 |
| 25 code analysis | confirmed; one gap | SQL Prompt (rule codes, bulk fix; no baseline or suppression-with-reason seen), SQL Complete (180+ rules), SQL Enlight (CLI, SonarQube; SSMS 22 build unverified); ErikEJ's T-SQL analyzer (MIT) claims Error List and squiggles on SSMS 22 | B+ for a findings list; GAP for squiggles | PRB-ED-05, PRB-ED-06 | 1, 2, 4 |
| 26 object search | crowded | Redgate SQL Search (free, SSMS 22), dbForge Search (free, with data search), SSMSBoost, Tools Pack, SQL Sage; locate-in-Object-Explorer shipping in two extensions | B+ | PRB-OE-05 | 1, 2, 3 |
| 27 decrypt modules | confirmed in principle; premise contested | dbForge SQL Decryptor is an app that says no DAC is needed; SQL Complete lists an in-SSMS decrypt (claimed); SQLExtended has a DAC-based implementation folder (not read for correctness) | B+ | PRB-SQL-04; check whether the DAC is required at all | 2, 4 |
| 28 job timeline | confirmed, partly | SQLTreeo Virtual Folders shows job overlaps with a suggested start time inside SSMS 22; its Job Timeline (baselines, 95th percentile) is a separate app; SSMS's Job Activity Monitor is a grid | B+ | PRB-OE-06; msdb T-SQL | 2, 4 |
| 29 delegated job changes | unique | None found | C | PRB-JC-01 | all |
| 30 editor toolkit | crowded | Every sub-feature ships in SSMS 22 in SSMSBoost, Tools Pack, SQL Complete, SQL Prompt, SQLExtended or AxialSqlTools; formatting is native since 22.10.2 | B+ | Only the sub-features that feed the hub or the instance window are worth building | 1, 2, 3 |
| 31 source control | crowded and partly native | Redgate SQL Source Control and dbForge Source Control on 22; VersionSQL adds security objects and Agent jobs; SSMS 22.10.2 made Database DevOps (SQL projects, Git, schema compare) generally available | B+ | What remains is the server-side "who changed it" capture (class C) | 1, 2, 4 |
| 32 server-side audit | unique | No add-in audits on the server; SQL Sage keeps a client-side hash chain; Tools Pack can write history to a user-chosen table (a precedent for writing to the instance) | C | The installer | 2 |
| 34 health overview | confirmed | dbForge Monitor's Overview tab; Activity Monitor is native | B+ | PRB-LIVE-01 | 2, 4 |
| 38 one tool window per instance | confirmed; one limit | dbForge Monitor opens one window per instance from Object Explorer; SQLExtended pins dashboards per server but marks them Transient because a pinned connection cannot be restored at startup | B+ | Window restore after restart | 2, 3 |
| 39 history on the instance | unique among add-ins | dbForge Monitor "does not log data"; DBA Dash (MIT) keeps a repository with its own SSMS extension; Tools Pack logs to a table | C | None in SSMS | 2, 3 |
| 40 light by design | confirmed by failures | A grid proxy on every result grid broke scrollbars (SqlPulse); a WinForms timer "silently fails" in the SSMS host (SqlPulse); SQL Prompt froze SSMS on tab switch; 22.3 crashed add-ins at startup | n/a | Add "no compile-time SSMS references" as the startup-crash mitigation | 1, 3 |
| 41 Query Store explorer | confirmed; two native overlaps | Native per-database Query Store reports; the Query Hint Recommendation Tool (GA 22.4.1) ranks hints and can apply a Query Store hint; SQL Sage links history entries to Query Store plans | B+ (pure T-SQL) | None in SSMS; avoid duplicating the hint tool | 2, 4 |
| 42 plan inspector | read side confirmed; viewer GAP | Plan XML read from `ShowPlanControl.GetShowPlanXml()` by Performance Studio and DBA Dash; the brokered `GetQueryPlanXmlSegmentAsync` exists unused; no extension opens a plan in SSMS's own viewer; plan comparison is native | B+ read, GAP show | PRB-LIVE-06 (the temp `.sqlplan` probe) decides FR-039 and FR-042 | 3, 4 |
| 43 error log reader | unique among add-ins | Native Log File Viewer only | B+ (pure T-SQL) | None | 4 |
| 44 deadlock viewer | list confirmed; graph GAP | DBA Dash reads a deadlock graph out of SSMS's viewer through its private model; nobody opens one in the viewer | B+ list, GAP graph | PRB-XE-06 (the temp `.xdl` probe) decides FR-058 | 3, 4 |
| 45 PerfMon trends | confirmed | `sys.dm_os_performance_counters` is in-instance; host counters are class C | B+ | None | 4 |
| 46 shared time brush | unique | Nothing in any add-in or native report | B+ (own UI) | None | all |
| 47 health facts | confirmed, partly | Vulnerability assessment (security only) is native; SQL Sage's "Check this database"; DBA Dash | B+ | None | 2, 4 |
| 48 collection health | unique among add-ins | Repository products only | C | None | 3 |
| 49 row to prompt | confirmed | A connected new window through `ScriptFactory.CreateNewBlankScript` in seven extensions; the grid menu `0x0070` in three; SQLTreeo opens missing-index scripts the same way; Copilot's Document, Explain, Fix and Optimize sit on the same `0x0050` editor menu NoName uses | B+ | PRB-ED-04 | 2, 3 |

**ASM-23** (ideas from the owner's private toolkit generalize): armed-run confirmation has four commercial and five open-source precedents, so it generalizes. Placeholder fill and build-currency badges have no precedent; PRINT capture has a route but no product. These three stay open.

## 4. Already native in SSMS 22: do not rebuild

From [track 4](ssms22-extensibility-surface-and-native-features.md) (release notes read 2026-11-04) and [track 2](commercial-ssms-addins-devart-and-others.md).

| Native feature | Since | Overlaps | What the idea still adds |
|---|---|---|---|
| GitHub Copilot chat, inline chat, /doc /explain /fix /optimize | GA 22.4.1 | 01, 02, 07 | Local redaction, no account, any AI, a preview |
| Copilot reads the results pane, plan, messages, client statistics | 22.5.0 | 03, 42, 49 | Row-level redaction choices |
| Copilot Agent mode with the bundled `sql-tools` MCP server, skills for waits, slow queries, IO and memory | GA 22.10.2 | 16, 22, 34, 47 | Deterministic, local, token-free tooling; the audit |
| Query Hint Recommendation Tool; applies the chosen hint as a Query Store hint (claimed) | GA 22.4.1 | 41, 24 | Do not duplicate; link to it |
| SQL Formatter | GA 22.10.2 | 30 | Nothing |
| Schema Compare | GA 22.10.2 | 31 | Nothing |
| Database DevOps: SQL projects, Git, create project from database | GA 22.10.2 | 31 | Only the server-side change capture |
| Object Explorer "group by schema" | 22.4.1 | 26 (partial) | Search across module text |
| Show Execution Plan in New Tab; plan comparison | 22.0.0; pre-21 | 42 | Prompt from a plan; Query Store link |
| Tab rename; colour by extension (claimed, from a blog review); one status-bar colour | 22.x | 30 | Per-environment colour is the gap SSMSBoost and Tools Pack fill |
| Unified Settings tab for IntelliSense and shortcuts | 22.6.0 | 04 | Nothing; it is the settings mechanism to use |
| Query Store reports (per database), XEvent Profiler, Live Query Statistics, Activity Monitor, deadlock graph viewer, Log File Viewer, Job Activity Monitor, vulnerability assessment | pre-21 | 41, 23, 22, 44, 43, 28, 47 | The instance-level view, the time brush, grouping, history and the prompt |

Not native, and claimed absent from the 21.x and 22.x release notes: query history with session restore, live T-SQL code analysis, instance-wide search, a blocking tree with deltas, any redaction.

## 5. Gaps, breaks and traps

**Nobody does it (probe first, design second).**

| Gap | Candidate route | Decides |
|---|---|---|
| Show a plan in SSMS's own viewer from an extension | Write a fictional-name `.sqlplan` to a temp folder and open it through `DTE.ItemOperations.OpenFile`; no `.sqlplan` editor registration was found in the pkgdefs | FR-039, FR-042 (PRB-LIVE-06) |
| Show a deadlock graph in SSMS's own viewer | The same with `.xdl` | FR-058 (PRB-XE-06) |
| Error List entries and squiggles | Documented VS APIs; ErikEJ's analyzer is the only shipping example and was not run | FR-018 (PRB-ED-05, PRB-ED-06) |
| Detect Copilot or hand text to its chat | None found; `ICopilotResponderProvider` is Microsoft's contract, third-party use unknown | NFR-006's "optional fast path" |
| A pure out-of-process (VisualStudio.Extensibility) third-party extension | Only Microsoft ships them; the host is present; SSMS is not a documented host | Nothing now |
| Open a Query Store report with parameters | No entry point | IDEA-41 reads the views directly |
| Object Explorer menu by `.vsct` | No stable GUID; every extension hooks `TreeView.ContextMenuStripChanged` by reflection | IDEA-26, IDEA-49 entry points: fragile by nature |
| Read a background tab's connection | Contested: SQLExtended says active window only; EnvTabs, AxialSqlTools and DBA Dash read `m_connection` per frame | IDEA-38's per-instance window |
| A Manage Extensions dialog in SSMS 22 | Conflicting evidence; the Extension Manager package is present and the Options page is removed by `SSMS.pkgundef` | One look at the 22.11 menu bar |

**Breaks recorded by shipping extensions (canary rows for IDEA-05).**

| SSMS build | What broke | Source |
|---|---|---|
| 22.0 | Stale extension cache after upgrade (`SSMS.exe /updateconfiguration` fixes it); first-run settings import froze with SQL Prompt installed | track 1 |
| 22.3 | "changes to internal components used by extensions": older SQL Prompt, SQL Search, SQL Test, SQL Doc crashed SSMS at startup or on first Object Explorer use; fixed builds no longer load on 22.2 | tracks 1, 4 |
| 22.4 | SQL Prompt failed to load at startup | track 1 |
| 22.5 | Older SQL Source Control, SQL Doc, Dependency Tracker, Data Generator crashed SSMS | track 1 |
| 22.6 | Every brokered tab-data method gained a leading editor-moniker parameter; SQL Prompt could no longer switch off built-in IntelliSense | tracks 1, 3 |
| 22.10 | SQLitePCLRaw redirected to 3.x, breaking an extension's `Microsoft.Data.Sqlite` history store (claimed) | track 3 |
| 21 onward | An add-in's top-level menu is hidden until the user enables it under Extensions > Customize Menu | track 2 |

**Engineering traps the sources agree on** ([track 3](open-source-ssms-extensions-proven-apis.md), candidate ideas 10 to 14):
- The shell's `IDM_VS_CTXT_CODEWIN` placement "merges without error and never appears in a query window"; only `{33F13AC3-80BB-4ECB-85BC-225435603A5E}` `0x0050` shows. NoName's `.vsct` declares both; the log does not say which one the 2026-10-30 click came from. That one group GUID holds four menus, resolved by shipping extensions: `0x0050` query editor, `0x0060` Messages pane, `0x0070` results grid, `0x0080` execution plan ([track 3](open-source-ssms-extensions-proven-apis.md), menu-id table).
- Applying editor text while Copilot Chat is active crashed SQL Prompt until its 11.3.12 fix: a coexistence case for the canary and for C-08, to test on the install with the AI component ([track 1](commercial-ssms-addins-redgate-apexsql.md), matrix row 10a).
- Bump `ProvideMenuResource` after every `.vsct` change and run `/updateconfiguration` once, or the shell keeps its merged copy.
- Key bindings in the SQL editor scope beat Global ones silently; Microsoft's own shortcuts (Ctrl+Alt+D, E, F, R; Ctrl+K, Ctrl+Q; Ctrl+Shift+F5) win Global clashes.
- Hold the `CommandEvents` object in a static or the Execute hook stops firing minutes into the session.
- Bundling host assemblies (SqlClient, ScriptDom, SMO) is contested: one extension says 22.3 broke those that bundled, another bundles SqlClient's whole net48 closure.
- A WinForms `Timer` fails silently in the SSMS host; use a `DispatcherTimer`.
- Per-user against all-users install is unresolved: NoName installs per user; one installer script says SSMS 22 needs `/admin`; an elevated install under another account is invisible to the user.
- Microsoft's own extensions list arm64; NoName's manifest is amd64 only (PRD-001 NFR-039 keeps ARM64 for later).

## 6. New candidate ideas, consolidated

For the next brainstorm pass. Each names the idea it extends, its evidence label, and its source track. Duplicates across tracks are merged.

| # | Candidate | Extends | Label | Evidence | Track |
|---|---|---|---|---|---|
| N1 | Prompt context checklist: selection or script, execution errors, IntelliSense errors, object schema, plan, formatting, analysis findings, all built locally and shown in the preview | 01, 42, 49 | B+ | Redgate Assistant's tool set | 1 |
| N2 | Error-triggered prompt offer: when a batch fails, offer "Build a prompt for this error"; filter by error class (Redgate's popup misfired on timeouts) | 01 | B+ | SQL Prompt's Fix popup; the execute-complete heuristic | 1, 3 |
| N3 | Armed-run markers: comment tokens in a script that force confirm, forbid, or lift the guard once | 16, 30 | B+ | SSMSBoost's Ask, Prohibit and Unblock tokens | 2 |
| N4 | Blocking view that names the idle session holding an open transaction, with KILL as a reviewed script when the server audit is absent | 22, 32 | B+ | SQL Sage; dbForge Monitor has no tree | 2 |
| N5 | Object Explorer right-click as the per-object entry point: prompt for this procedure, missing indexes, calling jobs, dependents | 49, 24, 26 | B+ fragile | SQLTreeo, SQLExtended, AxialSqlTools; reflection-only menu | 2, 3 |
| N6 | Agent job overlap view with a suggested start time, then per-job baselines (median, 95th percentile) | 28 | B+ | SQLTreeo Virtual Folders (hosted) and Job Timeline (app) | 2 |
| N7 | Prefer the brokered services for reads the shell exposes (connection, messages, grid rows, plan XML) with reflection fallback, resolved by name and arity | 03, 19, 42 | B | The installed contract carries more than any extension uses; it changed once already | 3 |
| N8 | Canary rows from the break list, plus a "built for SSMS 22.x or later" warning on load | 05 | A | Section 5; SQL Test's installer warning | 1 |
| N9 | Coexistence rules: detect SQL Prompt, SQL Complete, SSMSBoost and yield shortcuts or overlapping features; a command dump as a probe | 05, 40 | B+ | SQL Sage yields to SQL Prompt and SQL Complete; SSMSBoost's command dump | 2 |
| N10 | Post-execution handlers for long runs: notify, save results, run a command | 30 | B+ | SSMSBoost | 2 |
| N11 | Run one script on several databases of this instance with saved groups | new | B+ | Tools Pack, SQL Complete | 2 |
| N12 | Tab colour by environment role through the shell's regex colour file (there is no tab-colour API) | 16, 21 | B+ | SQLExtended, EnvTabs | 3 |
| N13 | Client-side hash-chained evidence export, as the no-server-audit fallback row | 32 | B+ | SQL Sage | 2 |
| N14 | Query journal storage without a bundled SQLite: the 22.10 binding redirect and the documented SQLite corruption cases argue for JSON Lines, which NoName already has | 30, 40 | n/a | SQL Prompt's SQL History corruption page; SSMSPlus | 1, 3 |
| N15 | Foreign-key dependency map for a table | 34, 47 | B+ | Tools Pack 7.4 | 2 |
| N16 | Search result to redacted prompt: the only search feature a free incumbent does not have | 26, 49 | B+ | SQL Search and dbForge Search are free on 22 | 1, 2 |

Not recommended from the research: a formatter (native), hosted index advice that sends the query off the PC (SQL Prompt), Object Explorer tree rewrites (fragile; group-by-schema is native), and anything that only ever shipped in the 32-bit SSMS 18 and 19 add-ins (ApexSQL's line, support ended 2025-12-31 per Quest notices that could not be read directly).

## 7. What this means for the probe catalog

RES-NN-001's catalog and the proposed probes map onto the evidence like this. "Survived by" counts the shipping extensions in track 3 that use the same API on SSMS 22.

| Probe | Survived by | Expectation |
|---|---|---|
| PRB-SQL-01 connection (classic) | 6 extensions | Pass |
| PRB-SQL-02 connection (brokered) | StatisticsParser reads the moniker only | Pass for the moniker; `ConnectionString` contents unknown, never log it |
| PRB-GRID-01 grid by reflection | 6 extensions | Pass; displayed text only, NULL needs `IsCellDataNull`, column spaces differ |
| PRB-GRID-02, PRB-PLAN-01 brokered grid and plan | nobody calls them | Unknown; names exist in the installed DLL |
| PRB-MSG-01 brokered messages | StatisticsParser | Pass on 22.6 or later; signature drift |
| PRB-OE-01 selected node | 6 extensions | Pass |
| PRB-PARSE-01 ScriptDom | 5 extensions | Pass |
| PRB-ED-03 execute intercept (proposed) | 5 extensions | Pass, with the static-reference caveat |
| PRB-ED-04 connected new window (proposed) | 7 extensions | Pass |
| PRB-ED-05, PRB-ED-06 squiggles, Error List (proposed) | 1 (unverified) | Unknown |
| PRB-LIVE-06, PRB-XE-06 open in SSMS's viewer (proposed) | nobody | Unknown; these two decide the viewer requirements |
| PRB-UI-03 flow map (proposed) | nobody | Unknown |
| New: Options page mechanism (Unified Settings against `DialogPage`) | StatisticsParser, ssms-helper | Both work; pick one |
| New: which context-menu parent showed on 2026-10-30 | SQLExtended | `0x0050` only |
| New: Manage Extensions dialog present | conflicting | Look |
| New: background tab's connection | contested | Probe |

## 8. Where the tracks disagree

| Topic | Track says | Other track says | Reading |
|---|---|---|---|
| SSMS Tools Pack on 22 | Track 2 read the vendor page: "22.7+" | Track 3: status not found | The vendor page wins |
| Error List and squiggles | Track 3: a gap, nothing in the sources it grepped | Track 4: ErikEJ's analyzer (MIT) has Error List and tagging code and claims SSMS 22 | One unverified example; still a probe |
| Class A | Tracks 1 and 2: vendor-proven; track 3: shipping code; track 4: first party only | Section 2's vocabulary | Use B+ in the brainstorm |
| Decrypting modules | BRN-001 IDEA-27: over the DAC | Devart: "no need to use the DAC mode" | Verify before PRB-SQL-04 is written |
| Background tab's connection | SQLExtended: active window only | EnvTabs, AxialSqlTools, DBA Dash: per frame | Probe |
| Bundling host assemblies | ssms-helper: never | SQLExtended: bundles SqlClient | Late binding sidesteps most of it; decide per assembly |
| ApexSQL status | Track 1: support ended 2025-12-31 and no SSMS 21 or 22 statement | Every Quest page refused automated reading (HTTP 202 or 403) | All ApexSQL facts are claimed |
| Where the third-party checkouts live | The brief to track 3 named `tmp/repos/` | Track 3: the six extension checkouts are under `tmp/older/`, and `tmp/repos/` holds only dbatools, sql-docs and sqlserver-kit | CLAUDE.md says only "the git-ignored `tmp/`", which is right; nothing to fix. Name `tmp/older/` in future briefs |

## 9. Licence notes for reading third-party code

Ideas only, never code, whatever the licence (CLAUDE.md). For the record, from track 3: MIT (StatisticsParser, Performance Studio, SQLExtended, ssms-helper, SSMS Executor, DBA Dash, ErikEJ SqlServer.Rules), Apache-2.0 (AxialSqlTools with an ethical-use notice, MSSQLTOOL, SSMS-EnvTabs, Flank), GPL-2.0 (SSMS Schema Folders, local-llm-chat-ssms), GPL-3.0 (Object Explorer Menu), AGPL-3.0 (SSMS Extension Manager app, Right Way SQL Formatter, Poor Man's T-SQL Formatter), no licence (the SSMSPlus fork, SqlPulse, SQL Sage). Microsoft's brokered-contract assembly is loaded from the SSMS install at run time and never redistributed.

## 10. The track files

1. [Commercial SSMS add-ins: Redgate and Quest (ApexSQL)](commercial-ssms-addins-redgate-apexsql.md)
2. [Commercial SSMS add-ins: Devart (dbForge) and other vendors](commercial-ssms-addins-devart-and-others.md)
3. [Open-source SSMS 21/22 extensions and the proven-API matrix](open-source-ssms-extensions-proven-apis.md)
4. [SSMS 22: extensibility surface, Microsoft's stance, and native features](ssms22-extensibility-surface-and-native-features.md)
