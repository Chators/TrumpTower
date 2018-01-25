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

        private static SoundEffect InitnoAvailable;
        private static SoundEffectInstance noAvailable;

        private static SoundEffect InitPlaneTurbo;
        private static SoundEffectInstance PlaneTurbo;

        private static SoundEffect InitRice;
        private static SoundEffectInstance Rice;

        private static SoundEffect InitRiceSplash;
        private static SoundEffectInstance RiceSplash;

        private static SoundEffect InitGameOver;
        private static SoundEffectInstance GameOver;

        private static SoundEffect InitPayAtCheckout;
        private static SoundEffectInstance PayAtCheckout;

        private static SoundEffect InitYouWin;
        private static SoundEffectInstance YouWin;

        private static SoundEffect InitLowLife;
        private static SoundEffectInstance LowLife;

        private static SoundEffect InitWallBreak;
        private static SoundEffectInstance WallBreak;

        private static SoundEffect InitLaunchingChain;
        private static SoundEffectInstance LaunchingChain;

        private static SoundEffect InitStalledChain;
        private static SoundEffectInstance StalledChain;

        private static SoundEffect InitGangnamStyle;
        private static SoundEffectInstance GangnamStyle;

        private static SoundEffect InitExplosionC4;
        private static SoundEffectInstance ExplosionC4;

        private static SoundEffect InitGragasLaught;
        private static SoundEffectInstance GragasLaught;

        static public void LoadContent(ContentManager Content)
        {
            InitGragasLaught = Content.Load<SoundEffect>("Sound/boss3Laught");
            InitExplosionC4 = Content.Load<SoundEffect>("Sound/boss3ExplosionC4");
            InitGangnamStyle = Content.Load<SoundEffect>("Sound/boss3GangnamStyle");
            InitLaunchingChain = Content.Load<SoundEffect>("Sound/boss3LaunchingChain");
            InitStalledChain = Content.Load<SoundEffect>("Sound/boss3StalledChain");
            InitRiceSplash = Content.Load<SoundEffect>("Sound/riceSplash");
            InitWallBreak = Content.Load<SoundEffect>("Sound/wallBreak");
            InitRice = Content.Load<SoundEffect>("Sound/rice");
            InitPlaneTurbo = Content.Load<SoundEffect>("Sound/soundPlaneTurbo");
            InitManDie = Content.Load<SoundEffect>("Sound/songManDie");
            InitTowerShoot = Content.Load<SoundEffect>("Sound/songExplosion");
            InitExplosionAbility = Content.Load<SoundEffect>("Sound/explosionSound");
            InitButtonExplosionAbility = Content.Load<SoundEffect>("Sound/buttonExplosionSound");
            InitPauseIn = Content.Load<SoundEffect>("Sound/soundPauseIn");
            InitPauseOut = Content.Load<SoundEffect>("Sound/soundPauseOut");
            Song1 = Content.Load<Song>("Sound/song1");
            InitReloadSniper = Content.Load<SoundEffect>("Sound/reloadSniper");
            InitSniperShoot = Content.Load<SoundEffect>("Sound/sniperShoot");
            InitDestroyUnitAir = Content.Load<SoundEffect>("Sound/DestroyUnitAir");
            InitAlertRaidUnitsAir = Content.Load<SoundEffect>("Sound/AlertRaidUnitsAir");
            InitImpactUnitAir = Content.Load<SoundEffect>("Sound/ImpactUnitAir");
            InitTrumpGetOutOfHere = Content.Load<SoundEffect>("Sound/trumpGetOutOfHere");
            InitCoinUp = Content.Load<SoundEffect>("Sound/coinUp");
            InitPowerUp = Content.Load<SoundEffect>("Sound/powerUp");
            InitSell = Content.Load<SoundEffect>("Sound/sell");
            InitBuild = Content.Load<SoundEffect>("Sound/build");
            InitnoAvailable = Content.Load<SoundEffect>("Sound/no_available");
            InitGameOver = Content.Load<SoundEffect>("Sound/game_over");
            InitPayAtCheckout = Content.Load<SoundEffect>("Sound/PayAtCheckout");
            InitYouWin = Content.Load<SoundEffect>("Sound/you_win");
            InitLowLife = Content.Load<SoundEffect>("Sound/low_life_sound");
        }

        public static void PlayGragasLaught()
        {
            GragasLaught = InitGragasLaught.CreateInstance();
            GragasLaught.Volume = 1f;
            GragasLaught.Play();
        }

        public static void PlayExplosionC4()
        {
            ExplosionC4 = InitExplosionC4.CreateInstance();
            ExplosionC4.Volume = 1f;
            ExplosionC4.Play();
        }

        public static void PlayGangnamStyle()
        {
            GangnamStyle = InitGangnamStyle.CreateInstance();
            GangnamStyle.Volume = 1f;
            GangnamStyle.Play();
        }

        public static void PlayLaunchingChain()
        {
            LaunchingChain = InitLaunchingChain.CreateInstance();
            LaunchingChain.Volume = 1f;
            LaunchingChain.Play();
        }

        public static void PlayStalledChain()
        {
            StalledChain = InitStalledChain.CreateInstance();
            StalledChain.Volume = 1f;
            StalledChain.Play();
        }

        public static void PlayLowLife()
        {
            LowLife = InitLowLife.CreateInstance();
            LowLife.Volume = 1f;
            LowLife.Play();
        }
        public static void PlayPayAtCheckout()
        {
            PayAtCheckout = InitPayAtCheckout.CreateInstance();
            PayAtCheckout.Volume = 1f;
            PayAtCheckout.Play();
        }

        public static void PlayYouWin()
        {
            YouWin = InitYouWin.CreateInstance();
            YouWin.Volume = 1f;
            YouWin.Play();
        }

        public static void PlayRiceSplash()
        {
            RiceSplash = InitRiceSplash.CreateInstance();
            RiceSplash.Volume = 1f;
            RiceSplash.Play();
        }

        public static void PlayRice()
        {
            Rice = InitRice.CreateInstance();
            Rice.Volume = 0.6f;
            Rice.Play();
        }

        public static void PlayPlaneTurbo()
        {
            PlaneTurbo = InitPlaneTurbo.CreateInstance();
            PlaneTurbo.Volume = 0.8f;
            PlaneTurbo.Play();
        }

        public static void PlayGameOver()
        {
            CoinUp = InitGameOver.CreateInstance();
            CoinUp.Volume = 1f;
            CoinUp.Play();
        }

        public static void PlayNoAvailable()
        {
            CoinUp = InitnoAvailable.CreateInstance();
            CoinUp.Volume = 1f;
            CoinUp.Play();
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
            PowerUp.Volume = 0.4f;
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
            PauseIn.Volume = 0.3f;
            PauseIn.Play();
        }

        public static void PlayPauseOut()
        {
            PauseOut = InitPauseOut.CreateInstance();
            PauseOut.Volume = 0.3f;
            PauseOut.Play();
        }

        public static void PlayWallBreak()
        {
            WallBreak = InitWallBreak.CreateInstance();
            WallBreak.Volume = 0.3f;
            WallBreak.Play();
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
            ButtonExplosionAbility.Volume = 0.2f;
            ButtonExplosionAbility.Play();
        }

        public static void PlayReloadSniper()
        {
            ReloadSniper = InitReloadSniper.CreateInstance();
            ReloadSniper.Volume = 0.2f;
            ReloadSniper.Play();
        }

        public static void PlayManDie()
        {
            ManDie = InitManDie.CreateInstance();
            ManDie.Volume = 0.35f;
            ManDie.Play();
        }

        public static void PlaySniperShoot()
        {
            SniperShoot = InitSniperShoot.CreateInstance();
            SniperShoot.Volume = 0.15f;
            SniperShoot.Play();
        }

        public static void PlayDestroyUnitAir()
        {
            DestroyUnitAir = InitDestroyUnitAir.CreateInstance();
            DestroyUnitAir.Volume = 0.4f;
            DestroyUnitAir.Play();
        }

        public static void PlayAlertRaidUnitsAir()
        {
            AlertRaidUnitsAir = InitAlertRaidUnitsAir.CreateInstance();
            AlertRaidUnitsAir.Volume = 0.4f;
            AlertRaidUnitsAir.Play();
        }

        public static void PlayImpactUnitAir()
        {
            ImpactUnitAir = InitImpactUnitAir.CreateInstance();
            ImpactUnitAir.Volume = 0.4f;
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
