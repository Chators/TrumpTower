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
        public Vector2 Position { get; private set; }
        [DataMember]
        private Vector2 Velocity { get; set; }
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
            Position = position;
            Velocity = Vector2.Zero;
            Angle = 0;
            Damage = 30;
            Reload = 0;
            SpeedCharacter = 1f;
            TimeCurrentRepair = 0;
            TowerUnderRepair = null;

            TimeForRepair = 6 * 60;
            TimeForReload = 1 * 60;
        }

        public void Update()
        {
            // On cherche tous les ennemies à proximité si on est pas entrain de rechargé
            if (!IsReloading)
            {
                List<Enemy> enemies = Map.GetAllEnemies();
                foreach (Enemy enemy in enemies)
                {
                    if (Vector2.Distance(Position, enemy.Position) < 150)
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
            if (Velocity != Vector2.Zero)
            {
                Vector2 previousPosition = Position + Velocity;
                if (Map.IsGrass((int)(previousPosition.X / Constant.imgSizeMap)+1, (int)(previousPosition.Y / Constant.imgSizeMap)+1))
                    Position += Velocity;
                /*else if (Map.IsGrass((int)previousPosition.X / Constant.imgSizeMap, (int)previousPosition.Y / Constant.imgSizeMap))
                {

                }*/
                Velocity = Vector2.Zero;
            }
        }

        public void HandleInput(MouseState newStateMouse, MouseState lastStateMouse, KeyboardState newStateKeyboard, KeyboardState lastStateKeyboard)
        {
            if (newStateKeyboard.IsKeyDown(Keys.Z))
                Velocity += new Vector2(0, -1);
            if (newStateKeyboard.IsKeyDown(Keys.Q))
                Velocity += new Vector2(-1, 0);
            if (newStateKeyboard.IsKeyDown(Keys.S))
                Velocity += new Vector2(0, 1);
            if (newStateKeyboard.IsKeyDown(Keys.D))
                Velocity += new Vector2(1, 0);

            /* CAPACITE SPECIAL */
            /*
            if (newStateKeyboard.IsKeyDown(Keys.F))
            {

            }
            */
        }

        private bool IsReloading => Reload > 0;
    }
}
