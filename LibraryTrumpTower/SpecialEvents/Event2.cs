using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Console.Write("AddBonus");
            ManagerSound.PlayYouWin();
            //throw new NotImplementedException();
        }

        internal override void UpdateMalus()
        {
            Console.WriteLine("AddMalus");
            ManagerSound.PlayGameOver();
            //throw new NotImplementedException();
        }
    }
}
