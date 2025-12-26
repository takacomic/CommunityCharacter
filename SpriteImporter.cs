using Il2CppVampireSurvivors.Graphics;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunityCharacter
{
    internal class SpriteImporter : MonoBehaviour
    {
        internal static Dictionary<string, Texture2D> textures = new();
        //Taken from sup3p's Pokesurvivors
        public unsafe static string LoadTexture(string resourceName)
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

                if (!ImageConversion.LoadImage(texture, imageData))
                {
                    throw new Exception("ImageConversion.LoadImage failed");
                }

                texture.filterMode = FilterMode.Point;
                texture.name = resourceName.Split(".")[0];
                textures.TryAdd(texture.name, texture);
                return texture.name;
            }

        }
        internal static void SpriteStrip(string textureName, string name, int spriteCount, int spriteRow = 1, int spriteRowToRead = 0)
        {
            Texture2D texture = textures[textureName];
            int baseWidth = texture.width / spriteCount;
            int baseHeight = texture.height / spriteRow;
            Il2CppSystem.Collections.Generic.List<Sprite> sprites = new Il2CppSystem.Collections.Generic.List<Sprite>();

            for (int i = 0; i < spriteCount; i++)
            {
                string nameAppend = name + "_0" + (i+1);
                Sprite sprite = LoadSprite(texture, new Rect(baseWidth * i, baseHeight * spriteRowToRead, baseWidth, baseHeight), nameAppend);
                sprites.Add(sprite);
                SpriteManager.RegisterSprite(sprite);
            }
            CharacterControllerCommunity.sprites.Add(name, sprites);
        }
        internal static Sprite LoadSprite(Texture2D texture, Rect rect, string name)
        {
            Sprite sprite = Sprite.Create(texture, new Rect((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height), new Vector2(0.5f, 0.5f));
            sprite.name = name;
            return sprite;
        }
    }
}
