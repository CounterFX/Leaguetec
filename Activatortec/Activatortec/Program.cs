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
    class Program
    {
        static Obj_AI_Hero _player;

        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        static void GameEvents_GameStart()
        {
            _player = ObjectManager.GetLocalPlayer();

            if (_player == null)
            {
                return;
            }

            Items._player = _player;

            Menus.Initialize();

            Game.OnUpdate += Game_OnUpdate;
        }

        static void Game_OnUpdate()
        {
            if (_player.IsDead)
            {
                return;
            }

            Items.Update();

            Developer();
        }

        static void Developer()
        {
            if (Menus._menu["developer"]["itemlist"].Enabled)
            {
                Console.WriteLine("Item List:");
                foreach (InventorySlot invslot in _player.Inventory.Slots)
                {
                    if (invslot.SpellName != "No Script" && invslot.SpellSlot != SpellSlot.Recall)
                    {
                        Console.WriteLine(invslot.SpellSlot + " contains " 
                            + invslot.SpellName + " with ID: " + invslot.ItemId);
                    }
                }
            }

            if (Menus._menu["developer"]["bufflist"].Enabled)
            {
                Console.WriteLine("Buff List:");
                foreach (Buff buff in _player.Buffs)
                {
                    if (buff != null && buff.Name != "No Script")
                    {
                        Console.WriteLine(buff.Name);
                    }
                }
            }
        }
    }
}
