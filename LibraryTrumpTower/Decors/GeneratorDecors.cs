using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower.Constants;

namespace LibraryTrumpTower.Decors
{
    public static class GeneratorDecors
    {
        public static List<Decor> Generate(int[][] map)
        {
            List<Decor> decors = new List<Decor>();
            int mapHeight = map.Length;
            int mapWidth = map[0].Length;
            int numberOfDecors = mapHeight + mapWidth;

            Random rdn = new Random();
            int x, x1;
            int y, y1;
            int textureOfDecor;
            int count = 0;

            while (count < numberOfDecors)
            {
                x = rdn.Next(0, mapWidth);
                y = rdn.Next(0, mapHeight);

                if (map[y][x] != (int)MapTexture.dirt &&
                    map[y][x] != (int)MapTexture.emptyTower)
                {
                    x1 = rdn.Next(0, Constant.imgSizeMap);
                    y1 = rdn.Next(0, Constant.imgSizeMap);
                    textureOfDecor = rdn.Next(1, 8);
                    decors.Add(new Decor(textureOfDecor, new Vector2((x*Constant.imgSizeMap)-Constant.imgSizeMap+ x1, (y * Constant.imgSizeMap) - Constant.imgSizeMap + y1)));
                    count++;
                }
            }

            return decors;
        }
    }
}
