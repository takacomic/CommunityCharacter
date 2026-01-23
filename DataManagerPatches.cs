using HarmonyLib;
using Il2CppSystem.Reflection;
using Il2CppVampireSurvivors.Data;
using Newtonsoft.Json;
using CoffinTech.SaveData;
using CoffinTech.Utils;
using UnityEngine;

namespace CommunityCharacter
{
    internal static class DataManagerPatches
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
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string[] assets =
            {
                "character_community.png", "character_community_beta.png", "character_community_decapo.png",
                "character_community_festa.png", "character_community_hermies.png", "character_community_opal.png",
                "character_community_tempo.png", "character_community_vam.png", "character_community_zeta.png",
                "character_community_zeta_black.png", "character_community_zeta_city.png", "character_community_zeta_directer.png",
                "character_community_zeta_moon.png", "character_community_zeta_seawinds.png", "character_community_zeta_stone.png",
                "character_community_zeta_sun.png", "character_community_zeta_volcano.png", "character_community_outerRing.png", 
                "character_community_innerRing.png"
            };

            foreach (var asset in assets)
            {
                Texture2D texture = CoffinTech.Utils.SpriteImporter.LoadTextureFromAssembly(assembly, "CommunityCharacter.Assets",asset);
                if (asset == "character_community.png")
                    SpriteImporter.SpriteStrip(texture, "community", 1);
                else if (asset == "character_community_outerRing.png" || asset == "character_community_innerRing.png")
                    SpriteImporter.SpriteStrip(texture, "cc_" + asset.Split("community_").Last().Split('.').First(), 1);
                else
                    SpriteImporter.SpriteStrip(texture, asset.Split("community_").Last().Split('.').First(), 4);
            }
        }

        internal static readonly JsonSerializerSettings SerializerSettings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        private static void CharacterRegister(DataManager __instance)
        {
            CharacterType characterType = (CharacterType)20000;
            ModOptionsData.SetCharacterId(characterType, "DACommunityCharacter");
            ModCharacterControllerRegistry.Register(ModCharacterController.GetInstance<CharacterControllerAssistants>(), characterType.ToString());

            __instance._allCharactersJson.Add(characterType.ToString(), Assistants.Defaults());
        }
    }
}
