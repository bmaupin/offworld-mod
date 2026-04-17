## Modding tutorials

https://mohawkgames.com/2016/02/03/modding-tutorials/

#### Create a mod

https://www.youtube.com/watch?v=CDU-TOUBvos

1. Create a new directory in Documents/Games/Offworld/Mods with the name of the mod
1. Inside the mod directory, create a directory named `Data`
1. Start the game to extract data to Documents/My Games/Offworld/Mods/Hidden/Reference
   - These files will be used as references for the sections below

#### Override a file (not recommended)

https://www.youtube.com/watch?v=CDU-TOUBvos

1. In your mod's Data directory, copy one of the files from `Reference/Data` and change as needed

#### Add to a file

https://www.youtube.com/watch?v=tJLLzFsMQ6Q

1. In your mod's Data directory, create a file with the same name as a file from `Reference/Data` but append `-add`, e.g. `audio-add.xml`
1. Start with this:
   ```xml
   <?xml version="1.0"?>
   <Root>
       <Entry>
       </Entry>
   </Root>
   ```
1. TODO

#### Change a file

https://www.youtube.com/watch?v=tJLLzFsMQ6Q

1. In your mod's Data directory, create a file with the same name as a file from `Reference/Data` but append `-change`, e.g. `audio-change.xml`
1. Start with this:
   ```xml
   <?xml version="1.0"?>
   <Root>
   </Root>
   ```
1. Copy an existing `Entry` from the reference data that you want to change and paste it inside `Root`, e.g.
   ```xml
   <Entry>
   	<zType>AUDIO_MAIN_MENU_LOOP_MUSIC</zType>
   	<zAsset>Soundtrack/RedPlanetNocturne_Mixv2_Looped</zAsset>
   	<zClassicAsset/>
   </Entry>
   ```

## Assets

Assets must be in a Unity asset bundle. This can be created using Unity tools.

1. Get the exact Unity version from the game log
   - Currently `Unity Version: 2022.3.39f1` (https://unity.com/releases/editor/whats-new/2022.3.39f1)

## Game source

- The source game audio file is at ~/.local/share/Steam/steamapps/compatdata/271240/pfx/drive_c/users/steamuser/Documents/My Games/Offworld/Mods/Hidden/Reference/Data/audio.xml
  - This gets extracted when the game gets first run
- The assets seem to be in ~/.local/share/Steam/steamapps/common/Offworld Trading Company/Offworld_Data/resources.assets

## Continuous Innovation

The [Continuous Innovation](https://steamcommunity.com/sharedfiles/filedetails/?id=1343925506) mod has added audio

- Audio is inside Assets/modassets
  - This seems to be some kind of "UnityFS" binary file

## Troubleshooting

See logs in Documents/My Games/Offworld/Logs
