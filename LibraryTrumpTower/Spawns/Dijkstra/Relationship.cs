using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrumpTower.Spawns.Dijkstra
{
    class Relationship
    {
        public User[] Users { get; private set; }

        public Relationship(User user1, User user2)
        {
            Users = new User[2];
            Users[0] = user1;
            Users[1] = user2;
        }
    }
}
