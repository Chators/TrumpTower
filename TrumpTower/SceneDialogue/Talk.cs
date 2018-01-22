using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.SceneDialogue
{
    class Talk
    {
        MainScene Ctx { get; set; }
        Texture2D Character { get; set; }
        Texture2D Text { get; set; }

        public Talk(MainScene ctx, Texture2D character, Texture2D text)
        {
            Ctx = ctx;
            Character = character;
            Text = text;
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Text, new Vector2((Ctx.WWidth / 2) - (Text.Width / 2), Ctx.WHeight - (Ctx.WHeight * 10 / 100) - Text.Height), Color.White);
        }

        public bool IsLeft => Ctx._imgEntityLeft == Character;
    }
}
