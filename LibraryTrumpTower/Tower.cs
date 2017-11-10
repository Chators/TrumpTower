using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower.Constants;

namespace TrumpTower.LibraryTrumpTower
{
    public class Tower
    {
        Map _map;
        readonly string _name;
        readonly int _lvl;
        public double Scope { get; private set; }
        public int Damage { get; private set; }
        readonly double _attackSpeed;
        double _reload;
        public Vector2 Position { get; private set; }

        public Tower(Map map, string name, int lvl, Vector2 position)
        {
            _map = map;
            _name = name;
            _lvl = lvl;
            Damage = 40;
            Scope = 5;
            _attackSpeed = 0.5;
            _reload = 0;
            Position = position;
        }

        private bool UpdateShoot(Enemy myEnemy)
        {
            bool canShoot = WithinReachOf(myEnemy.Position) && IsReload;
            if (canShoot)
            {
                ManagerSound.Explosion.Play();
                ShootOn(myEnemy);
            }
            return canShoot;
        }

        //
        // List enemy for tower shoot
        //
        public void Update(List<Enemy> _enemies)
        {
            bool alreadyShoot = false;
            foreach (Enemy myEnemy in _enemies)
            {
                alreadyShoot = UpdateShoot(myEnemy);
                if (alreadyShoot) break;
            }
            Reloading();
        }

        internal bool IsReload => _reload <= 0;
        internal void Reloading() => _reload--;

        internal bool WithinReachOf(Vector2 target)
        {
            double distanceTower = DistanceOf(target);
            return distanceTower < Scope * Constant.imgSizeMap * Scope * Constant.imgSizeMap;

        }

        private double DistanceOf(Vector2 positionTarget)
        {
            return (positionTarget.X - Position.X) * (positionTarget.X - Position.X) + (positionTarget.Y - Position.Y) * (positionTarget.Y - Position.Y);
        }

        internal void ShootOn(Enemy myEnemy)
        {
            _reload = _attackSpeed * 60;
            _map.CreateMissile(new Missile(_map, "base", Damage, Position, myEnemy));
        }
    }
}
