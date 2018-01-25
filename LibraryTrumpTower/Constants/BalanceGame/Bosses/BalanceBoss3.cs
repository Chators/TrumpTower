using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.Constants.BalanceGame.Bosses
{
    public static class BalanceBoss3
    {
        public static double BOSS3_MAX_HP = 2000;
        public static double BOSS3_DEFAULT_SPEED = 1.5;
        public static double BOSS3_DEFAULT_RELOAD = 18 * 60;
        public static double BOSS3_DAMAGE = 1000; // Was supposed to 4shots the base. set Wall HP to 400...
        public static double BOSS3_ACTION_RADIUS = 300;
        public static double BOSS3_TIME_BEFORE_LAUNCH = 4*60;

        public static double BOSS3_CHAIN_MAX_HP = 100;
        public static double BOSS3_CHAIN_DAMAGE = 10;
        public static double BOSS3_CHAIN_SPEED = 25;
        public static double BOSS3_CHAIN_BREAK_RANGE = 900;
    }
}
