using HarmonyLib;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Objects.Characters;
using MelonLoader;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace CommunityCharacter
{
    [HarmonyPatch(typeof(CharacterFactory))]
    class CharacterFactoryPatch
    {
        [HarmonyPatch(nameof(CharacterFactory.GetCharacterPrefab))]
        [HarmonyPostfix]
        static void PostFix(CharacterType characterType, ref CharacterController __result)
        {
            
            __result.gameObject.GetComponent<CharacterController>().enabled = false;
            __result.gameObject.AddComponent<CharacterControllerCommunity>();
            UnityEngine.Object.Destroy(__result.gameObject.GetComponent<CharacterController>());
            MelonLogger.Msg(__result.GetComponent<CharacterController>().name);
            Melon<CommunityCharacterMod>.Logger.Msg(__result);
            //__result = __result.gameObject.GetComponent<CharacterControllerCommunity>();
        }
    }
}
