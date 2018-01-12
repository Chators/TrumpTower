using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.Decors
{
    public class Decor
    {
        public int _numberDecor; // The number of decor in the folder 
        public Vector2 _position;

        public Decor(int numberDecor, Vector2 position)
        {
            _numberDecor = numberDecor;
            _position = position;
        }
    }
}
