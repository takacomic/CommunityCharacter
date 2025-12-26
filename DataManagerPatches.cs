using HarmonyLib;
using Il2CppNewtonsoft.Json.Linq;
using Il2CppSystem.Reflection;
using Il2CppVampireSurvivors.App.Data;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Graphics;
using Il2CppVampireSurvivors.Objects.Characters;
using MelonLoader;
using Newtonsoft.Json;
using UnityEngine;

namespace CommunityCharacter
{
    static class DataManagerPatches
    {
        [HarmonyPatch(typeof(DataManager))]
        static class DataManagerPatch
        {
            [HarmonyPatch(nameof(DataManager.LoadBaseJObjects))]
            [HarmonyPostfix]
            static void LoadBaseJObjects_Postfix(DataManager __instance, object[] __args, MethodBase __originalMethod)
            {
                SpriteRegister();
                CharacterRegister(__instance);
            }
        }

        static void SpriteRegister()
        {

            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community.png"), "community", 1);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_beta.png"), "beta", 4);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_decapo.png"), "decapo", 4);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_festa.png"), "festa", 4);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_hermies.png"), "hermies", 4);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_opal.png"), "opal", 4);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_tempo.png"), "tempo", 4);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_vam.png"), "vam", 4);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_zeta.png"), "zeta", 4);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_zeta_black.png"), "zeta_black", 4);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_zeta_city.png"), "zeta_city", 4);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_zeta_directer.png"), "zeta_directer", 4);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_zeta_moon.png"), "zeta_moon", 4);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_zeta_seawinds.png"), "zeta_seawinds", 4);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_zeta_stone.png"), "zeta_stone", 4);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_zeta_sun.png"), "zeta_sun", 4);
            SpriteImporter.SpriteStrip(SpriteImporter.LoadTexture("character_community_zeta_volcano.png"), "zeta_volcano", 4);
        }

        internal static readonly JsonSerializerSettings SerializerSettings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        static void CharacterRegister(DataManager __instance)
        {
            CharacterType characterType = (CharacterType)20000;

            __instance._allCharactersJson.Add(characterType.ToString(), BaseStats.Defaults());
        }
    }
}
