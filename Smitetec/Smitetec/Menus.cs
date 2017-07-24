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
                    combomenu.Add(new MenuBool("comboblue", "Use Blue Smite", false));
                    combomenu.Add(new MenuBool("combored", "Use Red Smite"));

                    _menu.Add(combomenu);
                }

                if (smitemenu != null) 
                {
                    if (Game.MapId == GameMapId.SummonersRift)
                    {
                        smitemenu.Add(new MenuBool("red", "Smite Red Brambleback"));
                        smitemenu.Add(new MenuBool("blue", "Smite Blue Sentinel"));
                        smitemenu.Add(new MenuBool("gromp", "Smite Gromp", false));
                        smitemenu.Add(new MenuBool("krug", "Smite Ancient Krug", false));
                        smitemenu.Add(new MenuBool("raptor", "Smite Crimson Raptor", false));
                        smitemenu.Add(new MenuBool("wolf", "Smite Greater Murk Wolf", false));
                        smitemenu.Add(new MenuBool("crab", "Smite Rift Scuttler", false));
                        smitemenu.Add(new MenuBool("herald", "Smite Rift Herald"));
                        smitemenu.Add(new MenuBool("drake", "Smite Drakes"));
                        smitemenu.Add(new MenuBool("baron", "Smite Baron Nashor"));
                    }

                    if (Game.MapId == GameMapId.TwistedTreeline)
                    {
                        smitemenu.Add(new MenuBool("wraith", "Smite Wraith"));
                        smitemenu.Add(new MenuBool("golem", "Smite Big Golem"));
                        smitemenu.Add(new MenuBool("wolf", "Smite Giant Wolf"));
                        smitemenu.Add(new MenuBool("spiderboss", "Smite Vilemaw"));
                    }

                    _menu.Add(smitemenu);
                }

                if (ksmenu != null)
                {
                    ksmenu.Add(new MenuBool("ksblue", "KS with Blue Smite"));
                    ksmenu.Add(new MenuBool("ksred", "KS with Red Smite", false));

                    _menu.Add(ksmenu);
                }

                if (settingsmenu != null)
                {
                    settingsmenu.Add(new MenuSlider("smiteheal", "Use Smite to heal @ HP% <=", 25));

                    _menu.Add(settingsmenu);
                }

                _menu.Attach();
            }
        }
    }
}
