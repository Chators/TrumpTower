using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.LibraryTrumpTower.Constants
{
    public class ManagerSound
    {

        // Musique
        public static Song Song1;

        // Sound
        public static SoundEffect ManDie;
        public static SoundEffect Explosion;
        public static SoundEffect ExplosionAbility;
        public static SoundEffect buttonExplosionSound;
        public static SoundEffect SoundButton1;
        public static SoundEffect SoundPauseIn;
        public static SoundEffect SoundPauseOut;

        static public void LoadContent(ContentManager Content)
        {
            ManDie = Content.Load<SoundEffect>("Sound/songManDie");
            Explosion = Content.Load<SoundEffect>("Sound/songExplosion");
            ExplosionAbility = Content.Load<SoundEffect>("SpecialAbilities/explosionSound");
            buttonExplosionSound = Content.Load<SoundEffect>("SpecialAbilities/buttonExplosionSound");
            SoundButton1 = Content.Load<SoundEffect>("Buttons/soundButton1");
            SoundPauseIn = Content.Load<SoundEffect>("ManagerTime/soundPauseIn");
            SoundPauseOut = Content.Load<SoundEffect>("ManagerTime/soundPauseOut");
            Song1 = Content.Load<Song>("Sound/song1");
        }
    }
}
