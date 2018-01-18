using TrumpTower.LibraryTrumpTower.Constants;
using TrumpTower.LibraryTrumpTower.Spawns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using LibraryTrumpTower.Constants;
using LibraryTrumpTower;
using System.Runtime.Serialization;
using LibraryTrumpTower.Constants.BalanceGame.Enemies;

namespace TrumpTower.LibraryTrumpTower
{
    [DataContract(IsReference = true)]
    public class Enemy
    {
        [DataMember]
        readonly Map _map;
        [DataMember]
        bool Initiliaze { get; set; } // for serialization
        [DataMember]
        readonly Wave _wave;
        [DataMember]
        Vector2 _position;
        [DataMember]
        string _name;
        [DataMember]
        int _moveToState;
        [DataMember]
        public readonly EnemyType _type;
        [DataMember]
        public Move CurrentDirection { get; private set; }       
        [DataMember]
        public int TimerBeforeStarting { get; set; }
        [DataMember]
        public bool _hasCast;
        [DataMember]
        public bool _isCasting;
        [DataMember]
        public Tower _towerBeingCast;

        public double CurrentHp { get; private set; }
        public double _reload { get; private set; }// doc & mech units}
        public double Speed { get; set; }
        public double MaxHp
        {
            get
            {
                if (_type == EnemyType.defaultSoldier) return BalanceEnemyDefaultSoldier.ENEMY_DEFAULT_SOLDIER_MAX_HP;
                else if (_type == EnemyType.kamikaze) return BalanceEnemyKamikaze.ENEMY_KAMIKAZE_MAX_HP;
                else if (_type == EnemyType.doctor) return BalanceEnemyDoctor.ENEMY_DOCTOR_MAX_HP;
                else if (_type == EnemyType.saboteur) return BalanceEnemySaboteur.ENEMY_SABOTEUR_MAX_HP;
                else return 0;
            }
        }
        public double _damage
        {
            get
            {
                if (_type == EnemyType.defaultSoldier) return BalanceEnemyDefaultSoldier.ENEMY_DEFAULT_SOLDIER_DAMAGE;
                else if (_type == EnemyType.kamikaze) return BalanceEnemyKamikaze.ENEMY_KAMIKAZE_DAMAGE;
                else if (_type == EnemyType.doctor) return BalanceEnemyDoctor.ENEMY_DOCTOR_DAMAGE;
                else if (_type == EnemyType.saboteur) return BalanceEnemySaboteur.ENEMY_SABOTEUR_DAMAGE;
                else return 0;
            }
        }
        public double DefaultSpeed
        {
            get
            {
                if (_type == EnemyType.defaultSoldier) return BalanceEnemyDefaultSoldier.ENEMY_DEFAULT_SOLDIER_DEFAULT_SPEED;
                else if (_type == EnemyType.kamikaze) return BalanceEnemyKamikaze.ENEMY_KAMIKAZE_DEFAULT_SPEED;
                else if (_type == EnemyType.doctor) return BalanceEnemyDoctor.ENEMY_DOCTOR_DEFAULT_SPEED;
                else if (_type == EnemyType.saboteur) return BalanceEnemySaboteur.ENEMY_SABOTEUR_DEFAULT_SPEED;
                else return 0;
            }
        }
        public int Bounty
        {
            get
            {
                if (_type == EnemyType.defaultSoldier) return BalanceEnemyDefaultSoldier.ENEMY_DEFAULT_SOLDIER_BOUNTY;
                else if (_type == EnemyType.kamikaze) return BalanceEnemyKamikaze.ENEMY_KAMIKAZE_BOUNTY;
                else if (_type == EnemyType.doctor) return BalanceEnemyDoctor.ENEMY_DOCTOR_BOUNTY;
                else if (_type == EnemyType.saboteur) return BalanceEnemySaboteur.ENEMY_SABOTEUR_BOUNTY;
                else return 0;
            }
        }
        public double ActionRadius // doc & mech units
        {
            get
            {
                if (_type == EnemyType.doctor) return BalanceEnemyDoctor.ENEMY_DOCTOR_ACTION_RADIUS;
                else if (_type == EnemyType.saboteur) return BalanceEnemySaboteur.ENEMY_SABOTEUR_ACTION_RADIUS;
                else return 0;
            }
        }
        public double _heal // for the doc
        {
            get
            {
                if (_type == EnemyType.doctor) return BalanceEnemyDoctor.ENEMY_DOCTOR_HEAL;
                else return 0;
            }
        }
        public double _healCooldown // doc only
        {
            get
            {
                if (_type == EnemyType.doctor) return BalanceEnemyDoctor.ENEMY_DOCTOR_HEAL_COOLDOWN;
                else return 0;
            }
        }

        public Enemy(Map map, Wave wave, string name, EnemyType type)
        {
            Initiliaze = false;
            _type = type;
            _map = map;
            _wave = wave;
            _name = name;
            _position = wave.Position;
            _moveToState = 0;
            _isCasting = false;
            if (type == EnemyType.saboteur)
            {
                _hasCast = false;
                _towerBeingCast = null;
            }
        }


        private void UpdateMove()
        {
            if (_moveToState == ShortestWay.Count - 1 && WithinReach(Position, ShortestWay[_moveToState], Speed)) _position = _map.Wall.Position;
            Vector2 _moveToPosition = ShortestWay[_moveToState];
            if (WithinReach(Position, _moveToPosition, Speed))
            {
                _position = _moveToPosition;
                _moveToState++;
            }

            if (Position.X < _moveToPosition.X)
            {
                _position.X += (int)Speed;
                CurrentDirection = Move.right;
            }
            else if (Position.X > _moveToPosition.X)
            {
                _position.X -= (int)Speed;
                CurrentDirection = Move.left;
            }
            else if (Position.Y < _moveToPosition.Y)
            {
                _position.Y += (int)Speed;
                CurrentDirection = Move.down;
            }
            else if (Position.Y > _moveToPosition.Y)
            {
                _position.Y -= (int)Speed;
                CurrentDirection = Move.top;
            }
        }

       

        public void Update()
        {
            // Deserialization
            if (!Initiliaze)
            {
                CurrentHp = MaxHp;
                Speed = DefaultSpeed;
                if (_type == EnemyType.doctor)
                {
                    _reload = 0;
                }
                else if (_type == EnemyType.saboteur)
                {
                    _reload = BalanceEnemySaboteur.ENEMY_SABOTEUR_RELOADING;
                }
                Initiliaze = true;
            }

            if (!IsStarting) TimerBeforeStarting--;
            else
            {
                 UpdateAttackWall();
                 UpdateSaboteur(GetTowers(_position, ActionRadius));

                if (_isCasting == true)
                    StartCasting(_towerBeingCast);
                else if (_isCasting == false || _towerBeingCast == null)
                    UpdateMove(); // for the saboteur, is false by default.

                UpdateHeal(GetEnemies(_position, ActionRadius));
            } 
                
            
        }

        private void UpdateAttackWall()
        {
            if (WithinReach(Position, _map.Wall.Position, Speed))
            {
                _map.Wall.TakeHp(_damage);
                Die(true);
            }
        }

        private void UpdateHeal(List<Enemy> _enemiesToHeal)
        {
            if (_type == EnemyType.doctor)
            {
                if (!IsReload) Reloading();
                if (IsReload)
                {
                    foreach (Enemy enemy in _enemiesToHeal)
                    {

                        if (enemy.CurrentHp + _heal > enemy.MaxHp) enemy.CurrentHp = enemy.MaxHp;
                        else enemy.CurrentHp += _heal;
                        _reload = _healCooldown * 60;
                    }
                    _map.AnimHeal.Add(this);
                }
            }
            
        }

        public void UpdateSaboteur(List<Tower> _towersToDisable)
        {

            if (_type == EnemyType.saboteur)
            {
                if (_hasCast == false)
                {
                    foreach (Tower tower in _towersToDisable)
                    {
                        if (!tower.IsDisabled && !tower.IsCasted)
                        {
                            _towerBeingCast = tower;
                            _isCasting = true;
                            StartCasting(tower);
                            tower.IsCasted = true;
                            break;
                        }
                    }
                }
            }
            /*
            
             * La tour est disable pendant un certain temps puis revient à la normale.
             */
        }
        
        public void StartCasting(Tower tower)
        {
            List<Tower> towers = GetTowers(_position, ActionRadius);
            if (!towers.Contains(tower))
            {
                _isCasting = false;
                _towerBeingCast = null;
            }
            else if (_reload <= 0)
            {
                if (tower.Type == TowerType.bank)
                {
                    tower._reload = 15 * 60;
                }
                else
                {
                    tower._reload = BalanceEnemySaboteur.ENEMY_SABOTEUR_RELOADING;
                }
                _hasCast = true; // this minion cannot disable turrets anymore 
                _isCasting = false; // Resumes moving
                tower.IsDisabled = true;
                tower.IsCasted = false;
                _map.TowerDisabled.Add(tower);
                _towerBeingCast = null;

            }  else {
                _reload--;
            }
            
        }

        
        internal bool IsReload => _reload <= 0;
        internal void Reloading() => _reload--;

        private List<Enemy> GetEnemies(Vector2 position, double radius)
        {
            List<Enemy> _enemiesToHeal = new List<Enemy>();
            List<Enemy> _currentEnemies = _map.GetAllEnemies();

            foreach (Enemy enemy in _currentEnemies)
            {
                if (WithinReach(position, enemy.Position, radius)) _enemiesToHeal.Add(enemy);
            }

            return _enemiesToHeal;
        }

        private List<Tower> GetTowers(Vector2 position, double radius)
        {
            List<Tower> _towersToDisable = new List<Tower>();
            

            foreach (Tower tower in _map.Towers)
            {
                if (WithinReach(position, tower.Position, radius)) _towersToDisable.Add(tower);
            }

            return _towersToDisable;
        }


        public Vector2 Position => _position;
        public bool IsStarting => TimerBeforeStarting <= 0;
        public bool IsDead => CurrentHp <= 0;
        public void TakeHp(double damage) => CurrentHp -= damage;
        public List<Vector2> ShortestWay => _wave.ShortestWay;
        private bool WithinReach(Vector2 myPosition, Vector2 target, double speed)
        {
            double distanceEnemy = DistanceOf(myPosition, target);
            return distanceEnemy < speed * Constant.imgSizeMap;
        }

        private double DistanceOf(Vector2 myPosition, Vector2 target)
        {
            return (target.X - myPosition.X) * (target.X - myPosition.X) + (target.Y - myPosition.Y) * (target.Y - myPosition.Y);
        }

        public void Die()
        {
            ManagerSound.PlayManDie();
            _map.Dollars += Bounty;
            _wave.Enemies.Remove(this);
            _map.DeadEnemies.Add(this);
            if (_type == EnemyType.saboteur && _towerBeingCast != null) _towerBeingCast.IsCasted = false;
            // IS DISABLED
        }

        public void Die(bool Passedthebase)
        // Overload. If the ennemny unit passes the base, it dies but does not gives gold.
        {
            ManagerSound.PlayManDie();
            if (Passedthebase == false)
            {
                _map.Dollars += Bounty;
            }
            _wave.Enemies.Remove(this);
            _map.DeadEnemies.Add(this);
        }
    }
}
