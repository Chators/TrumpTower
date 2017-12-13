using LibraryTrumpTower;
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
using TrumpTower.LibraryTrumpTower.Spawns;

namespace MapEditorTrumpTower
{
    public class SelectorTexture
    {
        public Game1 Ctx { get; set; }
        public Map Map { get; set; }
        public Texture2D CloakTexture { get; set; }
        public MapTexture Texture { get; set; }
        public Vector2 Hoover { get; set; }

        public SelectorTexture (Game1 ctx, Map map, Texture2D cloakTexture)
        {
            Ctx = ctx;
            Map = map;
            CloakTexture = cloakTexture;
            Texture = MapTexture.myBase;
        }

        public void HandleInput(MouseState newStateMouse, MouseState lastStateMouse, KeyboardState newStateKeyboard, KeyboardState lastStateKeyboard)
        {
            Hoover = Vector2.Zero;

            Vector2 positionCase = Map.SearchCase(newStateMouse.X, newStateMouse.Y, Constant.imgSizeMap);

            // On verifie qu'on a une selection de Texture
            if (Texture != MapTexture.None && Texture != MapTexture.myBase)
            {
                if (positionCase != Vector2.Zero)
                {
                    // Si les Textures ne sont pas pareils rien
                    //if ((int)Texture != Map.MapArray[(int)positionCase.Y, (int)positionCase.X])
                    if ((int)Texture != Map.MapArray[(int)positionCase.Y][(int)positionCase.X])
                    {
                        // Si click on change
                        if (newStateMouse.LeftButton == ButtonState.Pressed && lastStateMouse.LeftButton == ButtonState.Released) Map.ChangeLocation((int)positionCase.X, (int)positionCase.Y, (int)Texture);
                        // Sinon on previsualise en vert
                        else Hoover = positionCase;
                    }
                }
            }
            else if (Texture == MapTexture.myBase)
            {
                /*if (Map.MapArray[(int)positionCase.Y, (int)positionCase.X] == (int)MapTexture.dirt &&
                    (positionCase.Y == 0 || positionCase.Y == Map.HeightArrayMap - 1 || positionCase.X == 0 || positionCase.X == Map.WidthArrayMap - 1))*/
                if (Map.MapArray[(int)positionCase.Y][(int)positionCase.X] == (int)MapTexture.dirt &&
                    (positionCase.Y == 0 || positionCase.Y == Map.HeightArrayMap - 1 || positionCase.X == 0 || positionCase.X == Map.WidthArrayMap - 1))
                {
                    if (newStateMouse.LeftButton == ButtonState.Pressed && lastStateMouse.LeftButton == ButtonState.Released) Map.CreateBase(new Wall(Map, 50, new Vector2(positionCase.X * Constant.imgSizeMap, positionCase.Y * Constant.imgSizeMap)));
                    else Hoover = positionCase;
                }
            }
            else if (Texture == MapTexture.None)
            {
                // Si on pointe le Wall
                if (Map.Wall != null && positionCase.X == Map.Wall.Position.X / Constant.imgSizeMap && positionCase.Y == Map.Wall.Position.Y / Constant.imgSizeMap)
                {
                    if (newStateMouse.LeftButton == ButtonState.Pressed && lastStateMouse.LeftButton == ButtonState.Released) Ctx.Wall_Pressed();
                    Hoover = positionCase;
                }
                // Si on pointe une route sur le bord
                /*else if (Map.MapArray[(int)positionCase.Y, (int)positionCase.X] == (int)MapTexture.dirt &&
                    (positionCase.Y == 0 || positionCase.Y == Map.HeightArrayMap - 1 || positionCase.X == 0 || positionCase.X == Map.WidthArrayMap - 1))*/
                else if (Map.MapArray[(int)positionCase.Y][(int)positionCase.X] == (int)MapTexture.dirt &&
                   (positionCase.Y == 0 || positionCase.Y == Map.HeightArrayMap - 1 || positionCase.X == 0 || positionCase.X == Map.WidthArrayMap - 1))
                {
                    Spawn containSpawn = null;
                    for (int i = 0; i < Map.SpawnsEnemies.Count; i++)
                    {
                        Spawn spawn = Map.SpawnsEnemies[i];
                        if (spawn.Position == new Vector2(positionCase.X * Constant.imgSizeMap, positionCase.Y * Constant.imgSizeMap))
                        {
                            containSpawn = spawn;
                            break;
                        }
                    }
                    // Si c'est un spawn
                    if (containSpawn != null)
                    {
                        if (newStateMouse.LeftButton == ButtonState.Pressed && lastStateMouse.LeftButton == ButtonState.Released)
                        {
                            Ctx.Spawn_Pressed(new Vector2(positionCase.X * Constant.imgSizeMap, positionCase.Y * Constant.imgSizeMap));
                        }
                    }
                    // Si c'est juste une route sans spawn
                    else
                    {
                        if (newStateMouse.LeftButton == ButtonState.Pressed && lastStateMouse.LeftButton == ButtonState.Released)
                            Ctx.Road_Pressed(new Vector2(positionCase.X * Constant.imgSizeMap, positionCase.Y * Constant.imgSizeMap));
                    }
                    Hoover = positionCase;
                }
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Hoover
            if (Hoover != Vector2.Zero)
            {
                if (Ctx.SelectTexture.Texture != MapTexture.None)
                {
                    Texture2D _currentTexture;
                    if (Texture == MapTexture.myBase) _currentTexture = Ctx.ImgWall;
                    else _currentTexture = Ctx.ImgMaps[(int)Texture];
                    spriteBatch.Draw(_currentTexture, new Vector2(Hoover.X * _currentTexture.Width, Hoover.Y * _currentTexture.Height), Color.White * 0.6f);
                }
                else
                {
                    spriteBatch.Draw(CloakTexture, new Vector2(Hoover.X * Constant.imgSizeMap, Hoover.Y * Constant.imgSizeMap), Color.White * 0.6f);
                }
            }
        }
    }
}