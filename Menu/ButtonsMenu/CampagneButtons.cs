using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Menu.Game1Menu;

namespace Menu.ButtonsMenu
{
    class CampagneButtons : GroupOfButtons
    {
        public CampagneButtons(Game1Menu ctx, int numberOfButtons, Dictionary<int, string> indexOfButtons, int buttonHeight, int buttonWidth) : base(ctx, numberOfButtons, indexOfButtons, buttonHeight, buttonWidth)
        {
            if (!IndexOfButtons.ContainsValue("World1/World-1")) throw new ArgumentException("Dictionary must contain World1/World-1");
            if (!IndexOfButtons.ContainsValue("World2/World-2")) throw new ArgumentException("Dictionary must contain World2/World-2");
            if (!IndexOfButtons.ContainsValue("World3/World-3")) throw new ArgumentException("Dictionary must contain World3/World-3");
            if (!IndexOfButtons.ContainsValue("return")) throw new ArgumentException("Dictionary must contain return");
        }

        public override void take_action_on_button(int i)
        {
            if (IndexOfButtons[i] == "World1/World-1")
            {
                _ctx.state = MenuState.WORLD1;
            }
            else if (IndexOfButtons[i] == "World2/World-2")
            {
                _ctx.state = MenuState.WORLD2;
            }
            else if (IndexOfButtons[i] == "World3/World-3")
            {
                _ctx.state = MenuState.WORLD3;
            }
            else if (IndexOfButtons[i] == "return")
            {
                _ctx.state = MenuState.MAIN;
            }
        }
    }
}
