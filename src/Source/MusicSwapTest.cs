// ⚠️⚠️⚠️🤖🤖🤖 This file was written entirely by AI 🤖🤖🤖⚠️⚠️⚠️

using Offworld.AppCore;
using Offworld.GameCore;
using UnityEngine;

public class MusicSwapTest : ModEntryPointAdapter
{
    private static readonly string[] Songs = new string[] { "A", "B", "C", "D", "E", "F" };

    public override void OnGameClientReady()
    {
        ApplyTrackOneToTrackTwelveSwap("OnGameClientReady");
    }

    public override void OnPostLoadGameClient()
    {
        ApplyTrackOneToTrackTwelveSwap("OnPostLoadGameClient");
    }

    private static void ApplyTrackOneToTrackTwelveSwap(string source)
    {
        if (Globals.Infos == null)
        {
            Debug.Log("[MusicSwapTest] Infos is null in " + source + "; skipping");
            return;
        }

        int swaps = 0;

        foreach (string song in Songs)
        {
            string node1Type = "AUDIO_GAME_MUSIC_NODE_" + song + "1";
            string node12Type = "AUDIO_GAME_MUSIC_NODE_" + song + "12";

            AudioTypeT node1 = Globals.Infos.getType<AudioTypeT>(node1Type, false);
            AudioTypeT node12 = Globals.Infos.getType<AudioTypeT>(node12Type, false);

            if (node1 == AudioTypeT.NONE || node12 == AudioTypeT.NONE)
            {
                Debug.Log("[MusicSwapTest] Missing type: " + node1Type + " or " + node12Type);
                continue;
            }

            InfoAudio info1 = Globals.Infos.audio(node1);
            InfoAudio info12 = Globals.Infos.audio(node12);

            if (info1 == null || info12 == null)
            {
                Debug.Log("[MusicSwapTest] Missing audio info: " + node1Type + " or " + node12Type);
                continue;
            }

            info1.mzAsset = info12.mzAsset;
            info1.mzClassicAsset = info12.mzClassicAsset;
            swaps++;

            Debug.Log("[MusicSwapTest] " + node1Type + " now uses asset from " + node12Type + ": " + info1.mzAsset);
        }

        Debug.Log("[MusicSwapTest] Applied swaps from " + source + ": " + swaps + " song(s)");
    }
}
