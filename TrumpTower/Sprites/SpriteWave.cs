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
    class SpriteWave
    {
        Game1 Ctx { get; set; }
        Dictionary<string, Texture2D> Textures { get; set; }
        SpriteFont SpriteFont { get; set; }
        SpriteBatch _spriteBatch;

        public SpriteWave(Game1 ctx, Dictionary<string, Texture2D> textures, SpriteFont spriteFont, SpriteBatch spriteBatch)
        {
            Ctx = ctx;
            Textures = textures;
            SpriteFont = spriteFont;
            _spriteBatch = spriteBatch;
        }

        public void Draw()
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, 270, 33);
            _spriteBatch.Draw(Textures["overlayWave"], new Vector2(5, 50), sourceRectangle, Color.Black * 0.6f);
            _spriteBatch.Draw(Textures["flagNorthKorea"], new Vector2(10, 50), Color.White);
            _spriteBatch.DrawString(SpriteFont, "Vagues " + Map.WavesCounter + "/" + Map.WavesTotals, new Vector2(50, 57), Color.White);
        }
    }
}
