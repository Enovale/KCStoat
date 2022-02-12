using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DiskCardGame;
using HarmonyLib;

namespace KCStoat.Patches
{
    [HarmonyPatch(typeof(AscensionSaveData), nameof(AscensionSaveData.NewRun), typeof(List<CardInfo>))]
    public class AscensionSaveData_NewRun
    {
        public static void Postfix(List<CardInfo> starterDeck, AscensionSaveData __instance)
        {
            if (__instance.currentRun.playerDeck.Cards.Exists(c => c.name.Contains("Stoat")))
            {
                var oldStoat = __instance.currentRun.playerDeck.Cards.First(c => c.name.Contains("Stoat"));
                var stoatIndex = __instance.currentRun.playerDeck.Cards.IndexOf(oldStoat);
                __instance.currentRun.playerDeck.RemoveCard(oldStoat);
                var newStoat = CardLoader.GetCardByName("Stoat_Talking");
                __instance.currentRun.playerDeck.Cards.Insert(stoatIndex, newStoat);

                // Dumb reflection thing
                var field = typeof(CardCollectionInfo).GetField("cardIds",
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);
                var list = (List<string>)field.GetValue(__instance.currentRun.playerDeck);
                list.Insert(stoatIndex, newStoat.name);

                __instance.currentRun.playerDeck.ModifyCard(newStoat, new CardModificationInfo(0, -1));
            }
        }
    }
}