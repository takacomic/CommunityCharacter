using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Objects.Characters;
using MelonLoader;
using UnityEngine;

namespace CommunityCharacter
{

    internal static class ModInfo
    {
        public const string Name = "CommunityCharacter";
        public const string Description = "";
        public const string Author = "Takacomic";
        public const string Company = "CorruptedInfluences";
        public const string Version = "0.2.0";
        public const string Download = "https://github.com/takacomic/.../latest";
    }
    public class CommunityCharacterMod : MelonMod
    {
        /*public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            /*ClassInjector.RegisterTypeInIl2Cpp<TestController>();
            base.OnSceneWasLoaded(buildIndex, sceneName);
        }*/
    }
}
