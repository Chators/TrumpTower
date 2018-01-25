using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.Constants.BalanceGame.Towers
{
    public static class BalanceTowerArea
    {
        public static int TOWER_AREA_DAMAGE = 40;
        public static int TOWER2_AREA_DAMAGE = 75;
        public static int TOWER3_AREA_DAMAGE = 100;

        public static double TOWER_AREA_SCOPE = 1200;
        public static double TOWER_AREA_ATTACK_SPEED = 1.4;

        public static double TOWER_AREA_BASE_PRICE = 400;
        public static double TOWER_AREA_PRICE = TOWER_AREA_BASE_PRICE * 1.5;

        private static double TOWER_AREA_BASE_SELL = TOWER_AREA_BASE_PRICE / 2;
        public static double TOWER_AREA_SELL = TOWER_AREA_BASE_PRICE;
        public static double TOWER2_AREA_SELL = TOWER_AREA_BASE_PRICE * 1.5;
        public static double TOWER3_AREA_SELL = TOWER_AREA_BASE_PRICE * 3;
    }
}
