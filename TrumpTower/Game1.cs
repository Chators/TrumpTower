using LibraryTrumpTower.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using TrumpTower.Draw;
using TrumpTower.Draw.Animations;
using TrumpTower.Draw.ButtonsUI;
using TrumpTower.Draw.ButtonsUI.SpecialAbilities;
using TrumpTower.Draw.DollarsAnimations;
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

        #region Fields

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Maps

        int[,] _mapPoint;
        Map _map;
        List<Texture2D> _imgMaps;
        public float VirtualWidth { get; set; }
        public float VirtualHeight { get; set; }
        Matrix scale;

        #endregion

        #region Towers
        SpriteFont _upgradeFont;
        TowerType _myTow;
            #region Image Tower

            Texture2D _imgTower1;
            Texture2D _imgTower2;
            Texture2D _imgTower3;
            Texture2D _imgTower1_2;
            Texture2D _imgTower2_2;
            Texture2D _imgTower3_2;
            Texture2D _imgTower1_3;
            Texture2D _imgTower2_3;
            Texture2D _imgTower3_3;
            Texture2D _imgWrong;
            Texture2D _imgTower4;
            Texture2D _imgTower4_empty;
       

            #endregion

            #region Towers Selection

            Texture2D _imgSelector;
            Vector2 _towerSelector;
            Texture2D _imgCoin;
            Vector2 _towerSelectorUpgrade;
            Texture2D _imgUpgrade;
            Texture2D _imgSell;
            bool _verif;
            bool _verif2;

            #endregion

        #endregion

        #region Missiles

        Texture2D _imgMissile;
        Texture2D _imgMissile1;

        #endregion

        #region Enemies

        Texture2D _imgEnemy1;
        Texture2D _imgKamikaze;

        #endregion

        #region Wall

        Texture2D _imgWall;

        #endregion

        #region Sound

        SoundEffect _explosion;
        SoundEffect _manDie;

        #endregion

        #region Waves

        SpriteFont _imgNextWave;
        WaveIsComingImg _waveSprite;
        Texture2D _flagNorthKorea;

        #endregion

        #region Dollars

        Texture2D _imgDollars;
        SpriteFont _spriteDollars;
        Texture2D _backgroundDollars;
        DollarsAnimationsDefinition AnimationsDollars;

        #endregion

        #region Buttons

            #region Timer Buttons

            GroupOfButtonsUITimer _groupOfButtonsUITimer;
            Texture2D _pauseButton;
            Texture2D _normalButton;
            Texture2D _fastButton;

            #endregion

            #region Abilities Buttons

            GroupOfButtonsUIAbilities _groupOfButtonsUIAbilities;
            SpriteFont _cooldownSprite;
            public Texture2D ImgExplosionButton { get; private set; }

            #endregion

        #endregion

        #region Cursor

        public Texture2D ImgCursor { get; set; }
        Texture2D _imgCursorBomb;
        Texture2D _imgCursorDefault;

        #endregion

        MouseState newStateMouse;
        MouseState lastStateMouse;
        KeyboardState lastStateKeyboard;
        public bool GameIsPaused { get; set; }
        public SimpleAnimationDefinition[] AnimSprites { get; private set; }

        #endregion

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
            #region Map INIT

            _mapPoint = new int[,]
            {
                {1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 },
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

            #endregion

            #region Graphics Device 
            
            Window.Position = new Point(800, 600);
            

            Window.Title = "Trump Tower";
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;

            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            // Résolution d'écran
            VirtualWidth = _map.WidthArrayMap * Constant.imgSizeMap;
            VirtualHeight = _map.HeightArrayMap * Constant.imgSizeMap;
            scale = Matrix.CreateScale(
                            GraphicsDevice.Viewport.Width / VirtualWidth,
                            GraphicsDevice.Viewport.Height / VirtualHeight,
                            1f);
            graphics.ApplyChanges();

            #endregion

            _waveSprite = new WaveIsComingImg(_map, Map.WaveIsComming);
            _groupOfButtonsUITimer = new GroupOfButtonsUITimer(this);

            // Animations
            AnimSprites = new SimpleAnimationDefinition[2];
            AnimSprites[0] = new SimpleAnimationDefinition(this, this, "animExplosion", new Point(100, 100), new Point(9, 9), 150, false);
            AnimSprites[1] = new SimpleAnimationDefinition(this, this, "Enemies/animBlood", new Point(64, 64), new Point(6, 1), 20, false);
            foreach (SimpleAnimationDefinition anim in this.AnimSprites) anim.Initialize();
            
            GameIsPaused = false;

            #region Tower Selector

            _towerSelector = new Vector2(-1000, -1000);
            _towerSelectorUpgrade = new Vector2(-1000, -1000);
            _verif = false;

            #endregion

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

            #region Maps

            _imgMaps = new List<Texture2D>();
            foreach (string name in Enum.GetNames(typeof(MapTexture)))
            {
                _imgMaps.Add(Content.Load<Texture2D>("Map/" + name));
            }

            #endregion

            #region Wall

            _imgWall = Content.Load<Texture2D>("wall");

            #endregion

            #region Enemies

            _imgEnemy1 = Content.Load<Texture2D>("Enemies/enemy1");
            _imgKamikaze = Content.Load <Texture2D>("Enemies/kamikaze");

            #endregion

            #region Towers

            #region Image Towers

            _imgTower1 = Content.Load<Texture2D>("Towers/tower1");
            _imgTower2 = Content.Load<Texture2D>("Towers/tower2");
            _imgTower3 = Content.Load<Texture2D>("Towers/tower3");
            _imgTower1_2 = Content.Load<Texture2D>("Towers/tower1_2");
            _imgTower2_2 = Content.Load<Texture2D>("Towers/tower2_2");
            _imgTower3_2 = Content.Load<Texture2D>("Towers/tower3_2");
            _imgTower1_3 = Content.Load<Texture2D>("Towers/tower1_3");
            _imgTower2_3 = Content.Load<Texture2D>("Towers/tower2_3");
            _imgTower3_3 = Content.Load<Texture2D>("Towers/tower3_3");
            _imgTower4 = Content.Load<Texture2D>("Towers/bank");
            _imgTower4_empty = Content.Load<Texture2D>("Towers/bank_empty");

            #endregion
            _upgradeFont = Content.Load<SpriteFont>("Towers/dollars");
            #region Towers Selection
            
            _imgSelector = Content.Load<Texture2D>("selector");
            _imgCoin = Content.Load<Texture2D>("Towers/coin");
            _imgUpgrade = Content.Load<Texture2D>("Towers/upgrade");
            _imgSell = Content.Load<Texture2D>("Towers/sell");
            _imgWrong = Content.Load<Texture2D>("Towers/wrong");

            #endregion

            #endregion

            #region Missiles

            _imgMissile = Content.Load<Texture2D>("Missiles/missile2");
            _imgMissile1 = Content.Load<Texture2D>("Missiles/missile1");

            #endregion

            #region Dollars

            _imgDollars = Content.Load<Texture2D>("Dollars/dollarsImg");
            _spriteDollars = Content.Load<SpriteFont>("Dollars/dollars");
            _backgroundDollars = Content.Load<Texture2D>("Dollars/backgroundDollars");
            AnimationsDollars = new DollarsAnimationsDefinition(new Vector2(50, 17), _spriteDollars, spriteBatch, (int)_map.Dollars);

            #endregion

            #region Waves

            _imgNextWave = Content.Load<SpriteFont>("NextWave/next_wave");
            WaveIsComingImg.LoadContent(Content);
            _flagNorthKorea = Content.Load<Texture2D>("NextWave/flagNorthKorea");

            #endregion

            #region Buttons

            #region Timer Buttons

            _groupOfButtonsUITimer = new GroupOfButtonsUITimer(this);

            #region Fast Button

            _fastButton = Content.Load<Texture2D>("ManagerTime/fastButton");
            Vector2 _positionFastButton = new Vector2(VirtualWidth - 50, 10);
            _groupOfButtonsUITimer.CreateButtonUI(new ButtonUITimer(_groupOfButtonsUITimer, "fastTimer", _positionFastButton, _fastButton));

            #endregion

            #region Normal Button

            _normalButton = Content.Load<Texture2D>("ManagerTime/normalButton");
            Vector2 _positionNormalButton = new Vector2(_positionFastButton.X - 50, 10);
            _groupOfButtonsUITimer.CreateButtonUI(new ButtonUITimer(_groupOfButtonsUITimer, "normalTimer", _positionNormalButton, _normalButton));
            _groupOfButtonsUITimer.ButtonActivated = _groupOfButtonsUITimer.ButtonsUIArray["normalTimer"];

            #endregion

            #region Pause Button

            _pauseButton = Content.Load<Texture2D>("ManagerTime/pauseButton");
            Vector2 _positionPauseButton = new Vector2(_positionNormalButton.X - 50, 10);
            _groupOfButtonsUITimer.CreateButtonUI(new ButtonUITimer(_groupOfButtonsUITimer, "pauseTimer", _positionPauseButton, _pauseButton));

            #endregion

            #endregion

            #region Abilities Buttons

            #region Explosion Button

            _cooldownSprite = Content.Load<SpriteFont>("cooldownText");
            _groupOfButtonsUIAbilities = new GroupOfButtonsUIAbilities(this, _map.Explosion, _cooldownSprite);
            ImgExplosionButton = Content.Load<Texture2D>("SpecialAbilities/explosion");
            Vector2 _positionExplosionAbilityButton = new Vector2(15, _mapPoint.GetLength(0) * Constant.imgSizeMap - 80);
            _groupOfButtonsUIAbilities.CreateButtonUI(new ButtonUIAbility(_groupOfButtonsUIAbilities, "explosionAbility", _positionExplosionAbilityButton, ImgExplosionButton));

            #endregion

            #endregion

            #endregion

            #region Cursor

            _imgCursorBomb = Content.Load<Texture2D>("cursorBomb");
            _imgCursorDefault = Content.Load<Texture2D>("cursor");

            #endregion

            #region Sound

            ManagerSound.LoadContent(Content);
            MediaPlayer.Play(ManagerSound.Song1);
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.IsRepeating = true;
            // When Ability Explosion Start
            _explosion = Content.Load<SoundEffect>("Sound/songExplosion");
            // When Enemies Die
            _manDie = Content.Load<SoundEffect>("Sound/songManDie");

            #endregion

            // ANIMATION EXPLOSION ABILITY
            foreach (SimpleAnimationDefinition anim in this.AnimSprites) anim.LoadContent(spriteBatch);
            // HEALTH BAR ON ENEMIES AND WALL
            HealthBar.LoadContent(Content);
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

            #region Prepare and Execut HandleInput

            newStateMouse = Mouse.GetState();
            newStateMouse = new MouseState( (int)(newStateMouse.X * (VirtualWidth / GraphicsDevice.Viewport.Width)),
                                            (int)(newStateMouse.Y * (VirtualHeight / GraphicsDevice.Viewport.Height)),
                                            newStateMouse.ScrollWheelValue,
                                            newStateMouse.LeftButton,
                                            newStateMouse.MiddleButton,
                                            newStateMouse.RightButton,
                                            newStateMouse.XButton1,
                                            newStateMouse.XButton2);

            KeyboardState newStateKeyboard = Keyboard.GetState();
            HandleInput(newStateMouse, lastStateMouse, newStateKeyboard, lastStateKeyboard);

            lastStateMouse = newStateMouse;
            lastStateKeyboard = newStateKeyboard;

            #endregion

            if (!GameIsPaused)
            {
                if (_map.Wall.IsDead()) Exit(); // If base loses hp, game will exit.

                _map.Update();

                _waveSprite.Update(Map.WaveIsComming);

                AnimationsDollars.Update((int)_map.Dollars);

                #region Anim Sprite

                foreach (SimpleAnimationDefinition def in AnimSprites)
                {
                    for (int j = 0; j < def.AnimatedSprite.Count; j++)
                    {
                        SimpleAnimationSprite animatedSprite = def.AnimatedSprite[j];
                        animatedSprite.Update(gameTime);
                    }
                }

                #region Anim Blood

                for (int i = 0; i < _map.DeadEnemies.Count; i++)
                {
                    Enemy deadEnemy = _map.DeadEnemies[i];
                    AnimSprites[1].AnimatedSprite.Add(new SimpleAnimationSprite(AnimSprites[1], (int)deadEnemy.Position.X, (int)deadEnemy.Position.Y));
                    _map.DeadEnemies.Remove(deadEnemy);
                }

                #endregion

                #endregion
            }

            base.Update(gameTime);
        }




        protected void HandleInput(MouseState newStateMouse, MouseState lastStateMouse, KeyboardState newStateKeyboard, KeyboardState lastStateKeyboard)
        {
            
            _groupOfButtonsUITimer.HandleInput(newStateMouse, lastStateMouse, newStateKeyboard, lastStateKeyboard);

            _groupOfButtonsUIAbilities.HandleInput(newStateMouse, lastStateMouse, newStateKeyboard, lastStateKeyboard);

            #region Towers

            #region Buy Tower

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
                    else if (newStateMouse.X > _towerSelector.X + Constant.imgSizeMap &&
                    newStateMouse.X < (_towerSelector.X + Constant.imgSizeMap) + Constant.imgSizeMap &&
                    newStateMouse.Y > _towerSelector.Y + Constant.imgSizeMap &&
                    newStateMouse.Y < (_towerSelector.Y + Constant.imgSizeMap) + Constant.imgSizeMap)
                    {
                        if (_map.Dollars >= Tower.TowerPrice(TowerType.bank))
                        {
                            _map.CreateTower(new Tower(_map, TowerType.bank, 1, _towerSelector));
                            _map.ChangeLocation((int)_towerSelector.X / Constant.imgSizeMap, (int)_towerSelector.Y / Constant.imgSizeMap, (int)MapTexture.notEmptyTower);
                            _map.Dollars -= Tower.TowerPrice(TowerType.bank);
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

            #endregion

            #region Upgrade or sell Towers

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
                        _myTow = tow.Type;
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
                                if (_map.Dollars >= Tower.TowerPrice(t.Type) * 1.5)
                                {
                                    t.Upgrade(t);
                                    ManagerSound.PowerUp.Play();
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
                            if (_map.Towers[j].Position == _towerSelectorUpgrade ) {
                                Tower tower = _map.Towers[j];
                                _map.Towers.Remove(tower);
                                _map.ChangeLocation((int)tower.Position.X / Constant.imgSizeMap, (int)tower.Position.Y / Constant.imgSizeMap, (int)MapTexture.emptyTower);
                                tower.Sell(tower);
                                ManagerSound.Sell.Play();
                            }
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
            if (newStateMouse.RightButton == ButtonState.Pressed &&
                    lastStateMouse.RightButton == ButtonState.Released)
            {
                foreach (Tower tower in _map.Towers)
                {
                    if (newStateMouse.X > tower.Position.X &&
                newStateMouse.X < (tower.Position.X + Constant.imgSizeMap) &&
                newStateMouse.Y > tower.Position.Y &&
                newStateMouse.Y < (tower.Position.Y + Constant.imgSizeMap)
                && tower.Type == TowerType.bank)
                    {
                        for (int j = 0; j < _map.Towers.Count; j++)
                        {
                            if (_map.Towers[j].Position == tower.Position)
                            {
                                Tower tower2 = _map.Towers[j];
                                if (tower2.Reload <= 0)
                                {
                                    _map.Dollars += tower2.Earnings;
                                    tower2.Reload = Constant.BankReloading;
                                }

                            }
                        }
                    }
                }
            }

            #endregion

            #endregion

        }


        /// <summary>
        /// This is called when the game should draw itself.elector
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, scale);

            #region Maps

            for (int y = 0; y < _map.HeightArrayMap; y++)
            {
                for (int x = 0; x < _map.WidthArrayMap; x++)
                {
                    spriteBatch.Draw(_imgMaps[_map.GetTypeArray(x, y)], new Vector2(x * Constant.imgSizeMap, y * Constant.imgSizeMap), null, Color.White);
                }
            }

            #endregion

            #region Wall

            Wall _wall = _map.Wall;
            spriteBatch.Draw(_imgWall, _wall.Position, Color.White);

            HealthBar wallHealthBar = new HealthBar(_wall.CurrentHp, _wall.MaxHp, 1.8f);
            wallHealthBar.Draw(spriteBatch, _wall.Position, _imgWall);

            #endregion

            #region Enemies

            List<Enemy> _enemies = _map.GetAllEnemies();
            foreach (Enemy enemy in _enemies)
            {
                float angle = 0;
                if (enemy.CurrentDirection == Move.right) angle = 0;
                else if (enemy.CurrentDirection == Move.down) angle = Constant.PI / 2;
                else if (enemy.CurrentDirection == Move.left) angle = Constant.PI;
                else if (enemy.CurrentDirection == Move.top) angle = 3 * Constant.PI / 2;
                // CHANGE TEXTURE ENEMY
                Texture2D _imgEnemy = null;
                if (enemy._type == EnemyType.defaultSoldier) _imgEnemy = _imgEnemy1;
                else if (enemy._type == EnemyType.kamikaze) _imgEnemy = _imgKamikaze;
                Rectangle sourceRectangle = new Rectangle(0, 0, _imgEnemy.Width, _imgEnemy.Height);
                Vector2 origin = new Vector2(_imgEnemy.Width / 2, _imgEnemy.Height / 2);
                spriteBatch.Draw(_imgEnemy, new Vector2(enemy.Position.X + (_imgEnemy.Width / 2), enemy.Position.Y + (_imgEnemy.Height / 2)), null, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);
                HealthBar enemyHealthBar = new HealthBar(enemy.CurrentHp, enemy.MaxHp, 1f);
                enemyHealthBar.Draw(spriteBatch, enemy.Position, _imgEnemy);
            }

            #endregion

            #region Towers

            foreach (Tower tower in _map.Towers)
            {
                Texture2D _imgTower = null;
                if (tower.Type == TowerType.simple)
                {
                    if (tower.TowerLvl == 1) _imgTower = _imgTower1;
                    else if (tower.TowerLvl == 2) _imgTower = _imgTower1_2;
                    else if (tower.TowerLvl == 3) _imgTower = _imgTower1_3;
                }
                else if (tower.Type == TowerType.slow)
                {
                    if (tower.TowerLvl == 1) _imgTower = _imgTower2;
                    else if (tower.TowerLvl == 2) _imgTower = _imgTower2_2;
                    else if (tower.TowerLvl == 3) _imgTower = _imgTower2_3;
                }
                else if (tower.Type == TowerType.area)
                {
                    if (tower.TowerLvl == 1) _imgTower = _imgTower3;
                    else if (tower.TowerLvl == 2) _imgTower = _imgTower3_2;
                    else if (tower.TowerLvl == 3) _imgTower = _imgTower3_3;
                }
                else if(tower.Type == TowerType.bank)
                {
                    if (tower.Reload > 0)
                    {
                        if (tower.TowerLvl == 1) _imgTower = _imgTower4_empty;
                        else if (tower.TowerLvl == 2) _imgTower = _imgTower4_empty;
                        else if (tower.TowerLvl == 3) _imgTower = _imgTower4_empty;
                    }
                    else if(tower.Reload <= 0)
                    {
                        
                        if (tower.TowerLvl == 1) _imgTower = _imgTower4;
                        else if (tower.TowerLvl == 2) _imgTower = _imgTower4;
                        else if (tower.TowerLvl == 3) _imgTower = _imgTower4;
                    }
                    if(tower.Reload == 0)
                    {
                        ManagerSound.CoinUp.Play();
                    }
                }

                Rectangle sourceRectangle = new Rectangle(0, 0, _imgTower.Width, _imgTower.Height);
                Vector2 origin = new Vector2(_imgTower.Width / 2, _imgTower.Height / 2);
                spriteBatch.Draw(_imgTower, new Vector2(tower.Position.X + (_imgTower.Width / 2), tower.Position.Y + (_imgTower.Height / 2)), null, Color.White, tower.Angle, origin, 1.0f, SpriteEffects.None, 1);
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
                spriteBatch.Draw(_imgTower4, _towerSelector + new Vector2(Constant.imgSizeMap, Constant.imgSizeMap), null, Color.White);


                if (_map.Dollars < Tower.TowerPrice(TowerType.simple))
                {
                    spriteBatch.Draw(_imgWrong, _towerSelector + new Vector2(-Constant.imgSizeMap, -Constant.imgSizeMap), null, Color.White);
                }
                if (_map.Dollars < Tower.TowerPrice(TowerType.slow))
                {
                    spriteBatch.Draw(_imgWrong, _towerSelector + new Vector2(Constant.imgSizeMap, -Constant.imgSizeMap), null, Color.White);
                }
                if (_map.Dollars < Tower.TowerPrice(TowerType.area))
                {
                    spriteBatch.Draw(_imgWrong, _towerSelector + new Vector2(-Constant.imgSizeMap, Constant.imgSizeMap), null, Color.White);
                }
                if (_map.Dollars < Tower.TowerPrice(TowerType.bank))
                {
                    spriteBatch.Draw(_imgWrong, _towerSelector + new Vector2(Constant.imgSizeMap, Constant.imgSizeMap), null, Color.White);
                }
            }
            
            if (_towerSelectorUpgrade != new Vector2(-1000, -1000))
            {
                spriteBatch.Draw(_imgSelector, _towerSelectorUpgrade + new Vector2(0, -(Constant.imgSizeMap + 5)), null, Color.White);
                spriteBatch.Draw(_imgUpgrade, _towerSelectorUpgrade + new Vector2(0, -(Constant.imgSizeMap + 5)), null, Color.White);
                spriteBatch.Draw(_imgSelector, _towerSelectorUpgrade + new Vector2(0, (Constant.imgSizeMap + 5)), null, Color.White);
                spriteBatch.Draw(_imgSell, _towerSelectorUpgrade + new Vector2(0, (Constant.imgSizeMap + 5)), null, Color.White);
                spriteBatch.DrawString(_upgradeFont, Tower.TowerPrice(_myTow)*1.5 +"$" ,_towerSelectorUpgrade + new Vector2(0, -(Constant.imgSizeMap + 30)), Color.White);

                if(_map.Dollars < (double)Tower.TowerPrice(_myTow)* 1.5)
                {
                    spriteBatch.Draw(_imgWrong, _towerSelectorUpgrade +new Vector2(0,-(Constant.imgSizeMap +5)) , null, Color.White);
                }
            }


            #endregion

            #region Missiles

            List<Missile> _missiles = _map.Missiles;
            foreach (Missile missile in _missiles)
            {
                Texture2D _imgMissileD  = null;
                if (missile.Tower == TowerType.simple) _imgMissileD = _imgMissile;
                else if (missile.Tower == TowerType.slow) _imgMissileD = _imgMissile;
                else if (missile.Tower == TowerType.area) _imgMissileD = _imgMissile;

                Rectangle sourceRectangle = new Rectangle(0, 0, _imgMissileD.Width, _imgMissileD.Height);
                Vector2 origin = new Vector2(_imgMissileD.Width / 2, _imgMissileD.Height / 2);
                spriteBatch.Draw(_imgMissileD, new Vector2(missile.Position.X + (_imgMissileD.Width / 2), missile.Position.Y + (_imgMissileD.Height / 2)), null, Color.White, missile.Angle, origin, 1.0f, SpriteEffects.None, 1);
            }

            #endregion

            #region Dollars

            Vector2 _positionDollars = new Vector2(10, 10);
            Rectangle _overlayDollars = new Rectangle(0, 0, 150, 33);
            spriteBatch.Draw(_backgroundDollars, new Vector2(5, 10), _overlayDollars, Color.Black * 0.6f);
            spriteBatch.Draw(_imgDollars, _positionDollars, Color.White);
            spriteBatch.DrawString(_spriteDollars, _map.Dollars + "", new Vector2(50, 17), Color.White);
            AnimationsDollars.Draw();

            #endregion

            #region Buttons

            #region Timer Buttons

            Vector2 _overlayManageTimePosition = new Vector2(_groupOfButtonsUITimer.ButtonsUIArray["pauseTimer"].Position.X - 5, _groupOfButtonsUITimer.ButtonsUIArray["pauseTimer"].Position.Y - 5);
            Rectangle _overlayManageTime = new Rectangle(0, 0, 143, 42);
            spriteBatch.Draw(_backgroundDollars, _overlayManageTimePosition, _overlayManageTime, Color.Black * 0.6f);
            _groupOfButtonsUITimer.Draw(spriteBatch);

            #endregion

            #region Abilities Buttons

            _groupOfButtonsUIAbilities.ButtonsUIArray["explosionAbility"].Draw(spriteBatch);

            #endregion

            #endregion

            #region North Korea is Comming

            Rectangle sourceRectanglee = new Rectangle(0, 0, 270, 33);
            spriteBatch.Draw(_backgroundDollars, new Vector2(5, 50), sourceRectanglee, Color.Black * 0.6f);
            spriteBatch.Draw(_flagNorthKorea, new Vector2(10, 50), Color.White);
            spriteBatch.DrawString(_imgNextWave, "Vagues " + Map.WavesCounter + "/" + Map.WavesTotals, new Vector2(50, 57), Color.White);
            _waveSprite.Draw(GraphicsDevice, spriteBatch);

            #endregion

            #region Cursor

            if (_groupOfButtonsUIAbilities.ButtonActivated != null && _groupOfButtonsUIAbilities.ButtonActivated.Name == "explosionAbility")
                spriteBatch.Draw(_imgCursorBomb, new Vector2(newStateMouse.X, newStateMouse.Y), Color.White);
            else
                spriteBatch.Draw(_imgCursorDefault, new Vector2(newStateMouse.X, newStateMouse.Y), Color.White);

            #endregion

            #region HELP DEBOGAGE

            /*
            spriteBatch.DrawString(_spriteDollars, "Mouse X : " + newStateMouse.X, new Vector2(50, 107), Color.DarkRed);
            spriteBatch.DrawString(_spriteDollars, "Mouse Y : " + newStateMouse.Y, new Vector2(50, 127), Color.DarkRed);
            
            spriteBatch.DrawString(_spriteDollars, "Mouse Y : " + newStateMouse.Y, new Vector2(50, 147), Color.DarkRed);
            */

            #endregion

            // ANIM EXPLOSION ABILITY
            foreach (SimpleAnimationDefinition def in AnimSprites)
            {
                foreach (SimpleAnimationSprite animatedSprite in def.AnimatedSprite) animatedSprite.Draw(gameTime, false);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }




        public Map Map => _map;
        public SpriteBatch SpriteBatch => spriteBatch;
    }
}
