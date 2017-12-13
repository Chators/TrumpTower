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

namespace TrumpTower.LibraryTrumpTower
{
    public class Enemy
    {
        readonly Map _map;
        readonly Wave _wave;
        public Wall Wall { get; private set; }
        Vector2 _position;
        string _name;
        int _moveToState;
        public readonly EnemyType _type;
        public Move CurrentDirection { get; private set; }
        public double CurrentHp { get; private set; }
        public double MaxHp { get; private set; }
        public double _damage;
        readonly double _heal; // for the doc
        public double Speed { get; private set; }
        public int Bounty { get; private set; }
        public int TimerBeforeStarting { get; set; }
        public double ActionRadius { get; private set; } // doc & mech units
        public double _reload; // doc & mech & boss units
        public double _healCooldown; // doc only
        public bool _hasCast;
        public bool _isCasting;
        public bool _isCharging; // For boss1
        public bool _hasCharged; // For boss1
        public double _timeBeforeCharging; // For boss1
        public double _timeBeforeEndofCastingCharge; // for boss1
        public bool _isVulnerable; // for boss1 after breaching wall
        public bool _isCastingBoss1;
        public bool _canChargeBoss1;




        public Enemy(Map map, Wave wave, string name, Wall wall, EnemyType type)
        {
            _type = type;
            _map = map;
            _wave = wave;
            Wall = wall;
            _name = name;
            _position = wave.Position;
            _moveToState = 0;
            _isCasting = false;

            if (type == EnemyType.defaultSoldier)
            {
                CurrentHp = 45;
                MaxHp = 85;
                _damage = 10;
                Speed = 3;
                Bounty = 100;
            } else if (type == EnemyType.kamikaze)
            {
                CurrentHp = 200;
                MaxHp = 200;
                _damage = Wall.MaxHp;
                Speed = 2.2;
                Bounty = 200;
            } else if (type == EnemyType.doctor)
            {
                CurrentHp = 60;
                MaxHp = 120;
                _damage = 5;
                Speed = 3;
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
                Bounty = 150;
                ActionRadius = 500;
                _reload = 180;
                _hasCast = false;
            } else if (type == EnemyType.boss1) // Che Guevarra
            {
                CurrentHp = 500;
                MaxHp = 500;
                _damage = Wall.MaxHp / 4;
                Speed = 1.5;
                _reload = 2 * 60; // attacks every two seconds
                _isCharging = false;
                _hasCharged = false;
                _timeBeforeCharging = 6 * 60; // When it comes to 0, boss1 starts casting charge 
                _isVulnerable = false;
                _isCastingBoss1 = false;
                _timeBeforeEndofCastingCharge = 3 * 60; // When it comes to 0, boss1 charges, doubling his speed and dammage, build a wall to stop him
                
            }
        }


        private void UpdateMove()
        {
            if (_moveToState == ShortestWay.Count - 1 && WithinReach(Position, ShortestWay[_moveToState], Speed)) _position = Wall.Position;
            

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
            else if (_type != EnemyType.boss1 && _type != EnemyType.boss2 && _type != EnemyType.boss3)
            {
                 UpdateAttackWall();
                 UpdateSaboteur(GetTowers(_position, ActionRadius));
                if (_isCasting == false) UpdateMove(); // for the saboteur, is false by default.
                 UpdateHeal(GetEnemies(_position, ActionRadius));
            } else if (_type == EnemyType.boss1)
            {
                if (_position != Wall.Position && _isCastingBoss1 == false) UpdateMove();
                UpdateBoss1();
            }  
        }

        private void UpdateBoss1()
        {
            if (_timeBeforeCharging > 0) _timeBeforeCharging--;
            else if (_timeBeforeCharging == 0 && _hasCharged == false && _isCharging == false) ChargeBoss1();
            UpdateAttackWallBoss1();
        }

        private void ChargeBoss1() // Stops moving for couple of secs, before truly charging
        {
            _isCastingBoss1 = true;

            if (_timeBeforeEndofCastingCharge == 0) ChargingBoss1();
            else  if (_timeBeforeEndofCastingCharge > 0) _timeBeforeEndofCastingCharge--;
        }
        
        private void ChargingBoss1() // Is charging
        {
            _hasCharged = true;
            _isCharging = true;
            _isCastingBoss1 = false;
            _damage = _damage * 2;
            Speed = Speed * 5;
        }

        private void EncounterWallCreated() // Boss1
        {
            // If boss is charging and encounters a wall created, the wall breaks.
            //Then _isCharging goes false.
            // Boss resumes normal speed and dmg after a few seconds of stun where he takes double dmg.
            // Keeps _hasCharged = true so he doesnt resume charging 
        }

        private void UpdateAttackWallBoss1()
        {
            if (WithinReach(Position, Wall.Position, Speed))
            {
                if (_reload != 0) _reload --;
                else
                {
                    Wall.TakeHp(_damage);
                    _reload = 2 * 60;
                }
            }
        }

        private void UpdateAttackWall()
        {
            if (WithinReach(Position, Wall.Position, Speed))
            {
                Wall.TakeHp(_damage);
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
                        if (!tower.IsDisabled || !tower.IsCasted)
                        _isCasting = true; 
                        StartCasting(tower);
                        tower.IsCasted = true;
                        
                    }
                }
            }
            /*
            
             * La tour est disable pendant un certain temps puis revient à la normale.
             */
        }
        
        public void StartCasting(Tower tower)
        {
            if (_reload <= 0) {
                if (tower.Type == TowerType.bank)
                {
                    tower._reload = 15 * 60;
                }
                else
                {
                    tower._reload = 5 * 60;
                }
                _hasCast = true; // this minion cannot disable turrets anymore 
                _isCasting = false; // Resumes moving
                tower.IsDisabled = true;

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
