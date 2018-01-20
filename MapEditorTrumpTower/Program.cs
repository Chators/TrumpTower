using System;
using System.Diagnostics;

namespace MapEditorTrumpTower
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Debugger.Launch();
            using (var game = new Game1MapEditor())
                game.Run();

            Process.Start("Menu");
        }
    }
#endif
}
