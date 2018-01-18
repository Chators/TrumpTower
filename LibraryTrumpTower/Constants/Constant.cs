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

        public static int MinWidthMap = 30;
        public static int MaxWidthMap = 45;
        public static int MinHeightMap =30;
        public static int MaxHeightMap = 45;

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

        #region Entity
        public static double LOSTGAUGE = 1;
        public static double MINLOSTGAUGE = 0;
        public static double MAXLOSTGAUGE = 2;

        public static double MAXGAUGE = 500;
        public static int PRICEIMPROVEGAUGE = 50;
        public static double ADDGAUGEWHENIMPROVE = 100;
        #endregion

        public static int imgSizeMap = 64;
        public static int ImgSizePlane = 64;
        public static int ImgSizeEnemy = 64;
        public static float PI = 3.14159265359f;

        #region Info Editor Map
        public static string ROAD_INFO = "Permet aux ennemis de se deplacer";
        public static string EMPTY_TOWER_INFO = "Un emplacement pour construire des tourelles";
        public static string WHITE_HOUSE_INFO = "Si la maison blanche est detruite le joueur a perdu,\n elle est unique et se construit sur une route (uniquement sur le bord de la carte)";
        public static string SELECTION_INFO = "Permet 2 choses, construire un spawn d'ennemis sur la carte (uniquement sur le bord) ou modifier la maison blanche (si elle est construite)";
        public static string OTHER_INFO = "Element de decors";
        #endregion
    }
}
