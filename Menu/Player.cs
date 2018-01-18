using LibraryTrumpTower.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Menu
{
    [DataContract(IsReference = true)]
    public class Player
    {
        [DataMember]
        public string _name; // name
        [DataMember]
        private string _password; // password
        [DataMember]
        public int _lvlAccess; // the lvl of the player can access

        public Player(string name, string password, int lvlAccess)
        {
            _name = name;
            _password = password;
            _lvlAccess = lvlAccess;
        }

        public void Serialize()
        {
            BinarySerializer.Serialize<Player>(this, "CurrentPlayer.xml");
        }
    }
}
