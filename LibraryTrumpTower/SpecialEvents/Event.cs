using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.SpecialEvents
{
    public abstract class Event
    {
        internal Events Events { get; set; }
        public int CurrentTime { get; private set; }
        public double CurrentGauge { get; private set; }

        public int TimeBeforeOver { get; private set; }
        internal double PointsByPressureEffect { get; set; }
        public double MaxGauge { get; private set; }

        public Event (Events events, int timeBeforeOver, double maxGauge, double pointsByPressureEffect)
        {
            Events = events;
            PointsByPressureEffect = pointsByPressureEffect;
            MaxGauge = maxGauge;
            CurrentGauge = 0;
            TimeBeforeOver = timeBeforeOver;
            CurrentTime = timeBeforeOver;
        }

        internal abstract void UpdateBonus();

        internal abstract void UpdateMalus();

        internal void Update()
        {
            CurrentTime--;
        }

        internal void AddGauge() => CurrentGauge += PointsByPressureEffect;
        internal bool IsSuccessFull => CurrentGauge >= MaxGauge;
        internal bool TimeIsOver => CurrentTime <= 0;
        internal void Reset()
        {
            CurrentGauge = 0;
            CurrentTime = TimeBeforeOver;
        }
    }
}
