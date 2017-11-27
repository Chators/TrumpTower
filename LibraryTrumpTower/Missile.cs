﻿using LibraryTrumpTower.Constants;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower.Constants;

namespace TrumpTower.LibraryTrumpTower
{
    public class Missile
    {
        Map _map;
        readonly Tower _tower;
        float _rotate;
        readonly int _damage;
        public double Speed { get; private set; }
        public Enemy Target { get; private set; }
        Vector2 _position;

        public Missile(Map map, Tower tower, int damage, Vector2 position, Enemy target)
        {
            _map = map;
            _tower = tower;
            _damage = damage;
            Speed = 10;
            Target = target;
            _position = position;
            _rotate = 0;
        }

        private void UpdateMove()
        {
            if (Position.X < Target.Position.X) _position.X += (int)Speed;
            if (Position.X > Target.Position.X) _position.X -= (int)Speed;
            if (Position.Y < Target.Position.Y) _position.Y += (int)Speed;
            if (Position.Y > Target.Position.Y) _position.Y -= (int)Speed;
        }

        private void UpdateTryAttack()
        {
            if (WithinReach(Target.Position, Speed))
            {
                Target.TakeHp(_damage);
                if (Target.IsDead) Target.Die();
                Die();
            }
        }

        public TowerType Tower => _tower.Type;

        public void Update()
        {
            UpdateTryAttack();
            UpdateMove();
            SetRotate(new Vector2(Position.X + 32, Position.Y + 32), new Vector2(Target.Position.X + 32,Target.Position.Y + 32));
        }

        public Vector2 Position => _position;

        private bool WithinReach(Vector2 target, double speed)
        {
            double distanceEnemy = DistanceOf(target);
            return distanceEnemy < speed * Constant.imgSizeMap;
        }
        private double DistanceOf(Vector2 positionTarget)
        {
            return (positionTarget.X - Position.X) * (positionTarget.X - Position.X) + (positionTarget.Y - Position.Y) * (positionTarget.Y - Position.Y);
        }

        private void Die()
        {
            _map.Missiles.RemoveAt(_map.Missiles.IndexOf(this));
        }

        public void SetRotate(Vector2 _position, Vector2 _targetPosition)
        {
            Vector2 direction = _position - _targetPosition;
            direction.Normalize();
            _rotate = (float)Math.Atan2(-direction.X, direction.Y);
        }

        public double Damage => _damage;
        public float Angle => _rotate;
    }
}
