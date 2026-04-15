#!/usr/bin/env bash

# ⚠️⚠️⚠️🤖🤖🤖 This file was written entirely by AI 🤖🤖🤖⚠️⚠️⚠️

set -euo pipefail

MOD_ROOT="$(cd "$(dirname "$0")/.." && pwd)"
SRC_FILE="${MOD_ROOT}/src/Source/MusicSwapTest.cs"
OUT_DLL="${MOD_ROOT}/src/MusicSwapTest.dll"
REF_DIR="$HOME/.local/share/Steam/steamapps/compatdata/271240/pfx/drive_c/users/steamuser/Documents/My Games/Offworld/Mods/Hidden/Reference"
MANAGED_DIR="$HOME/.local/share/Steam/steamapps/common/Offworld Trading Company/Offworld_Data/Managed"

REF_GAME="${REF_DIR}/Assembly-CSharp-firstpass.dll"
REF_UNITY="${MANAGED_DIR}/UnityEngine.dll"
REF_UNITY_CORE="${MANAGED_DIR}/UnityEngine.CoreModule.dll"
REF_NETSTANDARD="${MANAGED_DIR}/netstandard.dll"

if [[ ! -f "$SRC_FILE" ]]; then
    echo "Source file not found: $SRC_FILE" >&2
    exit 1
fi

if [[ ! -f "$REF_GAME" ]]; then
    echo "Reference DLL not found: $REF_GAME" >&2
    exit 1
fi

if [[ ! -f "$REF_UNITY" ]]; then
    echo "Reference DLL not found: $REF_UNITY" >&2
    exit 1
fi

if [[ ! -f "$REF_UNITY_CORE" ]]; then
    echo "Reference DLL not found: $REF_UNITY_CORE" >&2
    exit 1
fi

if [[ ! -f "$REF_NETSTANDARD" ]]; then
    echo "Reference DLL not found: $REF_NETSTANDARD" >&2
    exit 1
fi

mkdir -p "${MOD_ROOT}/src"

echo "Compiling MusicSwapTest.dll ..."
mcs \
    -target:library \
    -sdk:2 \
    -out:"$OUT_DLL" \
    -r:"$REF_GAME" \
    -r:"$REF_UNITY" \
    -r:"$REF_UNITY_CORE" \
    -r:"$REF_NETSTANDARD" \
    "$SRC_FILE"

echo "Built: $OUT_DLL"
