using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.Constants.BalanceGame.Towers
{
    public static class BalanceTowerSimple
    {
        public static int TOWER_SIMPLE_DAMAGE = 50;
        public static int TOWER2_SIMPLE_DAMAGE = 80;
        public static int TOWER3_SIMPLE_DAMAGE = 110;

        public static double TOWER_SIMPLE_SCOPE = 1200;
        public static double TOWER_SIMPLE_ATTACK_SPEED = 0.7;

        public static double TOWER_SIMPLE_BASE_PRICE = 400;
        public static double TOWER_SIMPLE_PRICE = TOWER_SIMPLE_BASE_PRICE * 1.5;

        private static double TOWER_SIMPLE_BASE_SELL = TOWER_SIMPLE_BASE_PRICE / 2;
        public static double TOWER_SIMPLE_SELL = TOWER_SIMPLE_BASE_PRICE;
        public static double TOWER2_SIMPLE_SELL = TOWER_SIMPLE_BASE_PRICE * 1.5;
        public static double TOWER3_SIMPLE_SELL = TOWER_SIMPLE_BASE_PRICE * 3;

    }
}
