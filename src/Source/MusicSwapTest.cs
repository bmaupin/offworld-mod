// ⚠️⚠️⚠️🤖🤖🤖 This file was written entirely by AI 🤖🤖🤖⚠️⚠️⚠️

using Offworld.AppCore;
using Offworld.GameCore;
using UnityEngine;

public class MusicSwapTest : ModEntryPointAdapter
{
    private static readonly string[] Songs = new string[] { "A", "B", "C", "D", "E", "F" };
    private static readonly System.Random Random = new System.Random();
    private static readonly int[,] SelectedTrackNumbers = new int[6, 10];
    private static bool sessionRandomisationInitialised;

    public override void Initialize()
    {
        EnsureSessionRandomisation();
    }

    public override void OnGameClientReady()
    {
        ApplyGameplayTrackRandomSwaps("OnGameClientReady");
    }

    public override void OnPostLoadGameClient()
    {
        ApplyGameplayTrackRandomSwaps("OnPostLoadGameClient");
    }

    private static void EnsureSessionRandomisation()
    {
        if (sessionRandomisationInitialised)
        {
            return;
        }

        for (int songIndex = 0; songIndex < Songs.Length; songIndex++)
        {
            for (int sourceTrack = 2; sourceTrack <= 11; sourceTrack++)
            {
                SelectedTrackNumbers[songIndex, sourceTrack - 2] = Random.Next(2, 12);
            }
        }

        sessionRandomisationInitialised = true;

        for (int songIndex = 0; songIndex < Songs.Length; songIndex++)
        {
            string summary = "";
            for (int sourceTrack = 2; sourceTrack <= 11; sourceTrack++)
            {
                if (summary.Length > 0)
                {
                    summary += ", ";
                }

                summary += sourceTrack + "->" + SelectedTrackNumbers[songIndex, sourceTrack - 2];
            }

            Debug.Log("[MusicSwapTest] Song " + Songs[songIndex] + " session mapping: " + summary);
        }
    }

    private static void ApplyGameplayTrackRandomSwaps(string source)
    {
        if (Globals.Infos == null)
        {
            Debug.Log("[MusicSwapTest] Infos is null in " + source + "; skipping");
            return;
        }

        EnsureSessionRandomisation();

        int swaps = 0;

        for (int songIndex = 0; songIndex < Songs.Length; songIndex++)
        {
            string song = Songs[songIndex];

            for (int sourceTrack = 2; sourceTrack <= 11; sourceTrack++)
            {
                int replacementTrack = SelectedTrackNumbers[songIndex, sourceTrack - 2];
                string sourceType = "AUDIO_GAME_MUSIC_NODE_" + song + sourceTrack;
                string replacementType = "AUDIO_GAME_MUSIC_NODE_" + song + replacementTrack;

                AudioTypeT sourceNode = Globals.Infos.getType<AudioTypeT>(sourceType, false);
                AudioTypeT replacementNode = Globals.Infos.getType<AudioTypeT>(replacementType, false);

                if (sourceNode == AudioTypeT.NONE || replacementNode == AudioTypeT.NONE)
                {
                    Debug.Log("[MusicSwapTest] Missing type: " + sourceType + " or " + replacementType);
                    continue;
                }

                InfoAudio sourceInfo = Globals.Infos.audio(sourceNode);
                InfoAudio replacementInfo = Globals.Infos.audio(replacementNode);

                if (sourceInfo == null || replacementInfo == null)
                {
                    Debug.Log("[MusicSwapTest] Missing audio info: " + sourceType + " or " + replacementType);
                    continue;
                }

                sourceInfo.mzAsset = replacementInfo.mzAsset;
                sourceInfo.mzClassicAsset = replacementInfo.mzClassicAsset;
                swaps++;

                Debug.Log("[MusicSwapTest] " + sourceType + " now uses asset from " + replacementType + ": " + sourceInfo.mzAsset);
            }
        }

        Debug.Log("[MusicSwapTest] Applied gameplay random swaps from " + source + ": " + swaps + " node(s)");
    }
}
