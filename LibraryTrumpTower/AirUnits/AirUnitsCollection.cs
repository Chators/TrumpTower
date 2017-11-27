using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;

namespace LibraryTrumpTower.AirUnits
{
    public class AirUnitsCollection
    {
        public Map Ctx { get; private set; }
        public List<AirUnit> Array { get; private set; }
        private int TimerBeforeStarting { get; set; }

        public AirUnitsCollection (Map ctx, int timerBeforeStarting, int numbersPlane)
        {
            Ctx = ctx;
            Array = new List<AirUnit>();
            TimerBeforeStarting = timerBeforeStarting;

            for (int i = 0; i < numbersPlane; i++) CreatePlane(new AirUnit(Array, Ctx.Wall, 100, 100, new Vector2(1500, 400), 2, 0, i*300));
        }

        public void Update()
        {
            for (int i = 0; i < Array.Count; i++)
            {
                AirUnit unit = Array[i];
                unit.Update();
            }
        }

        public AirUnit CreatePlane(AirUnit unit)
        {
            Array.Add(unit);
            return unit;
        }

        public bool IsStarting => TimerBeforeStarting <= 0;
    }
}
