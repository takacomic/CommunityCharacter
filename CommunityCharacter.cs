using CommunityCharacter;
using MelonLoader;

[assembly: MelonInfo(typeof(CommunityCharacterMod), ModInfo.Name, ModInfo.Version, ModInfo.Author, ModInfo.Download)]
[assembly: MelonGame("poncle", "Vampire Survivors")]
//[assembly: MelonAdditionalDependencies("SaveDataInvestigator")]

namespace CommunityCharacter
{

    internal static class ModInfo
    {
        public const string Name = "CommunityCharacter";
        public const string Author = "Takacomic";
        public const string Version = "0.3.0";
        public const string Download = "https://github.com/takacomic/.../latest";
    }
    public class CommunityCharacterMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
        }

        public override void OnGUI()
        {

        }
    }
}
