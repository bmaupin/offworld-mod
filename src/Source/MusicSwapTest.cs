// ⚠️⚠️⚠️🤖🤖🤖 This file was written entirely by AI 🤖🤖🤖⚠️⚠️⚠️

using Offworld.AppCore;
using Offworld.GameCore;
using UnityEngine;

public class MusicSwapTest : ModEntryPointAdapter
{
    private static readonly string[] Songs = new string[] { "A", "B", "C", "D", "E", "F" };
    private static readonly System.Random Random = new System.Random();
    private static int selectedTrackNumber = -1;

    public override void Initialize()
    {
        if (selectedTrackNumber < 2)
        {
            // Pick once per Offworld launch. Reused for all matches in this session.
            selectedTrackNumber = Random.Next(2, 16);
        }

        Debug.Log("[MusicSwapTest] Selected replacement track index: " + selectedTrackNumber);
    }

    public override void OnGameClientReady()
    {
        ApplyTrackOneRandomSwap("OnGameClientReady");
    }

    public override void OnPostLoadGameClient()
    {
        ApplyTrackOneRandomSwap("OnPostLoadGameClient");
    }

    private static void ApplyTrackOneRandomSwap(string source)
    {
        if (Globals.Infos == null)
        {
            Debug.Log("[MusicSwapTest] Infos is null in " + source + "; skipping");
            return;
        }

        if (selectedTrackNumber < 2)
        {
            selectedTrackNumber = Random.Next(2, 16);
            Debug.Log("[MusicSwapTest] Late-selected replacement track index: " + selectedTrackNumber);
        }

        int swaps = 0;

        foreach (string song in Songs)
        {
            string node1Type = "AUDIO_GAME_MUSIC_NODE_" + song + "1";
            string replacementType = "AUDIO_GAME_MUSIC_NODE_" + song + selectedTrackNumber;

            AudioTypeT node1 = Globals.Infos.getType<AudioTypeT>(node1Type, false);
            AudioTypeT replacement = Globals.Infos.getType<AudioTypeT>(replacementType, false);

            if (node1 == AudioTypeT.NONE || replacement == AudioTypeT.NONE)
            {
                Debug.Log("[MusicSwapTest] Missing type: " + node1Type + " or " + replacementType);
                continue;
            }

            InfoAudio info1 = Globals.Infos.audio(node1);
            InfoAudio replacementInfo = Globals.Infos.audio(replacement);

            if (info1 == null || replacementInfo == null)
            {
                Debug.Log("[MusicSwapTest] Missing audio info: " + node1Type + " or " + replacementType);
                continue;
            }

            info1.mzAsset = replacementInfo.mzAsset;
            info1.mzClassicAsset = replacementInfo.mzClassicAsset;
            swaps++;

            Debug.Log("[MusicSwapTest] " + node1Type + " now uses asset from " + replacementType + ": " + info1.mzAsset);
        }

        Debug.Log("[MusicSwapTest] Applied random swaps from " + source + ": " + swaps + " song(s)");
    }
}
