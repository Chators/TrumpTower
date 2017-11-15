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
    class ButtonUI
    {
        readonly string _name;
        Vector2 _position;
        Texture2D _img;

        public ButtonUI(string name, Vector2 position, Texture2D img)
        {
            _name = name;
            _position = position;
            _img = img;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_img, _position, Color.White);
        }

        public string Name => _name;
        public Texture2D Texture => _img;
        public Vector2 Position => _position;
    }
}
