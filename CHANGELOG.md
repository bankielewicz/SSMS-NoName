# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## Unreleased

### Added

- NoName Diagnostics 0.1.0, a test-bench SSMS 22 extension (`NoName.Diagnostics.Vsix`) that loads in the background, adds a Tools-menu and a query-window command, and logs every call, result, error and click to one JSON Lines file per SSMS session.
- `NoName.Diagnostics.Core`, the netstandard2.0 library behind it: the session log, the sanitizer that logs values as length and hash unless verbose mode is on and never logs connection-string secrets, and the probe runner that records each API call's outcome, time limit and cancellation.
- CI on GitHub Actions: the Core library's tests on Ubuntu with .NET 8 behind a 95% line-coverage gate (`scripts/coverage_gate.py`), which is the required check for merging to `main`, and the same tests on Windows on net8.0 and net48.
- The MIT licence (`LICENSE`).
- The planning documents under `docs/specs/`: the suite brainstorm BRN-001 (version 2), the release design DSN-001, the product requirements PRD-001 (version 2, draft), the first brainstorm IDEA-NN-001 and the feasibility spike RES-NN-001, with their provenance register.
