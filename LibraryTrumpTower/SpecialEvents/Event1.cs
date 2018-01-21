using LibraryTrumpTower.Constants.BalanceGame.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower.Constants;

namespace LibraryTrumpTower.SpecialEvents
{
    class Event1 : Event
    {
        public Event1(Events events, int timeBeforeOver, double maxGauge, double pointsByPressureEffect) : base(events, timeBeforeOver, maxGauge, pointsByPressureEffect)
        {
        }

        internal override void UpdateBonus()
        {
            Events.Map.Dollars += BalanceEvent1.EVENT1_BONUS_DOLLARS;
            ManagerSound.PlayYouWin();
            //throw new NotImplementedException();
        }

        internal override void UpdateMalus()
        {
            Events.Map.Dollars += BalanceEvent1.EVENT1_MALUS_DOLLARS;
            ManagerSound.PlayGameOver();
            //throw new NotImplementedException();
        }
    }
}
