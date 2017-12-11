using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorTrumpTower
{
    public enum StateType
    {
        Default,
        Init,
        CreatePathForSpawn,
    }

    public class GameState
    {
        public StateType ActualState { get; set; }

        public GameState (StateType state)
        {
            ActualState = state;
        }
    }
}
