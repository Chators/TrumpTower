using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower.Constants;

namespace TrumpTower.LibraryTrumpTower.Spawns
{
    public class Spawn
    {
        Map _ctx;
        public Vector2 Position { get; private set; }
        public List<Wave> Waves { get; private set; }
        public List<Vector2> ShortestWay { get; private set; }
        public int[,] MapArray { get { return _ctx.MapArray; } }

        public Spawn(Map ctx, Vector2 position, List<Wave> waves)
        {
            _ctx = ctx;
            Position = position;

            if (waves == null) Waves = new List<Wave>();
            else Waves = waves;

            ShortestWay = SeekShortestWay(_ctx.MapArray, _ctx.Wall, Position);
        }

        public void Update()
        {
            foreach (Wave wave in Waves) wave.Update();
        }

        /*public Wave SeekWaveIsComming()
        {
            Wave _isComming = null;
            foreach (Wave wave in Waves)
            {
                if (_isComming == null || 
                    wave.TimerBeforeStarting > 0 &&
                    wave.TimerBeforeStarting < _isComming.TimerBeforeStarting) _isComming = wave;
            }
            return _isComming;
        }*/

        public void CreateWave(Wave wave)
        {
            Waves.Add(wave);
            Map.WavesTotals++;
        }

        #region pathFinding
        private List<Vector2> SeekShortestWay(int[,] mapArray, Wall wall, Vector2 currentPosition, Move lastDirection = Move.none)
        {
            List<Vector2> _shortestWay = new List<Vector2>();

            while (!_shortestWay.Contains(wall.Position))
            {
                // On détermine les directions possible
                List<Move> possiblesDirections = SeekClosePossiblesDirections(mapArray, currentPosition, lastDirection);

                // Si on a plus d'une possibilité
                if (possiblesDirections.Count > 1)
                {
                    _shortestWay.Add(currentPosition);
                    for (int i = 0; i < possiblesDirections.Count; i++)
                    {
                        Vector2 tryPosition = new Vector2(currentPosition.X, currentPosition.Y);
                        if (possiblesDirections[i] == Move.down) tryPosition.Y += Constant.imgSizeMap;
                        if (possiblesDirections[i] == Move.top) tryPosition.Y -= Constant.imgSizeMap;
                        if (possiblesDirections[i] == Move.right) tryPosition.X += Constant.imgSizeMap;
                        if (possiblesDirections[i] == Move.left) tryPosition.X -= Constant.imgSizeMap;

                        List<Vector2> tryPath = SeekShortestWay(mapArray, wall, tryPosition, possiblesDirections[i]);
                        if (tryPath != null) _shortestWay.AddRange(tryPath);
                    }
                }
                else if (possiblesDirections.Count == 1)
                {
                    // On enregistre la bonne direction
                    Move goodDirection = possiblesDirections[0];
                    // Si on change de direction on enregistre dans la tableau
                    if (lastDirection != goodDirection) _shortestWay.Add(currentPosition);
                    // On avance le curseur
                    if (goodDirection == Move.down) currentPosition.Y += Constant.imgSizeMap;
                    if (goodDirection == Move.top) currentPosition.Y -= Constant.imgSizeMap;
                    if (goodDirection == Move.right) currentPosition.X += Constant.imgSizeMap;
                    if (goodDirection == Move.left) currentPosition.X -= Constant.imgSizeMap;

                    lastDirection = goodDirection;
                }
                else
                {
                    if (currentPosition == wall.Position)
                    {
                        _shortestWay.Add(currentPosition);
                        return _shortestWay;
                    }
                    return null;
                }

            }

            return _shortestWay;
        }

        private List<Move> SeekClosePossiblesDirections(int[,] mapArray, Vector2 currentPosition, Move lastPosition)
        {
            int X = (int)currentPosition.X / Constant.imgSizeMap;
            int Y = (int)currentPosition.Y / Constant.imgSizeMap;
            List<Move> possiblesDirections = new List<Move>();

            if (Y + 1 < mapArray.GetLength(0) && mapArray[Y + 1, X] == (int)MapTexture.dirt && lastPosition != Move.top) possiblesDirections.Add(Move.down);
            if (Y - 1 >= 0 && mapArray[Y - 1, X] == (int)MapTexture.dirt && lastPosition != Move.down) possiblesDirections.Add(Move.top);
            if (X + 1 < mapArray.GetLength(1) && mapArray[Y, X + 1] == (int)MapTexture.dirt && lastPosition != Move.left) possiblesDirections.Add(Move.right);
            if (X - 1 >= 0 && mapArray[Y, X - 1] == (int)MapTexture.dirt && lastPosition != Move.right) possiblesDirections.Add(Move.left);

            return possiblesDirections;
        }

        #endregion
    }
}
