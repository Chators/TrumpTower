using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorTrumpTower.Button
{
    public enum ActionCreatePath
    {
        Back,
        Add,
        Reset,
        Undo,
        Validate,
        None
    }

    public class ButtonCreatePath
    {
        Game1MapEditor Ctx { get; set; }
        ActionCreatePath Action { get; set; }
        string Description { get; set; }
        Texture2D TextureButton { get; set; }
        SpriteFont TextShortCut { get; set; }
        Vector2 Position { get; set; }
        string ShortCut { get; set; }
        bool Hoover { get; set; }
        List<Keys> KeysShortCut { get; set; }

        public ButtonCreatePath(Game1MapEditor ctx, string description, Texture2D textureButton, SpriteFont textShortCut, ActionCreatePath action, Vector2 position, string shortCut, List<Keys> keysShortCut)
        {
            Ctx = ctx;
            Action = action;
            TextureButton = textureButton;
            TextShortCut = textShortCut;
            Description = description;
            Position = position;
            ShortCut = shortCut;
            Hoover = false;
            KeysShortCut = keysShortCut;
        }

        public void HandleInput(MouseState newStateMouse, MouseState lastStateMouse, KeyboardState newStateKeyboard, KeyboardState lastStateKeyboard)
        {

            Keys[] StatePressedKeys = newStateKeyboard.GetPressedKeys();

            bool hasShortCut = (StatePressedKeys.Length == KeysShortCut.Count) ? true : false;

            for (int nbShortCut = 0; nbShortCut < StatePressedKeys.Length; nbShortCut++)
            {
                Keys pressedShortCut = StatePressedKeys[nbShortCut];
                if (KeysShortCut.Contains(pressedShortCut) == false) hasShortCut = false;
            }
            if (hasShortCut) Ctx.CurrentActionCreatePath = Action;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureButton, Position, Color.White);
            spriteBatch.DrawString(TextShortCut, ShortCut, new Vector2(Position.X + 20 - ShortCut.Length * 2, Position.Y + TextureButton.Height + 5), Color.White);
        }
    }
}
