using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;

namespace LibraryTrumpTower
{
    public enum EntityFace
    {
        VeryAngry,
        Angry,
        Happy,
        None
    }

    [DataContract(IsReference = true)]
    public class Entity
    {
        [DataMember]
        private Map Map { get; set; }
        [DataMember]
        public double CurrentGauge { get; private set; }
        [DataMember]
        public double MaxGauge { get; private set; }
        [DataMember]
        public double LostGauge { get; set; }
        [DataMember]
        public int PriceImproveGauge { get; set; }
        [DataMember]
        private double AddGaugeWhenImprove { get; set; }
        [DataMember]
        public EntityFace EntityFace { get; set; }

        public Entity(Map map)
        {
            Map = map;
            CurrentGauge = Constant.MAXGAUGE;
            MaxGauge = Constant.MAXGAUGE;
            LostGauge = Constant.LOSTGAUGE;
            PriceImproveGauge = Constant.PRICEIMPROVEGAUGE;
            AddGaugeWhenImprove = Constant.ADDGAUGEWHENIMPROVE;
            EntityFace = EntityFace.Happy;
        }

        public void Update()
        {
            CurrentGauge = CurrentGauge - LostGauge < 0 ? 0 : CurrentGauge - LostGauge; // Update Gauge

            if (CurrentGauge < (MaxGauge * 25) / 100)
            {
                // malus
                EntityFace = EntityFace.VeryAngry;
            }
            else if (CurrentGauge < (MaxGauge * 50) / 100)
            {
                // malus
                EntityFace = EntityFace.Angry;
            }
            else 
            {
                // bonus
                EntityFace = EntityFace.Happy;
            }
        }

        public void PayEntity()
        {
            if (Map.Dollars >= PriceImproveGauge)
            {
                Map.Dollars -= PriceImproveGauge;
                CurrentGauge = CurrentGauge + AddGaugeWhenImprove > MaxGauge ? MaxGauge : (CurrentGauge + AddGaugeWhenImprove);
                ManagerSound.PlayPayAtCheckout();
            }
        }
    }
}
