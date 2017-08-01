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
    class Menus
    {
        public static Menu _menu;
        public static string[] minionlist;

        public static void Initialize()
        {
            _menu = new Menu("Activatortec", "Activatortec", true);
            minionlist = new string[] { "Melee", "Ranged", "Siege" };

            if (_menu != null)
            {
                Menu offmenu = new Menu("offensive", "Offensive Items");
                Menu defmenu = new Menu("defensive", "Defensive Items");
                Menu utilmenu = new Menu("utility", "Utility Items");
                Menu consumemenu = new Menu("consumable", "Consumable Items");
                Menu summonermenu = new Menu("summoner", "Summoner Spells");
                Menu devmenu = new Menu("developer", "Developer Debug");

                if (offmenu != null)
                {
                    offmenu.AddBoolean("cutlass", "KS with Bilgewater Cutlass", false);
                    offmenu.AddBoolean("botrk", "KS with Blade of the Ruined King", false);
                    offmenu.AddBoolean("cutlass", "KS with Bilgewater Cutlass", false);
                    offmenu.AddBoolean("botrk", "KS with Blade of the Ruined King", false);
                    offmenu.AddBoolean("gunblade", "KS with Hextech Gunblade");
                    offmenu.AddBoolean("tiamat", "KS with Tiamat");
                    offmenu.AddBoolean("ravenous", "KS with Ravenous Hydra");

                    _menu.Add(offmenu);
                }

                if (utilmenu != null)
                {
                    utilmenu.AddBoolean("banner", "Use Banner of Command");
                    utilmenu.AddList("bannerunit", "Minion to promote:", minionlist, 2);

                    if (GameObjects.AllyHeroes.Where(a => !a.IsMe).Count() > 0)
                    {
                        defmenu.AddBoolean("vow", "Partner Knight's Vow");

                        Menu partnermenu = new Menu("partner", "Partner Whitelist:");

                        if (partnermenu != null)
                        {
                            foreach (Obj_AI_Hero ally in GameObjects.AllyHeroes.Where(a => !a.IsMe))
                            {
                                if (ally.ChampionName == GameObjects.AllyHeroes
                                    .Where(a => Extensions.ADC_ChampionNameList.Contains(a.ChampionName))
                                    .OrderBy(a => a.TotalAttackDamage).FirstOrDefault().ChampionName)
                                {
                                    partnermenu.AddBoolean(ally.Name, ally.Name + "'s " + ally.ChampionName);
                                }
                                else
                                {
                                    partnermenu.AddBoolean(ally.Name, ally.Name + "'s " + ally.ChampionName, false);
                                }
                            }

                            utilmenu.Add(partnermenu);
                        }

                        utilmenu.AddBoolean("zeke", "Bind Zeke's Convergence");
                        Menu bindmenu = new Menu("bind", "Binding Whitelist:");

                        if (bindmenu != null)
                        {
                            foreach (Obj_AI_Hero ally in GameObjects.AllyHeroes.Where(a => !a.IsMe))
                            {
                                if (ally.ChampionName == GameObjects.AllyHeroes
                                    .Where(a => Extensions.ADC_ChampionNameList.Contains(a.ChampionName))
                                    .OrderBy(a => a.TotalAttackDamage).FirstOrDefault().ChampionName)
                                {
                                    bindmenu.AddBoolean(ally.Name, ally.Name + "'s " + ally.ChampionName);
                                }
                                else
                                {
                                    bindmenu.AddBoolean(ally.Name, ally.Name + "'s " + ally.ChampionName, false);
                                }
                            }

                            utilmenu.Add(bindmenu);
                        }
                    }

                    _menu.Add(utilmenu);
                }

                if (consumemenu != null) 
                {
                    consumemenu.AddBoolean("usepot", "Use Healing Potions");
                    consumemenu.AddSlider("potpercent", "When player's Health % is <=", 25);
                    consumemenu.AddBoolean("useelixir", "Use Elixirs");

                    if (Game.MapId == GameMapId.HowlingAbyss)
                    {
                        consumemenu.AddBoolean("useoracle", "Use Oracle's Extract");
                        consumemenu.AddBoolean("usesnax", "Feed Poro-Snaxs");
                    }

                    _menu.Add(consumemenu);
                }

                if (devmenu != null)
                {
                    devmenu.AddKeyBind("itemlist", "Item List", KeyCode.Up, KeybindType.Press);
                    devmenu.AddKeyBind("bufflist", "Buff List", KeyCode.Down, KeybindType.Press);

                    _menu.Add(devmenu);
                }

                _menu.Attach();
            }
        }
    }
}
