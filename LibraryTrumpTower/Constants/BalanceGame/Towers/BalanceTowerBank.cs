using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.Constants.BalanceGame.Towers
{
    public static class BalanceTowerBank
    {
        public static double TOWER_BANK_EARNINGS_MONEY = 200;
        public static double TOWER2_BANK_EARNINGS_MONEY = 300;
        public static double TOWER3_BANK_EARNINGS_MONEY = 400;

        public static double TOWER_BANK_RELOADING = 10 * 60;
        public static int TOWER_BANK_DAMAGE = 13;
        public static double TOWER_BANK_SCOPE = 800;
        public static double TOWER_BANK_ATTACK_SPEED = 1.2;

        public static double TOWER_BANK_BASE_PRICE = 400;
        public static double TOWER_BANK_PRICE = TOWER_BANK_BASE_PRICE * 1.5;

        private static double TOWER_BANK_BASE_SELL = TOWER_BANK_BASE_PRICE / 2;
        public static double TOWER_BANK_SELL = TOWER_BANK_BASE_PRICE;
        public static double TOWER2_BANK_SELL = TOWER_BANK_BASE_PRICE * 1.5;
        public static double TOWER3_BANK_SELL = TOWER_BANK_BASE_PRICE * 3;
    }
}
