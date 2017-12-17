using System;
using TrumpTower.LibraryTrumpTower;
using TrumpTower;
using MapEditorTrumpTower;

namespace Menu
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            State = MainState.Menu;
            while (State != MainState.Quit)
            {
                if (State == MainState.Menu)
                {
                    using (var game = new Game1Menu())
                        game.Run();
                    if (State == MainState.Menu)
                        State = MainState.Quit;
                }
                else if (State == MainState.Game)
                {
                    using (var game = new TrumpTower.Game1())
                        game.Run();
                    State = MainState.Menu;
                }
                else if (State == MainState.EditorMap)
                {
                    using (var game = new MapEditorTrumpTower.Game1MapEditor())
                        game.Run();
                    State = MainState.Menu;
                }
            }
        }

        public static MainState State;
        public enum MainState
        {
            Menu,
            Game,
            EditorMap,
            Quit,
            None
        }

    }
#endif
}
