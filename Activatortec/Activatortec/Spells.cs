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
    class Spells
    {
        public static Obj_AI_Hero _player;
        public static Aimtec.SDK.Spell _Smite;
        public static bool hasPerformedAction;

        public static void Initialize()
        {
            SpellSlot smiteSlot = _player.SpellBook.Spells
                .FirstOrDefault(s => s != null && s.SpellData.Name.ToLower().Contains("smite")).Slot;

            _Smite = new Aimtec.SDK.Spell(smiteSlot, 500);
            hasPerformedAction = false;
        }

        public static float SmiteDamage(Obj_AI_Base target)
        {
            float damage = new float[]
            {
                390, 410, 430, 450, 480, 510, 540, 570, 600, 640, 680, 720, 760, 800, 850, 900, 950, 1000
            }[_player.Level - 1];

            return (float)_player.CalculateDamage(target, DamageType.True, damage);
        }

        public static float SmiteHeal(Obj_AI_Base target)
        {
            float amount = 0;

            if (Extensions.BigMonsterNameList.Contains(target.Name))
            {
                amount += 70 + (0.1f * _player.MaxHealth);
            }

            return amount;
        }

        public static float ChallengingDamage(Obj_AI_Base target)
        {
            float damage = 54 + (6 * _player.Level);

            return (float)_player.CalculateDamage(target, DamageType.True, damage);
        }

        public static float ChillingDamage(Obj_AI_Base target)
        {
            float damage = 20 + (8 * _player.Level);

            return (float)_player.CalculateDamage(target, DamageType.True, damage);
        }

        public static void CastSmite(List<Obj_AI_Base> list, bool IsKillSteal = false)
        {
            if (_Smite != null && _Smite.Ready)
            {
                if (list.Count > 0)
                {
                    list.OrderBy(a => a.Health);
                }

                Obj_AI_Base target = list
                    .Where(a => a.IsValidTarget() && a.IsInRange(_Smite.Range)
                        && (!IsKillSteal || a.Health <= SmiteDamage(a)))
                    .FirstOrDefault();
                
                if (target != null)
                {
                    hasPerformedAction = _Smite.Cast(target);
                }
            }
        }

        public static void CastBlueSmite(List<Obj_AI_Base> list, bool IsKillSteal = false)
        {
            if (_Smite != null && _Smite.Ready)
            {
                if (list.Count > 0)
                {
                    list.OrderBy(a => a.Health);
                }

                Obj_AI_Base target = list
                    .Where(a => a.IsValidTarget() && a.IsInRange(_Smite.Range)
                        && (!IsKillSteal || a.Health <= ChillingDamage(a)))
                    .FirstOrDefault();

                if (target != null)
                {
                    hasPerformedAction = _Smite.Cast(target);
                }
            }
        }

        public static void CastRedSmite(List<Obj_AI_Base> list, bool IsKillSteal = false)
        {
            if (_Smite != null && _Smite.Ready)
            {
                if (list.Count > 0)
                {
                    list.OrderBy(a => a.Health);
                }

                Obj_AI_Base target = list
                    .Where(a => a.IsValidTarget() && a.IsInRange(_Smite.Range)
                        && !a.BuffManager.HasBuff("itemsmitechallenge")
                        && (!IsKillSteal || a.Health <= ChallengingDamage(a)))
                    .FirstOrDefault();

                if (target != null)
                {
                    hasPerformedAction = _Smite.Cast(target);
                }
            }
        }

        public static void CastRedSmiteWithBasicAttack(List<Obj_AI_Base> list, bool IsKillSteal = false)
        {
            if (_Smite != null && _Smite.Ready)
            {
                if (list.Count > 0)
                {
                    list.OrderBy(a => a.Health);
                }

                Obj_AI_Base target = list
                    .Where(a => a.IsValidTarget() && a.IsInRange(_Smite.Range)
                        && !a.BuffManager.HasBuff("itemsmitechallenge")
                        && (!IsKillSteal || a.Health <= ChallengingDamage(a)))
                    .FirstOrDefault();

                if (target != null)
                {
                    _Smite.Cast(target);

                    if (target.HasBuff("itemsmitechallenge"))
                    {
                        hasPerformedAction = _player.IssueOrder(OrderType.AttackUnit, target);
                    }
                }
            }
        }
    }
}
