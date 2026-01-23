using CoffinTech.Utils;
using HarmonyLib;
using Il2Cpp;
using Il2CppDG.Tweening;
using Il2CppVampireSurvivors.App.Tools;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Stage;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Framework.NumberTypes;
using Il2CppVampireSurvivors.Framework.Phaser;
using Il2CppVampireSurvivors.Framework.PhaserTweens;
using Il2CppVampireSurvivors.Framework.TimerSystem;
using Il2CppVampireSurvivors.Objects;
using Il2CppVampireSurvivors.Objects.Characters;
using Il2CppVampireSurvivors.Objects.Items;
using Il2CppVampireSurvivors.Objects.Pickups;
using Il2CppVampireSurvivors.Signals;
using Il2CppVampireSurvivors.Tools;
using Il2CppVampireSurvivors.Graphics;
using Il2CppVampireSurvivors.UI.Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Timer = Il2CppVampireSurvivors.Framework.TimerSystem.Timer;


namespace CommunityCharacter;

#region Supplimental Patches
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
            if(CharacterControllerAssistants.CharacterControllerCommunity._characterType == (CharacterType)20000)
                if(CharacterControllerAssistants.CurrentSkinType == Festa.SkinType) return false;

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
            if (__instance._targetPlayer != CharacterControllerAssistants.CharacterControllerCommunity) return;
            if (__instance.TargetPlayer.CurrentCharacterData.currentSkin.ToString() != TempoDiMelma.SkinType) return;
            CharacterController character = CharacterControllerAssistants.CharacterControllerCommunity;
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
            if (CharacterControllerAssistants.CurrentSkinType != Festa.SkinType) return true;
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
            if (__instance.PlayerOne != CharacterControllerAssistants.CharacterControllerCommunity) return;
            if (__instance.PlayerOne.CurrentCharacterData.currentSkin.ToString() != Beta.SkinType) return;
            if (treasure.hasRandoms) return;
            if (treasure.prizeTypes.IndexOf(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVOLUTION)) > -1)
            {
                treasure.prizeTypes.Clear();
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVOLUTION));
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EXISTING_ANY));
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVOLUTION));
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EXISTING_ANY));
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EXISTING_ANY));
                __result.SetData(ItemType.TREASURE, treasure);
                __result.SetWithEvo();
            }
            else if (!treasure.hasArcana)
            {
                treasure.prizeTypes.Clear();
                treasure.hasArcana = true;
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVO_ARCANA));
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVOLUTION));
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVOLUTION));
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVOLUTION));
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVOLUTION));
                __result.SetData(ItemType.TREASURE, treasure);
                __result.SetArcana(true);
            }
        }
    }
#endregion

#region CharacterController

    internal sealed class CharacterControllerAssistants : ModCharacterController
    {
        internal static CharacterController CharacterControllerCommunity;
        private Timer _hermiesMove;
        private EggFloat _moveSpeed;
        private MultiTargetTween _outerTween;
        private MultiTargetTween _innerTween;
        private MultiTargetTween _scaleTween;
        private PhaserSprite _outerSprite;
        private PhaserSprite _innerSprite;
        private Image _chargeBar;
        private Image _chargeBarFill;
        private Image _zetaBar;
        private Image _zetaBarFill;
    
        private int _killsToTrigger;
        private int _killsToTriggerOffset;
        private int _zetaKills;
        private int _zetaKillsOffset;
        private int _zetaTriggerEffect;
        private int _hermiesLevel;
        private int _opalLevel;
        private int _followerNum;
        internal static string CurrentSkinType;
    
        private float _hermiesTrigger;
        private float _chargeTime;
        private const float MaxChargeTime = 8000f;
    
        private bool _opalFly;
        private bool _vamSub;
        private bool _isCharging;
        private bool _changingSkin;
    
        public override void AfterFullInit(CharacterController instance)
    {
        if (instance._characterType != (CharacterType)20000) return;
        CharacterControllerCommunity = instance;
        CharacterControllerCommunity.OnCriticalHP = new Action(CriticalHp);
        CurrentSkinType = instance.CurrentCharacterData.currentSkin.ToString();
        
        _killsToTrigger = 100;
        _killsToTriggerOffset = 0;
        _zetaKills = 0;
        _zetaKillsOffset = 0;
        _zetaTriggerEffect = 1;
        _hermiesTrigger = 0.33f;
        _hermiesLevel = 1;
        _opalLevel = 1;
        _followerNum = 1;
        
        _changingSkin = false;
        _opalFly = false;
        _vamSub = false;
        CharacterControllerCommunity.IsCriticalHPEnabled = true;
        CharacterControllerCommunity.HasAnyCriticalHPSkill = true;

        if (CurrentSkinType is Zeta.SkinType or nameof(SkinType.DEFAULT))
        {
            instance._spriteAnimation.AddAnimation("black", SpriteManager.GetAnimationFrames("zeta_black_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_black", 2, CharacterControllerCommunity.RespectAnimationXPivots), 8, true);
            instance._spriteAnimation.AddAnimation("moon", SpriteManager.GetAnimationFrames("zeta_moon_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_moon", 2, CharacterControllerCommunity.RespectAnimationXPivots), 8, true);
            instance._spriteAnimation.AddAnimation("city", SpriteManager.GetAnimationFrames("zeta_city_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_city", 2, CharacterControllerCommunity.RespectAnimationXPivots), 8, true);
            instance._spriteAnimation.AddAnimation("stone", SpriteManager.GetAnimationFrames("zeta_stone_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_stone", 2, CharacterControllerCommunity.RespectAnimationXPivots), 8, true);
            instance._spriteAnimation.AddAnimation("sun", SpriteManager.GetAnimationFrames("zeta_sun_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_sun", 2, CharacterControllerCommunity.RespectAnimationXPivots), 8, true);
            instance._spriteAnimation.AddAnimation("volcano", SpriteManager.GetAnimationFrames("zeta_volcano_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_volcano", 2, CharacterControllerCommunity.RespectAnimationXPivots), 8, true);
            instance._spriteAnimation.AddAnimation("seawinds", SpriteManager.GetAnimationFrames("zeta_seawinds_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_seawinds", 2, CharacterControllerCommunity.RespectAnimationXPivots), 8, true);
            instance._spriteAnimation.AddAnimation("directer", SpriteManager.GetAnimationFrames("zeta_directer_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_directer", 2, CharacterControllerCommunity.RespectAnimationXPivots), 8, true);
            instance._spriteAnimation.SetAnimation("black");
            
            var zetaCanvas = Object.Instantiate(CharacterControllerCommunity.gameObject.transform.FindChild("CharacterRenderer/Canvas - HealthBar").gameObject,
                CharacterControllerCommunity.gameObject.transform.FindChild("CharacterRenderer"), true);
            zetaCanvas.name = "ZetaCanvas";
            var zetaHealth = zetaCanvas.GetComponent<HealthBar>();
            Object.Destroy(zetaHealth);
            var zetaHealthBar = zetaCanvas.transform.Find("HealthBar").gameObject;
            Object.Destroy(zetaHealthBar);
            var zetaCanvasTransform =  zetaCanvas.GetComponent<RectTransform>();
            zetaCanvasTransform.anchoredPosition = new Vector2(-0.182f, 0.13f);
            zetaCanvasTransform.sizeDelta = new Vector2(14f, 150f);
            
            var zetaBar = new GameObject
            {
                name = "ZetaBar"
            };
            var zetaBarFill = new GameObject
            {
                name = "ZetaBarFill"
            };
            
            zetaBar.transform.SetParent(zetaCanvas.transform);
            zetaBarFill.transform.SetParent(zetaBar.transform);
            zetaBar.AddComponent<Image>();
            zetaBarFill.AddComponent<Image>();
        
            var zetaBarRect = zetaBar.GetComponent<RectTransform>();
            var zetaBarFillRect = zetaBarFill.GetComponent<RectTransform>();
        
            zetaBarRect.anchoredPosition = new Vector2(0, 0);
            zetaBarRect.anchorMax = new Vector2(1, 1);
            zetaBarRect.anchorMin = new Vector2(0, 0);
            zetaBarRect.sizeDelta = new Vector2(0, 0);
        
            zetaBarFillRect.anchorMax = new Vector2(1, 1);
            zetaBarFillRect.anchorMin = new Vector2(0, 0);
            zetaBarFillRect.sizeDelta = new Vector2(0, 0);
            
            _zetaBar = zetaBar.GetComponent<Image>();
            _zetaBar.transform.localScale = new Vector3(1f, 1f, 1f);
            _zetaBar.color = new Color32(0, 0, 0, 255);
        
            _zetaBarFill = zetaBarFill.GetComponent<Image>();
            _zetaBarFill.fillMethod = Image.FillMethod.Vertical;
            _zetaBarFill.type = Image.Type.Filled;
            _zetaBarFill.color = new Color32(70, 28, 200, 255);
        
            _zetaBar.sprite = _zetaBarFill.sprite = SpriteManager.GetUnpackedSprite("UISquare");
        }

        if (instance.CurrentCharacterData.currentSkin != SkinType.DEFAULT) return;
        _changingSkin = true;
        instance._spriteAnimation.AddAnimation("hermies", SpriteManager.GetAnimationFrames("hermies_", 1, 4, new Vector2(0.5f, 0f), "character_community_hermies", 2, CharacterControllerCommunity.RespectAnimationXPivots), 8, true);
        instance._spriteAnimation.AddAnimation("opal", SpriteManager.GetAnimationFrames("opal_", 1, 4, new Vector2(0.5f, 0f), "character_community_opal", 2, CharacterControllerCommunity.RespectAnimationXPivots), 8, true);
        instance._spriteAnimation.AddAnimation("festa", SpriteManager.GetAnimationFrames("festa_", 1, 4, new Vector2(0.5f, 0f), "character_community_festa", 2, CharacterControllerCommunity.RespectAnimationXPivots), 8, true);
        instance._spriteAnimation.AddAnimation("beta", SpriteManager.GetAnimationFrames("beta_", 1, 4, new Vector2(0.5f, 0f), "character_community_beta", 2, CharacterControllerCommunity.RespectAnimationXPivots), 8, true);
        instance._spriteAnimation.AddAnimation("tempo", SpriteManager.GetAnimationFrames("tempo_", 1, 4, new Vector2(0.5f, 0f), "character_community_tempo", 2, CharacterControllerCommunity.RespectAnimationXPivots), 8, true);
        instance._spriteAnimation.AddAnimation("vam", SpriteManager.GetAnimationFrames("vam_", 1, 4, new Vector2(0.5f, 0f), "character_community_vam", 2, CharacterControllerCommunity.RespectAnimationXPivots), 8, true);
        instance._spriteAnimation.AddAnimation("decapo", SpriteManager.GetAnimationFrames("decapo_", 1, 4, new Vector2(0.5f, 0f), "character_community_decapo", 2, CharacterControllerCommunity.RespectAnimationXPivots), 8, true);

        var chargeBar = new GameObject
        {
            name = "ChargeBar"
        };
        var chargeBarFill = new GameObject
        {
            name = "ChargeBarFill"
        };
        chargeBar.transform.SetParent(CharacterControllerCommunity.gameObject.transform.FindChild("CharacterRenderer/Canvas - HealthBar").transform);
        chargeBarFill.transform.SetParent(chargeBar.transform);
        
        chargeBar.transform.localPosition = new Vector3(0f, -24f, 0f);
        
        chargeBar.AddComponent<Image>();
        chargeBarFill.AddComponent<Image>();
        
        var chargeBarRect = chargeBar.GetComponent<RectTransform>();
        var chargeBarFillRect = chargeBarFill.GetComponent<RectTransform>();
        
        chargeBarRect.anchorMax = new Vector2(1, 1);
        chargeBarRect.anchorMin = new Vector2(0, 0);
        chargeBarRect.sizeDelta = new Vector2(0, 0);
        
        chargeBarFillRect.anchorMax = new Vector2(1, 1);
        chargeBarFillRect.anchorMin = new Vector2(0, 0);
        chargeBarFillRect.sizeDelta = new Vector2(0, 0);
        
        _chargeBar = chargeBar.GetComponent<Image>();
        _chargeBar.transform.localScale = new Vector3(1f, 1f, 1f);
        _chargeBar.color = new Color32(0, 0, 0, 255);
        
        _chargeBarFill = chargeBarFill.GetComponent<Image>();
        _chargeBarFill.fillMethod = Image.FillMethod.Horizontal;
        _chargeBarFill.type = Image.Type.Filled;
        _chargeBarFill.color = new Color32(170, 95, 0, 255);
        
        _chargeBar.sprite = _chargeBarFill.sprite = SpriteManager.GetUnpackedSprite("UISquare");
        CharacterControllerCommunity.gameObject.transform.FindChild("WickedSeason").transform.localPosition = new Vector3(0f, -0.129f, 0f);

        _chargeTime = 0f;
        _isCharging = false;
        
        _outerSprite =
            instance.AddPhaserSprite(Vector2.zero, "character_community_outerRing", "cc_outerRing_01");
        _innerSprite =
            instance.AddPhaserSprite(Vector2.zero, "character_community_innerRing", "cc_innerRing_01");
        
        _outerSprite.setDepth(-1);
        _innerSprite.setDepth(-1);
        
        _outerSprite.setAlpha(0);
        _innerSprite.setAlpha(0);
        
        _outerSprite.transform.localPosition = new Vector3(0f, 0.16f, 0f);
        _innerSprite.transform.localPosition = new Vector3(0f, 0.16f, 0f);
        
        var outerTween = _outerTween;
        var innerTween = _innerTween;
        var scaleTween = _scaleTween;
        
        outerTween?.Stop();
        innerTween?.Stop();
        scaleTween?.Stop();
        _outerTween = Tweens.Add(new TweenConfig
        {
            targets = new[]
            {
                _outerSprite
            },
            angle = new Il2CppSystem.Nullable<float>(360),
            duration = 2000f,
            repeat = -1
        });
        _innerTween = Tweens.Add(new TweenConfig
        {
            targets = new[]
            {
                _innerSprite
            },
            angle = new Il2CppSystem.Nullable<float>(-360),
            duration = 2300f,
            repeat = -1
        });
        _scaleTween = Tweens.Add(new TweenConfig
        {
            targets = new[]
            {
                _outerSprite,
                _innerSprite
            },
            angle = new Il2CppSystem.Nullable<float>(360),
            duration = 2000f,
            repeat = -1,
            yoyo = true,
            ease = Ease.InOutSine,
            staggerDelay = Tweens.Stagger(150f, new StaggerConfig
            {
                start = 0f
            })
        });
        
        CycleSkin();
    }
    
        public override void OnUpdate(CharacterController instance)
        {
            // Zeta
            if (CurrentSkinType == Zeta.SkinType) ZetaAspectEffects();
            // Hermies
            if (CurrentSkinType == Hermies.SkinType && _hermiesMove == null && !instance._spriteAnimation.IsPaused)
            { 
                _moveSpeed = instance._playerStats.MoveSpeed;
                _hermiesMove = Timers.Register(20000, new Action(HermiesMovespeedTake), new Action<float>(HermiesMovespeedGive));
            }
            // Opal
            if (CurrentSkinType == Opal.SkinType && !_opalFly)
            {
                _opalFly = true;
                CharacterControllerCommunity._gameManager._physicsManager._playersWithWallCollisionGroup.remove(CharacterControllerCommunity.GetComponent<ArcadeSprite>());
            }
            // Tempo
            if (CurrentSkinType == TempoDiMelma.SkinType) TempoEffects();
            // Vam
            if (CurrentSkinType == VamAndPyre.SkinType)
            {
                if (!_vamSub)
                {
                    _vamSub = true;
                    CharacterControllerCommunity._signalBus.Subscribe<GameplaySignals.EnemyKilledImmediateSignal>(
                        new Action(MakeEnemyFollower));
                }
            }
            else if (_vamSub)
            {
                CharacterControllerCommunity._signalBus.TryUnsubscribe<GameplaySignals.EnemyKilledImmediateSignal>(
                    new Action(MakeEnemyFollower));
                _vamSub = false;
            }

            if (!_changingSkin) return;
            if (instance.Walked != 0f)
            {
                _outerSprite.setAlpha(0f);
                _innerSprite.setAlpha(0f);
                _chargeBarFill.fillAmount = 0f;
                HideCharge(); 
                _chargeTime = 0f;
                return;
            }
            ShowCharge();
            _chargeTime += PauseSystem.DeltaTimeMillis;
            float fill = _chargeTime / MaxChargeTime;
            float alpha = 0.25f + 0.75f * fill;
            _outerSprite.setAlpha(alpha);
            _innerSprite.setAlpha(alpha);
            _chargeBarFill.fillAmount = fill;
            if(_chargeTime < MaxChargeTime) return;
            _outerSprite.setAlpha(0f);
            _innerSprite.setAlpha(0f);
            HideCharge();
            _chargeTime = 0f;
            CycleSkin();
        }

        public override void HandleLateUpdate(CharacterController instance)
        {
            var enemyPullSystem = new EnemyPullSystem();
            if (PauseSystem.Paused) return;
            if (CurrentSkinType == DeCapo.SkinType)
                enemyPullSystem.ProcessEnemies(instance);
        }
    
        public override void OnStop(CharacterController instance)
        {
            if (_hermiesMove != null)
                _hermiesMove.Complete();
        }
    
        public override void LevelUp(CharacterController instance)
        {
            // Hermies Armor gain

            if (CurrentSkinType == Hermies.SkinType)
            {
                _hermiesLevel++;
                if (_hermiesLevel % 20 == 0)
                    instance.PlayerStats.Armor += 1f;
            }

            if (CurrentSkinType == Opal.SkinType) OpalEffect();
        }

        /// <summary>
        /// Regular Methods to call
        /// </summary>
        private void CriticalHp() 
        {
            if (CurrentSkinType == Zeta.SkinType)
                CharacterControllerCommunity.AddTemporaryBonus(new Action(ZetaCritGive), new Action(ZetaCritRemove), 5000f);
    }

        private void HideCharge()
        {
            _chargeBarFill.SetAlpha(0.35f);
            _chargeBar.SetAlpha(0.35f);
            _isCharging = false;
        }

        private void ShowCharge()
        {
            _chargeBarFill.SetAlpha(1f);
            _chargeBar.SetAlpha(1f);
            if(!_isCharging) _isCharging = true;
        }

        /// <summary>
        /// Zeta Methods
        /// </summary>
        private void ZetaCritGive()
        {
            CharacterControllerCommunity._playerStats.Power += 0.5f;
            CharacterControllerCommunity._playerStats.Speed += 0.5f;
            CharacterControllerCommunity._playerStats.Duration += 0.5f;
            CharacterControllerCommunity._playerStats.Area += 0.5f;
            CharacterControllerCommunity._playerStats.Regen += 7f;
        }

        private void ZetaCritRemove()
        {
            CharacterControllerCommunity._playerStats.Power -= 0.5f;
            CharacterControllerCommunity._playerStats.Speed -= 0.5f;
            CharacterControllerCommunity._playerStats.Duration -= 0.5f;
            CharacterControllerCommunity._playerStats.Area -= 0.5f;
            CharacterControllerCommunity._playerStats.Regen -= 7f;
        }

        private void ZetaGemEffect()
        {

            var pickups = new Il2CppSystem.Collections.Generic.HashSet<Pickup>();
            var i = 0;
            foreach (var a in CharacterControllerCommunity._gameManager._gems)
            {
                var gem = a.Cast<Gem>();
                var bless = CharacterControllerCommunity.PAmount() + Mathf.Log(CharacterControllerCommunity._gameManager._gems.Count)/UnityEngine.Random.Range(1,7);
                gem.BlessColor(bless, i);
                pickups.Add(gem);
                i++;
            }
            CharacterControllerCommunity._gameManager._gems = pickups;
        }

        private void ZetaHeartEffect()
        {
            var position = CharacterControllerCommunity.transform.position;
            const float num = 9f;
            const float num2 = 3.1415927f / num;
            var num3 = UnityEngine.Random.value * 1.5707963f;
            for (var i = 0; i <= 17; i++)
            {
                var pos = new Vector2
                {
                    x = position.x + Mathf.Sin(num3 + num2 * i) * 1.25f,
                    y = position.y + Mathf.Cos(num3 + num2 * i) * 1.25f
                };
                PickupManager.PickupItems.Add(PickupManager.CreatePickup(pos, ItemType.LITTLEHEART));
            }
        }

        private void ZetaChestEffect()
        {
            var pickups = new List<Pickup>();
            var pickupsRemove = new List<Pickup>();
            foreach (var a in PickupManager.PickupItems)
            {
                if(pickups.Contains(a)) continue;
                if (a.PickupType != ItemType.TREASURE) continue;
                
                var treasureC = a.Cast<TreasureChest>();
                var treasure = treasureC._treasure;
                
                treasure.level = 3;
                treasure.prizeTypes.Clear();
                
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVOLUTION));
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVOLUTION));
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVOLUTION));
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVOLUTION));
                treasure.prizeTypes.Add(new Il2CppSystem.Nullable<PrizeType>(PrizeType.EVOLUTION));
                
                treasureC.SetData(ItemType.TREASURE, treasure);
                treasureC.SetFrame("BoxOpen3");
                
                pickups.Add(treasureC);
                pickupsRemove.Add(a);
            }
            pickups.ForEach(pickup => PickupManager.PickupItems.Add(pickup));
            pickupsRemove.ForEach(pickup => PickupManager.PickupItems.Remove(pickup));
        }

        private void ZetaAspectEffects()
        {
            var kills = CharacterControllerCommunity._playerOptions.Config.RunEnemies;

            if (kills - _zetaKills > 50)
            {
                var offsetCache = 0;
                if (_zetaKillsOffset == 0 || kills - _zetaKillsOffset > 100)
                    _zetaKillsOffset = kills;
                else
                {
                    offsetCache = kills - _zetaKillsOffset;
                }
                
                _zetaKills += offsetCache;
                _zetaKillsOffset += offsetCache;
            }
            else
                _zetaKills = kills;

            var fill = (float)(_zetaKills - _killsToTriggerOffset) / (_killsToTrigger - _killsToTriggerOffset);
            _zetaBarFill.fillAmount = fill;
            if (_zetaKills < _killsToTrigger) return;
            
            _killsToTriggerOffset = _killsToTrigger;
            _killsToTrigger = (int)(_killsToTrigger + (_killsToTrigger * 0.2f));
            
            //Chest to Gold chest
            //Gems - shine or color change
            switch (_zetaTriggerEffect)
            {
                case 1:
                    CharacterControllerCommunity._gameManager.MakeAndActivatePickup(ItemType.CLOVER, CharacterControllerCommunity);
                    CharacterControllerCommunity.Anims.SetAnimation("moon");
                    break;
                case 2:
                    CharacterControllerCommunity._gameManager.MakeAndActivatePickup(ItemType.ROSARY, CharacterControllerCommunity);
                    CharacterControllerCommunity.Anims.SetAnimation("city");
                    break;
                case 3:
                    ZetaGemEffect();
                    CharacterControllerCommunity.Anims.SetAnimation("stone");
                    break;
                case 4:
                    CharacterControllerCommunity._gameManager.TriggerGoldFever(20000);
                    CharacterControllerCommunity.Anims.SetAnimation("sun");
                    break;
                case 5:
                    ZetaHeartEffect();
                    CharacterControllerCommunity.Anims.SetAnimation("volcano");
                    break;
                case 6:
                    ZetaChestEffect();
                    CharacterControllerCommunity.Anims.SetAnimation("seawinds");
                    break;
                case 7:
                    CharacterControllerCommunity._gameManager.MakeAndActivatePickup(ItemType.VACUUM, CharacterControllerCommunity);
                    CharacterControllerCommunity.Anims.SetAnimation("directer");
                    break;
                default:
                    CharacterControllerCommunity._gameManager.MakeAndActivatePickup(ItemType.CLOVER, CharacterControllerCommunity);
                    CharacterControllerCommunity._gameManager.MakeAndActivatePickup(ItemType.ROSARY, CharacterControllerCommunity);
                    ZetaGemEffect();
                    CharacterControllerCommunity._gameManager.TriggerGoldFever(20000);
                    ZetaHeartEffect();
                    ZetaChestEffect();
                    CharacterControllerCommunity._gameManager.MakeAndActivatePickup(ItemType.VACUUM, CharacterControllerCommunity);
                    _zetaTriggerEffect = 0;
                    CharacterControllerCommunity.Anims.SetAnimation("black");
                    break;
            }

            _zetaTriggerEffect++;
        }

        /// <summary>
        /// Hermies Methods
        /// </summary>
        private void HermiesMovespeedGive(float time)
        {
            if (time < _hermiesTrigger) return;
            _hermiesTrigger+= 0.33f;
            CharacterControllerCommunity._playerStats.MoveSpeed += 0.01f;
        }

        private void HermiesMovespeedTake()
        {
            _hermiesMove.Cancel();
            _hermiesTrigger = 0f;
            CharacterControllerCommunity._playerStats.MoveSpeed = _moveSpeed;
            _hermiesMove = null;
        }

        /// <summary>
        /// Opal Methods
        /// </summary>
        private void OpalEffect()
        {
            _opalLevel++;
            if (_opalLevel % 4 == 0) CharacterControllerCommunity._playerStats.Charm++;
            
            var position = CharacterControllerCommunity.transform.position;
            var num = (int)CharacterControllerCommunity.PAmount();
            if (num == 0) return;
            
            var num2 = 3.1415927f / num;
            var num3 = UnityEngine.Random.value * 1.5707963f;
            for (var i = 0; i <= num; i++)
            {
                var pos = new Vector2
                {
                    x = position.x + Mathf.Sin(num3 + num2 * i) * 1.25f,
                    y = position.y + Mathf.Cos(num3 + num2 * i) * 1.25f
                };
                CharacterControllerCommunity._gameManager.Stage.MakeDestructible(CharacterControllerCommunity._gameManager.Stage.DestructibleType, pos);
            }
        }

        /// <summary>
        /// Tempo Methods
        /// </summary>
        private void TempoEffects()
        {
            var kills = CharacterControllerCommunity._playerOptions.Config.RunEnemies;
            var random = UnityEngine.Random.Range(1, 16);
            
            if (kills < _killsToTrigger) return;

            _killsToTrigger = (int)(_killsToTrigger + (_killsToTrigger * 0.2f));

            switch (random)
            {
                case 1:
                    CharacterControllerCommunity.PlayerStats.Amount += 0.25f;
                    break;
                case 2:
                    CharacterControllerCommunity.PlayerStats.Area += 0.25f;
                    break;
                case 3:
                    CharacterControllerCommunity.PlayerStats.Armor += 0.25f;
                    break;
                case 4:
                    CharacterControllerCommunity.PlayerStats.Cooldown -= 0.05f;
                    break;
                case 5:
                    CharacterControllerCommunity.PlayerStats.Duration += 0.25f;
                    break;
                case 6:
                    CharacterControllerCommunity.PlayerStats.Greed += 0.25f;
                    break;
                case 7:
                    CharacterControllerCommunity.PlayerStats.Growth += 0.25f;
                    break;
                case 8:
                    CharacterControllerCommunity.PlayerStats.Luck += 0.25f;
                    break;
                case 9:
                    CharacterControllerCommunity.PlayerStats.Magnet += 0.25f;
                    break;
                case 10:
                    CharacterControllerCommunity.PlayerStats.MaxHp += 5f;
                    break;
                case 11:
                    CharacterControllerCommunity.PlayerStats.MoveSpeed += 0.25f;
                    break;
                case 12:
                    CharacterControllerCommunity.PlayerStats.Power += 0.25f;
                    break;
                case 13:
                    CharacterControllerCommunity.PlayerStats.Regen += 0.25f;
                    break;
                case 14:
                    CharacterControllerCommunity.PlayerStats.Revivals += 0.25f;
                    break;
                case 15:
                    CharacterControllerCommunity.PlayerStats.Speed += 0.25f;
                    break;
            }

        }

        /// <summary>
        /// Beta and Vamp Methods
        /// </summary>
        private void MakeEnemyFollower()
        {
            if (CharacterControllerCommunity._gameManager.GetLatestKilledEnemyThatCanBeFollower() == null) return;
            if (CharacterControllerCommunity._gameManager.GetNumAliveEnemyFollowers(CharacterControllerCommunity) >= 5) return;
            
            var enemyFollower = CharacterControllerCommunity._gameManager.AddLastEnemyFollower(CharacterControllerCommunity);
            
            if (enemyFollower == null) return;
            if (enemyFollower.HasSetName) return;
            
            var enemyData = enemyFollower.CurrentCharacterData;
            
            enemyData.charName = $"{enemyData.charName} {_followerNum}";
            enemyFollower.HasSetName = true;
            _followerNum++;
        }

        private void ZetaSkinGetter()
        {
            switch (_zetaTriggerEffect)
            {
                case 1:
                    CharacterControllerCommunity.Anims.SetAnimation("moon");
                    break;
                case 2:
                    CharacterControllerCommunity.Anims.SetAnimation("city");
                    break;
                case 3:
                    CharacterControllerCommunity.Anims.SetAnimation("stone");
                    break;
                case 4:
                    CharacterControllerCommunity.Anims.SetAnimation("sun");
                    break;
                case 5:
                    CharacterControllerCommunity.Anims.SetAnimation("volcano");
                    break;
                case 6:
                    CharacterControllerCommunity.Anims.SetAnimation("seawinds");
                    break;
                case 7:
                    CharacterControllerCommunity.Anims.SetAnimation("directer");
                    break;
                default:
                    CharacterControllerCommunity.Anims.SetAnimation("black");
                    break;
            }
        }

        private void CycleSkin()
        {
            if (CurrentSkinType == nameof(SkinType.DEFAULT))
            {
                Zeta.ZetaStatApply(CharacterControllerCommunity);
                CurrentSkinType = Zeta.SkinType;
                return;
            }
            
            switch (CurrentSkinType)
            {
                case Zeta.SkinType:
                    Zeta.ZetaStatApply(CharacterControllerCommunity, false);
                    _zetaBar.gameObject.active = false;
                    CharacterControllerCommunity.Anims.SetAnimation("hermies");
                    CurrentSkinType = Hermies.SkinType;
                    Hermies.HermiesStatApply(CharacterControllerCommunity);
                    break;
                case Hermies.SkinType:
                    Hermies.HermiesStatApply(CharacterControllerCommunity, false);
                    CharacterControllerCommunity.Anims.SetAnimation("opal");
                    CurrentSkinType = Opal.SkinType;
                    Opal.OpalStatApply(CharacterControllerCommunity);
                    break;
                case Opal.SkinType:
                    Opal.OpalStatApply(CharacterControllerCommunity, false);
                    CharacterControllerCommunity.Anims.SetAnimation("festa");
                    CurrentSkinType = Festa.SkinType;
                    Festa.FestaStatApply(CharacterControllerCommunity);
                    break;
                case Festa.SkinType:
                    Festa.FestaStatApply(CharacterControllerCommunity, false);
                    CharacterControllerCommunity.Anims.SetAnimation("beta");
                    CurrentSkinType = Beta.SkinType;
                    Beta.BetaStatApply(CharacterControllerCommunity);
                    break;
                case Beta.SkinType:
                    Beta.BetaStatApply(CharacterControllerCommunity, false);
                    CharacterControllerCommunity.Anims.SetAnimation("tempo");
                    CurrentSkinType = TempoDiMelma.SkinType;
                    TempoDiMelma.TempoStatApply(CharacterControllerCommunity);
                    break;
                case TempoDiMelma.SkinType:
                    TempoDiMelma.TempoStatApply(CharacterControllerCommunity, false);
                    CharacterControllerCommunity.Anims.SetAnimation("vam");
                    CurrentSkinType = VamAndPyre.SkinType;
                    VamAndPyre.VamStatApply(CharacterControllerCommunity);
                    break;
                case VamAndPyre.SkinType:
                    VamAndPyre.VamStatApply(CharacterControllerCommunity, false);
                    CharacterControllerCommunity.Anims.SetAnimation("decapo");
                    CurrentSkinType = DeCapo.SkinType;
                    DeCapo.DeCapoStatApply(CharacterControllerCommunity);
                    break;
                case DeCapo.SkinType:
                    DeCapo.DeCapoStatApply(CharacterControllerCommunity, false);
                    _zetaBar.gameObject.active = true;
                    ZetaSkinGetter();
                    CurrentSkinType = Zeta.SkinType;
                    Zeta.ZetaStatApply(CharacterControllerCommunity);
                    break;
            }

        }
    }
#endregion

#region EnemyPuller

public class EnemyPullSystem
{
    private readonly List<EnemyController> _enemies = new(512);
    
    private const float MaxPullRange = 8.0f;
    private const float MinPullRange = 0.2f; // dead zone
    private const float MaxForceClamp = 10.0f;
    
    private int _batchIndex;
    private int _batchSize = 256;

    private const int MinBatchSize = 16;
    private const int MaxBatchSize = 512;

    private const float TargetBatchTime = 0.016f;

    private int _cacheFrame = -1;
    
    
    private void CacheEnemies(CharacterController cc)
    {
        var frame = Time.frameCount;
        if (_cacheFrame == frame) return;

        _cacheFrame = frame;
        _batchIndex = 0;
        _enemies.Clear();

        var children = cc._gameManager.Enemies.getChildren();
        foreach (var obj in children)
        {
            if (obj.body == null || !obj.body._enable)
                continue;

            var enemy = obj.Cast<EnemyController>();
            var type = enemy.EnemyType;

            if (type == EnemyType.EX_CART_DROWNER ||
                type == EnemyType.BOSS_DROWNER_RASH ||
                type == EnemyType.BOSS_DROWNER_NORMAL ||
                type == EnemyType.BOSS_XLDROWNER ||
                type == EnemyType.TP_BOSS_DEATH ||
                type == EnemyType.TP_BOSS_DEATH_SCYTHEBIG ||
                type == EnemyType.TP_BOSS_DEATHARM)
                continue;

            _enemies.Add(enemy);
        }
    }

    public void ProcessEnemies(CharacterController cc)
    {
        CacheEnemies(cc);

        var count = _enemies.Count;
        if (count == 0) return;

        var playerPos = cc.transform.position.ToFloat2();
        playerPos.x += cc.flipX ? -0.25f : 0.25f;

        var dt = PauseSystem.DeltaTime;
        var baseStrength = cc.PArea();

        var end = _batchIndex + _batchSize;
        if (end > count) end = count;

        var sw = System.Diagnostics.Stopwatch.StartNew();

        for (var i = _batchIndex; i < end; i++)
        {
            var enemy = _enemies[i];
            
            var dir = playerPos - enemy.position;
            var distSq = math.dot(dir, dir);

            if (distSq < MinPullRange * MinPullRange ||
                distSq > MaxPullRange * MaxPullRange)
                continue;

            var dist = math.sqrt(distSq);
            var normDir = dir / dist;

            var t = 1f - math.saturate(dist / MaxPullRange);
            var falloff = t * t;

            var force = math.min(baseStrength * falloff, MaxForceClamp);
            enemy.position += normDir * force * dt;
        }

        sw.Stop();

        var elapsedMs = (float)sw.Elapsed.TotalMilliseconds;

        switch (elapsedMs)
        {
            case < TargetBatchTime * 0.75f when _batchSize < MaxBatchSize:
                _batchSize += 8;
                break;
            case > TargetBatchTime * 1.25f when _batchSize > MinBatchSize:
                _batchSize -= 8;
                break;
        }

        _batchIndex = end >= count ? 0 : end;
    }
}
#endregion
