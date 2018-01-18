using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower.Constants;

namespace LibraryTrumpTower.Constants.BalanceGame.Enemies
{
    public static class BalanceEnemySaboteur
    {
        public static double ENEMY_SABOTEUR_MAX_HP = 50;
        public static double ENEMY_KAMIKAZE_DAMAGE = 5;
        public static double ENEMY_KAMIKAZE_DEFAULT_SPEED = 4;
        public static double ENEMY_KAMIKAZE_BOUNTY = 150;
        public static double ENEMY_KAMIKAZE_ACTION_RADIUS = 500;
        public static double ENEMY_SABOTEUR_RELOADING = Constant.DisabledTower * 60; // init
    }
}
