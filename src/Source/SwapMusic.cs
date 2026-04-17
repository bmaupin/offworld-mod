using Offworld.AppCore;
using Offworld.GameCore;
using System.Collections.Generic;
using UnityEngine;

public class SwapMusic : ModEntryPointAdapter {
    private static readonly string[] Songs = new string[] { "A", "B", "C", "D", "E", "F" };
    private static readonly System.Random Random = new System.Random();

    // Get mod entry point methods from IModEntryPoint.cs
    // Initialize gets called only once at mod load
    // The mod may get unloaded, e.g. if Daily Challenge or Infinite Map Challenge are selected; these seem to disable mods
    public override void Initialize() {
        ReplaceGameSoundtrack();
    }

    private static void ReplaceGameSoundtrack() {
        Debug.Log("[SwapMusic] ReplaceGameSoundtrack()");

        var customSoundtrackTracks = new List<string>();
        for (int i = 1; i <= 67; i++) {
            customSoundtrackTracks.Add($"AUDIO_CUSTOM_SOUNDTRACK_{i}");
        }

        for (int songIndex = 0; songIndex < Songs.Length; songIndex++) {
            string song = Songs[songIndex];

            for (int sourceTrack = 2; sourceTrack <= 11; sourceTrack++) {
                string sourceType = "AUDIO_GAME_MUSIC_NODE_" + song + sourceTrack;

                // Pick a random track and then pop it off the array so each track doesn't get picked more than once
                int randomTrackIndex = Random.Next(0, customSoundtrackTracks.Count);
                string replacementType = customSoundtrackTracks[randomTrackIndex];
                customSoundtrackTracks.RemoveAt(randomTrackIndex);

                AudioTypeT sourceNode = Globals.Infos.getType<AudioTypeT>(sourceType, false);
                AudioTypeT replacementNode = Globals.Infos.getType<AudioTypeT>(replacementType, false);

                if (sourceNode == AudioTypeT.NONE || replacementNode == AudioTypeT.NONE) {
                    Debug.Log("[SwapMusic] Missing type: " + sourceType + " or " + replacementType);
                    continue;
                }

                InfoAudio sourceInfo = Globals.Infos.audio(sourceNode);
                InfoAudio replacementInfo = Globals.Infos.audio(replacementNode);

                if (sourceInfo == null || replacementInfo == null) {
                    Debug.Log("[SwapMusic] Missing audio info: " + sourceType + " or " + replacementType);
                    continue;
                }

                sourceInfo.mzAsset = replacementInfo.mzAsset;

                Debug.Log("[SwapMusic] " + sourceType + " now uses asset from " + replacementType + ": " + sourceInfo.mzAsset);
            }
        }
    }
}
