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

namespace Potiontec
{
    class Menus
    {
        public static Menu _menu;

        public static void Initialize()
        {
            _menu = new Menu("Potiontec", "Potiontec", true);

            if (_menu != null)
            {
                Menu potmenu = new Menu("pots", "Potions");

                if (potmenu != null) 
                {
                    potmenu.Add(new MenuSlider("potpercent", "Use any Potion to heal @ HP% <=", 25));

                    _menu.Add(potmenu);
                }

                _menu.Attach();
            }
        }
    }
}