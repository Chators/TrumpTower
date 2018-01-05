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
        public Vector2 Position { get; set; }
        public double Angle { get; set; }
        public bool _isBreached { get; set; }
        public bool _isUsed { get; set; }
        public int Radius { get; set; }



        public WallBoss(Map ctx)
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
       
        

        public void Update()
        {
            if (IsActivate && _isBreached == false)
            {
                
                Console.WriteLine(Position);
                Console.WriteLine("LAAAA");
            }
        }
        
    }


}
