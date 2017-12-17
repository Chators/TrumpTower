using LibraryTrumpTower;
using LibraryTrumpTower.AirUnits;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.LibraryTrumpTower
{
    [DataContract(IsReference = true)]
    public class Wall
    {
        [DataMember]
        Map _ctx;
        [DataMember]
        public double CurrentHp { get; private set; }
        [DataMember]
        public double MaxHp { get; private set; }
        [DataMember]
        public Vector2 Position { get; private set; }

        public Wall(Map ctx, double hp, Vector2 position)
        {
            if (hp < 0) throw new ArgumentException("Parameter cannot < 0", "hp must be > 0");
            _ctx = ctx;
            MaxHp = hp;
            CurrentHp = hp;
            Position = position;
        }

        public void TakeHp(double dammage)
        {
            CurrentHp -= dammage;
        }

        public bool IsDead()
        {
            return (CurrentHp <= 0);

        }

        public void ChangeHp(int hp)
        {
            if (hp < 0) throw new ArgumentException("Parameter cannot < 0", "hp must be > 0");
            MaxHp = hp;
            CurrentHp = hp;
        }

        public void ChangePosition(Vector2 position)
        {
            Position = position;

            foreach (AirUnitsCollection airCollection in _ctx.AirUnits)
                airCollection.ResetWaveOfPlane();
        }
    }
}
