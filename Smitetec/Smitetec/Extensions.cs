﻿using System;
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
    static class Extensions
    {
        /// <summary>
        /// Returns true if the specified unit possess the recalling buff.
        /// </summary>
        public static bool IsRecalling(this Obj_AI_Base source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(source.Name);
            }

            return source.BuffManager.HasBuff("recall");
        }

        /// <summary>
        /// Returns true if the specified unit has a Monster name.
        /// </summary>
        public static bool IsMonster(this Obj_AI_Base source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(source.Name);
            }

            return MonsterNameList.Contains(source.Name);
        }

        /// <summary>
        /// Returns true if the specified unit has a Buff Monster name.
        /// </summary>
        public static bool IsBuffMonster(this Obj_AI_Base source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(source.Name);
            }

            return BuffMonsterNameList.Contains(source.UnitSkinName);
        }

        #region Lists
        public static List<string> MinionNameList = new List<string>()
        {
            "SRU_OrderMinionMelee", "SRU_OrderMinionRanged", "SRU_OrderMinionSiege", "SRU_OrderMinionSuper",
            "SRU_ChaosMinionMelee", "SRU_ChaosMinionRanged", "SRU_ChaosMinionSiege", "SRU_ChaosMinionSuper",
            "HA_OrderMinionMelee", "HA_OrderMinionRanged", "HA_OrderMinionSiege", "HA_OrderMinionSuper",
            "HA_ChaosMinionMelee", "HA_ChaosMinionRanged", "HA_ChaosMinionSiege", "HA_ChaosMinionSuper",
        };

        public static List<string> BigMinionNameList = new List<string>()
        {
            "SRU_OrderMinionSiege", "SRU_OrderMinionSuper",
            "SRU_ChaosMinionSiege", "SRU_ChaosMinionSuper",
            "HA_OrderMinionSiege", "HA_OrderMinionSuper",
            "HA_ChaosMinionSiege", "HA_ChaosMinionSuper",
        };

        public static List<string> MonsterNameList = new List<string>()
        {
            "SRU_BlueMini", "BlueMini2", "SRU_RedMini", "SRU_KrugMini",
            "SRU_MurkwolfMini", "SRU_RazorbeakMini", "TT_NWraith2",
            "TT_NGolem2", "TT_NWolf2",
            "SRU_Blue", "SRU_Red", "SRU_Murkwolf", "SRU_Razorbeak",
            "SRU_Krug", "SRU_Gromp", "Sru_Crab",
            "SRU_Dragon_Air", "SRU_Dragon_Fire", "SRU_Dragon_Water", "SRU_Dragon_Earth",
            "SRU_Dragon_Elder", "SRU_RiftHerald", "SRU_Baron",
            "TT_NWraith", "TT_NGolem", "TT_NWolf", "TT_Spiderboss",
        };

        public static List<string> BigMonsterNameList = new List<string>()
        {
            "SRU_Blue", "SRU_Red", "SRU_Murkwolf", "SRU_Razorbeak",
            "SRU_Krug", "SRU_Gromp", "Sru_Crab",
            "SRU_Dragon_Air", "SRU_Dragon_Fire", "SRU_Dragon_Water", "SRU_Dragon_Earth",
            "SRU_Dragon_Elder", "SRU_RiftHerald", "SRU_Baron",
            "TT_NWraith", "TT_NGolem", "TT_NWolf", "TT_Spiderboss",
        };

        public static List<string> BuffMonsterNameList = new List<string>()
        {
            "SRU_Blue", "SRU_Red",
            "SRU_Dragon_Air", "SRU_Dragon_Fire", "SRU_Dragon_Water", "SRU_Dragon_Earth",
            "SRU_Dragon_Elder", "SRU_RiftHerald",
            "SRU_Baron", "TT_Spiderboss",
        };

        public static List<string> EpicMonsterNameList = new List<string>()
        {
            "SRU_Dragon_Air", "SRU_Dragon_Fire", "SRU_Dragon_Water", "SRU_Dragon_Earth",
            "SRU_Dragon_Elder", "SRU_RiftHerald",
            "SRU_Baron", "TT_Spiderboss",
        };

        public static List<string> DragonMonsterNameList = new List<string>()
        {
            "SRU_Dragon_Air", "SRU_Dragon_Fire", "SRU_Dragon_Water", "SRU_Dragon_Earth",
            "SRU_Dragon_Elder",
        };
        #endregion
    }
}