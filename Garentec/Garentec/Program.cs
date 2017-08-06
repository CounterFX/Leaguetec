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
    class Program
    {
        static Obj_AI_Hero _player;
        static Orbwalker _orbwalker;

        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart()
        {
            _player = ObjectManager.GetLocalPlayer();
            _orbwalker = new Orbwalker();

            if (_player == null) return;

            if (_player.ChampionName != "Garen") return;

            Spells._player = _player;
            Modes._player = _player;
            Spells.Initialize();
            Menus.Initialize(_orbwalker);

            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate()
        {
            if (_player.IsDead) return;

            Spells.hasPerformedAction = false;

            if (!_player.IsRecalling())
            {
                if (_orbwalker.Mode.HasFlag(OrbwalkingMode.Combo))
                {
                    Modes.Combo();
                }

                if (_orbwalker.Mode.HasFlag(OrbwalkingMode.Laneclear))
                {
                    Modes.Clear();
                }

                if (_orbwalker.Mode.HasFlag(OrbwalkingMode.Lasthit))
                {
                    Modes.Lasthit();
                }
            }

            Modes.Killsteal();
        }
    }
}
