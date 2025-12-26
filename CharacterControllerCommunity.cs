using HarmonyLib;
using Il2Cpp;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Characters;
using Il2CppVampireSurvivors.Data.Stage;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Framework.DLC;
using Il2CppVampireSurvivors.Framework.Loading;
using Il2CppVampireSurvivors.Framework.NumberTypes;
using Il2CppVampireSurvivors.Framework.TimerSystem;
using Il2CppVampireSurvivors.Objects;
using Il2CppVampireSurvivors.Objects.Characters;
using Il2CppVampireSurvivors.Objects.Items;
using Il2CppVampireSurvivors.Objects.Pickups;
using Il2CppVampireSurvivors.Signals;
using Il2CppVampireSurvivors.Tools;
using Il2CppVampireSurvivors.Graphics;
using Unity.Mathematics;
using UnityEngine;
using Timer = Il2CppVampireSurvivors.Framework.TimerSystem.Timer;


namespace CommunityCharacter
{

    [HarmonyPatch(typeof(LoadingManager))]
    static class LoadingManagerPatch
    {
        [HarmonyPatch(nameof(LoadingManager.MountDlc))]
        [HarmonyPrefix]
        public static void MountDlc_Prefix(LoadingManager __instance, DlcType dlcType, Action callback)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), DlcSystem.DlcCatalog._DlcData[dlcType]._Steam._AppID);
            AddressableLoader.SetInternalIdTransform();
            AddressableLoader.SetPath(path);
            if (!string.IsNullOrEmpty(path) && path != Directory.GetCurrentDirectory())
                __instance.MountedPaths.TryAdd(dlcType, path);
        }
    }

    [HarmonyPatch(typeof(EnemyController))]
    static class EnemyControllerPatch
    {
        static bool canBeRetal = true;
        static Timer retalTimer;

        [HarmonyPatch(nameof(EnemyController.OnPlayerOverlap))]
        [HarmonyPostfix]
        public static void OnPlayerOverlap_Postfix(EnemyController __instance, CharacterController player)
        {
            if (player.CurrentCharacterData.currentSkin.ToString() != DeCapo.SkinType || player.CurrentCharacterData.currentSkin.ToString() != Hermies.SkinType) return;
            if (!canBeRetal) return;
            float num = 0;
            if (player.CurrentCharacterData.currentSkin.ToString() == DeCapo.SkinType) num = player.PlayerStats.Magnet * 5;
            else num = player.PMoveSpeed();
            __instance.GetDamaged(num, HitVfxType.Default, 20, WeaponType.VOID, true);
            canBeRetal = false;
            Timer retalTimerLocal = retalTimer;
            if (retalTimerLocal != null) retalTimerLocal.Cancel();
            Action action = () =>
            {
                canBeRetal = true;
            };
            retalTimer = TimerHelper.RegisterMillis(250f, action);
        }
    }
    
    [HarmonyPatch(typeof(Stage))]
    static class StagePatch
    {
        [HarmonyPatch(nameof(Stage.DebugSpawnDestructibles))]
        [HarmonyPrefix]
        public static bool DebugSpawnDestructibles_Prefix()
        {
            if(CharacterControllerCommunity.characterControllerCommunity._characterType == (CharacterType)20000)
                if(CharacterControllerCommunity.currentSkinType == Festa.SkinType) return false;

            return true;
        }
    }

    [HarmonyPatch(typeof(TreasureChest))]
    static class TreasureChestPatch
    {
        [HarmonyPatch(nameof(TreasureChest.GetTaken))]
        [HarmonyPostfix]
        public static void GetTaken_Postfix(TreasureChest __instance)
        {
            if (__instance._targetPlayer != CharacterControllerCommunity.characterControllerCommunity) return;
            if (__instance.TargetPlayer.CurrentCharacterData.currentSkin.ToString() != TempoDiMelma.SkinType) return;
            CharacterController character = CharacterControllerCommunity.characterControllerCommunity;
            character.PlayerStats.Power += 0.01f;
            character.PlayerStats.Speed += 0.01f;
            character.PlayerStats.Duration += 0.01f;
            character.PlayerStats.Area += 0.01f;
        }
    }

    [HarmonyPatch(typeof(Destructible))]
    static class DestructiblePatch
    {

        [HarmonyPatch(nameof(Destructible.OnDestroyed))]
        [HarmonyPrefix]
        public static bool OnDestroy_Prefix(Destructible __instance)
        {
            if (CharacterControllerCommunity.characterControllerCommunity == null) return true;
            if (CharacterControllerCommunity.currentSkinType != Festa.SkinType) return true;
            if (!__instance.CanEmitLight()) return true;
            if (UnityEngine.Random.Range(0, 31) < 29f) return true;

            Il2CppSystem.Collections.Generic.List<float> chances = new Il2CppSystem.Collections.Generic.List<float>();
            chances.Add(3f);
            chances.Add(33f);
            chances.Add(100f);
            Il2CppSystem.Collections.Generic.List<Il2CppSystem.Nullable<PrizeType>> prizeTypes = new Il2CppSystem.Collections.Generic.List<Il2CppSystem.Nullable<PrizeType>>();
            prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EXISTING_ANY));
            prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EXISTING_ANY));
            prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EXISTING_ANY));
            prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EXISTING_ANY));
            prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EXISTING_ANY));

            Treasure treasure = new Treasure
            {
                chances = chances,
                prizeTypes = prizeTypes
            };
            __instance._gameManager.Stage.SetTreasureLevelFromChance(treasure);
            __instance._gameManager.MakeTreasure(__instance.transform.position, treasure);

            __instance._playerOptions.IncreaseDestroyedPropCount(__instance._destructibleType);
            __instance.HandleArcanas();
            return false;
        }
    }

    [HarmonyPatch(typeof(GameManager))]
    static class GameManagerPatch
    {
        [HarmonyPatch(nameof(GameManager.MakeTreasure))]
        [HarmonyPostfix]
        public static void MakeTreasure_Postfix(GameManager __instance, Vector2 pos, Treasure treasure, ref TreasureChest __result)
        {
            if (__instance.PlayerOne != CharacterControllerCommunity.characterControllerCommunity) return;
            if (__instance.PlayerOne.CurrentCharacterData.currentSkin.ToString() != Beta.SkinType) return;
            if (!treasure.prizeTypes.Contains(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVOLUTION)))
            {
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVOLUTION));
            }
            else if (!treasure.hasArcana)
            {
                treasure.hasArcana = true;
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVO_ARCANA));
                __result.SetArcana(treasure.hasArcana);
            }
            __result.SetData(ItemType.TREASURE, treasure);
        }
    }

    [HarmonyPatch(typeof(CharacterController))]
    static class CharacterControllerCommunity
    {
        internal static CharacterController characterControllerCommunity;
        internal static string currentSkinType;
        static int killsToTrigger;
        static int zetaTriggerEffect;
        static float hermiesTrigger;
        static Timer hermiesMove;
        static EggFloat moveSpeed;
        static int hermiesLevel;
        static bool opalFly;
        static int followerNum;
        static bool betaVamSub;

        [HarmonyPatch(nameof(CharacterController.AfterFullInitialization))]
        [HarmonyPostfix]
        public static void AfterFullInitialization_Postfix(CharacterController __instance)
        {
            if (__instance._characterType != (CharacterType)20000) return;

            characterControllerCommunity = __instance;
            currentSkinType = __instance.CurrentCharacterData.currentSkin.ToString();
            killsToTrigger = 100;
            zetaTriggerEffect = 1;
            characterControllerCommunity.IsCriticalHPEnabled = true;
            characterControllerCommunity.HasAnyCriticalHPSkill = true;
            characterControllerCommunity.OnCriticalHP = new Action(CriticalHp);
            hermiesTrigger = 0.33f;
            hermiesLevel = 1;
            opalFly = false;
            followerNum = 1;
            betaVamSub = false;
            if (currentSkinType == Zeta.SkinType)
            {
                __instance._spriteAnimation.AddAnimation("black", SpriteManager.GetAnimationFrames("zeta_black_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_black", 2, characterControllerCommunity.RespectAnimationXPivots), 8, true);
                __instance._spriteAnimation.AddAnimation("moon", SpriteManager.GetAnimationFrames("zeta_moon_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_moon", 2, characterControllerCommunity.RespectAnimationXPivots), 8, true);
                __instance._spriteAnimation.AddAnimation("city", SpriteManager.GetAnimationFrames("zeta_city_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_city", 2, characterControllerCommunity.RespectAnimationXPivots), 8, true);
                __instance._spriteAnimation.AddAnimation("stone", SpriteManager.GetAnimationFrames("zeta_stone_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_stone", 2, characterControllerCommunity.RespectAnimationXPivots), 8, true);
                __instance._spriteAnimation.AddAnimation("sun", SpriteManager.GetAnimationFrames("zeta_sun_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_sun", 2, characterControllerCommunity.RespectAnimationXPivots), 8, true);
                __instance._spriteAnimation.AddAnimation("volcano", SpriteManager.GetAnimationFrames("zeta_volcano_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_volcano", 2, characterControllerCommunity.RespectAnimationXPivots), 8, true);
                __instance._spriteAnimation.AddAnimation("seawinds", SpriteManager.GetAnimationFrames("zeta_seawinds_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_seawinds", 2, characterControllerCommunity.RespectAnimationXPivots), 8, true);
                __instance._spriteAnimation.AddAnimation("directer", SpriteManager.GetAnimationFrames("zeta_directer_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_directer", 2, characterControllerCommunity.RespectAnimationXPivots), 8, true);
                __instance._spriteAnimation.SetAnimation("black");
            }
        }
        [HarmonyPatch(nameof(CharacterController.OnUpdate))]
        [HarmonyPostfix]
        public static void OnUpdate_Postfix(CharacterController __instance)
        {
            if (__instance._characterType != (CharacterType)20000) return;
            // Zeta
            if (currentSkinType == Zeta.SkinType) ZetaAspectEffects();
            // Hermies
            if (currentSkinType == Hermies.SkinType && hermiesMove == null && !__instance._spriteAnimation.IsPaused)
            {
                moveSpeed = __instance._playerStats.MoveSpeed;
                hermiesMove = Timers.Register(20000, HermiesMovespeedTake, HermiesMovespeedGive);
            }
            // Opal
            if (currentSkinType == Opal.SkinType && !opalFly)
            {
                opalFly = true;
                characterControllerCommunity._gameManager._physicsManager._playersWithWallCollisionGroup.remove(characterControllerCommunity.GetComponent<ArcadeSprite>());
            }
            // Beta and Vam
            if (currentSkinType == Beta.SkinType || currentSkinType == VamAndPyre.SkinType)
            {
                if (betaVamSub) return;
                betaVamSub = true;
                characterControllerCommunity._signalBus.Subscribe<GameplaySignals.EnemyKilledImmediateSignal>(new Action(MakeEnemyFollower));
            }
            else if (betaVamSub)
                characterControllerCommunity._signalBus.Unsubscribe<GameplaySignals.EnemyKilledImmediateSignal>(new Action(MakeEnemyFollower));
        }

        [HarmonyPatch(nameof(CharacterController.HandleLateUpdate))]
        [HarmonyPostfix]
        public static void HandleLateUpdate_Postfix(CharacterController __instance)
        {
            if (PauseSystem.Paused) return;
            if (currentSkinType == DeCapo.SkinType)
            {
                Il2CppSystem.Collections.Generic.HashSet<PhaserGameObject> children = __instance._gameManager.Enemies.getChildren();
                float2 lhs = __instance.transform.position.ToFloat2();
                lhs.x += 0.25f * (float)(__instance.flipX ? -1 : 1);
                foreach (PhaserGameObject phaserGameObject in children)
                {
                    if (phaserGameObject.body != null && phaserGameObject.body._enable)
                    {
                        EnemyType enemyType = phaserGameObject.Cast<EnemyController>().EnemyType;
                        if (enemyType != EnemyType.EX_CART_DROWNER && enemyType != EnemyType.BOSS_DROWNER_RASH && enemyType != EnemyType.BOSS_DROWNER_NORMAL && enemyType != EnemyType.BOSS_XLDROWNER && enemyType != EnemyType.TP_BOSS_DEATH && enemyType != EnemyType.TP_BOSS_DEATH_SCYTHEBIG && enemyType != EnemyType.TP_BOSS_DEATHARM)
                        {
                            float2 @float = lhs - phaserGameObject.transform.position.ToFloat2();
                            float num = math.dot(@float, @float);
                            if (num > 0.1f)
                            {
                                float2 rhs = @float / num * PauseSystem.DeltaTime * 4f * 1 * __instance.PArea();
                                phaserGameObject.Cast<EnemyController>().position += rhs;
                            }
                        }
                    }
                }
            }
        }

        /*[HarmonyPatch(nameof(CharacterController.Despawn))]
        [HarmonyPostfix]
        public static void Despawn_Postfix()
        {
            if (characterControllerCommunity == null) return;
            if (characterControllerCommunity.CharacterType != (CharacterType)20000) return;
            characterControllerCommunity._signalBus.TryUnsubscribe<GameplaySignals.EnemyKilledImmediateSignal>(new Action(MakeEnemyFollower));
        }*/

        [HarmonyPatch(nameof(CharacterController.OnStop))]
        [HarmonyPostfix]
        public static void OnStop_Postfix(CharacterController __instance)
        {
            if (hermiesMove != null)
                hermiesMove.Complete();
        }

        [HarmonyPatch(nameof(CharacterController.LevelUp))]
        [HarmonyPostfix]
        public static void LevelUp_Postfix(CharacterController __instance)
        {
            // Hermies Armor gain

            if (currentSkinType == Hermies.SkinType)
            {
                hermiesLevel++;
                if (hermiesLevel % 20 == 0)
                    __instance.PlayerStats.Armor += 1f;
            }

            if (currentSkinType == Opal.SkinType) OpalEffect();
        }

        /// <summary>
        /// Regular Methods to call
        /// </summary>
        internal static void CriticalHp()
        {
            if (currentSkinType == Zeta.SkinType)
                characterControllerCommunity.AddTemporaryBonus(new Action(ZetaCritGive), new Action(ZetaCritRemove), 5000f);
        }


        /// <summary>
        /// Zeta Methods
        /// </summary>
        internal static void ZetaCritGive()
        {
            characterControllerCommunity._playerStats.Power += 0.5f;
            characterControllerCommunity._playerStats.Speed += 0.5f;
            characterControllerCommunity._playerStats.Duration += 0.5f;
            characterControllerCommunity._playerStats.Area += 0.5f;
            characterControllerCommunity._playerStats.Regen += 7f;
        }
        internal static void ZetaCritRemove()
        {
            characterControllerCommunity._playerStats.Power -= 0.5f;
            characterControllerCommunity._playerStats.Speed -= 0.5f;
            characterControllerCommunity._playerStats.Duration -= 0.5f;
            characterControllerCommunity._playerStats.Area -= 0.5f;
            characterControllerCommunity._playerStats.Regen -= 7f;
        }
        internal static void ZetaGemEffect()
        {

            Il2CppSystem.Collections.Generic.HashSet<Pickup> pickups = new Il2CppSystem.Collections.Generic.HashSet<Pickup>();
            int i = 0;
            foreach (Pickup a in characterControllerCommunity._gameManager._gems)
            {
                Gem gem = a.Cast<Gem>();
                float bless = characterControllerCommunity.PAmount() + Mathf.Log(characterControllerCommunity._gameManager._gems.Count)/UnityEngine.Random.Range(1,7);
                gem.BlessColor(bless, i);
                pickups.Add(gem);
                i++;
            }
            characterControllerCommunity._gameManager._gems = pickups;
        }
        internal static void ZetaHeartEffect()
        {
            Vector3 position = characterControllerCommunity.transform.position;
            float num = 9f;
            float num2 = 3.1415927f / num;
            float num3 = UnityEngine.Random.value * 1.5707963f;
            for (int i = 0; i <= 17; i++)
            {
                Vector2 pos = new Vector2
                {
                    x = position.x + Mathf.Sin(num3 + num2 * (float)i) * 1.25f,
                    y = position.y + Mathf.Cos(num3 + num2 * (float)i) * 1.25f
                };
                PickupManager.PickupItems.Add(PickupManager.CreatePickup(pos, ItemType.LITTLEHEART));
            }
        }
        internal static void ZetaChestEffect()
        {
            //TODO implement
        }
        internal static void ZetaAspectEffects()
        {
            int kills = characterControllerCommunity._playerOptions.Config.RunEnemies;

            if (kills < killsToTrigger) return;

            killsToTrigger = (int)(killsToTrigger + (killsToTrigger * 0.2f));

            //Chest to Gold chest
            //Gems - shine or color change
            switch (zetaTriggerEffect)
            {
                case 1:
                    characterControllerCommunity._gameManager.MakeAndActivatePickup(ItemType.CLOVER);
                    characterControllerCommunity.Anims.SetAnimation("moon");
                    break;
                case 2:
                    characterControllerCommunity._gameManager.MakeAndActivatePickup(ItemType.ROSARY);
                    characterControllerCommunity.Anims.SetAnimation("city");
                    break;
                case 3:
                    ZetaGemEffect();
                    characterControllerCommunity.Anims.SetAnimation("stone");
                    break;
                case 4:
                    characterControllerCommunity._gameManager.TriggerGoldFever(20000);
                    characterControllerCommunity.Anims.SetAnimation("sun");
                    break;
                case 5:
                    ZetaHeartEffect();
                    characterControllerCommunity.Anims.SetAnimation("volcano");
                    break;
                case 6:
                    ZetaChestEffect();
                    characterControllerCommunity.Anims.SetAnimation("seawinds");
                    break;
                case 7:
                    characterControllerCommunity._gameManager.MakeAndActivatePickup(ItemType.VACUUM);
                    characterControllerCommunity.Anims.SetAnimation("directer");
                    break;
                default:
                    characterControllerCommunity._gameManager.MakeAndActivatePickup(ItemType.CLOVER);
                    characterControllerCommunity._gameManager.MakeAndActivatePickup(ItemType.ROSARY);
                    ZetaGemEffect();
                    characterControllerCommunity._gameManager.TriggerGoldFever(20000);
                    ZetaHeartEffect();
                    ZetaChestEffect();
                    characterControllerCommunity._gameManager.MakeAndActivatePickup(ItemType.VACUUM);
                    zetaTriggerEffect = 0;
                    characterControllerCommunity.Anims.SetAnimation("black");
                    break;
            }

            zetaTriggerEffect++;
        }

        /// <summary>
        /// Hermies Methods
        /// </summary>

        static Action<float> HermiesMovespeedGive = (float time) =>
        {
            if (time < hermiesTrigger) return;
            hermiesTrigger+= 0.33f;
            characterControllerCommunity._playerStats.MoveSpeed += 0.01f;
        };
        static Action HermiesMovespeedTake = () =>
        {
            hermiesMove.Cancel();
            characterControllerCommunity._playerStats.MoveSpeed = moveSpeed;
            hermiesMove = null;
        };

        internal static void OpalEffect()
        {
            Vector3 position = characterControllerCommunity.transform.position;
            int num = (int)characterControllerCommunity.PAmount();
            if (num == 0) return;
            float num2 = 3.1415927f / num;
            float num3 = UnityEngine.Random.value * 1.5707963f;
            for (int i = 0; i <= num; i++)
            {
                Vector2 pos = new Vector2
                {
                    x = position.x + Mathf.Sin(num3 + num2 * (float)i) * 1.25f,
                    y = position.y + Mathf.Cos(num3 + num2 * (float)i) * 1.25f
                };
                characterControllerCommunity._gameManager.Stage.MakeDestructible(characterControllerCommunity._gameManager.Stage.DestructibleType, pos);
            }
        }

        internal static void TempoEffects()
        {
            int kills = characterControllerCommunity._playerOptions.Config.RunEnemies;

            if (kills < killsToTrigger) return;

            killsToTrigger = (int)(killsToTrigger + (killsToTrigger * 0.2f));

            int random = UnityEngine.Random.Range(1, 16);

            switch (random)
            {
                case 1:
                    characterControllerCommunity.PlayerStats.Amount += 0.5f;
                    break;
                case 2:
                    characterControllerCommunity.PlayerStats.Area += 0.5f;
                    break;
                case 3:
                    characterControllerCommunity.PlayerStats.Armor += 0.5f;
                    break;
                case 4:
                    characterControllerCommunity.PlayerStats.Cooldown -= 0.5f;
                    break;
                case 5:
                    characterControllerCommunity.PlayerStats.Duration += 0.5f;
                    break;
                case 6:
                    characterControllerCommunity.PlayerStats.Greed += 0.5f;
                    break;
                case 7:
                    characterControllerCommunity.PlayerStats.Growth += 0.5f;
                    break;
                case 8:
                    characterControllerCommunity.PlayerStats.Luck += 0.5f;
                    break;
                case 9:
                    characterControllerCommunity.PlayerStats.Magnet += 0.5f;
                    break;
                case 10:
                    characterControllerCommunity.PlayerStats.MaxHp += 5f;
                    break;
                case 11:
                    characterControllerCommunity.PlayerStats.MoveSpeed += 0.5f;
                    break;
                case 12:
                    characterControllerCommunity.PlayerStats.Power += 0.5f;
                    break;
                case 13:
                    characterControllerCommunity.PlayerStats.Regen += 0.5f;
                    break;
                case 14:
                    characterControllerCommunity.PlayerStats.Revivals += 0.5f;
                    break;
                case 15:
                    characterControllerCommunity.PlayerStats.Speed += 0.5f;
                    break;
            }

        }

        internal static void MakeEnemyFollower()
        {
            if (characterControllerCommunity._gameManager.GetLatestKilledEnemyThatCanBeFollower() == null) return;
            if (characterControllerCommunity._gameManager.GetNumAliveEnemyFollowers(characterControllerCommunity) >= 5) return;
            FollowerEnemy_CharacterController enemyFollower = characterControllerCommunity._gameManager.AddLastEnemyFollower(characterControllerCommunity);
            if (enemyFollower == null) return;
            if (enemyFollower.HasSetName) return;
            CharacterData enemyData = enemyFollower.CurrentCharacterData;
            enemyData.charName = $"{enemyData.charName} {followerNum}";
            enemyFollower.HasSetName = true;
            followerNum++;
        }
    }
}
