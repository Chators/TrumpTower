using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.Constants.BalanceGame.Towers
{
    public static class BalanceTowerSlow
    {
        public static int TOWER_SLOW_DAMAGE = 19;
        public static int TOWER2_SLOW_DAMAGE = 25;
        public static int TOWER3_SLOW_DAMAGE = 32;

        public static double TOWER_SLOW_SCOPE = 800;
        public static double TOWER_SLOW_ATTACK_SPEED = 1.8;

        public static double TOWER_SLOW_BASE_PRICE = 300;
        public static double TOWER_SLOW_PRICE = TOWER_SLOW_BASE_PRICE * 1.5;

        private static double TOWER_SLOW_BASE_SELL = TOWER_SLOW_BASE_PRICE / 2;
        public static double TOWER_SLOW_SELL = TOWER_SLOW_BASE_PRICE;
        public static double TOWER2_SLOW_SELL = TOWER_SLOW_BASE_PRICE * 1.5;
        public static double TOWER3_SLOW_SELL = TOWER_SLOW_BASE_PRICE * 3;
    }
}
