using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;
using TrumpTower.LibraryTrumpTower.Spawns;

namespace TrumpTower.Drawing
{
    class WaveIsComingImg
    {
        Vector2 _position;
        public Wave WaveIsComing { get; private set; }
        Map _map;
        static SpriteFont _imgTimer;
        static Texture2D _imgNorthKoreaIsComing;
        float compteur;
        float add;
        private float angle;

        public WaveIsComingImg(Map map, Wave wave)
        {
            _map = map;
            WaveIsComing = wave;
            compteur = 0.3f;
            add = 0.01f;
            angle = 0;
            
        }

        public static void LoadContent(ContentManager Content)
        {
            _imgNorthKoreaIsComing = Content.Load<Texture2D>("north_korea_is_comming");
            _imgTimer = Content.Load<SpriteFont>("timerIsComming");
        }

        public void Update(Wave wave)
        {
            if (compteur <= 0.2 || compteur >= 0.7) add = -add;
            compteur += add;
            WaveIsComing = wave;
            if (wave != null)
            {
                //Left
                if (wave.Position.X / Constant.imgSizeMap == 0)
                    _position = new Vector2(WaveIsComing.Position.X + _imgNorthKoreaIsComing.Width / 2 + 30, WaveIsComing.Position.Y + _imgNorthKoreaIsComing.Height / 2);
                //Right
                else if (wave.Position.X / Constant.imgSizeMap == _map.WidthArrayMap - 1)
                    _position = new Vector2(WaveIsComing.Position.X + _imgNorthKoreaIsComing.Width / 2 - 30, WaveIsComing.Position.Y + _imgNorthKoreaIsComing.Height / 2);
                //Top
                else if (wave.Position.Y / Constant.imgSizeMap == 0)
                    _position = new Vector2(WaveIsComing.Position.X + _imgNorthKoreaIsComing.Width / 2, WaveIsComing.Position.Y + _imgNorthKoreaIsComing.Height / 2 + 30);
                //Down
                else if (wave.Position.Y / Constant.imgSizeMap == _map.HeightArrayMap - 1)
                    _position = new Vector2(WaveIsComing.Position.X + _imgNorthKoreaIsComing.Width / 2, WaveIsComing.Position.Y + _imgNorthKoreaIsComing.Height / 2 - 30);
            }
        }

        public void Draw(GraphicsDevice GraphicsDevice, SpriteBatch spriteBatch)
        {
            Wave waveIsComming = Map.WaveIsComming;
            if (waveIsComming != null)
            {
                if (waveIsComming.TimerBeforeStarting > 0)
                {
                    Rectangle sourceRectangle = new Rectangle(0, 0, _imgNorthKoreaIsComing.Width, _imgNorthKoreaIsComing.Height);
                    Vector2 origin = new Vector2(_imgNorthKoreaIsComing.Width / 2, _imgNorthKoreaIsComing.Height / 2);
                    spriteBatch.Draw(_imgNorthKoreaIsComing, _position, null, Color.White * compteur, angle, origin, 1.0f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(_imgTimer, WaveIsComing.TimerBeforeStarting/60+"", new Vector2(_position.X-4, _position.Y-10), Color.White * (compteur + 0.2f));
                }
            }
        }
    }
}
