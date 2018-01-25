using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Menu.Game1Menu;

namespace Menu.ButtonsMenu
{
    class OptionsButtons : GroupOfButtons
    {
        public OptionsButtons(Game1Menu ctx, int numberOfButtons, Dictionary<int, string> indexOfButtons, int buttonHeight, int buttonWidth) : base(ctx, numberOfButtons, indexOfButtons, buttonHeight, buttonWidth)
        {
            if (!IndexOfButtons.ContainsValue("Import_Map")) throw new ArgumentException("Dictionary must contain Import_Map");
            if (!IndexOfButtons.ContainsValue("Export_Map")) throw new ArgumentException("Dictionary must contain Export_Map");
            if (!IndexOfButtons.ContainsValue("how_to_play")) throw new ArgumentException("Dictionary must contain how_to_play");
            if (!IndexOfButtons.ContainsValue("return")) throw new ArgumentException("Dictionary must contain return");
        }

        public override void take_action_on_button(int i)
        {
            if (IndexOfButtons[i] == "Import_Map")
            {
                _ctx.DownloadMap_Pressed();
            }
            else if (IndexOfButtons[i] == "Export_Map")
            {
                _ctx.UploadMap_Pressed();
            }
            else if (IndexOfButtons[i] == "how_to_play")
            {
                _ctx.LaunchSite("http://trumptower.heberge-tech.fr:2232/how_to_play.php");
            }
            else if (IndexOfButtons[i] == "return")
            {
                _ctx.state = MenuState.MAIN;
            }            
        }
    }
}
