using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.Draw.ButtonsUI.SpecialAbilities
{
    class ButtonUIAbility
    {
        GroupOfButtonsUIAbilities _ctx;
        readonly string _name;
        Vector2 _position;
        Texture2D _img;

        public ButtonUIAbility(GroupOfButtonsUIAbilities ctx, string name, Vector2 position, Texture2D img)
        {
            _ctx = ctx;
            _name = name;
            _position = position;
            _img = img;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if (_ctx.ButtonHover == this) color = Color.LightSlateGray;
            if (_ctx.ButtonActivated == this) color = Color.LightSlateGray;
            Rectangle reloadedRect = new Rectangle();

            if (_name == "explosionAbility")
            {
                double _currentTimer = _ctx.Explosion.CurrentTimer;
                // Si ce n'est pas rechargé
                if (_currentTimer > 0) color = Color.Gray * 0.6f;
                reloadedRect = new Rectangle((int)_position.X, (int)_position.Y, _img.Width, _img.Height);
                spriteBatch.Draw(_img, reloadedRect, color);

                if (_currentTimer > 0)
                {
                    Vector2 _positionText = new Vector2(reloadedRect.Location.X + (_img.Width / 2) - 19, reloadedRect.Location.Y + (_img.Height / 2) - 8);
                    spriteBatch.DrawString(_ctx.CooldownSprite, _ctx.Explosion.CurrentTimer / 60 + "", _positionText, Color.YellowGreen);
                }
            }


            if(_name == "sniperAbility")
            {
                if (_ctx.Ctx.Map.Dollars < _ctx.Ctx.Map.Sniper.Cost || !_ctx.Ctx.Map.Sniper.IsReload) color = Color.Gray * 0.6f;
                reloadedRect = new Rectangle((int)_position.X, (int)_position.Y, _img.Width, _img.Height);
                spriteBatch.Draw(_img, reloadedRect, color);
            }

            if (_name == "stickyRiceAbility")
            {
                double _currentTimer = _ctx.Ctx.Map.StickyRice.CurrentTimer;
                // Si ce n'est pas rechargé
                if (_currentTimer > 0) color = Color.Gray * 0.6f;
                reloadedRect = new Rectangle((int)_position.X, (int)_position.Y, _img.Width, _img.Height);
                spriteBatch.Draw(_img, reloadedRect, color);

                if (_currentTimer > 0)
                {
                    Vector2 _positionText = new Vector2(reloadedRect.Location.X + (_img.Width / 2) - 19, reloadedRect.Location.Y + (_img.Height / 2) - 8);
                    spriteBatch.DrawString(_ctx.CooldownSprite, (int)_currentTimer / 60 + "", _positionText, Color.YellowGreen);
                }
            }
            if (_name == "wallBossAbility")
            {
                reloadedRect = new Rectangle((int)_position.X, (int)_position.Y, _img.Width, _img.Height);
                spriteBatch.Draw(_img, reloadedRect, color);
            }

            if (_name == "entityAbility")
            {
                if (_ctx.Ctx.Map.Dollars < _ctx.Ctx.Map.Entity.PriceImproveGauge) color = Color.Gray * 0.6f;
                reloadedRect = new Rectangle((int)_position.X, (int)_position.Y, _img.Width, _img.Height);
                spriteBatch.Draw(_img, reloadedRect, color);
            }
        }

        public string Name => _name;
        public Texture2D Texture => _img;
        public Vector2 Position => _position;
    }
}

