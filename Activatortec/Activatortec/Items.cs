using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aimtec;
using Aimtec.SDK;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Menu.Config;
using Aimtec.SDK.Menu.Theme;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Prediction;
using Aimtec.SDK.Prediction.Collision;
using Aimtec.SDK.Prediction.Health;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.Cache;
using Aimtec.SDK.Util.ThirdParty;

namespace Activatortec
{
    class Items
    {
        public static Obj_AI_Hero _player;
        static Obj_AI_Hero _target;

        static float _totalDamage;

        static bool hasPerformedAction;
        static bool useCutlass;
        static bool useBotrk;
        static bool useGunblade;
        static bool useTiamat;
        static bool useRavenous;

        public static void Update()
        {
            _target = null;
            
            _totalDamage = 0;

            hasPerformedAction = false;
            useCutlass = false;
            useBotrk = false;
            useGunblade = false;
            useTiamat = false;
            useRavenous = false;

            Offensive();
            Utility();
            Consumables();
        }

        #region Offensive
        static void Offensive()
        {
            TotalActiveDamage();

            BilgewaterCutlass(_target);
            BladeoftheRuinedKing(_target);
            HextechGunblade(_target);
            Tiamat(_target);
            RavenousHydra(_target);
        }

        static void TotalActiveDamage()
        {
            bool hasCutlass = Menus._menu["offensive"]["cutlass"].Enabled && _player.HasAndCanUseItem(ItemId.BilgewaterCutlass);
            bool hasBotrk = Menus._menu["offensive"]["botrk"].Enabled && _player.HasAndCanUseItem(ItemId.BladeoftheRuinedKing);
            bool hasGunblade = Menus._menu["offensive"]["gunblade"].Enabled && _player.HasAndCanUseItem(ItemId.HextechGunblade);
            bool hasTiamat = Menus._menu["offensive"]["tiamat"].Enabled && _player.HasAndCanUseItem(ItemId.Tiamat);
            bool hasRavenous = Menus._menu["offensive"]["ravenous"].Enabled && _player.HasAndCanUseItem(ItemId.RavenousHydra);

            if (hasCutlass || hasBotrk || hasGunblade || hasTiamat || hasRavenous)
            {
                _target = GameObjects.EnemyHeroes
                    .OrderBy(a => a.Health)
                    .Where(a => a.IsLegitimate())
                    .FirstOrDefault();
            }

            if (_target != null)
            {
                if (hasTiamat || hasRavenous)
                {
                    if (_target.IsInRange(250))
                    {
                        _totalDamage += (float)_player.CalculateDamage(_target, DamageType.Physical, (_player.TotalAttackDamage));
                        
                        if (hasTiamat)
                        {
                            useTiamat = true;
                        }
                        else if (hasRavenous)
                        {
                            useRavenous = true;
                        }
                    }
                    else if (_target.IsInRange(400))
                    {
                        _totalDamage += (float)_player.CalculateDamage(_target, DamageType.Physical, (_player.TotalAttackDamage * 0.6));

                        if (hasTiamat)
                        {
                            useTiamat = true;
                        }
                        else if (hasRavenous)
                        {
                            useRavenous = true;
                        }
                    }
                }

                if (_target.IsInRange(550))
                {
                    if (hasCutlass)
                    {
                        _totalDamage += (float) _player.CalculateDamage(_target, DamageType.Magical, 100);
                        useCutlass = true;
                    }
                    
                    if (hasBotrk)
                    {
                        _totalDamage += (float) _player.CalculateDamage(_target, DamageType.Magical, 100);
                        useBotrk = true;
                    }
                }

                if (_target.IsInRange(700) && hasGunblade)
                {
                    float damage = new float[]{ 175, 179, 183, 187, 191, 195, 199, 203, 207, 211, 215, 220, 225, 230, 235, 240, 245, 250 }[_player.Level - 1];
                    
                    _totalDamage += (float) _player.CalculateDamage(_target, DamageType.Magical, damage + (_player.TotalAbilityDamage * 0.3));
                    useGunblade = true;
                }
            }
        }

        static void BilgewaterCutlass(Obj_AI_Base target = null)
        {
            if (target != null && target.Health <= _totalDamage && useCutlass && !hasPerformedAction)
            {
                hasPerformedAction = _player.UseItem(ItemId.BilgewaterCutlass, target);
            }
        }

        static void BladeoftheRuinedKing(Obj_AI_Base target = null)
        {
            if (target != null && target.Health <= _totalDamage && useBotrk && !hasPerformedAction)
            {
                hasPerformedAction = _player.UseItem(ItemId.BladeoftheRuinedKing, target);
            }
        }

        static void HextechGunblade(Obj_AI_Base target = null)
        {
            if (target != null && target.Health <= _totalDamage && useGunblade && !hasPerformedAction)
            {
                hasPerformedAction = _player.UseItem(ItemId.HextechGunblade, target);
            }
        }

        static void Tiamat(Obj_AI_Base target = null)
        {
            if (target != null && target.Health <= _totalDamage && useTiamat && !hasPerformedAction)
            {
                hasPerformedAction = _player.UseItem(ItemId.Tiamat);
            }
        }

        static void RavenousHydra(Obj_AI_Base target = null)
        {
            if (target != null && target.Health <= _totalDamage && useRavenous && !hasPerformedAction)
            {
                hasPerformedAction = _player.UseItem(ItemId.RavenousHydra);
            }
        }
        #endregion

        #region Utility
        static void Utility()
        {
            BannerOfCommand();
            KnightsVow();
            ZekesConvergence();
        }

        static void BannerOfCommand()
        {
            if (Menus._menu["defensive"]["banner"].Enabled
                && _player.HasAndCanUseItem(ItemId.BannerofCommand))
            {
                Obj_AI_Base target = GameObjects.AllyMinions
                    .OrderBy(a => a.Health)
                    .Where(a => a.IsLegitimate() && a.IsInRange(1200)
                    && a.HealthPercent() >= 75 && !a.HasBuff("ItemPromote")
                    && a.UnitSkinName.Contains(Menus.minionlist[Menus._menu["defensive"]["bannerunit"].Value]))
                    .FirstOrDefault();
                
                if (target != null)
                {
                    _player.UseItem(ItemId.BannerofCommand, target);
                }
            }
        }

        static void KnightsVow()
        {
            if (GameObjects.AllyHeroes.Where(a => !a.IsMe).Count() > 0
                && Menus._menu["defensive"]["vow"].Enabled
                && _player.HasAndCanUseItem(ItemId.KnightsVow))
            {
                Obj_AI_Base target = GameObjects.AllyHeroes
                    .OrderBy(a => a.TotalAttackDamage)
                    .Where(a => !a.IsMe && a.IsLegitimate() && a.IsInRange(1000)
                    && Menus._menu["defensive"]["partner"][a.Name].Enabled
                    && !_player.HasBuff("itemknightsvowknight") && !a.HasBuff("itemknightsvowliege"))
                    .FirstOrDefault();
                
                if (target != null)
                {
                    _player.UseItem(ItemId.KnightsVow, target);
                }
            }
        }

        static void ZekesConvergence()
        {
            if (GameObjects.AllyHeroes.Where(a => !a.IsMe).Count() > 0
                && Menus._menu["defensive"]["zeke"].Enabled
                && _player.HasAndCanUseItem(ItemId.ZekesHarbinger))
            {
                Obj_AI_Base target = GameObjects.AllyHeroes
                    .OrderBy(a => a.TotalAttackDamage)
                    .Where(a => !a.IsMe && a.IsLegitimate() && a.IsInRange(1000)
                    && Menus._menu["defensive"]["bind"][a.Name].Enabled
                    && !_player.HasBuff("itemzekesconduitdisplayself") && !a.HasBuff("itemzekesconduitdisplayally"))
                    .FirstOrDefault();
                
                if (target != null)
                {
                    _player.UseItem(ItemId.ZekesHarbinger, target);
                }
            }
        }
        #endregion

        #region Consumables
        static void Consumables()
        {
            Potions();
            Elixirs();
            Additionals();
        }

        static void Potions()
        {
            if (Menus._menu["consumable"]["usepot"].Enabled)
            {
                bool hasBuff = false;

                foreach (string buff in Extensions.PotionBuffs)
                {
                    if (_player.HasBuff(buff))
                    {
                        if (!hasBuff)
                        {
                            hasBuff = true;
                        }
                    }
                }
                
                foreach (uint item in Extensions.PotionItems)
                {
                    if (_player.HasAndCanUseItem(item) && !hasBuff
                        && _player.HealthPercent() <= Menus._menu["consumable"]["potpercent"].Value)
                    {
                        _player.UseItem(item);
                    }
                }
            }
        }

        static void Elixirs()
        {
            if (Menus._menu["consumable"]["useelixir"].Enabled)
            {
                if (_player.HasAndCanUseItem(ItemId.ElixirofIron) && !_player.HasBuff("ElixirOfIron"))
                {
                    _player.UseItem(ItemId.ElixirofIron);
                }

                if (_player.HasAndCanUseItem(ItemId.ElixirofSorcery) && !_player.HasBuff("ElixirofSorcery")
                    && (_player.CountEnemyHeroesInRange(1200) > 0 || _player.CountEnemyTurretsInRange(1200) > 0))
                {
                    _player.UseItem(ItemId.ElixirofSorcery);
                }

                if (_player.HasAndCanUseItem(ItemId.ElixirofWrath) && !_player.HasBuff("ElixirofWrath"))
                {
                    _player.UseItem(ItemId.ElixirofWrath);
                }
            }
        }

        static void Additionals()
        {
            if (Game.MapId == GameMapId.HowlingAbyss)
            {
                if (Menus._menu["consumable"]["useoracle"].Enabled
                    && _player.HasAndCanUseItem(ItemId.OraclesExtract)
                    && !_player.HasBuff("OracleExtractSight"))
                {
                    _player.UseItem(ItemId.OraclesExtract);
                }

                if (Menus._menu["consumable"]["usesnax"].Enabled)
                {
                    if (_player.HasAndCanUseItem(ItemId.PoroSnax))
                    {
                        _player.UseItem(ItemId.PoroSnax);
                    }

                    if (_player.HasAndCanUseItem(ItemId.DietPoroSnax))
                    {
                        _player.UseItem(ItemId.DietPoroSnax);
                    }
                }
            }
        }
        #endregion
    }
}
