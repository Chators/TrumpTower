using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.Constants.BalanceGame.Events
{
    public static class BalanceEvent3
    {
        public static int EVENT3_CURRENT_SPEED_IN_PERCENT = 0;
        public static bool EVENT3_IS_BONUS = true;
        public static int EVENT3_BONUS_SPEED_IN_PERCENT = 50; // min 0 - max 100
        public static int EVENT3_MALUS_SPEED_IN_PERCENT = 50; // min 0 - max 100
        public static int EVENT3_TIME_ACTIF = 10*60; 
        public static int EVENT3_TIME_BEFORE_OVER = 20*60;
        public static int EVENT3_POINTS_BY_PRESSURE_EFFECT = 300 / 15; // Number Wall PV / Number pressure
        public static int EVENT3_MAX_GAUGE = 300;
    }
}
