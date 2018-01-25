using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.Constants.BalanceGame.Towers
{
    public static class BalanceTowerSlow
    {
        public static int TOWER_SLOW_DAMAGE = 150;
        public static int TOWER2_SLOW_DAMAGE = 250;
        public static int TOWER3_SLOW_DAMAGE = 400;

        public static double TOWER_SLOW_SCOPE = 2000;
        public static double TOWER_SLOW_ATTACK_SPEED = 2;

        public static double TOWER_SLOW_BASE_PRICE = 400;
        public static double TOWER_SLOW_PRICE = TOWER_SLOW_BASE_PRICE * 1.5;

        private static double TOWER_SLOW_BASE_SELL = TOWER_SLOW_BASE_PRICE / 2;
        public static double TOWER_SLOW_SELL = TOWER_SLOW_BASE_PRICE;
        public static double TOWER2_SLOW_SELL = TOWER_SLOW_BASE_PRICE * 1.5;
        public static double TOWER3_SLOW_SELL = TOWER_SLOW_BASE_PRICE * 3;
    }
}
