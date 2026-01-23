using Il2CppVampireSurvivors.Graphics;
using MelonLoader;
using System.Reflection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;

namespace CommunityCharacter
{
    internal class SpriteImporter : MonoBehaviour
    {
        internal static Dictionary<string, Texture2D> textures = new();
        
        public static byte[] LoadFromAssembly(Assembly callingAssembly, string nameSpacePath, string filename)
        {
            ArgumentNullException.ThrowIfNull(callingAssembly);
            if (string.IsNullOrEmpty(nameSpacePath)) 
                throw new ArgumentException($"{nameof(nameSpacePath)} cannot be null or empty");
            if (string.IsNullOrEmpty(filename)) 
                throw new ArgumentException($"{nameof(filename)} cannot be null or empty");
        
            string resourceName = $"{nameSpacePath}.{filename}";

        
            using (Stream stream = callingAssembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) 
                    throw new ArgumentException($"Resource {resourceName} not found");
            
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }
        
        //Taken from sup3p's Pokesurvivors
        /*public unsafe static string LoadTexture(string resourceName)
        {

            // Get the assembly where the resource is embedded
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Format the full resource name: typically "Namespace.FolderName.FileName"
            string resourcePath = $"CommunityCharacter.Assets.{resourceName}";

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                {
                    Melon<CommunityCharacterMod>.Logger.Msg($"Resource {resourcePath} not found.");
                    Debug.LogError($"Resource {resourcePath} not found.");
                    return null;
                }

                // Read the image data from the stream
                byte[] imageData = new byte[stream.Length];
                stream.Read(imageData, 0, imageData.Length);

                // Load image data into a Texture2D
                Texture2D texture = new Texture2D(2, 2);
                ImageConversion.Lo

                if (!ImageConversion.LoadImage(texture, imageData))
                {
                    throw new Exception("ImageConversion.LoadImage failed");
                }

                texture.filterMode = FilterMode.Point;
                texture.name = resourceName.Split(".")[0];
                textures.TryAdd(texture.name, texture);
                return texture.name;
            }

        }*/
        internal static void SpriteStrip(Texture2D texture, string name, int spriteCount)
        {
            int baseWidth = texture.width / spriteCount;

            for (int i = 0; i < spriteCount; i++)
            {
                string nameAppend = name + "_0" + (i+1);
                Sprite sprite = LoadSprite(texture, new Rect(baseWidth * i, 0, baseWidth, texture.height), nameAppend);
                SpriteManager.RegisterSprite(sprite);
            }
        }
        internal static Sprite LoadSprite(Texture2D texture, Rect rect, string name)
        {
            Sprite sprite = Sprite.Create(texture, new Rect((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height), new Vector2(0.5f, 0.5f));
            sprite.name = name;
            return sprite;
        }
    }
}
