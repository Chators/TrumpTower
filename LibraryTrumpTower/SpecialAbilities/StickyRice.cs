using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;

namespace LibraryTrumpTower.SpecialAbilities
{
    [DataContract(IsReference = true)]
    public class StickyRice
    {
        #region Fields
        [DataMember]
        Map _ctx;
        [DataMember]
        public int Cooldown { get; private set; }
        [DataMember]
        public int CurrentTimer { get; private set; }
        [DataMember]
        public double Radius { get; private set; }
        [DataMember]
        public double SpeedReduceInPercent { get; private set; }
        [DataMember]
        public List<Enemy> AffectedEnemies { get; set; }
        [DataMember]
        public Vector2 Position { get; set; }
        [DataMember]
        public int InProgress { get; set; }
        [DataMember]
        public Vector2 PositionPlaneOfRice;
        [DataMember]
        public bool PlaneIsClose { get; set; }
        [DataMember]
        public float SpeedPlane { get; set; }

        #endregion

        public StickyRice(Map ctx)
        {
            _ctx = ctx;
            Cooldown = 25 * 60;
            CurrentTimer = 0;
            Radius = 1200;
            SpeedReduceInPercent = 50;
            SpeedPlane = 12f;
            Position = new Vector2(-1000, -1000);
            PositionPlaneOfRice = new Vector2(-1000, -1000);
            PlaneIsClose = false;
            AffectedEnemies = new List<Enemy>();
        }

        private void UpdateSlowEnemies(List<Enemy> enemies)
        {
            InProgress--;

            for (int i = 0; i < AffectedEnemies.Count; i++)
            {
                Enemy enemy = AffectedEnemies[i];
                if (enemy.IsDead) AffectedEnemies.Remove(enemy);
                else enemy.Speed = enemy.DefaultSpeed;
            }

            AffectedEnemies = new List<Enemy>();

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];
                double reduce = (enemy.Speed * SpeedReduceInPercent / 100);
                enemy.Speed = reduce;
                AffectedEnemies.Add(enemy);
            }

            if (InProgress <= 0)
            {
                for (int i = 0; i < AffectedEnemies.Count; i++)
                {
                    Enemy enemy = AffectedEnemies[i];
                    enemy.Speed = enemy.DefaultSpeed;
                }
                AffectedEnemies = new List<Enemy>();
                Position = new Vector2(-1000, -1000);
            }
        }

        public void Update()
        {
            // Si on le lance
            if (IsActivate && IsReloaded && InProgress <= 0)
            {
                PositionPlaneOfRice = new Vector2(0, Position.Y);
                CurrentTimer = Cooldown;
                InProgress = 10 * 60;
                ManagerSound.PlayPlaneTurbo();
            }
            if (IsActivate)
            {
                // Si l'avion est parti
                if (PositionPlaneOfRice != new Vector2(-1000, -1000))
                {
                    PositionPlaneOfRice.X += SpeedPlane;
                    if (!PlaneIsClose && Vector2.Distance(PositionPlaneOfRice, Position) < SpeedPlane)
                    {
                        PlaneIsClose = true;
                        ManagerSound.PlayRiceSplash();
                    }
                }
                if (PlaneIsClose)
                    UpdateSlowEnemies(GetEnemies(Position, Radius));

            }
            // On recharge
            if (!IsReloaded) CurrentTimer--;
        }

        public void On(Vector2 position) => Position = position;

        private List<Enemy> GetEnemies(Vector2 position, double radius)
        {
            List<Enemy> _enemies = new List<Enemy>();
            List<Enemy> _currentEnemies = _ctx.GetAllEnemies();

            foreach (Enemy enemy in _currentEnemies)
            {
                if (WithinReach(position, enemy.Position, radius)) _enemies.Add(enemy);
            }

            return _enemies;
        }

        private bool WithinReach(Vector2 myPosition, Vector2 target, double radius)
        {
            double distanceEnemy = DistanceOf(myPosition, target);
            return distanceEnemy < radius * Constant.imgSizeMap;
        }

        private double DistanceOf(Vector2 myPosition, Vector2 target)
        {
            return (target.X - myPosition.X) * (target.X - myPosition.X) + (target.Y - myPosition.Y) * (target.Y - myPosition.Y);
        }

        public bool IsReloaded => CurrentTimer <= 0;
        public bool IsActivate => Position != new Vector2(-1000, -1000);
    }
}
