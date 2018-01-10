﻿using TrumpTower.LibraryTrumpTower.Constants;
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

namespace TrumpTower.LibraryTrumpTower
{
    [DataContract(IsReference = true)]
    public class Enemy
    {
        [DataMember]
        readonly Map _map;
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
        public double CurrentHp { get; private set; }
        [DataMember]
        public double MaxHp { get; private set; }
        [DataMember]
        readonly double _damage;
        [DataMember]
        readonly double _heal; // for the doc
        [DataMember]
        public double Speed { get; set; }
        [DataMember]
        public double DefaultSpeed { get; private set; }
        [DataMember]
        public int Bounty { get; private set; }
        [DataMember]
        public int TimerBeforeStarting { get; set; }
        [DataMember]
        public double ActionRadius { get; private set; } // doc & mech units
        [DataMember]
        public double _reload; // doc & mech units
        [DataMember]
        public double _healCooldown; // doc only
        [DataMember]
        public bool _hasCast;
        [DataMember]
        public bool _isCasting;
        [DataMember]
        public Tower _towerBeingCast;

        public Enemy(Map map, Wave wave, string name, EnemyType type)
        {
            _type = type;
            _map = map;
            _wave = wave;
            _name = name;
            _position = wave.Position;
            _moveToState = 0;
            _isCasting = false;

            if (type == EnemyType.defaultSoldier)
            {
                CurrentHp = 85;
                MaxHp = 85;
                _damage = 10;
                Speed = 3;
                DefaultSpeed = 3;
                Bounty = 100;
            } else if (type == EnemyType.kamikaze)
            {
                CurrentHp = 200;
                MaxHp = 200;
                _damage = Constant.MaxWallHp;
                Speed = 2.2;
                DefaultSpeed = 2.2;
                Bounty = 200;
            } else if (type == EnemyType.doctor)
            {
                CurrentHp = 120;
                MaxHp = 120;
                _damage = 5;
                Speed = 3;
                DefaultSpeed = 3;
                Bounty = 150;
                _heal = 20;
                ActionRadius = 400;
                _reload = 0;
                _healCooldown = 3;
            } else if(type == EnemyType.saboteur)
            {
                CurrentHp = 50;
                MaxHp = 50;
                _damage = 5;
                Speed = 4;
                DefaultSpeed = 4;
                Bounty = 150;
                ActionRadius = 500;
                _reload = Constant.DisabledTower*60;
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
                    tower._reload = Constant.DisabledTower * 60;
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
