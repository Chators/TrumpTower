using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Animation.SpriteSheet
{
    public class SimpleAnimationDefinition
    {
        public Game1Menu Ctx { get; set; }
        Game Game { get; set; }
        public string AssetName { get; set; }
        public Point FrameSize { get; set; }
        public Point NbFrames { get; set; }
        public int FrameRate { get; set; }
        public bool Loop { get; set; }
        public Texture2D Sprite { get; private set; }
        public double TimeBetweenFrame = 16;
        public List<SimpleAnimationSprite> AnimatedSprite { get; set; }

        public SimpleAnimationDefinition(Game1Menu ctx, Game game, string assetName, Point frameSize, Point nbFrames, int frameRate, bool loop)
        {
            Ctx = ctx;
            Game = game;
            AssetName = assetName;
            FrameSize = frameSize;
            NbFrames = nbFrames;
            FrameRate = frameRate;
            Loop = loop;
            AnimatedSprite = new List<SimpleAnimationSprite>();
        }

        public void Initialize()
        {
            /* Initialisation */
            Framerate = FrameRate;
        }

        public void LoadContent(SpriteBatch spritebatch = null)
        {
            /* Chargements des donnees */
            Sprite = Ctx.Content.Load<Texture2D>(AssetName);
        }

        private int _Framerate = 60;
        public int Framerate
        {
            get { return _Framerate; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Framerate can't be less or equal to 0");
                if (_Framerate != value)
                {
                    _Framerate = value;
                    TimeBetweenFrame = 1000.0d / (double)this._Framerate;
                }
            }
        }
    }
}
