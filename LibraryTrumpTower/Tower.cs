using LibraryTrumpTower.Constants;
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
        readonly TowerType _type;
        int _lvl;
        int _price;
        public double Scope { get; private set; }
        public int Damage { get;  set; }
        readonly double _attackSpeed;
        double _reload;
        public Vector2 Position { get; private set; }

        public Tower(Map map, TowerType type, int lvl, Vector2 position)
        {
            _map = map;
            _type = type;
            _lvl = lvl;
            _reload = 0;
            Position = position;

            if(type == TowerType.simple)
            {
                Damage = 6;
                Scope = 5;
                _attackSpeed = 0.8;  
            }
            else if(type == TowerType.slow)
            {
                Damage = 19;
                Scope = 3.5;
                _attackSpeed = 1.8;  
            }
            else if(type == TowerType.area)
            {
                Damage = 13;
                Scope = 6.5;
                _attackSpeed = 1.2;
            }
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

        public TowerType Type => _type;

        public int TowerLvl
        {
            get { return _lvl; }
            set
            {
                if (value <= 3)
                {
                    _lvl = value;
                }
                else
                {
                    _lvl = 3;
                }
            }
        }

        public void Upgrade(Tower upgradedTower)
        {
            upgradedTower.TowerLvl++;
            if (upgradedTower.Type == TowerType.simple)
            {
                if (upgradedTower.TowerLvl == 2)
                {
                    upgradedTower.Damage = 13;
                }
                else if (upgradedTower.TowerLvl == 3)
                {
                    upgradedTower.Damage = 20;
                }
            }
            else if (upgradedTower.Type == TowerType.slow)
            {
                if (upgradedTower.TowerLvl == 2)
                {
                    upgradedTower.Damage = 25;

                }
                else if (upgradedTower.TowerLvl == 3)
                {
                    upgradedTower.Damage = 32;

                }
            }
            else if (upgradedTower.Type == TowerType.area)
            {
                if (upgradedTower.TowerLvl == 2)
                {
                    upgradedTower.Damage = 19;

                }
                else if (upgradedTower.TowerLvl == 3)
                {
                    upgradedTower.Damage = 25;

                }
            }
            Console.WriteLine("Upgrade tower successfull !");
        }

            static public int TowerPrice(TowerType type)
        {
            if(type == TowerType.simple)
            {
                return 200;
            }
            else if(type == TowerType.slow)
            {
                return 300;
            }
            else if(type == TowerType.area)
            {
                return 400;
            }
            return 0;
        }

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
            _map.CreateMissile(new Missile(_map, this, Damage, Position, myEnemy));
        }

    }
}
