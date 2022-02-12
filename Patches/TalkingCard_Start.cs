using System.Collections.Generic;
using System.Reflection;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace KCStoat.Patches;

[HarmonyPatch(typeof(TalkingCard), "Start")]
public class TalkingCard_Start
{
    public static void Postfix(TalkingCard __instance)
    {
        if (SaveFile.IsAscension)
        {
            // Find the CharacterFace (Prefab)/Anim/Body::SpriteRenderer
            var renderer = __instance.Face.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();

            // Get the list of emotions so we can rip the face
            var info = typeof(CharacterFace).GetField("emotionSprites", BindingFlags.Instance | BindingFlags.NonPublic);
            var emotions = (List<CharacterFace.EmotionSprites>)info.GetValue(__instance.Face);

            // Get rid of dumb PO3 and replace it with cute stoat
            renderer.sprite = emotions[2].face;
        }
    }
}