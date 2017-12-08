using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;

namespace MapEditorTrumpTower
{
    class SelectorTexture
    {
        public Game1 Ctx { get; set; }
        public Map Map { get; set; }
        public MapTexture Texture { get; set; }
        public Vector2 Hoover { get; set; }

        public SelectorTexture (Game1 ctx, Map map)
        {
            Ctx = ctx;
            Map = map;
            Texture = MapTexture.dirt;
        }

        public void HandleInput(MouseState newStateMouse, MouseState lastStateMouse, KeyboardState newStateKeyboard, KeyboardState lastStateKeyboard)
        {
            Hoover = Vector2.Zero;

            // On verifie qu'on a une selection de Texture
            if (Texture != MapTexture.None)
            {
                Vector2 positionCase = Map.SearchCase(newStateMouse.X, newStateMouse.Y, Constant.imgSizeMap);
                if (positionCase != Vector2.Zero)
                {
                    // Si les Textures ne sont pas pareils rien
                    if ((int)Texture != Map.MapArray[(int)positionCase.Y, (int)positionCase.X])
                    {
                        // Si click on change
                        if (newStateMouse.LeftButton == ButtonState.Pressed && lastStateMouse.LeftButton == ButtonState.Released)
                        {
                            Map.ChangeLocation((int)positionCase.X, (int)positionCase.Y, (int)Texture);
                        }
                        // Sinon on previsualise en vert
                        else
                        {
                            Hoover = new Vector2(positionCase.X, positionCase.Y);
                        }
                    }
                }
            }
        }

        public void Update()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Hoover
            if (Hoover != Vector2.Zero)
            {
                Texture2D _currentTexture = Ctx.ImgMaps[(int)Texture];
                spriteBatch.Draw(_currentTexture, new Vector2(Hoover.X * _currentTexture.Width, Hoover.Y * _currentTexture.Height), Color.White*0.6f);
            }
        }
    }
}