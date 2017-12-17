using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;

namespace LibraryTrumpTower.AirUnits
{
    [DataContract(IsReference = true)]
    public class AirUnitsCollection
    {
        #region Fields
        [DataMember]
        public Map Ctx { get; private set; }
        [DataMember]
        public List<AirUnit> Array { get; set; }
        [DataMember]
        public Wall Wall { get; private set; }
        [DataMember]
        public PlaneType TypeOfPlane { get; private set; }
        [DataMember]
        public int TimerBeforeStarting { get; private set; }
        [DataMember]
        public Random Random { get; private set; }
        #endregion

        public AirUnitsCollection (Map ctx, int timerBeforeStarting, int numbersPlane, PlaneType type, Wall wall)
        {
            Ctx = ctx;
            Array = new List<AirUnit>();
            TimerBeforeStarting = timerBeforeStarting;
            Wall = wall;
            TypeOfPlane = type;
            Random = new Random();
            for (int i = 1; i < numbersPlane+1; i++) CreatePlane(type, i*60, Random);
        }

        public void Update()
        {
            if (IsStarting)
            {
                for (int i = 0; i < Array.Count; i++)
                {
                    AirUnit unit = Array[i];
                    unit.Update();
                }
            }
            else TimerBeforeStarting--;
        }

        public AirUnit CreatePlane(PlaneType type, int timer, Random random)
        {
            #region RandomPosition
            Vector2 positionPlane = new Vector2();
            int randomSide = random.Next(1, 4);
            // Wall at Left
            if (Ctx.Wall.Position.X == 0)
            {
                if (randomSide == 1 || randomSide == 2)
                {
                    positionPlane.X = random.Next(100, Ctx.WidthArrayMap * Constant.imgSizeMap);
                    randomSide = random.Next(1, 3);
                    if (randomSide == 1) positionPlane.Y = 0;
                    else if (randomSide == 2) positionPlane.Y = Ctx.HeightArrayMap * Constant.imgSizeMap;
                }
                else if (randomSide == 3)
                {
                    positionPlane.X = Ctx.WidthArrayMap * Constant.imgSizeMap;
                    positionPlane.Y = random.Next(0, Ctx.HeightArrayMap * Constant.imgSizeMap);
                }
            }
            // Wall at Right
            else if (Ctx.Wall.Position.X == Ctx.WidthArrayMap * Constant.imgSizeMap - Constant.imgSizeMap)
            {
                if (randomSide == 1 || randomSide == 2)
                {
                    positionPlane.X = random.Next(0, Ctx.WidthArrayMap * Constant.imgSizeMap - 100);
                    randomSide = random.Next(1, 3);
                    if (randomSide == 1) positionPlane.Y = 0;
                    else if (randomSide == 2) positionPlane.Y = Ctx.HeightArrayMap * Constant.imgSizeMap;
                }
                else if (randomSide == 3)
                {
                    positionPlane.X = 0;
                    positionPlane.Y = random.Next(0, Ctx.HeightArrayMap * Constant.imgSizeMap);
                }
            }
            // Wall at Top
            else if (Ctx.Wall.Position.Y == 0)
            {
                if (randomSide == 1 || randomSide == 2)
                {
                    positionPlane.Y = random.Next(100, Ctx.WidthArrayMap * Constant.imgSizeMap);
                    randomSide = random.Next(1, 3);
                    if (randomSide == 1) positionPlane.X = 0;
                    else if (randomSide == 2) positionPlane.X = Ctx.WidthArrayMap * Constant.imgSizeMap;
                }
                else if (randomSide == 3)
                {
                    positionPlane.Y = Ctx.HeightArrayMap * Constant.imgSizeMap;
                    positionPlane.X = random.Next(0, Ctx.WidthArrayMap * Constant.imgSizeMap);
                }
            }
            // Wall at Bottom
            else if (Ctx.Wall.Position.Y == Ctx.HeightArrayMap * Constant.imgSizeMap - Constant.imgSizeMap)
            {
                if (randomSide == 1 || randomSide == 2)
                {
                    positionPlane.Y = random.Next(0, Ctx.WidthArrayMap * Constant.imgSizeMap - 100);
                    randomSide = random.Next(1, 3);
                    if (randomSide == 1) positionPlane.X = 0;
                    else if (randomSide == 2) positionPlane.X = Ctx.WidthArrayMap * Constant.imgSizeMap;
                }
                else if (randomSide == 3)
                {
                    positionPlane.Y = 0;
                    positionPlane.X = random.Next(0, Ctx.HeightArrayMap * Constant.imgSizeMap - 100);
                }
            }
            #endregion
            AirUnit unit = null;
            if (type == PlaneType.PlaneSlow) unit = new AirUnit(this, "Lent", 100, 1000, positionPlane, 2, 0, timer, PlaneType.PlaneSlow);
            else if (type == PlaneType.PlaneNormal) unit = new AirUnit(this, "Normal", 100, 500, positionPlane, 4, 0, timer, PlaneType.PlaneNormal);
            else if (type == PlaneType.PlaneFast) unit = new AirUnit(this, "Rapide", 100, 200, positionPlane, 6, 0, timer, PlaneType.PlaneFast);
            Array.Add(unit);
            return unit;
        }

        public void ResetWaveOfPlane ()
        {
            int nbPlane = Array.Count;
            Array = new List<AirUnit>();
            for (int i = 0; i < nbPlane; i++) CreatePlane(TypeOfPlane, i * 60, Random);
        }

        public bool IsStarting => TimerBeforeStarting <= 0;
    }
}
