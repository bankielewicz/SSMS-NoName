# SSMS 22: extensibility surface, Microsoft's stance, and native features
Research track 4 of 4 for NoName. Written 2026-11-04 by a Sonnet 5.5 research subagent at the owner's request. Evidence, not decisions.

## Scope and method

**What this covers.** Part A is what a third-party extension can reach in SSMS 22 today, and what Microsoft says about third-party extensions. Part B is what SSMS 22 already does natively, so NoName does not rebuild it. Every page below was read on 2026-11-04.

**How the evidence was gathered.**
- Microsoft pages were fetched as raw HTML and converted to text, so the quotes are exact. The page date is the `ms.date` and `updated_at` metadata of the page, not the read date. Older page versions came from the Internet Archive.
- Ten public SSMS 22 extension repositories were shallow-cloned and read. Nothing was built or run. Licences are in Sources. Only short quotations appear here.
- The local SSMS 22 install was inspected read-only: folder listings, text manifests, and file versions through PowerShell `VersionInfo`. No binary was executed or copied.
- NoName's own diagnostic logs (`%LOCALAPPDATA%\NoName\Diagnostics\logs`) hold two short sessions (1633 and 714 bytes) and no probe run summary. So "first-party" below means what the project's CLAUDE.md already states, not new probe results.

**Evidence labels used in the matrix.**

| Label | Meaning |
|---|---|
| FP | First-party: NoName's own diagnostic extension, as stated in the project CLAUDE.md |
| MS | Shipping in the SSMS 22.11 install, written by Microsoft (read from the install folder) |
| 3P | A shipping third-party extension whose source was read. Secondary evidence: the author's claims and notes are cited as claims |
| DOC | Documentation only |
| UNK | Nothing found |

**Class legend (BRN-001).** A is proven in SSMS 22. B is inside SSMS and waits on a named probe. C is outside SSMS. The "class" column is a **suggestion**. It gives A only where FP or MS evidence exists. Anything resting on 3P evidence is B, because a third party's success in one SSMS build is not a proof for NoName in the next.

**Marked "claimed"** means not verified on a current Microsoft page (search-result summaries, author notes, or archived text).

## Microsoft's documented stance on third-party SSMS extensions (quotes, URLs, dates)

**Short answer.** Third-party extensions are unsupported, are not blocked, and there is no official install, update, or discovery path in SSMS 22. The earlier promise to investigate official support is gone from the current FAQ.

### Quotes, newest first

| # | Source (page date) | Quote |
|---|---|---|
| 1 | SSMS FAQ, current, `ms.date` 2026-05-29. URL: https://learn.microsoft.com/en-us/ssms/faq | "Third-party extensions are not supported. SSMS does not actively block them from loading, but any items related to third-party extensions on the feedback site are closed without investigation." Preceded by: "Some SSMS functionality is delivered through Microsoft workloads and components installed via the Visual Studio Installer, such as the SQL Server Migration extension, SSIS, SSAS, and GitHub Copilot in SSMS." |
| 2 | Same FAQ, Internet Archive copy, page `ms.date` 2025-12-17 (updated 2025-12-30). URL: https://web.archive.org/web/2025id_/https://learn.microsoft.com/en-us/ssms/faq | "While SSMS currently doesn't have full support for third-party extensions, we don't block third-party extensions. That means that existing extensions can be updated to work with SSMS without any restrictions. However, we still don't officially support them, and any items requesting help on the feedback site will be closed." Then: "We're investigating official support, which includes VSIX support, extension management, and a marketplace presence, but there are no timelines to share currently." |
| 3 | SSMS 21 Preview 7 announcement, 2025-05-01. URL: https://techcommunity.microsoft.com/blog/sqlserver/announcing-the-release-of-ssms-21-preview-7/4410222 | "For SSMS 21, we will not have full support for third-party extensions." "We will begin looking at official support, which includes the following: VSIX support ... Extension management – leverage Extension menu in SSMS, eliminating the need to use the command line for uninstalling extensions. ... Marketplace presence". "There are no timelines that we can share currently." |
| 4 | SSMS 21 Preview 1 announcement, 2024-11-12. URL: https://techcommunity.microsoft.com/t5/sql-server-blog/announcing-the-release-of-sql-server-management-studio-21/ba-p/4293314 | "any third-party extensions for SSMS will need to be updated to 64-bit to work with SSMS 21." "Long-term, we will bring full support for extensions to SSMS, including the ability to browse extensions from within SSMS, a presence in the Visual Studio marketplace, and support of the VSIXInstaller." |
| 5 | SSMS 21 FAQ page, `ms.date` current. URL: https://learn.microsoft.com/en-us/ssms/ssms-21/faq | Serves the same text as quote 1. |

### What changed, and what is absent
- Between the archived FAQ (quote 2) and the current FAQ (quote 1), the sentences "doesn't have full support", "existing extensions can be updated", and "We're investigating official support ... marketplace presence" were dropped. The commitment to investigate no longer appears on the FAQ.
- The current roadmap (https://learn.microsoft.com/en-us/ssms/roadmap, `ms.date` 2026-01-30, updated 2026-10-14) lists customer feedback, SQL Projects, performance, and AI. It has no extension item. An archived copy of the same page (page `ms.date` 2025-05-19) also has no extension lines. A search-result summary claimed the SSMS 21 roadmap listed in-SSMS extension browsing, a marketplace, and VSIXInstaller support. **Claimed, not found on either copy.** Quotes 3 and 4 do state those goals, in blog posts.
- The 22.x release notes (https://learn.microsoft.com/en-us/ssms/release-notes-22, `ms.date` 2026-10-30, 22.0.0 through 22.11.3) contain no mention of third-party extensions, an Extension Manager, VSIX, or a marketplace. A search for "extens", "vsix", and "third" matched only the phrase "extensive retries".
- `https://learn.microsoft.com/en-us/ssms/whats-new` returns 404. No dedicated extensions page for SSMS was found.

### VisualStudio.Extensibility (out-of-process) and SSMS
- The Learn model-comparison page (`ms.date` 2026-10-20, https://learn.microsoft.com/en-us/visualstudio/extensibility/visualstudio.extensibility/extensibility-models?view=visualstudio) says: "Visual Studio versions: Visual Studio 2022 and later" for VisualStudio.Extensibility, and "Includes support for Visual Studio 2019 and earlier" for VSSDK.
- It states which scenarios still need VSSDK or the hybrid model: custom editors, **IntelliSense providers, light bulb actions**, custom project systems, build integration, debugger engines, and source control providers. Supported out of process: commands, tool windows, editor margins, taggers, CodeLens, LSP, debugger visualizers, settings.
- None of four pages read (models, overview, in-process, extension anatomy) names SSMS. SSMS is not documented as a VisualStudio.Extensibility host. Whether a pure out-of-process third-party extension loads in SSMS 22 is **UNK**; no one has published a test.
- First-party proof that the host is present and that hybrid manifests are accepted is in the next section.

### Third-party reporting (secondary)
- Brent Ozar, 2026-05-19 (https://www.brentozar.com/archive/2026/05/announcing-the-ssms-statisticsparser-extension/): "Microsoft doesn't officially support SSMS extensions yet. They don't prohibit SSMS extensions or bar them from loading". "SSMS v22 doesn't have a Manage Extensions dialog yet, so you have to uninstall it via the command line too." Install is by `VSIXInstaller.exe` from the SSMS IDE folder. He names the missing pieces: marketplace, auto-update, and Microsoft testing new builds against common extensions.
- Redgate, post about SSMS 22.3 (installer path dated 2026-02-20): "SSMS 22.3 introduced changes to internal components used by extensions. Older Versions of the tools depend on earlier version of these components. When loaded in SSMS 22.3, this mismatch causes SSMS to crash during startup or when Object Explorer in SSMS is accessed." Fix: updated tools "will no longer work with SSMS 22.2 and below." URL: https://productsupport.red-gate.com/hc/en-us/community/posts/34092306272029-SSMS-22-3-compatibility-and-fix. Search summaries also said Microsoft contacted extension owners. **Claimed, not found in the post.** This is a precedent for the canary (idea 05): one minor SSMS update broke commercial extensions at startup.
- Community discovery exists outside Microsoft: an SSMS-only gallery at https://erikej.github.io/SsmsExtensions/ (52 extensions listed on read), an "Open SSMS VSIX Gallery" at ssmsgallery.azurewebsites.net, and a curated list in brink-daniel's repository.

### Is there an Extension Manager in SSMS 22? **Unresolved. Do not pick a side yet.**

| Evidence | For or against |
|---|---|
| Install: `CommonExtensions\Microsoft.VisualStudio.ExtensionManager.Implementation.pkgdef` is present. It registers `ExtensionManagementPackage`, the Visual Studio Gallery repository, and a GitHub MCP Registry. | Package present (MS) |
| Install: `SSMS.pkgundef` removes `ToolsOptionsPages\Environment\Extensions` and `...\ExtensionManager`, and the Extension Manager "UI Refresh" feature flag. No line in the file undefines the package's `Menus` entry. | Options page removed; menu removal not found |
| Brent Ozar, 2026-05-19: no Manage Extensions dialog. | Against (secondary, SSMS 22.6 era) |
| SQLExtended docs, repo commit 2026-11-04: "Uninstall ... from SSMS → Extensions → Manage Extensions", and galleries registered under "Tools → Options → Environment → Extensions → Additional Extension Galleries". | For (secondary). Conflicts with the local `pkgundef`, which removes that Options page. |
| ErikEJ gallery page lists a "SSMS Extension Manager: a standalone C# WPF app for managing third-party SSMS 22+ VSIX extensions." | Against a built-in one |

One look at the menu bar of SSMS 22.11 settles it. Probe: open the top-level menus and Tools > Options, and record whether "Manage Extensions" exists.

## Local install inspection (SSMS 22.11: folders, SSMS-specific assemblies, the Copilot extension's manifest, VisualStudio.Extensibility presence)

Root: `F:\Program Files\Microsoft SQL Server Management Studio 22\Release\Common7\IDE` (WSL: `/mnt/f/Program Files/...`). Read 2026-11-04.

### Host and versions

| Item | Value |
|---|---|
| `SSMS.exe` | 22.11.12328.437. Release notes: 22.11.3 (2026-10-29) "Updated to Visual Studio 18.11.3 [12328.437]". |
| SSMS to shell mapping, from the release notes | 22.0.0 = Visual Studio 18.0, 22.10.x = 18.10.x, 22.11.x = 18.11.x |
| `Microsoft.VisualStudio.Shell.Interop.dll` (PublicAssemblies) | 18.11.1020.58502 |
| SSMS-specific assemblies | All 22.11.171.48639 (product 22.11.171+bdff7947ff.RN) |
| `Microsoft.SqlServer.TransactSql.ScriptDom.dll` (Extensions\Application) | 18.0.115.0 |
| `Microsoft.SqlServer.Management.SqlParser.dll` | 18.0.9.0 |
| `Microsoft.SqlServer.Smo.dll` | 18.100.11.38 |
| `Microsoft.SqlServer.XEvent.Linq.dll` | 2025.0170.0855.00 (product 17.0.855.0) |
| Other instances on the machine (ProgramData `_Instances`) | `dd3b7a98` (SSMS 22), `d0065fac` (SSMS 21), `c2658058` (a Visual Studio 18.0 instance) |
| Per-user extension store | `%LOCALAPPDATA%\Microsoft\SSMS\22.0_dd3b7a98\Extensions\` holds metadata caches (`ExtensionMetadata*.mpack`, `CompatibilityList.xml`) and one random-named folder with the NoName files (`NoName.Diagnostics.Vsix.dll`, `.pkgdef`, `extension.vsixmanifest`, `manifest.json`, `catalog.json`). |

### Folder map

| Folder | Content (names only) |
|---|---|
| `Extensions\Application\` | The SSMS application itself as an extension: `SQLEditors.dll`, `SqlWorkbenchProject.dll`, `Microsoft.SqlServer.Management.{SqlStudio, SqlStudio.Explorer, SqlStudio.Actions, SqlStudio.ResourceMonitor, QueryStoreUI, QueryStoreModel, QueryStoreVS, XEventUI, Reports, RegisteredServersUI, ServerInformation, Scripting, PbmUiPackage, HadrTasks, ThreatDetection, UnifiedConnectionDialog}.dll`, `Microsoft.SqlServer.XEventStorage.dll`, `Microsoft.SqlServer.QueryTuningAssistant.*`, Always Encrypted wizard assemblies, `Microsoft.SqlServer.TransactSql.ScriptDom.dll`, `.pkgdef` files for most of these. |
| `Extensions\Microsoft\` | `Copilot`, `SSMSAgent`, `SSMS.CopilotUiTools`, `SSMS.SqlCompletions`, `SSMS.SqlFormatter`, `SSMS.SchemaCompare`, `SSMS.DevOps`, `SSMS.PresenterMode`, `SSMS.MigrationAssistant`, plus generic Visual Studio parts (CodeSense, ConnectedServices, DebugAdapterHost, TemplateEngine, Web Tools). |
| `Extensions\Community\` | About 40 Visual Studio registration `.pkgdef` files only. No community extensions. |
| `CommonExtensions\Microsoft\` | Visual Studio shell parts present: `Editor`, `ErrorList`, `Extensibility`, `Language`, `LanguageServer`, `Search`, `TextMate`, `Terminal`, `OpenFolder`, `TeamFoundation`, `ServiceHub`, `Microsoft.VisualStudio.Copilot.Contracts`, and more. |
| `CommonExtensions\` (root) | `Microsoft.VisualStudio.ExtensionManager.Implementation.pkgdef` and four other `.pkgdef` files. |
| `VSExtensions\Microsoft\` | `CopilotLogViewer` |
| IDE root | `SqlWorkbench.Interfaces.dll`, `Microsoft.SqlServer.GridControl.dll`, `ObjectExplorer.dll`, `SqlPackageBase.dll` (hosts `ServiceCache`), `Microsoft.SqlServer.Management.UI.VSIntegration.SqlEditor.BrokeredContracts.dll`, SMO and `Microsoft.SqlServer.Management.*` (Registered Servers, XEvent, Collector, Sdk.*), `CopilotCli\copilot.exe`, `Microsoft.VisualStudio.Extensibility.Finalizer.exe`. |
| `PrivateAssemblies\` | 188 files (Visual Studio shell and threading libraries, for example `Microsoft.VisualStudio.Threading.dll`). |
| `..\ServiceHub\Hosts\` | `ServiceHub.Host.Extensibility.amd64` |

### Microsoft extensions in the install, and what their manifests show

| Extension | Manifest facts (read from `extension.vsixmanifest` and `.vsextension`) |
|---|---|
| **GitHub Copilot** (`VisualStudio.GitHub.Copilot` 18.11.1093.4562) | `SystemComponent="true"`. Target `Microsoft.VisualStudio.Community [18.0, 19.0)`, amd64 and arm64. MEF assets plus pkgdefs plus `Mcp\mcp.json`. Its folder holds `GitHub.Copilot.SDK.dll`, `ModelContextProtocol.Core.dll`, `Anthropic.dll` and Azure OpenAI and Semantic Kernel connectors (in `Conversations.Service`), `LibGit2Sharp.dll`, and `copilot.registration.json`. Its `mcp.json` lists one server: Microsoft Learn (`https://learn.microsoft.com/api/mcp`). |
| **SSMSAgent** (`Microsoft.SqlServer.Management.Copilot.SSMSAgent` 22.200.11.171) | `ExtensionType="VisualStudio.Extensibility"`, `AllUsers="true"`, `Preview=true`. Target `Microsoft.VisualStudio.Community [18.10,)`, amd64 and arm64. `DotnetTargetVersions` = `net10.0`. Folder holds `SqlCopilotServer.exe` (about 93 MB), `Microsoft.VisualStudio.Extensibility*.dll`, and its own copy of the SqlEditor BrokeredContracts. |
| **SSMSAgent `.vsextension\extension.json`** | Places its Document, Explain, Fix, and Optimize commands in a group whose parent is `legacyParentId` guid `33f13ac3-80bb-4ecb-85bc-225435603a5e`, id 80 (0x50). This is the query-window context menu NoName's CLAUDE.md names. It also exports `Microsoft.VisualStudio.Copilot.ICopilotResponderProvider`. |
| **SSMS.CopilotUiTools** | `ExtensionType="VSSDK+VisualStudio.Extensibility"`, `SystemComponent="true"`, target `Microsoft.VisualStudio.Ssms [22.0,)`, amd64 and arm64. Has `Asset Type="mcp.json"`. Its MCP server `sql-tools` is `SQLtoolsMCPserver.exe` over stdio with env `SQLtools__ConnectionProvider=SSMS`. |
| SSMS.SqlFormatter, SchemaCompare, DevOps (SqlProj), SqlCompletions, PresenterMode, MigrationAssistant | Present as separate extension folders. Not read further. |

**Reading of the above (inference, not stated by Microsoft):**
- The out-of-process host is present: `ServiceHub.Host.Extensibility.amd64`, `Microsoft.VisualStudio.Extensibility*.dll` 18.11.1090.14795, `Microsoft.VisualStudio.Extensibility.Editor.Contracts.dll`, and `Microsoft.VisualStudio.Extensibility.Finalizer.exe`. Microsoft's own Copilot agent runs on it.
- The hybrid manifest type with SSMS as the install target is accepted for a Microsoft component. This is the only first-party proof that `Microsoft.VisualStudio.Ssms` is a valid target for that type.
- An extension can ship an MCP server through an `mcp.json` asset, and SSMS Copilot Agent mode reads MCP servers. This matters for the optional MCP channel later.
- Both Microsoft extensions list arm64. NoName's diagnostic manifest and the project notes list amd64 only. SSMS 22.0.0 notes say "initial Windows Arm64 support".

## Extensibility surface matrix

**Source numbers refer to the Sources section.** "3P" evidence names the repository by short name. SQLX is SQLExtended (JamTheRadar), SP is StatisticsParserExtension (Brent Ozar), AX is AxialSqlTools, ENV is SSMS-EnvTabs, ERIK is ErikEJ SqlServer.Rules `tools/SqlAnalyzerSsms`, EXE is SSMS Executor, SCH is SSMS Schema Folders, OEM is SSMS Object Explorer Menu.

| # | Surface | Documented VSSDK or SSMS-internal | Assembly or namespace | Evidence it works in SSMS 22 | Class | Ideas | Probe needed | Source |
|---|---|---|---|---|---|---|---|---|
| 1 | Main-menu (Tools) commands via `.vsct` | VSSDK, documented | Package, `Microsoft.VisualStudio.Shell` | FP (commands fire). 3P: SQLX, SP, AX, OEM | A | all | none | S30, S31 |
| 2 | Query-window context menu, `{33F13AC3-80BB-4ecb-85BC-225435603A5E}` id `0x0050` (`IDM_SQLWB_SQLSCRIPT_CONTEXT`) | SSMS-internal GUID | `SQLEditors.dll` command set | MS: SSMSAgent places a group here (legacy parent id 80). FP: NoName places on 0x0050 and on `IDM_VS_CTXT_CODEWIN`. 3P: SQLX says "a placement on the code-window menu merges without error and never appears". | A for 0x0050, **check which parent the diagnostic's menu item actually appeared under** | 01, 02, 07, 49 | Record where the diagnostic command showed. Drop `IDM_VS_CTXT_CODEWIN` if it never shows. | S16 `SsmsSchemaViewerPackage.vsct`, MS manifest |
| 3 | Messages-pane context menu `0x0060`, results-grid context menu `0x0070` (same GUID) | SSMS-internal | `SQLEditors.dll` | 3P: SQLX only ("values extracted from SSMS 22's SQLWorkbenchCommands") | B | 03, 49, 19 | Place a command on each, confirm it shows and knows the grid or pane | S16 |
| 4 | Query-tab context menu `IDM_VS_CTXT_EZDOCWINTAB` | Shell | Shell | 3P: SQLX (tab highlight menu) | B | 30 | Place a command, confirm it shows | S16 |
| 5 | `CommandPlacements`, dynamic visibility, `VisibilityConstraints`, dynamic menu items | VSSDK, documented | `.vsct` | DOC. 3P: SQLX uses dynamic groups. SSMS does not ship a documented subset list. | B | 30 | Visibility by active document type (SQL editor only) | S32, S33, S34 |
| 6 | **Object Explorer node context menus** | SSMS-internal. No stable GUID. | `ObjectExplorer.dll` tree, `System.Windows.Forms.TreeView` | 3P: SQLX adds a submenu by reflection on `TreeView.ContextMenuStripChanged` ("SSMS builds those menus dynamically and there is no stable `IDM_OBJEXPL_*` GUID"). OEM and SCH also hook Object Explorer. | B (class A is not possible by VSCT) | 38, 22, 26, 34 | Hook the tree by reflection, add an item on server and database nodes, read node URN | S16 `ObjectExplorer/CLAUDE.md`, S20, S21 |
| 7 | Tool windows (WPF, docking, multi-instance, `ProvideToolWindow`) | VSSDK, documented | `ToolWindowPane` | 3P: SQLX 20 declarations, AX 15, SP | B (FP covers none yet) | 12, 22, 34, 38 | Open a docked WPF window, dock it, restore on restart (SQLX marks its panes `Transient`) | S35, S16 `Monitoring/CLAUDE.md` |
| 8 | Document-well tabs as editors | VSSDK (editor factory) | n/a | UNK. No community extension found. | UNK | 38 | Not needed if tool windows suffice | n/a |
| 9 | Extra tab inside the results pane | SSMS-internal (reflection into `m_sqlResultsControl`) | `SQLEditors.dll` | 3P: SP `InPaneTab\TabPageSupervisor` | B | 19, 45 | Add a tab, survive theme change | S17 |
| 10 | `AsyncPackage`, background load, `JoinableTaskFactory` | VSSDK, documented | `Microsoft.VisualStudio.Shell` | FP (loads on `ShellInitialized`). 3P: all repos. | A | all | none | S36, S37 |
| 11 | Options page (`DialogPage`) and Unified Settings | VSSDK. Release notes 22.6.0: SSMS "Migrated ... IntelliSense and Keyboard Query shortcuts settings ... to the new Unified Settings tab". | `Microsoft.VisualStudio.Shell`, `CommonExtensions\Microsoft\Editor\UnifiedSettings` | 3P: SP has Options and a `UnifiedSettings` folder. EXE, SCH, OEM use `DialogPage`. | B | 40, 38 | Open Tools > Options page, persist a value | S38, S5 |
| 12 | Per-user settings store (`WritableSettingsStore`) | VSSDK, documented | Shell | DOC only. UNK in SSMS. | B | 01, 07 | Write and read a key across SSMS restarts | S39 |
| 13 | Status bar (`IVsStatusbar`) | VSSDK, documented | Shell.Interop | 3P: SQLX, AX (AX shows precise execution time) | B | 12, 30 | Write progress and text | S40 |
| 14 | Info bars (`IVsInfoBarUIFactory`) | VSSDK, documented | Shell.Interop | 3P: SQLX, AX, ENV | B | 05, 16 | Show on a document frame and on the main window | S41 |
| 15 | Notifications hub, toasts | VSSDK, documented as UX guidance | `Microsoft.Internal.VisualStudio.UserNotifications.dll` ships under the Copilot folder | UNK for third parties | UNK | 05 | Optional. Info bars are enough. | S42 |
| 16 | Output window pane (`IVsOutputWindow`) | VSSDK, documented | Shell.Interop | 3P: SP, SCH. Release 22.4.1 added an SMO channel to View > Output. | B | 05, 12 | Create a pane, write a line | S43 |
| 17 | Theme tokens (`EnvironmentColors`, `VsColors`), `ThemedDialogWindow`, Image Service and monikers | VSSDK, documented. Visual Studio 2026 moved to semantic Fluent tokens. | Shell.Styles, `Shell.*.pkgdef` | 3P: AX 11 uses, SP 4, SQLX 7, SQLX 26 moniker uses | B | 40, all UI | Render a window in each SSMS theme (Cool Breeze, dark, tinted), compare with a native tool window | S44, S45, S46 |
| 18 | Editor MEF: classifiers, taggers (`IViewTaggerProvider`), content type `SQL` | VSSDK, documented | `CommonExtensions\Microsoft\Editor\*` | 3P: SQLX (comment and rainbow taggers), ERIK (content types `SQL` and `SQL Server Tools`) | B | 25, 30 | Export a tagger for `SQL`, see it on a real query window | S47, S48 |
| 19 | QuickInfo (`IAsyncQuickInfoSourceProvider`) | VSSDK, documented | Language.Intellisense | 3P: SQLX (`ContentType("SQL")`) | B | 26, 30 | Hover over an object name | S49 |
| 20 | Completion (`IAsyncCompletionSourceProvider`) | VSSDK, documented | Language.Intellisense | 3P: SQLX, with key processors and command filters | B | none in the idea list | Not needed | S50 |
| 21 | **Squiggles plus Error List** (`ITagger<IErrorTag>`, `ITableDataSource`) | VSSDK, documented | Text.UI, `Shell.TableManager` | 3P: ERIK, source read. README says live analysis in SSMS 22 with results in the Error List. Release note 22.3.0 confirms SSMS has an Error List ("1 Error, Click to navigate to the error list"). | B (strongest 3P case) | 25 | Push a fake error for a known statement, see squiggle and Error List row | S23, S51, S52 |
| 22 | Light bulb actions (`ISuggestedActionsSource`) | VSSDK, documented. Not supported out of process (S7). | Language.Intellisense | UNK. No SSMS extension found using it. | UNK | 17, 25 | Offer one action on a fake squiggle | S53 |
| 23 | Editor margins, adornments | VSSDK, documented | Text.UI.Wpf | UNK. ENV reads the built-in "Indicator" margin only. | UNK | 30, 40 | Optional | S54 |
| 24 | Read selection, caret, edits with undo (`IVsTextManager.GetActiveView`, `IVsEditorAdaptersFactoryService.GetWpfTextView`, `IWpfTextView`) | VSSDK, documented | TextManager.Interop, Text.UI.Wpf | 3P: SQLX (align, qualify, snippets, star-expand edits), AX | B | 17, 30, 49 | Replace a placeholder in the active query window, undo with Ctrl+Z | S55 |
| 25 | **Intercept and cancel Execute** (`DTE.Events.CommandEvents[...].BeforeExecute` with `CancelDefault`) | VSSDK/EnvDTE, documented. The command id of `Query.Execute` is SSMS-internal and resolved by name. | `EnvDTE` | 3P: SQLX. Its notes: "Confirmed in a running SSMS 22 (2026-10-15): `Query.Execute` resolves by that name, and `CancelDefault` stops the execution." One command behind F5, Ctrl+E, and the toolbar Execute. | B | 16 | Hook, cancel, then check F5, Ctrl+E, toolbar, and run-selection. Hold the `CommandEvents` object in a static or the handler stops firing. Fail open. | S16 `Safety/CLAUDE.md`, S56 |
| 26 | Priority command target (`IVsRegisterPriorityCommandTarget`) as a fallback for 25 | VSSDK, documented | Shell | UNK | UNK | 16 | Only if 25 fails | S56 |
| 27 | Running Document Table events (`IVsRunningDocTableEvents3`) | VSSDK, documented | Shell.Interop | 3P: ENV (`RdtEventManager`), AX | B | 30 (journal, session restore, open-transaction watch) | Detect open, save, close, and rename of query documents | S57 |
| 28 | `IVsUIShell`, themed dialogs | VSSDK, documented | Shell.Interop | 3P: many | B | all | none beyond theme probe | S45 |
| 29 | VisualStudio.Extensibility, out of process | Documented for Visual Studio, not for SSMS | `Microsoft.VisualStudio.Extensibility*` | MS only (SSMSAgent). Third party: UNK. | B for hybrid, UNK for pure | none for now | Install a trivial pure OOP extension targeting `Ssms` | S7, S8, S9 |
| 30 | Hybrid `VSSDK+VisualStudio.Extensibility` | Documented for Visual Studio | same | MS only (CopilotUiTools targets `Ssms [22.0,)`). Third party: UNK. | B | none for now | Same as 29 | S8 |
| 31 | Brokered services, MEF-exported brokered services | VSSDK, partly documented | `ServiceBroker`, `BrokeredContracts.dll` | MS: Copilot ships `MefExportedBrokeredServices.pkgdef`. 3P: SP calls the SqlEditor brokered services. | B | 19, 05 | See SSMS-specific section | S17 |

**Not found on any current page or in any read repository:** how to read the SSMS theme name at runtime, how to open a Query Store report with parameters, and a list of which Visual Studio shell parts SSMS strips (the `SSMS.pkgundef` file names many removals, for example Debugger, Task List, and Extensions options pages).

## SSMS-specific surfaces (Object Explorer, connection, results grid, plan and deadlock viewers, Registered Servers and CMS, Query Store reports, XEvent UI)

None of these is documented. All evidence is from reading shipping extensions or the install. The late-binding rule in NoName's CLAUDE.md fits: SQLX references the assemblies at compile time from the install folder; SP loads `BrokeredContracts.dll` by name with `Assembly.LoadFrom`.

| Surface | What the evidence shows | Doc status | Assembly | 3P evidence | Probe NoName needs |
|---|---|---|---|---|---|
| **Object Explorer service** | `IObjectExplorerService` provides `GetSelectedNodes(out int, out INodeInformation[])`, `ConnectToServer`, and (per SQLX notes) `FindNode` and `SynchronizeTree`. No method returns all connected servers (SQLX source comment). Nodes expose `UrnPath` and `NavigationContext`. The service is reached through `ServiceCache.ServiceProvider` (reflection into `SqlPackageBase.dll`) or `Package.GetGlobalService`. Node hierarchy files `sqlexplorerhier.xml` and `objectexplorerreplication.xml` are embedded in `ObjectExplorer.dll`. | Undocumented | `SqlWorkbench.Interfaces.dll`, `ObjectExplorer.dll`, `SqlPackageBase.dll` | SQLX; SCH; OEM | Resolve the service, read selected node and URN on a server and a database node |
| **Active connection** | `ScriptFactory.CurrentlyActiveWndConnectionInfo.UIConnectionInfo`, reached by reflection through `ServiceCache.ScriptFactory`. SQLX tries several strategies to turn it into a connection string. The brokered route `ISqlEditorServiceBrokered.GetCurrentConnectionAsync` returns `SqlEditorConnectionDetails` with `EditorMoniker` and `IsActive`. | Undocumented | `SQLEditors.dll`, `SqlPackageBase.dll`, `BrokeredContracts.dll` | SQLX, SP | Read server, database, auth type, and login of the active window. Confirm no password is exposed. |
| **Open a new connected query window** | `ScriptFactory.CreateNewBlankScript(ScriptType.Sql, UIConnectionInfo, null)`. SQLX notes the interface method is implemented explicitly. | Undocumented | `SQLEditors.dll` | SQLX (`QueryWindowOpener`) | Open a window pre-connected, with generated text |
| **Results grid** | One `GridControl` per result set (the `Microsoft.SqlServer.GridControl` library). Selection from `SelectedCells`. Cell text from `IGridStorage.GetCellDataAsString(row, col + 1)`. Grids are found by walking window handles from the focused control. The paint hook `CustomizeCellGDIObjects` exists. Column space for `SelectedCells` differs from storage space (SQLX comment). | Undocumented | `Microsoft.SqlServer.GridControl.dll`, `SQLEditors.dll` | SQLX (`ResultsGridReader`, copy-as-IN, find in results, aggregates) | Read header and selected cells (idea 03, 49). Check wide result sets and NULLs. |
| **Messages pane and execution completion** | SP reads the messages text through `IQueryEditorTabDataServiceBrokered.GetMessagesTabSegmentAsync` in pages. Its code comment: "SSMS 22.6+ added a leading `string editorMoniker` parameter to every `IQueryEditorTabDataServiceBrokered` method". SP's minimum is now SSMS 22.6 (its CHANGELOG). For completion events SP searches the results control for events whose name contains "Completed" (the project lists `ScriptExecutionCompleted`). | Undocumented. Contract changed within SSMS 22. | `BrokeredContracts.dll`, `SQLEditors.dll` | SP | Capture PRINT output (idea 19) and statistics text. Resolve methods by name and arity so a signature change fails soft. |
| **Execution plan viewer** | SSMS shows plans in the results pane and opens `.sqlplan` files. Release 22.0.0 added "Show Execution Plan in New Tab". Copilot 22.5.0 can use "execution plan, messages, client statistics, and the results grid". The reverse direction exists: Performance Studio adds "Open in Performance Studio" to the plan context menu (claimed, from a search summary: SSMS 18 to 22). I found **no** `.sqlplan` or `.xdl` editor registration in `SSMS.pkgdef`, `SSMS.Application.pkgdef`, or the `Extensions\Application\*.pkgdef` files (one match: a "Showplan Compare Properties" key in `QueryStoreVS.pkgdef`). That does not prove there is none: registration may be code-based. | Undocumented | `SQLEditors.dll` (probably) | none for open-in-SSMS | Write a fictional-name `.sqlplan` to a temp folder and open it through `DTE.ItemOperations.OpenFile` or `VsShellUtilities.OpenDocument`. Does the graphical viewer appear? |
| **Deadlock graph viewer** | Microsoft's doc says a `deadlock_xml` value "opens the XML in a new SSMS window", and saved `.xdl` files open with File > Open (search-summary wording, **claimed**; page: https://learn.microsoft.com/en-us/sql/relational-databases/performance/open-view-and-print-a-deadlock-file-sql-server-management-studio?view=sql-server-ver17). No extension opens it programmatically that I found. | Documented for users, not for extensions | UNK | none | Same temp-file probe with `.xdl` |
| **Registered Servers and CMS** | SMO `Microsoft.SqlServer.Management.RegisteredServers.RegisteredServersStore.LocalFileStore.DatabaseEngineServerGroup` reads local groups (SQLX `RegisteredServersReader`). The assembly is in the IDE folder. CMS use by an extension: UNK. | SMO namespace, not read on a Learn page (**claimed documented**) | `Microsoft.SqlServer.Management.RegisteredServers.dll` | SQLX | Low priority: NoName is single-instance by design |
| **Query Store reports** | Implemented in `Microsoft.SqlServer.Management.QueryStoreUI/Model/VS.dll` (22.11.171). Opened from the Query Store node of a database. No public entry point to open a report with parameters. | Undocumented | as listed | none | None for opening. NoName reads Query Store views directly (idea 41). |
| **XEvent UI** | `Microsoft.SqlServer.Management.XEventUI.dll`, `XEventStorage.dll`, `Microsoft.SqlServer.XEvent.Linq.dll` 17.0.855.0 (public reader library, **claimed documented**), Object Explorer "XEvent Profiler" node. Live data viewer exists. No path to hand a `.xel` file to the viewer from an extension was found. | Undocumented for hand-off | as listed | none | Read events with the Linq reader, or plain T-SQL (`sys.fn_xe_file_target_read_file`). Opening in SSMS: temp-file probe. |
| **Copilot integration hooks** | `ICopilotResponderProvider` is Microsoft's contract for chat agents (SSMSAgent). MCP servers: users add them in the Tools panel or `%USERPROFILE%\.mcp.json` (https://learn.microsoft.com/en-us/ssms/github-copilot/mcp-servers). An extension may contribute an `mcp.json` asset (as the Copilot and CopilotUiTools manifests do). Third party use of `ICopilotResponderProvider`: UNK. | MCP page documented. Responder contract undocumented. | `Microsoft.VisualStudio.Copilot.*` | none | Only for the later MCP channel: ship an `mcp.json` asset and see it listed |

## What SSMS 22 does natively (table: feature · arrived in · what it does · overlaps idea · what the idea still adds · URL)

Limited to features that overlap a numbered idea. Version "arrived in" comes from the 22.x and 21.x release notes (S5, S6). "Pre-21" means the release notes I read do not show the first release.

| Feature | Arrived in | What it does | Overlaps idea | What the idea still adds (owner's design, to be confirmed) | URL |
|---|---|---|---|---|---|
| GitHub Copilot in SSMS: chat, inline chat, /doc /explain /fix /optimize | Preview 21 (Copilot private preview), 22.0.0 preview, **GA 22.4.1** | Chat and code assistance in the editor. Sends prompts to a chosen model. Docs: it "might send additional information about the version of SQL to which you're connected, or information about your database". | 01, 02, 07 | Local redaction before anything leaves; works without Copilot or a GitHub account; any AI the organization allows; prompt preview | https://learn.microsoft.com/en-us/ssms/github-copilot/overview |
| Copilot models, BYOM, instructions | BYOM 22.1.0; database instructions 22.3.0; user custom instructions 22.4.1; `CONSTITUTION.md` execution context 22.7.0 | Model choice, project-level and database-level instructions | 01 | Instructions steer a model but do not redact | https://learn.microsoft.com/en-us/ssms/github-copilot/execution-context |
| Copilot reads the results pane | 22.5.0 | Chat can use "execution plan, messages, client statistics, and the results grid" | 49, 03, 42 | Row-level redaction choices and a preview before sending | S5 |
| Copilot Agent mode and `sql-tools` MCP server | Preview 22.7.0, **GA 22.10.2** (also 22.8.0 skills: statistics, wait categories, slow query, IO pressure, memory) | Runs queries, reads plans, edits schema "with your approval". Local MCP server `sql-tools` 3.0.17. Docs: "The security boundary is SQL Server's permission enforcement, not Copilot's approval system." | 16, 22, 34, 47 | Agent mode is a competing path to the same investigations. NoName's value is the deterministic, local, no-token tooling plus redaction. | https://learn.microsoft.com/en-us/ssms/github-copilot/agent-mode |
| MCP server support in Copilot | 22.7.0 with Agent mode | Add servers in the Tools panel or `%USERPROFILE%\.mcp.json`; "Switching to Agent mode is required" (search summary, **claimed**) | the later MCP channel | NoName's MCP channel would feed the same host | https://learn.microsoft.com/en-us/ssms/github-copilot/mcp-servers |
| Query Store reports | Pre-21 (docs: Query Wait Statistics needs "SSMS 18.0 or higher"). 22.0.0 improved Find Query and Run Query in Tracked Queries; 22.3.0 fixed time-range and truncated text. | Regressed Queries, Overall Resource Consumption, Top Resource Consuming, Queries With Forced Plans, Query Wait Statistics, Tracked Queries. Plan forcing from the pane. | 41, 42, 46, 47 | A single instance-level view and a shared time brush; plan-change markers; a prompt per regression. Per-database report windows (inferred from docs navigation, not quoted). | https://learn.microsoft.com/en-us/sql/relational-databases/performance/monitoring-performance-by-using-the-query-store?view=sql-server-ver17 |
| Query Store hints | SQL Server 2022 engine feature; no SSMS editor for it found | Procedure-based hints per query | 41 | UI to review and set | https://learn.microsoft.com/en-us/sql/relational-databases/performance/query-store-hints?view=sql-server-ver17 |
| Query Hint Recommendation Tool | Preview 22.0.0, **GA 22.4.1** | Runs a query with different hints, ranks them, can append a hint; "To keep the hint beyond that editor session, apply it as a Query Store hint" (search summary). | 41, 24 | Review and apply as a Query Store hint | https://learn.microsoft.com/en-us/ssms/query-hint-tool/hint-tool-overview |
| XEvent Profiler and Live Data viewer | Profiler: SSMS 17.3. Viewer fixes in 22.5.0 and 22.11.1. | Live view of the Standard and T-SQL sessions | 23, 44 | Curated capture presets, redaction, prompt link | https://learn.microsoft.com/en-us/sql/relational-databases/extended-events/use-the-ssms-xe-profiler?view=sql-server-ver17 |
| Extended Events viewer and merge | Pre-21 | Open `.xel`, merge files | 23 | Same | https://learn.microsoft.com/en-us/sql/relational-databases/extended-events/advanced-viewing-of-target-data-from-extended-events-in-sql-server?view=sql-server-ver17 |
| Live Query Statistics | Pre-21 (SSMS 2016 era). Defaults for plans, client statistics, and LQS added in 22.10.2. | Live plan of a running query; uses `sys.dm_exec_query_profiles` | 22, 42 | Instance-level monitor rather than one query | https://learn.microsoft.com/en-us/sql/relational-databases/performance/live-query-statistics?view=sql-server-ver17 |
| Execution plan viewer, plan comparison | Comparison since SSMS 16. 22.0.0: "Show Execution Plan in New Tab", JSON and vector index operations. | Side-by-side compare of two plans | 42 | Prompt from a plan with redaction; Query Store regression link | https://learn.microsoft.com/en-us/sql/relational-databases/performance/compare-execution-plans?view=sql-server-ver17 |
| Activity Monitor | Pre-21. 22.2.1 and 22.0.0 fixes. | Panes: Overview, Processes, Resource Waits, Data File I/O, Recent Expensive Queries, Active Expensive Queries | 22, 34 | Wait deltas over time, blocking tree, persisted history | https://learn.microsoft.com/en-us/sql/relational-databases/performance-monitor/activity-monitor?view=sql-server-ver17 |
| Deadlock graph viewer | Pre-21 | Opens a deadlock graph from `.xdl` or from `system_health` XML in a result cell | 44 | List deadlocks from `system_health`, redact, prompt | https://learn.microsoft.com/en-us/sql/relational-databases/performance/open-view-and-print-a-deadlock-file-sql-server-management-studio?view=sql-server-ver17 |
| Log File Viewer: SQL Server error log | Pre-21 | Management > SQL Server Logs | 43 | Parse, group, and prompt | https://learn.microsoft.com/en-us/sql/relational-databases/performance/view-the-sql-server-error-log-sql-server-management-studio?view=sql-server-ver17 |
| Job Activity Monitor and job history | Pre-21 | Read-only grid of job state. History in the Log File Viewer. | 28 | A timeline of runs and overlaps | https://learn.microsoft.com/en-us/ssms/agent/view-job-activity |
| Database DevOps: SQL projects, Git, schema compare | Git integration returned in 21.0.0. Database DevOps preview 22.4.1, **GA 22.10.2**. Schema compare preview 22.7.0, **GA 22.10.2**. 22.5.0: import objects from a database; 22.6.0: Create Project from Database. | Offline project of database objects in source control | 31 | Idea 31 is mostly covered. What may remain: only scheduled capture from the live instance. | https://learn.microsoft.com/en-us/ssms/release-notes-22 |
| SQL Formatter, SQL Completions | Formatter preview 22.7.0, **GA 22.10.2** | Formatting with many options | 30 (formatting is not listed there) | none | S5 |
| Vulnerability assessment | **Removed in SSMS 19.1**: "we have removed the SQL Vulnerability Assessment from SSMS as of version 19.1"; Microsoft recommends Microsoft Defender for SQL instead (page read 2026-11-05) | Nothing in SSMS 22; older SSMS versions ran a rule-based scan of security configuration | 47 | The idea stands: config drift, build currency, memory dumps, capability checks | https://learn.microsoft.com/en-us/sql/relational-databases/security/sql-vulnerability-assessment?view=sql-server-ver17 |
| Always Encrypted wizard | Online encryption in 21.0.0 | Key management and column encryption | none | none | https://learn.microsoft.com/en-us/sql/relational-databases/security/encryption/always-encrypted-wizard?view=sql-server-ver17 |
| Themes | 22.0.0: "Introduced new themes and removed the Blue (SSMS) theme". 22.9.0: connection dialog theme support. Theme revert bug fixed in 22.9.0. | Light, dark, tinted themes | 40 and all UI | Match all themes through shell tokens | S5 |
| Object Explorer: group by schema | 22.4.1 | Toggle in Object Explorer | 26 (partial) | Search across objects and module text | S5 |
| Query history, session restore | **Not found** in the 21.x or 22.x release notes. A search found no Microsoft page. Third parties (Redgate SQL Prompt, Devart) provide it. | n/a | 30 | The idea stands. Claimed absent. | n/a |
| T-SQL code analysis, live | **Not found** in the 22.x notes. ErikEJ's analyzer extension fills it. | n/a | 25 | The idea stands. Claimed absent. | S27 |
| Azure Data Studio retirement | ADS retired 2026-02-28 | Admin and Agent stay in SSMS; notebooks and Query Profiler go to VS Code with the MSSQL extension | 23 | Nothing new moved into SSMS | https://learn.microsoft.com/en-us/azure-data-studio/whats-happening-azure-data-studio |

**Notes on this table.**
- The ADS page says: "Keep job scheduling and classic administration tasks in SQL Server Management Studio (SSMS), which remains the supported home for SQL Server Agent and general administration." It lists no feature that moved into SSMS.
- Features with no overlap to any numbered idea (skipped): Database Diagrams, linked server wizard, Maintenance Plans, replication wizards, Migration Assistant, DTA, Import/Export wizard.
- Third-party overlap you should know about for ideas 22, 26, 27, 28, 30. SQLExtended (MIT, 2026-11-04) ships dashboards for Performance, Agent jobs, Always On, and Replication; SQLite-backed object search including Agent job steps; a "Decryption" folder with `EncryptedModuleCrypto.cs` and `DacConnectionFactory.cs`; a SQL History tool window; and a guard before UPDATE or DELETE without WHERE. AxialSqlTools (Apache-2.0 with an extra ethical-use NOTICE) shows an open-transaction warning and precise execution time. Reading for evidence only; the README says "Every multi-tool suite for SSMS is commercial. This is the open-source one."

## Capabilities that are aspirational or blocked today (with the reason)

| Capability | Status | Reason |
|---|---|---|
| Official, supported third-party extension model | Blocked | Current FAQ: "Third-party extensions are not supported." No Microsoft marketplace, no auto-update. |
| Stable breakage notice before SSMS updates | Blocked | Redgate's SSMS 22.3 post shows an internal-component change crashing startup. SP raised its minimum to SSMS 22.6 after a brokered-method signature change. This is the case for idea 05. |
| Object Explorer menu placement by `.vsct` | Blocked | No stable GUID. Reflection on `TreeView.ContextMenuStripChanged` is the only shipping route. |
| Gating AI-driven query execution (idea 16) | Partly blocked | `CommandEvents.BeforeExecute` sees the `Query.Execute` command only. Copilot Agent mode runs queries through its own `sql-tools` MCP server and asks its own approval, so a hook on the editor command probably never sees them. **Inference from the docs, not tested.** |
| Light bulb fixes out of process | Blocked | Listed as unsupported out of process (S7). In process it is documented, but no SSMS example found. |
| Pure out-of-process third-party extension | Unknown | SSMS is not named in the VisualStudio.Extensibility docs. Only Microsoft's extensions are proven. |
| Open a plan or deadlock in SSMS's own viewer from an extension | Unknown | No registration found in pkgdefs. Only the temp-file probe can tell. |
| Open a Query Store report with preset parameters | Blocked | No entry point found. |
| Read the Messages pane stably | Fragile | Brokered signatures changed in 22.6. |
| Install or update from inside SSMS | Unresolved | See the Extension Manager table above. |
| Idea 27 (decrypt WITH ENCRYPTION modules) | Possible in principle, outside the SSMS surface | SQLExtended's file names suggest a Dedicated Admin Connection approach. I did not read it for correctness and I take no position on policy. |
| Ideas 32, 39, 28 (audit, history database, Agent timeline) | Instance-side | They do not depend on SSMS extension surfaces. Class C for the data side. |
| Idea 45 (PerfMon counters) | Split | `sys.dm_os_performance_counters` is in-instance. Windows PerfMon counters are outside SSMS and SQL (class C). |

## Open questions and unverifiable claims

1. Is there a Manage Extensions menu in SSMS 22.11? (The table above.) One launch of the install settles it.
2. Which parent did NoName's diagnostic context-menu item actually appear under: `0x0050` or `IDM_VS_CTXT_CODEWIN`? SQLExtended says only the first shows.
3. Does a pure out-of-process extension install into SSMS 22 with `Microsoft.VisualStudio.Ssms` as target? Does it need the hybrid type? The Copilot agent targets `Microsoft.VisualStudio.Community [18.10,)` instead, which I cannot explain.
4. The Redgate statement that Microsoft contacted extension owners about 22.3: **claimed, not in the post I read.**
5. Search-summary claims about the SSMS 21 roadmap page, about Performance Studio's plan-menu integration, and about Microsoft's Query Hint tool "Query Store hint" sentence: **claimed**; the pages were not read in full.
6. The deadlock-file doc quote (`deadlock_xml` opens a window): **claimed**, from a search summary; the page was not read.
7. SMO `RegisteredServersStore` documentation: not read on Learn. **Claimed documented.**
8. The FAQ, release notes, and roadmap pages carry Learn dates up to 2026-10-30. A page changed after my fetch would not show here.
9. SQLExtended's own docs are the only source for its "Confirmed in a running SSMS 22 (2026-10-15)" note, and its CLAUDE.md files read like machine-assisted notes. Treat its API facts as strong leads, then probe.
10. Wayback copies: the FAQ copy has page `ms.date` 2025-12-17, but the archive crawl date was not read. Only the page's own dates are stated.
11. Redgate and ErikEJ gallery counts (52 extensions) change daily.
12. NoName's diagnostic logs contain no probe results yet, so no 3P claim above has been re-checked by NoName itself.

## Sources (one line each: URL or path · read date · what it supports)

All read 2026-11-04 unless noted.

**Microsoft pages**
- S1 https://learn.microsoft.com/en-us/ssms/faq · FAQ extension wording, page date 2026-05-29
- S2 https://web.archive.org/web/2025id_/https://learn.microsoft.com/en-us/ssms/faq · earlier FAQ wording, page date 2025-12-17/30
- S3 https://learn.microsoft.com/en-us/ssms/roadmap and https://learn.microsoft.com/en-us/ssms/ssms-21/roadmap · live roadmap 2026-10-14, no extension item
- S4 https://web.archive.org/web/2025id_/https://learn.microsoft.com/en-us/ssms/ssms-21/roadmap · archived roadmap, page date 2025-05-19
- S5 https://learn.microsoft.com/en-us/ssms/release-notes-22 · all 22.x notes, page date 2026-10-30
- S6 https://learn.microsoft.com/en-us/ssms/release-notes-21 · 21.x notes, page date 2026-02-06 (Git returns in 21.0.0, Always Encrypted, Query Store fixes)
- S7 https://learn.microsoft.com/en-us/visualstudio/extensibility/visualstudio.extensibility/extensibility-models?view=visualstudio · model comparison, 2026-10-20
- S8 https://learn.microsoft.com/en-us/visualstudio/extensibility/visualstudio.extensibility/get-started/in-proc-extensions?view=visualstudio · hybrid type and manifest
- S9 https://learn.microsoft.com/en-us/visualstudio/extensibility/visualstudio.extensibility/inside-the-sdk/extension-anatomy?view=visualstudio · supported parts
- S10 https://learn.microsoft.com/en-us/visualstudio/extensibility/visualstudio.extensibility/visualstudio-extensibility?view=visualstudio · overview
- S11 https://techcommunity.microsoft.com/blog/sqlserver/announcing-the-release-of-ssms-21-preview-7/4410222 (2025-05-01) and https://techcommunity.microsoft.com/t5/sql-server-blog/announcing-the-release-of-sql-server-management-studio-21/ba-p/4293314 (2024-11-12) · extension plan quotes
- S12 https://learn.microsoft.com/en-us/ssms/known-issues · checked for extension items, none
- S13 Copilot docs: https://learn.microsoft.com/en-us/ssms/github-copilot/overview , /agent-mode , /mcp-servers , /execution-context (all `ms.date` 2026-09-28)
- S14 https://learn.microsoft.com/en-us/azure-data-studio/whats-happening-azure-data-studio · ADS retirement, 2026-06-09
- S15 Native feature pages: Query Store, Query Store hints, XEvent Profiler, XEvent viewer, Live Query Statistics, compare plans, Activity Monitor, error log, vulnerability assessment (the page says SSMS removed it in 19.1), Always Encrypted wizard, display actual plan (all under https://learn.microsoft.com/en-us/sql/relational-databases/ , links in the table); https://learn.microsoft.com/en-us/ssms/agent/view-job-activity ; https://learn.microsoft.com/en-us/ssms/query-hint-tool/hint-tool-overview

**Community and vendor**
- S16 SQLExtended, JamTheRadar/SQLExtended, commit 84ed1b7 (2026-11-04), MIT · `README.md`, `SQLExtended/ServiceCacheProxy.cs`, `ObjectExplorerHelper.cs`, `QueryWindowOpener.cs`, `ConnectionHelper.cs`, `ResultsGrid/ResultsGridReader.cs`, `SsmsSchemaViewerPackage.vsct`, `Safety/CLAUDE.md`, `ObjectExplorer/CLAUDE.md`, `Monitoring/CLAUDE.md`, `SoluitionDocs/Deployment.md` · SSMS-internal API use and Extension Manager claims
- S17 StatisticsParserExtension, BrentOzarULTD, commit e1526b4 (2026-05-21), MIT · `Capture/ContractTypes.cs`, `MessagesBrokeredClient.cs`, `InPaneTab/TabPageSupervisor.cs`, `CHANGELOG.md`, manifest (Ssms [22.6,], amd64 and arm64)
- S18 AxialSqlTools, Axial-SQL, commit 7b8d4f0 (2026-11-02), Apache-2.0 plus NOTICE · README; manifest `Ssms [22.0,)`
- S19 ssms-executor, tkwj, commit f29ba21 (2026-10-10), MIT · manifest `Ssms [21.0, 23.0)`
- S20 ssms-object-explorer-menu, brink-daniel, commit bfe1366 (2026-11-01), **GPL**: read-only, never copy · list `SSMSExtensionList.md`
- S21 SSMS-Schema-Folders, nicholas-ross, commit 39313a6 (2025-11-13) · manifest `Ssms [21.0,23.0)` amd64
- S22 SSMS-EnvTabs, Blake-goofy, commit e919902 (2026-10-19), Apache-2.0 · `RdtEventManager*.cs`
- S23 SqlServer.Rules, ErikEJ, commit 2331cef (2026-11-04), MIT · `tools/SqlAnalyzerSsms/Linter/ErrorList/*`, `Tagging/*` (Ssms [22.0,))
- S24 SSMSPlus, akarzazi, commit 9ccb961 (2024-07-13) · targets SSMS 15 only; not 22 evidence
- S25 Performance Studio, ErikDarlingData, https://github.com/ErikDarlingData/PerformanceStudio · plan context menu integration (claimed from search summary)
- S26 https://www.brentozar.com/archive/2026/05/announcing-the-ssms-statisticsparser-extension/ · 2026-05-19
- S27 https://erikej.github.io/sql/dacfx/visualstudio/ssms/2026/06/21/analysis-ssms-visualstudio.html · 2026-06-21, T-SQL Analyzer in SSMS 22
- S28 https://erikej.github.io/SsmsExtensions/ · gallery listing, 52 extensions; https://raw.githubusercontent.com/brink-daniel/ssms-object-explorer-menu/main/SSMSExtensionList.md
- S29 https://productsupport.red-gate.com/hc/en-us/community/posts/34092306272029-SSMS-22-3-compatibility-and-fix · 22.3 breakage

**Visual Studio SDK documentation (cited for the matrix; URLs from search results, pages not read in full)**
- S30 https://learn.microsoft.com/en-us/visualstudio/extensibility/internals/commands-menus-and-toolbars?view=vs-2022
- S31 https://learn.microsoft.com/en-us/visualstudio/extensibility/internals/designing-xml-command-table-dot-vsct-files?view=visualstudio
- S32 https://learn.microsoft.com/en-us/visualstudio/extensibility/commandplacements-element (`ms.date` 2016-11-04)
- S33 https://learn.microsoft.com/en-us/visualstudio/extensibility/visibilityitem-element (2016-11-04)
- S34 https://learn.microsoft.com/en-us/visualstudio/extensibility/dynamically-adding-menu-items?view=vs-2022
- S35 https://learn.microsoft.com/en-us/visualstudio/extensibility/extending-and-customizing-tool-windows (2016-11-04)
- S36 https://learn.microsoft.com/en-us/visualstudio/extensibility/how-to-use-asyncpackage-to-load-vspackages-in-the-background (2016-11-04)
- S37 https://learn.microsoft.com/en-us/visualstudio/extensibility/managing-multiple-threads-in-managed-code (2016-11-04)
- S38 https://learn.microsoft.com/en-us/visualstudio/extensibility/creating-an-options-page (2019-03-16)
- S39 https://learn.microsoft.com/en-us/visualstudio/extensibility/writing-to-the-user-settings-store (2019-05-23)
- S40 https://learn.microsoft.com/en-us/visualstudio/extensibility/extending-the-status-bar (2025-11-06)
- S41 https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.infobarmodel?view=visualstudiosdk-2022
- S42 https://learn.microsoft.com/en-us/visualstudio/extensibility/ux-guidelines/notifications-and-progress-for-visual-studio?view=visualstudio
- S43 https://learn.microsoft.com/en-us/visualstudio/extensibility/extending-the-output-window (2016-11-04)
- S44 https://learn.microsoft.com/en-us/visualstudio/extensibility/ux-guidelines/colors-and-styling-for-visual-studio?view=visualstudio
- S45 https://learn.microsoft.com/en-us/visualstudio/extensibility/ux-guidelines/theme-color-token-reference?view=visualstudio
- S46 https://learn.microsoft.com/en-us/visualstudio/extensibility/ux-guidelines/images-and-icons-for-visual-studio (2017-04-26)
- S47 https://learn.microsoft.com/en-us/visualstudio/extensibility/inside-the-editor (2016-11-04)
- S48 https://learn.microsoft.com/en-us/visualstudio/extensibility/walkthrough-highlighting-text (2016-11-04)
- S49 https://learn.microsoft.com/en-us/visualstudio/extensibility/walkthrough-displaying-quickinfo-tooltips (2016-11-04)
- S50 https://learn.microsoft.com/en-us/visualstudio/extensibility/language-service-and-editor-extension-points (2025-10-22)
- S51 https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.shell.tablemanager.itablemanager?view=visualstudiosdk-2022
- S52 https://learn.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2013/bb161501(v=vs.120) (`ErrorListProvider`, 2013 era)
- S53 https://learn.microsoft.com/en-us/visualstudio/extensibility/walkthrough-displaying-light-bulb-suggestions (2016-11-04)
- S54 https://learn.microsoft.com/en-us/visualstudio/extensibility/walkthrough-creating-a-margin-glyph (2025-11-06)
- S55 https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.textmanager.interop.ivstextmanager.getactiveview?view=visualstudiosdk-2022
- S56 https://learn.microsoft.com/en-us/dotnet/api/envdte.commandeventsclass.beforeexecute?view=visualstudiosdk-2019 and https://learn.microsoft.com/en-us/dotnet/api/envdte.commandevents?view=visualstudiosdk-2022 (the 2022 interface page marks the members as not for direct use; **claimed**, from a search summary)
- S57 `IVsRunningDocumentTable` docs: not read; evidence is ENV source only

**Local (read-only, 2026-11-04)**
- L1 `/mnt/f/Program Files/Microsoft SQL Server Management Studio 22/Release/Common7/IDE/` · folder listings, `Extensions\Microsoft\Copilot\extension.vsixmanifest`, `SSMSAgent\extension.vsixmanifest`, `SSMSAgent\.vsextension\extension.json`, `SSMS.CopilotUiTools\extension.vsixmanifest`, `McpServer\mcp.json`, `Copilot\Mcp\mcp.json`, `SSMS.pkgundef`, `CommonExtensions\Microsoft.VisualStudio.ExtensionManager.Implementation.pkgdef`, `Extensions\Application\Microsoft.SqlServer.Management.QueryStoreVS.pkgdef`, and file versions via PowerShell `VersionInfo`
- L2 `%LOCALAPPDATA%/Microsoft/SSMS/22.0_dd3b7a98/Extensions/` · per-user extension store
- L3 `%LOCALAPPDATA%/NoName/Diagnostics/logs/` · two sessions, no probe summary
- L4 `<repo>/CLAUDE.md` · host facts and first-party claims (FP)
