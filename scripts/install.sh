#!/usr/bin/env bash

source "$(dirname "$(which "$0")")/build.sh"

mod_name="Beyond Earth Soundtrack"
mod_directory="/home/$USER/.local/share/Steam/steamapps/compatdata/271240/pfx/drive_c/users/steamuser/Documents/My Games/Offworld/Mods/${mod_name}"

echo "Copying mod files ..."
rm -rf "${mod_directory}"
mkdir "${mod_directory}"
cp -r src/* "${mod_directory}"
