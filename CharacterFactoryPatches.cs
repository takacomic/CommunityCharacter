using HarmonyLib;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Objects.Characters;
using MelonLoader;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace CommunityCharacter
{
    /*[HarmonyPatch(typeof(CharacterFactory))]
    class CharacterFactoryPatch
    {
        [HarmonyPatch(nameof(CharacterFactory.GetCharacterPrefab))]
        [HarmonyPostfix]
        static void PostFix(CharacterType characterType, ref CharacterController __result)
        {
            CharacterController character;
            CharacterControllerCommunity characterCommunity = __result as CharacterControllerCommunity;
            Melon<CommunityCharacterMod>.Logger.Msg(__result);
            __result = characterCommunity;
        }
    }*/
}
