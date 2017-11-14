using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.Drawing
{
    class HealthBar
    {
        static Texture2D _imgLife;
        double _currentHp;
        double _maxHp;

        public HealthBar(double currentHp, double maxHp)
        {
            _currentHp = currentHp;
            _maxHp = maxHp;
        }

        static public void LoadContent(ContentManager Content)
        {
            _imgLife = Content.Load<Texture2D>("life");
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 target, Texture2D imgTarget)
        {
            Rectangle lifeRectangle = LifeRectangle(target, imgTarget);
            spriteBatch.Draw(_imgLife, lifeRectangle, Color.Green);
            Rectangle lessLifeRectangle = LessLifeRectangle(lifeRectangle);
            spriteBatch.Draw(_imgLife, lessLifeRectangle, Color.Red);
        }

        private Rectangle LifeRectangle(Vector2 target, Texture2D imgTarget)
        {
            double positionX = target.X + (imgTarget.Width / 2) - (_imgLife.Width / 2);
            double positionY = target.Y - _imgLife.Height;
            double lessPercent = _imgLife.Width * (1 - (((_maxHp - _currentHp) * 100 / _maxHp) / 100));
            return new Rectangle((int)positionX, (int)positionY, (int)lessPercent, _imgLife.Height);
        }

        private Rectangle LessLifeRectangle(Rectangle lifeRectangle)
        {
            int positionX = lifeRectangle.X + lifeRectangle.Width;
            double addPercent = _imgLife.Width * ((_maxHp - _currentHp) * 100 / _maxHp) / 100;
            return new Rectangle(positionX, lifeRectangle.Y, (int)addPercent, _imgLife.Height);
        }
    }
}
