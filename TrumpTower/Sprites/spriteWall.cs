using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.Drawing;
using TrumpTower.LibraryTrumpTower;

namespace TrumpTower.Sprites
{
    class SpriteWall
    {
        Wall _wall;
        Texture2D _texture;
        SpriteBatch _spriteBatch;

        public SpriteWall(Wall wall, Texture2D texture, SpriteBatch spriteBatch)
        {
            _wall = wall;
            _texture = texture;
            _spriteBatch = spriteBatch;
        }

        public void Draw()
        {
            _spriteBatch.Draw(_texture, _wall.Position, Color.White);
            HealthBar wallHealthBar = new HealthBar(_wall.CurrentHp, _wall.MaxHp, 1.8f);
            wallHealthBar.Draw(_spriteBatch, _wall.Position, _texture);
        }
    }
}
