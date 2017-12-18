using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;

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
            SpeedCharacter = 0.5f;
            TimeCurrentRepair = 0;
            TowerUnderRepair = null;

            TimeForRepair = 6 * 60;
            TimeForReload = 1 * 60;
        }

        public void Update()
        {

        }

        public void HandleInput(MouseState newStateMouse, MouseState lastStateMouse, KeyboardState newStateKeyboard, KeyboardState lastStateKeyboard)
        {
            //if ()
        }
    }
}
