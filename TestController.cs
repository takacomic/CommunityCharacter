using System.Runtime.CompilerServices;
using Il2Cpp;
using Il2CppDG.Tweening;
using Il2CppInterop.Runtime.Attributes;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppVampireSurvivors.App.Tools;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Characters;
using Il2CppVampireSurvivors.Data.Stage;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Framework.NumberTypes;
using Il2CppVampireSurvivors.Framework.Phaser;
using Il2CppVampireSurvivors.Framework.PhaserTweens;
using Il2CppVampireSurvivors.Framework.TimerSystem;
using Il2CppVampireSurvivors.Graphics;
using Il2CppVampireSurvivors.Objects;
using Il2CppVampireSurvivors.Objects.Characters;
using Il2CppVampireSurvivors.Objects.Items;
using Il2CppVampireSurvivors.Objects.Pickups;
using Il2CppVampireSurvivors.Signals;
using Il2CppVampireSurvivors.UI.Player;
using MelonLoader;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Timer = Il2CppVampireSurvivors.Framework.TimerSystem.Timer;

namespace CommunityCharacter;
[RegisterTypeInIl2Cpp]
public class CharacterControllerCommunity : CharacterController
{
    public CharacterControllerCommunity(IntPtr ptr) : base(ptr)
    {
    }
    public CharacterControllerCommunity() : base(ClassInjector.DerivedConstructorPointer<CharacterControllerCommunity>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }
    
    internal  string currentSkinType;
    int killsToTrigger;
    int zetaTriggerEffect;
    float hermiesTrigger;
    Timer hermiesMove;
    EggFloat moveSpeed;
    int hermiesLevel;
    int opalLevel;
    bool opalFly;
    int followerNum;
    bool betaVamSub;
    MultiTargetTween outerTween;
    MultiTargetTween innerTween;
    PhaserSprite outerSprite;
    PhaserSprite innerSprite;
    float chargeTime;
    Image healthBar;
    Image healthBarFill;
    bool isCharging;
    float maxChargeTime = 8000f;
    MultiTargetTween scaleTween;
    bool changingSkin;
    
    public override void AfterFullInitialization()
        {
            if (_characterType != (CharacterType)20000) return;
            changingSkin = false;
            currentSkinType = CurrentCharacterData.currentSkin.ToString();
            killsToTrigger = 100;
            zetaTriggerEffect = 1;
            IsCriticalHPEnabled = true;
            HasAnyCriticalHPSkill = true;
            OnCriticalHP = new Action(CriticalHp);
            hermiesTrigger = 0.33f;
            hermiesLevel = 1;
            opalLevel = 1;
            opalFly = false;
            followerNum = 1;
            betaVamSub = false;
            
            if (currentSkinType == Zeta.SkinType ||  currentSkinType == nameof(SkinType.DEFAULT))
            {
                _spriteAnimation.AddAnimation("black", SpriteManager.GetAnimationFrames("zeta_black_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_black", 2, RespectAnimationXPivots), 8, true);
                _spriteAnimation.AddAnimation("moon", SpriteManager.GetAnimationFrames("zeta_moon_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_moon", 2, RespectAnimationXPivots), 8, true);
                _spriteAnimation.AddAnimation("city", SpriteManager.GetAnimationFrames("zeta_city_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_city", 2, RespectAnimationXPivots), 8, true);
                _spriteAnimation.AddAnimation("stone", SpriteManager.GetAnimationFrames("zeta_stone_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_stone", 2, RespectAnimationXPivots), 8, true);
                _spriteAnimation.AddAnimation("sun", SpriteManager.GetAnimationFrames("zeta_sun_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_sun", 2, RespectAnimationXPivots), 8, true);
                _spriteAnimation.AddAnimation("volcano", SpriteManager.GetAnimationFrames("zeta_volcano_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_volcano", 2, RespectAnimationXPivots), 8, true);
                _spriteAnimation.AddAnimation("seawinds", SpriteManager.GetAnimationFrames("zeta_seawinds_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_seawinds", 2, RespectAnimationXPivots), 8, true);
                _spriteAnimation.AddAnimation("directer", SpriteManager.GetAnimationFrames("zeta_directer_", 1, 4, new Vector2(0.5f, 0f), "character_community_zeta_directer", 2, RespectAnimationXPivots), 8, true);
                _spriteAnimation.SetAnimation("black");
            }
            
            if(CurrentCharacterData.currentSkin == SkinType.DEFAULT)
            {
                changingSkin = true;
                _spriteAnimation.AddAnimation("hermies", SpriteManager.GetAnimationFrames("hermies_", 1, 4, new Vector2(0.5f, 0f), "character_community_hermies", 2, RespectAnimationXPivots), 8, true);
                _spriteAnimation.AddAnimation("opal", SpriteManager.GetAnimationFrames("opal_", 1, 4, new Vector2(0.5f, 0f), "character_community_opal", 2, RespectAnimationXPivots), 8, true);
                _spriteAnimation.AddAnimation("festa", SpriteManager.GetAnimationFrames("festa_", 1, 4, new Vector2(0.5f, 0f), "character_community_festa", 2, RespectAnimationXPivots), 8, true);
                _spriteAnimation.AddAnimation("beta", SpriteManager.GetAnimationFrames("beta_", 1, 4, new Vector2(0.5f, 0f), "character_community_beta", 2, RespectAnimationXPivots), 8, true);
                _spriteAnimation.AddAnimation("tempo", SpriteManager.GetAnimationFrames("tempo_", 1, 4, new Vector2(0.5f, 0f), "character_community_tempo", 2, RespectAnimationXPivots), 8, true);
                _spriteAnimation.AddAnimation("vam", SpriteManager.GetAnimationFrames("vam_", 1, 4, new Vector2(0.5f, 0f), "character_community_vam", 2, RespectAnimationXPivots), 8, true);
                _spriteAnimation.AddAnimation("decapo", SpriteManager.GetAnimationFrames("decapo_", 1, 4, new Vector2(0.5f, 0f), "character_community_decapo", 2, RespectAnimationXPivots), 8, true);

                GameObject chargeBarObject = new GameObject();
                chargeBarObject.name = "ChargeBar";
                chargeBarObject.transform.SetParent(gameObject.transform.FindChild("CharacterRenderer/Canvas - HealthBar").transform);
                GameObject chargeBarFillObject = new GameObject();
                chargeBarFillObject.name = "ChargeBarFill";
                chargeBarFillObject.transform.SetParent(chargeBarObject.transform);
                chargeBarObject.transform.localPosition = new Vector3(0f, -24f, 0f);
                chargeBarObject.AddComponent<Image>();
                chargeBarFillObject.AddComponent<Image>();
                RectTransform chargeBarRect = chargeBarObject.GetComponent<RectTransform>();
                RectTransform chargeBarFillRect = chargeBarFillObject.GetComponent<RectTransform>();
                chargeBarRect.anchorMax = new Vector2(1, 1);
                chargeBarRect.anchorMin = new Vector2(0, 0);
                chargeBarRect.sizeDelta = new Vector2(0, 0);
                chargeBarFillRect.anchorMax = new Vector2(1, 1);
                chargeBarFillRect.anchorMin = new Vector2(0, 0);
                chargeBarFillRect.sizeDelta = new Vector2(0, 0);
                healthBar = chargeBarObject.GetComponent<Image>();
                healthBar.transform.localScale = new Vector3(1f, 1f, 1f);
                healthBar.color = new Color32(0, 0, 0, 255);
                healthBarFill = chargeBarFillObject.GetComponent<Image>();
                healthBarFill.fillMethod = Image.FillMethod.Horizontal;
                healthBarFill.type = Image.Type.Filled;
                healthBarFill.color = new Color32(170, 95, 0, 255);
                healthBar.sprite = healthBarFill.sprite = SpriteManager.GetUnpackedSprite("UISquare");
                gameObject.transform.FindChild("WickedSeason").transform.localPosition = new Vector3(0f, -0.129f, 0f);
                
                chargeTime = 0f;
                isCharging = false;
                outerSprite = this.AddPhaserSprite(Vector2.zero, "character_community_outerRing", "cc_outerRing_01");
                innerSprite = this.AddPhaserSprite(Vector2.zero, "character_community_innerRing", "cc_innerRing_01");
                outerSprite.setDepth(-1);
                innerSprite.setDepth(-1);
                outerSprite.setAlpha(0);
                innerSprite.setAlpha(0);
                outerSprite.transform.localPosition = new Vector3(0f, 0.16f, 0f);
                innerSprite.transform.localPosition = new Vector3(0f, 0.16f, 0f);
                MultiTargetTween _outerTween = outerTween;
                MultiTargetTween _innerTween = innerTween;
                MultiTargetTween _scaleTween = scaleTween;
                if (_outerTween != null) _outerTween.Stop();
                if (_innerTween != null) _innerTween.Stop();
                if (_scaleTween != null) _scaleTween.Stop();
                outerTween = Tweens.Add(new TweenConfig
                {
                    targets = new[]
                    {
                        outerSprite
                    },
                    angle = new Il2CppSystem.Nullable<float>(360),
                    duration = 2000f,
                    repeat = -1
                });
                innerTween = Tweens.Add(new TweenConfig
                {
                    targets = new[]
                    {
                        innerSprite
                    },
                    angle = new Il2CppSystem.Nullable<float>(-360),
                    duration = 2300f,
                    repeat = -1
                });
                scaleTween = Tweens.Add(new TweenConfig
                {
                    targets = new[]
                    {
                        outerSprite,
                        innerSprite
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
        }
    
    public override void OnUpdate()
        {
            base.OnUpdate();
            if (_characterType != (CharacterType)20000) return;
            // Zeta
            if (currentSkinType == Zeta.SkinType) ZetaAspectEffects();
            // Hermies
            if (currentSkinType == Hermies.SkinType && hermiesMove == null && !_spriteAnimation.IsPaused)
            {
                moveSpeed = _playerStats.MoveSpeed;
                Action<float> HermiesMovespeedGive = (time) =>
                {
                    if (time < hermiesTrigger) return;
                    hermiesTrigger+= 0.33f;
                    _playerStats.MoveSpeed += 0.01f;
                };
                Action HermiesMovespeedTake = () =>
                {
                    hermiesMove?.Cancel();
                    hermiesTrigger = 0f;
                    _playerStats.MoveSpeed = moveSpeed;
                    hermiesMove = null;
                };
                hermiesMove = Timers.Register(20000, HermiesMovespeedTake, HermiesMovespeedGive);
            }
            // Opal
            if (currentSkinType == Opal.SkinType && !opalFly)
            {
                opalFly = true;
                _gameManager._physicsManager._playersWithWallCollisionGroup.remove(GetComponent<ArcadeSprite>());
            }
            // Tempo
            if (currentSkinType == TempoDiMelma.SkinType) TempoEffects();
            // Vam
            if (currentSkinType == VamAndPyre.SkinType)
            {
                if (!betaVamSub)
                {
                    betaVamSub = true;
                    _signalBus.Subscribe<GameplaySignals.EnemyKilledImmediateSignal>(
                        new Action(MakeEnemyFollower));
                }
            }
            else if (betaVamSub)
            {
                _signalBus.TryUnsubscribe<GameplaySignals.EnemyKilledImmediateSignal>(
                    new Action(MakeEnemyFollower));
                betaVamSub = false;
            }

            if (!changingSkin) return;
            if (Walked != 0f)
            {
                outerSprite.setAlpha(0f);
                innerSprite.setAlpha(0f);
                healthBarFill.fillAmount = 0f;
                HideCharge();
                chargeTime = 0f;
                return;
            }
            ShowCharge();
            chargeTime += PauseSystem.DeltaTimeMillis;
            float fill = chargeTime / maxChargeTime;
            float alpha = 0.25f + 0.75f * fill;
            outerSprite.setAlpha(alpha);
            innerSprite.setAlpha(alpha);
            healthBarFill.fillAmount = fill;
            if(chargeTime < maxChargeTime) return;
            outerSprite.setAlpha(0f);
            innerSprite.setAlpha(0f);
            HideCharge();
            chargeTime = 0f;
            CycleSkin();
        }
    
    public new void HandleLateUpdate()
    {
        base.HandleLateUpdate();
        if (PauseSystem.Paused) return;
        if (currentSkinType == DeCapo.SkinType)
        {
            Il2CppSystem.Collections.Generic.HashSet<PhaserGameObject> children = _gameManager.Enemies.getChildren();
            float2 lhs = transform.position.ToFloat2();
            lhs.x += 0.25f * (float)(flipX ? -1 : 1);
            foreach (PhaserGameObject phaserGameObject in children)
            {
                int random = UnityEngine.Random.Range(0, 7);
                if (children.InternalIndexOf(phaserGameObject) % 17 != random) continue;
                if (phaserGameObject.body != null && phaserGameObject.body._enable)
                {
                    EnemyType enemyType = phaserGameObject.Cast<EnemyController>().EnemyType;
                    if (enemyType != EnemyType.EX_CART_DROWNER && enemyType != EnemyType.BOSS_DROWNER_RASH && enemyType != EnemyType.BOSS_DROWNER_NORMAL && enemyType != EnemyType.BOSS_XLDROWNER && enemyType != EnemyType.TP_BOSS_DEATH && enemyType != EnemyType.TP_BOSS_DEATH_SCYTHEBIG && enemyType != EnemyType.TP_BOSS_DEATHARM)
                    {
                        float2 @float = lhs - phaserGameObject.transform.position.ToFloat2();
                        float num = math.dot(@float, @float);
                        if (num > 0.2f)
                        {
                            float2 rhs = @float / num * PauseSystem.DeltaTime * 5f * 1 * PArea();
                            phaserGameObject.Cast<EnemyController>().position += rhs;
                        }
                    }
                }
            }
        }
    }
    
    public override void OnStop()
    {
        base.OnStop();
        if (hermiesMove != null)
            hermiesMove.Complete();
    }
    
    public override void LevelUp()
    {
        // Hermies Armor gain

        if (currentSkinType == Hermies.SkinType)
        {
            hermiesLevel++;
            if (hermiesLevel % 20 == 0)
                PlayerStats.Armor += 1f;
        }

        if (currentSkinType == Opal.SkinType) OpalEffect();
    }
    
    internal void CriticalHp()
        {
            if (currentSkinType == Zeta.SkinType)
                AddTemporaryBonus(new Action(ZetaCritGive), new Action(ZetaCritRemove), 5000f);
        }
        internal void HideCharge()
        {
            healthBarFill.SetAlpha(0.35f);
            healthBar.SetAlpha(0.35f);
            isCharging = false;
        }
        internal void ShowCharge()
        {
            healthBarFill.SetAlpha(1f);
            healthBar.SetAlpha(1f);
            if(!isCharging) isCharging = true;
        }

        /// <summary>
        /// Zeta Methods
        /// </summary>
        internal void ZetaCritGive()
        {
            _playerStats.Power += 0.5f;
            _playerStats.Speed += 0.5f;
            _playerStats.Duration += 0.5f;
            _playerStats.Area += 0.5f;
            _playerStats.Regen += 7f;
        }
        internal void ZetaCritRemove()
        {
            _playerStats.Power -= 0.5f;
            _playerStats.Speed -= 0.5f;
            _playerStats.Duration -= 0.5f;
            _playerStats.Area -= 0.5f;
            _playerStats.Regen -= 7f;
        }
        internal void ZetaGemEffect()
        {

            Il2CppSystem.Collections.Generic.HashSet<Pickup> pickups = new Il2CppSystem.Collections.Generic.HashSet<Pickup>();
            int i = 0;
            foreach (Pickup a in _gameManager._gems)
            {
                Gem gem = a.Cast<Gem>();
                float bless = PAmount() + Mathf.Log(_gameManager._gems.Count)/UnityEngine.Random.Range(1,7);
                gem.BlessColor(bless, i);
                pickups.Add(gem);
                i++;
            }
            _gameManager._gems = pickups;
        }
        internal void ZetaHeartEffect()
        {
            Vector3 position = transform.position;
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
        internal void ZetaChestEffect()
        {
            //TODO implement
            List<Pickup> pickups = new List<Pickup>();
            List<Pickup> pickupsRemove = new List<Pickup>();
            foreach (Pickup a in PickupManager.PickupItems)
            {
                if(pickups.Contains(a)) continue;
                if (a.PickupType != ItemType.TREASURE) continue;
                TreasureChest treasureC = a.Cast<TreasureChest>();
                Treasure treasure = treasureC._treasure;
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
                PickupManager.PickupItems.Add(treasureC);
                pickupsRemove.Add(a);
            }

            foreach (Pickup a in pickupsRemove)
            {
                PickupManager.PickupItems.Remove(a);
            }
        }
        internal void ZetaAspectEffects()
        {
            int kills = _playerOptions.Config.RunEnemies;

            if (kills < killsToTrigger) return;

            killsToTrigger = (int)(killsToTrigger + (killsToTrigger * 0.2f));

            //Chest to Gold chest
            //Gems - shine or color change
            switch (zetaTriggerEffect)
            {
                case 1:
                    _gameManager.MakeAndActivatePickup(ItemType.CLOVER);
                    Anims.SetAnimation("moon");
                    break;
                case 2:
                    _gameManager.MakeAndActivatePickup(ItemType.ROSARY);
                    Anims.SetAnimation("city");
                    break;
                case 3:
                    ZetaGemEffect();
                    Anims.SetAnimation("stone");
                    break;
                case 4:
                    _gameManager.TriggerGoldFever(20000);
                    Anims.SetAnimation("sun");
                    break;
                case 5:
                    ZetaHeartEffect();
                    Anims.SetAnimation("volcano");
                    break;
                case 6:
                    ZetaChestEffect();
                    Anims.SetAnimation("seawinds");
                    break;
                case 7:
                    _gameManager.MakeAndActivatePickup(ItemType.VACUUM);
                    Anims.SetAnimation("directer");
                    break;
                default:
                    _gameManager.MakeAndActivatePickup(ItemType.CLOVER);
                    _gameManager.MakeAndActivatePickup(ItemType.ROSARY);
                    ZetaGemEffect();
                    _gameManager.TriggerGoldFever(20000);
                    ZetaHeartEffect();
                    ZetaChestEffect();
                    _gameManager.MakeAndActivatePickup(ItemType.VACUUM);
                    zetaTriggerEffect = 0;
                    Anims.SetAnimation("black");
                    break;
            }

            zetaTriggerEffect++;
        }

        /// <summary>
        /// Opal Methods
        /// </summary>
        
        internal void OpalEffect()
        {
            opalLevel++;
            if (opalLevel % 4 == 0) _playerStats.Charm++;
            Vector3 position = transform.position;
            int num = (int)PAmount();
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
                _gameManager.Stage.MakeDestructible(_gameManager.Stage.DestructibleType, pos);
            }
        }

        /// <summary>
        /// Tempo Methods
        /// </summary>
        
        internal void TempoEffects()
        {
            int kills = _playerOptions.Config.RunEnemies;

            if (kills < killsToTrigger) return;

            killsToTrigger = (int)(killsToTrigger + (killsToTrigger * 0.2f));

            int random = UnityEngine.Random.Range(1, 16);

            switch (random)
            {
                case 1:
                    PlayerStats.Amount += 0.5f;
                    break;
                case 2:
                    PlayerStats.Area += 0.5f;
                    break;
                case 3:
                    PlayerStats.Armor += 0.5f;
                    break;
                case 4:
                    PlayerStats.Cooldown -= 0.5f;
                    break;
                case 5:
                    PlayerStats.Duration += 0.5f;
                    break;
                case 6:
                    PlayerStats.Greed += 0.5f;
                    break;
                case 7:
                    PlayerStats.Growth += 0.5f;
                    break;
                case 8:
                    PlayerStats.Luck += 0.5f;
                    break;
                case 9:
                    PlayerStats.Magnet += 0.5f;
                    break;
                case 10:
                    PlayerStats.MaxHp += 5f;
                    break;
                case 11:
                    PlayerStats.MoveSpeed += 0.5f;
                    break;
                case 12:
                    PlayerStats.Power += 0.5f;
                    break;
                case 13:
                    PlayerStats.Regen += 0.5f;
                    break;
                case 14:
                    PlayerStats.Revivals += 0.5f;
                    break;
                case 15:
                    PlayerStats.Speed += 0.5f;
                    break;
            }

        }

        /// <summary>
        /// Beta and Vamp Methods
        /// </summary>
        
        internal void MakeEnemyFollower()
        {
            if (_gameManager.GetLatestKilledEnemyThatCanBeFollower() == null) return;
            if (_gameManager.GetNumAliveEnemyFollowers(this) >= 5) return;
            FollowerEnemy_CharacterController enemyFollower = _gameManager.AddLastEnemyFollower(this);
            if (enemyFollower == null) return;
            if (enemyFollower.HasSetName) return;
            CharacterData enemyData = enemyFollower.CurrentCharacterData;
            enemyData.charName = $"{enemyData.charName} {followerNum}";
            enemyFollower.HasSetName = true;
            followerNum++;
        }
        
        internal void ZetaSkinGetter()
        {
            switch (zetaTriggerEffect)
            {
                case 1:
                    Anims.SetAnimation("moon");
                    break;
                case 2:
                    Anims.SetAnimation("city");
                    break;
                case 3:
                    Anims.SetAnimation("stone");
                    break;
                case 4:
                    Anims.SetAnimation("sun");
                    break;
                case 5:
                    Anims.SetAnimation("volcano");
                    break;
                case 6:
                    Anims.SetAnimation("seawinds");
                    break;
                case 7:
                    Anims.SetAnimation("directer");
                    break;
                default:
                    Anims.SetAnimation("black");
                    break;
            }
        }
        
        internal void CycleSkin()
        {
            if (currentSkinType == nameof(SkinType.DEFAULT))
            {
                Zeta.ZetaStatApply(this);
                currentSkinType = Zeta.SkinType;
                return;
            }
            
            switch (currentSkinType)
            {
                case Zeta.SkinType:
                    Zeta.ZetaStatApply(this, false);
                    Anims.SetAnimation("hermies");
                    currentSkinType = Hermies.SkinType;
                    Hermies.HermiesStatApply(this);
                    break;
                case Hermies.SkinType:
                    Hermies.HermiesStatApply(this, false);
                    Anims.SetAnimation("opal");
                    currentSkinType = Opal.SkinType;
                    Opal.OpalStatApply(this);
                    break;
                case Opal.SkinType:
                    Opal.OpalStatApply(this, false);
                    Anims.SetAnimation("festa");
                    currentSkinType = Festa.SkinType;
                    Festa.FestaStatApply(this);
                    break;
                case Festa.SkinType:
                    Festa.FestaStatApply(this, false);
                    Anims.SetAnimation("beta");
                    currentSkinType = Beta.SkinType;
                    Beta.BetaStatApply(this);
                    break;
                case Beta.SkinType:
                    Beta.BetaStatApply(this, false);
                    Anims.SetAnimation("tempo");
                    currentSkinType = TempoDiMelma.SkinType;
                    TempoDiMelma.TempoStatApply(this);
                    break;
                case TempoDiMelma.SkinType:
                    TempoDiMelma.TempoStatApply(this, false);
                    Anims.SetAnimation("vam");
                    currentSkinType = VamAndPyre.SkinType;
                    VamAndPyre.VamStatApply(this);
                    break;
                case VamAndPyre.SkinType:
                    VamAndPyre.VamStatApply(this, false);
                    Anims.SetAnimation("decapo");
                    currentSkinType = DeCapo.SkinType;
                    DeCapo.DeCapoStatApply(this);
                    break;
                case DeCapo.SkinType:
                    DeCapo.DeCapoStatApply(this, false);
                    ZetaSkinGetter();
                    currentSkinType = Zeta.SkinType;
                    Zeta.ZetaStatApply(this);
                    break;
            }

        }
}

internal static class CharacterPrefabGenerator
{
    static Il2CppSystem.Collections.Generic.List<GameObject> objects = new();
    static Il2CppSystem.Collections.Generic.List<CharacterController> controllers = new();
    static GameObject gameObject = new GameObject("Character_Community");

    static void CharacterRenderer()
    {
        GameObject characterRenderer = new GameObject("CharacterRenderer");
        GameObject Canvas = new GameObject("Canvas - HealthBar");
        GameObject HealthBar = new GameObject("HealthBar");
        GameObject HealthBarFill = new GameObject("HealthBarFill");
        GameObject SpriteOutliner = new GameObject("SpriteOutliner");
        characterRenderer.transform.SetParent(gameObject.transform);
        Canvas.transform.SetParent(characterRenderer.transform);
        HealthBar.transform.SetParent(Canvas.transform);
        HealthBarFill.transform.SetParent(HealthBar.transform);
        SpriteOutliner.transform.SetParent(characterRenderer.transform);
        
        characterRenderer.transform.localScale = new Vector3(2, 0, 1);
        characterRenderer.AddComponent<SpriteRenderer>();
        characterRenderer.AddComponent<SpriteTrail>();
        characterRenderer.AddComponent<SpriteAnimation>();

        Canvas.AddComponent<RectTransform>();
        Canvas.transform.localScale = Vector3.zero;
        Canvas.AddComponent<Canvas>();
        Canvas.AddComponent<CanvasScaler>();
        Canvas.AddComponent<GraphicRaycaster>();
        Canvas.AddComponent<HealthBar>();

        HealthBar.AddComponent<RectTransform>();
        HealthBar.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        HealthBar.AddComponent<CanvasRenderer>();
        HealthBar.AddComponent<Image>();
        
        HealthBarFill.AddComponent<RectTransform>();
        HealthBarFill.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        HealthBarFill.AddComponent<CanvasRenderer>();
        HealthBarFill.AddComponent<Image>();
        
        SpriteOutliner.transform.localScale = new Vector3(0.35f, 0.35f, 1);
        SpriteOutliner.AddComponent<MeshFilter>();
        SpriteOutliner.AddComponent<MeshRenderer>();
        SpriteOutliner.AddComponent<SpriteOutlinerControl>();
    }

    static void NoHurtRenderer()
    {
        GameObject NoHurt = new GameObject("NoHurtRenderer");
        NoHurt.transform.SetParent(gameObject.transform);
        NoHurt.transform.localScale = new Vector3(1, 1, 1);
        
        NoHurt.AddComponent<SpriteRenderer>();
    
    
    static void WickedSeason()
    {
        GameObject Wicked = new GameObject("WickedSeason");
        GameObject Fan = new GameObject("SeasonFan");
        GameObject Parent = new GameObject("SeasonIconParent");
        GameObject Icon = new GameObject("SeasonIcon");
        NoHurt.transform.SetParent(gameObject.transform);
        NoHurt.transform.localScale = new Vector3(1, 1, 1);
        
        NoHurt.AddComponent<SpriteRenderer>();
    }
    static void CharacterPrefabGeneratorSet()
    {
        
        
        GameObject Light2D = new GameObject("Light 2D");
        gameObject.tag = "Player";
        gameObject.transform.localScale = Vector3.one;
        gameObject.AddComponent<CharacterWeaponsManager>();
        gameObject.AddComponent<CharacterAccessoriesManager>();
        gameObject.AddComponent<CharacterControllerCommunity>();
    }
    internal static GameObject GetCharacterPrefab()
    {
        CharacterPrefabGeneratorSet();
        objects.Add(gameObject);
        controllers.Add(gameObject.GetComponent<CharacterControllerCommunity>());
        return gameObject;
    }
}