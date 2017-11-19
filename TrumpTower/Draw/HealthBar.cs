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
        double _sizeBar;

        public HealthBar(double currentHp, double maxHp, double sizeBar)
        {
            _currentHp = currentHp;
            _maxHp = maxHp;
            _sizeBar = sizeBar;
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
            double positionX = target.X + (imgTarget.Width / 2) - (_imgLife.Width / 2) * _sizeBar;
            double positionY = target.Y - _imgLife.Height * _sizeBar;
            double lessPercent = _imgLife.Width * _sizeBar * (1 - (((_maxHp - _currentHp) * 100 / _maxHp) / 100));
            return new Rectangle((int)positionX, (int)positionY, (int)lessPercent, Convert.ToInt32(_imgLife.Height * _sizeBar));
        }

        private Rectangle LessLifeRectangle(Rectangle lifeRectangle)
        {
            int positionX = lifeRectangle.X + lifeRectangle.Width;
            double addPercent = _imgLife.Width * _sizeBar * ((_maxHp - _currentHp) * 100 / _maxHp) / 100;
            return new Rectangle(positionX, lifeRectangle.Y, (int)addPercent, Convert.ToInt32(_imgLife.Height * _sizeBar));
        }
    }
}
