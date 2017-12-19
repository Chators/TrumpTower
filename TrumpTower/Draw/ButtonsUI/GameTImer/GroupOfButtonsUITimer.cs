using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TrumpTower.LibraryTrumpTower.Constants;

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

        public void HandleInput(MouseState newStateMouse, MouseState lastStateMouse, KeyboardState newStateKeyboard, KeyboardState lastStateKeyboard)
        {
            ButtonHover = null;

            // PAUSE
            ButtonUITimer button = ButtonsUIArray["pauseTimer"];
            if (newStateMouse.X > button.Position.X && newStateMouse.X < button.Position.X + button.Texture.Width &&
                newStateMouse.Y > button.Position.Y && newStateMouse.Y < button.Position.Y + button.Texture.Height ||
                newStateKeyboard.IsKeyDown(Keys.Space) && lastStateKeyboard.IsKeyUp(Keys.Space))
            {
                if (newStateMouse.LeftButton == ButtonState.Pressed && lastStateMouse.LeftButton == ButtonState.Released ||
                            newStateKeyboard.IsKeyDown(Keys.Space) && lastStateKeyboard.IsKeyUp(Keys.Space))
                {
                    if (_ctx.GameIsPaused == false)
                    {
                        ButtonActivated = button;
                        _ctx.GameIsPaused = true;
                        _ctx.stratPause++;
                        if (_ctx.stratPause >= 6 )
                        {
                            ManagerSound.PlayNoAvailable();
                            _ctx.stratPause = 5;
                        }
                        ManagerSound.PlayPauseIn();
                    }
                    else
                    {
                        ButtonActivated = ButtonsUIArray["normalTimer"];
                        _ctx.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);
                        _ctx.GameIsPaused = false;
                        ManagerSound.PlayPauseOut();
                    }
                }
                ButtonHover = button;
            }

            // START
            button = ButtonsUIArray["normalTimer"];
            if (newStateMouse.X > button.Position.X && newStateMouse.X < button.Position.X + button.Texture.Width &&
                newStateMouse.Y > button.Position.Y && newStateMouse.Y < button.Position.Y + button.Texture.Height)
            {
                if (newStateMouse.LeftButton == ButtonState.Pressed && lastStateMouse.LeftButton == ButtonState.Released)
                {
                    ButtonActivated = button;
                    _ctx.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);
                    _ctx.GameIsPaused = false;
                    ManagerSound.PlayPauseOut();
                }
                ButtonHover = button;
            }

            // FAST
            button = ButtonsUIArray["fastTimer"];
            if (newStateMouse.X > button.Position.X && newStateMouse.X < button.Position.X + button.Texture.Width &&
                newStateMouse.Y > button.Position.Y && newStateMouse.Y < button.Position.Y + button.Texture.Height ||
                newStateKeyboard.IsKeyDown(Keys.OemPlus) && lastStateKeyboard.IsKeyUp(Keys.OemPlus))
            {
                if (newStateMouse.LeftButton == ButtonState.Pressed && lastStateMouse.LeftButton == ButtonState.Released ||
                    newStateKeyboard.IsKeyDown(Keys.OemPlus) && lastStateKeyboard.IsKeyUp(Keys.OemPlus))
                {
                    ButtonActivated = button;
                    _ctx.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 120.0f);
                    _ctx.GameIsPaused = false;
                    ManagerSound.PlayPauseOut();
                }
                ButtonHover = button;
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
