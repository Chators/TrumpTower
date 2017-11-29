using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;

namespace LibraryTrumpTower.AirUnits
{
    public class AirUnit
    {
        public AirUnitsCollection Ctx { get; private set; }
        public Wall Wall { get; private set; }
        public double CurrentHp { get; private set; }
        public double MaxHp { get; private set; }
        public double Damage { get; private set; }
        public int Bounty { get; private set; }
        public Vector2 _position;
        public double Speed { get; private set; }
        public float Rotate { get; private set; }
        public int TimerBeforeStarting { get; set; }

        public AirUnit (AirUnitsCollection ctx, Wall wall, double life, double damage, Vector2 position, double speed, float rotate, int timerBeforeStarting)
        {
            Ctx = ctx;
            Wall = wall;
            CurrentHp = life;
            MaxHp = life;
            Damage = damage;
            Bounty = 100;
            _position = position;
            Speed = speed;
            Rotate = rotate;
            TimerBeforeStarting = timerBeforeStarting;
        }

        private void UpdateTryAttack()
        {
            if (WithinReach(Wall.Position, Speed))
            {
                Wall.TakeHp(Damage);
                Die(true);
            }
        }

        private void UpdateMove(Vector2 targetPosition)
        {
            if (_position.X < targetPosition.X) _position.X += (int)Speed;
            if (_position.X > targetPosition.X) _position.X -= (int)Speed;
            if (_position.Y < targetPosition.Y) _position.Y += (int)Speed;
            if (_position.Y > targetPosition.Y) _position.Y -= (int)Speed;
        }

        public void Update ()
        {
            if (IsStarting)
            {
                SetRotate(new Vector2(Position.X + 32, Position.Y + 32), new Vector2(Wall.Position.X + 32, Wall.Position.Y + 32));
                UpdateTryAttack();
                UpdateMove(Wall.Position);
            }
            TimerBeforeStarting--;
        }

        private bool WithinReach(Vector2 target, double speed)
        {
            double distanceEnemy = DistanceOf(target);
            return distanceEnemy < speed * Constant.imgSizeMap;
        }

        private double DistanceOf(Vector2 positionTarget)
        {
            return (positionTarget.X - Position.X) * (positionTarget.X - Position.X) + (positionTarget.Y - Position.Y) * (positionTarget.Y - Position.Y);
        }

        public void SetRotate(Vector2 _position, Vector2 _targetPosition)
        {
            Vector2 direction = _position - _targetPosition;
            direction.Normalize();
            Rotate = (float)Math.Atan2(-direction.X, direction.Y);
        }

        public void TakeHp(double damage) => CurrentHp -= damage;
        public bool IsDead => CurrentHp <= 0;
        public void Die()
        {
            SoundEffectInstance InstanceManDie = ManagerSound.ManDie.CreateInstance();
            InstanceManDie.Volume = 0.8f;
            InstanceManDie.Play();
            Ctx.Ctx.Dollars += Bounty;
            Ctx.Array.Remove(this);
        }

        public void Die(bool PassedTheBase)
        {
            SoundEffectInstance InstanceManDie = ManagerSound.ManDie.CreateInstance();
            InstanceManDie.Volume = 0.8f;
            InstanceManDie.Play();
            if (!PassedTheBase) Ctx.Ctx.Dollars += Bounty;
            Ctx.Array.Remove(this);
        }

        public bool IsStarting => TimerBeforeStarting <= 0;
        public Vector2 Position => _position;
    }
}
