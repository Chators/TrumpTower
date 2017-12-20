using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;

namespace LibraryTrumpTower
{
    [DataContract(IsReference = true)]
    public class Hero
    {
        [DataMember]
        private Map Map { get; set; }
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public Vector2 _position;
        [DataMember]
        private double Speed { get; set; }
        [DataMember]
        public double Angle { get; private set; }
        [DataMember]
        private double Damage { get; set; }
        [DataMember]
        private int Reload { get; set; }
        [DataMember]
        private float SpeedCharacter { get; set; }
        [DataMember]
        private int TimeCurrentRepair { get; set; }
        [DataMember]
        private Tower TowerUnderRepair { get; set; }
        [DataMember]
        public Vector2 PositionGo { get; set; }
        //[DataMember]
        //public HeroesSpecialCapacity Capacity { get; set; }

        [DataMember]
        private int TimeForRepair { get; set; }
        [DataMember]
        private int TimeForReload { get; set; }

        public Hero (Map map, string name, Vector2 position)
        {
            Map = map;
            Name = name;
            _position = position;
            Speed = 5;
            Angle = 0;
            Damage = 30;
            Reload = 0;
            SpeedCharacter = 1f;
            TimeCurrentRepair = 0;
            TowerUnderRepair = null;
            PositionGo = Vector2.Zero;

            TimeForRepair = 6 * 60;
            TimeForReload = 1 * 60;
        }

        private void UpdateMove(Vector2 targetPosition)
        {
            if (_position.X < targetPosition.X) _position.X += (int)Speed;
            if (_position.X > targetPosition.X) _position.X -= (int)Speed;
            if (_position.Y < targetPosition.Y) _position.Y += (int)Speed;
            if (_position.Y > targetPosition.Y) _position.Y -= (int)Speed;
        }

        public void Update()
        {
            // On cherche tous les ennemies à proximité si on est pas entrain de rechargé et si on est pas entrain de bouger
            if (!IsReloading && PositionGo == null)
            {
                List<Enemy> enemies = Map.GetAllEnemies();
                foreach (Enemy enemy in enemies)
                {
                    if (Vector2.Distance(_position, enemy.Position) < 150)
                    {
                        enemy.TakeHp(Damage);
                        Reload = TimeForReload;
                        ManagerSound.PlayHeroesShoot();
                        break;
                    }
                }
            }
            else Reload--;


            // On déplace le personnage
            if (PositionGo != new Vector2(-100,-100))
            {
                SetRotate(new Vector2((int)_position.X + 32, (int)_position.Y + 32), new Vector2((int)PositionGo.X + 32, (int)PositionGo.Y + 32));
                UpdateMove(PositionGo);
            }
        }

        public void SetRotate(Vector2 _position, Vector2 _targetPosition)
        {
            Vector2 direction = _position - _targetPosition;
            direction.Normalize();
            Angle = (float)Math.Atan2(-direction.X, direction.Y);
        }

        public Vector2 Position => _position;
        private bool IsReloading => Reload > 0;
    }
}
