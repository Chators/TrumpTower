﻿using LibraryTrumpTower.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TrumpTower.Draw;
using TrumpTower.Drawing;
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

        // MAPS
        int[,] _mapPoint;
        Map _map;
        List<Texture2D> _imgMaps;

        // TOWER SELECTOR
        Vector2 _towerSelector;
        bool _verif;
        Texture2D _imgSelector;

        // WALL
        Vector2 _towerSelectorUpgrade;
        Texture2D _imgWall;

        // ENEMIES
        Texture2D _imgUpgrade;
        Texture2D _imgSell;
        Texture2D _imgEnemy1;
        
        // TOWERS
        Texture2D _imgTower1;
        Texture2D _imgTower2;
        Texture2D _imgTower3;
        
        // SOUND
        Texture2D _imgTower3;
        Texture2D _imgTower1_2;
        Texture2D _imgTower2_2;
        Texture2D _imgTower3_2;
        Texture2D _imgTower1_3;
        Texture2D _imgTower2_3;
        Texture2D _imgTower3_3;
        bool _verif;
        bool _verif2;
        Texture2D _imgSelector;
        SoundEffect _explosion;
        SoundEffect _manDie;

        // WAVES
        SpriteFont _imgDollars;
        SpriteFont _imgNextWave;
        WaveIsComingImg _waveSprite;
        Texture2D _flagNorthKorea;

        // MISSILES
        Texture2D _imgMissile;
        Texture2D _imgMissile1;

        // DOLLARS
        SpriteFont _spriteDollars;
        Texture2D _imgDollars;
        Texture2D _backgroundDollars;

        // BUTTONS
        Dictionary<string,ButtonUI> _buttonsUI;
        public ButtonUI ButtonIsHover { get; private set; }
        public ButtonUI ButtonIsActivated { get; private set; }

        // TIMER BUTTON
        Texture2D _pauseButton;
        Texture2D _normalButton;
        Texture2D _fastButton;

        // PAUSE
        bool gameIsPaused;

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
            _verif = false;
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
            _towerSelector = new Vector2(-1000, -1000);
            _towerSelectorUpgrade = new Vector2(-1000, -1000);
            graphics.PreferredBackBufferWidth = _mapPoint.GetLength(1) * Constant.imgSizeMap;
            graphics.PreferredBackBufferHeight = _mapPoint.GetLength(0) * Constant.imgSizeMap;
            graphics.ApplyChanges();

            _waveSprite = new WaveIsComingImg(_map, Map.WaveIsComming);
            _buttonsUI = new Dictionary<string, ButtonUI>();

            
            gameIsPaused = false;

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
            _imgTower2 = Content.Load<Texture2D>("tower2");
            _imgTower3 = Content.Load<Texture2D>("tower3");
            _imgTower1_2 = Content.Load<Texture2D>("tower1_2");
            _imgTower2_2 = Content.Load<Texture2D>("tower2_2");
            _imgTower3_2 = Content.Load<Texture2D>("tower3_2");
            _imgTower1_3 = Content.Load<Texture2D>("tower1_3");
            _imgTower2_3 = Content.Load<Texture2D>("tower2_3");
            _imgTower3_3 = Content.Load<Texture2D>("tower3_3");
            _explosion = Content.Load<SoundEffect>("songExplosion");
            _imgUpgrade = Content.Load<Texture2D>("upgrade");
            _imgSell = Content.Load<Texture2D>("sell");

            // MISSILE 
            _imgMissile = Content.Load<Texture2D>("missile2");
            _imgMissile1 = Content.Load<Texture2D>("missile1");

            // TEXT
            _imgDollars = Content.Load<SpriteFont>("dollars");
            _imgNextWave = Content.Load<SpriteFont>("next_wave");

            // TIMER BUTTON
            _fastButton = Content.Load<Texture2D>("ManagerTime/fastButton");
            Vector2 _positionFastButton = new Vector2(_mapPoint.GetLength(1) * Constant.imgSizeMap - 50, 10);
            _buttonsUI["fastTimer"] = new ButtonUI(this, "fastTimer", _positionFastButton, _fastButton);
            
            _normalButton = Content.Load<Texture2D>("ManagerTime/normalButton");
            Vector2 _positionNormalButton = new Vector2(_positionFastButton.X - 50, 10);
            _buttonsUI["normalTimer"] = new ButtonUI(this, "normalTimer", _positionNormalButton, _normalButton);

            _pauseButton = Content.Load<Texture2D>("ManagerTime/pauseButton");
            Vector2 _positionPauseButton = new Vector2(_positionNormalButton.X - 50, 10);
            _buttonsUI["pauseTimer"] = new ButtonUI(this, "pauseTimer", _positionPauseButton, _pauseButton);

            // WAVE
            _imgNextWave = Content.Load<SpriteFont>("NextWave/next_wave");
            // TEXTURE KOREA
            WaveIsComingImg.LoadContent(Content);

            //SELECTOR
            _imgSelector = Content.Load<Texture2D>("selector");

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
            MouseState newStateMouse = Mouse.GetState();
            HandleInput(newStateMouse, lastStateMouse);
            lastStateMouse = newStateMouse;
            
            if (!gameIsPaused)
            {
                if (_map.Wall.IsDead()) Exit(); // If base loses hp, game will exit.
                _map.Update();
                _waveSprite.Update(Map.WaveIsComming);
            }

            base.Update(gameTime);
        }

        protected void HandleInput(MouseState newStateMouse, MouseState lastStateMouse)
        {
            ButtonIsHover = null;
            foreach (ButtonUI button in _buttonsUI.Values)
            {
                if (newStateMouse.X > button.Position.X  &&
                    newStateMouse.X < button.Position.X + button.Texture.Width &&
                    newStateMouse.Y > button.Position.Y  &&
                    newStateMouse.Y < button.Position.Y + button.Texture.Height)
                {
                    if (button.Name == "pauseTimer")
                    {
                        if (newStateMouse.LeftButton == ButtonState.Pressed &&
                            lastStateMouse.LeftButton == ButtonState.Released)
                        {
                            if (gameIsPaused == false)
                            {
                                ButtonIsActivated = button;
                                gameIsPaused = true;
                            }
                            else
                            {
                                ButtonIsActivated = _buttonsUI["normalTimer"];
                                TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);
                                gameIsPaused = false;
                            }
                        }
                        ButtonIsHover = button;
                    }
                    else if (button.Name == "normalTimer")
                    {
                        if (newStateMouse.LeftButton == ButtonState.Pressed &&
                            lastStateMouse.LeftButton == ButtonState.Released)
                        {
                            ButtonIsActivated = button;
                            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);
                            gameIsPaused = false;
                        }
                        ButtonIsHover = button;
                    }
                    else if (button.Name == "fastTimer")
                    {
                        if (newStateMouse.LeftButton == ButtonState.Pressed &&
                            lastStateMouse.LeftButton == ButtonState.Released)
                        {
                            ButtonIsActivated = button;
                            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 120.0f);
                            gameIsPaused = false;
                        }
                        ButtonIsHover = button;
                    }
                }
            }
            // Buy Tower
            List<Vector2> emptyTowers = _map.SearchPositionTextureInArray(MapTexture.emptyTower);
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
                        _towerSelector = new Vector2(position.X * Constant.imgSizeMap, position.Y * Constant.imgSizeMap);
                        _verif = true;
                    }
                }
            }

            if (newStateMouse.LeftButton == ButtonState.Pressed &&
            lastStateMouse.LeftButton == ButtonState.Released)
            {   
                if (_towerSelector != new Vector2(-1000, -1000))
                {
                    if (newStateMouse.X > _towerSelector.X - Constant.imgSizeMap &&
                    newStateMouse.X < (_towerSelector.X + Constant.imgSizeMap) - Constant.imgSizeMap &&
                    newStateMouse.Y > _towerSelector.Y - Constant.imgSizeMap &&
                    newStateMouse.Y < (_towerSelector.Y + Constant.imgSizeMap) - Constant.imgSizeMap)
                    {
                        if (_map.Dollars >= Tower.TowerPrice(TowerType.simple))
                        {
                            _map.CreateTower(new Tower(_map, TowerType.simple, 1, _towerSelector));
                            _map.ChangeLocation((int)_towerSelector.X / Constant.imgSizeMap, (int)_towerSelector.Y / Constant.imgSizeMap, (int)MapTexture.notEmptyTower);
                            _map.Dollars -= Tower.TowerPrice(TowerType.simple);
                            _towerSelector = new Vector2(-1000, -1000);
                        }
                    }
                    else if (newStateMouse.X > _towerSelector.X + Constant.imgSizeMap &&
                    newStateMouse.X < (_towerSelector.X + Constant.imgSizeMap) + Constant.imgSizeMap &&
                    newStateMouse.Y > _towerSelector.Y - Constant.imgSizeMap &&
                    newStateMouse.Y < (_towerSelector.Y + Constant.imgSizeMap) - Constant.imgSizeMap)
                    {
                        if (_map.Dollars >= Tower.TowerPrice(TowerType.slow))
                        {
                            _map.CreateTower(new Tower(_map, TowerType.slow, 1, _towerSelector));
                            _map.ChangeLocation((int)_towerSelector.X / Constant.imgSizeMap, (int)_towerSelector.Y / Constant.imgSizeMap, (int)MapTexture.notEmptyTower);
                            _map.Dollars -= Tower.TowerPrice(TowerType.slow);
                            _towerSelector = new Vector2(-1000, -1000);
                        }
                    }
                    else if (newStateMouse.X > _towerSelector.X - Constant.imgSizeMap &&
                    newStateMouse.X < (_towerSelector.X + Constant.imgSizeMap) - Constant.imgSizeMap &&
                    newStateMouse.Y > _towerSelector.Y + Constant.imgSizeMap &&
                    newStateMouse.Y < (_towerSelector.Y + Constant.imgSizeMap) + Constant.imgSizeMap)
                    {
                        if (_map.Dollars >= Tower.TowerPrice(TowerType.area))
                        {
                            _map.CreateTower(new Tower(_map, TowerType.area, 1, _towerSelector));
                            _map.ChangeLocation((int)_towerSelector.X / Constant.imgSizeMap, (int)_towerSelector.Y / Constant.imgSizeMap, (int)MapTexture.notEmptyTower);
                            _map.Dollars -= Tower.TowerPrice(TowerType.area);
                            _towerSelector = new Vector2(-1000, -1000);
                        }
                    }
                    else if (_verif == false)
                    {
                        _towerSelector = new Vector2(-1000, -1000);
                    }
                    _verif = false;
                }
            }
            

            //Upgrade or sell towers
            
            if (newStateMouse.LeftButton == ButtonState.Pressed &&
            lastStateMouse.LeftButton == ButtonState.Released)
            {
                foreach (Tower tow in _map.Towers)
                {
                    if (newStateMouse.X > tow.Position.X &&
                        newStateMouse.X < tow.Position.X + _imgMaps[5].Width &&
                        newStateMouse.Y > tow.Position.Y &&
                        newStateMouse.Y < tow.Position.Y + _imgMaps[5].Height)
                    {
                        _towerSelectorUpgrade = new Vector2(tow.Position.X, tow.Position.Y);
                        _verif2 = true;
                    }
                }
            }
            if (newStateMouse.LeftButton == ButtonState.Pressed &&
            lastStateMouse.LeftButton == ButtonState.Released)
            {
                if (_towerSelectorUpgrade != new Vector2(-1000, -1000))
                {
                    if (newStateMouse.X > _towerSelectorUpgrade.X &&
                    newStateMouse.X < (_towerSelectorUpgrade.X + Constant.imgSizeMap) &&
                    newStateMouse.Y > _towerSelectorUpgrade.Y - Constant.imgSizeMap &&
                    newStateMouse.Y < (_towerSelectorUpgrade.Y + Constant.imgSizeMap) - Constant.imgSizeMap)
                    {
                        foreach(Tower t in _map.Towers)
                        {
                            if (newStateMouse.X > t.Position.X &&
                            newStateMouse.X < (t.Position.X + Constant.imgSizeMap) &&
                            newStateMouse.Y > t.Position.Y - Constant.imgSizeMap &&
                            newStateMouse.Y < (t.Position.Y + Constant.imgSizeMap) - Constant.imgSizeMap)
                            {
                                Console.WriteLine("lvl : " + t.TowerLvl);
                                Console.WriteLine("type : " + t.Type);
                                if (_map.Dollars >= Tower.TowerPrice(t.Type) * 1.5)
                                {
                                    t.Upgrade(t);
                                }
                            }
                        }
                        _towerSelectorUpgrade = new Vector2(-1000, -1000);
                    }
                    else if (newStateMouse.X > _towerSelectorUpgrade.X &&
                    newStateMouse.X < (_towerSelectorUpgrade.X + Constant.imgSizeMap) &&
                    newStateMouse.Y > _towerSelectorUpgrade.Y + Constant.imgSizeMap &&
                    newStateMouse.Y < (_towerSelectorUpgrade.Y + Constant.imgSizeMap) + Constant.imgSizeMap)
                    {
                        for(int j = 0; j < _map.Towers.Count; j++)
                        {
                            Tower tower = _map.Towers[j];
                            _map.Towers.Remove(tower);
                            _map.ChangeLocation((int)tower.Position.X/Constant.imgSizeMap ,(int)tower.Position.Y/Constant.imgSizeMap ,(int)MapTexture.emptyTower);
                            tower.Sell(tower);
                        }
                      
                        _towerSelectorUpgrade = new Vector2(-1000, -1000);
                    }
                    else if (_verif2 == false)
                    {
                        _towerSelectorUpgrade = new Vector2(-1000, -1000);
                    }
                    _verif2 = false;
                }
            }
        }
        
        /// <summary>
        /// This is called when the game should draw itself.elector
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
                float angle = 0;
                if (enemy.CurrentDirection == Move.right) angle = 0;
                else if (enemy.CurrentDirection == Move.down) angle = Constant.PI / 2;
                else if (enemy.CurrentDirection == Move.left) angle = Constant.PI;
                else if (enemy.CurrentDirection == Move.top) angle = 3 * Constant.PI / 2;
                Rectangle sourceRectangle = new Rectangle(0, 0, _imgEnemy1.Width, _imgEnemy1.Height);
                Vector2 origin = new Vector2(_imgEnemy1.Width / 2, _imgEnemy1.Height / 2);
                spriteBatch.Draw(_imgEnemy1, new Vector2(enemy.Position.X + (_imgEnemy1.Width / 2), enemy.Position.Y + (_imgEnemy1.Height / 2)), null, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);
                HealthBar enemyHealthBar = new HealthBar(enemy.CurrentHp, enemy.MaxHp);
                enemyHealthBar.Draw(spriteBatch, enemy.Position, _imgEnemy1);
            }

            //TOWERS
            
            foreach (Tower tower in _map.Towers)
            {
                if (tower.Type == TowerType.simple)
                {
                    if (tower.TowerLvl == 1)
                    {
                        spriteBatch.Draw(_imgTower1, tower.Position, null, Color.White);
                    }
                    else if (tower.TowerLvl == 2)
                    {
                        spriteBatch.Draw(_imgTower1_2, tower.Position, null, Color.White);
                    }
                    else if(tower.TowerLvl == 3)
                    {
                        spriteBatch.Draw(_imgTower1_3, tower.Position, null, Color.White);
                    }
                }
                else if (tower.Type == TowerType.slow)
                {
                    if (tower.TowerLvl == 1)
                    {
                        spriteBatch.Draw(_imgTower2, tower.Position, null, Color.White);
                    }
                    else if (tower.TowerLvl == 2)
                    {
                        spriteBatch.Draw(_imgTower2_2, tower.Position, null, Color.White);
                    }
                    else if (tower.TowerLvl == 3)
                    {
                        spriteBatch.Draw(_imgTower2_3, tower.Position, null, Color.White);
                    }
                }
                else if (tower.Type == TowerType.area)
                {
                    if (tower.TowerLvl == 1)
                    {
                        spriteBatch.Draw(_imgTower3, tower.Position, null, Color.White);
                    }
                    else if (tower.TowerLvl == 2)
                    {
                        spriteBatch.Draw(_imgTower3_2, tower.Position, null, Color.White);
                    }
                    else if (tower.TowerLvl == 3)
                    {
                        spriteBatch.Draw(_imgTower3_3, tower.Position, null, Color.White);
                    }
                }
            }

            if (_towerSelector != new Vector2(-1000, -1000))
            {

                spriteBatch.Draw(_imgSelector, _towerSelector + new Vector2(-Constant.imgSizeMap, -Constant.imgSizeMap), null, Color.White);
                spriteBatch.Draw(_imgTower1, _towerSelector + new Vector2(-Constant.imgSizeMap, -Constant.imgSizeMap), null, Color.White);
                spriteBatch.Draw(_imgSelector, _towerSelector + new Vector2(Constant.imgSizeMap, -Constant.imgSizeMap), null, Color.White);
                spriteBatch.Draw(_imgTower2, _towerSelector + new Vector2(Constant.imgSizeMap, -Constant.imgSizeMap), null, Color.White);
                spriteBatch.Draw(_imgSelector, _towerSelector + new Vector2(-Constant.imgSizeMap, Constant.imgSizeMap), null, Color.White);
                spriteBatch.Draw(_imgTower3, _towerSelector + new Vector2(-Constant.imgSizeMap, Constant.imgSizeMap), null, Color.White);
                spriteBatch.Draw(_imgSelector, _towerSelector + new Vector2(Constant.imgSizeMap, Constant.imgSizeMap), null, Color.White);
            }
            if (_towerSelectorUpgrade != new Vector2(-1000, -1000))
            {

                spriteBatch.Draw(_imgSelector, _towerSelectorUpgrade + new Vector2(0, -(Constant.imgSizeMap +5)), null, Color.White);
                spriteBatch.Draw(_imgUpgrade, _towerSelectorUpgrade + new Vector2(0, -(Constant.imgSizeMap +5)), null, Color.White);
                spriteBatch.Draw(_imgSelector, _towerSelectorUpgrade + new Vector2(0, (Constant.imgSizeMap +5)), null, Color.White);
                spriteBatch.Draw(_imgSell, _towerSelectorUpgrade + new Vector2(0, (Constant.imgSizeMap +5)), null, Color.White);
            }

            //MISSILES
            List<Missile> _missiles = _map.Missiles;
            foreach (Missile missile in _missiles) spriteBatch.Draw(_imgMissile, missile.Position, null, Color.White);
            

            foreach (Missile missile in _missiles)
            {
                if(missile.Tower == TowerType.simple)
                {
                    spriteBatch.Draw(_imgMissile, missile.Position, null, Color.White);
                }
                else if (missile.Tower == TowerType.slow)
                {
                    spriteBatch.Draw(_imgMissile, missile.Position, null, Color.White);
                }
                else if (missile.Tower == TowerType.area)
                {
                    spriteBatch.Draw(_imgMissile1, missile.Position, null, Color.White);
                }
            }

            //TEXT DOLLARS
            Vector2 _positionDollars = new Vector2(10, 10);
            Rectangle _overlayDollars = new Rectangle(0, 0, 150, 33);
            spriteBatch.Draw(_backgroundDollars, new Vector2(5, 10), _overlayDollars, Color.Black*0.6f);
            spriteBatch.Draw(_imgDollars, _positionDollars, Color.White);
            spriteBatch.DrawString(_spriteDollars, _map.Dollars+"", new Vector2(50, 17), Color.White);
            // TIMER BUTTON
            Vector2 _overlayManageTimePosition = new Vector2(_buttonsUI["pauseTimer"].Position.X - 5, _buttonsUI["pauseTimer"].Position.Y-5);
            Rectangle _overlayManageTime = new Rectangle(0, 0, 143, 42);
            spriteBatch.Draw(_backgroundDollars, _overlayManageTimePosition, _overlayManageTime, Color.Black * 0.6f);
            _buttonsUI["pauseTimer"].Draw(spriteBatch);
            _buttonsUI["normalTimer"].Draw(spriteBatch);
            _buttonsUI["fastTimer"].Draw(spriteBatch);            
            //TEXT
            spriteBatch.DrawString(_imgDollars, "Dollars : " + _map.Dollars, new Vector2(1620, 10), Color.White);

            // IMG IS COMMING NORTH KOREA MDR
            spriteBatch.DrawString(_imgNextWave, "Vague : " + Map.WavesCounter + "/" + Map.WavesTotals, new Vector2(10, 10), Color.White);
            _waveSprite.Draw(GraphicsDevice, spriteBatch);
              

            spriteBatch.DrawString(_imgNextWave, "Prochaine vague dans : " + Map.TimerNextWave / 60, new Vector2(10, 40), Color.White);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
