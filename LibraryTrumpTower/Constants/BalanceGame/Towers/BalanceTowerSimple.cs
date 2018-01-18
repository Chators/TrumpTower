using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.Constants.BalanceGame.Towers
{
    public static class BalanceTowerSimple
    {
        public static int TOWER_SIMPLE_DAMAGE = 6;
        public static int TOWER2_SIMPLE_DAMAGE = 13;
        public static int TOWER3_SIMPLE_DAMAGE = 20;

        public static double TOWER_SIMPLE_SCOPE = 800;
        public static double TOWER_SIMPLE_ATTACK_SPEED = 0.8;

        public static double TOWER_SIMPLE_BASE_PRICE = 200;
        public static double TOWER_SIMPLE_PRICE = TOWER_SIMPLE_BASE_PRICE * 1.5;

        private static double TOWER_SIMPLE_BASE_SELL = TOWER_SIMPLE_BASE_PRICE / 2;
        public static double TOWER_SIMPLE_SELL = TOWER_SIMPLE_BASE_PRICE;
        public static double TOWER2_SIMPLE_SELL = TOWER_SIMPLE_BASE_PRICE * 1.5;
        public static double TOWER3_SIMPLE_SELL = TOWER_SIMPLE_BASE_PRICE * 3;

    }
}
