using LibraryTrumpTower;
using LibraryTrumpTower.Constants;
using LibraryTrumpTower.Constants.BalanceGame.Towers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower.Constants;

namespace TrumpTower.LibraryTrumpTower
{
    [DataContract(IsReference = true)]
    public class Tower
    {
        [DataMember]
        Map _map;
        [DataMember]
        readonly TowerType _type;
        [DataMember]
        float _rotate;
        [DataMember]
        int _lvl;
        [DataMember]
        public double _reload;
        [DataMember]
        public Vector2 Position { get; private set; }
        [DataMember]
        public bool _isDisabled { get; set; }
        [DataMember]
        public bool _isCasted { get; set; }

        public double Scope
        {
            get
            {
                if (_type == TowerType.simple) return BalanceTowerSimple.TOWER_SIMPLE_SCOPE;
                else if (_type == TowerType.slow) return BalanceTowerSlow.TOWER_SLOW_SCOPE;
                else if (_type == TowerType.area) return BalanceTowerArea.TOWER_AREA_SCOPE;
                else if (_type == TowerType.bank) return BalanceTowerBank.TOWER_BANK_SCOPE;
                else return 0;
            }
            /*private set
            {
                if (_type == TowerType.simple) BalanceTowerSimple.TOWER_SIMPLE_SCOPE = value;
                else if (_type == TowerType.slow) BalanceTowerSlow.TOWER_SLOW_SCOPE = value;
                else if (_type == TowerType.area) BalanceTowerArea.TOWER_AREA_SCOPE = value;
                else if (_type == TowerType.bank) BalanceTowerBank.TOWER_BANK_SCOPE = value;
            }*/
        }
        public int Damage
        {
            get
            {
                if (_type == TowerType.simple) return BalanceTowerSimple.TOWER_SIMPLE_DAMAGE;
                else if (_type == TowerType.slow) return BalanceTowerSlow.TOWER_SLOW_DAMAGE;
                else if (_type == TowerType.area) return BalanceTowerArea.TOWER_AREA_DAMAGE;
                else if (_type == TowerType.bank) return BalanceTowerBank.TOWER_BANK_DAMAGE;
                else return 0;
            }
            /*private set
            {
                if (_type == TowerType.simple) BalanceTowerSimple.TOWER_SIMPLE_DAMAGE = value;
                else if (_type == TowerType.slow) BalanceTowerSlow.TOWER_SLOW_DAMAGE = value;
                else if (_type == TowerType.area) BalanceTowerArea.TOWER_AREA_DAMAGE = value;
                else if (_type == TowerType.bank) BalanceTowerBank.TOWER_BANK_DAMAGE = value;
            }*/
        }
        public double _attackSpeed
        {
            get
            {
                if (_type == TowerType.simple) return BalanceTowerSimple.TOWER_SIMPLE_ATTACK_SPEED;
                else if (_type == TowerType.slow) return BalanceTowerSlow.TOWER_SLOW_ATTACK_SPEED;
                else if (_type == TowerType.area) return BalanceTowerArea.TOWER_AREA_ATTACK_SPEED;
                else if (_type == TowerType.bank) return BalanceTowerBank.TOWER_BANK_ATTACK_SPEED;
                else return 0;
            }
            /*private set
            {
                if (_type == TowerType.simple) BalanceTowerSimple.TOWER_SIMPLE_ATTACK_SPEED = value;
                else if (_type == TowerType.slow) BalanceTowerSlow.TOWER_SLOW_ATTACK_SPEED = value;
                else if (_type == TowerType.area) BalanceTowerArea.TOWER_AREA_ATTACK_SPEED = value;
                else if (_type == TowerType.bank) BalanceTowerBank.TOWER_BANK_ATTACK_SPEED = value;
            }*/
        }
        public double Earnings
        {
            get
            {
                if (_type == TowerType.simple) return 0;
                else if (_type == TowerType.slow) return 0;
                else if (_type == TowerType.area) return 0;
                else if (_type == TowerType.bank) return BalanceTowerBank.TOWER_BANK_EARNINGS_MONEY;
                else return 0;
            }
            /*private set
            {
                if (_type == TowerType.bank) BalanceTowerBank.TOWER_BANK_EARNINGS_MONEY = value;
            }*/
        }

        public Tower(Map map, TowerType type, int lvl, Vector2 position)
        {
            _map = map;
            _type = type;
            _lvl = lvl;
            Position = position;
            _rotate = 0;
            Earnings = 0;
            ManagerSound.PlayBuild();
            _isDisabled = false;
            if(type == TowerType.bank) _reload = BalanceTowerBank.TOWER_BANK_RELOADING;
            else _reload = 0;
        }

        private void UpdateShoot(Enemy myEnemy)
        {
            ManagerSound.PlayTowerShoot();
            ShootOn(myEnemy);
        }
        

        //
        // List enemy for tower shoot
        //
        public void Update(List<Enemy> _enemies)
        {
            if (IsReload) _isDisabled = false;

            foreach (Enemy myEnemy in _enemies)
            {
                /* Algo de visée */
                double hpEnemy = myEnemy.CurrentHp;
                foreach (Missile missile in _map.Missiles)
                {
                    if (missile.Target == myEnemy) hpEnemy -= missile.Damage;
                }
                

                if (WithinReachOf(myEnemy.Position) && hpEnemy > 0)
                { 
                    if (!IsDisabled)
                        SetRotate(new Vector2(Position.X + 32, Position.Y + 32), new Vector2(myEnemy.Position.X + 32, myEnemy.Position.Y + 32));
                    if (IsReload)
                        UpdateShoot(myEnemy); 
                    break;
                }
            }
            Reloading();
        }
        
        public void SetRotate (Vector2 _position, Vector2 _targetPosition)
        {
            Vector2 direction = _position - _targetPosition;
            direction.Normalize();
            _rotate = (float)Math.Atan2(-direction.X, direction.Y);
        }

        internal bool IsReload => _reload <= 0;
        internal void Reloading() => _reload--;

        public TowerType Type => _type;

        

        public double Reload
        {
            get { return _reload; }
            set { _reload = value; }
        }

        public int TowerLvl
        {
            get { return _lvl; }
            set
            {
                if (value <= 3) _lvl = value;
                else _lvl = 3;
            }
        }

        public void Sell(Tower tower)
        {
            if (tower.Type == TowerType.simple)
            {
                double initialPrice = Tower.TowerPrice(TowerType.simple)/2;
                if (tower.TowerLvl == 1) _map.Dollars += initialPrice;
                else if (tower.TowerLvl == 2) _map.Dollars += 1.5*initialPrice + initialPrice;
                else if (tower.TowerLvl == 3) _map.Dollars += 3 * initialPrice + initialPrice;
            }
            else if (tower.Type == TowerType.slow)
            {
                double initialPrice = Tower.TowerPrice(TowerType.slow)/2;
                if (tower.TowerLvl == 1) _map.Dollars += initialPrice;
                else if (tower.TowerLvl == 2) _map.Dollars += 1.5 * initialPrice + initialPrice;
                else if (tower.TowerLvl == 3) _map.Dollars += 3 * initialPrice + initialPrice;
            }
            else if (tower.Type == TowerType.area)
            {
                double initialPrice = Tower.TowerPrice(TowerType.area)/2;
                if (tower.TowerLvl == 1) _map.Dollars += initialPrice;
                else if (tower.TowerLvl == 2) _map.Dollars += 1.5 * initialPrice + initialPrice;
                else if (tower.TowerLvl == 3) _map.Dollars += 3 * initialPrice + initialPrice;
            }
            else if (tower.Type == TowerType.bank)
            {
                double initialPrice = Tower.TowerPrice(TowerType.bank) / 2;
                if (tower.TowerLvl == 1) _map.Dollars += initialPrice;
            }
        }
        public void Upgrade(Tower upgradedTower)
        {
            if (upgradedTower.TowerLvl != 3)
            {

                upgradedTower.TowerLvl++;
                if (upgradedTower.Type == TowerType.simple)
                {
                    if (upgradedTower.TowerLvl == 2)
                    {
                        upgradedTower.Damage = 13;
                        _map.Dollars -= Tower.TowerPrice(upgradedTower.Type) * 1.5;

                    }
                    else if (upgradedTower.TowerLvl == 3)
                    {
                        upgradedTower.Damage = 20;
                        _map.Dollars -= Tower.TowerPrice(upgradedTower.Type) * 1.5;

                    }

                }
                else if (upgradedTower.Type == TowerType.slow)
                {
                    if (upgradedTower.TowerLvl == 2)
                    {
                        upgradedTower.Damage = 25;
                        _map.Dollars -= Tower.TowerPrice(upgradedTower.Type) * 1.5;

                    }
                    else if (upgradedTower.TowerLvl == 3)
                    {
                        upgradedTower.Damage = 32;
                        _map.Dollars -= Tower.TowerPrice(upgradedTower.Type) * 1.5;

                    }
                }
                else if (upgradedTower.Type == TowerType.area)
                {
                    if (upgradedTower.TowerLvl == 2)
                    {
                        upgradedTower.Damage = 19;
                        _map.Dollars -= Tower.TowerPrice(upgradedTower.Type) * 1.5;

                    }
                    else if (upgradedTower.TowerLvl == 3)
                    {
                        upgradedTower.Damage = 25;
                        _map.Dollars -= Tower.TowerPrice(upgradedTower.Type) * 1.5;

                    }
                }
                else if (upgradedTower.Type == TowerType.bank)
                {
                    if(upgradedTower.TowerLvl == 2)
                    {
                        upgradedTower.Earnings = 300;
                        _map.Dollars -= Tower.TowerPrice(upgradedTower.Type) * 1.5;
                    }
                    else if(upgradedTower.TowerLvl == 3)
                    {
                        upgradedTower.Earnings = 400;
                        _map.Dollars -= Tower.TowerPrice(upgradedTower.Type) * 1.5;
                    }
                }
            }
           
        }

        static public int TowerPrice(TowerType type)
        {
            if (type == TowerType.simple) return 200;
            else if (type == TowerType.slow) return 300;
            else if (type == TowerType.area) return 400;
            else if (type == TowerType.bank) return 400;
            return 0;
        }

        internal bool WithinReachOf(Vector2 target)
        {
            double distanceTower = DistanceOf(target);
            return distanceTower < Scope * Constant.imgSizeMap;

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

        public float Angle => _rotate;

        public bool IsDisabled
        {
            get{ return _isDisabled; }
            set{ _isDisabled = value; }
        }

        
        public bool IsCasted
        {
            get { return _isCasted; }
            set { _isCasted = value; }
        }
       
    }

       
}
