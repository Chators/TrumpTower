using System;
using System.Collections.Generic;
using TrumpTower.LibraryTrumpTower.Constants;
using TrumpTower.LibraryTrumpTower.Spawns;
using Microsoft.Xna.Framework;
using LibraryTrumpTower.SpecialAbilities;
using LibraryTrumpTower.Constants;

namespace TrumpTower.LibraryTrumpTower
{
    public class Map
    {
        public static int TimerNextWave;

        public int[,] MapArray { get; private set; }
        public List<Spawn> SpawnsEnemies { get; private set; }
        public Wall Wall { get; private set; }
        public double Dollars { get; set; }
        public int WidthArrayMap { get; private set; }
        public int HeightArrayMap { get; private set; }
        public List<Tower> Towers { get; private set; }
        public List<Missile> Missiles { get; set; }
        public List<Enemy> DeadEnemies { get; set; }

        // WAVE
        public static int WavesCounter { get; set; }
        public static int WavesTotals { get; set; }
        public static Wave WaveIsComming { get; set; }

        // SPECIAL ABILITIES
        public Explosion Explosion { get; set; }

        public Map(int[,] map)
        {
            MapArray = map;
            WidthArrayMap = map.GetLength(1);
            HeightArrayMap = map.GetLength(0);
            SpawnsEnemies = new List<Spawn>();
            Wall = new Wall(this, 500000, new Vector2(0 * Constant.imgSizeMap, 10 * Constant.imgSizeMap));
            Dollars = 8888888200;
            Towers = new List<Tower>();
            Missiles = new List<Missile>();
            Explosion = new Explosion(this);
            DeadEnemies = new List<Enemy>();

            WavesCounter = 0;
            WavesTotals = 0;

            //
            // Create Wave TESTTTTTTTTTTTTTTTTTTSSS
            //
            // VAGUE SUR LA PREMIERE ROUTE 

            // CREATE 2 SPAWN
            CreateSpawn(new Spawn(this, new Vector2(28 * Constant.imgSizeMap, 2 * Constant.imgSizeMap), new List<Wave>()));
            CreateSpawn(new Spawn(this, new Vector2(28 * Constant.imgSizeMap, 6 * Constant.imgSizeMap), new List<Wave>()));

            // WAVE 1 FOR TOWER GREEN
            SpawnsEnemies[0].CreateWave(new Wave(SpawnsEnemies[0], new List<Enemy>(), 6 * 60));
            SpawnsEnemies[0].Waves[0].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[0], "base", Wall, EnemyType.defaultSoldier));
            SpawnsEnemies[0].Waves[0].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[0], "base", Wall, EnemyType.defaultSoldier));

            // WAVE 2 FOR TOWER YELLOW
            SpawnsEnemies[0].CreateWave(new Wave(SpawnsEnemies[0], new List<Enemy>(), 15 * 60));
            SpawnsEnemies[0].Waves[1].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[1], "base", Wall, EnemyType.defaultSoldier));
            SpawnsEnemies[0].Waves[1].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[1], "base", Wall, EnemyType.doctor));
            SpawnsEnemies[0].Waves[1].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[1], "base", Wall, EnemyType.defaultSoldier));

            // WAVE 3 FOR EXPLOSION
            SpawnsEnemies[0].CreateWave(new Wave(SpawnsEnemies[0], new List<Enemy>(), 24 * 60));
            SpawnsEnemies[0].Waves[2].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[2], "base", Wall, EnemyType.defaultSoldier));
            SpawnsEnemies[0].Waves[2].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[2], "base", Wall, EnemyType.defaultSoldier));
            SpawnsEnemies[0].Waves[2].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[2], "base", Wall, EnemyType.defaultSoldier));
            SpawnsEnemies[0].Waves[2].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[2], "base", Wall, EnemyType.defaultSoldier));
            SpawnsEnemies[0].Waves[2].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[2], "base", Wall, EnemyType.defaultSoldier));

            // WAVE REAL GAME
            // WAVE 1
            SpawnsEnemies[0].CreateWave(new Wave(SpawnsEnemies[0], new List<Enemy>(), 40 * 60));
            SpawnsEnemies[0].Waves[3].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[3], "base", Wall, EnemyType.defaultSoldier));
            SpawnsEnemies[0].Waves[3].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[3], "base", Wall, EnemyType.defaultSoldier));
            SpawnsEnemies[0].Waves[3].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[3], "base", Wall, EnemyType.defaultSoldier));
            SpawnsEnemies[0].Waves[3].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[3], "base", Wall, EnemyType.kamikaze));
            for (int i = 0; i < 10; i++) SpawnsEnemies[0].Waves[3].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[3], "base", Wall, EnemyType.defaultSoldier));
            // WAVE 2
            SpawnsEnemies[1].CreateWave(new Wave(SpawnsEnemies[1], new List<Enemy>(), 43 * 60));
            SpawnsEnemies[1].Waves[0].CreateEnemy(new Enemy(this, SpawnsEnemies[1].Waves[0], "base", Wall, EnemyType.kamikaze));
            SpawnsEnemies[1].Waves[0].CreateEnemy(new Enemy(this, SpawnsEnemies[1].Waves[0], "base", Wall, EnemyType.defaultSoldier));
            SpawnsEnemies[1].Waves[0].CreateEnemy(new Enemy(this, SpawnsEnemies[1].Waves[0], "base", Wall, EnemyType.defaultSoldier));
            SpawnsEnemies[1].Waves[0].CreateEnemy(new Enemy(this, SpawnsEnemies[1].Waves[0], "base", Wall, EnemyType.defaultSoldier));
            SpawnsEnemies[1].Waves[0].CreateEnemy(new Enemy(this, SpawnsEnemies[1].Waves[0], "base", Wall, EnemyType.defaultSoldier));
            SpawnsEnemies[1].Waves[0].CreateEnemy(new Enemy(this, SpawnsEnemies[1].Waves[0], "base", Wall, EnemyType.kamikaze));
            for (int i = 0; i < 4; i++) SpawnsEnemies[1].Waves[0].CreateEnemy(new Enemy(this, SpawnsEnemies[1].Waves[0], "base", Wall, EnemyType.defaultSoldier));
            // WAVE 3
            SpawnsEnemies[0].CreateWave(new Wave(SpawnsEnemies[0], new List<Enemy>(), 50 * 60));
            for (int i = 0; i < 30; i++) SpawnsEnemies[0].Waves[4].CreateEnemy(new Enemy(this, SpawnsEnemies[0].Waves[4], "base", Wall, EnemyType.kamikaze));
            // WAVE 4
            SpawnsEnemies[1].CreateWave(new Wave(SpawnsEnemies[1], new List<Enemy>(), 54 * 60));
            for (int i = 0; i < 10; i++) SpawnsEnemies[1].Waves[1].CreateEnemy(new Enemy(this, SpawnsEnemies[1].Waves[1], "base", Wall, EnemyType.defaultSoldier));
        }

        public void Update()
        {
            List<Wave> _waves = new List<Wave>();
            foreach (Spawn spawn in SpawnsEnemies)
            {
                spawn.Update();
                _waves.AddRange(spawn.Waves);
            }

            WaveIsComming = null;
            for (int i = 0; i < _waves.Count; i++)
            {
                if (WaveIsComming == null) WaveIsComming = _waves[i];
                else if (WaveIsComming.TimerBeforeStarting == 0 || _waves[i].TimerBeforeStarting < WaveIsComming.TimerBeforeStarting  && _waves[i].TimerBeforeStarting > 0) WaveIsComming = _waves[i];
            }

            foreach (Tower tower in Towers) tower.Update(GetAllEnemies());

            for (int i = 0; i < Missiles.Count; i++)
            {
                Missile myMissile = Missiles[i];
                myMissile.Update();
            }

            Explosion.Update();

        }

        public int GetTypeArray(int x, int y)
        {
            return MapArray[y, x];
        }

        public List<Enemy> GetAllEnemies()
        {
            List<Enemy> allEnemies = new List<Enemy>();
            foreach (Spawn spawn in SpawnsEnemies)
            {
                foreach (Wave wave in spawn.Waves)
                {
                    if (wave.IsStarting) allEnemies.InsertRange(allEnemies.Count, wave.EnemiesVisible());
                }
            }
            return allEnemies;
        }

        public List<Vector2> SearchPositionTextureInArray(MapTexture nameTexture)
        {
            List<Vector2> arrayEmptyTower = new List<Vector2>();
            for (int y = 0; y < HeightArrayMap; y++)
            {
                for (int x = 0; x < WidthArrayMap; x++)
                {
                    if (MapArray[y, x] == (int)nameTexture) arrayEmptyTower.Add(new Vector2(x, y));
                }
            }
            return arrayEmptyTower;
        }

        /// <summary>
        /// Allows to change an element of the map
        /// </summary>
        /// <param name="x">position x</param>
        /// <param name="y">position y</param>
        /// <param name="number">number of texture (example dirt = 0)</param>
        public void ChangeLocation(int x, int y, int number) => MapArray[y, x] = number;

        private void CreateSpawn(Spawn spawn)
        {
            foreach (Spawn _spawn in SpawnsEnemies)
            {
                if (spawn.Position == _spawn.Position) throw new ArgumentException("Spawn already existing");
            }
            SpawnsEnemies.Add(spawn);
        }

        public Tower CreateTower(Tower tower)
        {
            Towers.Add(tower);
            return tower;
        }

        public Missile CreateMissile(Missile missile)
        {
            Missiles.Add(missile);
            return missile;
        }

        public void UseExplosionAbility(Vector2 position)
        {
            Explosion.AttackOn(position);
        }
    }
}