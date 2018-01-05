using LibraryTrumpTower.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Menu.Game1Menu;

namespace Menu.ButtonsMenu
{
    class WorldButton : GroupOfButtons
    {
        string _nameWorld; // Name of World
        List<string> _nameMaps; // Name of Map in file for deserialization

        public WorldButton(Game1Menu ctx, int numberOfButtons, Dictionary<int, string> indexOfButtons, int buttonHeight, int buttonWidth, string nameWorld, List<string> nameMaps) : base(ctx, numberOfButtons, indexOfButtons, buttonHeight, buttonWidth)
        {
            _nameWorld = nameWorld;
            _nameMaps = nameMaps;
        }

        public override void take_action_on_button(int i)
        {
            if (IndexOfButtons[i] == "return")
            {
                _ctx.state = MenuState.CAMPAGNE;
            }
            // when you click on a map
            else
            {
                string[] filesInDirectory = Directory.GetFileSystemEntries(BinarySerializer.pathCampagneMap + "/" + _nameWorld);
                FileInfo file = new FileInfo(filesInDirectory[i]);
                // On copie le fichier dans CurrentMap
                file.CopyTo(BinarySerializer.pathCurrentMapXml, true);
                Process.Start("TrumpTower");
                _ctx.Exit();
            }
        }
    }
}
