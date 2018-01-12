using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Menu.Animation
{
    class VersusAnimation : Animation
    {
        TimeSpan lastNextTime = TimeSpan.FromSeconds(0);
        const int nextTime = 20;
        float _moveAnim = 0.1f;

        public VersusAnimation(Game1Menu ctx, Texture2D texture, Vector2 position, bool inAnimation, float transparancy, Color color, SoundEffect announcement) : base(ctx, texture, position, inAnimation, transparancy, color, announcement)
        {
        }

        public override void Update(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            /*if (lastNextTime.Seconds + 20 > gameTime.TotalGameTime.Seconds)
            {
                lastNextTime = gameTime.TotalGameTime;
                InAnimation = true;
            }
            if (InAnimation)
            {
                UpdateMove();
            }
            UpdateMove();*/
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (gameTime.TotalGameTime > TimeSpan.FromSeconds(5))
            {
                spriteBatch.Draw(Texture, _position, Color * Transparancy);
            }
        }

        /*public void UpdateMove()
        {
            //if (Color == Color.White) Color = Color.Red;
            //else Color = Color.White;
            if (_moveAnim > 0) _moveAnim = -5;
            else if (_moveAnim < 0) _moveAnim = 5f;
            _position.X += _moveAnim;
        }*/
    }
}
