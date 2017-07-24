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

namespace Smitetec
{
    class Modes
    {
        public static Obj_AI_Hero _player;

        public static void Combo()
        {
            List<Obj_AI_Base> targets = ObjectManager.Get<Obj_AI_Base>().Where(a => a.IsEnemy && a.IsHero && !a.IsDead).ToList();

            if (Menus._menu["combo"]["comboblue"].Enabled)
            {
                Spells.CastBlueSmite(targets);
            }

            if (Menus._menu["combo"]["combored"].Enabled)
            {
                Spells.CastRedSmite(targets);
            }
        }

        public static void Smite()
        {
            List<Obj_AI_Base> targets = new List<Obj_AI_Base>();

            if (Game.MapId == GameMapId.SummonersRift)
            {
                if (Menus._menu["smite"]["red"].Enabled)
                {
                    targets.Add(ObjectManager.Get<Obj_AI_Base>()
                        .Where(a => a.UnitSkinName == "SRU_Red" && !a.IsDead).FirstOrDefault());
                    Spells.CastSmite(targets, true);
                }
                if (Menus._menu["smite"]["blue"].Enabled)
                {
                    targets.Add(ObjectManager.Get<Obj_AI_Base>()
                        .Where(a => a.UnitSkinName == "SRU_Blue" && !a.IsDead).FirstOrDefault());
                    Spells.CastSmite(targets, true);
                }
                if (Menus._menu["smite"]["gromp"].Enabled)
                {
                    targets.Add(ObjectManager.Get<Obj_AI_Base>()
                        .Where(a => a.UnitSkinName == "SRU_Gromp" && !a.IsDead).FirstOrDefault());
                    Spells.CastSmite(targets, true);
                }
                if (Menus._menu["smite"]["krug"].Enabled)
                {
                    targets.Add(ObjectManager.Get<Obj_AI_Base>()
                        .Where(a => a.UnitSkinName == "SRU_Krug" && !a.IsDead).FirstOrDefault());
                    Spells.CastSmite(targets, true);
                }
                if (Menus._menu["smite"]["raptor"].Enabled)
                {
                    targets.Add(ObjectManager.Get<Obj_AI_Base>()
                        .Where(a => a.UnitSkinName == "SRU_Razorbeak" && !a.IsDead).FirstOrDefault());
                    Spells.CastSmite(targets, true);
                }
                if (Menus._menu["smite"]["wolf"].Enabled)
                {
                    targets.Add(ObjectManager.Get<Obj_AI_Base>()
                        .Where(a => a.UnitSkinName == "SRU_Murkwolf" && !a.IsDead).FirstOrDefault());
                    Spells.CastSmite(targets, true);
                }
                if (Menus._menu["smite"]["crab"].Enabled)
                {
                    targets.Add(ObjectManager.Get<Obj_AI_Base>()
                        .Where(a => a.UnitSkinName == "Sru_Crab" && !a.IsDead).FirstOrDefault());
                    Spells.CastSmite(targets, true);
                }
                if (Menus._menu["smite"]["herald"].Enabled)
                {
                    targets.Add(ObjectManager.Get<Obj_AI_Base>()
                        .Where(a => a.UnitSkinName == "SRU_RiftHerald" && !a.IsDead).FirstOrDefault());
                    Spells.CastSmite(targets, true);
                }
                if (Menus._menu["smite"]["drake"].Enabled)
                {
                    targets.Add(ObjectManager.Get<Obj_AI_Base>()
                        .Where(a => Extensions.DragonMonsterNameList.Contains(a.UnitSkinName) && !a.IsDead).FirstOrDefault());
                    Spells.CastSmite(targets, true);
                }
                if (Menus._menu["smite"]["baron"].Enabled)
                {
                    targets.Add(ObjectManager.Get<Obj_AI_Base>()
                        .Where(a => a.UnitSkinName == "SRU_Baron" && !a.IsDead).FirstOrDefault());
                    Spells.CastSmite(targets, true);
                }
            }
            if (Game.MapId == GameMapId.TwistedTreeline)
            {
                if (Menus._menu["smite"]["wraith"].Enabled)
                {
                    targets.Add(ObjectManager.Get<Obj_AI_Base>()
                        .Where(a => a.UnitSkinName == "TT_NWraith" && !a.IsDead).FirstOrDefault());
                    Spells.CastSmite(targets, true);
                }
                if (Menus._menu["smite"]["golem"].Enabled)
                {
                    targets.Add(ObjectManager.Get<Obj_AI_Base>()
                        .Where(a => a.UnitSkinName == "TT_NGolem" && !a.IsDead).FirstOrDefault());
                    Spells.CastSmite(targets, true);
                }
                if (Menus._menu["smite"]["wolf"].Enabled)
                {
                    targets.Add(ObjectManager.Get<Obj_AI_Base>()
                        .Where(a => a.UnitSkinName == "TT_NWolf" && !a.IsDead).FirstOrDefault());
                    Spells.CastSmite(targets, true);
                }
                if (Menus._menu["smite"]["spiderboss"].Enabled)
                {
                    targets.Add(ObjectManager.Get<Obj_AI_Base>()
                        .Where(a => a.UnitSkinName == "TT_Spiderboss" && !a.IsDead).FirstOrDefault());
                    Spells.CastSmite(targets, true);
                }
            }
        }

        public static void Killsteal()
        {
            List<Obj_AI_Base> targets = ObjectManager.Get<Obj_AI_Base>()
                .Where(a => a.IsEnemy && a.IsHero && !a.IsDead).ToList();
            
            if (Menus._menu["killsteal"]["ksblue"].Enabled)
            {
                Spells.CastBlueSmite(targets, true);
            }

            if (Menus._menu["killsteal"]["ksred"].Enabled)
            {
                Spells.CastRedSmiteWithBasicAttack(targets, true);
            }
        }

        public static void Healing()
        {
            List<Obj_AI_Base> targets = ObjectManager.Get<Obj_AI_Base>()
                .Where(a => a.IsEnemy && Extensions.BigMonsterNameList.Contains(a.UnitSkinName) && !a.IsDead).ToList();

            if (_player.HealthPercent() <= Menus._menu["settings"]["smiteheal"].Value)
            {
                Spells.CastSmite(targets);
            }
        }
    }
}
