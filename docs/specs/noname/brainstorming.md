---
template_version: "2.0.0"
template_type: "brainstorming"
template_source: "https://github.com/bankielewicz/Specs/blob/c36d38957cf8c90864dd02f18acd6e48f700ae12/docs/templates/discovery/brainstorming.template.md"
template_revision: "c36d38957cf8c90864dd02f18acd6e48f700ae12"
provenance_register: "provenance.json"
provenance_links: ["LINK-NN-0001", "LINK-NN-0002", "LINK-NN-0003", "LINK-NN-0004", "LINK-NN-0005", "LINK-NN-0006", "LINK-NN-0007", "LINK-NN-0008", "LINK-NN-0009", "LINK-NN-0010", "LINK-NN-0011", "LINK-NN-0012", "LINK-NN-0013", "LINK-NN-0014", "LINK-NN-0015", "LINK-NN-0016", "LINK-NN-0017", "LINK-NN-0018", "LINK-NN-0019", "LINK-NN-0020", "LINK-NN-0021", "LINK-NN-0022", "LINK-NN-0023", "LINK-NN-0024", "LINK-NN-0025", "LINK-NN-0026", "LINK-NN-0027", "LINK-NN-0028", "LINK-NN-0029", "LINK-NN-0030", "LINK-NN-0041", "LINK-NN-0042", "LINK-NN-0043", "LINK-NN-0044", "LINK-NN-0045", "LINK-NN-0046"]
document_id: "IDEA-NN-001"
document_version: "0.3.0"
project_id: "NONAME"
status: "draft"
owner: "Bryan Ankielewicz (product owner)"
created: "2026-10-29"
updated: "2026-10-30"
source_of_truth: ["INPUT-NN-001", "INPUT-NN-002", "INPUT-NN-003", "INPUT-NN-004", "INPUT-NN-005", "INPUT-NN-006", "INPUT-NN-007", "INPUT-NN-008", "INPUT-NN-009", "INPUT-NN-010", "INPUT-NN-012", "INPUT-NN-013", "INPUT-NN-014"]
related_documents: ["RES-NN-001"]
depends_on: []
supersedes: ["IDEA-NN-001@0.2.0"]
assumptions: ["ASM-NN-001", "ASM-NN-002", "ASM-NN-003"]
open_questions: ["Q-NN-001", "Q-NN-002", "Q-NN-003", "Q-NN-004", "Q-NN-005", "Q-NN-006", "Q-NN-008", "Q-NN-009", "Q-NN-011"]
---

# Brainstorming: redacted AI prompts from SSMS 22

## Purpose and use

This document explores an SSMS 22 extension that turns SQL Server context into an AI prompt with sensitive information redacted. It is aimed at organizations that don't allow AI tools to connect to databases directly. The owner chose to keep NoName as the working name for now (INPUT-NN-008); the final product name is still open (Q-NN-001). Everything here is a draft proposal unless it is marked as confirmed owner input.

## Inputs

### Confirmed owner input

These are recorded from the owner's statements in Claude Code sessions on 2026-10-29 and 2026-10-30. Speaker: Bryan Ankielewicz, product owner.

<a id="input-nn-001"></a>**INPUT-NN-001:** "Let's brainstorm designing a SQL server management studio extension for version 22 of ssms"

<a id="input-nn-002"></a>**INPUT-NN-002:** "I wasn't happy with the results of codex. This is why we're working in a new folder". The earlier Codex project (`bankielewicz/SSMS`) is background reading only; none of its decisions carry over.

<a id="input-nn-003"></a>**INPUT-NN-003:** "MCP servers with ssms is optional. The requirement for organizations not allowing Ai directly via mcp, the ssms extensions we will create will generate an AI prompt with options to redact sensitive information such as AD/ldap objects, query text with sensitive information, etc…"

<a id="input-nn-004"></a>**INPUT-NN-004:** on whether database, table and column names are replaced: "configuration option based on end/user preferences"

<a id="input-nn-005"></a>**INPUT-NN-005:** on result rows: "I'm ok with your recommendations with the option of including but extension has ability to dynamically redact data a user selects such as a column header or a cell." The accepted recommendation was that result rows are excluded from prompts by default.

<a id="input-nn-006"></a>**INPUT-NN-006:** on who controls redaction settings: "each user". There is no administrator-managed policy.

<a id="input-nn-007"></a>**INPUT-NN-007:** on audience: "me then open source"

<a id="input-nn-008"></a>**INPUT-NN-008** (2026-10-30), after name candidates were offered: "let's continue with noname for now". NoName stays the working name; the final name is deferred, not decided.

<a id="input-nn-009"></a>**INPUT-NN-009** (2026-10-30), after a reviewer noted that the grid-data route might depend on Copilot: "The goal is to create a SSMS extension which will work with both copilot/ai components installed or not."

### Sources

| Input | Reference and version | What it establishes | Reliability / missing context |
| --- | --- | --- | --- |
| Owner input | INPUT-NN-001 to INPUT-NN-007 above | Product direction, defaults and audience | Statements in conversation; no other written record |
| [SSMS FAQ](https://learn.microsoft.com/en-us/ssms/faq) | Docs commit `209c900`, page date 2026-05-29 | Third-party extensions aren't supported but aren't blocked | Primary source, read 2026-10-29 |
| [SSMS 22 release notes](https://learn.microsoft.com/en-us/ssms/release-notes-22) | Docs commit `6c459d2`, updated 2026-10-29 | 22.11.3 is current (2026-10-29) on Visual Studio 18.11.3; Copilot Agent Mode became generally available in 22.10.2 | Primary source; the notes never mention extensions |
| [MCP servers in SSMS](https://learn.microsoft.com/en-us/ssms/github-copilot/mcp-servers) | Docs commit `3bbe907`, page date 2026-09-28 | Users can add MCP servers (local or HTTP) from 22.7; administrators can turn off Agent mode and MCP, or allow-list servers | Primary source |
| [StatisticsParser announcement](https://www.brentozar.com/archive/2026/05/announcing-the-ssms-statisticsparser-extension/) | Brent Ozar Unlimited, May 2026 | Extensions install with `VSIXInstaller.exe` from SSMS 22's `Common7\IDE` folder; SSMS 22 has no Manage Extensions dialog | Practitioner report |
| [sqlfluff-extension-ssms issue #69](https://github.com/mnigliazzo/sqlfluff-extension-ssms/issues/69) | Closed by PR #83 | SSMS 22 extensions build on the in-process Visual Studio SDK 17.14; the newer VisualStudio.Extensibility model lacks in-process editor and Error List equivalents | Third-party developer; the author notes nothing in it had run in SSMS yet |
| [Microsoft Q&A on ARM64](https://learn.microsoft.com/en-us/answers/questions/5665791/will-an-ssms-22-extension-built-for-x64-run-on-ssm) | Unversioned | x64-only builds reportedly fail on ARM64; build AnyCPU | Known from a search summary only; page not opened |
| [ErikEJ SSMS extension directory](https://erikej.github.io/SsmsExtensions/) | Unversioned; no last-updated date | 50 listed extensions; none builds redacted AI prompts | Absence from one directory isn't proof that none exist |
| Redaction tool search: [PasteSecure](https://extscope.org/extension/ebolpbgeaeammpddffibagpobicfnhoc), [queryanonymizer](https://github.com/datateampl/queryanonymizer) | Unversioned | Generic paste redactors and reversible SQL anonymizers exist outside SSMS | Known from search summaries; pages not opened |
| [Earlier Codex project](https://github.com/bankielewicz/SSMS) | Commit `29e1755` | Background only (INPUT-NN-002) | No decisions carried forward |
| Local SSMS 22 install, inspected read-only from WSL | `F:\Program Files\Microsoft SQL Server Management Studio 22\Release`, `SSMS.exe` 22.11.12328.437; files pinned by SHA-256 in the register; vswhere output, 2026-10-30 | E-12 to E-20 | Observed on the owner's PC; file contents were read, nothing ran inside SSMS |

## Problem framing

- **Affected people:** SQL Server developers and DBAs who use SSMS 22 on Windows, in organizations that block AI tools from connecting to databases but allow an approved AI chat. Copilot Agent mode or MCP may be turned off by policy there, and SSMS's AI component may not be installed at all; the extension works either way (INPUT-NN-009). The first user is the owner (INPUT-NN-007). Keyboard-only and screen-reader users must be able to review every redaction (proposal).
- **Current task and workaround:** copy a query, error, plan or deadlock graph into the AI chat and delete sensitive parts by hand, or don't use AI. This is a hypothesis (E-09), not an observation.
- **Observable pain:** unknown; nothing has been measured. EXP-NN-002 gathers a baseline: minutes per manual redaction and items missed.
- **Why investigate now:** SSMS 22's built-in AI route depends on Copilot Agent mode and MCP, which administrators can turn off (E-03). Users in those organizations have no in-tool option. Owner requirement: INPUT-NN-003.
- **Desired change:** a user gets AI help with SQL Server work while choosing, and seeing, exactly what leaves the machine. Identities, secrets, infrastructure names and selected data are replaced first.
- **Explicit non-goals:**
  - The extension sending anything to an AI service itself. This is the default; MCP is a possible later channel.
  - Organization-managed or locked policy (INPUT-NN-006).
  - Guaranteeing that every sensitive item is found.
  - Running AI-suggested SQL automatically.
  - Replacing Copilot where organizations allow it.
  - Platforms other than Windows; SSMS runs only on Windows.
- **Decision owner and timebox:** Bryan Ankielewicz. No timebox is set (Q-NN-006).

## Evidence and hypotheses

| Local ID | Claim | Classification: observed / reported / hypothesis | Source and date | Confidence and contrary evidence | Next check |
| --- | --- | --- | --- | --- | --- |
| E-01 | SSMS doesn't block third-party extensions but doesn't support them; feedback items about them are closed without investigation | observed (documentation) | SSMS FAQ, 2026-05-29 | High; no contrary evidence found | EXP-NN-001 (a) |
| E-02 | Current build is 22.11.3 (2026-10-29) on Visual Studio 18.11.3; there were seven releases between 2026-09-08 and 2026-10-29 | observed (documentation) | Release notes, 2026-10-29 | High | Re-check before each of our releases |
| E-03 | Copilot Agent mode accepts user-added MCP servers from 22.7; administrators can turn off Agent mode and MCP, or allow-list servers | observed (documentation) | MCP page, 2026-09-28 | High | No check needed for the core product; revisit with OPT-05 |
| E-04 | Extensions install with `VSIXInstaller.exe` from SSMS 22's IDE folder; there's no Manage Extensions dialog | reported | Brent Ozar, May 2026 | Medium-high; one practitioner | EXP-NN-001 (a) |
| E-05 | Extensions build on the in-process Visual Studio SDK 17.14; VisualStudio.Extensibility lacks in-process editor and Error List equivalents | reported | sqlfluff-extension-ssms issue #69 | Medium; the author hadn't run the changes in SSMS | EXP-NN-001 |
| E-06 | x64-only builds fail on ARM64, and AnyCPU builds work | reported | Microsoft Q&A (search summary) | Low-medium; page not opened | EXP-NN-001 (g) |
| E-07 | No existing SSMS extension builds redacted AI prompts | observed absence | ErikEJ directory and one web search, 2026-10-29 | Medium; the directory may be incomplete | Search the Visual Studio Marketplace and GitHub before choosing a name |
| E-08 | Generic paste redactors and reversible SQL anonymizers exist outside SSMS | reported | Web search, 2026-10-29 | Medium | Review one reversible anonymizer's placeholder design for Q-NN-002 |
| E-09 | Manual redaction is slow and misses items hidden in plans and deadlock XML | hypothesis | Owner requirement and reasoning | Unknown | EXP-NN-002 |
| E-10 | Exact names read from the connection (logins, server, databases, linked servers) catch AD and infrastructure names more reliably than patterns | hypothesis | Proposal | Unknown | EXP-NN-003 |
| E-11 | An extension can read the results-grid selection, a column header or a cell | hypothesis | Needed by INPUT-NN-005; no documentation found | Unknown; no public API found yet; see E-15 to E-17 | EXP-NN-001 (e) |
| E-12 | The owner's PC has SSMS 22.11.3 (build 22.11.12328.437) on F:, SSMS 21.6.17 on C:, and Visual Studio Community 2026 18.11.3 with the extension-development workload and MSBuild | observed | vswhere output, 2026-10-30 | High | None needed; the spike targets SSMS 22 only |
| E-13 | SSMS 22 runs on .NET Framework 4.7.2 or later | observed | `SSMS.exe.config` `supportedRuntime`, 2026-10-30 | High | Build the VSIX for .NET Framework |
| E-14 | SSMS 22 ships ScriptDom 18.0.115 with parsers TSql80 to TSql180, and redirects ScriptDom versions 0.0.0.0 to 18.0.0.0 to that copy | observed | ScriptDom DLL and `SSMS.Admin.pkgdef`, 2026-10-30 | High for this build; the redirect may change between releases | EXP-NN-001 (f): call the parser from the extension |
| E-15 | SSMS 22 contains Copilot's brokered-service contracts, which name `GetGridResultsSegmentAsync`, `GetQueryPlanXmlSegmentAsync`, `GetMessagesTabSegmentAsync`, `GetClientStatisticsAsync`, `GetCurrentConnectionAsync` and `GetOpenEditorsAsync` | observed (names only) | `SqlEditor.BrokeredContracts.dll` 22.11.171, 2026-10-30 | High that the names exist; there are two copies with different bytes; nothing was called | EXP-NN-001 (c), (d), (e) |
| E-16 | A third-party extension can obtain those brokered services | hypothesis | Inference from E-15 | Unknown; access may be limited to Copilot. They return result data, not which cell the user selected | EXP-NN-001 (e) |
| E-17 | The results-grid control is an internal, undocumented assembly | observed | `Microsoft.SqlServer.GridControl.dll` 22.11.171, 2026-10-30 | High | EXP-NN-001 (e) |
| E-18 | The owner's PC is x64 (AMD64), so ARM64 can't be tested on it | observed | Windows `PROCESSOR_ARCHITECTURE`, 2026-10-30 | High | Needs an ARM64 device (Q-NN-004) |
| E-19 | The owner's SSMS 22 has the AI workload (`Microsoft.SqlServer.Workload.SSMS.AI`) and the GitHub Copilot component installed; the `SSMSAgent` folder is that component (`Microsoft.SqlServer.Management.Copilot.SSMSAgent` 22.200.11.171) | observed | vswhere package list and the `SSMSAgent` manifest, 2026-10-30 | High | A test on this PC alone can't show how the extension behaves without the AI component |
| E-20 | The brokered services (E-15) exist even without the AI component | hypothesis | Their names appear in core `SQLEditors.dll` (in `Extensions\Application`) as well as in the Copilot component; no broker registration was found in SSMS's pkgdef files | Unknown; the static evidence points both ways | EXP-NN-001 on an install without the AI component |

## Users, jobs, and exclusions

| User segment or role | Job and trigger | Current barrier | Consequence if excluded | Evidence reference |
| --- | --- | --- | --- | --- |
| Owner (first user; DBA and developer) | Get AI help with a query, plan, error or deadlock at work | AI can't connect directly; redacting by hand | The product has no first user | INPUT-NN-003, INPUT-NN-007 |
| Open-source adopters (proposed, not observed) | The same job in their own organizations | The same | Smaller audience and no outside feedback | INPUT-NN-007 |
| Security and compliance reviewers (affected, not users) | Decide whether the tool is acceptable at work | Need to know what it does and doesn't guarantee | The tool may be banned | Proposal |
| Users whose organizations allow Copilot with MCP | Use AI inside SSMS directly | None; Copilot already serves them | Low; the optional MCP channel can serve them later (OPT-05) | E-03 |
| Users on macOS or Linux | Use SSMS | SSMS runs only on Windows | Excluded by the host, not by this product | SSMS FAQ |

## Options

| Option ID | Concept | User outcome | Smallest experiment | Major dependency | Cost / effort range and basis | Main risk |
| --- | --- | --- | --- | --- | --- | --- |
| OPT-01 | Do nothing: copy and redact by hand | No new tool; likely slow and error-prone (E-09) | EXP-NN-002 baseline | None | No build cost | Leaks through missed items |
| OPT-02 | A generic paste redactor, such as a browser extension like PasteSecure | Catches secrets and emails in pasted text | Paste a sample plan and deadlock graph; count missed items | Third-party tool | Not estimated; no capacity data (Q-NN-006) | Doesn't know your logins, server names or plan and deadlock XML; can't restore names in SQL answers |
| OPT-03 | A standalone Windows app that connects to SQL Server itself | Connection-aware redaction outside SSMS | Prototype the engine as a command-line tool | Its own connection setup | Not estimated (Q-NN-006) | No right-click or grid selection in SSMS; a second place to manage connections |
| OPT-04 (recommended) | An SSMS extension (VSIX) over a standalone .NET redaction engine, clipboard only | Prompts built in context, a preview of what leaves, and names restored in answers | EXP-NN-001 host spike | Unsupported extension surface (E-01) | Not estimated (Q-NN-006) | SSMS updates break the extension (E-02) |
| OPT-05 | OPT-04 plus an optional MCP channel through the same redaction step | The same, plus Copilot where it's allowed | Only after OPT-04's engine exists | Copilot in SSMS 22.7 or later | Not estimated (Q-NN-006) | Policy turns off MCP for the target users (E-03) |

## Proposed concept

These are proposals for review, not approved requirements.

### Flow

1. Right-click a query or selection, an error message, an execution plan, a deadlock graph, a results-grid selection or an Object Explorer object, and choose **Build AI prompt**.
2. Pick a task template: explain, tune, fix an error, review a change script or analyze a deadlock. Users can add their own templates.
3. The engine gathers context through the current connection, such as definitions of the objects involved, their indexes, row counts and the plan.
4. A preview window shows the exact prompt with each redaction highlighted by category. The user switches items on or off, and can mark more text, a column header or a cell for redaction (INPUT-NN-005).
5. The user copies the prompt to the clipboard or saves it to a file. The extension sends nothing. Prompts are written using prompting best practices (INPUT-NN-012); this copy-and-paste route is how users without Copilot get AI help.
6. The user pastes the AI's answer back. Placeholders are swapped for the real names locally, and any SQL opens in a new editor window. It is never run automatically.

### Redaction categories

| Category | Examples | Proposed default |
| --- | --- | --- |
| Secrets | Passwords in `CREATE LOGIN` and `ALTER LOGIN`, credential secrets, shared access tokens, key and certificate passwords | Always on (Q-NN-009) |
| Identity | Windows logins and groups (`DOMAIN\name`), user principal names, SIDs, LDAP distinguished names, email addresses | On |
| Infrastructure | Server and instance names, fully qualified domain names, IP addresses, linked servers, file and network paths, connection strings | On |
| Query values | String, number and date literals, comments, parameter values inside plans | Strings and comments on; numbers and dates open (Q-NN-003) |
| Object names | Database, schema, table and column names | User setting (INPUT-NN-004); off in the Standard preset |
| Result data | Results-grid rows | Excluded by default; can be included, with per-column and per-cell redaction (INPUT-NN-005) |
| Custom | The user's own words and patterns, such as project code names or employee ID formats | The user's list |

### Techniques

- Parse T-SQL with Microsoft's open-source ScriptDom parser (MIT licence) to find every literal, comment and name. Read plan and deadlock XML by element and attribute. Use text patterns only for free text such as error messages.
- Read exact names from the connection and redact every exact match. Sources include Windows logins and groups in `sys.server_principals`, `@@SERVERNAME`, database names and linked servers (E-10).
- Respect labels the database already has, in `sys.sensitivity_classifications` and `sys.masked_columns`, where the server version provides them.
- Replace each name with a consistent placeholder: `CONTOSO\jsmith` becomes `LOGIN_1` everywhere it appears. Placeholders are valid T-SQL names. The mapping stays in memory for that prompt and is never included in it. The placeholder format is open (Q-NN-002).
- Offer presets that each user saves for themselves (INPUT-NN-006):
  - **Minimal:** secrets and identity.
  - **Standard:** adds infrastructure and query values.
  - **Strict:** also replaces object names.

### Architecture sketch

- **Engine:** a .NET library and command-line tool with no SSMS dependency. It can be tested in CI with synthetic fixtures and a SQL Server container.
- **VSIX:** the commands, the preview window and paste-back. It is kept thin, so fixing it after an SSMS update is cheap.
- **Later, optionally:** an MCP server built on the same engine (OPT-05).

## Constraints and assumptions

| Item | Constraint or assumption | Evidence / authority | Cost of being wrong | Owner | Revisit trigger |
| --- | --- | --- | --- | --- | --- |
| C-01 | Constraint: SSMS doesn't support third-party extensions | E-01 | No vendor help when an SSMS change breaks the extension | Bryan | Each SSMS minor release |
| C-02 | Constraint: SSMS releases often | E-02 | Frequent compatibility fixes | Bryan | Each SSMS release |
| C-03 | Constraint: by default the extension makes no network calls to AI services | INPUT-NN-003 | The product defeats its own purpose | Bryan | Only by owner decision |
| C-04 | Constraint: settings are per user, with no administrator lock | INPUT-NN-006 | Organizations that require central policy may not adopt it | Bryan | When adopters ask for central policy |
| C-05 | Constraint: SSMS runs on Windows x64 and ARM64 | SSMS FAQ; E-06 | ARM64 users are excluded if we build x64 only | Bryan | Q-NN-004 |
| ASM-NN-001 | Assumption: target users have an approved AI chat to paste into | Implied by INPUT-NN-003 | Prompts have nowhere to go | Bryan | First outside adopter feedback |
| C-06 | Constraint: the extension runs on .NET Framework 4.7.2 or later inside SSMS | E-13 | Code that needs a newer .NET can't run in-process | Bryan | Each SSMS minor release |
| C-07 | Constraint: inside SSMS, the extension gets SSMS's ScriptDom (18.0.115 today), whatever version it was built against | E-14 | Parser behaviour changes when SSMS updates it | Bryan | Each SSMS minor release |
| C-08 | Constraint: every core feature works whether or not SSMS's AI/Copilot component is installed. Services that may come with Copilot (E-15, E-20) are an optional fast path with a fallback | INPUT-NN-009 | Users without Copilot, the main audience, lose features | Bryan | Only by owner decision |
| ASM-NN-002 | Assumption: the extension can call ScriptDom in-process inside SSMS 22 | Partly settled: SSMS ships and redirects it (E-14); a call from our code hasn't run | A different parser or an out-of-process engine is needed | Bryan; engineering by Claude | EXP-NN-001 (f) |
| ASM-NN-003 | Assumption: placeholders survive an AI's rewrite well enough to restore names | Proposal | Restoring names is unreliable and needs manual fixes | Bryan; engineering by Claude | EXP-NN-003 and Q-NN-002 |
| Budget and delivery window | Unknown | Not yet stated by the owner | Work can't be scheduled | Bryan | Q-NN-006 |

## Experiments and falsification

| Experiment ID | Hypothesis | Method and participants | Metric and denominator | Success / stop threshold and window | Owner | Actual result reference |
| --- | --- | --- | --- | --- | --- | --- |
| EXP-NN-001 | A VSIX can do everything the concept needs in SSMS 22.11.x | On Bryan's Windows machine with SSMS 22.11.x, a minimal VSIX that: (a) installs with `VSIXInstaller.exe` and loads; (b) adds a right-click command that reads the editor selection; (c) reads the current connection's server and database; (d) gets the actual execution plan XML; (e) reads results-grid data through the brokered service (E-15), and the selection, both a column header and a cell; (f) runs ScriptDom in-process; (g) loads on ARM64 if a device is available. Run (a) to (f) twice: on this PC, which has the AI component (E-19), and on an SSMS 22 install without it (Q-NN-011). The extension checks whether each service is available and falls back when it isn't | Capabilities passed out of 7, for each install | Stop if (a) or (b) fails, and reconsider OPT-03. If grid data or plan XML works only with the AI component installed, a fallback must pass on the install without it, or that feature is left out when the component is missing (C-08). If (e) works only by reaching into undocumented SSMS types, record that and decide whether column and cell redaction moves into the preview window. Window: before any product code; waits on Q-NN-010 | Bryan; spike code by Claude | (a) to (f) `not_run`; (g) `blocked`, this PC is AMD64 (E-18). Inspection-only findings: E-12 to E-20. Run plan: [RES-NN-001](feasibility-spike.md) |
| EXP-NN-002 | Manual redaction is slow and misses items | Bryan redacts three sanitized samples by hand (a query, an actual plan and a deadlock graph), then a second pass counts missed items | Minutes per sample; missed items out of planted items | Proposed: if all three take under 2 minutes with nothing missed, re-examine the product's value before building it. Window: before the PRD | Bryan | `not_run` |
| EXP-NN-003 | Connection-based redaction catches planted identities and secrets | Synthetic fixtures (queries, plans, deadlock XML and error text) with planted logins, SIDs, distinguished names, user principal names, emails, server names, IP addresses and passwords, run through an engine prototype | Planted items caught out of planted items, by category; placeholder round trips that restore correctly after a simulated AI rewrite | Proposed: 100% for secrets; the owner sets the other thresholds after the first run. Window: engine prototype | Bryan; engine by Claude | `not_run` |

- **Resolve first:** EXP-NN-001 (e), access to the results grid. INPUT-NN-005 depends on it, and no documentation was found. The brokered service (E-15) is the first thing to try. Even if it works, it returns data rather than the user's selection, so column and cell picking would happen in the preview window. Because the service may come with Copilot (E-20), the test must also run on an install without the AI component (C-08).
- **Safeguards:** synthetic fixtures only. No real logins, server names or customer data go into the repository or into test prompts.
- **What would make us stop:** the VSIX can't load or add commands in 22.11.x, or the engine can't catch 100% of planted secrets.
- **What results can't establish:** synthetic fixtures don't prove coverage on real data, and one machine and build don't prove behaviour on other SSMS builds. A pass on this PC alone says nothing about installs without the AI component (E-19).

## Decision and handoff

| Decision | Selected option or deferred choice | Evidence | Decision owner and date | Next artifact |
| --- | --- | --- | --- | --- |
| Continue (proposed) | OPT-04 now; OPT-05 later | INPUT-NN-003 to INPUT-NN-007; E-01 to E-08 | Bryan; pending (proposed 2026-10-29) | Feasibility spike packet `docs/specs/noname/feasibility-spike.md` for EXP-NN-001, written once the owner approves installing a throwaway VSIX (Q-NN-010). The spike runs on installs with and without the AI component (C-08, Q-NN-011). The engine prototype (EXP-NN-003) can start in WSL meanwhile. Then a PRD |

Confirmed needs INPUT-NN-003 to INPUT-NN-007, INPUT-NN-009 and INPUT-NN-012 carry into the PRD. Hypotheses E-09 to E-11 stay with their experiments. The spike packet now exists as [RES-NN-001](feasibility-spike.md), written as a diagnostic extension (INPUT-NN-010, INPUT-NN-011). At the design stage, mockups are prepared through DevForgeAI's design workflow with the Claude Design handoff, so the product looks good enough that people need to use it (INPUT-NN-014).

## Optional enterprise extension

`not_applicable`: settings are per user (INPUT-NN-006). Owner: Bryan. Revisit if open-source adopters ask for centrally managed policy.

## Provenance

| Subject record/document | Register link IDs | Origin or upstream record | Derivation / verification gaps |
| --- | --- | --- | --- |
| IDEA-NN-001 | LINK-NN-0001 | Brainstorming template 2.0.0 at `c36d389` | The bundled copy's hash matched the template map; the GitHub URL is in a private repository and wasn't opened |
| INPUT-NN-001 to INPUT-NN-007 | LINK-NN-0002 to LINK-NN-0008 | Owner statements, 2026-10-29 | Recorded from the conversation; there is no separate durable record |
| E-01 to E-08, C-01 to C-05 | LINK-NN-0009 to LINK-NN-0017 | Web sources listed in Inputs | Microsoft pages are pinned to their docs commits; other pages are unversioned; the Q&A and redaction-tool pages are known from search summaries only |
| INPUT-NN-002 background | LINK-NN-0018 | `bankielewicz/SSMS` at `29e1755` | Read as background; nothing derived from it |
| IDEA-NN-001 v0.3.0 | LINK-NN-0041 to LINK-NN-0046 | v0.2.0; INPUT-NN-010, INPUT-NN-012, INPUT-NN-013 and INPUT-NN-014 (recorded in RES-NN-001) | Q-NN-007 and Q-NN-010 answered; spike packet linked; prompt best practices and design-stage mockups added |
| IDEA-NN-001 v0.2.0 | LINK-NN-0019 to LINK-NN-0030 | v0.1.0, INPUT-NN-008, INPUT-NN-009, the local SSMS 22 files and the vswhere output | Local files are pinned by SHA-256; the vswhere output is unversioned. Sections unchanged since v0.1.0 keep links LINK-NN-0002 to LINK-NN-0018 |

## Open questions

| ID | Question | Owner | Due | Blocking scope |
| --- | --- | --- | --- | --- |
| Q-NN-001 | What is the product name? Candidates: Understudy, SQL Airlock, Stand-In. Avoid: Veil (`SqlVeil` already exists for SQL masking), names containing Mask (confusable with SQL Server's Dynamic Data Masking), Guard (SQL Guard exists), Copilot, or a leading "SSMS" | Bryan | Deferred by INPUT-NN-008; due before creating the public repository | Repository, package and namespace names; not the spike |
| Q-NN-002 | Which placeholder format stays valid T-SQL and survives an AI's rewrite? | Bryan; proposal by Claude | Before designing paste-back | Restoring names in answers |
| Q-NN-003 | Does the Standard preset redact number and date literals? | Bryan | Before setting preset defaults | Preset defaults |
| Q-NN-004 | Does the first release support ARM64? | Bryan | Before packaging | Build configuration |
| Q-NN-005 | Which open-source licence, and when is it published? | Bryan | Before public release | Publication |
| Q-NN-006 | What time and capacity are available for the spike and the MVP? | Bryan | Before scheduling | Calendar; not the spike itself |
| Q-NN-007 | Should `NoName` become a git repository, and a GitHub repository? | Bryan | Answered by INPUT-NN-013 ([RES-NN-001](feasibility-spike.md#input-nn-013)); repository initialized 2026-10-30 | Version history and pull-request workflow |
| Q-NN-008 | Which inputs are in the MVP: query text, error messages, plans, deadlock graphs, grid selection, Object Explorer objects? | Bryan | Before the PRD | PRD scope |
| Q-NN-009 | Are secrets always redacted, even if a user's settings would allow them? | Bryan | Before designing presets | Settings model |
| Q-NN-010 | May a throwaway spike VSIX be installed into SSMS 22 on the owner's PC (not SSMS 21), and removed afterwards with `VSIXInstaller.exe /uninstall`? | Bryan | Answered yes by INPUT-NN-010 ([RES-NN-001](feasibility-spike.md#input-nn-010)) | EXP-NN-001 (a) to (f) |
| Q-NN-011 | Which SSMS 22 install without the AI component is used for testing: a VM or Windows Sandbox with a fresh core-only install, or temporarily removing the AI workload from this PC with the Visual Studio Installer? | Bryan | Before the spike's second run | EXP-NN-001 without the AI component; evidence for C-08 |

## Exit criteria

- [x] The problem, users, current workaround, and non-goals are explicit.
- [x] Evidence is distinguishable from hypotheses and generated suggestions.
- [x] Alternatives and the cost of doing nothing have been considered.
- [x] The next experiment or direction has an owner and falsifiable decision rule.
- [x] Blockers and the handoff artifact are recorded; no implementation readiness is implied.
