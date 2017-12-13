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
        public int TimerBeforeStarting { get; private set; }
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

        public void UpdateTimerStartingEnemies(List<Enemy> enemies)
        {
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
        
        public void CreateEnemy(Enemy enemy)
        {
            Enemies.Add(enemy);
        }

        public Vector2 Position => _spawn.Position;
        public List<Vector2> ShortestWay => _spawn.ShortestWay;
    }
}
