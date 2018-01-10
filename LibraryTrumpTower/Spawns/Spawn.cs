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
            {
                ResetShortestWay();
                //ShortestWay = SeekShortestWay(Ctx.MapArray, Ctx.Wall, Position);
            }
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
            //ShortestWay = SeekShortestWay(Ctx.MapArray, Ctx.Wall, Position);
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
        #region AVANT
        /*public List<Vector2> SeekShortestWay(int[][] mapArray, Wall wall, Vector2 currentPosition, Move lastDirection = Move.none, List<Vector2> visited = null)
        {
            List<Vector2> _shortestWay = new List<Vector2>();
            if (visited == null) visited = new List<Vector2>();

            while (!_shortestWay.Contains(wall.Position))
            {
                // On détermine les directions possible
                List<Move> possiblesDirections = SeekClosePossiblesDirections(mapArray, currentPosition, lastDirection, visited);

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

        private List<Move> SeekClosePossiblesDirections(int[][] mapArray, Vector2 currentPosition, Move lastPosition, List<Vector2> visited)
        {
            int X = (int)currentPosition.X / Constant.imgSizeMap;
            int Y = (int)currentPosition.Y / Constant.imgSizeMap;
            List<Move> possiblesDirections = new List<Move>();
            bool downAlreadyVisited = IsVisitedCase(visited, new Vector2(currentPosition.X, currentPosition.Y+Constant.imgSizeMap));
            bool topAlreadyVisited = IsVisitedCase(visited, new Vector2(currentPosition.X, currentPosition.Y-Constant.imgSizeMap));
            bool rightAlreadyVisited = IsVisitedCase(visited, new Vector2(currentPosition.X + Constant.imgSizeMap, currentPosition.Y));
            bool leftAlreadyVisited = IsVisitedCase(visited, new Vector2(currentPosition.X - Constant.imgSizeMap, currentPosition.Y));

            if (!downAlreadyVisited && Y + 1 < mapArray.GetLength(0) && mapArray[Y + 1][X] == (int)MapTexture.dirt && lastPosition != Move.top) possiblesDirections.Add(Move.down);
            if (!topAlreadyVisited && Y - 1 >= 0 && mapArray[Y - 1][X] == (int)MapTexture.dirt && lastPosition != Move.down) possiblesDirections.Add(Move.top);
            if (!rightAlreadyVisited && X + 1 < mapArray.GetLength(0) && mapArray[Y][X + 1] == (int)MapTexture.dirt && lastPosition != Move.left) possiblesDirections.Add(Move.right);
            if (!leftAlreadyVisited && X - 1 >= 0 && mapArray[Y][X - 1] == (int)MapTexture.dirt && lastPosition != Move.right) possiblesDirections.Add(Move.left);

            return possiblesDirections;
        }

        private bool IsVisitedCase(List<Vector2> visited, Vector2 targetPosition)
        {
            foreach (Vector2 hasVisited in visited)
            {
                if (hasVisited == targetPosition) return true;
            }
            return false;
        }
        /*public int InContactWith(User targetUser, List<User> notVisited, List<User> notVisitedMoreDepth, List<User> visited, int depthLevel = 0)
        {
            User userCandidat;

            // On retire le sommet des non visités et on le rajoute dans visité
            notVisited.Remove(this);
            visited.Add(this);

            // On analyse toutes les arrêtes pour voir si il n'y a pas le sommet cible
            foreach (Relationship relationship in CircleOfRelationships.Values)
            {
                userCandidat = (relationship.Users[0] == this) ? relationship.Users[1] : relationship.Users[0];
                bool isUnknown = !notVisited.Contains(userCandidat) && !visited.Contains(userCandidat) && !notVisitedMoreDepth.Contains(userCandidat);
                if (isUnknown) notVisitedMoreDepth.Add(userCandidat);
                // Les personnes sont en contact
                if (userCandidat == targetUser) return depthLevel + 1;
            }

            // Les personnes ne sont pas en contact
            if (notVisited.Count == 0 && notVisitedMoreDepth.Count == 0) return 0;
            // On continue à chercher
            else
            {
                if (notVisited.Count == 0 && notVisitedMoreDepth.Count > 0)
                {
                    notVisited = notVisitedMoreDepth;
                    notVisitedMoreDepth = new List<User>();
                    depthLevel++;
                }
                return notVisited[0].InContactWith(targetUser, notVisited, notVisitedMoreDepth, visited, depthLevel);
            } 
        }*/
        #endregion

        #endregion
    }
}
