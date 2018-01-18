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
        }
        public int Damage
        {
            get
            {
                if (_type == TowerType.simple)
                {
                    if (_lvl == 1) return BalanceTowerSimple.TOWER_SIMPLE_DAMAGE;
                    else if (_lvl == 2) return BalanceTowerSimple.TOWER2_SIMPLE_DAMAGE;
                    else if (_lvl == 3) return BalanceTowerSimple.TOWER3_SIMPLE_DAMAGE;
                    return 0;
                }
                else if (_type == TowerType.slow)
                {
                    if (_lvl == 1) return BalanceTowerSlow.TOWER_SLOW_DAMAGE;
                    else if (_lvl == 2) return BalanceTowerSlow.TOWER2_SLOW_DAMAGE;
                    else if (_lvl == 3) return BalanceTowerSlow.TOWER3_SLOW_DAMAGE;
                    return 0;
                }
                else if (_type == TowerType.area)
                {
                    if (_lvl == 1) return BalanceTowerArea.TOWER_AREA_DAMAGE;
                    else if (_lvl == 2) return BalanceTowerArea.TOWER2_AREA_DAMAGE;
                    else if (_lvl == 3) return BalanceTowerArea.TOWER3_AREA_DAMAGE;
                    return 0;
                }
                else if (_type == TowerType.bank) return BalanceTowerBank.TOWER_BANK_DAMAGE;
                else return 0;
            }
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
        }
        public double Earnings
        {
            get
            {
                if (_type == TowerType.simple) return 0;
                else if (_type == TowerType.slow) return 0;
                else if (_type == TowerType.area) return 0;
                else if (_type == TowerType.bank)
                {
                    if (_lvl == 1) return BalanceTowerBank.TOWER_BANK_EARNINGS_MONEY;
                    else if (_lvl == 2) return BalanceTowerBank.TOWER2_BANK_EARNINGS_MONEY;
                    else if (_lvl == 3) return BalanceTowerBank.TOWER3_BANK_EARNINGS_MONEY;
                    return 0;
                }
                else return 0;
            }
        }

        public Tower(Map map, TowerType type, int lvl, Vector2 position)
        {
            _map = map;
            _type = type;
            _lvl = lvl;
            Position = position;
            _rotate = 0;
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
                if (tower.TowerLvl == 1) _map.Dollars += BalanceTowerSimple.TOWER_SIMPLE_SELL;
                else if (tower.TowerLvl == 2) _map.Dollars += BalanceTowerSimple.TOWER2_SIMPLE_SELL;
                else if (tower.TowerLvl == 3) _map.Dollars += BalanceTowerSimple.TOWER3_SIMPLE_SELL;
            }
            else if (tower.Type == TowerType.slow)
            {
                if (tower.TowerLvl == 1) _map.Dollars += BalanceTowerSlow.TOWER_SLOW_SELL;
                else if (tower.TowerLvl == 2) _map.Dollars += BalanceTowerSlow.TOWER2_SLOW_SELL;
                else if (tower.TowerLvl == 3) _map.Dollars += BalanceTowerSlow.TOWER3_SLOW_SELL;
            }
            else if (tower.Type == TowerType.area)
            {
                if (tower.TowerLvl == 1) _map.Dollars += BalanceTowerArea.TOWER_AREA_SELL;
                else if (tower.TowerLvl == 2) _map.Dollars += BalanceTowerArea.TOWER2_AREA_SELL;
                else if (tower.TowerLvl == 3) _map.Dollars += BalanceTowerArea.TOWER3_AREA_SELL;
            }
            else if (tower.Type == TowerType.bank)
            {
                if (tower.TowerLvl == 1) _map.Dollars += BalanceTowerBank.TOWER_BANK_SELL;
                else if (tower.TowerLvl == 2) _map.Dollars += BalanceTowerBank.TOWER2_BANK_SELL;
                else if (tower.TowerLvl == 3) _map.Dollars += BalanceTowerBank.TOWER3_BANK_SELL;
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
                        _map.Dollars -= BalanceTowerSimple.TOWER_SIMPLE_PRICE;
                    else if (upgradedTower.TowerLvl == 3)
                        _map.Dollars -= BalanceTowerSimple.TOWER_SIMPLE_PRICE;

                }
                else if (upgradedTower.Type == TowerType.slow)
                {
                    if (upgradedTower.TowerLvl == 2)
                        _map.Dollars -= BalanceTowerSlow.TOWER_SLOW_PRICE;
                    else if (upgradedTower.TowerLvl == 3)
                        _map.Dollars -= BalanceTowerSlow.TOWER_SLOW_PRICE;
                }
                else if (upgradedTower.Type == TowerType.area)
                {
                    if (upgradedTower.TowerLvl == 2)
                        _map.Dollars -= BalanceTowerArea.TOWER_AREA_PRICE;
                    else if (upgradedTower.TowerLvl == 3)
                        _map.Dollars -= BalanceTowerArea.TOWER_AREA_PRICE;
                }
                else if (upgradedTower.Type == TowerType.bank)
                {
                    if (upgradedTower.TowerLvl == 2)
                        _map.Dollars -= BalanceTowerBank.TOWER_BANK_PRICE;
                    else if (upgradedTower.TowerLvl == 3)
                        _map.Dollars -= BalanceTowerBank.TOWER_BANK_PRICE;
                }
            }
           
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
