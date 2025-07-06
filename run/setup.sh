#!/usr/bin/env bash
set -euxo pipefail

DOTNET_SDK_VERSION=8.0

curl -sSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
bash /tmp/dotnet-install.sh --channel "$DOTNET_SDK_VERSION" --install-dir /usr/share/dotnet
export PATH="/usr/share/dotnet:$PATH"

dotnet restore

dotnet build -c Release-Codex
dotnet test -c Release-Codex
