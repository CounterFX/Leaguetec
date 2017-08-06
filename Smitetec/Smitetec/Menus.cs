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
    class Menus
    {
        public static Menu _menu;

        public static void Initialize(Orbwalker orbwalker)
        {
            _menu = new Menu("Smitetec", "Smitetec", true);
            orbwalker.Attach(_menu);

            if (_menu != null)
            {
                Menu combomenu = new Menu("combo", "Combo");
                Menu smitemenu = new Menu("smite", "Smite");
                Menu ksmenu = new Menu("killsteal", "KillSteal");
                Menu settingsmenu = new Menu("settings", "Settings");


                if (combomenu != null)
                {
                    combomenu.AddBoolean("comboblue", "Use Blue Smite", false);
                    combomenu.AddBoolean("combored", "Use Red Smite");

                    _menu.Add(combomenu);
                }

                if (smitemenu != null) 
                {
                    if (Game.MapId == GameMapId.SummonersRift)
                    {
                        smitemenu.AddBoolean("red", "Smite Red Brambleback");
                        smitemenu.AddBoolean("blue", "Smite Blue Sentinel");
                        smitemenu.AddBoolean("gromp", "Smite Gromp", false);
                        smitemenu.AddBoolean("krug", "Smite Ancient Krug", false);
                        smitemenu.AddBoolean("raptor", "Smite Crimson Raptor", false);
                        smitemenu.AddBoolean("wolf", "Smite Greater Murk Wolf", false);
                        smitemenu.AddBoolean("crab", "Smite Rift Scuttler", false);
                        smitemenu.AddBoolean("herald", "Smite Rift Herald");
                        smitemenu.AddBoolean("drake", "Smite Drakes");
                        smitemenu.AddBoolean("baron", "Smite Baron Nashor");
                    }

                    if (Game.MapId == GameMapId.TwistedTreeline)
                    {
                        smitemenu.AddBoolean("wraith", "Smite Wraith");
                        smitemenu.AddBoolean("golem", "Smite Big Golem");
                        smitemenu.AddBoolean("wolf", "Smite Giant Wolf");
                        smitemenu.AddBoolean("spiderboss", "Smite Vilemaw");
                    }

                    _menu.Add(smitemenu);
                }

                if (ksmenu != null)
                {
                    ksmenu.AddBoolean("ksblue", "KS with Blue Smite");
                    ksmenu.AddBoolean("ksred", "KS with Red Smite", false);

                    _menu.Add(ksmenu);
                }

                if (settingsmenu != null)
                {
                    settingsmenu.AddSlider("smiteheal", "Use Smite to heal @ HP% <=", 25);

                    _menu.Add(settingsmenu);
                }

                _menu.Attach();
            }
        }
    }
}
