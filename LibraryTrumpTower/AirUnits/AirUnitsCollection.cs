using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;

namespace LibraryTrumpTower.AirUnits
{
    public class AirUnitsCollection
    {
        public Map Ctx { get; private set; }
        public List<AirUnit> Array { get; private set; }
        private int TimerBeforeStarting { get; set; }
        public Random Random { get; private set; }

        public AirUnitsCollection (Map ctx, int timerBeforeStarting, int numbersPlane)
        {
            Ctx = ctx;
            Array = new List<AirUnit>();
            TimerBeforeStarting = timerBeforeStarting;

            Random = new Random();
            for (int i = 0; i < numbersPlane; i++) CreatePlane(PlaneType.PlaneFast, i*60, Random);
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
            else if (Ctx.Wall.Position.X == Ctx.WidthArrayMap * Constant.imgSizeMap)
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
            else if (Ctx.Wall.Position.Y == Ctx.HeightArrayMap * Constant.imgSizeMap)
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
            if (type == PlaneType.PlaneSlow) unit = new AirUnit(Array, Ctx.Wall, 100, 1000, positionPlane, 3, 0, timer);
            else if (type == PlaneType.PlaneNormal) unit = new AirUnit(Array, Ctx.Wall, 100, 500, positionPlane, 5, 0, timer);
            else if (type == PlaneType.PlaneFast) unit = new AirUnit(Array, Ctx.Wall, 100, 200, positionPlane, 8, 0, timer);
            Array.Add(unit);
            return unit;
        }

        public bool IsStarting => TimerBeforeStarting <= 0;
    }
}
