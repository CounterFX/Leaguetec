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

namespace Potiontec
{
    static class Extensions
    {
        #region Booleans
        /// <summary>
        /// Returns true if the specified unit has and can use a specified item.
        /// </summary>
        public static bool HasAndCanUseItem(this Obj_AI_Base source, uint item)
        {
            return source.HasItem(item) && source.CanUseItem(item);
        }

        /// <summary>
        /// Returns true if the specified unit is a valid targetable target and not invulernable or dead.
        /// </summary>
        public static bool IsLegitimate(this Obj_AI_Base source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return !source.IsDead && !source.IsInvulnerable && source.IsValid && source.IsTargetable;
        }

        /// <summary>
        /// Returns true if the specified unit possess the recalling buff.
        /// </summary>
        public static bool IsRecalling(this Obj_AI_Base source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
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
                throw new ArgumentNullException("source");
            }

            return MonsterNameList.Contains("source");
        }

        /// <summary>
        /// Returns true if the specified unit has a Buff Monster name.
        /// </summary>
        public static bool IsBuffMonster(this Obj_AI_Base source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return BuffMonsterNameList.Contains(source.UnitSkinName);
        }
        #endregion

        #region Counts
        /// <summary>
        /// Returns the number of specified units within a given range.
        /// </summary>
        public static int CountUnitsInRange(this Obj_AI_Base source, List<Obj_AI_Base> units, float range)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (units.Count < 0)
            {
                throw new ArgumentNullException("units");
            }

            return units.Count(a => a.IsValidTarget() && a.IsInRange(range));
        }

        /// <summary>
        /// Returns the number of enemy turrets within a given range.
        /// </summary>
        public static int CountEnemyTurretsInRange(this Obj_AI_Base source, float range)
        {
            List<Obj_AI_Base> list = ObjectManager.Get<Obj_AI_Base>()
                .Where(a => !a.IsDead && a.IsEnemy && a.Type == GameObjectType.obj_AI_Turret).ToList();

            return source.CountUnitsInRange(list, range);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the SpellSlot of an Item for a specified unit.
        /// </summary>
        public static SpellSlot GetItemSlot(this Obj_AI_Base source, uint item)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (item == default(uint))
            {
                throw new ArgumentNullException("item");
            }

            return source.Inventory.Slots
                .Where(a => a.ItemId == item).Select(a => a.SpellSlot).FirstOrDefault();
        }

        /// <summary>
        /// Returns true if the Item can be used.
        /// </summary>
        public static bool CanUseItem(this Obj_AI_Base source, uint item)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (item == default(uint))
            {
                throw new ArgumentNullException("item");
            }

            return source.SpellBook.GetSpellState(source.GetItemSlot(item)) == SpellState.Ready;
        }

        /// <summary>
        /// Casts the item.
        /// </summary>
        public static bool UseItem(this Obj_AI_Base source, uint item)
        {
            if (item == default(uint))
            {
                throw new ArgumentNullException("item");
            }

            return source.SpellBook.CastSpell(source.GetItemSlot(item));
        }

        /// <summary>
        /// Casts the item on a specified target.
        /// </summary>
        public static bool UseItem(this Obj_AI_Base source, uint item, Obj_AI_Base target)
        {
            if (item == default(uint))
            {
                throw new ArgumentNullException("item");
            }

            return source.SpellBook.CastSpell(source.GetItemSlot(item), target);
        }

        /// <summary>
        /// Returns a MenuComponent to create a boolean switch in the Menu object.
        /// </summary>
        public static MenuComponent AddBoolean(this Menu menu, string internalName, string displayName, bool enabled = true)
        {
            if (menu == null)
            {
                throw new ArgumentNullException("menu");
            }

            return menu.Add(new MenuBool(internalName, displayName, enabled));
        }

        /// <summary>
        /// Returns a MenuComponent to create a List in the Menu object.
        /// </summary>
        public static MenuComponent AddKeyBind(this Menu menu, string internalName, string displayName,KeyCode key, KeybindType keybindType, bool active = false)
        {
            if (menu == null)
            {
                throw new ArgumentNullException("menu");
            }

            return menu.Add(new MenuKeyBind(internalName, displayName, key, keybindType, active));
        }

        /// <summary>
        /// Returns a MenuComponent to create a List in the Menu object.
        /// </summary>
        public static MenuComponent AddList(this Menu menu, string internalName, string displayName, string[] items, int selectedValue)
        {
            if (menu == null)
            {
                throw new ArgumentNullException("menu");
            }

            return menu.Add(new MenuList(internalName, displayName, items, selectedValue));
        }

        /// <summary>
        /// Returns a MenuComponent to display a seperator in the Menu object.
        /// </summary>
        public static MenuComponent AddSeperator(this Menu menu, string internalName)
        {
            if (menu == null)
            {
                throw new ArgumentNullException("menu");
            }

            return menu.Add(new MenuSeperator(internalName.ToLower(), internalName));
        }

        /// <summary>
        /// Returns a MenuComponent to create a Slider in the Menu object.
        /// </summary>
        public static MenuComponent AddSlider(this Menu menu, string internalName, string displayName, int value, int minValue = 0, int maxValue = 100)
        {
            if (menu == null)
            {
                throw new ArgumentNullException("menu");
            }

            return menu.Add(new MenuSlider(internalName, displayName, value, minValue, maxValue));
        }

        /// <summary>
        /// Returns a new list of Obj_AI_Base objects converted from Obj_AI_Hero objects.
        /// </summary>
        public static List<Obj_AI_Base> ToObj_AI_BaseList(this IEnumerable<Obj_AI_Hero> list)
        {
            List<Obj_AI_Base> newlist = new List<Obj_AI_Base>();

            if (list.Count() > 0)
            {
                foreach (Obj_AI_Hero obj in list)
                {
                    newlist.Add(obj as Obj_AI_Base);
                }
            }

            return newlist;
        }

        /// <summary>
        /// Returns a new list of Obj_AI_Base objects converted from Obj_AI_Minion objects.
        /// </summary>
        public static List<Obj_AI_Base> ToObj_AI_BaseList(this IEnumerable<Obj_AI_Minion> list)
        {
            List<Obj_AI_Base> newlist = new List<Obj_AI_Base>();

            if (list.Count() > 0)
            {
                foreach (Obj_AI_Minion obj in list)
                {
                    newlist.Add(obj as Obj_AI_Base);
                }
            }

            return newlist;
        }
        #endregion

        #region Lists
        public static List<uint> PotionItems = new List<uint>()
        {
            ItemId.HealthPotion, ItemId.TotalBiscuitofRejuvenation,
            ItemId.RefillablePotion, ItemId.CorruptingPotion, ItemId.HuntersPotion,
        };

        public static List<string> PotionBuffs = new List<string>()
        {
            "RegenerationPotion", "itemMiniRegenPotion",
            "itemCrystalFlask", "itemDarkCrystalFlask", "itemCrystalFlaskJungle",
        };

        public static List<uint> HextechItems = new List<uint>()
        {
            ItemId.HextechGunblade, ItemId.HextechProtobelt01, ItemId.HextechGLP800,
        };

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

        public static List<string> ADC_ChampionNameList = new List<string>()
        {
            "Ashe", "Caitlyn", "Corki", "Draven", "Ezreal",
            "Graves", "Jhin", "Jinx", "Kalista", "Kindred",
            "KogMaw", "Lucian", "MissFortune", "Quinn", "Sivir",
            "Tristana", "Twitch", "Urgot", "Varus", "Vayne",
            "Xayah",
        };
        #endregion
    }
}
