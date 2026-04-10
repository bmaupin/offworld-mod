#!/usr/bin/env bash

# ⚠️⚠️⚠️ This file was written entirely by AI

set -euo pipefail

# Extract soundtrack-related AudioClips from Offworld resources.assets.
# Usage:
#   ./scripts/extract-soundtrack.sh [offworld_data_dir] [output_dir]

OFFWORLD_DATA_DIR="${1:-$HOME/.local/share/Steam/steamapps/common/Offworld Trading Company/Offworld_Data}"
OUTPUT_DIR="${2:-$PWD/tmp/extracted-soundtrack}"
ASSET_FILE="${OFFWORLD_DATA_DIR}/resources.assets"
VENV_DIR="/tmp/offworld-unitypy-venv"

if [[ ! -f "${ASSET_FILE}" ]]; then
  echo "Could not find ${ASSET_FILE}" >&2
  exit 1
fi

if [[ ! -x "${VENV_DIR}/bin/python" ]]; then
  python3 -m venv "${VENV_DIR}"
  "${VENV_DIR}/bin/pip" install --quiet UnityPy
fi

mkdir -p "${OUTPUT_DIR}"

"${VENV_DIR}/bin/python" - <<'PY' "${ASSET_FILE}" "${OUTPUT_DIR}"
import re
import sys
from pathlib import Path
from collections import defaultdict
import wave
import UnityPy

asset_file = Path(sys.argv[1])
out_dir = Path(sys.argv[2])
out_dir.mkdir(parents=True, exist_ok=True)

pattern = re.compile(r"^[A-F](?:0[1-9]|1[0-5])_Edit[0-9]+_[0-9]+$")
menu_names = {"RedPlanetNocturne_Mixv2_Looped", "Opening_Video"}

env = UnityPy.load(str(asset_file))
rows = []

for obj in env.objects:
    if obj.type.name != "AudioClip":
        continue

    clip = obj.read()
    name = (getattr(clip, "m_Name", "") or "").strip()
    if not (pattern.match(name) or name in menu_names):
        continue

    for sample_name, blob in clip.samples.items():
        out_file = out_dir / sample_name
        out_file.parent.mkdir(parents=True, exist_ok=True)
        out_file.write_bytes(blob)

        duration = None
        try:
            with wave.open(str(out_file), "rb") as wf:
                duration = wf.getnframes() / float(wf.getframerate())
        except Exception:
            duration = None

        rows.append(
            {
                "clip": name,
                "file": sample_name,
                "channels": getattr(clip, "m_Channels", None),
                "frequency": getattr(clip, "m_Frequency", None),
                "compression": getattr(clip, "m_CompressionFormat", None),
                "load_type": getattr(clip, "m_LoadType", None),
                "duration": duration,
            }
        )

rows.sort(key=lambda r: r["clip"])

report = out_dir / "soundtrack-report.tsv"
with report.open("w", encoding="utf-8") as f:
    f.write("clip\tfile\tchannels\tfrequency\tcompression\tload_type\tduration_seconds\n")
    for r in rows:
        dur = "" if r["duration"] is None else f"{r['duration']:.2f}"
        f.write(
            f"{r['clip']}\t{r['file']}\t{r['channels']}\t{r['frequency']}\t{r['compression']}\t{r['load_type']}\t{dur}\n"
        )

totals = defaultdict(float)
counts = defaultdict(int)
for r in rows:
    if len(r["clip"]) >= 3 and r["clip"][0] in "ABCDEF" and r["clip"][1:3].isdigit() and r["duration"]:
        bucket = r["clip"][0]
        totals[bucket] += r["duration"]
        counts[bucket] += 1

print(f"Extracted {len(rows)} soundtrack clips to {out_dir}")
print(f"Wrote report: {report}")
for key in sorted(counts):
    print(f"Song {key}: {counts[key]} nodes, total {totals[key]:.2f}s")
PY

printf "\nDone. Output directory: %s\n" "${OUTPUT_DIR}"
