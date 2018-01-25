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
    class KimAnimation : Animation
    {
        public KimAnimation(Game1Menu ctx, Texture2D texture, Vector2 position, bool inAnimation, float transparancy, Color color, SoundEffect announcement) : base(ctx, texture, position, inAnimation, transparancy, color, announcement)
        {
        }

        public override void Update(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            if (gameTime.TotalGameTime > TimeSpan.FromSeconds(5))
            {
                if (FirstApparition)
                {
                    Announcement.Play();
                    FirstApparition = false;
                }

                if (InAnimation)
                {
                    if (_position.Y > graphics.GraphicsDevice.Viewport.Height - Texture.Height + 60) _position.Y -= 28f;
                }
                if (Transparancy < 0.4) TransparancyVariance = 0.03f;
                else if (Transparancy > 1) TransparancyVariance = -0.03f;

                Transparancy += TransparancyVariance;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture, _position, Color * Transparancy);
        }
    }
}
