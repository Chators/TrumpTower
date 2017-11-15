using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.Draw
{
    public class ButtonUI
    {
        Game1 _ctx;
        readonly string _name;
        Vector2 _position;
        Texture2D _img;

        public ButtonUI(Game1 ctx, string name, Vector2 position, Texture2D img)
        {
            _ctx = ctx;
            _name = name;
            _position = position;
            _img = img;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if (_ctx.ButtonIsHover == this) color = Color.Red;
            if (_ctx.ButtonIsActivated == this) color = Color.Red;
            spriteBatch.Draw(_img, _position, color);
        }

        public string Name => _name;
        public Texture2D Texture => _img;
        public Vector2 Position => _position;
    }
}
