﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TrumpTower.Draw;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;
using TrumpTower.LibraryTrumpTower.Spawns;

namespace TrumpTower
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int[,] _mapPoint;
        Map _map;
        List<Texture2D> _imgMaps;
        Texture2D _imgWall;
        Texture2D _imgEnemy1;
        Texture2D _imgTower1;
        SoundEffect _explosion;
        SoundEffect _manDie;
        SpriteFont _imgDollars;
        SpriteFont _imgNextWave;
        Texture2D _imgMissile;
        WaveIsCommingImg _waveSprite;
        MouseState lastStateMouse;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            _mapPoint = new int[,]
            {
                {1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,10,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 },
                {1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,7 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 },
                {1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,10,5 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 },
                {1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,5 ,0 ,13,2 ,2 ,2 ,2 ,2 ,2 ,2 ,2 ,2 ,2 ,2 ,2 },
                {1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,5 ,0 ,4 ,10,1 ,10,1 ,1 ,10,1 ,1 ,1 ,1 ,1 ,1 },
                {1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,5 ,0 ,16,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 },
                {1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,5 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 },
                {1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,5 ,0 ,13,2 ,2 ,2 ,2 ,2 ,2 ,2 ,2 ,2 ,2 ,2 ,2 },
                {1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,5 ,0 ,4 ,1 ,10,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 },
                {3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,3 ,6 ,5 ,0 ,16,3 ,3 ,3 ,6 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 },
                {0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,4 ,5 ,0 ,0 ,0 ,0 ,0 ,4 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 },
                {2 ,2 ,2 ,2 ,2 ,2 ,2 ,2 ,2 ,2 ,2 ,14,0 ,4 ,9 ,2 ,2 ,2 ,14,0 ,4 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 },
                {1 ,1 ,1 ,1 ,1 ,1 ,1 ,10,1 ,1 ,10,5 ,0 ,4 ,10,1 ,10,10,5 ,0 ,4 ,10,1 ,1 ,1 ,1 ,1 ,1 ,1 },
                {1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,5 ,0 ,16,3 ,3 ,3 ,3 ,15,0 ,4 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 },
                {1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,5 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,4 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 },
                {1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,9 ,2 ,2 ,2 ,2 ,2 ,2 ,2 ,2 ,8 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 }
            };
            _map = new Map(_mapPoint);

            graphics.PreferredBackBufferWidth = _mapPoint.GetLength(1) * Constant.imgSizeMap;
            graphics.PreferredBackBufferHeight = _mapPoint.GetLength(0) * Constant.imgSizeMap;
            graphics.ApplyChanges();

            _waveSprite = new WaveIsCommingImg(_map, Map.WaveIsComming);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // MAP
            _imgMaps = new List<Texture2D>();
            foreach (string name in Enum.GetNames(typeof(MapTexture)))
            {
                _imgMaps.Add(Content.Load<Texture2D>(name));
            }

            // HEALTH BAR
            HealthBar.LoadContent(Content);

            // WALL
            _imgWall = Content.Load<Texture2D>("wall");

            // ENEMY 
            _imgEnemy1 = Content.Load<Texture2D>("enemy1");
            _manDie = Content.Load<SoundEffect>("songManDie");

            // TOWER
            _imgTower1 = Content.Load<Texture2D>("tower1");
            _explosion = Content.Load<SoundEffect>("songExplosion");

            // MISSILE 
            _imgMissile = Content.Load<Texture2D>("missile2");

            // TEXT
            _imgDollars = Content.Load<SpriteFont>("dollars");
            _imgNextWave = Content.Load<SpriteFont>("next_wave");

            // TEXTURE KOREA
            WaveIsCommingImg.LoadContent(Content);

            ManagerSound.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (_map.Wall.IsDead()) Exit(); // If base loses hp, game will exit.
            _map.Update();

            MouseState newStateMouse = Mouse.GetState();
            HandleInput(newStateMouse, lastStateMouse);
            lastStateMouse = newStateMouse;
            _waveSprite.Update(Map.WaveIsComming);

            base.Update(gameTime);
        }

        protected void HandleInput(MouseState newStateMouse, MouseState lastStateMouse)
        {
            // Buy Tower
            List<Vector2> emptyTowers = _map.SearchEmptyTowers();
            if (newStateMouse.LeftButton == ButtonState.Pressed &&
                lastStateMouse.LeftButton == ButtonState.Released)
            {
                foreach (Vector2 position in emptyTowers)
                {
                    if (newStateMouse.X > position.X * Constant.imgSizeMap &&
                        newStateMouse.X < position.X * Constant.imgSizeMap + _imgMaps[5].Width &&
                        newStateMouse.Y > position.Y * Constant.imgSizeMap &&
                        newStateMouse.Y < position.Y * Constant.imgSizeMap + _imgMaps[5].Height)
                    {
                        if (_map.Dollars >= 400)
                        {
                            _map.CreateTower(new Tower(_map, "base", 1, new Vector2(position.X * Constant.imgSizeMap, position.Y * Constant.imgSizeMap)));
                            _map.ChangeLocation((int)position.X, (int)position.Y, (int)MapTexture.notEmptyTower);
                            _map.Dollars -= 400;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            // MAPS
            for (int y = 0; y < _map.HeightArrayMap; y++)
            {
                for (int x = 0; x < _map.WidthArrayMap; x++)
                {
                    spriteBatch.Draw(_imgMaps[_map.GetTypeArray(x, y)], new Vector2(x * Constant.imgSizeMap, y * Constant.imgSizeMap), null, Color.White);
                }
            }
            
            //WALL
            Wall _wall = _map.Wall;
            spriteBatch.Draw(_imgWall, _wall.Position, Color.White);
            HealthBar wallHealthBar = new HealthBar(_wall.CurrentHp, _wall.MaxHp);
            wallHealthBar.Draw(spriteBatch, _wall.Position, _imgWall);

            //ENEMIES
            List<Enemy> _enemies = _map.GetAllEnemies();
            foreach (Enemy enemy in _enemies)
            {
                Rectangle sourceRectangle = new Rectangle(0, 0, _imgEnemy1.Width, _imgEnemy1.Height);
                Vector2 origin = new Vector2(_imgEnemy1.Width / 2, _imgEnemy1.Height / 2);
                spriteBatch.Draw(_imgEnemy1, enemy.Position, sourceRectangle, Color.White, Constant.PI, origin, 1.0f, SpriteEffects.None, 1);
                HealthBar enemyHealthBar = new HealthBar(enemy.CurrentHp, enemy.MaxHp);
                enemyHealthBar.Draw(spriteBatch, enemy.Position, _imgEnemy1);
            }

            //TOWERS
            List<Tower> _towers = _map.Towers;
            foreach (Tower tower in _towers) spriteBatch.Draw(_imgTower1, tower.Position, null, Color.White);

            //MISSILES
            List<Missile> _missiles = _map.Missiles;
            foreach (Missile missile in _missiles) spriteBatch.Draw(_imgMissile, missile.Position, null, Color.White);
            
            //TEXT
            spriteBatch.DrawString(_imgDollars, "Dollars : " + _map.Dollars, new Vector2(1620, 10), Color.White);

            // IMG IS COMMING NORTH KOREA MDR
            spriteBatch.DrawString(_imgNextWave, "Vague : " + Map.WavesCounter + "/" + Map.WavesTotals, new Vector2(10, 10), Color.White);
            _waveSprite.Draw(GraphicsDevice, spriteBatch);
              

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
