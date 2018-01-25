using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Animation
{
    abstract class Animation
    {
        internal Game1Menu Ctx {get;set;}
        internal Vector2 _position;
        internal Texture2D Texture { get; set; }
        internal bool InAnimation { get; set; }
        internal float Transparancy { get; set; }
        internal float TransparancyVariance { get; set; }
        internal bool FirstApparition { get; set; }
        internal SoundEffect Announcement { get; set; }
        internal Color Color { get; set; }

        public Animation (Game1Menu ctx, Texture2D texture, Vector2 position, bool inAnimation, float transparancy, Color color, SoundEffect announcement)
        {
            Ctx = ctx;
            Texture = texture;
            _position = position;
            InAnimation = true;
            Transparancy = transparancy;
            TransparancyVariance = -1;
            Announcement = announcement;
            FirstApparition = true;
            Color = color;
        }

        public abstract void Update(GraphicsDeviceManager graphics, GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
