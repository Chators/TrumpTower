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
        private static SoundEffect InitManDie;
        private static SoundEffectInstance ManDie;

        public static SoundEffect Explosion;
        public static SoundEffect ExplosionAbility;
        public static SoundEffect buttonExplosionSound;
        public static SoundEffect SoundButton1;
        public static SoundEffect SoundPauseIn;
        public static SoundEffect SoundPauseOut;
        public static SoundEffect ReloadSniper;

        private static SoundEffect InitSniperShoot;
        private static SoundEffectInstance SniperShoot;

        private static SoundEffect InitDestroyUnitAir;
        private static SoundEffectInstance DestroyUnitAir;

        private static SoundEffect InitAlertRaidUnitsAir;
        private static SoundEffectInstance AlertRaidUnitsAir;

        private static SoundEffect InitImpactUnitAir;
        private static SoundEffectInstance ImpactUnitAir;

        static public void LoadContent(ContentManager Content)
        {
            InitManDie = Content.Load<SoundEffect>("Sound/songManDie");
            Explosion = Content.Load<SoundEffect>("Sound/songExplosion");
            ExplosionAbility = Content.Load<SoundEffect>("SpecialAbilities/explosionSound");
            buttonExplosionSound = Content.Load<SoundEffect>("SpecialAbilities/buttonExplosionSound");
            SoundButton1 = Content.Load<SoundEffect>("Buttons/soundButton1");
            SoundPauseIn = Content.Load<SoundEffect>("ManagerTime/soundPauseIn");
            SoundPauseOut = Content.Load<SoundEffect>("ManagerTime/soundPauseOut");
            Song1 = Content.Load<Song>("Sound/song1");
            ReloadSniper = Content.Load<SoundEffect>("SpecialAbilities/Sniper/reloadSniper");
            InitSniperShoot = Content.Load<SoundEffect>("SpecialAbilities/Sniper/sniperShoot");
            InitDestroyUnitAir = Content.Load<SoundEffect>("Sound/DestroyUnitAir");
            InitAlertRaidUnitsAir = Content.Load<SoundEffect>("Sound/AlertRaidUnitsAir");
            InitImpactUnitAir = Content.Load<SoundEffect>("Sound/ImpactUnitAir");
        }

        public static void PlayManDie()
        {
            ManDie = InitManDie.CreateInstance();
            ManDie.Volume = 0.8f;
            ManDie.Play();
        }

        public static void PlaySniperShoot()
        {
            SniperShoot = InitSniperShoot.CreateInstance();
            SniperShoot.Volume = 0.2f;
            SniperShoot.Play();
        }

        public static void PlayDestroyUnitAir()
        {
            DestroyUnitAir = InitDestroyUnitAir.CreateInstance();
            DestroyUnitAir.Volume = 1f;
            DestroyUnitAir.Play();
        }

        public static void PlayAlertRaidUnitsAir()
        {
            AlertRaidUnitsAir = InitAlertRaidUnitsAir.CreateInstance();
            AlertRaidUnitsAir.Volume = 1f;
            AlertRaidUnitsAir.Play();
        }

        public static void PlayImpactUnitAir()
        {
            ImpactUnitAir = InitImpactUnitAir.CreateInstance();
            ImpactUnitAir.Volume = 0.85f;
            ImpactUnitAir.Play();
        }
    }
}
