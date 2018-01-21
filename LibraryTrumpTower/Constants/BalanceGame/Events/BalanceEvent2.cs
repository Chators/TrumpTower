using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.Constants.BalanceGame.Events
{
    public static class BalanceEvent2
    {
        public static int EVENT2_BONUS_HP_HEAL_IN_PERCENT = 25; // min 0 - max 100
        public static int EVENT2_MALUS_HP_DAMAGE_IN_PERCENT = 25; // min 0 - max 100
        public static int EVENT2_TIME_BEFORE_OVER = 20*60;
        public static int EVENT2_POINTS_BY_PRESSURE_EFFECT = 300 / 15;
        public static int EVENT2_MAX_GAUGE = 300;
    }
}
