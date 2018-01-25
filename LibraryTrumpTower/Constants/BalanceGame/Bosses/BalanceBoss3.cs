using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.Constants.BalanceGame.Bosses
{
    public static class BalanceBoss3
    {
        public static double BOSS3_MAX_HP = 20000000;
        public static double BOSS3_DEFAULT_SPEED = 1.5;
        public static double BOSS3_DEFAULT_RELOAD_SPECIAL = 2 * 60;
        public static double BOSS3_DEFAULT_RELOAD = 2 * 60;
        public static double BOSS3_DAMAGE = 35; // Was supposed to 4shots the base. set Wall HP to 400...
        public static double BOSS3_ACTION_RADIUS = 300;
        public static double BOSS3_TIME_BEFORE_LAUNCH = 4*60;

        public static double BOSS3_CHAIN_MAX_HP = 100;
        public static double BOSS3_CHAIN_DAMAGE = 100;
        public static double BOSS3_CHAIN_SPEED = 40;
        public static double BOSS3_CHAIN_BREAK_RANGE = 900;
    }
}
