using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using static Menu.Game1Menu;
using Microsoft.Xna.Framework;

namespace Menu.ButtonsMenu
{
    class MainButtons : GroupOfButtons
    {
        public MainButtons(Game1Menu ctx, int numberOfButtons, Dictionary<int, string>indexOfButtons, int buttonHeight, int buttonWidth) : base(ctx, numberOfButtons, indexOfButtons, buttonHeight, buttonWidth)
        {
            if (!IndexOfButtons.ContainsValue("mode_campagne")) throw new ArgumentException("Dictionary must contain mode_campagne");
            if (!IndexOfButtons.ContainsValue("mode_custom")) throw new ArgumentException("Dictionary must contain mode_custom");
            if (!IndexOfButtons.ContainsValue("editeur_de_map")) throw new ArgumentException("Dictionary must contain creer_votre_map");
            if (!IndexOfButtons.ContainsValue("website")) throw new ArgumentException("Dictionary must contain website");
            if (!IndexOfButtons.ContainsValue("options")) throw new ArgumentException("Dictionary must contain options");
            if (!IndexOfButtons.ContainsValue("quitter")) throw new ArgumentException("Dictionary must contain quit");
        }

        public override void take_action_on_button(int i)
        {
            if (IndexOfButtons[i] == "mode_campagne")
            {
                _ctx.state = MenuState.CAMPAGNE;
            }
            else if (IndexOfButtons[i] == "mode_custom")
            {
                _ctx.MapPlay_Pressed();
            }
            else if (IndexOfButtons[i] == "editeur_de_map")
            {
               
                _ctx.MapEditor_Pressed();
            }
            else if(IndexOfButtons[i] == "website")
            {
               _ctx.LaunchSite("http://trumptower.heberge-tech.fr:2232/");
            }
            else if (IndexOfButtons[i] == "options")
            {
                _ctx.state = MenuState.OPTIONS;
            }
            else if (IndexOfButtons[i] == "quitter")
            {
                _ctx.Exit();
            }

        }
    }
}
