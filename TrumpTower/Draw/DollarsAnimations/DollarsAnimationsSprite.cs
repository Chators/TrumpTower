using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.Draw.DollarsAnimations
{
    public class DollarsAnimationsSprite
    {
        DollarsAnimationsDefinition Def { get; set; }
        Vector2 _position;
        public SpriteBatch SpriteBatch { get; set; }
        public int Dollars { get; private set; }
        public bool IsWin { get; private set; }
        public Color Color { get; private set; }

        public DollarsAnimationsSprite (DollarsAnimationsDefinition def, SpriteBatch spriteBatch, int dollars, bool isWin, Color color)
        {
            Def = def;
            _position = def._startPosition;
            Dollars = dollars;
            SpriteBatch = spriteBatch;
            IsWin = isWin;
            Color = color;
        }

        public void Update()
        {
            if (_position.X != Def.EndPosition.X) _position.X += Def.Speed;
            else if (_position.Y != Def.EndPosition.Y) _position.Y -= Def.Speed;

            if (_position == Def.EndPosition) Def.DollarsArray.Remove(this);
        }

        public void Draw()
        {
            string textDollars = Dollars+"";
            if (IsWin) textDollars = "+" + Dollars;
            SpriteBatch.DrawString(Def.Sprite, textDollars, _position, Color);
        }
    }
}
