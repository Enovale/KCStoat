using DiskCardGame;
using HarmonyLib;

namespace KCStoat.Patches;

[HarmonyPatch(typeof(TalkingCard), nameof(TalkingCard.OnDrawn))]
public class TalkingCard_OnDrawn
{
    public static bool Prefix()
    {
        if (SaveFile.IsAscension)
        {
            // Make sure we don't interrupt the player on draw unless its against the prospector
            if (Singleton<TurnManager>.Instance.SpecialSequencer is not BossBattleSequencer ||
                ((BossBattleSequencer)Singleton<TurnManager>.Instance.SpecialSequencer).BossType != Opponent.Type.ProspectorBoss)
            {
                return false;
            }
        }

        return true;
    }
}