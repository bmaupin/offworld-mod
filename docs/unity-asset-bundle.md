# Unity asset bundles

## Create a Unity asset bundle

ⓘ Offworld can only read assets (e.g. music files) stored in a Unity asset bundle, which must be created using the Unity Editor. The version must exactly match the version of Unity used by the game.

1. Get the exact Unity version from the game log (Documents/My Games/Offworld/Logs/output.txt)
   - Currently `Unity Version: 2022.3.39f1` (https://unity.com/releases/editor/whats-new/2022.3.39f1)

1. Download and extract [Unity Editor 2022.3.39f1](https://unity.com/releases/editor/whats-new/2022.3.39f1)
   - TODO: Project needs _Windows Build Support (Mono)_

1. Download and install the [Unity Hub](https://docs.unity.com/hub/install-hub)

1. Open Unity Hub
   - This will allow activation of the licence

1. Start Unity Editor
   - This will add it to Unity Hub

1. Manually create a Unity 2022.3.39f1 project

   ```
   ./Unity -batchmode -quit -createProject "/home/$USER/Desktop/tmp-offworld/TestProject" -logFile -
   ```

1. Open the project in Unity Hub
   1. In Unity Hub go to _Projects_

   1. Click _Add_ > _Add project from disk_

   1. Select the version of Unity as the one you downloaded manually

1. Import assets
   1. _Assets_ > _Import New Asset_

   1. Browse to the files to import (you can import many at a time)

   1. If you wish to organise the assets, you can right-click Assets (in the Project folder at the bottom of the screen), create a new directory, and drag assets into it

1. Create the asset bundle
   1. In Assets, select all of the folders containing the assets you wish in the bundle

   1. This will show all of the assets in the main Project pain. Select them all (Ctrl+A or drag a box around them)

   1. Make sure _Load in Background_ is unchecked

   1. Make sure _Preload Audio Data_ is checked

   1. In the Inspector pane on the bottom right if you only see _N Audio Clips_, drag from the top of that box until you see _AssetBundle_

   1. Click the first dropdown near AssetBundle, then click the down arrow > _New_

   1. Type a name, e.g. `modassets`

1. Build the asset bundle ([source](https://docs.unity3d.com/2022.3/Documentation/Manual/AssetBundles-Workflow.html))
   1. Create a folder in Assets named `Editor`

   1. Right-click in the Editor folder > _Create_ > _C# Script_

   1. (Optional) Rename the script, e.g. `BuildAssetBundles.cs`

   1. Double-click the script and paste these contents

      ```c#
      using UnityEditor;
      using System.IO;

      public class CreateAssetBundles
      {
          [MenuItem("Assets/Build AssetBundles")]
          static void BuildAllAssetBundles()
          {
              // Ensure the AssetBundles directory exists, and if it doesn't, create it.
              string assetBundleDirectory = "Assets/AssetBundles";
              if (!Directory.Exists(assetBundleDirectory))
                  Directory.CreateDirectory(assetBundleDirectory);

              // Build all AssetBundles and place them in the specified directory.
              BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                              BuildAssetBundleOptions.None,
                                              BuildTarget.StandaloneWindows);
          }
      }
      ```

   1. This will add a new item to the game menu; right-click Assets > _Build AssetBundles_

## Reference assets in an asset bundle

- Case does not matter; paths and filenames are always lower-cased in the bundle and in the game code that loads the bundle
- Leave out the extension
- Replace `assets` in the asset path with the name of the asset file
  - e.g. if the asset is `assets/asset.mp3` use `assetbundlefile/asset`
- ⚠️ It's not recommended to put assets in a folder inside the bundle
  - If you put assets in a folder inside the bundle, they need to be referenced like this:

    ```xml
    <zAsset>modassets\folder/asset</zAsset>
    ```

    - Note the backslash; this is because `AssetBundleManager` in the game uses `Path.GetDirectoryName` to parse the asset Component value (e.g. `assets/folder/asset.mp3`) which converts the slash to a backslash
    - 👉 It's not clear whether this would be cross-platform; the Mac build may not have the same behaviour since it's likely that `Path.GetDirectoryName` wouldn't convert a slash to a backslash
