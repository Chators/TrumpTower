using LibraryTrumpTower.Constants.BalanceGame.Bosses;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;

namespace LibraryTrumpTower.SpecialAbilities
{
    public class ChainBoss
    {
        public enum ChainBossState
        {
            HANGING,
            LAUNCH,
            STALLED

        }

        public double MaxHp { get; set; }
        public double CurrentHp { get; set; }
        public Vector2 _position;
        public double Speed { get; set; }
        public double Damage { get; set; }
        public Tower TowerTarget { get; set; }
        public Wall Wall { get; set; }
        public ChainBossState CurrentState { get; set; }
        public Map Ctx { get; set; }
        public Enemy Boss3 { get; set; }

        public ChainBoss(Map ctx, double maxHp, Vector2 position, Tower towerTarget, Wall wall, double damage)
        {
            Ctx = ctx;
            MaxHp = maxHp;
            CurrentHp = MaxHp;
            _position = position;
            TowerTarget = towerTarget;
            Speed = BalanceBoss3.BOSS3_CHAIN_SPEED;
            Wall = wall;
            Damage = damage;
            CurrentState = ChainBossState.LAUNCH;
        }

        public void Update()
        {
            if (CurrentState == ChainBossState.LAUNCH)
            {
                UpdateMove(TowerTarget.Position);
                if (WithinReach(_position, TowerTarget.Position, Speed))
                {
                    _position = TowerTarget.Position;
                    CurrentState = ChainBossState.HANGING;
                }
            }
            else if (CurrentState == ChainBossState.STALLED)
            {
                UpdateMove(Ctx.Wall.Position);
                if (WithinReach(_position, Ctx.Wall.Position, Speed))
                {
                    Wall.TakeHp(Damage);
                    Ctx.CurrentChainsBoss.Remove(this);
                }
            }
        }

        private void UpdateMove(Vector2 positionTarget)
        {
            if (WithinReach(_position, positionTarget, Speed)) _position = positionTarget;

            if (_position.X < positionTarget.X) _position.X += (int)Speed;
            else if (_position.X > positionTarget.X) _position.X -= (int)Speed;
            else if (_position.Y < positionTarget.Y) _position.Y += (int)Speed;
            else if (_position.Y > positionTarget.Y) _position.Y -= (int)Speed;
        }

        public bool IsArrived() => CurrentState == ChainBossState.HANGING;
        public bool IsDead() => CurrentHp < 0;
        #region WithinReach
        private bool WithinReach(Vector2 myPosition, Vector2 target, double speed)
        {
            double distanceEnemy = DistanceOf(myPosition, target);
            return distanceEnemy < speed * Constant.imgSizeMap;
        }

        private double DistanceOf(Vector2 myPosition, Vector2 target)
        {
            return (target.X - myPosition.X) * (target.X - myPosition.X) + (target.Y - myPosition.Y) * (target.Y - myPosition.Y);
        }
        #endregion
    }
}
