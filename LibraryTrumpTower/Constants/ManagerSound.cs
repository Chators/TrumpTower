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

        private static SoundEffect InitTowerShoot;
        private static SoundEffectInstance TowerShoot;

        private static SoundEffect InitExplosionAbility;
        private static SoundEffectInstance ExplosionAbility;

        private static SoundEffect InitButtonExplosionAbility;
        private static SoundEffectInstance ButtonExplosionAbility;

        private static SoundEffect InitPauseIn;
        private static SoundEffectInstance PauseIn;

        private static SoundEffect InitPauseOut;
        private static SoundEffectInstance PauseOut;

        private static SoundEffect InitReloadSniper;
        private static SoundEffectInstance ReloadSniper;

        private static SoundEffect InitSniperShoot;
        private static SoundEffectInstance SniperShoot;

        private static SoundEffect InitDestroyUnitAir;
        private static SoundEffectInstance DestroyUnitAir;

        private static SoundEffect InitAlertRaidUnitsAir;
        private static SoundEffectInstance AlertRaidUnitsAir;

        private static SoundEffect InitImpactUnitAir;
        private static SoundEffectInstance ImpactUnitAir;

        private static SoundEffect InitTrumpGetOutOfHere;
        private static SoundEffectInstance TrumpGetOutOfHere;

        private static SoundEffect InitCoinUp;
        private static SoundEffectInstance CoinUp;

        private static SoundEffect InitPowerUp;
        private static SoundEffectInstance PowerUp;

        private static SoundEffect InitSell;
        private static SoundEffectInstance Sell;

        private static SoundEffect InitBuild;
        private static SoundEffectInstance Build;

        static public void LoadContent(ContentManager Content)
        {
            InitManDie = Content.Load<SoundEffect>("Sound/songManDie");
            InitTowerShoot = Content.Load<SoundEffect>("Sound/songExplosion");
            InitExplosionAbility = Content.Load<SoundEffect>("SpecialAbilities/explosionSound");
            InitButtonExplosionAbility = Content.Load<SoundEffect>("SpecialAbilities/buttonExplosionSound");
            InitPauseIn = Content.Load<SoundEffect>("ManagerTime/soundPauseIn");
            InitPauseOut = Content.Load<SoundEffect>("ManagerTime/soundPauseOut");
            Song1 = Content.Load<Song>("Sound/song1");
            InitReloadSniper = Content.Load<SoundEffect>("SpecialAbilities/Sniper/reloadSniper");
            InitSniperShoot = Content.Load<SoundEffect>("SpecialAbilities/Sniper/sniperShoot");
            InitDestroyUnitAir = Content.Load<SoundEffect>("Sound/DestroyUnitAir");
            InitAlertRaidUnitsAir = Content.Load<SoundEffect>("Sound/AlertRaidUnitsAir");
            InitImpactUnitAir = Content.Load<SoundEffect>("Sound/ImpactUnitAir");
            InitTrumpGetOutOfHere = Content.Load<SoundEffect>("Sound/trumpGetOutOfHere");
            InitCoinUp = Content.Load<SoundEffect>("Sound/coinUp");
            InitPowerUp = Content.Load<SoundEffect>("Sound/powerUp");
            InitSell = Content.Load<SoundEffect>("Sound/sell");
            InitBuild = Content.Load<SoundEffect>("Sound/build");
        }

        public static void PlayCoinUp()
        {
            CoinUp = InitCoinUp.CreateInstance();
            CoinUp.Volume = 1f;
            CoinUp.Play();
        }

        public static void PlayPowerUp()
        {
            PowerUp = InitPowerUp.CreateInstance();
            PowerUp.Volume = 0.7f;
            PowerUp.Play();
        }

        public static void PlaySell()
        {
            Sell = InitSell.CreateInstance();
            Sell.Volume = 0.5f;
            Sell.Play();
        }

        public static void PlayBuild()
        {
            Build = InitBuild.CreateInstance();
            Build.Volume = 1f;
            Build.Play();
        }

        public static void PlayPauseIn()
        {
            PauseIn = InitPauseIn.CreateInstance();
            PauseIn.Volume = 0.4f;
            PauseIn.Play();
        }

        public static void PlayPauseOut()
        {
            PauseOut = InitPauseOut.CreateInstance();
            PauseOut.Volume = 0.4f;
            PauseOut.Play();
        }

        public static void PlayTowerShoot()
        {
            TowerShoot = InitTowerShoot.CreateInstance();
            TowerShoot.Volume = 0.1f;
            TowerShoot.Play();
        }

        public static void PlayExplosionAbility()
        {
            ExplosionAbility = InitExplosionAbility.CreateInstance();
            ExplosionAbility.Volume = 0.8f;
            ExplosionAbility.Play();
        }

        public static void PlayButtonExplosionAbility()
        {
            ButtonExplosionAbility = InitButtonExplosionAbility.CreateInstance();
            ButtonExplosionAbility.Volume = 0.4f;
            ButtonExplosionAbility.Play();
        }

        public static void PlayReloadSniper()
        {
            ReloadSniper = InitReloadSniper.CreateInstance();
            ReloadSniper.Volume = 0.4f;
            ReloadSniper.Play();
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

        public static void PlayTrumpGetOutOfHere()
        {
            TrumpGetOutOfHere = InitTrumpGetOutOfHere.CreateInstance();
            TrumpGetOutOfHere.Volume = 0.85f;
            TrumpGetOutOfHere.Play();
        }
    }
}
