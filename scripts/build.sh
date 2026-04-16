#!/usr/bin/env bash

mod_name="BeyondEarthSoundtrack"
offworld_managed_dir="$HOME/.local/share/Steam/steamapps/common/Offworld Trading Company/Offworld_Data/Managed"
offworld_reference_dir="$HOME/.local/share/Steam/steamapps/compatdata/271240/pfx/drive_c/users/steamuser/Documents/My Games/Offworld/Mods/Hidden/Reference"
dll_filename={mod_name}.dll

if [[ $build_type == "development" ]]; then
    dll_filename="${mod_name}-$(date +%s).dll"
fi

rm src/*.dll 2>/dev/null

echo "Compiling ..."
mcs \
    -target:library \
    -sdk:2 \
    -out:"src/${dll_filename}" \
    -r:"${offworld_reference_dir}/Assembly-CSharp-firstpass.dll" \
    -r:"${offworld_managed_dir}/UnityEngine.dll" \
    -r:"${offworld_managed_dir}/UnityEngine.CoreModule.dll" \
    -r:"${offworld_managed_dir}/netstandard.dll" \
    "src/Source/*"
