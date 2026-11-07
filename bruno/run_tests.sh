#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "${SCRIPT_DIR}"

ENV_NAME="${1:-Local-MsSql}"
shift || true

# The 06-Mutations suite needs role tokens; mint dev tokens (HS256, local-only) unless provided.
DEV_TOKEN_SCRIPT="${SCRIPT_DIR}/../../crm-infra/scripts/dev-token.sh"
if [ -x "${DEV_TOKEN_SCRIPT}" ]; then
    export CRM_ADMIN_TOKEN="${CRM_ADMIN_TOKEN:-$("${DEV_TOKEN_SCRIPT}" admin)}"
    export CRM_USER_TOKEN="${CRM_USER_TOKEN:-$("${DEV_TOKEN_SCRIPT}" user)}"
fi

BRU_BIN="bru"
if ! command -v bru >/dev/null 2>&1 && [ -x "/opt/homebrew/bin/bru" ]; then
    BRU_BIN="/opt/homebrew/bin/bru"
fi

exec "${BRU_BIN}" run . -r --env "${ENV_NAME}" "$@"
