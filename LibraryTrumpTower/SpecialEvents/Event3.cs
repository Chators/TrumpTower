using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.SpecialEvents
{
    class Event3 : Event
    {
        public Event3(Events events, int timeBeforeOver, double maxGauge, double pointsByPressureEffect) : base(events, timeBeforeOver, maxGauge, pointsByPressureEffect)
        {
        }

        internal override void UpdateBonus()
        {
            Console.Write("AddBonus");
            //throw new NotImplementedException();
        }

        internal override void UpdateMalus()
        {
            Console.WriteLine("AddMalus");
            //throw new NotImplementedException();
        }
    }
}
