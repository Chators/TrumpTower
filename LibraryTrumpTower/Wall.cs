using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.LibraryTrumpTower
{
    public class Wall
    {
        Map _ctx;
        public double CurrentHp { get; private set; }
        public double MaxHp { get; private set; }
        public Vector2 Position { get; private set; }

        public Wall(Map ctx, double hp, Vector2 position)
        {
            _ctx = ctx;
            CurrentHp = hp;
            MaxHp = hp;
            Position = position;
        }

        public void TakeHp(int dammage)
        {
            CurrentHp -= dammage;
        }

        public bool IsDead()
        {
            return (CurrentHp <= 0);

        }
    }
}
