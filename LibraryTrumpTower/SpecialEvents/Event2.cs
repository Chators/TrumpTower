using LibraryTrumpTower.Constants.BalanceGame.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;

namespace LibraryTrumpTower.SpecialEvents
{
    class Event2 : Event
    {
        public Event2(Events events, int timeBeforeOver, double maxGauge, double pointsByPressureEffect) : base(events, timeBeforeOver, maxGauge, pointsByPressureEffect)
        {
        }

        internal override void UpdateBonus()
        {
            Wall wall = Events.Map.Wall;
            wall.HealHp(wall.MaxHp - (BalanceEvent2.EVENT2_BONUS_HP_HEAL_IN_PERCENT * wall.MaxHp / 100));
            ManagerSound.PlayYouWin();
            //throw new NotImplementedException();
        }

        internal override void UpdateMalus()
        {
            Wall wall = Events.Map.Wall;
            wall.TakeHp(wall.MaxHp - (BalanceEvent2.EVENT2_MALUS_HP_DAMAGE_IN_PERCENT * wall.MaxHp / 100));
            ManagerSound.PlayGameOver();
            //throw new NotImplementedException();
        }
    }
}
