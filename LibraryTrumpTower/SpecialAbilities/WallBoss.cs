using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;


namespace LibraryTrumpTower.SpecialAbilities
{
    public class WallBoss
    {
        Map _ctx;
        Vector2 _position { get; set; }
        public double Angle { get; set; }
        public int UsingCharge { get; set; }
        public bool _isBreached { get; set; }
        public bool _isUsed { get; set; }


        public WallBoss(Map ctx)
        {
            _ctx = ctx;
            UsingCharge = 1;
            _isBreached = false;
            _position = new Vector2(-1000, -1000);
            _isUsed = false;
        }
    }

    
}
