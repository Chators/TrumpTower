using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TrumpTower.Draw.ButtonsUI
{
    public class GroupOfButtonsUITimer
    {
        Game1 _ctx;
        public Dictionary<string, ButtonUITimer> ButtonsUIArray { get; private set; }
        public ButtonUITimer ButtonHover { get; set; }
        public ButtonUITimer ButtonActivated { get; set; }

        public GroupOfButtonsUITimer(Game1 ctx)
        {
            _ctx = ctx;
            ButtonsUIArray = new Dictionary<string, ButtonUITimer>();
        }

        public void HandleInput(MouseState newStateMouse, MouseState lastStateMouse)
        {
            foreach (ButtonUITimer button in ButtonsUIArray.Values)
            {
                ButtonHover = null;

                if (newStateMouse.X > button.Position.X &&
                    newStateMouse.X < button.Position.X + button.Texture.Width &&
                    newStateMouse.Y > button.Position.Y &&
                    newStateMouse.Y < button.Position.Y + button.Texture.Height)
                {
                    // GAME TIMER
                    if (button.Name == "pauseTimer")
                    {
                        if (newStateMouse.LeftButton == ButtonState.Pressed &&
                            lastStateMouse.LeftButton == ButtonState.Released)
                        {
                            if (_ctx.GameIsPaused == false)
                            {
                                ButtonActivated = button;
                                _ctx.GameIsPaused = true;
                            }
                            else
                            {
                                ButtonActivated = ButtonsUIArray["normalTimer"];
                                _ctx.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);
                                _ctx.GameIsPaused = false;
                            }
                        }
                        ButtonHover = button;
                    }
                    else if (button.Name == "normalTimer")
                    {
                        if (newStateMouse.LeftButton == ButtonState.Pressed &&
                            lastStateMouse.LeftButton == ButtonState.Released)
                        {
                            ButtonActivated = button;
                            _ctx.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);
                            _ctx.GameIsPaused = false;
                        }
                        ButtonHover = button;
                    }
                    else if (button.Name == "fastTimer")
                    {
                        if (newStateMouse.LeftButton == ButtonState.Pressed &&
                            lastStateMouse.LeftButton == ButtonState.Released)
                        {
                            ButtonActivated = button;
                            _ctx.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 120.0f);
                            _ctx.GameIsPaused = false;
                        }
                        ButtonHover = button;
                    }
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ButtonUITimer button in ButtonsUIArray.Values) button.Draw(spriteBatch);
        }

        public void CreateButtonUI(ButtonUITimer buttonUI)
        {
            ButtonsUIArray[buttonUI.Name] = buttonUI;
        }
    }
}
