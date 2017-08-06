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

namespace Garentec
{
    class Spells
    {
        public static Obj_AI_Hero _player;
        public static Aimtec.SDK.Spell _R;
        public static bool hasPerformedAction;

        public static void Initialize()
        {
            _R = new Aimtec.SDK.Spell(SpellSlot.R, 400);
            
            hasPerformedAction = false;
        }

        public static float RDamage(Obj_AI_Base target)
        {
            if (target.HasBuff("garenpassiveenemytarget"))
            {
                return (float)_player.CalculateDamage(target, DamageType.True, Damage.GetSpellDamage(_player, target, SpellSlot.R));
            }

            return (float)_player.CalculateDamage(target, DamageType.Magical, Damage.GetSpellDamage(_player, target, SpellSlot.R));
        }

        public static void CastBasicAttack(List<Obj_AI_Base> list, bool IsKillSteal = false)
        {
            if (!hasPerformedAction && list.Count() > 0)
            {
                Obj_AI_Base target = list
                    .Where(a => a != null && a.IsLegitimate() && a.IsInRange(_player.AttackRange + _player.BoundingRadius)
                        && (!IsKillSteal || a.Health <= _player.GetAutoAttackDamage(a)))
                    .OrderBy(a => a.Health)
                    .FirstOrDefault();

                if (target != null)
                {
                    hasPerformedAction = _player.IssueOrder(OrderType.AttackUnit, target);
                }
            }
        }

        public static void CastR(List<Obj_AI_Base> list, bool IsKillSteal = false)
        {
            if (_R != null && _R.Ready && !hasPerformedAction && list.Count() > 0)
            {
                Obj_AI_Base target = list
                    .Where(a => a != null && a.IsLegitimate() && a.IsInRange(_R.Range)
                        && (!IsKillSteal || a.Health <= RDamage(a)))
                    .OrderBy(a => a.HasBuff("garenpassiveenemytarget"))
                    .ThenBy(a => a.Health)
                    .FirstOrDefault();

                if (target != null)
                {
                    hasPerformedAction = _R.Cast(target);
                }
            }
        }
    }
}
