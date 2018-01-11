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
    public class Explosion
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
        public double Damage { get; private set; }
        [DataMember]
        private Vector2 Position { get; set; }
        #endregion

        public Explosion (Map ctx)
        {
            _ctx = ctx;
            Cooldown = Constant.EXPLOSION_COOLDOWN;
            CurrentTimer = 0;
            Radius = Constant.EXPLOSION_RADIUS;
            Damage = Constant.EXPLOSION_DAMAGE;
            Position = new Vector2(-1000, -1000);
        }

        private void UpdateAttackEnemies(List<Enemy> enemies)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.TakeHp(Damage);
                if (enemy.IsDead) enemy.Die();
            }
            CurrentTimer = Cooldown;
            Position = new Vector2(-1000, -1000);
        }

        public void Update()
        {
            if (IsActivate && IsReloaded) UpdateAttackEnemies(GetEnemies(Position, Radius));
            if (!IsReloaded) CurrentTimer--;
        }

        public void AttackOn(Vector2 position) => Position = position;

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
