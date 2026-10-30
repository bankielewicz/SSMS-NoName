#!/usr/bin/env bash
# Run the .NET CLI from WSL (including inside Claude Code's sandbox) without writing to $HOME:
# NuGet packages go to the repo's ignored .cache/ folder, CLI state to a temp folder.
set -euo pipefail
repo="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
tmp="${TMPDIR:-/tmp}"
export DOTNET_CLI_HOME="$tmp/dotnet-cli-home"
export NUGET_PACKAGES="$repo/.cache/nuget/packages"
export NUGET_HTTP_CACHE_PATH="$tmp/nuget-http-cache"
export DOTNET_NOLOGO=1 DOTNET_CLI_TELEMETRY_OPTOUT=1 DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
# Package signatures are still verified; only the online certificate-revocation lookup is skipped.
export NUGET_CERT_REVOCATION_MODE=offline
mkdir -p "$DOTNET_CLI_HOME" "$NUGET_PACKAGES" "$NUGET_HTTP_CACHE_PATH"
exec "${DOTNET:-dotnet}" "$@"
