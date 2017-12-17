using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;

namespace LibraryTrumpTower.AirUnits
{
    [DataContract(IsReference = true)]
    public class AirUnit
    {
        #region Fields
        [DataMember]
        public AirUnitsCollection Ctx { get; private set; }
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public double CurrentHp { get; private set; }
        [DataMember]
        public double MaxHp { get; private set; }
        [DataMember]
        public double Damage { get; private set; }
        [DataMember]
        public int Bounty { get; private set; }
        [DataMember]
        public Vector2 _position;
        [DataMember]
        public double Speed { get; private set; }
        [DataMember]
        public float Rotate { get; private set; }
        [DataMember]
        public int TimerBeforeStarting { get; set; }
        [DataMember]
        public PlaneType TypeOfPlane { get; set; }
        #endregion

        public AirUnit (AirUnitsCollection ctx, string name, double life, double damage, Vector2 position, double speed, float rotate, int timerBeforeStarting, PlaneType type)
        {
            Ctx = ctx;
            Name = name;
            CurrentHp = life;
            MaxHp = life;
            Damage = damage;
            Bounty = 100;
            _position = position;
            Speed = speed;
            Rotate = rotate;
            TimerBeforeStarting = timerBeforeStarting;
            TypeOfPlane = type;
        }

        private void UpdateTryAttack()
        {
            if (WithinReach(Ctx.Wall.Position, Speed))
            {
                Ctx.Wall.TakeHp(Damage);
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
                SetRotate(new Vector2((int)Position.X + 32, (int)Position.Y + 32), new Vector2((int)Ctx.Wall.Position.X + 32, (int)Ctx.Wall.Position.Y + 32));
                UpdateTryAttack();
                UpdateMove(Ctx.Wall.Position);
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

        public void TakeHp(double damage)
        {
            CurrentHp -= damage;
            ManagerSound.PlayImpactUnitAir();
        }
        public bool IsDead => CurrentHp <= 0;
        public void Die()
        {
            ManagerSound.PlayDestroyUnitAir();
            Ctx.Ctx.Dollars += Bounty;
            Ctx.Array.Remove(this);
            Ctx.Ctx.DeadUnitsAir.Add(this);
        }

        public void Die(bool PassedTheBase)
        {
            ManagerSound.PlayDestroyUnitAir();
            if (!PassedTheBase) Ctx.Ctx.Dollars += Bounty;
            Ctx.Array.Remove(this);
            Ctx.Ctx.DeadUnitsAir.Add(this);
        }

        public bool IsStarting => TimerBeforeStarting <= 0;
        public Vector2 Position => _position;
    }
}
