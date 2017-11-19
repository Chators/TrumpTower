using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.Draw.DollarsAnimations
{
    public class DollarsAnimationsDefinition
    {
        public Vector2 _startPosition;
        public Vector2 EndPosition { get; set; }
        public SpriteFont Sprite { get; set; }
        public int Speed { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        int CurrentDollars { get; set; }
        int LastDollars { get; set; }
        public Color WinMoney { get; set; }
        public Color LoseMoney { get; set; }
        public List<DollarsAnimationsSprite> DollarsArray { get; set; }

        public DollarsAnimationsDefinition(Vector2 startPosition, SpriteFont sprite, SpriteBatch spriteBatch, int dollars)
        {
            _startPosition = startPosition;
            EndPosition = new Vector2(startPosition.X+250, startPosition.Y-50);
            Sprite = sprite;
            Speed = 5;
            SpriteBatch = spriteBatch;
            CurrentDollars = dollars;
            LastDollars = dollars;
            WinMoney = Color.Green;
            LoseMoney = Color.Red;
            DollarsArray = new List<DollarsAnimationsSprite>();
        }

        public void Update(int dollars)
        {
            if (dollars != CurrentDollars) CurrentDollars = dollars;
            for (int i = 0; i < DollarsArray.Count; i++)
            {
                DollarsAnimationsSprite sprite = DollarsArray[i];
                sprite.Update();
            }

            if (CurrentDollars != LastDollars)
            {
                if (CurrentDollars > LastDollars) DollarsArray.Add(new DollarsAnimationsSprite(this, SpriteBatch, CurrentDollars - LastDollars, true, WinMoney));
                if (CurrentDollars < LastDollars) DollarsArray.Add(new DollarsAnimationsSprite(this, SpriteBatch, CurrentDollars - LastDollars, false, LoseMoney));
                LastDollars = CurrentDollars;
            }
        }

        public void Draw()
        {
            for (int i = 0; i < DollarsArray.Count; i++)
            {
                DollarsAnimationsSprite sprite = DollarsArray[i];
                sprite.Draw();
            }
        }
    }
}
