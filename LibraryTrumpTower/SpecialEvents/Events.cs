using LibraryTrumpTower.Constants.BalanceGame.Events;
using LibraryTrumpTower.SpecialEvents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;

namespace LibraryTrumpTower
{
    public class Events
    {
        public Map Map { get; set; }
        private List<Event> EventAvailable { get; set; }
        public bool IsActivate { get; set; } // event is in progress
        public Event CurrentEvent{ get; set; } 
        public int Reloading { get; set; } // time to realod
        private Random Random { get; set; }
        public bool IsActivateFirstTime { get; set; }

        private int PercentChanceOfAppearing { get; set; }
        private int TimeToReloading { get; set; }

        public Events (Map map, int percentOfChanceOfAppearing, int timeToReloading)
        {
            Map = map;
            EventAvailable = new List<Event>();
            EventAvailable.Add(new Event1(this, BalanceEvent1.EVENT1_TIME_BEFORE_OVER, BalanceEvent1.EVENT1_MAX_GAUGE, BalanceEvent1.EVENT1_POINTS_BY_PRESSURE_EFFECT));
            EventAvailable.Add(new Event2(this, BalanceEvent2.EVENT2_TIME_BEFORE_OVER, BalanceEvent2.EVENT2_MAX_GAUGE, BalanceEvent2.EVENT2_POINTS_BY_PRESSURE_EFFECT));
            EventAvailable.Add(new Event3(this, BalanceEvent3.EVENT3_TIME_BEFORE_OVER, BalanceEvent3.EVENT3_MAX_GAUGE, BalanceEvent3.EVENT3_POINTS_BY_PRESSURE_EFFECT));
            PercentChanceOfAppearing = percentOfChanceOfAppearing;
            TimeToReloading = timeToReloading;
            Random = new Random();
            IsActivate = false;
            IsActivateFirstTime = false;
            Reloading = TimeToReloading;
        }

        public void Update()
        {
            if (IsActivate)
            {
                CurrentEvent.Update();
                if (CurrentEvent.IsSuccessFull)
                {
                    CurrentEvent.UpdateBonus();
                    CurrentEvent = null;
                    IsActivate = false;
                }
                else if (CurrentEvent.TimeIsOver)
                {
                    CurrentEvent.UpdateMalus();
                    CurrentEvent = null;
                    IsActivate = false;
                }
            }
            else
            {
                Reloading--;
                if (Reloading <= 0)
                {
                    int nbRdn = Random.Next(0, 100);
                    if (nbRdn < PercentChanceOfAppearing)
                    {
                        IsActivate = true;
                        IsActivateFirstTime = true;
                        nbRdn = Random.Next(0, EventAvailable.Count-1);
                        CurrentEvent = EventAvailable[nbRdn];
                        CurrentEvent.Reset();
                    }
                    Reloading = TimeToReloading;
                }
            }
        }

        public void AddGauge() => CurrentEvent.AddGauge();
    }
}
