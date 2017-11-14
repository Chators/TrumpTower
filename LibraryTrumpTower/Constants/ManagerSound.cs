using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.LibraryTrumpTower.Constants
{
    public class ManagerSound
    {
        public static SoundEffect ManDie;
        public static SoundEffect Explosion;

        static public void LoadContent(ContentManager Content)
        {
            ManDie = Content.Load<SoundEffect>("Sound/songManDie");
            Explosion = Content.Load<SoundEffect>("Sound/songExplosion");
        }
    }
}
