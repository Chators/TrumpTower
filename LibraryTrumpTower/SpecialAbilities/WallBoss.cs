using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;
using Microsoft.Xna.Framework;

namespace LibraryTrumpTower.SpecialAbilities
{
    [DataContract(IsReference = true)]
    public class WallBoss
    {
       [DataMember]
        Map _ctx;
        [DataMember]
        public Vector2 Position { get; set; }
        [DataMember]
        public double Angle { get; set; }
        [DataMember]
        public bool _isBreached { get; set; }
        [DataMember]
        public bool _isUsed { get; set; }
        [DataMember]
        public int Radius { get; set; }



        public WallBoss
            (Map ctx)
        {
            _ctx = ctx;

            _isBreached = false;
            Position = new Vector2(-1000, -1000);
            _isUsed = false;
            Radius = 100;

        }

        public void PutWallBoss(Vector2 position)
        {
            Position = position;
            _isUsed = true;
        }
        public bool IsActivate => Position != new Vector2(-1000, -1000);


    }
}
