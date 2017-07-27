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
    class Program
    {
        static Obj_AI_Hero _player;
        static List<uint> PotionItems;
        static List<string> PotionBuffs;

        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart()
        {
            _player = ObjectManager.GetLocalPlayer();

            if (_player == null)
            {
                return;
            }

            PotionItems = new List<uint>()
            {
                ItemId.HealthPotion, ItemId.TotalBiscuitofRejuvenation,
                ItemId.RefillablePotion, ItemId.CorruptingPotion, ItemId.HuntersPotion,
            };

            PotionBuffs = new List<string>()
            {
                "RegenerationPotion", "itemMiniRegenPotion",
                "itemCrystalFlask", "itemDarkCrystalFlask", "itemCrystalFlaskJungle",
            };

            Menus.Initialize();

            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate()
        {
            if (_player.IsDead)
            {
                return;
            }

            bool hasBuff = false;

            foreach (string buff in PotionBuffs)
            {
                if (_player.HasBuff(buff))
                {
                    if (!hasBuff)
                    {
                        hasBuff = true;
                    }
                }
            }
            
            foreach (uint item in PotionItems)
            {
                if (_player.HasItem(item))
                {
                    if (_player.CanUseItem(item) && !hasBuff
                        && _player.HealthPercent() <= Menus._menu["pots"]["potpercent"].Value)
                    {
                        _player.UseItem(item);
                    }
                }
            }
        }
    }
}
