using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;

namespace TrumpTower.Sprites
{
    class SpriteDollars
    {
        Map _map;
        Dictionary<string, Texture2D> Textures { get; set; }
        SpriteFont SpriteFontDollars { get; set; }
        SpriteBatch _spriteBatch;

        public SpriteDollars(Map map, Dictionary<string, Texture2D> textures, SpriteFont spriteFontDollars, SpriteBatch spriteBatch)
        {
            _map = map;
            Textures = textures;
            SpriteFontDollars = spriteFontDollars;
            _spriteBatch = spriteBatch;
        }

        public void Draw()
        {
            Vector2 _positionDollars = new Vector2(10, 10);
            Rectangle _overlayDollars = new Rectangle(0, 0, 150, 33);
            _spriteBatch.Draw(Textures["spriteDollars"], new Vector2(5, 10), _overlayDollars, Color.Black * 0.6f);
            _spriteBatch.Draw(Textures["dollarsImg"], _positionDollars, Color.White);
            _spriteBatch.DrawString(SpriteFontDollars, _map.Dollars + "", new Vector2(50, 17), Color.White);
        }
    }
}
