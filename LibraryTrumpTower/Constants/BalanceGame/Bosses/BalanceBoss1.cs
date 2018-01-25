using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower.Constants;

namespace LibraryTrumpTower.Constants.BalanceGame.Bosses
{
    public static class BalanceBoss1
    {
        public static double BOSS1_MAX_HP = 8000;
        public static double BOSS1_TIME_BEFORE_CHARGING = 3 * 60;
        public static double BOSS1_TIME_OF_VULNERABILITY = 3 * 60;
        public static double BOSS1_TIME_BEFORE_END_OF_CASTING_CHARGE = 1.5 * 60;
        public static double BOSS1_DAMAGE = 250; // Was supposed to 4shots the base. set Wall HP to 1000...
        public static double BOSS1_DEFAULT_SPEED = 2;
        public static double BOSS1_DEFAULT_RELOAD = 2 * 60;
        public static double BOSS1_RANGE = 100;
    }
}
