using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.LibraryTrumpTower.Constants
{
    public class Constant
    {
        #region The Rules Map
        public static int MinWidthMap = 20;
        public static int MaxWidthMap = 80;
        public static int MinHeightMap = 20;
        public static int MaxHeightMap = 80;

        public static int MinNameMap = 5;
        public static int MaxNameMap = 20;

        public static int MinDollarsMap = 0;
        public static int MaxDollarsMap = 10000;

        public static int MinPlaneInWave = 1;
        public static int MaxPlaneInWave = 100;
        public static int MinPlaneTimer = 1;
        public static int MaxPlaneTimer = 1000;

        #region Unit Earthly
        public static int MinEarthlyTimer = 1;
        public static int MaxEarthlyTimer = 1000;

        public static int MinDefaultUnit = 0;
        public static int MaxDefaultUnit = 100;

        public static int MinSaboteurUnit = 0;
        public static int MaxSaboteurUnit = 100;

        public static int MinDoctorUnit = 0;
        public static int MaxDoctorUnit = 100;

        public static int MinKamikazeUnit = 0;
        public static int MaxKamikazeUnit = 100;
        #endregion

        public static int MinWallHp = 1;
        public static int MaxWallHp = 100000;

        #endregion

        public static int imgSizeMap = 64;
        public static int ImgSizePlane = 64;
        public static int ImgSizeEnemy = 64;
        public static float PI = 3.14159265359f;
        public static double BankReloading = 10 * 60;
        public static int DisabledTower = 5;
    }
}
