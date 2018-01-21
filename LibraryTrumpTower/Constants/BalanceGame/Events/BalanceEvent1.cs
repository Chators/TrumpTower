﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.Constants.BalanceGame.Events
{
    public static class BalanceEvent1
    {
        public static int EVENT1_BONUS_DOLLARS = 500;
        public static int EVENT1_MALUS_DOLLARS = -500;
        public static int EVENT1_TIME_BEFORE_OVER = 20*60;
        public static int EVENT1_POINTS_BY_PRESSURE_EFFECT = 300 / 15; // Number Wall PV / Number pressure
        public static int EVENT1_MAX_GAUGE = 300;
    }
}
