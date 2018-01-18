using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using static Menu.Game1Menu;
using Microsoft.Xna.Framework;

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D _imgIndicate = _ctx._imgArrowRight;
            if (_ctx._player._lvlAccess < 6) spriteBatch.Draw(_imgIndicate, new Vector2(_buttonRectangle[0].X - _imgIndicate.Width, _buttonRectangle[0].Y + 18), Color.White);
            else if (_ctx._player._lvlAccess < 11) spriteBatch.Draw(_imgIndicate, new Vector2(_buttonRectangle[1].X - _imgIndicate.Width, _buttonRectangle[1].Y + 18), Color.White);
            else spriteBatch.Draw(_imgIndicate, new Vector2(_buttonRectangle[2].X - _imgIndicate.Width, _buttonRectangle[2].Y + 18), Color.White);

            base.Draw(spriteBatch);
        }

        public override void take_action_on_button(int i)
        {
            if (IndexOfButtons[i] == "World1/World-1")
            {
                _ctx.state = MenuState.WORLD1;
            }
            else if (IndexOfButtons[i] == "World2/World-2")
            {
                if (_ctx._player._lvlAccess >= 6)
                    _ctx.state = MenuState.WORLD2;
                else
                    Console.WriteLine("VOUS NE POUVEZ PAS JOUER A LA MAP MDR");
            }
            else if (IndexOfButtons[i] == "World3/World-3")
            {
                if (_ctx._player._lvlAccess >= 11)
                    _ctx.state = MenuState.WORLD3;
                else
                    Console.WriteLine("VOUS NE POUVEZ PAS JOUER A LA MAP MDR");
            }
            else if (IndexOfButtons[i] == "return")
            {
                _ctx.state = MenuState.MAIN;
            }
        }
    }
}
