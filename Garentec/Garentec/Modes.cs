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
    class Modes
    {
        public static Obj_AI_Hero _player;

        public static void Combo()
        {
            List<Obj_AI_Base> targets = ObjectManager.Get<Obj_AI_Base>().Where(a => a.IsEnemy && a.IsHero && !a.IsDead).ToList();
        }

        public static void Clear()
        {
            List<Obj_AI_Base> targets = ObjectManager.Get<Obj_AI_Base>().Where(a => a.IsEnemy && a.IsMinion && !a.IsDead).ToList();
        }

        public static void Lasthit()
        {
            List<Obj_AI_Base> targets = ObjectManager.Get<Obj_AI_Base>().Where(a => a.IsEnemy && a.IsMinion && !a.IsDead).ToList();
        }

        public static void Killsteal()
        {
            List<Obj_AI_Base> targets = ObjectManager.Get<Obj_AI_Base>().Where(a => a.IsEnemy && a.IsHero && !a.IsDead).ToList();
            
            if (Menus._menu["ksmenu"]["ksAA"].Enabled)
            {
                Spells.CastBasicAttack(targets, true);
            }

            if (Menus._menu["ksmenu"]["ksR"].Enabled)
            {
                Spells.CastR(targets, true);
            }
        }
    }
}
