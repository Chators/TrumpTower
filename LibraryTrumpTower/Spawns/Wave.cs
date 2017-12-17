using LibraryTrumpTower.Constants;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.LibraryTrumpTower.Spawns
{
    [DataContract(IsReference = true)]
    public class Wave
    {
        #region Fields
        [DataMember]
        Spawn _spawn;
        [DataMember]
        public List<Enemy> Enemies { get; set; }
        [DataMember]
        public int TimerBeforeStarting { get; set; }
        [DataMember]
        public bool IsStarting { get; private set; }
        #endregion

        public Wave(Spawn spawn, List<Enemy> enemies, int timerBeforeStarting)
        {
            _spawn = spawn ?? throw new ArgumentNullException();
            Enemies = enemies;
            TimerBeforeStarting = timerBeforeStarting;
            IsStarting = false;
        }

        private static readonly Random _shuffleRnd = new Random(); // nécessaire pour la méthode ShuffleArray
        public void UpdateTimerStartingEnemies(List<Enemy> enemies)
        {
            // On mélange les ennemis
            int arrayLength = enemies.Count;
            // parcours de la liste en partant de la fin
            for (int i = arrayLength - 1; i > 1; --i)
            {
                // tirage au sort d'un index entre 0 et la valeur courante de "i"
                int randomIndex = _shuffleRnd.Next(i);
                // intervertion des éléments situés aux index "i" et "randomIndex"
                Enemy temp = enemies[i];
                enemies[i] = enemies[randomIndex];
                enemies[randomIndex] = temp;
            }

            Map.WavesCounter++;
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy myEnemy = enemies[i];
                myEnemy.TimerBeforeStarting = i * 30;
            }
            
            IsStarting = true;
        }

        public void Update()
        {
            if (!IsStarting)
            {
                TimerBeforeStarting--;
                if (TimerBeforeStarting <= 0) UpdateTimerStartingEnemies(Enemies);
            }
            else
            {
                for (int i = 0; i < Enemies.Count; i++)
                {
                    Enemy enemy = Enemies[i];
                    enemy.Update();
                }
            }
        }

        public List<Enemy> EnemiesVisible()
        {
            List<Enemy> enemiesVisible = new List<Enemy>();
            foreach(Enemy enemy in Enemies)
            {
                if (enemy.IsStarting) enemiesVisible.Add(enemy);
            }
            return enemiesVisible;
        }
        
        public void CreateEnemies(EnemyType enemyType, int number)
        {
            for (int i = 0; i < number; i++)
                Enemies.Add(new Enemy(_spawn.Ctx, this, "lol", enemyType));
        }

        public void DeleteEnemies(EnemyType enemyType, List<Enemy> enemyAtDelete)
        {
            for (int i = 0; i < enemyAtDelete.Count; i++)
                Enemies.Remove(enemyAtDelete[i]);
        }

        public Vector2 Position => _spawn.Position;
        public List<Vector2> ShortestWay => _spawn.ShortestWay;

        public Dictionary<EnemyType, List<Enemy>> GetAllEnemiesByType ()
        {
            // Init
            Dictionary<EnemyType, List<Enemy>> DicUnits = new Dictionary<EnemyType, List<Enemy>>();
            foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
                DicUnits[enemyType] = new List<Enemy>();

            // Process
            foreach (Enemy enemy in Enemies)
                DicUnits[enemy._type].Add(enemy);

            return DicUnits;
        }

        public Wall Wall => _spawn.Wall;
    }
}
