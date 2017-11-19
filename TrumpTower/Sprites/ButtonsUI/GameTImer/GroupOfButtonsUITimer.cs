using Microsoft.Xna.Framework;
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
        public Dictionary<string, Texture2D> Textures { get; set; }

        public GroupOfButtonsUITimer(Game1 ctx, Dictionary<string, Texture2D> textures)
        {
            _ctx = ctx;
            Textures = textures;
            ButtonsUIArray = new Dictionary<string, ButtonUITimer>();

            // Creates buttons
            Vector2 _positionFastButton = new Vector2(_ctx._mapPoint.GetLength(1) * Constant.imgSizeMap - 50, 10);
            CreateButtonUI(new ButtonUITimer(this, "fastTimer", _positionFastButton, Textures["fastButton"]));

            Vector2 _positionNormalButton = new Vector2(_positionFastButton.X - 50, 10);
            CreateButtonUI(new ButtonUITimer(this, "normalTimer", _positionNormalButton, Textures["normalButton"]));

            Vector2 _positionPauseButton = new Vector2(_positionNormalButton.X - 50, 10);
            CreateButtonUI(new ButtonUITimer(this, "pauseTimer", _positionPauseButton, Textures["pauseButton"])); 
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
                        ManagerSound.SoundPauseIn.Play();
                    }
                    else
                    {
                        ButtonActivated = ButtonsUIArray["normalTimer"];
                        _ctx.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);
                        _ctx.GameIsPaused = false;
                        ManagerSound.SoundPauseOut.Play();
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
                    ManagerSound.SoundPauseOut.Play();
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
                    ManagerSound.SoundPauseOut.Play();
                }
                ButtonHover = button;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 _overlayManageTimePosition = new Vector2(ButtonsUIArray["pauseTimer"].Position.X - 5, ButtonsUIArray["pauseTimer"].Position.Y - 5);
            Rectangle _overlayManageTime = new Rectangle(0, 0, 143, 42);
            spriteBatch.Draw(Textures["overlayButtons"], _overlayManageTimePosition, _overlayManageTime, Color.Black * 0.6f);
            foreach (ButtonUITimer button in ButtonsUIArray.Values) button.Draw(spriteBatch);
        }

        public void CreateButtonUI(ButtonUITimer buttonUI)
        {
            ButtonsUIArray[buttonUI.Name] = buttonUI;
        }
    }
}
