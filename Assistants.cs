using Il2CppNewtonsoft.Json.Linq;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Objects;
using Il2CppVampireSurvivors.Objects.Characters;

namespace CommunityCharacter
{
    internal static class Assistants
    {
        private const float Area = 1f;
        private const float Hp = 100f;
        private const float Cooldown = 0.9f;
        private const float Curse = 1f;
        private const float Duration = 1f;
        private const float Greed = 1f;
        private const float Growth = 1.15f;
        private const float Luck = 1.5f;
        private const float MoveSpeed = 1f;
        private const double Power = 1d;
        private const float Speed = 1f;
        private const double Revival = 1d;
        private const float Reroll = 20f;
        private const float Skip = 20f;
        private const float Banish = 20f;

        public static JObject SkinData(string skinType, string name, string texture, string sprite, string suffix = "", int walk = 4, bool unlocked = true)
        {
            JObject skin = new JObject
            {
                ["skinType"] = skinType,
                ["name"] = name,
                ["textureName"] = texture,
                ["spriteName"] = sprite,
                ["suffix"] = suffix,
                ["walkingFrames"] = walk,
                ["unlocked"] = unlocked
            };

            return skin;
        }

        public static JArray Defaults()
        {
            var jObject = new JObject();
            var level20 = new JObject();
            var level21 = new JObject();
            var level40 = new JObject();
            var level41 = new JObject();
            var exitArray = new JArray();
            var skins = new JArray();
            var skin = SkinData("DEFAULT", "Default", "character_community", "community_01.png");
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
            jObject["walkFrameRate"] = 8;
            jObject["description"] = "The one from many, the many from one. Charge ability: Change form";
            jObject["isBought"] = true;
            jObject["price"] = 0f;
            jObject["maxHp"] = Hp;
            jObject["cooldown"] = Cooldown;
            jObject["armor"] = 0f;
            jObject["regen"] = 0f;
            jObject["moveSpeed"] = MoveSpeed;
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
            level20["level"] = 20;
            level21["level"] = 21;
            level40["level"] = 40;
            level41["level"] = 41;
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
        private const string Name = "Zeta";
        private const string Texture = "character_community_zeta";
        private const string Sprite = "zeta_01.png";
        private const string Suffix = "(Zeta Settetails)";
        private const string HiddenWeapons = @"[ 'SILF_COUNTER', 'SILF2_COUNTER' ]";

        private const float Hp = -50f;
        private const float MoveSpeed = 0.3f;
        private const float Area = 0.3f;

        internal static void ZetaStatApply(CharacterController character, bool add = true)
        {
            var stats = character.PlayerStats;
            ModifierStats onEveryLevelUp = new();
            var weapons = character._gameManager.WeaponsFacade;
            if (add)
            {
                stats.MaxHp.Val += Hp;
                stats.MoveSpeed.Val += MoveSpeed;
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
                stats.MoveSpeed.Val -= MoveSpeed;
                stats.Area.Val -= Area;
                weapons.RemoveHiddenWeapon(WeaponType.SILF_COUNTER, character);
                weapons.RemoveHiddenWeapon(WeaponType.SILF2_COUNTER, character);
            }
            character._playerStats =  stats;
            character._onEveryLevelUp =  onEveryLevelUp;
        }

        private static JObject OnEveryLevelUp()
        {
            var jObject = new JObject
            {
                ["power"] = 0.01f,
                ["speed"] = 0.01f,
                ["duration"] = 0.01f,
                ["area"] = 0.01f
            };

            return jObject;
        }
        public static JObject SkinData()
        {
            var skin = Assistants.SkinData(SkinType, Name, Texture, Sprite, Suffix);
            skin["maxHp"] = Hp;
            skin["moveSpeed"] = MoveSpeed;
            skin["area"] = Area;
            skin["walkFrameRate"] = 8;
            skin["description"] = "Calls upon the Directer at critical moments. Starts with a hidden Cygnus and Zharptysia.Gains + 1 % Might, Area, Speed and Duration every level.";
            skin["hiddenWeapons"] = JArray.Parse(HiddenWeapons);
            skin["onEveryLevelUp"] = OnEveryLevelUp();

            return skin;
        }
    }
    internal static class Hermies
    {
        internal const string SkinType = "50001";
        private const string Name = "Hermies";
        private const string Texture = "character_community_hermies";
        private const string Sprite = "hermies_01.png";
        private const string Suffix = "(Hermies)";
        private const string HiddenWeapons = @"[ 'LAUREL', 'TRAPANO2' ]";

        private const float MoveSpeed = 2f;
        private const float Speed = 0.5f;
        private const float Regen = 0.5f;
        private const float Defang = 0.1f;

        internal static void HermiesStatApply(CharacterController character, bool add = true)
        {
            var stats = character.PlayerStats;
            var weapons = character._gameManager.WeaponsFacade;
            ModifierStats onEveryLevelUp = new();
            if (add)
            {
                stats.Speed.Val += Speed;
                stats.MoveSpeed.Val += MoveSpeed;
                stats.Regen.Val += Regen;
                stats.Defang += Defang;
                onEveryLevelUp.Regen = 0.05f;
                weapons.AddHiddenWeapon(WeaponType.LAUREL, character);
                weapons.AddHiddenWeapon(WeaponType.TRAPANO2, character);
            }
            else
            {
                stats.Speed.Val -= Speed;
                stats.MoveSpeed.Val -= MoveSpeed;
                stats.Regen.Val -= Regen;
                stats.Defang -= Defang;
                weapons.RemoveHiddenWeapon(WeaponType.LAUREL, character);
                weapons.RemoveHiddenWeapon(WeaponType.TRAPANO2, character);
            }
            character._playerStats = stats;
            character._onEveryLevelUp = onEveryLevelUp;
        }

        private static JObject OnEveryLevelUp()
        {
            var jObject = new JObject
            {
                ["regen"] = 0.025f
            };

            return jObject;
        }
        public static JObject SkinData()
        {
            var skin = Assistants.SkinData(SkinType, Name, Texture, Sprite, Suffix);
            skin["moveSpeed"] = MoveSpeed;
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
        private const string Name = "Opal";
        private const string Texture = "character_community_opal";
        private const string Sprite = "opal_01.png";
        private const string Suffix = "(Opal)";
        private const string HiddenWeapons = @"[ 'TRIASSO3' ]";

        private const float MoveSpeed = 0.5f;
        private const float Regen = 5f;

        internal static void OpalStatApply(CharacterController character, bool add = true)
        {
            var stats = character.PlayerStats;
            ModifierStats onEveryLevelUp = new();
            var weapons = character._gameManager.WeaponsFacade;
            if (add)
            {
                stats.MoveSpeed.Val += MoveSpeed;
                stats.Regen.Val += Regen;
                weapons.AddHiddenWeapon(WeaponType.TRIASSO3, character);
            }
            else
            {
                stats.MoveSpeed.Val -= MoveSpeed;
                stats.Regen.Val -= Regen;
                weapons.RemoveHiddenWeapon(WeaponType.TRIASSO3, character);
            }
            character._playerStats = stats;
            character._onEveryLevelUp = onEveryLevelUp;
        }
        public static JObject SkinData()
        {
            var skin = Assistants.SkinData(SkinType, Name, Texture, Sprite, Suffix);
            skin["moveSpeed"] = MoveSpeed;
            skin["regen"] = Regen;
            skin["description"] = "Has a hidden Embrace of Gaea. Can Fly. Gains +0.25 Charm and Summons light sources every level.";
            skin["hiddenWeapons"] = JArray.Parse(HiddenWeapons);

            return skin;
        }
    }
    internal static class Festa
    {
        internal const string SkinType = "50003";
        private const string Name = "Festa";
        private const string Texture = "character_community_festa";
        private const string Sprite = "festa_01.png";
        private const string Suffix = "(Festa)";
        private const string HiddenWeapons = @"[ 'JUBILEE' ]";

        private const float Luck = 0.5f;
        private const float Greed = 0.5f;
        private const float Regen = 1f;

        internal static void FestaStatApply(CharacterController character, bool add = true)
        {
            var stats = character.PlayerStats;
            ModifierStats onEveryLevelUp = new();
            var weapons = character._gameManager.WeaponsFacade;
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

        private static JObject OnEveryLevelUp()
        {
            var jObject = new JObject
            {
                ["luck"] = 0.01f
            };

            return jObject;
        }
        public static JObject SkinData()
        {
            var skin = Assistants.SkinData(SkinType, Name, Texture, Sprite, Suffix);
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
        private const string Name = "Beta";
        private const string Texture = "character_community_beta";
        private const string Sprite = "beta_01.png";
        private const string Suffix = "(Beta)";
        private const string HiddenWeapons = @"[ 'FOLLOWER_KNIFE1' ]";

        private const float Power = 0.2f;
        private const float Speed = 0.2f;
        private const float Duration = 0.2f;
        private const float Area = 0.2f;
        private const float Growth = 0.25f;
        private const float Amount = 1f;

        internal static void BetaStatApply(CharacterController character, bool add = true)
        {
            var stats = character.PlayerStats;
            ModifierStats onEveryLevelUp = new();
            var weapons = character._gameManager.WeaponsFacade;
            if (add)
            {
                stats.Power.Val += Power;
                stats.Speed.Val += Speed;
                stats.Duration.Val += Duration;
                stats.Area.Val += Area;
                stats.Growth.Val += Growth;
                stats.Amount.Val += Amount;
                onEveryLevelUp.Growth = 0.01f;
                weapons.AddHiddenWeapon(WeaponType.FOLLOWER_KNIFE1, character);
            }
            else
            {
                stats.Power.Val -= Power;
                stats.Speed.Val -= Speed;
                stats.Duration.Val -= Duration;
                stats.Area.Val -= Area;
                stats.Growth.Val -= Growth;
                stats.Amount.Val -= Amount;
                weapons.RemoveHiddenWeapon(WeaponType.FOLLOWER_KNIFE1, character);
            }
            character._playerStats = stats;
            character._onEveryLevelUp = onEveryLevelUp;
        }

        private static JObject OnEveryLevelUp()
        {
            var jObject = new JObject
            {
                ["growth"] = 0.01f
            };

            return jObject;
        }
        public static JObject SkinData()
        {
            var skin = Assistants.SkinData(SkinType, Name, Texture, Sprite, Suffix);
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
        private const string Name = "Tempo";
        private const string Texture = "character_community_tempo";
        private const string Sprite = "tempo_01.png";
        private const string Suffix = "(Tempo di Melma)";
        private const string HiddenWeapons = @"[ 'STIGRANGATTI' ]";

        private const float Hp = 100f;
        private const float Power = 0.1f;
        private const float Speed = 0.1f;
        private const float Duration = 0.1f;
        private const float Area = 0.1f;
        private const float Regen = 1f;
        private const float Magnet = 0.25f;
        private static float _magnetChange;
        
        internal static void TempoStatApply(CharacterController character, bool add = true)
        {
            var stats = character.PlayerStats;
            ModifierStats onEveryLevelUp = new();
            var weapons = character._gameManager.WeaponsFacade;
            var magnet = character._magnet;
            if (add)
            {
                stats.Power.Val += Power;
                stats.Speed.Val += Speed;
                stats.Duration.Val += Duration;
                stats.Area.Val += Area;
                stats.Regen.Val += Regen;
                stats.MaxHp.Val += Hp;
                stats.Magnet.Val += Magnet;
                magnet.Radius += _magnetChange = magnet.Radius * Magnet;
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
                stats.Regen.Val -= Regen;
                stats.MaxHp.Val -= Hp;
                stats.Magnet.Val -= Magnet;
                magnet.Radius -= _magnetChange;
                magnet.RefreshSize();
                weapons.RemoveHiddenWeapon(WeaponType.STIGRANGATTI, character);
            }
            character._playerStats = stats;
            character._onEveryLevelUp = onEveryLevelUp;
            character._magnet = magnet;
        }

        private static JObject OnEveryLevelUp()
        {
            var jObject = new JObject
            {
                ["maxHp"] = 1f,
                ["magnet"] = 0.01f
            };

            return jObject;
        }
        public static JObject SkinData()
        {
            var skin = Assistants.SkinData(SkinType, Name, Texture, Sprite, Suffix);
            skin["maxHp"] = Hp;
            skin["regen"] = Regen;
            skin["magnet"] = Magnet;
            skin["power"] = Power;
            skin["speed"] = Speed;
            skin["duration"] = Duration;
            skin["area"] = Area;
            skin["description"] = "Gains stat bonuses upon opening chests and after killing a certain amount of enemies. Has a Hidden Vicious Hunger. Gains +1 Max HP and +1% Magnet every level.";
            skin["hiddenWeapons"] = JArray.Parse(HiddenWeapons);
            skin["onEveryLevelUp"] = OnEveryLevelUp();

            return skin;
        }
    }
    internal static class VamAndPyre
    {
        internal const string SkinType = "50006";
        private const string Name = "Vam&Pyre";
        private const string Texture = "character_community_vam";
        private const string Sprite = "vam_01.png";
        private const string Suffix = "(Vam and Pyre)";
        private const string HiddenWeapons = @"[ 'MISSPELL2' ]";

        private const float Hp = -25f;
        private const float MoveSpeed = -0.1f;
        private const float Power = 0.3f;
        private const float Speed = 0.3f;
        private const float Duration = 0.3f;
        private const float Area = 0.3f;
        private const float Luck = 0.2f;
        private const float Amount = 1f;
        private const float Greed = 0.99f;

        internal static void VamStatApply(CharacterController character, bool add = true)
        {
            var stats = character.PlayerStats;
            ModifierStats onEveryLevelUp = new();
            var weapons = character._gameManager.WeaponsFacade;
            if (add)
            {
                stats.Power.Val += Power;
                stats.Speed.Val += Speed;
                stats.Duration.Val += Duration;
                stats.Area.Val += Area;
                stats.Luck.Val += Luck;
                stats.Amount.Val += Amount;
                stats.MaxHp.Val += Hp;
                stats.MoveSpeed.Val += MoveSpeed;
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
                stats.MaxHp.Val -= Hp;
                stats.MoveSpeed.Val -= MoveSpeed;
                stats.Greed.Val -= Greed;
                weapons.RemoveHiddenWeapon(WeaponType.MISSPELL2, character);
            }
            character._playerStats = stats;
            character._onEveryLevelUp = onEveryLevelUp;
        }

        private static JObject OnEveryLevelUp()
        {
            var jObject = new JObject
            {
                ["power"] = 0.02f
            };

            return jObject;
        }
        public static JObject SkinData()
        {
            var skin = Assistants.SkinData(SkinType, Name, Texture, Sprite, Suffix);
            skin["maxHp"] = Hp;
            skin["moveSpeed"] = MoveSpeed;
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
        private const string Name = "DeCapo";
        private const string Texture = "character_community_decapo";
        private const string Sprite = "decapo_01.png";
        private const string Suffix = "(De Capo)";
        private const string HiddenWeapons = @"[ 'SIRE' ]";

        private const float Hp = 100;
        private const float Armor = 5f;
        private const float MoveSpeed = -0.2f;
        private const float Growth = 0.25f;
        private const float Magnet = 1f;
        private const int Charm = 30;
        private static float _magnetChange;
        
        internal static void DeCapoStatApply(CharacterController character, bool add = true)
        {
            var stats = character.PlayerStats;
            ModifierStats onEveryLevelUp = new();
            var weapons = character._gameManager.WeaponsFacade;
            var magnet = character._magnet;
            if (add)
            {
                stats.MaxHp.Val += Hp;
                stats.Armor.Val += Armor;
                stats.Growth.Val += Growth;
                stats.Charm += Charm;
                stats.MoveSpeed.Val += MoveSpeed;
                stats.Magnet.Val += Magnet;
                magnet.Radius += _magnetChange = magnet.Radius * Magnet;
                magnet.RefreshSize();
                onEveryLevelUp.Curse = 0.01f;
                weapons.AddHiddenWeapon(WeaponType.SIRE, character);
            }
            else
            {
                stats.MaxHp.Val -= Hp;
                stats.Armor.Val -= Armor;
                stats.Growth.Val -= Growth;
                stats.Charm -= Charm;
                stats.MoveSpeed.Val -= MoveSpeed;
                stats.Magnet.Val -= Magnet;
                magnet.Radius -= _magnetChange;
                magnet.RefreshSize();
                weapons.RemoveHiddenWeapon(WeaponType.SIRE, character);
            }
            character._playerStats = stats;
            character._onEveryLevelUp = onEveryLevelUp;
            character._magnet = magnet;
        }

        private static JObject OnEveryLevelUp()
        {
            var jObject = new JObject
            {
                ["curse"] = 0.01f
            };

            return jObject;
        }
        public static JObject SkinData()
        {
            var skin = Assistants.SkinData(SkinType, Name, Texture, Sprite, Suffix);
            skin["maxHp"] = Hp;
            skin["armor"] = Armor;
            skin["moveSpeed"] = MoveSpeed;
            skin["growth"] = Growth;
            skin["magnet"] = Magnet;
            skin["charm"] = Charm;
            skin["description"] = "Pulls enemies towards itself. Retaliates. Has a hidden Gorgeous moon and +30 Charm. Gains +1% Curse every level.";
            skin["hiddenWeapons"] = JArray.Parse(HiddenWeapons);
            skin["onEveryLevelUp"] = OnEveryLevelUp();

            return skin;
        }
    }
}
