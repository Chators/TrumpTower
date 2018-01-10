using LibraryTrumpTower;
using LibraryTrumpTower.Spawns.Dijkstra;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower.Constants;

namespace TrumpTower.LibraryTrumpTower.Spawns
{
    [DataContract(IsReference = true)]
    public class Spawn
    {
        #region Fields
        [DataMember]
        public Map Ctx { get; private set; }
        [DataMember]
        public Vector2 Position { get; private set; }
        [DataMember]
        public List<Wave> Waves { get; private set; }
        [DataMember]
        public List<Vector2> ShortestWay { get; set; }
        [DataMember]
        public int[][] MapArray { get { return Ctx.MapArray; } }
        #endregion

        public Spawn(Map ctx, Vector2 position, List<Wave> waves)
        {
            Ctx = ctx;
            Position = position;

            if (waves == null) Waves = new List<Wave>();
            else Waves = waves;

            if (ctx.Wall != null)
                ResetShortestWay();
            else
                ShortestWay = null;
        }

        public void Update()
        {
            foreach (Wave wave in Waves) wave.Update();
        }

        public void CreateWave(Wave wave)
        {
            Waves.Add(wave);
            Map.WavesTotals++;
        }

        public void DeleteWave(int nb)
        {
            Waves.RemoveAt(nb);
            Map.WavesTotals--;
        }

        public Wall Wall => Ctx.Wall;

        public void ResetShortestWay()
        {
            Dictionary<string, User> usersDic = CreateGraph(Ctx.MapArray);
            User userSpawn = null;
            User userWall = null;
            foreach(User user in usersDic.Values)
            {
                if (new Vector2(user._position.X * Constant.imgSizeMap, user._position.Y * Constant.imgSizeMap) == Position) userSpawn = user;
                if (new Vector2(user._position.X * Constant.imgSizeMap, user._position.Y * Constant.imgSizeMap) == Ctx.Wall.Position) userWall = user;
            }
            Vector2 positionWall = Ctx.Wall.Position;
            List<User> usersShortestPosition = userSpawn.OnSFaitUnPtitDijkstra(usersDic, userWall);
            ShortestWay = new List<Vector2>();

            foreach(User user in usersShortestPosition)
                ShortestWay.Add(user._position*new Vector2(Constant.imgSizeMap, Constant.imgSizeMap));

            List<Vector2> optimiseShortestWay = new List<Vector2>();
            optimiseShortestWay.Add(ShortestWay[0]);
            Move lastDirection = Move.none;
            Move newDirection = Move.none;
            for (int i = 0; i < ShortestWay.Count; i++)
            {
                Vector2 position = ShortestWay[i];
                if (position.X > optimiseShortestWay[optimiseShortestWay.Count-1].X) newDirection = Move.down;
                else if (position.X < optimiseShortestWay[optimiseShortestWay.Count-1].X) newDirection = Move.top;
                else if (position.Y > optimiseShortestWay[optimiseShortestWay.Count-1].X) newDirection = Move.right;
                else if (position.Y < optimiseShortestWay[optimiseShortestWay.Count-1].X) newDirection = Move.left;

                if (lastDirection == newDirection)
                    optimiseShortestWay[optimiseShortestWay.Count - 1] = position;
                else
                    optimiseShortestWay.Add(position);
                lastDirection = newDirection;
            }
            ShortestWay = optimiseShortestWay;
        }

        #region pathFinding
        List<User> UsersList { get; set; }

        #region formation graphe

        private Dictionary<string, User> CreateGraph(int[][] mapArray)
        {
            Dictionary<string, User> Users = new Dictionary<string, User>();
            int mdrctropmoche = 0;
            // D'abord on rentre tous les noeuds
            for (int y = 0; y < Ctx.HeightArrayMap; y++)
            {
                for (int x = 0; x < Ctx.WidthArrayMap; x++)
                {
                    if (mapArray[y][x] == (int)MapTexture.dirt)
                        Users[mdrctropmoche.ToString()] = new User(mdrctropmoche + "", "", "", new Vector2(x, y));
                    mdrctropmoche++;
                }
            }

            // Après on les lie ensemble
            foreach (User user in Users.Values)
            //for (int i = 0; i < Users.Count; i++)
            {
                //User user = Users[i];
                Vector2 positionUser = user._position;
                foreach (User userTarget in Users.Values)
                //for (int j = 0; j < Users.Count; j++)
                {
                    if (positionUser + new Vector2(1, 0) == userTarget._position) user.AddRelationship(userTarget);
                    if (positionUser + new Vector2(0, 1) == userTarget._position) user.AddRelationship(userTarget);
                    if (positionUser + new Vector2(-1, 0) == userTarget._position) user.AddRelationship(userTarget);
                    if (positionUser + new Vector2(0, -1) == userTarget._position) user.AddRelationship(userTarget);
                }
            }
            return Users;
        }

        #endregion

        #endregion
    }
}
