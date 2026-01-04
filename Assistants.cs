
using Il2CppNewtonsoft.Json.Linq;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Objects;
using Il2CppVampireSurvivors.Objects.Characters;


namespace CommunityCharacter
{
    internal class Assistants
    {
    }
    internal static class BaseStats
    {
        static float Area = 1f;
        static float Hp = 100f;
        static float Cooldown = 0.9f;
        static float Curse = 1f;
        static float Duration = 1f;
        static float Greed = 1f;
        static float Growth = 1.15f;
        static float Luck = 1.5f;
        static float Movespeed = 1f;
        static double Power = 1d;
        static float Speed = 1f;
        static double Revival = 1d;
        static float Reroll = 20f;
        static float Skip = 20f;
        static float Banish = 20f;

        public static JObject SkinData(string skinType, string name, string texture, string sprite, string suffix = "", int walk = 4, bool unlocked = true)
        {
            JObject skin = new JObject();

            skin["skinType"] = skinType;
            skin["name"] = name;
            skin["textureName"] = texture;
            skin["spriteName"] = sprite;
            skin["suffix"] = suffix;
            skin["walkingFrames"] = walk;
            skin["unlocked"] = unlocked;

            return skin;
        }

        public static JArray Defaults()
        {
            JObject jObject = new JObject();
            JObject level20 = new JObject();
            JObject level21 = new JObject();
            JObject level40 = new JObject();
            JObject level41 = new JObject();
            JArray exitArray = new JArray();
            JArray skins = new JArray();
            JObject skin = SkinData("DEFAULT", "Default", "character_community", "community_01.png");
            skins.Add(skin);
            skins.Add(Zeta.SkinData());
            skins.Add(Hermies.SkinData());
            skins.Add(Opal.SkinData());
            skins.Add(Festa.SkinData());
            skins.Add(Beta.SkinData());
            skins.Add(TempoDiMelma.SkinData());
            skins.Add(VamAndPyre.SkinData());
            skins.Add(DeCapo.SkinData());

            jObject["level"] = 1f;
            jObject["startingWeapon"] = "VOID";
            jObject["charName"] = "Directer’s Assistants";
            jObject["surname"] = "";
            jObject["textureName"] = "character_community";
            jObject["spriteName"] = "community_01.png";
            jObject["currentSkin"] = "50000";
            jObject["walkingFrames"] = 4f;
            jObject["description"] = "The one from many, the many from one. Charge ability: Change form";
            jObject["isBought"] = true;
            jObject["price"] = 0f;
            jObject["maxHp"] = Hp;
            jObject["cooldown"] = Cooldown;
            jObject["armor"] = 0f;
            jObject["regen"] = 0f;
            jObject["moveSpeed"] = Movespeed;
            jObject["power"] = Power;
            jObject["area"] = Area;
            jObject["speed"] = Speed;
            jObject["duration"] = Duration;
            jObject["amount"] = 0f;
            jObject["luck"] = Luck;
            jObject["growth"] = Growth;
            jObject["greed"] = Greed;
            jObject["curse"] = Curse;
            jObject["magnet"] = 0f;
            jObject["revivals"] = Revival;
            jObject["rerolls"] = Reroll;
            jObject["skips"] = Skip;
            jObject["banish"] = Banish;
            jObject["skins"] = skins;
            level20["growth"] = 1;
            level21["growth"] = -1;
            level40["growth"] = 1;
            level41["growth"] = -1;
            exitArray.Add(jObject);
            exitArray.Add(level20);
            exitArray.Add(level21);
            exitArray.Add(level40);
            exitArray.Add(level41);
            return exitArray;
        }
    }
    public static class Zeta
    {
        internal const string SkinType = "50000";
        static string Name = "Zeta";
        static string Texture = "character_community_zeta";
        static string Sprite = "zeta_01.png";
        static string Suffix = "(Zeta Settetails)";
        static string HiddenWeapons = @"[ 'SILF_COUNTER', 'SILF2_COUNTER' ]";

        static float Hp = -50f;
        static float Movespeed = 0.3f;
        static float Area = 0.3f;

        internal static void ZetaStatApply(CharacterController character, bool add = true)
        {
            PlayerModifierStats stats = character.PlayerStats;
            ModifierStats onEveryLevelUp = new();
            WeaponsFacade weapons = character._gameManager.WeaponsFacade;
            if (add)
            {
                stats.MaxHp.Val += Hp;
                stats.MoveSpeed.Val += Movespeed;
                stats.Area.Val += Area;
                onEveryLevelUp.Power = 0.01f;
                onEveryLevelUp.Speed = 0.01f;
                onEveryLevelUp.Duration = 0.01f;
                onEveryLevelUp.Area = 0.01f;
                weapons.AddHiddenWeapon(WeaponType.SILF_COUNTER, character);
                weapons.AddHiddenWeapon(WeaponType.SILF2_COUNTER, character);
            }
            else
            {
                stats.MaxHp.Val -= Hp;
                stats.MoveSpeed.Val -= Movespeed;
                stats.Area.Val -= Area;
                weapons.RemoveHiddenWeapon(WeaponType.SILF_COUNTER, character);
                weapons.RemoveHiddenWeapon(WeaponType.SILF2_COUNTER, character);
            }
            character._playerStats =  stats;
            character._onEveryLevelUp =  onEveryLevelUp;
        }
        static JObject OnEveryLevelUp()
        {
            JObject jObject = new JObject();

            jObject["power"] = 0.01f;
            jObject["speed"] = 0.01f;
            jObject["duration"] = 0.01f;
            jObject["area"] = 0.01f;

            return jObject;
        }
        public static JObject SkinData()
        {
            JObject skin = BaseStats.SkinData(SkinType, Name, Texture, Sprite, Suffix);
            skin["maxHp"] = Hp;
            skin["moveSpeed"] = Movespeed;
            skin["area"] = Area;
            skin["description"] = "Calls upon the Directer at critical moments. Starts with a hidden Cygnus and Zharptysia.Gains + 1 % Might, Area, Speed and Duration every level.";
            skin["hiddenWeapons"] = JArray.Parse(HiddenWeapons);
            skin["onEveryLevelUp"] = OnEveryLevelUp();

            return skin;
        }
    }
    internal static class Hermies
    {
        internal const string SkinType = "50001";
        static string Name = "Hermies";
        static string Texture = "character_community_hermies";
        static string Sprite = "hermies_01.png";
        static string Suffix = "(Hermies)";
        static string HiddenWeapons = @"[ 'LAUREL', 'TRAPANO2' ]";

        static float Movespeed = 2f;
        static float Speed = 0.5f;
        static float Regen = 1f;
        static float Defang = 0.1f;
        
        internal static void HermiesStatApply(CharacterController character, bool add = true)
        {
            PlayerModifierStats stats = character.PlayerStats;
            ModifierStats onEveryLevelUp = new();
            WeaponsFacade weapons = character._gameManager.WeaponsFacade;
            onEveryLevelUp = new();
            if (add)
            {
                stats.Speed.Val += Speed;
                stats.MoveSpeed.Val += Movespeed;
                stats.Regen.Val += Regen;
                stats.Defang += Defang;
                onEveryLevelUp.Regen = 0.05f;
                weapons.AddHiddenWeapon(WeaponType.LAUREL, character);
                weapons.AddHiddenWeapon(WeaponType.TRAPANO2, character);
            }
            else
            {
                stats.Speed.Val -= Speed;
                stats.MoveSpeed.Val -= Movespeed;
                stats.Regen.Val -= Regen;
                stats.Defang -= Defang;
                weapons.RemoveHiddenWeapon(WeaponType.LAUREL, character);
                weapons.RemoveHiddenWeapon(WeaponType.TRAPANO2, character);
            }
            character._playerStats = stats;
            character._onEveryLevelUp = onEveryLevelUp;
        }
        static JObject OnEveryLevelUp()
        {
            JObject jObject = new JObject();

            jObject["regen"] = 0.05f;

            return jObject;
        }
        public static JObject SkinData()
        {
            JObject skin = BaseStats.SkinData(SkinType, Name, Texture, Sprite, Suffix);
            skin["moveSpeed"] = Movespeed;
            skin["speed"] = Speed;
            skin["regen"] = Regen;
            skin["defang"] = Defang;
            skin["description"] = "Move speed increases while continuously moving and deals contact damage based on move speed.Has a Hidden Laurel and Valkyrie Turner.Gains + 0.05 Recovery and +0.05 Armor per level.";
            skin["hiddenWeapons"] = JArray.Parse(HiddenWeapons);
            skin["onEveryLevelUp"] = OnEveryLevelUp();

            return skin;
        }
    }
    internal static class Opal
    {
        internal const string SkinType = "50002";
        static string Name = "Opal";
        static string Texture = "character_community_opal";
        static string Sprite = "opal_01.png";
        static string Suffix = "(Opal)";
        static string HiddenWeapons = @"[ 'EX_GAEA2' ]";

        static float Movespeed = 0.5f;
        static float Regen = 5f;
        static float Cooldown = -0.1f;
        
        internal static void OpalStatApply(CharacterController character, bool add = true)
        {
            PlayerModifierStats stats = character.PlayerStats;
            ModifierStats onEveryLevelUp = new();
            WeaponsFacade weapons = character._gameManager.WeaponsFacade;
            if (add)
            {
                stats.Cooldown.Val += Cooldown;
                stats.MoveSpeed.Val += Movespeed;
                stats.Regen.Val += Regen;
                weapons.AddHiddenWeapon(WeaponType.EX_GAEA2, character);
            }
            else
            {
                stats.Cooldown.Val -= Cooldown;
                stats.MoveSpeed.Val -= Movespeed;
                stats.Regen.Val -= Regen;
                weapons.RemoveHiddenWeapon(WeaponType.EX_GAEA2, character);
            }
            character._playerStats = stats;
            character._onEveryLevelUp = onEveryLevelUp;
        }
        public static JObject SkinData()
        {
            JObject skin = BaseStats.SkinData(SkinType, Name, Texture, Sprite, Suffix);
            skin["moveSpeed"] = Movespeed;
            skin["regen"] = Regen;
            skin["cooldown"] = Cooldown;
            skin["description"] = "Has a hidden Embrace of Gaea. Can Fly. Gains +0.25 Charm and Summons light sources every level.";
            skin["hiddenWeapons"] = JArray.Parse(HiddenWeapons);

            return skin;
        }
    }
    internal static class Festa
    {
        internal const string SkinType = "50003";
        static string Name = "Festa";
        static string Texture = "character_community_festa";
        static string Sprite = "festa_01.png";
        static string Suffix = "(Festa)";
        static string HiddenWeapons = @"[ 'JUBILEE' ]";

        static float Luck = 1.5f;
        static float Greed = 0.5f;
        static float Regen = 1f;
        
        internal static void FestaStatApply(CharacterController character, bool add = true)
        {
            PlayerModifierStats stats = character.PlayerStats;
            ModifierStats onEveryLevelUp = new();
            WeaponsFacade weapons = character._gameManager.WeaponsFacade;
            if (add)
            {
                stats.Luck.Val += Luck;
                stats.Greed.Val += Greed;
                stats.Regen.Val += Regen;
                onEveryLevelUp.Luck = 0.01f;
                weapons.AddHiddenWeapon(WeaponType.JUBILEE, character);
            }
            else
            {
                stats.Luck.Val -= Luck;
                stats.Greed.Val -= Greed;
                stats.Regen.Val -= Regen;
                weapons.RemoveHiddenWeapon(WeaponType.JUBILEE, character);
            }
            character._playerStats = stats;
            character._onEveryLevelUp = onEveryLevelUp;
        }
        static JObject OnEveryLevelUp()
        {
            JObject jObject = new JObject();

            jObject["luck"] = 0.01f;

            return jObject;
        }
        public static JObject SkinData()
        {
            JObject skin = BaseStats.SkinData(SkinType, Name, Texture, Sprite, Suffix);
            skin["luck"] = Luck;
            skin["regen"] = Regen;
            skin["greed"] = Greed;
            skin["description"] = "May find chests in unexpected places. Has a hidden Greatest Jubilee. Gains +1% Luck every level.";
            skin["hiddenWeapons"] = JArray.Parse(HiddenWeapons);
            skin["onEveryLevelUp"] = OnEveryLevelUp();

            return skin;
        }
    }
    internal static class Beta
    {
        internal const string SkinType = "50004";
        static string Name = "Beta";
        static string Texture = "character_community_beta";
        static string Sprite = "beta_01.png";
        static string Suffix = "(Beta)";
        static string HiddenWeapons = @"[ 'VESPERS' ]";

        static float Power = 0.2f;
        static float Speed = 0.2f;
        static float Duration = 0.2f;
        static float Area = 0.2f;
        static float Growth = 0.25f;
        static float Amount = 1f;
        
        internal static void BetaStatApply(CharacterController character, bool add = true)
        {
            PlayerModifierStats stats = character.PlayerStats;
            ModifierStats onEveryLevelUp = new();
            WeaponsFacade weapons = character._gameManager.WeaponsFacade;
            if (add)
            {
                stats.Power.Val += Power;
                stats.Speed.Val += Speed;
                stats.Duration.Val += Duration;
                stats.Area.Val += Area;
                stats.Growth.Val += Growth;
                stats.Amount.Val += Amount;
                onEveryLevelUp.Growth = 0.01f;
                weapons.AddHiddenWeapon(WeaponType.VESPERS, character);
            }
            else
            {
                stats.Power.Val -= Power;
                stats.Speed.Val -= Speed;
                stats.Duration.Val -= Duration;
                stats.Area.Val -= Area;
                stats.Growth.Val -= Growth;
                stats.Amount.Val -= Amount;
                weapons.RemoveHiddenWeapon(WeaponType.VESPERS, character);
            }
            character._playerStats = stats;
            character._onEveryLevelUp = onEveryLevelUp;
        }
        static JObject OnEveryLevelUp()
        {
            JObject jObject = new JObject();

            jObject["growth"] = 0.01f;

            return jObject;
        }
        public static JObject SkinData()
        {
            JObject skin = BaseStats.SkinData(SkinType, Name, Texture, Sprite, Suffix);
            skin["power"] = Power;
            skin["speed"] = Speed;
            skin["duration"] = Duration;
            skin["area"] = Area;
            skin["growth"] = Growth;
            skin["amount"] = Amount;
            skin["description"] = "Likes to keep secrets. Gains +1% Growth per level.";
            skin["hiddenWeapons"] = JArray.Parse(HiddenWeapons);
            skin["onEveryLevelUp"] = OnEveryLevelUp();

            return skin;
        }
    }
    internal static class TempoDiMelma
    {
        internal const string SkinType = "50005";
        static string Name = "Tempo";
        static string Texture = "character_community_tempo";
        static string Sprite = "tempo_01.png";
        static string Suffix = "(Tempo di Melma)";
        static string HiddenWeapons = @"[ 'STIGRANGATTI' ]";

        static float Hp = 100f;
        static float Armor = 3f;
        static float Power = 0.1f;
        static float Speed = 0.1f;
        static float Duration = 0.1f;
        static float Area = 0.1f;
        static float Regen = 1f;
        static float Magnet = 0.25f;
        static float Amount = 2f;
        private static float MagnetChange;
        
        internal static void TempoStatApply(CharacterController character, bool add = true)
        {
            PlayerModifierStats stats = character.PlayerStats;
            ModifierStats onEveryLevelUp = new();
            WeaponsFacade weapons = character._gameManager.WeaponsFacade;
            MagnetZone magnet = character._magnet;
            if (add)
            {
                stats.Power.Val += Power;
                stats.Speed.Val += Speed;
                stats.Duration.Val += Duration;
                stats.Area.Val += Area;
                stats.Armor.Val += Armor;
                stats.Amount.Val += Amount;
                stats.Regen.Val += Regen;
                stats.MaxHp.Val += Hp;
                stats.Magnet.Val += Magnet;
                magnet.Radius += MagnetChange = magnet.Radius * Magnet;
                magnet.RefreshSize();
                onEveryLevelUp.MaxHp = 1f;
                onEveryLevelUp.Magnet = 0.01f;
                weapons.AddHiddenWeapon(WeaponType.STIGRANGATTI, character);
            }
            else
            {
                stats.Power.Val -= Power;
                stats.Speed.Val -= Speed;
                stats.Duration.Val -= Duration;
                stats.Area.Val -= Area;
                stats.Armor.Val -= Armor;
                stats.Amount.Val -= Amount;
                stats.Regen.Val -= Regen;
                stats.MaxHp.Val -= Hp;
                stats.Magnet.Val -= Magnet;
                magnet.Radius -= MagnetChange;
                magnet.RefreshSize();
                weapons.RemoveHiddenWeapon(WeaponType.STIGRANGATTI, character);
            }
            character._playerStats = stats;
            character._onEveryLevelUp = onEveryLevelUp;
            character._magnet = magnet;
        }
        static JObject OnEveryLevelUp()
        {
            JObject jObject = new JObject();

            jObject["maxHp"] = 1f;
            jObject["magnet"] = 0.01f;

            return jObject;
        }
        public static JObject SkinData()
        {
            JObject skin = BaseStats.SkinData(SkinType, Name, Texture, Sprite, Suffix);
            skin["maxHp"] = Hp;
            skin["regen"] = Regen;
            skin["magnet"] = Magnet;
            skin["power"] = Power;
            skin["speed"] = Speed;
            skin["duration"] = Duration;
            skin["area"] = Area;
            skin["armor"] = Armor;
            skin["amount"] = Amount;
            skin["description"] = "Gains stat bonuses upon opening chests and after killing a certain amount of enemies. Has a Hidden Vicious Hunger. Gains +1 Max HP and +1% Magnet every level.";
            skin["hiddenWeapons"] = JArray.Parse(HiddenWeapons);
            skin["onEveryLevelUp"] = OnEveryLevelUp();

            return skin;
        }
    }
    internal static class VamAndPyre
    {
        internal const string SkinType = "50006";
        static string Name = "Vam&Pyre";
        static string Texture = "character_community_vam";
        static string Sprite = "vam_01.png";
        static string Suffix = "(Vam and Pyre)";
        static string HiddenWeapons = @"[ 'MISSPELL2' ]";

        static float Hp = -25f;
        static float Movespeed = -0.1f;
        static float Cooldown = -0.05f;
        static float Power = 0.3f;
        static float Speed = 0.3f;
        static float Duration = 0.3f;
        static float Area = 0.3f;
        static float Luck = 0.2f;
        static float Amount = 1f;
        static float Greed = 0.99f;

        internal static void VamStatApply(CharacterController character, bool add = true)
        {
            PlayerModifierStats stats = character.PlayerStats;
            ModifierStats onEveryLevelUp = new();
            WeaponsFacade weapons = character._gameManager.WeaponsFacade;
            if (add)
            {
                stats.Power.Val += Power;
                stats.Speed.Val += Speed;
                stats.Duration.Val += Duration;
                stats.Area.Val += Area;
                stats.Luck.Val += Luck;
                stats.Amount.Val += Amount;
                stats.Cooldown.Val += Cooldown;
                stats.MaxHp.Val += Hp;
                stats.MoveSpeed.Val += Movespeed;
                stats.Greed.Val += Greed;
                onEveryLevelUp.Power = 0.02f;
                weapons.AddHiddenWeapon(WeaponType.MISSPELL2, character);
            }
            else
            {
                stats.Power.Val -= Power;
                stats.Speed.Val -= Speed;
                stats.Duration.Val -= Duration;
                stats.Area.Val -= Area;
                stats.Luck.Val -= Luck;
                stats.Amount.Val -= Amount;
                stats.Cooldown.Val -= Cooldown;
                stats.MaxHp.Val -= Hp;
                stats.MoveSpeed.Val -= Movespeed;
                stats.Greed.Val -= Greed;
                weapons.RemoveHiddenWeapon(WeaponType.MISSPELL2, character);
            }
            character._playerStats = stats;
            character._onEveryLevelUp = onEveryLevelUp;
        }

        static JObject OnEveryLevelUp()
        {
            JObject jObject = new JObject();

            jObject["power"] = 0.02f;

            return jObject;
        }
        public static JObject SkinData()
        {
            JObject skin = BaseStats.SkinData(SkinType, Name, Texture, Sprite, Suffix);
            skin["maxHp"] = Hp;
            skin["moveSpeed"] = Movespeed;
            skin["cooldown"] = Cooldown;
            skin["power"] = Power;
            skin["speed"] = Speed;
            skin["duration"] = Duration;
            skin["area"] = Area;
            skin["luck"] = Luck;
            skin["amount"] = Amount;
            skin["greed"] = Greed;
            skin["description"] = "May activate weapons twice and turn defeated enemies into followers. Has a hidden Ashes of Muspell. Gains +2% might every level.";
            skin["hiddenWeapons"] = JArray.Parse(HiddenWeapons);
            skin["onEveryLevelUp"] = OnEveryLevelUp();

            return skin;
        }
    }
    internal static class DeCapo
    {
        internal const string SkinType = "50007";
        static string Name = "DeCapo";
        static string Texture = "character_community_decapo";
        static string Sprite = "decapo_01.png";
        static string Suffix = "(De Capo)";
        static string HiddenWeapons = @"[ 'SIRE' ]";

        static float Armor = 5f;
        static float Movespeed = -0.2f;
        static float Growth = 0.25f;
        static float Magnet = 1f;
        static int Charm = 30;
        private static float MagnetChange;
        
        internal static void DeCapoStatApply(CharacterController character, bool add = true)
        {
            PlayerModifierStats stats = character.PlayerStats;
            ModifierStats onEveryLevelUp = new();
            WeaponsFacade weapons = character._gameManager.WeaponsFacade;
            MagnetZone magnet = character._magnet;
            if (add)
            {
                stats.Armor.Val += Armor;
                stats.Growth.Val += Growth;
                stats.Charm += Charm;
                stats.MoveSpeed.Val += Movespeed;
                stats.Magnet.Val += Magnet;
                magnet.Radius += MagnetChange = magnet.Radius * Magnet;
                magnet.RefreshSize();
                onEveryLevelUp.Curse = 0.01f;
                weapons.AddHiddenWeapon(WeaponType.SIRE, character);
            }
            else
            {
                stats.Armor.Val -= Armor;
                stats.Growth.Val -= Growth;
                stats.Charm -= Charm;
                stats.MoveSpeed.Val -= Movespeed;
                stats.Magnet.Val -= Magnet;
                magnet.Radius -= MagnetChange;
                magnet.RefreshSize();
                weapons.RemoveHiddenWeapon(WeaponType.SIRE, character);
            }
            character._playerStats = stats;
            character._onEveryLevelUp = onEveryLevelUp;
            character._magnet = magnet;
        }
        static JObject OnEveryLevelUp()
        {
            JObject jObject = new JObject();

            jObject["curse"] = 0.01f;

            return jObject;
        }
        public static JObject SkinData()
        {
            JObject skin = BaseStats.SkinData(SkinType, Name, Texture, Sprite, Suffix);
            skin["armor"] = Armor;
            skin["moveSpeed"] = Movespeed;
            skin["growth"] = Growth;
            skin["magnet"] = Magnet;
            skin["charm"] = Charm;
            skin["description"] = "Pulls enemies towards itself. Retaliates. Has a hidden Gorgeous moon and +50 Charm. Gains +1% Curse every level.";
            skin["hiddenWeapons"] = JArray.Parse(HiddenWeapons);
            skin["onEveryLevelUp"] = OnEveryLevelUp();

            return skin;
        }
    }
}
