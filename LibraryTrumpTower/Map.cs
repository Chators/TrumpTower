﻿using System;
using System.Collections.Generic;
using TrumpTower.LibraryTrumpTower.Constants;
using TrumpTower.LibraryTrumpTower.Spawns;
using Microsoft.Xna.Framework;
using LibraryTrumpTower.SpecialAbilities;
using LibraryTrumpTower.Constants;
using LibraryTrumpTower.AirUnits;
using LibraryTrumpTower;
using System.Runtime.Serialization;

namespace TrumpTower.LibraryTrumpTower
{
    [DataContract(IsReference = true)]
    public class Map
    {
        #region Fields

        public static int TimerNextWave;
        // WAVE
        public static int WavesCounter { get; set; }
        public static int WavesTotals { get; set; }
        public static Wave WaveIsComming { get; set; }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int[][] MapArray { get; private set; }
        [DataMember]
        public List<Spawn> SpawnsEnemies { get; private set; }
        [DataMember]
        public List<Enemy> DeadEnemies { get; set; }
        [DataMember]
        public Wall Wall { get; private set; }
        [DataMember]
        public double Dollars { get; set; }
        [DataMember]
        public int WidthArrayMap { get; private set; }
        [DataMember]
        public int HeightArrayMap { get; private set; }
        [DataMember]
        public List<Tower> Towers { get; private set; }
        [DataMember]
        public List<Missile> Missiles { get; set; }
        [DataMember]
        public List<AirUnitsCollection> AirUnits { get; set; }
        [DataMember]
        public List<AirUnit> DeadUnitsAir { get; set; }

        [DataMember]
        public List<Tower> TowerDisabled;
        // SPECIAL ABILITIES
        [DataMember]
        public Explosion Explosion { get; set; }
        [DataMember]
        public Sniper Sniper { get; set; }

        #endregion

        public Map(int[][] map)
        {
            Name = null;
            MapArray = map;
            WidthArrayMap = map[0].Length;
            HeightArrayMap = map.Length;
            SpawnsEnemies = new List<Spawn>();
            Wall = null;
            Dollars = 200;
            Towers = new List<Tower>();
            Missiles = new List<Missile>();
            Explosion = new Explosion(this);
            Sniper = new Sniper(this);
            DeadEnemies = new List<Enemy>();
            AirUnits = new List<AirUnitsCollection>();
            DeadUnitsAir = new List<AirUnit>();
            TowerDisabled = new List<Tower>();

            WavesCounter = 0;
            WavesTotals = 0;
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

            foreach (AirUnitsCollection unitsCollection in AirUnits) unitsCollection.Update();

            for (int i = 0; i < Missiles.Count; i++)
            {
                Missile myMissile = Missiles[i];
                myMissile.Update();
            }

            Explosion.Update();
            Sniper.Update();

        }

        public int GetTypeArray(int x, int y)
        {
            return MapArray[y][x];
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

        public List<AirUnit> GetAllAirEnemies()
        {
            List<AirUnit> allAirUnits = new List<AirUnit>();
            foreach (AirUnitsCollection airUnitsCollection in AirUnits)
            {
                foreach (AirUnit unit in airUnitsCollection.Array)
                {
                    if (unit.IsStarting) allAirUnits.Add(unit);
                }
            }
            return allAirUnits;
        }

        public List<Vector2> SearchPositionTextureInArray(MapTexture nameTexture)
        {
            List<Vector2> arrayEmptyTower = new List<Vector2>();
            for (int y = 0; y < HeightArrayMap; y++)
            {
                for (int x = 0; x < WidthArrayMap; x++)
                {
                    if (MapArray[y][x] == (int)nameTexture) arrayEmptyTower.Add(new Vector2(x, y));
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
        public void ChangeLocation(int x, int y, int number) => MapArray[y][x] = number;

        public Vector2 SearchCase (int mouseX, int mouseY, int SizePixelMap)
        {
            for (int y = 0; y < WidthArrayMap; y++)
            {
                for (int x = 0; x < WidthArrayMap; x++)
                {
                    // Si on a la souris qui pointe sur un élèment
                    if (mouseX > x * Constant.imgSizeMap && mouseX < x * Constant.imgSizeMap + Constant.imgSizeMap &&
                        mouseY > y * Constant.imgSizeMap && mouseY < y * Constant.imgSizeMap + Constant.imgSizeMap)
                    {
                        return new Vector2(x, y);
                    }
                }
            }
            return Vector2.Zero;
        }

        public void CreateBase(Wall wall)
        {
            Wall = wall;
        }

        public void CreateSpawn(Spawn spawn)
        {
            foreach (Spawn _spawn in SpawnsEnemies)
            {
                if (spawn.Position == _spawn.Position) throw new ArgumentException("Spawn already existing");
            }
            SpawnsEnemies.Add(spawn);
        }

        public void DeleteSpawn(Spawn spawn)
        {
            SpawnsEnemies.Remove(spawn);
        }

        public void DeleteAirWave(AirUnitsCollection unitsCollection)
        {
            AirUnits.Remove(unitsCollection);
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

        public void UseSniperAbility(Vector2 position)
        {
            Sniper.AttackOn(position);
        }

        public void SettingTheMap (string name, int dollars)
        {
            Name = name;
            Dollars = dollars;
        }
    }
}