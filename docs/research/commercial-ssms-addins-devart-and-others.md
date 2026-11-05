# Commercial SSMS add-ins: Devart (dbForge) and other vendors
Research track 2 of 4 for NoName. Written 2026-11-04 by a Sonnet 5.5 research subagent at the owner's request. Evidence, not decisions: nothing here is a requirement until the brainstorm or PRD takes it up.

## Scope and method

**Scope.** Devart dbForge SQL Tools add-ins (not dbForge Studio), plus SSMSBoost, SSMS Tools Pack, SolarWinds Plan Explorer and SQL Sentry, Idera, SQL Enlight, SQLTreeo, VersionSQL, Quest Spotlight, Minion, and newer SSMS 22 entrants found through the community gallery (S43). Redgate and ApexSQL belong to another track; they appear only where their compatibility notes are evidence.

**How pages were read.** The `WebFetch` tool was not available. Vendor pages were fetched with `curl` and stripped to text; the SQL Enlight site is a JavaScript app, so its facts come from its published JS bundles. All read dates are 2026-11-04. Every `docs.devart.com`, vendor and Microsoft page below was read directly unless marked.

**Evidence labels.**
- **V** = read on the vendor's own current page (the vendor's claim, not first-party proof that it runs on the owner's host).
- **M** = Microsoft page read directly.
- **S** = secondary: a web-search summary, reseller mirror, blog or community catalog that I did not confirm on a vendor page.
- **inferred** = my reading of how a feature works; no vendor states the mechanism.
- **claimed** = marketing statement, not verified.

**Class rule used in the matrix.** BRN-001 class A needs the capability to be hosted inside SSMS and the vendor to state SSMS 22 support. Everything marked A below is therefore vendor-claimed, which is the standard the task set. A product that is a standalone app (Event Profiler, SQL Decryptor, Job Timeline, Plan Explorer's viewer) is class C precedent even when it has a small SSMS launcher.

**Host context.** SSMS 22.11.3 (released 2026-10-29) updated Visual Studio to 18.11.3 [12328.437] (M, S42). That build number equals the owner's host, so the host is 22.11.3. No vendor below names 22.11 explicitly; the newest explicit statements are SSMSBoost 22.10.2 and SQL Complete 22.6 (S, see S48).

**Microsoft's own position (M, S41).** The SSMS FAQ says third-party extensions are not supported, SSMS does not actively block them, and feedback items about them are closed without investigation. This is the risk frame for every row below.

## Vendor status in SSMS 22 and 21

"Hosted" = runs inside SSMS (menus, tool windows, editor, Object Explorer, grid). "App" = standalone process.

| Product | Hosted in SSMS or standalone | SSMS 22 | SSMS 21 | 18/19 only? | Source | Read |
|---|---|---|---|---|---|---|
| dbForge SQL Complete | Hosted (SSMS and Visual Studio) | Yes, added in v2025.3 (V) | Yes (V) | No; also 2016, 17, 18, 19, 20 | S4, S7, S3 | 2026-11-04 |
| dbForge Search (free) | Hosted | Yes (V) | Yes (V) | No | S10, S21 | 2026-11-04 |
| dbForge Monitor (free) | Hosted (Object Explorer right-click) | Bundle page lists SSMS v22 (V); per-product requirements page not found | Same | No | S3, S11, S13 | 2026-11-04 |
| dbForge Index Manager | Hosted | Yes (V) | Yes (V) | No | S10 | 2026-11-04 |
| dbForge Unit Test | Hosted | Yes (V) | Yes (V) | No | S10, S17 | 2026-11-04 |
| dbForge Source Control | Hosted | Yes (V) | Yes (V) | No | S10, S18 | 2026-11-04 |
| dbForge Schema Compare, Data Compare | Both: standalone app and SSMS add-in | Yes (V) | Yes (V) | No | S10, S20 | 2026-11-04 |
| dbForge Data Generator, Documenter | Both: standalone app and SSMS add-in (install-time choice) | Yes (V) | Yes (V) | No | S10, S20 | 2026-11-04 |
| dbForge Data Pump | Hosted | Yes (V) | Yes (V) | No | S10, S19 | 2026-11-04 |
| dbForge Query Builder | App (editions table shows "app" only) | n/a | n/a | n/a | S2 | 2026-11-04 |
| dbForge Event Profiler (free) | App with its own Start Page | n/a; bundle page lists SSMS v22 | same | n/a | S2, S14 | 2026-11-04 |
| dbForge SQL Decryptor (free) | App; its FAQ says "runs as a standalone tool" | n/a | n/a | n/a | S16 | 2026-11-04 |
| dbForge AI Assistant | Feature inside SQL Complete (and Studio products); not a separate SSMS add-in | Yes, via SQL Complete (V) | Yes | No | S8 | 2026-11-04 |
| SSMSBoost 5.8 (build 2026-10-20) | Hosted | Yes; builds named for 22.5.2, 22.7.1, 22.10.2 (V) | Yes (V) | No; supports 2012 and newer | S22, S23, S24 | 2026-11-04 |
| SSMS Tools Pack 7.4.1.0 (2026-10-14) | Hosted | "22.7+" (V) | "21.x" (V) | No; also 17 down to 2008 | S27, S28 | 2026-11-04 |
| SolarWinds Plan Explorer | App; SSMS 22 plugin is a separate VSIX | Plugin exists, download from Customer Portal (V) | Not stated | SSMS 20 supported by 2024.3 (S) | S30, S31 | 2026-11-04 |
| SolarWinds SQL Sentry | Separate monitoring product; only SSMS link found is the Plan Explorer plugin | via Plan Explorer plugin | not stated | n/a | S30, S31 | 2026-11-04 |
| Idera SQL Diagnostic Manager 14.3, SQL Doctor, SQL Admin Toolset | Standalone products; no SSMS add-in found on their pages | no evidence | no evidence | no evidence | S46 | 2026-11-04 |
| SQL Enlight (Ubitsoft) | Hosted (SSMS and Visual Studio), plus CLI and SonarQube | System Requirements page lists SSMS 22; product table stops at SSMS 21 (since build 2.0.4.99); no build stated for 22 | Yes | No | S32 | 2026-11-04 |
| SQLTreeo Virtual Folders for SSMS | Hosted, SSMS 22 only | Yes, v22.0.0.126 (2026-11-01); first 22 build 2026-09-06 (V) | Not offered | n/a | S33 | 2026-11-04 |
| SQLTreeo Job Timeline, Compare, Restore, T-SQL Debugger | Standalone Windows desktop apps | n/a | n/a | n/a | S33 | 2026-11-04 |
| VersionSQL 2.7 (2026-09-25) | Hosted | Yes, native ARM64 installer since 2.6.1 (V) | Yes | No; changelog also names SSMS 20 and earlier | S34 | 2026-11-04 |
| SQL Shades 2.0.1091 (2026-10-15) | Hosted (themes) | Yes (V) | Yes | Covers SSMS 18 to 22 | S35 | 2026-11-04 |
| Tabs Studio | Hosted | Yes (V) | Yes (V) | Older SSMS in separate installers | S40 | 2026-11-04 |
| SQL Sage (LUMA) 1.0.0 | Hosted, SSMS 22 only | Yes (V, repo README) | No | No | S37 | 2026-11-04 |
| SQL Guard | Hosted | "SSMS 18-22" (claimed) | claimed | claimed | S38 | 2026-11-04 |
| RenSQL, SQL Lizard, SQLinForm | Hosted | RenSQL: 21 and 22; SQL Lizard: 22; SQLinForm: 18 to 22 (claimed) | see left | no | S40 | 2026-11-04 |
| SqlSmash | Hosted | No SSMS 22 statement; page copyright 2023 | unknown | unknown | S39 | 2026-11-04 |
| Quest Spotlight (SSMS extensions) | Old SSMS plug-ins found only in dated support articles | no evidence | no evidence | probably SSMS 2016 era (S) | S48 | 2026-11-04 |
| Minion Enterprise | Product pages are empty shells; no SSMS add-in evident | unknown | unknown | unknown | S47 | 2026-11-04 |

**Menu quirk that affects NoName's menu design (V).** Both Devart (S8) and SSMSBoost (S23) document that from SSMS 21 an add-in's top-level menu is not added to the menu bar by default; the user opens Extensions > Customize Menu and clears or sets the add-in's checkbox. SSMSBoost's changelog calls it a "breaking change with SSMS 21/22 Main Menu".

**Re-support per SSMS release is the norm (V, supports IDEA-05).**
- SSMSBoost shipped builds for 22.5.2 (v5.5), 22.7.1 (v5.6) and 22.10.2 (v5.7) (S23).
- SSMS Tools Pack lists "22.7+" only, so earlier 22 builds are not supported (S27).
- VersionSQL 2.6.1 fixed "assembly version mismatches on SSMS 22.0 to 22.2" and dark-mode flyout items (S34).
- Redgate says 22.5 crashes older SQL Source Control, SQL Doc, SQL Data Generator and SQL Dependency Tracker builds (S44).
- SQL Shades needs 22.7+ for Query Store tooltips (S45 search summary, S).

## Feature inventories

### dbForge SQL Complete (hosted; includes the AI Assistant)
Feature list is from the vendor editions page (V, S7). The edition split (Standard, Professional, Enterprise, Express) was lost when the page was stripped to text, so **edition-per-feature is not captured**. Prices on S7: Enterprise 199.95, Professional 149.95, Standard 129.95 USD per subscription year, Express free.

| Group | Concrete features | How it likely works (inferred) |
|---|---|---|
| Completion | Context-aware suggestions for SELECT, DML, DROP, EXEC, SET, USE; JOIN ON suggestions and JOIN auto-generation; CTE and linked-server names; column picker; wildcard expansion; statement expansion for INSERT, EXEC, ALTER, UPDATE; auto table aliases; named regions; data-sensitivity-classification suggestions; exclude databases from suggestions | Replaces or sits beside SSMS IntelliSense using its own metadata cache of the connected database |
| Quick info | Hover tooltip with columns, types, nullability, estimated row count, keys, indexes, extended-property description; v2026.1 adds an indexed-column icon (V, S5) | Metadata queries on catalog views, shown in an editor adornment |
| Formatting | Wide set of formatting options (count not stated on the page), profiles, CamelCase, format on paste, `noformat` tag, bulk file and folder formatting, CLI formatter | Own parser and formatter, also runs outside SSMS (CLI) |
| Refactoring | Rename aliases, variables, objects with reference fix-up; add or remove brackets (new in 2026.1); semicolon insertion; find invalid objects (CLI too); smart rename | Own T-SQL parser plus catalog lookups |
| Code analysis | "180+ rules and best practices" (V, S6); customizable profiles | Static analysis on editor text |
| Debugger | T-SQL debugger with breakpoints, step, watch; vendor says it restores what Microsoft removed in SSMS 18 (V, S9). Not verified working on SSMS 22 | Debug engine on the SQL Server session; **claimed** for SSMS 22 |
| Productivity | Query history; execute current statement; execute to cursor; document outline; jump between queries; go to definition; execution warnings; execution notifications; transaction reminder; decrypt encrypted objects; CRUD generator; inline EXEC expansion; run script on multiple databases; settings export | Editor text, command interception before execute, the Messages and grid events after execute |
| Session safety | Restore last document session; restore last closed tab; tab, status bar and Object Explorer coloring; custom window title and tab captions; close unmodified tabs | Own persistence of document list and connections |
| Results grid | Find in grid; aggregates; copy as Excel or CSV; Generate Script As; copy headers with types; data visualizers; import selected data to files | Reads SSMS grid control state (inferred; compare SSMSBoost's "internal SSMS storage" statement) |
| AI Assistant | Chat with a database attached for schema context; NL to SQL; fix and explain errors from the Error List; optimize with index suggestions (v2026.1 sends index metadata); DDL compare; attach documents; custom instructions; web search | Vendor-hosted service |

**AI Assistant data handling (V, S8).** The docs say it is based on OpenAI, sees "only metadata" and not data, and stores only the conversation on the vendor server. It needs sign-in and is inactive without a trial, Express licence or purchase. This is the opposite of NoName's design: remote service, no local redaction step.

**Automation.** SQL Complete formatting and find-invalid-objects run from a command line (V, S7).

### Other dbForge add-ins (hosted unless noted)

| Add-in | Concrete features (V) | Mechanism | Relation to BRN-001 |
|---|---|---|---|
| Monitor (free) | Opens from Object Explorer right-click Monitor > Show Monitor, one window per instance. Tabs: Overview (CPU, memory, disk, waiting tasks, deadlocks, host and SQL properties), Data IO (read and write volume and latency per file), Databases (state, recovery model, size, collation), Wait Stats (type, duration, count), Top Queries (text and cost share), Sessions (database, user, status, wait, CPU, physical I/O, program), Last Month Backups. **"Does not log data"; real-time only.** No blocking tree or kill documented (S11) | DMVs and msdb (inferred from tab content) | Confirms IDEA-22 is buildable in SSMS 22. IDEA-22's blocking tree and audited kill go beyond it. No history is a gap IDEA-39 fills |
| Index Manager | Scan fragmentation across databases, maintenance recommendations, tunable thresholds, script generation, rebuild, export scan results, status tracking, CLI (S2) | `sys.dm_db_index_physical_stats` (inferred) | Confirms IDEA-24 (fragmentation part only; no usage or statistics review stated) |
| Search (free) | Objects and table data search across databases, bulk search, customizable, result management (S2, S21) | Catalog views and per-table scans (inferred) | Confirms IDEA-26 including data search |
| Unit Test | One-click tSQLt install; the install **adds stored procedures, functions and a SQL CLR object to the target database**; Test List Manager and Test Results tabs; CLI and PowerShell (S17) | Server-side objects plus a tool window | Evidence for the "server-side objects it installs" pattern; not a NoName topic |
| Source Control | Schema and static data to Git, SVN, TFVC, Mercurial, Perforce, Vault, Azure DevOps; dedicated or shared model; conflicts; history; rollback (S2, S18) | Object scripting plus VCS client | Confirms IDEA-31 for objects; server objects such as Agent jobs not stated here |
| Schema Compare, Data Compare | Compare databases, scripts folders, backups; sync; reports; CLI; both app and add-in (S2, S20) | Own diff engine | Native overlap: SSMS 22.10.2 made Schema Compare generally available (S42) |
| Data Generator, Documenter | Test data with 200+ generators; HTML and PDF documentation; both app and add-in (S2, S20) | Own engines | Out of NoName scope |
| Data Pump | Import from 10 and export to 14 formats, templates, CLI (S19) | Wizard plus bulk load | Out of scope |
| Event Profiler (app) | Profile Server Events wizard on its own Start Page; templates grouped Locks and Blocks, Query Execution, System Monitoring, Trace System plus Standard; needs ALTER TRACE, ALTER ANY EVENT SESSION, CONNECT SQL, VIEW SERVER STATE; Server Profile grid with filters (S14, S15) | Extended Events sessions on the server (inferred; template names such as "Query Batch Sampling" match SSMS's own XE session templates, inferred) | **No SSMS-hosted precedent**: it is an app. IDEA-23 would be first in the dbForge line to host it |
| SQL Decryptor (app) | Decrypts all module types, batch decrypt to one or many `.sql` files, decrypt in place, SSMS-like Object Explorer, "connect with no need to use the DAC mode" (S16) | Unknown; Devart says the DAC is not needed | **No in-SSMS precedent from this product.** SQL Complete lists "Decrypt encrypted objects" in its Productivity Extension (S7), mechanism unstated. IDEA-27's "over the DAC" wording conflicts with Devart's claim; verify |
| Query Builder (app) | Visual query diagrams, sub-queries, profiler, pivot tables (S2) | n/a | Out of scope |

### SSMSBoost 5.8 (hosted; Community free with limits, Professional perpetual)
Inventory is the vendor's full Features list (V, S24, S25, S26). Community limit confirmed on the page: preferred connections capped at 2. The 120-day refresh rule for free users is from a search summary only (S). Pricing was not verified.

| Area | Features | Mechanism evidence |
|---|---|---|
| Connections | Preferred connections with aliases, additional connection parameters, auto-connect Object Explorer at startup, auto-open query windows, quick connection switch toolbar drop-down, recent connections, set editor connection from Object Explorer, connection coloring of the status bar, "important database" alert text with tokens for server, database, user | "Tracks connection changes in real time" (V) |
| Safety | **Fatal Actions Guard**: parses scripts at execute time for DELETE or UPDATE without WHERE and for TRUNCATE; stop or ask; exclude tables; comment tokens that force Ask, force Prohibit, or unblock one script. **Transactions Guard**: floating reminder after a batch leaves an open transaction, closes at TRANCOUNT 0 | Transactions Guard "reuses native SSMS functionality" that returns only yes or no (V, S26): evidence an SSMS-internal open-transaction check exists |
| Editor | Select and run current statement; Columns Organizer expands `*`, sorts, adds aliases; Pivot Builder from grid values; Script Object (F2, go to definition); Locate Object in Object Explorer; Edit Top N rows from the editor; jump between brackets, BEGIN/END, INSERT column/value; regions; autoreplacements (snippets); find and replace templates; vertical guidelines; comment with block syntax; two formatters (one external command-line formatter bundled) | Uses SSMS command names: its macro feature chains `Query.ResultsToGrid` and `Query.Execute` (V) |
| History | Executed SQL history to a local folder with search; Recent Sessions and Workspaces (restore saved and unsaved documents with connections); recent tabs; SQL editor contents history (periodic snapshots, "infinite undo") | Disk files |
| Grid | Find in grid, find columns, aggregates, copy headers, copy cell 1:1, copy as SQL values list, script results, copy to Excel, run selected query as an Excel `.dqy`, visualizers for large text, XML, binary | Visualizers read "internal SSMS storage" so full cell values come out although the grid truncates (V, S26): evidence the grid's backing data is reachable |
| Navigation and tooling | Advanced Objects Search across connections with wildcards; search by token (Shift+F2); track current database; window title template; keyboard-shortcut manager; **Dump all SSMS commands and shortcuts**; macros over existing commands; post-execution handlers (run a command line or PowerShell after a long query, Outlook mail, tray notification); update checker | Command dump lists every registered command id, including other add-ins' (V) |

### SSMS Tools Pack 7.4.1.0 (hosted, commercial)
From S27, S28, S29 (V). Release 7.4 on 2026-10-14; 7.0 on 2026-01-20 states "full support for SSMS 21 and 22".
- **SQL History**: logs executed queries to local files or **a table in a database you specify by connection string**; window-content snapshots; restores saved tab sessions with connections; 7.4 restores the last tab session automatically at start.
- **SQL Editor Plus**: expand SELECT *, run SQL at cursor, Show Object Definition, F12 find object in Object Explorer, Find, Sort and Rename Tab, **Accidental Data Destruction Protector** (DELETE or UPDATE without WHERE, TRUNCATE, DROP TABLE, DROP DATABASE, possible open transaction; can ignore temp tables).
- **Search data** in tables and views across databases; **run script on multiple databases** with saved groups; search results in grid; column data statistics; script data generator (INSERT, JSON, CSV, WHERE clause); export to Excel without Excel installed.
- **Object Explorer**: custom folders by name conditions or regex, favorite connections, run custom scripts from Object Explorer nodes with keyword substitution, connection coloring, dependency viewer (FK map, new in 7.4).
- **Execution Plan Analyzer** appears on the Features list ("suggests possible improvements") but no detail page was found: **claimed**. Copy execution plans as an image is a separate feature.
- Deployment: PowerShell licence activation for organisation-wide rollout (V, relevant to a future NoName distribution story).

### SQL Sage 1.0.0 (hosted, SSMS 22 only, commercial; repo README read, S37)
This is the closest existing product to the NoName hub, and it is a shipping SSMS 22 VSIX. All statements below are **claimed** by the README; none were run.
- **AI through the user's own account**: drives the Claude Code CLI or the Codex CLI the user already signed into; no vendor key, no vendor-metered tokens. Agent mode (may run read-only queries and its own tools; writes ask first) and Ask mode (no tools).
- **Mask names**: replaces server, database, login, host and file names with stable placeholders (for example `<database2>`, `<login1>`) in tool results, conversation and system prompt. **Table and column names are not masked.** From 0.35.1, inline completions are masked and the real names are restored locally.
- **"What leaves your computer" chip** above the message box on every turn (for example "Schema + query to Claude"), plus a privacy panel. Query results are never sent without a per-session opt-in, and are not re-sent to another provider after a switch.
- **ScriptDom gate** classifies each statement; catalog and DMV reads run freely, DML and DDL stop for explicit confirmation, anything reaching linked servers, OPENQUERY, OPENROWSET or OPENDATASOURCE always stops.
- **Tamper-evident audit log**: local, hash-chained, with an evidence export; secrets replaced with `?`.
- **Free tier** (no AI): related tables after JOIN, object and text search across all databases plus "who writes and who reads this column" with a confidence label, **What's blocking now** (blocking chain including an idle session holding an open transaction; KILL and GRANT only as scripts in a new window), Check this database (read-only findings with what changed since last check), catalog-proven Explain error, F5 warnings, Format SQL (uses SSMS's own formatter when present), Expand `*`, encrypted local query history with "recently closed unsaved" tabs.
- **Licensed**: Change-Impact before ALTER or DROP, Incident Mode (blocking, top waits, expensive queries, each claim cited to a DMV number), Prove-It (multiset fingerprint proves an AI rewrite returns the same rows), a card that links a history entry to Query Store plans and run times.
- **Coexistence behaviour**: stays off features where SQL Prompt or SQL Complete is installed and yields classic shortcuts to other add-ins (README).
- Price in README: from 39 USD one-time or 29 USD per year (**claimed**).

### SQLTreeo (S33)
**Virtual Folders for SSMS** (hosted, SSMS 22, release notes read): folders in Object Explorer for databases, tables, views, procedures, logins and Agent jobs, matched by name; dynamic filter box in the tree (`;` for several criteria, `%` and `_` wildcards); built-in Agent folders for disabled, replication and reporting jobs; right-click menus: row count and size per table, database and log sizes, **missing indexes in a new connected query window with ready CREATE INDEX text**, object created and modified dates, parameters, calling Agent jobs, role members, login folders (disabled, SQL, Windows); **Job details and Job overview windows showing overlapping schedules and run times with a suggested new start time** (22.0.0.125, 2026-11-01). Folder definitions persist in a vendor-hosted free service under the licence key, or in one table on a SQL Server the user picks. The zip variant is extracted into the SSMS Extensions folder; the MSI is also offered; there is also a Microsoft Store listing.

**Job Timeline** is a standalone Windows desktop app, not an SSMS add-in: read-only SELECTs on `msdb`, no server objects, per-job baseline (median, 95th percentile, longest, trend), Rhythm view folding a week onto 24 hours, problems list with the failing step and first message line, multi-server estate page (S33). It is the IDEA-28 reference; no source states it runs inside SSMS.

### VersionSQL 2.7 (hosted, S34)
Commits schema, security objects (users, roles, permissions, application roles, ownership), Agent jobs and static data to Git, Subversion or TFVC from an Object Explorer right-click; Schema Compare and Deploy from a revision, folder or database (2.7, 2026-09-25); undo or deployment script from any revision; a "Redgate compatibility preset"; "no background scanning, works when you ask it to" (V). Supports IDEA-31, and its scope (Agent jobs, security) is wider than dbForge Source Control's stated list.

### SQL Enlight (hosted, S32)
Rules-based T-SQL analysis ("260+" rules for SonarQube, "300+" on the features page: the two numbers disagree), custom rules, quality gates, formatting, refactoring, CLI with MSBuild tasks and an automation API, Azure DevOps, Jenkins, Docker, SonarQube plugin (V). Suppression by comment and a 14-day trial come from a 2013 review (S, old). Confirms IDEA-25 mechanics; SSMS 22 build unknown.

### SolarWinds Plan Explorer SSMS plugin (S30, S31)
The viewer is a free standalone .NET app. The SSMS side is: right-click a graphical plan, "View with SQL Sentry Plan Explorer", which opens the plan with server and database context; plan XML copied from "Show Execution Plan XML" is auto-detected on the clipboard. The SSMS 22 plugin is a **separate VSIX named for Visual Studio 2026** (`PlanExplorer.SsmsIntegration.VS2026`), installed after SSMS 22, with an uninstall by VSIXInstaller id (V). Whether a free-tool user can obtain it without a Customer ID is disputed in forum posts (S, unverified). Mechanism: plan context-menu hook plus a separate process: class C viewer with an A-class launcher.

### Smaller hosted tools (V unless noted)
- **SQL Guard**: blocks 9 destructive patterns at F5 with a target confirmation; free plan; says "no network calls" (S38).
- **SqlSmash**: search all object text, references of the object under the cursor, recent tabs and executed queries, data from foreign-key relationships of grid rows, expand selection to logical parent, hierarchical script outline, run query at cursor, warning for UPDATE or DELETE without WHERE; free community version; **no SSMS 22 statement** (S39).
- **RenSQL**: saves each executed statement into a searchable library (SSMS 21 and 22) (S40).
- **SQL Lizard**: grid with pivot tables, charts, dashboards, exports, linting, notes, history (SSMS 22) (S40).
- **SQL Shades**: themes and animated "scenes"; its FAQ claims SSMS's own dark mode misses Object Explorer, while a 2026-04 review says dark mode is consistent across editor, Object Explorer and grid (S35 vs S45). Check on the owner's host before relying on either.
- **Tabs Studio**: tab titles from document content or comments, SSMS 22 and 21 (S40). **SQLinForm**: formatter plug-in with 180-plus options (S40).

## Capability matrix

Columns: capability, products, SSMS status, API surface it needs, class (A hosted and vendor-stated SSMS 22; B waits on a probe; C outside SSMS), relation to BRN-001, evidence. "Ships" means the vendor says so (V); nothing here was run on the owner's host. API surfaces: **ED** editor text and commands, **OE** Object Explorer, **GRID** results grid, **SQL** T-SQL and DMVs, **SRV** server-side objects, **PROC** separate process.

| # | Capability | Products | Ships in | API surface | Class | Relation | Evidence |
|---|---|---|---|---|---|---|---|
| 1 | Pre-execute warning for DELETE or UPDATE without WHERE, TRUNCATE, DROP | SSMSBoost Fatal Actions Guard; SSMS Tools Pack Protector; SQL Complete execution warnings; SQL Guard; SqlSmash; SQL Sage | 22 (all V) | ED (intercept execute, parse text) | A | Confirms IDEA-30. Extends IDEA-16: SSMSBoost's comment tokens (Ask, Prohibit, Unblock) are a per-script arming model | S23, S26, S29, S7, S38, S37 |
| 2 | Open-transaction reminder after a batch | SSMSBoost Transactions Guard; SQL Complete; Tools Pack Protector | 22 | ED plus an SSMS-internal check (SSMSBoost says it reuses native yes or no) | A | Confirms IDEA-30 open-transaction watch | S26, S7, S29 |
| 3 | Session restore, recently closed tabs, window snapshots | SSMSBoost; Tools Pack; SQL Complete; SQL Sage; RenSQL | 22 | ED (document list, connections), files | A | Confirms IDEA-30 query journal and restore | S26, S28, S7, S37 |
| 4 | Executed-statement history, searchable | SSMSBoost; Tools Pack (files or a user-chosen database table); SQL Complete; SQL Sage (DPAPI-encrypted) | 22 | ED/GRID events after execute, local files, optional table | A | Confirms IDEA-30. Tools Pack's database-table option is a precedent for IDEA-39-style history | S26, S29, S37 |
| 5 | Expand `*` to columns | SSMSBoost; Tools Pack; SQL Complete; SQL Sage | 22 | ED plus SQL (catalog) | A | Confirms IDEA-30 | S26, S29, S7, S37 |
| 6 | Go to definition, show object script | SSMSBoost F2; SQL Complete; Tools Pack; SqlSmash | 22 | ED plus SQL (`OBJECT_DEFINITION`) | A | Confirms IDEA-30 | S25, S29, S7 |
| 7 | Script outline, statement at caret, execute current statement | SQL Complete; SqlSmash; SSMSBoost | 22 | ED (parse) | A | Confirms IDEA-30 | S7, S39, S25 |
| 8 | Grid helpers: find, aggregates, copy headers, copy as INSERT or JSON, visualizers | SSMSBoost; Tools Pack; SQL Complete; SQL Lizard | 22 | GRID (SSMSBoost reads "internal SSMS storage" for full cell values) | A | Confirms IDEA-30 grid helpers; the internal-storage claim is the probe target for IDEA-03 and IDEA-49 | S26, S29, S7 |
| 9 | Object and metadata search, multi-database | dbForge Search; SSMSBoost Advanced Search; Tools Pack; SQL Sage Search; SqlSmash | 22 | SQL (catalog views), tool window | A | Confirms IDEA-26 | S21, S26, S29, S37 |
| 10 | Data search across tables | dbForge Search; Tools Pack "Search data" | 22 | SQL (scans) | A | Confirms IDEA-26 data search; both are plain features, neither is described as audited | S21, S29 |
| 11 | "Who writes and who reads this column", with confidence label | SQL Sage | 22 | SQL (dependency views) | A | New candidate (C7 below); extends IDEA-26 | S37 |
| 12 | Live monitor: waits, sessions, top queries, file I/O, backups | dbForge Monitor | 22 (bundle page) | OE context menu, tool window, SQL (DMVs, msdb) | A | Confirms IDEA-22; **no logging**, no blocking tree, no kill | S11, S13, S3 |
| 13 | Blocking chain incl. idle session holding a transaction; KILL as script only | SQL Sage "What's blocking now", Incident Mode | 22 | SQL (DMVs) | A | Extends IDEA-22 (design contrast: script, not execute) | S37 |
| 14 | Index fragmentation scan and rebuild scripts | dbForge Index Manager | 22 | SQL (index physical stats), tool window | A | Confirms IDEA-24 (fragmentation only) | S2, S10 |
| 15 | Missing indexes from an Object Explorer right-click into a connected query window | SQLTreeo Virtual Folders | 22 | OE context menu, SQL (missing-index DMVs), new editor window | A | Extends IDEA-24 and IDEA-49 entry-point pattern | S33 |
| 16 | Agent job overlap, suggested new start time | SQLTreeo Virtual Folders (hosted); Job Timeline (app) | 22 (hosted part) | OE, SQL (`msdb`) | A for overlap view; C for the timeline app | Confirms IDEA-28 inside SSMS 22; Job Timeline is standalone | S33 |
| 17 | Per-job duration baseline (median, 95th percentile), failing step message | SQLTreeo Job Timeline | app only | SQL (`msdb`), separate process | C | Extends IDEA-28 | S33 |
| 18 | Extended Events capture templates | dbForge Event Profiler (app) | app only; SSMS has its own XEvent Profiler | SQL (XE DDL), separate process | C precedent | IDEA-23 has no hosted competitor | S14, S15, S42 |
| 19 | Decrypt WITH ENCRYPTION modules | SQL Decryptor (app); SQL Complete "Decrypt encrypted objects" | app; hosted claimed | SQL (mechanism unstated; Devart says DAC not needed) | C for the app; A claimed for SQL Complete | IDEA-27 buildable in principle; the DAC premise needs checking | S16, S7 |
| 20 | T-SQL static analysis with rules | SQL Complete (180+); SQL Enlight (260-300+, custom rules, CLI) | 22 (SQL Complete); Enlight build unknown | ED (parse) | A (Complete); unverified (Enlight) | Confirms IDEA-25; Enlight adds CLI/CI | S6, S32 |
| 21 | Source control of objects | dbForge Source Control; VersionSQL (adds security, Agent jobs, static data) | 22 | OE context menu, scripting, VCS client | A | Confirms IDEA-31. Native partial overlap: Database DevOps GA in 22.10.2 | S18, S34, S42 |
| 22 | AI chat in SSMS on a vendor service | dbForge AI Assistant (OpenAI, metadata only) | 22 | ED, tool window, remote service | A | New context for IDEA-01/02: the vendor-hosted alternative | S8 |
| 23 | AI in SSMS on the user's own account via CLI | SQL Sage | 22 | tool window, PROC (CLI subprocess) | A | Overlaps IDEA-01/02/06. Different: NoName is copy-paste, SQL Sage drives the CLI | S37 |
| 24 | Name masking before AI | SQL Sage "Mask names" | 22 | ED plus SQL (knows server and database names) | A | **Confirms IDEA-01, 04, 08.** Gap: table and column names unmasked; no secret or data-value redaction described | S37 |
| 25 | Per-turn "what leaves your computer" indicator | SQL Sage | 22 | tool window | A | Confirms IDEA-07 and IDEA-14 intent | S37 |
| 26 | ScriptDom statement classification gate | SQL Sage | 22 | ED plus ScriptDom (late-bound) | A | Confirms IDEA-08 and IDEA-16 are feasible | S37 |
| 27 | Hash-chained local audit log with evidence export | SQL Sage | 22 | files | A (client side) | Extends IDEA-32 (client-side counterpart) | S37 |
| 28 | Plan hand-off to an external viewer | Plan Explorer plugin | 22 (separate VSIX) | plan context menu, clipboard, PROC | A launcher, C viewer | Confirms IDEA-42 repro/hand-off is possible; SSMS 22.x also opens plans in a new tab natively | S30, S42 |
| 29 | Query Store link from a history entry; Query Store reports | SQL Sage (licensed); SSMS native reports | 22 | SQL (Query Store views) | A | Partly confirms IDEA-41; native reports already exist | S37, S42 |
| 30 | Post-execution handlers (command line, mail, tray) | SSMSBoost | 22 | ED/events, PROC | A | New candidate C3 | S26 |
| 31 | Run one script on several databases of the instance | Tools Pack (saved groups); SQL Complete | 22 | ED, SQL | A | New candidate C4; fits the single-instance scope | S29, S7 |
| 32 | Command dump (list all registered SSMS commands) | SSMSBoost | 22 | DTE/command table | A | Supports IDEA-05: a ready-made probe for the canary | S26 |
| 33 | Object Explorer custom folders and context items | SQLTreeo; Tools Pack; (OSS: SQL Medic, in S43) | 22 | OE tree and context menu | A | Shows Object Explorer is extensible in 22; IDEA-49 entry point | S33, S29 |
| 34 | Tab rename, color, sort | Tabs Studio; Tools Pack; SQL Complete | 22 | ED/tab strip | A | Already native (see Things to avoid) | S40, S29, S42 |
| 35 | Formatter | SQL Complete; SQLinForm; Tools Pack; SSMSBoost | 22 | ED | A | Already native since 22.10.2 | S7, S42 |

## Candidate ideas for NoName

Each is new or extends a numbered idea. Evidence is vendor-stated; the probe column says what NoName would verify in its own diagnostic extension first.

| ID | Candidate | Extends | Evidence it is buildable in SSMS 22 | Probe or server-side piece needed |
|---|---|---|---|---|
| C1 | **Outbound-content chip.** Show on every prompt what will leave (counts of masked names, secrets, rows) before the user copies it | IDEA-07, 14 | SQL Sage ships a per-turn chip in an SSMS 22 panel (S37) | None beyond a tool window; NoName's version is a preview, not a send |
| C2 | **Redaction scope beyond names.** SQL Sage masks only server, database, login, host and file names. NoName's design adds secrets, infrastructure names and selected data | IDEA-01, 04, 08 | Name masking with stable placeholders is proven (S37) | Server and connection names from the connection object; ScriptDom for identifiers (PRB for ScriptDom late-binding already in scope) |
| C3 | **Armed-run markers in the script.** Comment tokens that force confirm, forbid, or lift the guard for one script | IDEA-16, 30 | SSMSBoost Fatal Actions Guard uses Ask, Prohibit and Unblock tokens (S26) | Probe: intercept the execute command before it runs |
| C4 | **Post-execution handler for long runs** (notify, save results, run a command) | IDEA-30 | SSMSBoost handlers (S26) | Probe: execution-finished event and the Messages text |
| C5 | **Run on several databases of this instance**, with saved groups | IDEA-30 | Tools Pack and SQL Complete (S29, S7) | Editor text plus per-database connection switching |
| C6 | **Agent job overlap with suggested start time and per-job baseline.** Hosted overlap view exists; the baseline view is app-only | IDEA-28 | SQLTreeo Virtual Folders 22.0.0.125 and Job Timeline (S33) | SQL on `msdb` only; no server objects |
| C7 | **Object Explorer right-click as the entry point for per-object work**: build prompt for this procedure, missing indexes, calling jobs, dependents | IDEA-49, 24, 26 | Virtual Folders does it on SSMS 22 (S33); SQL Sage adds a read/write column map with confidence labels (S37) | Probe: Object Explorer context-menu registration on SSMS 22.11 |
| C8 | **Blocking view with the idle-open-transaction culprit, KILL as a reviewed script** | IDEA-22 | SQL Sage (S37). dbForge Monitor shows sessions but no blocking tree (S11) | SQL on DMVs; IDEA-22's audited kill would execute, so it needs the IDEA-32 audit |
| C9 | **Monitor with history.** dbForge Monitor states it does not log data | IDEA-22, 39 | Monitor's tab set shows a DMV scope that fits the light-by-design polling budget of IDEA-40 (S11) | History database filled by Agent jobs (class C piece) |
| C10 | **Client-side evidence export** (hash chain over what the tool ran or blocked) | IDEA-32 | SQL Sage (S37) | Local files only |
| C11 | **Coexistence rules.** Yield shortcuts and turn off overlapping features when another add-in is present | IDEA-05, 40 | SQL Sage does this against SQL Prompt and SQL Complete (S37); SSMSBoost lists shortcuts via its command dump (S26) | Probe: enumerate commands and key bindings (SSMSBoost's dump shows it is possible) |
| C12 | **Dependency (foreign-key) map for a table** | IDEA-34, 47 | Tools Pack 7.4 (S29) | Catalog views |

**Design contrasts worth recording.**
- SQL Sage's hosted-service-free approach (user's own CLI sign-in) and Virtual Folders' vendor-hosted settings store both show the market splits on where state lives. NoName's C-class history database on the instance is a third option.
- No product I read redacts secrets or data values before a prompt leaves the machine; SQL Sage is the only one with any redaction (S37). The OSS catalog (S43) was not searched for others; that is the OSS track's job.

## Things to avoid

**Already native in SSMS 22 (M, S42).**
- **SQL Formatter**: generally available in 22.10.2 (2026-09-28), with many settings. Do not build a formatter.
- **Schema Compare**: generally available in 22.10.2.
- **Database DevOps** (SQL database projects and source control): generally available in 22.10.2. This narrows IDEA-31 to what projects do not cover.
- **GitHub Copilot in SSMS**: generally available in 22.4.1 (2026-03-18); Agent Mode generally available in 22.10.2 with a **bundled SQL Tools MCP server** (version 3.0.17). This bears on IDEA-06: an MCP channel to SQL Server already ships with SSMS when the Copilot component is installed.
- **Query Hint Recommendation Tool**: generally available in 22.4.1. Also native: Query Store reports, Activity Monitor, Extended Events Profiler and viewer, Database Engine Tuning Advisor, execution plan in a new tab, rename tabs.
- **Tab rename and color-by-extension**: native per a 2026-04 review (S45, S) and the release notes' Rename Tabs dialog entry (S42). Tabs Studio, Tools Pack and SQL Complete tab features overlap.
- **Per-connection status-bar color** exists natively but with one color for all connections, which is the gap SSMSBoost and Tools Pack fill (S22).

**Shipped only in old SSMS hosts, no SSMS 21/22 successor found.**
- Quest Spotlight SSMS extensions and plug-ins appear only in old support articles (S48, S). Treat as unavailable.
- Tools Pack "Debug sections" works on SSMS 2005 and 2008 only (S29).
- SSMSBoost 5.8 is the last build for SSMS 2008 and 2008 R2 (S23).

**Aspirational or unverified.**
- SQL Complete's T-SQL debugger on SSMS 22 is claimed (S9) but was not shown running.
- SQL Enlight's SSMS 22 support rests on one requirements list; its own product table stops at 21 (S32).
- SqlSmash states no SSMS 22 support (S39).
- Tools Pack "Execution Plan Analyzer" has no detail page (S29).
- Every hosted feature above is vendor-claimed. None replaces NoName's own diagnostic probes.

## Open questions and unverifiable claims

1. **Mechanisms.** No vendor documents how it hooks the execute command, the grid or the open-transaction check. The statements closest to mechanism are SSMSBoost's "internal SSMS storage", its reuse of a native yes-or-no transaction check, and its macros built from `Query.*` command names. Everything else in the "how it likely works" columns is inferred.
2. **Idera.** Product pages (S46) are marketing shells for SQL Diagnostic Manager 14.3; I found no SSMS plug-in for SQL Doctor or SQL Admin Toolset. An unverified search summary says SQL Diagnostic Manager 14.0 added AI-assisted optimization with copy-paste output (S48).
3. **Quest Spotlight and SQL Sentry.** No current SSMS element found beyond the Plan Explorer plugin. The Customer Portal gate on that plugin comes from forum posts (S48) and was not tested.
4. **Minion Enterprise.** Vendor pages render as empty shells; no add-in evidence.
5. **Per-product Devart requirement pages** for Monitor and Event Profiler returned not-found; their SSMS status rests on the bundle page (S3). A reseller article dated 2026-02-26 says the bundle update added SSMS 22 support (S48, S).
6. **SSMS 22.6 and later.** A reseller changelog summary says SQL Complete v2026.1.39 added SSMS 22.6 support (S48, S). Not read on a Devart page.
7. **Decryption without the DAC.** Devart says its app connects "with no need to use the DAC mode" (S16). How, and whether the in-SSMS SQL Complete command does the same, is unstated.
8. **Edition limits.** SQL Complete's edition-per-feature split was not captured (see above). SSMSBoost Pro-only features beyond the 2-connection cap were not captured; the page lists only "some differences" (S25).
9. **Theming.** SQL Shades says Object Explorer is not themed by Visual Studio's own dark setting; a review says SSMS 22 now themes it consistently (S35 vs S45). The owner's note on VS 2026 theme tokens may settle this.
10. **Prices and licences** were not verified except where shown with a source. Do not use them.

## Sources

All read 2026-11-04. Labels V/M/S as defined above.

- S1 · https://www.devart.com/dbforge/sql/sql-tools/ · V · 15-tool bundle, AI Assistant overview.
- S2 · https://www.devart.com/dbforge/sql/sql-tools/editions.html · V · per-tool integration (ssms, app, vs), feature rows, bundle prices.
- S3 · https://docs.devart.com/sql-tools/requirements-and-compatibility/compatibility.html · V · bundle supports SSMS v17 to v22 and 2016, Visual Studio 2026.
- S4 · https://docs.devart.com/sqlcomplete/release-notes-and-other-versions/release-notes-v2025-3.html · V · SQL Complete adds SSMS 22 and Visual Studio 2026.
- S5 · https://docs.devart.com/sqlcomplete/release-notes-and-other-versions/release-notes-v2026-1.html · V · brackets, index icon, index metadata for AI.
- S6 · https://www.devart.com/dbforge/sql/sqlcomplete/ · V · SQL Complete overview, 180+ rules.
- S7 · https://www.devart.com/dbforge/sql/sqlcomplete/editions.html · V · "SQL Server Management Studio 21 and 22", full feature list, prices.
- S8 · https://docs.devart.com/sqlcomplete/ai-assistant/ · V · AI Assistant features, OpenAI, metadata only, SSMS 21 menu note.
- S9 · https://docs.devart.com/sqlcomplete/sql-refactoring/debugger-overview.html · V · T-SQL debugger claim.
- S10 · https://docs.devart.com/{index-manager,search,unit-test,source-control,schema-compare-for-sql-server,data-compare-for-sql-server,data-pump,data-generator-for-sql-server,documenter-for-sql-server,sqlcomplete}/getting-started/requirements.html · V · each lists SSMS v22 down to 2016.
- S11 · https://docs.devart.com/monitor/monitoring-sql-performance/collect-performance-and-system-information.html and https://docs.devart.com/monitor/getting-started/database-performance-data.html · V · Monitor tabs, "does not log data".
- S13 · https://www.devart.com/dbforge/sql/monitor/ · V · Monitor FAQ, multi-instance via Object Explorer.
- S14 · https://docs.devart.com/event-profiler-for-sql-server/profiling-server-events/capture-server-events.html · V · permissions, wizard, Start Page.
- S15 · https://docs.devart.com/event-profiler-for-sql-server/profiling-server-events/set-event-sources.html · V · templates.
- S16 · https://www.devart.com/dbforge/sql/sqldecryptor/ · V · standalone, no DAC.
- S17 · https://www.devart.com/dbforge/sql/unit-test/ · V · tSQLt install adds objects and a CLR object.
- S18 · https://www.devart.com/dbforge/sql/source-control/ · V · version-control systems.
- S19 · https://www.devart.com/dbforge/sql/data-pump/ · V · formats.
- S20 · https://www.devart.com/dbforge/sql/{schemacompare,datacompare,documenter,data-generator,querybuilder}/ · V · app versus add-in.
- S21 · https://www.devart.com/dbforge/sql/search/ · V · object and data search.
- S22 · https://www.ssmsboost.com/ · V · supported SSMS list 2012 to 22, connection features.
- S23 · https://www.ssmsboost.com/Changelog · V · per-release SSMS 22 support, menu quirk.
- S24 · https://www.ssmsboost.com/Licenses · V · v5.8.9789, 2026-10-20, feature list.
- S25 · https://www.ssmsboost.com/VersionCompare · V · Community versus Professional table.
- S26 · https://www.ssmsboost.com/Features/ssms-add-in-{fatal-actions-guard,transactions-guard,execution-post-handling,results-grid-visualizers-feature,simple-macros,commands-dump,advanced-objects-search,recent-sessions,executed-sql-statements-logging-history,sql-editor-contents-history,columns-organizer} · V · mechanism statements.
- S27 · https://www.ssmstoolspack.com/ · V · 7.4.1.0, "22.7+".
- S28 · https://www.ssmstoolspack.com/Changelog · V · 7.0 full SSMS 21 and 22 support.
- S29 · https://www.ssmstoolspack.com/Features and ?f=3,5,6,14,18,19,21 · V · feature detail.
- S30 · https://documentation.solarwinds.com/en/success_center/sqlsentry/content/planexplorer/installation-overview.htm · V · SSMS 22 plugin VSIX, plan hand-off.
- S31 · https://www.solarwinds.com/free-tools/plan-explorer · V · free tool, SSMS add-in, SQL Sentry link.
- S32 · https://sqlenlight.com/assets/SystemRequirementsPage-ZOVqwe5K.js and https://sqlenlight.com/assets/index-EhcORvql.js · V · SSMS 17 to 22 list, product table to 21, rule counts.
- S33 · https://www.sqltreeo.com/ plus /downloads, /releases, /virtual-folders, /job-timeline, /sqldebugger · V · Virtual Folders SSMS 22 builds, Job Timeline standalone.
- S34 · https://www.versionsql.com/ and /changelog/ · V · 2.7, SSMS 22 fixes, scope.
- S35 · https://www.sqlshades.com/ · V · SSMS 18 to 22, theming FAQ.
- S37 · https://github.com/LUMASoftPL/sqlsage (README raw) · V (claims) · SQL Sage features.
- S38 · https://sqlguard.app/ · V · destructive-statement guard.
- S39 · https://sqlsmash.com · V · feature list, 2023 copyright.
- S40 · https://tabsstudio.com ; https://rowdyne.com/rensql/ ; https://sqllizard.com ; https://www.sqlinform.com/ssms-sql-formatter/ · V · SSMS support statements.
- S41 · https://learn.microsoft.com/en-us/ssms/faq · M · third-party extensions unsupported, not blocked.
- S42 · https://learn.microsoft.com/en-us/ssms/release-notes-22 · M · 22.11.3 build, native features and GA dates.
- S43 · https://erikej.github.io/SsmsExtensions/ · S · community catalog of 52 SSMS 22 extensions, used to find products.
- S44 · https://documentation.red-gate.com/sp/troubleshooting/common-issues/ssms-22-5-compatibility · V · 22.5 incompatibilities.
- S45 · https://www.sqlfingers.com/2026/04/ssms-22-whats-different-whats-worth-it.html · S · blog review, tabs, dark mode, extensions.
- S46 · https://www.idera.com/products/sql-diagnostic-manager-for-sql-server/ , /sql-doctor/ , /sql-admin-toolset/ · V · marketing shells, 14.3.
- S47 · https://minionware.net/ and /products/minion-enterprise/ · V · empty shells.
- S48 · Web-search summaries, unread pages · S · ComponentSource news (2026-02-26) and release changelog for SQL Complete 22.6 support; THWACK posts on the Plan Explorer plugin; Quest support articles on old Spotlight plug-ins; Simple Talk 2013 review of SQL Enlight; Devart MCP server on NuGet (not an SSMS add-in).
