using System;
using System.Diagnostics;

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
            //Debugger.Launch();
            using (var game = new Game1Menu())
                game.Run();
        }
    }
#endif
}
