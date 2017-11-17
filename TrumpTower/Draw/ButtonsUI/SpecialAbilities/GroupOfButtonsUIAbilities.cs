using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.Draw.ButtonsUI.SpecialAbilities
{
    class GroupOfButtonsUIAbilities
    {
        Game1 _ctx;
        public Dictionary<string, ButtonUIAbility> ButtonsUIArray { get; private set; }
        public ButtonUIAbility ButtonHover { get; set; }
        public ButtonUIAbility ButtonActivated { get; set; }

        public GroupOfButtonsUIAbilities(Game1 ctx)
        {
            _ctx = ctx;
            ButtonsUIArray = new Dictionary<string, ButtonUIAbility>();
        }

        public void HandleInput(MouseState newStateMouse, MouseState lastStateMouse)
        {
            foreach (ButtonUIAbility button in ButtonsUIArray.Values)
            {
                ButtonHover = null;

                if (newStateMouse.X > button.Position.X &&
                    newStateMouse.X < button.Position.X + button.Texture.Width &&
                    newStateMouse.Y > button.Position.Y &&
                    newStateMouse.Y < button.Position.Y + button.Texture.Height)
                {
                    if (button.Name == "explosionAbility")
                    {
                        if (newStateMouse.LeftButton == ButtonState.Pressed &&
                            lastStateMouse.LeftButton == ButtonState.Released)
                        {
                            ButtonActivated = button;
                        }
                        ButtonHover = button;
                    }
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(ButtonUIAbility button in ButtonsUIArray.Values) button.Draw(spriteBatch);
        }

        public void CreateButtonUI(ButtonUIAbility buttonUI)
        {
            ButtonsUIArray[buttonUI.Name] = buttonUI;
        }
    }
}
