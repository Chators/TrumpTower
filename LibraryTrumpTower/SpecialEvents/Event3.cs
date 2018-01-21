using LibraryTrumpTower.Constants.BalanceGame.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower.Constants;

namespace LibraryTrumpTower.SpecialEvents
{
    class Event3 : Event
    {
        public Event3(Events events, int timeBeforeOver, double maxGauge, double pointsByPressureEffect) : base(events, timeBeforeOver, maxGauge, pointsByPressureEffect)
        {
        }

        internal override void UpdateBonus()
        {
            BalanceEvent3.EVENT3_IS_BONUS = true;
            Events.EventTimerEffect = BalanceEvent3.EVENT3_TIME_ACTIF;
            BalanceEvent3.EVENT3_CURRENT_SPEED_IN_PERCENT = BalanceEvent3.EVENT3_BONUS_SPEED_IN_PERCENT;
            ManagerSound.PlayYouWin();
        }

        internal override void UpdateMalus()
        {
            BalanceEvent3.EVENT3_IS_BONUS = false;
            Events.EventTimerEffect = BalanceEvent3.EVENT3_TIME_ACTIF;
            BalanceEvent3.EVENT3_CURRENT_SPEED_IN_PERCENT = BalanceEvent3.EVENT3_MALUS_SPEED_IN_PERCENT;
            ManagerSound.PlayGameOver();
        }
    }
}
