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
    class Menus
    {
        public static Menu _menu;

        public static void Initialize(Orbwalker orbwalker)
        {
            _menu = new Menu("Garentec", "Garentec", true);
            orbwalker.Attach(_menu);

            if (_menu != null)
            {
                Menu ksmenu = new Menu("ksmenu", "Killsteal");

                if (ksmenu != null) 
                {
                    ksmenu.Add(new MenuBool("ksAA", "Basic attack to KS"));
                    ksmenu.Add(new MenuBool("ksR", "Use R to KS"));

                    _menu.Add(ksmenu);
                }

                _menu.Attach();
            }
        }
    }
}
