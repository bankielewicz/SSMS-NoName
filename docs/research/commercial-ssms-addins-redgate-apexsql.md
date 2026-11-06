# Commercial SSMS add-ins: Redgate and Quest (ApexSQL)

Research track 1 of 4 for NoName. Written 2026-11-04 by a Sonnet 5.5 research subagent at the owner's request. Evidence, not decisions: nothing here is a requirement until the brainstorm or PRD takes it up.

## Scope and method

Question: which features of commercial SSMS add-ins from Redgate and from Quest (the ApexSQL line) are real, shipping, and buildable inside an SSMS 22 extension today, and which of the vendor's products still install into SSMS 21 or 22.

Read date for every row below is **2026-11-04** unless a row says otherwise.

**Evidence tiers.** The task asks to mark anything not verified on a current vendor page as *claimed*. Three tiers are used:

| Tier | Meaning | Marker |
|---|---|---|
| T1 | Page fetched and read in full or in the relevant part on the read date, on a vendor domain (red-gate.com, documentation.red-gate.com, blog.quest.com) | none |
| T2 | Fact came only from a web-search summary of a vendor page that I could not fetch (support.quest.com and apexsql.com refuse automated fetches: HTTP 202/403) | **claimed** |
| T3 | Forum posts, community replies, third-party lists, reseller pages | **claimed (T3)** |

**Quest pages could not be fetched.** support.quest.com, quest.com and apexsql.com return a bot challenge or 403 to automated readers. I did not work around that. Almost every Quest statement below is therefore T2 or T3 and **claimed**. Redgate is almost entirely T1.

**What "class" means in the matrix.** "A (vendor-proven)" is this track's own label: a shipping SSMS 22 add-in does it, by the vendor's own page. It is not BRN-001's class A, which is "inside SSMS, proven" (BRN-001 section 1); the [synthesis](synthesis-ssms-extension-market-and-capabilities.md), section 2, maps this label to B+. The vendor may use internal SSMS APIs, so NoName's own VSIX still needs its own probe before relying on the same code path.

**Fetch problems in this session.** Three sub-agents of type web-fetch and one general-purpose sub-agent returned no reports (two retry rounds). All Redgate facts below were read directly with curl on public Redgate pages, and Quest facts came from web-search summaries.

## Vendor status in SSMS 22 and 21

Legend: hosted = add-in code runs in the SSMS process (menus, tool windows, editor, Object Explorer, grid). standalone = separate application, possibly launched from SSMS.

### Redgate (T1 unless marked)

| Product | Hosted in SSMS or standalone | SSMS 22 | SSMS 21 | 18/19 only | Source URL |
|---|---|---|---|---|---|
| SQL Prompt 11.5 (current 11.5.7, 2026-10-27) | Hosted add-in: menus, query editor, Object Explorer and Registered Servers context menus, results-grid context menu, dockable tool windows, toolbar button, command palette | Yes, since 11.2.0 (2025-11-21), with a stream of 22.x fixes (see timeline) | Yes, since 11.0.0 (2025-05-28) | No. Product page lists SSMS 22, 21, 20, 19, 18, 2017 and Visual Studio 2026, 2022, 2019 | https://documentation.red-gate.com/sp/release-notes-and-other-versions ; https://www.red-gate.com/products/sql-prompt/ |
| Redgate Assistant (preview, inside SQL Prompt >= 11.4.0, 2026-07-01) | Hosted dockable chat panel (WebView2) | Yes (docs: "all supported SSMS versions (SSMS18+)"; Visual Studio not supported) | Yes | No | https://documentation.red-gate.com/sp/ai-in-sql-prompt/prompt-ai-features/redgate-assistant-preview |
| SQL Search 3.x (docs home shows 3.10) | Hosted add-in, free | Yes, 3.9.0 (2025-11-21); 3.9.5 fixed the 22.3 crash. Requirements page lists SSMS 22 to 18 (18.0-18.4 excluded), Visual Studio 2026/2022/2019; ARM64 not supported | Yes | No | https://documentation.red-gate.com/ss/requirements ; https://documentation.red-gate.com/ss (T1); release-note detail for 3.9.x is **claimed** (search summary) |
| SQL Source Control 8.1 (8.1.22 on 2026-08-18) | Hosted add-in in Object Explorer; uses the SQL Compare engine | Yes, 8.1.0 (2025-11-21); TFS fix 8.1.2; 22.5 fixes (versions <= 8.1.11 crash SSMS 22.5) | Yes | No. Docs: SSMS 18 to 22; SSMS 2017 and older unsupported | https://documentation.red-gate.com/soc/requirements-and-supported-systems ; https://documentation.red-gate.com/soc/release-notes-and-other-versions/sql-source-control-8-1-release-notes |
| SQL Test 4.4.3+ | Hosted add-in (tSQLt runner) | Yes, needs 4.4.3 on SSMS >= 22.3; 4.4.2 or lower crashes 22.3 | Yes | No | https://documentation.red-gate.com/sqt/requirements ; https://documentation.red-gate.com/sqt/troubleshooting/sql-test-compatibility-update-for-ssms-22-3 |
| SQL Doc 5, SQL Dependency Tracker 3, SQL Data Generator 4 | Launched from Object Explorer right-click ("from within SSMS"). Whether the main window is a separate process is not stated: **claimed** | Listed as compatible with SSMS 22 on the Toolbelt roadmap and with 22.3 in the support post. For 22.5 need SQL Doc >= 5.5.6, Dependency Tracker >= 3.6.5, Data Generator >= 4.8.6 (older ones "may result in SSMS crash") | Redgate says SSMS 21 and earlier were unaffected by the 22.3 break; explicit 21 support not stated | Not stated | https://productsupport.red-gate.com/hc/en-us/community/posts/34092306272029 ; https://documentation.red-gate.com/sp/troubleshooting/common-issues/ssms-22-5-compatibility |
| SQL Compare 16 and SQL Data Compare 16 | Standalone WPF-style application plus a free "SSMS Integration Pack" add-in (Object Explorer "Compare schema to" opens the Schema and Data Compare tab, then launches the app). The add-in ships in the Toolbelt Essentials installer | **Not confirmed on any vendor page read.** A December 2025 blog said SSMS 22 integration was "coming soon". The Toolbelt roadmap says only "You can also open SQL Compare and SQL Data Compare scripts in SSMS 22" (script hand-off, not the add-in). Release notes 16.0 and 16.1 contain no SSMS 22 add-in statement | Add-in status for 21 not confirmed; 16.0 notes mention "SSMS 21 as an external SQL Editor" (script hand-off) | Add-in docs page still describes the OE flow without a version | https://documentation.red-gate.com/sc/sql-server-management-studio-add-in/getting-started-with-the-add-in ; https://www.red-gate.com/blog/introducing-sql-compare-sql-data-compare-v16-more-future-ready-more-secure/ ; https://www.red-gate.com/products/sql-toolbelt-essentials/roadmap/ |
| SQL Multi Script | Not verified (product page text gave no SSMS statement) | Not stated | Not stated | Not stated | https://www.red-gate.com/products/sql-toolbelt-essentials/ |
| SQL Change Automation 5 | SSMS add-in requirements page exists (updated 2025-12-12) but its SSMS version wording is garbled ("SQL Server Management Studio 2022 which includes ARM64 Support"). Retirement status not checked | Unclear | Unclear | Unclear | https://documentation.red-gate.com/sca/getting-started/system-requirements/ssms-add-in-requirements |
| Redgate Monitor 14.x | Web application on a server. No SSMS add-in found on vendor pages read ("none found", not proof of absence). Redgate Assistant also exists inside Monitor (v14.27+) | n/a | n/a | n/a | https://www.red-gate.com/products/redgate-assistant/ |

Licence context (T1 product pages): SQL Prompt is a $210 per user per year subscription, SQL Toolbelt Essentials $1,410 per user per year; Redgate Assistant and Prompt AI need a subscription (perpetual licences excluded); SQL Search is free.

### Quest and ApexSQL (all rows **claimed** unless marked T1)

Quest told ApexSQL customers its add-in line is ending and offers **Toad for SQL Server** (a standalone application, see below) as the free migration. No Quest page I could read states SSMS 21 or SSMS 22 support for any ApexSQL add-in. The newest dated release lines I saw are 2022.01 and 2022.02 for Complete, Refactor, Search, Diff, Data Diff, Propagate (a product-support page lists them), which predates SSMS 21 (2025). The only dated community statement says ApexSQL add-ins did not yet support SSMS 19 or Visual Studio 2022 and "dev is working on" a version.

| Product | Hosted or standalone | Status found | SSMS 22 | SSMS 21 | Source URL (read date 2026-11-04; fetch refused, summary only) |
|---|---|---|---|---|---|
| ApexSQL Complete | Hosted add-in (code completion in SSMS and Visual Studio); FAQ undated: "integrates with SSMS 2012 and higher" | Moved to limited support; support ended 2025-12-31; replacement Toad for SQL Server "at no additional cost"; latest release lines 2022.01 and 2022.02 | No vendor statement found | No vendor statement found | https://support.quest.com/product-notification/noti-00001580 ; https://support.quest.com/es-es/apexsql-complete/2022.02 ; https://www.quest.com/sql-tools-complete/faq.aspx |
| ApexSQL Refactor | Hosted add-in (claimed) | EOL notice, same pattern, support ended 2025-12-31 | No statement found | No statement found | https://support.quest.com/product-notification/noti-00001585 ; https://support.quest.com/apexsql-refactor/2022.01 |
| ApexSQL Propagate | Not verified | EOL notice, same pattern | No statement found | No statement found | https://support.quest.com/product-notification/noti-00001584 |
| ApexSQL Search | Hosted add-in (claimed) | In the EOL list quoted by Quest on a community thread (T3); a 2022.01 support page exists | No statement found | No statement found | https://support.quest.com/apexsql-search/2022.01 ; https://www.quest.com/community/apexsql/f/forum/34045/when-is-support-for-ms-sql-2022-available/80467 |
| ApexSQL Diff (schema) | Standalone (claimed) | Lifecycle table: 2022.02 support discontinued 2025-12-31 | n/a | n/a | https://support.quest.com/apexsql-diff/lifecycle |
| ApexSQL Data Diff | Standalone (claimed) | In the same EOL list (T3); 2022.01 page exists | n/a | n/a | https://support.quest.com/apexsql-data-diff/2022.01 |
| ApexSQL Analyze | Not verified | EOL notice: support ends 2025-12-31 | No statement found | No statement found | https://support.quest.com/apexsql-analyze/lifecycle |
| ApexSQL Decrypt, Script, Compare, Compare & Sync Toolkit, Fundamentals Toolkit | Not verified | In the EOL list quoted on the community thread (T3) with Toad for SQL Server Professional as replacement; I could not read the replacement matrix itself | No statement found | No statement found | https://support.quest.com/kb/4379627/apexsql-end-of-life-product-replacement-matrix (refused) ; community thread above |
| ApexSQL Log | Standalone (claimed) | Not in the EOL list. Lifecycle table lists only 2019.02: limited support 2025-06-23, support ends **2026-12-31**; patch 2019.02.2001 (2023-11-07) added SQL Server 2022 | n/a | n/a | https://support.quest.com/apexsql-log/2019.02 ; https://support.quest.com/apexsql-log/lifecycle |
| ApexSQL Recover | Standalone (claimed) | Lifecycle: 2019.02 support ends 2026-12-31 | n/a | n/a | https://support.quest.com/apexsql-recover/lifecycle |
| ApexSQL Plan | Historically an SSMS add-in: **claimed**, not verified | Only a 2019.01 page found, no EOL dates | No statement found | No statement found | https://support.quest.com/es-es/apexsql-plan/2019.01 |
| ApexSQL Doc, Audit, Build, Source Control | Not verified | Support pages for 2024.01 releases exist; no EOL notice found in what I searched | No statement found | No statement found | https://support.quest.com/apexsql-doc/2024.01 ; https://support.quest.com/apexsql-audit/2024.01 ; https://support.quest.com/apexsql-build/2024.01 ; https://support.quest.com/apexsql-source-control/2024.01 |
| ApexSQL Unit Test | Hosted add-in (claimed) | FAQ undated: "SSMS 2012 and later"; 2019.02 build per a download site (T3) | No statement found | No statement found | https://apexsql.com/sql-tools-unit-test/faq.aspx ; https://www.softpedia.com/get/Internet/Servers/Database-Utils/ApexSQL-Unit-Test.shtml |
| ApexSQL Mask, Generate, Trigger, Enforce, Defrag, Discover, Compliance, Monitor, Manage, Job, DevOps Toolkit | Not verified | Lifecycle or support pages exist for some (Generate lifecycle, Discover 2020.01, Manage 2020.07, Job 2019.03, DevOps Toolkit 2022.02, Enforce 2018.x PDF); nothing on Mask, Trigger, Defrag, Compliance, Monitor was found. Dates not read | No statement found | No statement found | https://support.quest.com/apexsql-generate/lifecycle ; https://support.quest.com/apexsql-discover/2020.01 ; https://support.quest.com/apexsql-manage/2020.07 |
| Toad for SQL Server 2026 R2 (replacement) | **Standalone** IDE with its own editor and Object Explorer (datasheet "goes beyond SSMS"; no SSMS add-in found). Class C, ideas only | Active: 2026 R2 release notes dated 2026-06-03, 2026 R1 on 2026-03-23, SQL Server 2025 and Windows Server 2025 compatibility | Not an SSMS add-in | Not an SSMS add-in | https://support.quest.com/toad-for-sql-server/lifecycle ; https://support.quest.com/technical-documents/toad-for-sql-server/2026%20r2/release-notes ; https://blog.quest.com/product-post/introducing-toad-ai-insights/ (T1) |

Unresolved hint (**claimed**, product ambiguous): a Quest datasheet comparing the ApexSQL Fundamentals Toolkit with Toad has a "plug-in" row naming SSMS up to 18 and Visual Studio up to 19. Source: https://www.quest.com/documents/comparing-apexsql-fundamentals-toolkit-to-toad-for-sql-server-datasheet-167854.pdf (summary only).

**Reading for NoName.** Redgate ported its hosted add-ins to SSMS 21 (May 2025) and SSMS 22 (November 2025) and is still shipping fixes every two weeks. Quest did not port the ApexSQL add-ins: it ended support on 2025-12-31 and points customers to a standalone IDE. No ApexSQL feature can be treated as "shipping in SSMS 22".

### SSMS 22.x break timeline from Redgate's own notes (T1)

These are the points where a shipping extension broke or needed work. They are a ready list of what a canary (IDEA-05) should watch.

| SSMS build | What broke | Vendor fix | URL |
|---|---|---|---|
| 22.0 (22.0.0, 2025-11-11) | SQL Prompt not visible or not loading after upgrade (stale SSMS extension cache); SSMS froze during first-run settings import when SQL Prompt was installed | Reinstall or run `SSMS.exe /updateconfiguration`; import freeze fixed in SQL Prompt 11.3.4 (2026-01-20) | https://documentation.red-gate.com/sp/troubleshooting/common-issues/trouble-loading-sql-prompt-in-ssms-v22-after-upgrading-to-v11-2-1-or-later ; https://documentation.red-gate.com/sp/troubleshooting/common-issues/ssms-22-freezes-while-importing-settings-for-the-first-time |
| 22.3 (22.3.0, 2026-02-10) | "SSMS 22.3 introduced changes to internal components used by extensions"; older SQL Prompt, SQL Search, SQL Test, SQL Doc, Dependency Tracker, Data Generator crashed SSMS at startup or when Object Explorer opened. New builds do not load in 22.2 or older | SQL Prompt 11.3.6 (2026-02-13), SQL Test 4.4.3, SQL Search 3.9.5, Toolbelt Essentials installer 2026-02-20 | https://productsupport.red-gate.com/hc/en-us/community/posts/34092306272029 |
| 22.4 (22.4.1, 2026-03-18) | SQL Prompt not loading correctly at start-up | 11.3.8 (2026-03-19) | https://documentation.red-gate.com/sp/release-notes-and-other-versions/sql-prompt-11-3-release-notes |
| 22.5 (22.5.0, 2026-04-14) | Older SQL Source Control (<= 8.1.11), SQL Doc (<= 5.5.5), Dependency Tracker (<= 3.6.4), Data Generator (<= 4.8.5) may crash SSMS | SQL Prompt 11.3.10 (2026-04-03) and updated versions of the others | https://documentation.red-gate.com/sp/troubleshooting/common-issues/ssms-22-5-compatibility |
| 22.6 (22.6.0, 2026-05-12) | SQL Prompt could not disable SSMS built-in IntelliSense; SQL error highlighting disabled when IntelliSense was off; error dialog opening Manage License | 11.3.18 (2026-06-30), 11.3.16, 11.3.14, 11.4.2 | https://documentation.red-gate.com/sp/release-notes-and-other-versions/sql-prompt-11-3-release-notes ; https://documentation.red-gate.com/sp/release-notes-and-other-versions/sql-prompt-11-4-release-notes |

SSMS release dates are from Microsoft's SSMS 22 release notes (https://learn.microsoft.com/en-us/ssms/release-notes-22, read 2026-11-05), which list no 22.4.0; Redgate's pages do not state them. A vendor fix is not an upper bound on an SSMS release date: SQL Prompt 11.3.10 (2026-04-03) came before 22.5.0 (2026-04-14).

## Feature inventories

How-it-works notes are **inference** from release notes and documentation unless a page states the mechanism. Redgate publishes no architecture documents.

### SQL Prompt 11.5 (hosted add-in, T1)

| Area | Concrete features | How it likely works / evidence |
|---|---|---|
| Code completion | Context-aware suggestions from the connected database schema; table definition tooltips; suggestions for functions and new syntax (JSON_OBJECT, JSON_ARRAYAGG, SQL Server 2025); read-only AG replicas; Fabric, Synapse, Azure SQL MI | Editor completion provider plus its own schema cache loaded per connection. It **turns off SSMS built-in IntelliSense** to avoid double popups (fix needed at 22.6). Schema load failures degrade features (release notes 11.3.4, 11.5.4: timeouts and empty results). |
| Formatting | One-shortcut formatting (Ctrl+K+Y), shareable styles, bulk formatting of files, standalone `SqlPrompt.Format.CommandLine.exe` | Parser plus layout engine; the CLI is a separate process, usable in CI (class C pattern). Sharing styles needs a Toolbelt Essentials subscription and the Redgate Platform. |
| Refactoring | Apply casing, insert semicolons, expand wildcards, qualify object names, add or remove brackets, rename alias, smart column rename across dependents, Inline EXEC, INSERT to UPDATE | Text transforms driven by a SQL parse of the editor buffer; Smart Rename reads dependencies from the database (queries system views, claimed). |
| Snippets | Customizable snippets with placeholders; sharing with a Toolbelt Essentials subscription | Editor text expansion. |
| Code analysis | Static rules with codes (for example BP005 asterisk in select list, MI003 unqualified column), enable or disable per rule, share rules, "List Code Analysis Issues" window, auto-fix in bulk, runs against AI output | Parser-based rules over the editor buffer. A baseline or suppression-with-reason feature was **not seen** in the pages read (relevant to IDEA-25). |
| SQL History | Records every query tab and its versions, searchable, starring, restores open tabs after crash or close, trims versions by retention period | Local SQLite file `SqlHistory.db` under `AppData\Local\Red Gate\SQL Prompt 11`. Docs describe corruption cases ("database disk image is malformed") and recovery steps. SSMS only. |
| Tab colouring | Colour tabs by server or database, also in Registered Servers; adapts to SSMS 21/22 themes | Reads connection of the active editor; reacts to connection changes. Marketed as "keeps your changes in the right environment". |
| Object Explorer | "Select in Object Explorer", group tables, procedures, functions, views by schema (11.3.18, 11.4.1), definition grips, Data Modeler menu (added 11.3.15, removed 11.4.1) | Object Explorer node context menus and tree customization. Entries for Registered Servers were broken after sign-out (11.3.13) which shows these menus are fragile. |
| Results grid | Copy as IN clause, Script as INSERT, Open in Excel, aggregate totals on selected cells, Excel rounding option for numbers over 15 digits | Grid context-menu entries that read the grid selection. Fixes in 11.3.13 (duplicate values with Ctrl+click) and 11.5.1 (Open in Excel error) show they run in SSMS 22. |
| Prompt AI (Alt+Z) | Free-text prompt, Explain, Fix (also a popup after an execution error), Optimize, follow-up suggestions, optional initial suggestions from SQL History, regenerate, per-window history; output auto-formatted and passed through code analysis | Reads editor selection (orange highlight) and schema; sends to Redgate's cloud service. Rate limit about 10 prompts per 5 minutes. SQL Server version of the connection is added to requests since 11.3.7. |
| AI code completion (preview, Ctrl+Alt+Up) | Inline completion that reacts to SQL comments; delay 2 s default from 11.5.4; model changed from Qwen3-Coder to Devstral 2 in 11.5.0 | Editor ghost-text or popup provider; opt-in, rate limited. |
| Query Index Analysis | Right-click a SELECT with JOIN or WHERE and see hinted indexes with estimated impact | Redgate-hosted ML model. The query text is sent; docs say "No data is sent", meaning no database data. Only non-clustered rowstore; simulated with uniform distributions. |
| Redgate Assistant (preview) | Dockable WebView2 chat, Alt+A, toolbar button and editor context menu entry (11.5.0), conversation persists across sessions and machines, Mermaid diagrams, free monthly usage plus credits | Tool set exposed through an **MCP server** that the SQL Prompt package starts and stops (see below). Needs WebView2 runtime and Windows 10/11 or Server 2016+. Model per docs in October 2026: Claude Opus 5.5. Calls go to `*.redgate-platform.com` and `*.ai.redgate.cloud`. |
| Shell integration | Main menu "SQL Prompt", command-palette entries (grouped in 11.3.7), SSMS toolbar button, theme switching aligned with SSMS 21/22 | Standard VSSDK menu and command-table features plus the VS command palette. |

**Redgate Assistant tool set (T1, docs plus release notes).** Tools it calls inside SSMS: get the entire script or the active selection; list T-SQL execution errors; list CodeSense (IntelliSense) errors; fetch database object information (schema of tables, views, procedures; never table contents); build the query execution plan for an editor selection (accepts snippets since 11.5.4; errors on CREATE TABLE and on scripts with several GO batches); format with the active style; run code analysis and return an auto-fixed version (11.5.4). The 9,500-character cap on tool calls was removed in 11.4.1. The MCP server is shipped with, started and stopped by the SQL Prompt package; whether it runs in process or as a child process is not stated. It failed to launch when licensing was not ready (11.4.1) and threw an exception on stop (11.5.0).

**Data handling (T1, https://documentation.red-gate.com/sp/ai-in-sql-prompt/sql-prompt-ai-data-handling).** Redgate "may transmit your prompt, the contents of your query window, SQL execution and SSMS/VS CodeSense errors, SQL execution plans and relevant database schema information" to its AI services, plus recent SQL History queries when that option is on. It states nothing is stored or used for training for Prompt AI, and that Redgate Assistant keeps conversation inputs, outputs and history up to 60 days. Controls found: opt-in per user, an organization-wide switch in the Redgate Portal, firewall allow-lists. **No local redaction or masking step is documented on any page I read.** That absence is a statement about the pages read, not proof about the product.

### SQL Search (hosted add-in, free, T1)

| Feature | Notes |
|---|---|
| Find text fragments in tables, views, procedures, functions, jobs and more, across several databases | Likely queries `sys.sql_modules` and job step text; matches live data, not a copy (inference) |
| Quick jump to any object by name (keyboard shortcut), find all references to an object, booleans and wildcards | Navigates Object Explorer to the object |
| Impact analysis use cases | Examples on the product page: find procedures that reference a column; find `SELECT *`; find `TODO` |

### SQL Source Control (hosted add-in, T1)

State-based version control for schema and static data from Object Explorer: link a database to Git, Subversion, TFS/Azure DevOps (or any CLI-driven system via a working folder), commit and update in two clicks, object history, object locking, filters, change icons on Object Explorer nodes, rollback. Git needs the command-line client on PATH. Pre/post deployment scripts. Uses the SQL Compare engine. Azure AD interactive MFA is not supported; Redgate steers those users to Flyway. Relevant fixes: icons in SSMS 21/22, theme switching (8.1.x), an "SSMS Tool Pack" conflict fix, TFS fix for SSMS 22 (8.1.2). Source: https://www.red-gate.com/products/sql-source-control/ ; https://documentation.red-gate.com/soc/requirements-and-supported-systems

### SQL Compare / SQL Data Compare (standalone, T1)

Schema and data comparison of live databases, backups and source-control versions; dependency-ordered deployment script; command line; the free SSMS add-in adds an Object Explorer "Compare schema to" entry that hands off to the application. Passwords now go to the Windows Credential Store (16.0). Source: https://www.red-gate.com/products/sql-compare/ ; https://documentation.red-gate.com/sc/release-notes-and-other-versions/sql-compare-16-0-release-notes

### SQL Test (hosted add-in, T1)

Test runner window for the tSQLt framework: create and run tests, code coverage. tSQLt itself installs objects in the tested database (inference from "test runner for tSQLt", server-side objects). Source: https://documentation.red-gate.com/sqt

### SQL Doc, SQL Dependency Tracker, SQL Data Generator (T1 product pages; mechanism claimed)

Object Explorer right-click entries to document a database, visualize dependencies, or generate test data. Not relevant to a single-instance DBA diagnostic suite beyond showing the Object Explorer launch pattern.

### Quest / ApexSQL (all **claimed**)

No SSMS 21 or 22 feature set can be listed as shipping. For history only:

| Product | What it did (general knowledge of the products and search summaries; not read on a vendor page this session) | Status |
|---|---|---|
| ApexSQL Complete | Code completion, snippets, formatting in SSMS and Visual Studio | Support ended 2025-12-31 |
| ApexSQL Refactor | Refactoring and formatting | Support ended 2025-12-31 |
| ApexSQL Search | Object and data search | In EOL list (T3) |
| ApexSQL Log, Recover | Read transaction log, undo or replay changes; recover dropped or damaged data | Standalone, support ends 2026-12-31 |
| ApexSQL Decrypt | Decrypt WITH ENCRYPTION modules (relates to IDEA-27) | In EOL list (T3); no active vendor tool found for IDEA-27 |
| Toad for SQL Server | Standalone IDE: schema and data compare, Table Data Search, text and folder diff with saved results, Toad Chat dockable panel with AI Explain, Optimize, Validate; chat can act on the editor SQL. Quest's Toad for Oracle has an embedded MCP server; no statement found that Toad for SQL Server has one | Active (2026 R2). Class C, never evidence for SSMS 22 |

## Capability matrix

Ships-in column: **22** means a vendor page read on 2026-11-04 says the feature works in SSMS 22 or its release notes fix it there. API surface: E editor, OE Object Explorer, G results grid, T-SQL/DMV, S server-side objects, P separate process, W tool window, M menu/command table. Class "A (vendor-proven)" is explained in Scope and method. Ideas are BRN-001 numbers.

| # | Capability | Product(s) | Ships in | API surface it needs | Class | Relation to BRN-001 | Evidence |
|---|---|---|---|---|---|---|---|
| 1 | AI prompt from editor text, error, selection, with schema context, sent to a vendor cloud, no local redaction documented | SQL Prompt 11 (Prompt AI), Redgate Assistant | 22 | E (selection, text replace), schema read via T-SQL, M, W | A (vendor-proven) | Confirms IDEA-01 mechanics; **differs** on redaction: NoName's local redaction is the gap | https://documentation.red-gate.com/sp/ai-in-sql-prompt/sql-prompt-ai-data-handling |
| 2 | Offer a fix when a query fails (popup on execution error, SSMS only) | SQL Prompt 11.1.6+ | 22 | E, messages pane or execution event | A (vendor-proven); event hook unproven for NoName | Extends IDEA-01 (error to prompt) | https://documentation.red-gate.com/sp/release-notes-and-other-versions/sql-prompt-11-1-release-notes ; https://documentation.red-gate.com/sp/ai-in-sql-prompt/prompt-ai-features/fix-sql-query-with-ai |
| 3 | Context sources for a prompt: script or selection, execution errors, IntelliSense errors, object schema, execution plan, formatting, code analysis | Redgate Assistant tool set | 22 | E, T-SQL (schema, plan via SHOWPLAN or estimated plan), M | A (vendor-proven) | Extends IDEA-01, IDEA-42, IDEA-49 (a checklist of what to put in a built prompt) | https://documentation.red-gate.com/sp/ai-in-sql-prompt/prompt-ai-features/redgate-assistant-preview |
| 4 | Dockable WebView2 web UI inside SSMS | Redgate Assistant | 22 (11.4.0, 2026-07-01) | W (tool window hosting WebView2), runtime installed | A (vendor-proven) | Relevant to IDEA-12, IDEA-38 (tool window technology choice) | https://documentation.red-gate.com/sp/release-notes-and-other-versions/sql-prompt-11-4-release-notes |
| 5 | MCP server shipped by the add-in, exposing editor and schema tools to an AI chat | Redgate Assistant | 22 | Tool code in the package; process model (in process or child process) not stated | B for the pattern (an MCP server shipped by an SSMS 22 add-in is feasible; process model unknown) | Supports IDEA-06 feasibility; notes startup-order failure | https://documentation.red-gate.com/sp/release-notes-and-other-versions/sql-prompt-11-4-release-notes ; .../sql-prompt-11-5-release-notes |
| 6 | Opt-in AI, org-wide disable, firewall allow-list, 60-day retention statement | Redgate AI | 22 | Account service, not SSMS API | n/a | Context for IDEA-04, IDEA-14 (what the market documents; no redaction receipt) | https://documentation.red-gate.com/sp/ai-in-sql-prompt/ai-in-sql-prompt-faq |
| 7 | Results-grid context menu: Copy as IN clause, Script as INSERT, Open in Excel, selection aggregates | SQL Prompt | 22 (fixes 11.3.13, 11.3.6, 11.5.1) | G (context menu, selection read) | A (vendor-proven) | Confirms grid hook for IDEA-30 grid helpers, IDEA-49 (row to prompt), parked IDEA-03 | https://documentation.red-gate.com/sp/sql-refactoring/refactoring-queries-and-query-results ; release notes 11.3, 11.5 |
| 8 | Query-editor context menu entries and selection icon | SQL Prompt, Redgate Assistant | 22 | E, M | A (proven by own diagnostics as well) | Confirms the query-window menu path already in NoName Diagnostics | https://documentation.red-gate.com/sp/release-notes-and-other-versions/sql-prompt-11-5-release-notes |
| 9 | Object Explorer node context menus and tree changes (group by schema, "Select in Object Explorer", icons, Registered Servers menus) | SQL Prompt, SQL Source Control, SQL Doc, Dependency Tracker, Data Generator, SQL Compare add-in | 22 (not confirmed for the SQL Compare add-in) | OE (undocumented SSMS APIs) | A (vendor-proven); fragile | Confirms IDEA-24/26/28 launch points; evidence for IDEA-05 canary | https://documentation.red-gate.com/sp/release-notes-and-other-versions/sql-prompt-11-4-release-notes ; https://www.red-gate.com/products/sql-source-control/ |
| 10 | Disable or replace SSMS built-in IntelliSense | SQL Prompt | 22 (broke at 22.6, fixed 11.3.18) | E, SSMS settings, undocumented | A (vendor-proven); fragile | IDEA-05; do not rely on it | https://documentation.red-gate.com/sp/release-notes-and-other-versions/sql-prompt-11-3-release-notes |
| 10a | Coexistence with SSMS's own Copilot Chat component: a third-party add-in crashed when applying changes from Copilot Chat (fixed in SQL Prompt 11.3.12, 2026-04-28) | SQL Prompt | 22 | E (editor text apply path shared with Copilot), undocumented | A (vendor-proven coexistence); failure mode shown | Relates to C-08 (works with or without Copilot) and IDEA-05: test applying text while Copilot Chat is active | https://documentation.red-gate.com/sp/release-notes-and-other-versions/sql-prompt-11-3-release-notes |
| 11 | Local query journal with crash restore, retention trimming, search, stars | SQL Prompt SQL History | 22 (SSMS only) | E (documents), local SQLite file, background writer | A (vendor-proven) | Confirms IDEA-30 query journal; storage pattern for IDEA-40 | https://documentation.red-gate.com/sp/ssms-tab-management/sql-history ; https://documentation.red-gate.com/sp/troubleshooting/common-issues/sqlite-database-corruption-sql-history |
| 12 | Tab colouring by server or database | SQL Prompt | 22 | E (tab chrome), connection events | A (vendor-proven) | Extends IDEA-16, IDEA-21 (connection safety badge) | https://www.red-gate.com/products/sql-prompt/ ; release notes 11.3.7, 11.3.11 |
| 13 | Static code analysis with rule codes, per-rule enable, issue list window, bulk auto-fix | SQL Prompt | 22 | E, parser, W (list window) | A (vendor-proven) | Confirms IDEA-25; baseline and suppression-with-reason not seen | https://documentation.red-gate.com/sp/sql-code-analysis |
| 14 | Refactorings: expand wildcards, qualify names, add semicolons, rename alias, Inline EXEC | SQL Prompt | 22 | E, parser, undoable text edits | A (vendor-proven) | Confirms IDEA-30 "expand *" | https://documentation.red-gate.com/sp/sql-refactoring/sql-prompt-actions |
| 15 | Formatting engine, plus a CLI for bulk/CI formatting | SQL Prompt | 22; CLI independent | E; P (separate exe) | A and C | Pattern for IDEA-25 CI runner (class C) | https://documentation.red-gate.com/sp/release-notes-and-other-versions/sql-prompt-11-3-release-notes |
| 16 | Object and text search across modules, jobs, databases | SQL Search (free) | 22 (requirements page lists SSMS 22; the 3.9.0 and 3.9.5 detail is claimed) | T-SQL, OE navigation, W | A (vendor-proven) | Confirms IDEA-26; a free incumbent exists | https://documentation.red-gate.com/ss/requirements |
| 17 | Source control of database objects with Object Explorer icons, history, locking | SQL Source Control | 22 (8.1.0, 2025-11-21) | OE, W, P (git CLI), engine | A (vendor-proven) | Confirms IDEA-31 for objects; server objects not covered | https://documentation.red-gate.com/soc/requirements-and-supported-systems |
| 18 | Schema compare launched from Object Explorer | SQL Compare add-in | **Not confirmed for 22**; scripts open in SSMS 22 | OE, P | B (waits on a probe or vendor confirmation) | Adjacent to IDEA-31 | https://www.red-gate.com/products/sql-toolbelt-essentials/roadmap/ |
| 19 | tSQLt test runner window | SQL Test | 22 (4.4.3 for >= 22.3) | W, S (tSQLt objects) | A (vendor-proven) | New, probably out of scope (developer testing, not DBA diagnostics) | https://documentation.red-gate.com/sqt/requirements |
| 20 | Index suggestions for a SELECT from a hosted ML service | SQL Prompt Query Index Analysis | 22 | E, W; query text leaves the PC | A for the UI; cloud dependent | Relates to IDEA-24; hosted approach conflicts with NoName's local stance | https://documentation.red-gate.com/sp/ai-in-sql-prompt/prompt-ai-features/query-index-analysis |
| 21 | Dark mode and theme switching in vendor windows | SQL Prompt (11.3.11 added theme switching; roadmap lists "Dark mode" in development), SQL Toolbelt Essentials dark mode in development | 22 partial | W, theme tokens | A (partial) | Relevant to the "look native" requirement; vendor itself is catching up | https://www.red-gate.com/products/sql-prompt/roadmap/ ; https://www.red-gate.com/products/sql-toolbelt-essentials/roadmap/ |
| 22 | Extension survives SSMS updates | All Redgate add-ins | 22 | n/a | n/a | Strong evidence for IDEA-05 (see timeline) | https://productsupport.red-gate.com/hc/en-us/community/posts/34092306272029 |
| 23 | Installer or loader warns when package and SSMS build do not match | SQL Test 4.4.3 installer; Toolbelt installer | 22 | Installer logic, SSMS version detection | C (installer) | New for IDEA-05/21 | https://documentation.red-gate.com/sqt/troubleshooting/sql-test-compatibility-update-for-ssms-22-3 |
| 24 | Code completion, refactor, search, decrypt, diff add-ins | ApexSQL (Quest) | **No SSMS 21/22 statement; support ended 2025-12-31 (claimed)** | n/a | Not usable as evidence | None | https://support.quest.com/product-notification/noti-00001580 |
| 25 | Standalone IDE with AI chat, table data search, compare tools | Toad for SQL Server | Not an SSMS add-in | P | C | Ideas only | https://blog.quest.com/product-post/introducing-toad-ai-insights/ |
| 26 | Server-side monitor with AI assistant | Redgate Monitor 14.27+ | Not an SSMS add-in | P (web app + collector) | C | Reference for IDEA-22/34 only | https://www.red-gate.com/products/redgate-assistant/ |

## Candidate ideas for NoName

Each item says what evidence shows it is buildable in SSMS 22 and what NoName would still need to prove. Probe names are left to the brainstorm.

1. **Prompt context checklist from Redgate Assistant's tool set** (extends IDEA-01, 42, 49). Seven context sources are proven reachable from an SSMS 22 package: selection or script, execution errors, IntelliSense errors, schema, plan, formatting, analysis results (matrix row 3). NoName builds the same bundle locally, redacts it, and shows it in the preview (IDEA-07). Needs: confirmation that NoName can read execution errors and IntelliSense errors through its own late-bound APIs; the plan path via T-SQL (estimated plan) is already inside NoName's data scope.
2. **Show exactly what leaves the PC** (extends IDEA-07, 14). Redgate documents in prose what it transmits and gives no preview. A receipt listing each context source, counts per redaction category, and a hash is a differentiator against a documented cloud flow. No new probe beyond IDEA-14's.
3. **Error-triggered prompt offer** (extends IDEA-01). Redgate shows a popup when execution fails (SSMS only, can be switched off). NoName could offer "Build a prompt for this error" in the same moment. Needs a probe for the execution-complete or messages-pane event; Redgate's own notes show false triggers (11.3.7: connection timeouts wrongly triggered the fix popup), so filter by error class.
4. **Result-grid helpers as prompt feeders** (extends IDEA-30 grid helpers, 49). Copy as IN clause and Script as INSERT prove the grid selection and context menu are reachable in SSMS 22 (class A, vendor-proven). "Row to prompt" can use the same hook, with redaction applied to selected cells. Needs the grid late-binding probe for selection text.
5. **Connection safety strip: tab colour and armed-run** (extends IDEA-16, 21). Tab colouring by server and database is a shipping, marketed feature. NoName could tie colour to environment role and require confirmation before apply-mode scripts. Needs: connection-change event and tab chrome access; Redgate also had bugs with Registered Servers colours (11.3.7).
6. **Query journal with a local SQLite store** (confirms and extends IDEA-30, 40). Concrete pattern: SQLite in AppData, restore after crash, retention trimming. Design for the documented failure (corruption, "malformed" errors): backup-on-open and a rebuild path. Privacy point: Redgate feeds history to AI suggestions when an option is on; NoName should keep journal text out of prompts unless redacted.
7. **Canary rows taken from Redgate's break timeline** (extends IDEA-05). Add checks for the points that broke: Object Explorer access after package load (22.3), IntelliSense switch (22.6), WebView2 host if used, extension cache invalidation (`/updateconfiguration` worked around a stale cache after upgrade). A "package built for SSMS >= x" warning on load mirrors what SQL Test's installer does.
8. **MCP server later** (IDEA-06, parked). Redgate ships an MCP server with its package and starts and stops it from the add-in, so the pattern is feasible; the process model is not stated and the startup-order failure (11.4.1) is the risk to design around.
9. **Object search with prompt hand-off** (extends IDEA-26). SQL Search is free and supported in SSMS 22, so a plain search tool adds little; the value is "search result to redacted prompt".

Not recommended from this track: hosted ML index advice (matrix row 20) because the query leaves the PC; table-grouping tweaks in Object Explorer because those depend on undocumented tree internals and Redgate itself adds and removes OE menu items between point releases.

## Things to avoid

| Item | Why | Evidence |
|---|---|---|
| Relying on turning off SSMS built-in IntelliSense | Broke on SSMS 22.6 for the market leader; needed three releases | https://documentation.red-gate.com/sp/release-notes-and-other-versions/sql-prompt-11-3-release-notes |
| Depending on SSMS internal components without a version gate | SSMS 22.3 changed internals used by extensions and crashed every older Redgate add-in at startup or Object Explorer open | https://productsupport.red-gate.com/hc/en-us/community/posts/34092306272029 |
| Building on the ApexSQL add-ins as a reference for SSMS 22 | Support ended 2025-12-31 and no SSMS 21/22 statement exists (claimed); the Toad replacement is a standalone IDE | https://support.quest.com/product-notification/noti-00001580 |
| Treating Toad Chat or Toad for Oracle's MCP server as SSMS evidence | Different host process, and the MCP server is documented for Toad for Oracle only | https://blog.quest.com/product-post/introducing-toad-ai-insights/ |
| Assuming SQL Compare's add-in works in SSMS 22 | Only script hand-off is documented; December 2025 blog said "coming soon" | https://www.red-gate.com/blog/introducing-sql-compare-sql-data-compare-v16-more-future-ready-more-secure/ |
| Sending query text, errors, plans and schema to a vendor cloud unredacted | This is what the market leader documents; it is the gap NoName fills | https://documentation.red-gate.com/sp/ai-in-sql-prompt/sql-prompt-ai-data-handling |
| Aspirational claims on Redgate's pages | "Tests fixes against a copy of your database (preview)", "Autonomous Operations agents", "27 hours a week back", Redgate Assistant in Flyway, SQL Prompt dark mode: roadmap or marketing, not shipping behaviour | https://www.red-gate.com/products/redgate-assistant/ ; https://www.red-gate.com/products/sql-prompt/roadmap/ |
| Duplicating features SSMS 22 already has natively | Not researched in this track; see the SSMS 22 extensibility and native features track file in docs/research | docs/research/ssms22-extensibility-surface-and-native-features.md |
| Pure-ML or hosted index analysis without server stats | Redgate's analysis simulates uniform distributions, not the instance's statistics | https://documentation.red-gate.com/sp/ai-in-sql-prompt/prompt-ai-features/query-index-analysis |

## Open questions and unverifiable claims

1. Does the SQL Compare / Data Compare SSMS add-in (Object Explorer "Compare schema to") load in SSMS 22 today? No page read confirms it. A direct install test would settle it.
2. Which Quest ApexSQL products, if any, are still sold (as opposed to supported)? The replacement matrix at support.quest.com could not be read; the product list above comes from search summaries and a forum thread (claimed).
3. Does any ApexSQL add-in run in SSMS 21 or 22? No vendor statement found; the last dated community statement predates SSMS 21.
4. Is ApexSQL Plan an SSMS-hosted add-in or standalone today? Not verified.
5. Does Toad for SQL Server include an MCP server? Only Toad for Oracle's is documented in the page read.
6. How does SQL Prompt disable and re-enable SSMS built-in IntelliSense, and which API does the 22.6 break involve? Release notes do not say.
7. Does Redgate Assistant's MCP server run in the SSMS process or as a child process, and does it listen on a local port or use stdio? Not stated; the notes only say the package launches and stops it.
8. Redgate AI trust hub contents (models, subprocessors per product) were not retrieved; the docs refer to ai.redgate.cloud/hub/trust.
9. Prices and SQL Toolbelt counts differ between Redgate pages (Toolbelt Essentials page and SQL Source Control page say eleven tools, the roadmap says ten). Treat counts as unreliable.
10. SQL Prompt on ARM64: a web-search summary of the December 2025 update blog said 11.2 supports SSMS 22 on ARM64 and x64; I did not read that page (claimed). SQL Source Control 8.1.0 states ARM64 unsupported at launch, with later ARM64 fixes in its notes.
11. Release-note-based "how it works" statements are inference. No Redgate architecture documents were found.
12. Date convention note: the SQL Compare list page shows the date of each minor version's latest build (16.0 "July 1st, 2026", 16.1 "November 3rd, 2026"), while the 16.0 page itself says "Published 10 December 2025", which matches the December 2025 blog. The SQL Prompt list page uses first-build dates. No 16.0.x or 16.1.x note read mentions the SSMS 22 add-in.

## Sources

All read dates are 2026-11-04. T1 = read; T2 = web-search summary only (**claimed**); T3 = community or third party (**claimed**).

| URL | Tier | Supports |
|---|---|---|
| https://www.red-gate.com/products/sql-prompt/roadmap/ | T1 | Shipped and planned SQL Prompt items: SSMS 22 support, AI, Assistant, dark mode in development |
| https://www.red-gate.com/products/sql-prompt/ | T1 | Product features, supported SSMS and Visual Studio versions, price |
| https://www.red-gate.com/products/redgate-assistant/ | T1 | Assistant product claims, credits, Monitor integration, roadmap statements |
| https://www.red-gate.com/products/sql-toolbelt-essentials/ | T1 | Toolbelt bundle contents and price |
| https://www.red-gate.com/products/sql-toolbelt-essentials/roadmap/ | T1 | SSMS 22 compatibility list; SQL Compare "open scripts in SSMS 22" |
| https://www.red-gate.com/products/sql-source-control/ | T1 | Source Control features |
| https://www.red-gate.com/products/sql-search/ | T1 | SQL Search features |
| https://www.red-gate.com/products/sql-compare/ | T1 | SQL Compare features |
| https://www.red-gate.com/products/sql-doc/ , .../sql-data-generator/ , .../sql-dependency-tracker/ , .../sql-test/ | T1 | Object Explorer launch pattern |
| https://www.red-gate.com/blog/introducing-sql-compare-sql-data-compare-v16-more-future-ready-more-secure/ | T1 | "SSMS 22 integration is coming soon" (2025-12-12) |
| https://documentation.red-gate.com/sp/release-notes-and-other-versions | T1 | SQL Prompt version list; 11.5 current (2026-08-11) |
| https://documentation.red-gate.com/sp/release-notes-and-other-versions/sql-prompt-11-0-release-notes (and 11-1, 11-2, 11-3, 11-4, 11-5 under the same path) | T1 | SSMS 21/22 support dates, AI features, grid fixes, 22.x fixes, Assistant and MCP tools |
| https://documentation.red-gate.com/sp/ai-in-sql-prompt | T1 | AI features overview |
| https://documentation.red-gate.com/sp/ai-in-sql-prompt/sql-prompt-ai-data-handling | T1 | What is transmitted; retention; controls |
| https://documentation.red-gate.com/sp/ai-in-sql-prompt/ai-in-sql-prompt-faq | T1 | Organization switch, rate limits, models |
| https://documentation.red-gate.com/sp/ai-in-sql-prompt/prompt-ai-features/redgate-assistant-preview | T1 | WebView2, tools, endpoints, model, SSMS 18+ |
| https://documentation.red-gate.com/sp/ai-in-sql-prompt/prompt-ai-features/query-index-analysis | T1 | Hosted index analysis |
| https://documentation.red-gate.com/sp/ai-in-sql-prompt/prompt-ai-features/fix-sql-query-with-ai and .../working-with-sql-prompt-ai | T1 | Fix popup on execution errors; auto-format and analysis of AI output |
| https://documentation.red-gate.com/sp/ssms-tab-management/sql-history | T1 | SQL History behaviour |
| https://documentation.red-gate.com/sp/troubleshooting/common-issues/sqlite-database-corruption-sql-history | T1 | SQLite store and corruption |
| https://documentation.red-gate.com/sp/troubleshooting/common-issues/trouble-loading-sql-prompt-in-ssms-v22-after-upgrading-to-v11-2-1-or-later | T1 | `/updateconfiguration` workaround |
| https://documentation.red-gate.com/sp/troubleshooting/common-issues/ssms-22-freezes-while-importing-settings-for-the-first-time | T1 | First-run import freeze |
| https://documentation.red-gate.com/sp/troubleshooting/common-issues/ssms-22-5-compatibility | T1 | 22.5 crash list |
| https://documentation.red-gate.com/sp/sql-code-analysis | T1 | Code analysis features |
| https://documentation.red-gate.com/sp/sql-refactoring/sql-prompt-actions and .../refactoring-queries-and-query-results | T1 | Refactorings and grid actions (page text dated 2019, behaviour confirmed by 2026 release notes) |
| https://documentation.red-gate.com/sp/features-requiring-a-specific-license | T1 | Licence gating |
| https://productsupport.red-gate.com/hc/en-us/community/posts/34092306272029 | T1 | SSMS 22.3 compatibility notice (official post, 2026-02-20) |
| https://documentation.red-gate.com/sqt/troubleshooting/sql-test-compatibility-update-for-ssms-22-3 and https://documentation.red-gate.com/sqt/requirements | T1 | SQL Test and 22.3 |
| https://documentation.red-gate.com/ss/requirements and https://documentation.red-gate.com/ss | T1 | SQL Search host list |
| https://documentation.red-gate.com/soc/requirements-and-supported-systems and .../sql-source-control-8-1-release-notes | T1 | Source Control support and history |
| https://documentation.red-gate.com/sc/sql-server-management-studio-add-in/getting-started-with-the-add-in , .../sc/getting-started/requirements , .../sc/release-notes-and-other-versions/sql-compare-16-0-release-notes , .../sql-compare-16-1-release-notes | T1 | SQL Compare add-in flow and release notes |
| https://documentation.red-gate.com/sca/getting-started/system-requirements/ssms-add-in-requirements | T1 (partial) | SQL Change Automation SSMS add-in page, ambiguous |
| https://blog.quest.com/product-post/introducing-toad-ai-insights/ | T1 | Toad AI Insights, Toad for Oracle MCP |
| https://blog.quest.com/product-post/ai-powered-sql-explanations-latest-updates-to-toad-for-sql-server/ | T1 | Toad for SQL Server features |
| https://support.quest.com/product-notification/noti-00001580 , .../noti-00001584 , .../noti-00001585 | T2 | Complete, Propagate, Refactor limited support, end 2025-12-31, Toad migration |
| https://support.quest.com/kb/4379627/apexsql-end-of-life-product-replacement-matrix | T2 (refused) | Title only; replacement matrix not read |
| https://support.quest.com/apexsql-diff/lifecycle , .../apexsql-analyze/lifecycle , .../apexsql-log/lifecycle , .../apexsql-recover/lifecycle | T2 | Support end dates (Diff 2025-12-31; Log and Recover 2026-12-31) |
| https://support.quest.com/es-es/apexsql-complete/2022.02 and other support.quest.com product pages cited in the Quest table | T2 | Latest release lines |
| https://www.quest.com/community/apexsql/f/forum/34045/when-is-support-for-ms-sql-2022-available/80467 | T3 | List of products entering EOL, Toad Professional replacement |
| https://www.quest.com/community/apexsql/f/forum/32885/apexsql-search-install-issue | T3 | ApexSQL add-ins did not support SSMS 19 or Visual Studio 2022 at that time |
| https://www.quest.com/sql-tools-complete/faq.aspx and https://apexsql.com/sql-tools-unit-test/faq.aspx | T2 | Undated "SSMS 2012 and higher" |
| https://support.quest.com/toad-for-sql-server/lifecycle and .../technical-documents/toad-for-sql-server/2026%20r2/release-notes | T2 | Toad for SQL Server active releases |
| https://www.quest.com/documents/comparing-apexsql-fundamentals-toolkit-to-toad-for-sql-server-datasheet-167854.pdf | T2 | Ambiguous "plug-in" row, SSMS up to 18 |
