using TrumpTower.LibraryTrumpTower.Constants;
using TrumpTower.LibraryTrumpTower.Spawns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

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
        Move _currentDirection;
        public double CurrentHp { get; private set; }
        public double MaxHp { get; private set; }
        readonly int _damage;
        public double Speed { get; private set; }
        public int Bounty { get; private set; }
        public int TimerBeforeStarting { get; set; }

        public Enemy(Map map, Wave wave, string name, Wall wall)
        {
            _map = map;
            _wave = wave;
            Wall = wall;
            _name = name;
            CurrentHp = 100;
            MaxHp = 100;
            _damage = 10;
            Speed = 3;
            Bounty = 100;
            _position = wave.Position;
            _moveToState = 0;
        }

        private void UpdateMove()
        {
            if (_moveToState == ShortestWay.Count - 1 && WithinReach(Position, ShortestWay[_moveToState], Speed)) _position = Wall.Position;

            Vector2 _moveToPosition = ShortestWay[_moveToState];
            if (WithinReach(Position, _moveToPosition, Speed)) _moveToState++;

            if (Position.X < _moveToPosition.X)
            {
                _position.X += (int)Speed;
                _currentDirection = Move.right;
            }
            if (Position.X > _moveToPosition.X)
            {
                _position.X -= (int)Speed;
                _currentDirection = Move.left;
            }
            if (Position.Y < _moveToPosition.Y)
            {
                _position.Y += (int)Speed;
                _currentDirection = Move.top;
            }
            if (Position.Y > _moveToPosition.Y)
            {
                _position.Y -= (int)Speed;
                _currentDirection = Move.down;
            }
        }

        public void Update()
        {
            if (!IsStarting) TimerBeforeStarting--;
            else
            {
                
                 UpdateAttackWall();
                 UpdateMove();
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

        

        public Vector2 Position => _position;
        public bool IsStarting => TimerBeforeStarting <= 0;
        public bool IsDead => CurrentHp <= 0;
        public void TakeHp(int damage) => CurrentHp -= damage;
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
            ManagerSound.ManDie.Play();
            _map.Dollars += Bounty;
            _wave.Enemies.RemoveAt(_wave.Enemies.IndexOf(this));
        }

        public void Die(bool Passedthebase)
        // Overload. If the ennemny unit passes the base, it dies but does not gives gold.
        {
            ManagerSound.ManDie.Play();
            if (Passedthebase == false)
            {
                _map.Dollars += Bounty;
            }
            _wave.Enemies.RemoveAt(_wave.Enemies.IndexOf(this));
        }
    }
}
