using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.Draw.Animations
{
    public class SimpleAnimationSprite
    {
        /* Proprietes */
        public Point Position;
        public double Angle;
        protected SimpleAnimationDefinition Definition;
        protected Point CurrentFrame;
        protected SpriteBatch _spriteBatch;
        protected bool FinishedAnimation = false;
        protected double TimeBetweenFrame; // 60 fps 
        protected double lastFrameUpdatedTime = 0;
        public int Time;

        public SimpleAnimationSprite(SimpleAnimationDefinition definition, int X, int Y, int time = 0)
        {
            /* Constructeur */
            Definition = definition;
            Position = new Point();
            CurrentFrame = new Point();
            _spriteBatch = definition.Ctx.SpriteBatch;
            Position.X = X;
            Position.Y = Y;
            TimeBetweenFrame = definition.TimeBetweenFrame;
            if (time != 0) Time = time*60;
            StartAnimation();
        }

        public void Reset()
        {
            /* Reinitialisation de l'animation */
            CurrentFrame = new Point();
            FinishedAnimation = false;
            lastFrameUpdatedTime = 0;
        }

        public void Update(GameTime time)
        {
            Time--;
            /* Mise a jour des donnees en vue de l'affichage */
            if (FinishedAnimation) return;
            this.lastFrameUpdatedTime += time.ElapsedGameTime.Milliseconds;
            if (this.lastFrameUpdatedTime > this.TimeBetweenFrame)
            {
                this.lastFrameUpdatedTime = 0;
                if (this.Definition.Loop)
                {
                    this.CurrentFrame.X++;
                    if (this.CurrentFrame.X >= this.Definition.NbFrames.X)
                    {
                        this.CurrentFrame.X = 0;
                        this.CurrentFrame.Y++;
                        if (this.CurrentFrame.Y >= this.Definition.NbFrames.Y)
                            this.CurrentFrame.Y = 0;
                    }
                    if (Time <= 0)
                    {
                        this.FinishedAnimation = true;
                        Definition.AnimatedSprite.Remove(this);
                    }
                }
                else
                {
                    this.CurrentFrame.X++;
                    if (this.CurrentFrame.X >= this.Definition.NbFrames.X)
                    {
                        this.CurrentFrame.X = 0;
                        this.CurrentFrame.Y++;
                        if (this.CurrentFrame.Y >= this.Definition.NbFrames.Y)
                        {
                            this.CurrentFrame.X = this.Definition.NbFrames.X - 1;
                            this.CurrentFrame.Y = this.Definition.NbFrames.Y - 1;
                            this.FinishedAnimation = true;
                            Definition.AnimatedSprite.Remove(this);
                        }
                    }
                }
            }
        }

        public void Draw(GameTime time, bool DoBeginEnd = true)
        {
            /* Affichage de l'animation */
            if (DoBeginEnd) _spriteBatch.Begin();
            Vector2 origine = new Vector2(Definition.Sprite.Width / 2, Definition.Sprite.Height / 2);
            _spriteBatch.Draw(Definition.Sprite,
                                new Rectangle(this.Position.X + (Definition.Sprite.Width / 2), this.Position.Y + (Definition.Sprite.Height / 2), this.Definition.FrameSize.X, this.Definition.FrameSize.Y),
                                new Rectangle(this.CurrentFrame.X * this.Definition.FrameSize.X, this.CurrentFrame.Y * this.Definition.FrameSize.Y, this.Definition.FrameSize.X, this.Definition.FrameSize.Y),
                                Color.White, 
                                (float)Angle, 
                                origine, 
                                SpriteEffects.None, 
                                1.0f);

            if (DoBeginEnd) _spriteBatch.End();
        }

        public bool StartAnimation()
        {
            if (FinishedAnimation == false) return false;
            Reset();
            FinishedAnimation = false;
            return true;
        }
        public bool IsAnimating() => FinishedAnimation == false;
    }
}
