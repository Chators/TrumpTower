﻿using LibraryTrumpTower.AirUnits;
using MapEditorTrumpTower.Button;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.NuclexGui;
using MonoGame.Extended.NuclexGui.Controls;
using MonoGame.Extended.NuclexGui.Controls.Desktop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;
using TrumpTower.LibraryTrumpTower.Spawns;

namespace MapEditorTrumpTower
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private readonly InputListenerComponent _inputManager;
        private readonly GuiManager _gui;

        public float VirtualWidth { get; set; }
        public float VirtualHeight { get; set; }
        private Matrix scale;
        MouseState newStateMouse;
        MouseState lastStateMouse;
        KeyboardState lastStateKeyboard;

        public List<Texture2D> _imgMaps;
        public Map _map;

        public SelectorTexture SelectTexture { get; private set; }

        private SpriteFont _debug;

        private Texture2D _imgAccept;
        private Texture2D _imgWall;
        private Texture2D _imgCursor;
        private Texture2D _imgNoSelect;
        private Texture2D _imgCloakTexture;
        private Texture2D _imgNextWaveIsComming;
        private Texture2D _imgClipBoards;
        private Texture2D _imgPlane1;
        private Texture2D _imgEnemy1;
        private Texture2D _imgKamikaze;
        private Texture2D _imgDoctor;
        private Texture2D _imgSaboteur;
        #region Init Dollars
        private Texture2D _imgDollars;
        private SpriteFont _spriteDollars;
        private Texture2D _backgroundDollars;
        #endregion

        #region Text Wave
        SpriteFont _imgNextWave;
        Texture2D _flagNorthKorea;
        #endregion

        private List<ButtonTexture> _buttonsTexture;
        private List<ButtonCreatePath> _buttonsCreatePath;

        #region gameState
        public GameState State { get; set; }
        public ActionCreatePath CurrentActionCreatePath { get; set; }
        #endregion
        int posMenuRight; 

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            // First, we create an input manager.
            _inputManager = new InputListenerComponent(this);

            // Then, we create GUI.
            var guiInputService = new GuiInputService(_inputManager);
            _gui = new GuiManager(Services, guiInputService);

            _buttonsTexture = new List<ButtonTexture>();
            _buttonsCreatePath = new List<ButtonCreatePath>();
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
            State = new GameState(StateType.Init);
            Window.Title = "TT Map Editor";
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            _gui.Screen = new GuiScreen(graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height);
            _gui.Screen.Desktop.Bounds = new UniRectangle(new UniScalar(0f, 0), new UniScalar(0f, 0), new UniScalar(1f, 0), new UniScalar(1f, 0));
            // Perform second-stage initialization
            _gui.Initialize();

            // POUR AJOUTER UN BOUTON
            // Create few controls.
            var button = new GuiButtonControl
            {
                Name = "button",
                Bounds = new UniRectangle(new UniScalar(0.0f, 20), new UniScalar(0.0f, 20), new UniScalar(0f, 120), new UniScalar(0f, 50)),
                Text = "Creer Map"
            };

            // Si on press le bouton
            button.Pressed += Button2_Pressed;

            _gui.Screen.Desktop.Children.Add(button);

            posMenuRight = GraphicsDevice.Viewport.Width * 85 / 100;
            CurrentActionCreatePath = ActionCreatePath.Add;

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

            #region Load Map
            _imgMaps = new List<Texture2D>();
            foreach (string name in Enum.GetNames(typeof(MapTexture)))
            {
                if (name != "None") _imgMaps.Add(Content.Load<Texture2D>("Map/" + name));
            }
            #endregion

            _imgAccept = Content.Load<Texture2D>("accept");
            _imgWall = Content.Load<Texture2D>("Wall");
            _imgNoSelect = Content.Load <Texture2D>("select");
            _imgCloakTexture = Content.Load<Texture2D>("cloakImgMap");
            _debug = Content.Load<SpriteFont>("DefaultFont");
            _imgNextWaveIsComming = Content.Load<Texture2D>("north_korea_is_comming");
            _imgClipBoards = Content.Load<Texture2D>("clipboards");

            _imgPlane1 = Content.Load<Texture2D>("Enemies/plane1");
            _imgEnemy1 = Content.Load<Texture2D>("Enemies/enemy1");
            _imgKamikaze = Content.Load<Texture2D>("Enemies/kamikaze");
            _imgDoctor = Content.Load<Texture2D>("Enemies/doctor");
            _imgSaboteur = Content.Load<Texture2D>("Enemies/saboteur");

            #region Load Dollars
            _imgDollars = Content.Load<Texture2D>("Dollars/dollarsImg");
            _spriteDollars = Content.Load<SpriteFont>("Dollars/dollars");
            _backgroundDollars = Content.Load<Texture2D>("Dollars/backgroundDollars");
            #endregion

            #region Load Text Wave
            _imgNextWave = Content.Load<SpriteFont>("NextWave/next_wave");
            _flagNorthKorea = Content.Load<Texture2D>("NextWave/flagNorthKorea");
            #endregion

            #region Load Cursor
            _imgCursor = Content.Load<Texture2D>("cursor");
            #endregion
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
            // Update both InputManager (which updates states of each device) and GUI
            //_gui.Update(gameTime);
            _inputManager.Update(gameTime);

            #region Update HandleInput
            newStateMouse = Mouse.GetState();
            newStateMouse = new MouseState((int)(newStateMouse.X * (VirtualWidth / GraphicsDevice.Viewport.Width)),
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

            if (State.ActualState == StateType.Default)
            {
            }
            else if (State.ActualState == StateType.CreatePathForSpawn)
            {
                if (CurrentActionCreatePath == ActionCreatePath.Add)
                {

                }
                else if (CurrentActionCreatePath == ActionCreatePath.Reset)
                {

                }
                else if (CurrentActionCreatePath == ActionCreatePath.Undo)
                {

                }
                else if (CurrentActionCreatePath == ActionCreatePath.Validate)
                {

                }
                else if (CurrentActionCreatePath == ActionCreatePath.Back)
                {
                    State.ActualState = StateType.Default;
                    CurrentActionCreatePath = ActionCreatePath.Add;
                }
            }

            base.Update(gameTime);
        }

        protected void HandleInput(MouseState newStateMouse, MouseState lastStateMouse, KeyboardState newStateKeyboard, KeyboardState lastStateKeyboard)
        {
            if (State.ActualState == StateType.Default)
            {
                SelectTexture.HandleInput(newStateMouse, lastStateMouse, newStateKeyboard, lastStateKeyboard);
                for (int i = 0; i < _buttonsTexture.Count; i++)
                {
                    ButtonTexture _buttonTexture = _buttonsTexture[i];
                    _buttonTexture.HandleInput(newStateMouse, lastStateMouse, newStateKeyboard, lastStateKeyboard);
                }

                if (newStateKeyboard.IsKeyDown(Keys.I) && !lastStateKeyboard.IsKeyDown(Keys.I))
                    MapSetting_Pressed();
                else if (newStateKeyboard.IsKeyDown(Keys.P) && !lastStateKeyboard.IsKeyDown(Keys.P))
                    ManagerAirPlane_Pressed();
                else if (newStateKeyboard.IsKeyDown(Keys.Enter) && !lastStateKeyboard.IsKeyDown(Keys.Enter))
                {
                    SaveMap(_map, "data.bin");
                    Exit();
                }

            }
            else if (State.ActualState == StateType.CreatePathForSpawn)
            {
                for (int i = 0; i < _buttonsCreatePath.Count; i++)
                {
                    ButtonCreatePath _buttonTexture = _buttonsCreatePath[i];
                    _buttonTexture.HandleInput(newStateMouse, lastStateMouse, newStateKeyboard, lastStateKeyboard);
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);

            // TODO: Add your drawing code here
            _gui.Draw(gameTime);

            if (_map != null)
            {
                #region Draw Right Menu
                spriteBatch.Begin();

                if (State.ActualState == StateType.Default)
                {
                    spriteBatch.DrawString(_debug, "Menu de Texture", new Vector2(85 + posMenuRight, 10), Color.Blue);
                    spriteBatch.DrawString(_debug, "Texture Utile", new Vector2(90 + posMenuRight, 50), Color.BlueViolet);
                    spriteBatch.DrawString(_debug, "Texture Decors", new Vector2(90 + posMenuRight, 220), Color.BlueViolet);
                    spriteBatch.DrawString(_debug, "Outil de Selection", new Vector2(90 + posMenuRight, 610), Color.BlueViolet);
                    for (int i = 0; i < _buttonsTexture.Count; i++)
                    {
                        ButtonTexture _buttonTexture = _buttonsTexture[i];
                        _buttonTexture.Draw(spriteBatch);
                    }
                }
                else if (State.ActualState == StateType.CreatePathForSpawn)
                {
                    spriteBatch.DrawString(_debug, "Outil de tracage de chemin", new Vector2(85 + posMenuRight, 10), Color.Blue);
                    spriteBatch.DrawString(_debug, "Les unites sortant du spawn emprunteront ce chemin.", new Vector2(20 + posMenuRight, 50), Color.BlueViolet);
                    spriteBatch.DrawString(_debug, "Il doit etre sur une route", new Vector2(20 + posMenuRight, 70), Color.BlueViolet);
                    spriteBatch.DrawString(_debug, "Il doit etre relie a la base", new Vector2(20 + posMenuRight, 90), Color.BlueViolet);
                    spriteBatch.DrawString(_debug, "Les cases vertes represente le chemin et vous ne pouvez pas revenir sur une meme case", new Vector2(20 + posMenuRight, 110), Color.BlueViolet);
                    for (int i = 0; i < _buttonsCreatePath.Count; i++)
                    {
                        ButtonCreatePath _buttonTexture = _buttonsCreatePath[i];
                        _buttonTexture.Draw(spriteBatch);
                    }


                }
                spriteBatch.End();
                #endregion

                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, scale);
                IsMouseVisible = true;

                #region Draw Map
                for (int y = 0; y < _map.HeightArrayMap; y++)
                {
                    for (int x = 0; x < _map.WidthArrayMap; x++)
                    {
                        spriteBatch.Draw(_imgMaps[_map.GetTypeArray(x, y)], new Vector2(x * Constant.imgSizeMap, y * Constant.imgSizeMap), null, Color.White);
                    }
                }
                #endregion

                #region Draw Debug
                spriteBatch.DrawString(_debug, SelectTexture.Texture + "", new Vector2(150, 150), Color.Red);
                spriteBatch.DrawString(_debug, CurrentActionCreatePath + "", new Vector2(150, 200), Color.Red);
                #endregion

                #region Draw SelectorTexture
                SelectTexture.Draw(spriteBatch);
                #endregion

                #region Draw Wall
                if (_map.Wall != null)
                {
                    
                    Wall _wall = _map.Wall;
                    Console.WriteLine(_wall.CurrentHp);
                    spriteBatch.Draw(_imgWall, _wall.Position, Color.White);
                }
                #endregion

                #region Draw Spawn
                for(int i = 0; i < _map.SpawnsEnemies.Count; i++)
                {
                    Spawn spawn = _map.SpawnsEnemies[i];
                    spriteBatch.Draw(_imgNextWaveIsComming, spawn.Position, Color.White);
                }
                #endregion

                #region Draw Dollars
                Vector2 _positionDollars = new Vector2(10, 10);
                Rectangle _overlayDollars = new Rectangle(0, 0, 150, 33);
                spriteBatch.Draw(_backgroundDollars, new Vector2(5, 10), _overlayDollars, Color.Black * 0.6f);
                spriteBatch.Draw(_imgDollars, _positionDollars, Color.White);
                spriteBatch.DrawString(_spriteDollars, _map.Dollars + "", new Vector2(50, 17), Color.White);
                #endregion

                #region Draw Number Of Wave
                Rectangle sourceRectanglee = new Rectangle(0, 0, 270, 33);
                spriteBatch.Draw(_backgroundDollars, new Vector2(5, 50), sourceRectanglee, Color.Black * 0.6f);
                spriteBatch.Draw(_flagNorthKorea, new Vector2(10, 50), Color.White);
                spriteBatch.DrawString(_imgNextWave, "Vagues " + Map.WavesCounter + "/" + Map.WavesTotals, new Vector2(50, 57), Color.White);
                #endregion

                spriteBatch.End();
            }

            _gui.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, scale);
            #region Draw Cursor
            spriteBatch.Draw(_imgCursor, new Vector2(newStateMouse.X, newStateMouse.Y), Color.White);
            #endregion
            spriteBatch.End();
            base.Draw(gameTime);
        }

        #region WINDOW

        #region Window Map Size
        private void Button2_Pressed(object sender, System.EventArgs e)
        {

            var window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(300), new UniScalar(200))),
                Title = "Taille de la map",
                EnableDragging = true
            };

            var labelChoiceX = new GuiLabelControl()
            {
                Text = "Largeur de la map",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var choiceX = new GuiInputControl
            {
                Name = "choiceX",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 55), new UniScalar(100), new UniScalar(25)),
                Text = "20"
            };

            var labelChoiceY = new GuiLabelControl()
            {
                Text = "Longueur de la map",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 80), new UniScalar(100), new UniScalar(25))
            };

            var choiceY = new GuiInputControl
            {
                Name = "choiceY",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 105), new UniScalar(100), new UniScalar(25)),
                Text = "20"
            };
            var button1 = new GuiButtonControl
            {
                Name = "confirm",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Confirmer"
            };
            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Retour"
            };

            button1.Pressed += DialogueConfirm_Pressed;
            button2.Pressed += DialogueCancel_Pressed;

            window.Children.Add(labelChoiceX);
            window.Children.Add(choiceX);
            window.Children.Add(labelChoiceY);
            window.Children.Add(choiceY);
            window.Children.Add(button1);
            window.Children.Add(button2);

            _gui.Screen.Desktop.Children.Add(window);
        }

        private void DialogueCancel_Pressed(object sender, System.EventArgs e)
        {
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
        }

        private void DialogueConfirm_Pressed(object sender, System.EventArgs e)
        {
            int _width = 0;
            int _height = 0;

            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "choiceX") _width = Convert.ToInt32(inputSize.Text);
                    if (control.Name == "choiceY") _height = Convert.ToInt32(inputSize.Text);
                }
            }

            int[,] _mapPoint = new int[_width, _height];
            for (int y = 0; y < _mapPoint.GetLength(0); y++)
            {
                for (int x = 0; x < _mapPoint.GetLength(1); x++)
                {
                    _mapPoint[y, x] = (int)MapTexture.grass;
                }
            }

            _map = new Map(_mapPoint);

            SelectTexture = new SelectorTexture(this, _map, _imgCloakTexture);

            #region Button Left Menu Default
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirt], _debug, MapTexture.dirt, new Vector2(posMenuRight + 10, 100), "1",
                new List<Keys>(new Keys[] { Keys.D1 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.grass], _debug, MapTexture.grass, new Vector2(posMenuRight + 80, 100), "2",
                new List<Keys>(new Keys[] { Keys.D2 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.emptyTower], _debug, MapTexture.emptyTower, new Vector2(posMenuRight + 150, 100), "3",
                new List<Keys>(new Keys[] { Keys.D3 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgWall, _debug, MapTexture.myBase, new Vector2(posMenuRight + 220, 100), "4",
                new List<Keys>(new Keys[] { Keys.D4 })));

            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtDownGrassUp], _debug, MapTexture.dirtDownGrassUp, new Vector2(posMenuRight + 10, 270), "CTRL+1",
                new List<Keys>(new Keys[] { Keys.LeftControl, Keys.D1 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtRightGrassLeft], _debug, MapTexture.dirtRightGrassLeft, new Vector2(posMenuRight + 80, 270), "CTRL+2",
                new List<Keys>(new Keys[] { Keys.LeftControl, Keys.D2 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtUpGrassDown], _debug, MapTexture.dirtUpGrassDown, new Vector2(posMenuRight + 150, 270), "CTRL+3",
                new List<Keys>(new Keys[] { Keys.LeftControl, Keys.D3 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtLeftGrassRight], _debug, MapTexture.dirtLeftGrassRight, new Vector2(posMenuRight + 220, 270), "CTRL+4",
                new List<Keys>(new Keys[] { Keys.LeftControl, Keys.D4 })));

            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.bigDirtCornerLeftDown], _debug, MapTexture.bigDirtCornerLeftDown, new Vector2(posMenuRight + 10, 380), "ALT+1",
                new List<Keys>(new Keys[] { Keys.LeftAlt, Keys.D1 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.bigDirtCornerRightDown], _debug, MapTexture.bigDirtCornerRightDown, new Vector2(posMenuRight + 80, 380), "ALT+2",
                new List<Keys>(new Keys[] { Keys.LeftAlt, Keys.D2 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.bigDirtCornerRightUp], _debug, MapTexture.bigDirtCornerRightUp, new Vector2(posMenuRight + 150, 380), "ALT+3",
                new List<Keys>(new Keys[] { Keys.LeftAlt, Keys.D3 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.bigDirtCornerLeftUp], _debug, MapTexture.bigDirtCornerLeftUp, new Vector2(posMenuRight + 220, 380), "ALT+4",
                new List<Keys>(new Keys[] { Keys.LeftAlt, Keys.D4 })));

            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtCornerLeftDown], _debug, MapTexture.dirtCornerLeftDown, new Vector2(posMenuRight + 10, 490), "SHIFT+1",
                new List<Keys>(new Keys[] { Keys.LeftShift, Keys.D1 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtCornerRightDown], _debug, MapTexture.dirtCornerRightDown, new Vector2(posMenuRight + 80, 490), "SHIFT+2",
                new List<Keys>(new Keys[] { Keys.LeftShift, Keys.D2 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtCornerRightUp], _debug, MapTexture.dirtCornerRightUp, new Vector2(posMenuRight + 150, 490), "SHIFT+3",
                new List<Keys>(new Keys[] { Keys.LeftShift, Keys.D3 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtCornerLeftUp], _debug, MapTexture.dirtCornerLeftUp, new Vector2(posMenuRight + 220, 490), "SHIFT+4",
                new List<Keys>(new Keys[] { Keys.LeftShift, Keys.D4 })));

            _buttonsTexture.Add(new ButtonTexture(this, _imgNoSelect, _debug, MapTexture.None, new Vector2(posMenuRight + 115, 660), "A",
                new List<Keys>(new Keys[] { Keys.A })));

            _buttonsTexture.Add(new ButtonTexture(this, _imgClipBoards, _debug, MapTexture.None, new Vector2(posMenuRight + 115, 760), "I",
                new List<Keys>(new Keys[] { Keys.I })));

            _buttonsTexture.Add(new ButtonTexture(this, _imgPlane1, _debug, MapTexture.None, new Vector2(posMenuRight + 115, 860), "P",
               new List<Keys>(new Keys[] { Keys.P })));

            _buttonsTexture.Add(new ButtonTexture(this, _imgAccept, _debug, MapTexture.None, new Vector2(posMenuRight + 115, 960), "Entrer",
               new List<Keys>(new Keys[] { Keys.Enter })));
            #endregion

            #region Button Left Menu CreatePath
            _buttonsCreatePath.Add(new ButtonCreatePath(this, "Ajouter un chemin", _imgNoSelect, _debug, ActionCreatePath.Add, new Vector2(posMenuRight + 10, 100), "A",
                new List<Keys>(new Keys[] { Keys.A })));
            _buttonsCreatePath.Add(new ButtonCreatePath(this, "Retour arriere", _imgNoSelect, _debug, ActionCreatePath.Undo, new Vector2(posMenuRight + 60, 100), "CTRL+Z",
                new List<Keys>(new Keys[] { Keys.LeftControl, Keys.Z })));
            _buttonsCreatePath.Add(new ButtonCreatePath(this, "Reinitialiser", _imgNoSelect, _debug, ActionCreatePath.Reset, new Vector2(posMenuRight + 110, 100), "R",
                new List<Keys>(new Keys[] { Keys.R })));
            _buttonsCreatePath.Add(new ButtonCreatePath(this, "Retour", _imgNoSelect, _debug, ActionCreatePath.Back, new Vector2(posMenuRight + 160, 100), "<--",
                new List<Keys>(new Keys[] { Keys.Back })));
            _buttonsCreatePath.Add(new ButtonCreatePath(this, "Valider", _imgNoSelect, _debug, ActionCreatePath.Validate, new Vector2(posMenuRight + 210, 100), "Entree",
                new List<Keys>(new Keys[] { Keys.Enter })));
            #endregion

            VirtualWidth = _map.WidthArrayMap * Constant.imgSizeMap;
            VirtualHeight = _map.HeightArrayMap * Constant.imgSizeMap;
            scale = Matrix.CreateScale(
                            (GraphicsDevice.Viewport.Width * 85 / 100) / VirtualWidth,
                            GraphicsDevice.Viewport.Height / VirtualHeight,
                            1f);

            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            GuiButtonControl deleteButton = null;
            foreach (GuiButtonControl button in _gui.Screen.Desktop.Children)
            {
                if (button.Name == "button") deleteButton = button;
            }

            State.ActualState = StateType.Default;
            _gui.Screen.Desktop.Children.Remove(deleteButton);
        }
        #endregion

        #region Window Wall
        public void Wall_Pressed()
        {

            var window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(300), new UniScalar(200))),
                Title = "Definir les points de vie de la base",
                EnableDragging = true
            };

            var labelHealthWall = new GuiLabelControl()
            {
                Text = "Points de vie",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var healthWall = new GuiInputControl
            {
                Name = "healthWall",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 55), new UniScalar(100), new UniScalar(25)),
                Text = "500"
            };

            var button1 = new GuiButtonControl
            {
                Name = "confirm",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Confirmer"
            };
            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Retour"
            };

            button1.Pressed += WallConfirm_Pressed;
            button2.Pressed += WallCancel_Pressed;

            window.Children.Add(labelHealthWall);
            window.Children.Add(healthWall);
            window.Children.Add(button1);
            window.Children.Add(button2);

            _gui.Screen.Desktop.Children.Add(window);
        }

        private void WallCancel_Pressed(object sender, System.EventArgs e)
        {
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
        }

        private void WallConfirm_Pressed(object sender, System.EventArgs e)
        {
            int _healthWall = 0;
            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "healthWall") _healthWall = Convert.ToInt32(inputSize.Text);
                }
            }

            _map.Wall.ChangeHp(_healthWall);

            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
        }
        #endregion

        #region Window Create Spawn
        public void Road_Pressed(Vector2 positionRoad)
        {
            var posX = new GuiInputControl
            {
                Name = "posX",
                Text = positionRoad.X+""
            };

            var posY = new GuiInputControl
            {
                Name = "posY",
                Text = positionRoad.Y+""
            };

            var window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(300), new UniScalar(200))),
                Title = "Creer un spawn ?",
                EnableDragging = true
            };

            var labelCreateSpawn = new GuiLabelControl()
            {
                Text = "Voulez-vous creer un spawn ?",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var button1 = new GuiButtonControl
            {
                Name = "confirm",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Confirmer"
            };
            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Retour"
            };

            button1.Pressed += RoadConfirm_Pressed;
            button2.Pressed += RoadCancel_Pressed;

            window.Children.Add(posX);
            window.Children.Add(posY);
            window.Children.Add(labelCreateSpawn);
            window.Children.Add(button1);
            window.Children.Add(button2);

            _gui.Screen.Desktop.Children.Add(window);
        }

        private void RoadCancel_Pressed(object sender, System.EventArgs e)
        {
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
        }

        private void RoadConfirm_Pressed(object sender, System.EventArgs e)
        {
            int _posX = 0;
            int _posY = 0;

            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "posX") _posX = Convert.ToInt32(inputSize.Text);
                    if (control.Name == "posY") _posY = Convert.ToInt32(inputSize.Text);
                }
            }
            _map.CreateSpawn(new Spawn(_map, new Vector2(_posX, _posY), new List<Wave>()));

            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
        }
        #endregion

        #region Window Modify Spawn
        public void Spawn_Pressed(Vector2 positionRoad)
        {
            Vector2 position = positionRoad;
            Spawn _spawn = null;
            // On trouve le spawn
            foreach (Spawn spawn in _map.SpawnsEnemies)
            {
                // On cible le spawn
                if (spawn.Position == new Vector2(positionRoad.X, positionRoad.Y))
                {
                    _spawn = spawn;
                    break;
                }
            }
            var posX = new GuiInputControl
            {
                Name = "posX",
                Text = positionRoad.X + ""
            };

            var posY = new GuiInputControl
            {
                Name = "posY",
                Text = positionRoad.Y + ""
            };

            var window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(400), new UniScalar(300))),
                Title = "Spawn",
                EnableDragging = true
            };

            var wavesList = new GuiListControl()
            {
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(1.0f, -20), new UniScalar(0f, 80)),
                
            };
            
            var addWave = new GuiButtonControl
            {
                Name = "AddWave",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 120), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Ajouter vague"
            };

            var modifyWave = new GuiButtonControl
            {
                Name = "ModifyWave",
                Bounds = new UniRectangle(new UniScalar(1.0f, -250), new UniScalar(0.0f, 120), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Modifier vague"
            };

            var deleteWave = new GuiButtonControl
            {
                Name = "DeleteWave",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(0.0f, 120), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Suppr vague"
            };

            var deleteSpawn = new GuiButtonControl
            {
                Name = "deleteSpawn",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Suppr spawn"
            };

            var createPath = new GuiButtonControl
            {
                Name = "createPath",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -80), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Not implemented"
            };

            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Retour"
            };

            deleteSpawn.Pressed += DeleteSpawn_Pressed;
            createPath.Pressed += CreatePath_Pressed;
            modifyWave.Pressed += ModifyWave_Pressed;
            button2.Pressed += SpawnCancel_Pressed;
            addWave.Pressed += CreateWave_Pressed;
            deleteWave.Pressed += DeleteWave_Pressed;

            window.Children.Add(wavesList);
            for (int i = 0; i < _spawn.Waves.Count; i++)
            {
                Wave wave = _spawn.Waves[i];
                wavesList.Items.Add("Vague " + (i+1) + " partira a " + wave.TimerBeforeStarting/60 + " secondes" );
            }
            wavesList.SelectionMode = ListSelectionMode.Single;
            window.Children.Add(addWave);
            if (_spawn.Waves.Count > 0)
            {
                window.Children.Add(modifyWave);
                window.Children.Add(deleteWave);
            }
            window.Children.Add(posX);
            window.Children.Add(posY);
            window.Children.Add(deleteSpawn);
            window.Children.Add(createPath);
            window.Children.Add(button2);
            _gui.Screen.Desktop.Children.Add(window);

        }

        private void WavesList_SelectionChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SpawnCancel_Pressed(object sender, System.EventArgs e)
        {
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
        }

        private void DeleteSpawn_Pressed(object sender, System.EventArgs e)
        {
            int _posX = 0;
            int _posY = 0;

            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "posX") _posX = Convert.ToInt32(inputSize.Text);
                    if (control.Name == "posY") _posY = Convert.ToInt32(inputSize.Text);
                }
            }

            foreach (Spawn _spawn in _map.SpawnsEnemies)
            {
                if (_spawn.Position == new Vector2(_posX, _posY))
                {
                    _map.DeleteSpawn(_spawn);
                    Map.WavesTotals -= _spawn.Waves.Count;
                    break;
                }     
            }

            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
        }

        private void CreatePath_Pressed(object sender, System.EventArgs e)
        {
            State.ActualState = StateType.CreatePathForSpawn;
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
        }

        private void CreateWave_Pressed(object sender, System.EventArgs e)
        {
            int _posX = 0;
            int _posY = 0;

            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "posX") _posX = Convert.ToInt32(inputSize.Text);
                    if (control.Name == "posY") _posY = Convert.ToInt32(inputSize.Text);
                }
            }

            foreach (Spawn _spawn in _map.SpawnsEnemies)
            {
                // On cible le spawn
                if (_spawn.Position == new Vector2(_posX, _posY))
                {
                    _spawn.CreateWave(new Wave(_spawn, new List<Enemy>(), 5*60));
                    break;
                }
            }

            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            Spawn_Pressed(new Vector2(_posX, _posY));
        }

        private void DeleteWave_Pressed(object sender, System.EventArgs e)
        {
            int _posX = 0;
            int _posY = 0;
            int _nbWave = 0;
            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiListControl))
                {
                    GuiListControl input = (GuiListControl)control;
                    _nbWave = input.SelectedItems[0];
                }
                else if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "posX") _posX = Convert.ToInt32(inputSize.Text);
                    if (control.Name == "posY") _posY = Convert.ToInt32(inputSize.Text);
                }
            }

            foreach (Spawn _spawn in _map.SpawnsEnemies)
            {
                // On cible le spawn
                if (_spawn.Position == new Vector2(_posX, _posY))
                    _spawn.DeleteWave(_nbWave);
            }

            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            Spawn_Pressed(new Vector2(_posX, _posY));
        }

        private void ModifyWave_Pressed(object sender, System.EventArgs e)
        {
            int _posX = 0;
            int _posY = 0;
            int _nbWave = 0;
            Spawn _spawn = null;
            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiListControl))
                {
                    GuiListControl input = (GuiListControl)control;
                    _nbWave = input.SelectedItems[0];
                }
                else if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "posX") _posX = Convert.ToInt32(inputSize.Text);
                    if (control.Name == "posY") _posY = Convert.ToInt32(inputSize.Text);
                }
            }

            int _nbSpawn = 0;
            for (int i = 0; i < _map.SpawnsEnemies.Count; i++)
            {
                Spawn spawn = _map.SpawnsEnemies[i];
                // On cible le spawn
                if (spawn.Position == new Vector2(_posX, _posY))
                {
                    _spawn = spawn;
                    _nbSpawn = i;
                    break;
                }
            }

            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            WindowModifyWave_Pressed(_spawn.Waves[_nbWave], _nbSpawn, _nbWave);
        }
        #endregion

        #region Window Modify Wave
        public void WindowModifyWave_Pressed(Wave wave, int nbSpawnX, int nbWaveX)
        {
            var nbSpawn = new GuiInputControl
            {
                Name = "nbSpawn",
                Text = nbSpawnX + ""
            };
            var nbWave = new GuiInputControl
            {
                Name = "nbWave",
                Text = nbWaveX+""
            };

            var window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(400), new UniScalar(350))),
                Title = "Vague",
                EnableDragging = true
            };

            var labelTimerWave = new GuiLabelControl()
            {
                Text = "Temps avant depart de la vague",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var timerWave = new GuiInputControl
            {
                Name = "timerWave",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 55), new UniScalar(100), new UniScalar(25)),
                Text = wave.TimerBeforeStarting/60+""
            };

            var labelEnemyDefault = new GuiLabelControl()
            {
                Text = "Nombres d'ennemis de base",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 80), new UniScalar(100), new UniScalar(25))
            };

            var enemyDefault = new GuiInputControl
            {
                Name = "enemyDefault",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 105), new UniScalar(100), new UniScalar(25)),
                Text = 0+""
            };

            var labelKamikaze = new GuiLabelControl()
            {
                Text = "Nombres de Kamikaze",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 130), new UniScalar(100), new UniScalar(25))
            };

            var kamikaze = new GuiInputControl
            {
                Name = "kamikaze",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 155), new UniScalar(100), new UniScalar(25)),
                Text = 0+""
            };

            var labelSaboteur = new GuiLabelControl()
            {
                Text = "Nombres de Saboteur",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 180), new UniScalar(100), new UniScalar(25))
            };

            var saboteur = new GuiInputControl
            {
                Name = "saboteur",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 205), new UniScalar(100), new UniScalar(25)),
                Text = 0+""
            };

            var labelDoctor = new GuiLabelControl()
            {
                Text = "Nombres de Docteur",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 230), new UniScalar(100), new UniScalar(25))
            };

            var doctor = new GuiInputControl
            {
                Name = "doctor",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 255), new UniScalar(100), new UniScalar(25)),
                Text = 0+""
            };

            var button1 = new GuiButtonControl
            {
                Name = "confirm",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Confirmer"
            };

            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Retour"
            };

            button1.Pressed += WaveConfirm_Pressed;
            button2.Pressed += WaveCancel_Pressed;

            window.Children.Add(labelTimerWave);
            window.Children.Add(timerWave);
            window.Children.Add(labelEnemyDefault);
            window.Children.Add(enemyDefault);
            window.Children.Add(labelKamikaze);
            window.Children.Add(kamikaze);
            window.Children.Add(labelSaboteur);
            window.Children.Add(saboteur);
            window.Children.Add(labelDoctor);
            window.Children.Add(doctor);
            window.Children.Add(button1);
            window.Children.Add(button2);
            _gui.Screen.Desktop.Children.Add(window);
        }

        private void WaveConfirm_Pressed(object sender, System.EventArgs e)
        {
            throw new ArgumentException();
            /*int _nbPlane = 0;
            int _timer = 0;
            PlaneType _type = PlaneType.PlaneSlow;

            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiListControl))
                {
                    GuiListControl input = (GuiListControl)control;
                    _type = (PlaneType)input.SelectedItems[0];
                }

                if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "numberPlane") _nbPlane = Convert.ToInt32(inputSize.Text);
                    if (control.Name == "timerAir") _timer = Convert.ToInt32(inputSize.Text);
                }
            }
            _map.AirUnits.Add(new AirUnitsCollection(_map, _timer * 60, _nbPlane, _type));
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            ManagerAirPlane_Pressed();*/
        }

        private void WaveCancel_Pressed(object sender, System.EventArgs e)
        {
            int _nbSpawn = 0;
            int _nbWave = 0;

            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "nbSpawnX") _nbSpawn = Convert.ToInt32(inputSize.Text);
                    if (control.Name == "nbWaveX") _nbWave = Convert.ToInt32(inputSize.Text);
                }
            }
            Spawn _spawn = _map.SpawnsEnemies[_nbSpawn];
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            Spawn_Pressed(_spawn.Position);
        }
        #endregion

        #region Window Map Setting
        public void MapSetting_Pressed()
        {

            var window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(300), new UniScalar(200))),
                Title = "Option Map",
                EnableDragging = true
            };

            var labelNameMap = new GuiLabelControl()
            {
                Text = "Nom de la map",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var nameMap = new GuiInputControl
            {
                Name = "nameMap",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 55), new UniScalar(100), new UniScalar(25)),
                Text = _map.Name
            };

            var labelDollarsMap = new GuiLabelControl()
            {
                Text = "Dollars initial de la map",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 85), new UniScalar(100), new UniScalar(25))
            };

            var dollarsMap = new GuiInputControl
            {
                Name = "dollarsMap",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 105), new UniScalar(100), new UniScalar(25)),
                Text = "500"
            };

            var button1 = new GuiButtonControl
            {
                Name = "confirm",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Confirmer"
            };
            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Retour"
            };

            button1.Pressed += MapSettingConfirm_Pressed;
            button2.Pressed += MapSettingCancel_Pressed;

            window.Children.Add(labelNameMap);
            window.Children.Add(nameMap);
            window.Children.Add(labelDollarsMap);
            window.Children.Add(dollarsMap);
            window.Children.Add(button1);
            window.Children.Add(button2);

            _gui.Screen.Desktop.Children.Add(window);
        }

        private void MapSettingCancel_Pressed(object sender, System.EventArgs e)
        {
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
        }

        private void MapSettingConfirm_Pressed(object sender, System.EventArgs e)
        {
            string _nameMap = "";
            int _dollarsMap = 0;
            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "nameMap") _nameMap = inputSize.Text;
                    if (control.Name == "dollarsMap") _dollarsMap = Convert.ToInt32(inputSize.Text);
                }
            }

            _map.SettingTheMap(_nameMap, _dollarsMap);

            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
        }
        #endregion

        #region Window Manager Air Plane
        public void ManagerAirPlane_Pressed()
        {
            var window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(400), new UniScalar(300))),
                Title = "Air plane",
                EnableDragging = true
            };

            var wavesListAir = new GuiListControl()
            {
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(1.0f, -20), new UniScalar(0f, 80)),
            };

            var addWaveAir = new GuiButtonControl
            {
                Name = "AddWaveAir",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 120), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Ajouter une vague"
            };

            var modifyWaveAir = new GuiButtonControl
            {
                Name = "ModifyWaveAir",
                Bounds = new UniRectangle(new UniScalar(1.0f, -250), new UniScalar(0.0f, 120), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Modifier vague"
            };

            var deleteWaveAir = new GuiButtonControl
            {
                Name = "DeleteWave",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(0.0f, 120), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Suppr vague"
            };

            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Retour"
            };

            addWaveAir.Pressed += ManagerAirUnitAdd_Pressed;
            modifyWaveAir.Pressed += ManagerAirUnitModify_Pressed;
            deleteWaveAir.Pressed += ManagerAirUnitDelete_Pressed;
            button2.Pressed += ManagerAirUnitCancel_Pressed;

            window.Children.Add(wavesListAir);
            
            for (int i = 0; i < _map.AirUnits.Count; i++)
            {
                AirUnitsCollection collectionUnit  = _map.AirUnits[i];
                wavesListAir.Items.Add("Vague aerienne " + (i + 1) + " partira a " + collectionUnit.TimerBeforeStarting/60 + " secondes");
            }
            wavesListAir.SelectionMode = ListSelectionMode.Single;
            if (_map.AirUnits.Count > 0)
            {
                window.Children.Add(modifyWaveAir);
                window.Children.Add(deleteWaveAir);
            }

            window.Children.Add(addWaveAir);
            window.Children.Add(button2);
            _gui.Screen.Desktop.Children.Add(window);
        }

        private void ManagerAirUnitCancel_Pressed(object sender, System.EventArgs e)
        {
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
        }

        private void ManagerAirUnitAdd_Pressed(object sender, System.EventArgs e)
        {
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            AddAirPlane_Pressed();
        }

        private void ManagerAirUnitModify_Pressed(object sender, System.EventArgs e)
        {
            AirUnitsCollection _unitsCollection = null;
            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiListControl))
                {
                    GuiListControl input = (GuiListControl)control;
                    _unitsCollection = _map.AirUnits[input.SelectedItems[0]];
                }
            }
            if (_unitsCollection != null)
            {
                _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
                AddAirPlane_Pressed(_unitsCollection);
            }
        }

        private void ManagerAirUnitDelete_Pressed(object sender, System.EventArgs e)
        {
            AirUnitsCollection _unitsCollection = null;
            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiListControl))
                {
                    GuiListControl input = (GuiListControl)control;
                    _unitsCollection = _map.AirUnits[input.SelectedItems[0]];
                }
            }
            _map.DeleteAirWave(_unitsCollection);
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            ManagerAirPlane_Pressed();
        }
        #endregion

        #region Window Add Air Plane
        public void AddAirPlane_Pressed(AirUnitsCollection unitsCollection = null)
        {
            var window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(400), new UniScalar(300))),
                Title = "Ajouter Air plane",
                EnableDragging = true
            };

            var labelTimerAir = new GuiLabelControl()
            {
                Text = "Temps d'arrivage en secondes",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var timerAir = new GuiInputControl
            {
                Name = "timerAir",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 55), new UniScalar(100), new UniScalar(25)),
            };
            if (unitsCollection != null) timerAir.Text = unitsCollection.TimerBeforeStarting/60+"";
            else timerAir.Text = "25";

            var labelNumberPlane = new GuiLabelControl()
            {
                Text = "Nombres d'avions",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 80), new UniScalar(100), new UniScalar(25))
            };

            var numberPlane = new GuiInputControl
            {
                Name = "numberPlane",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 105), new UniScalar(100), new UniScalar(25)),
            };
            if (unitsCollection != null) numberPlane.Text = unitsCollection.Array.Count + "";
            else timerAir.Text = "10";

            var labelTypeAir = new GuiLabelControl()
            {
                Text = "Type d'avions",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 130), new UniScalar(100), new UniScalar(25))
            };

            var typeAir = new GuiListControl()
            {
                Name = "typeAir",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 155), new UniScalar(1.0f, -20), new UniScalar(0f, 80)),
            };

            #region TypePlane
            Array planeType = Enum.GetValues(typeof(PlaneType));
            for (int i = 0; i < planeType.Length; i++)
                typeAir.Items.Add("Avion " + planeType.GetValue(i));
            typeAir.SelectionMode = ListSelectionMode.Single;
            #endregion

            var button1 = new GuiButtonControl
            {
                Name = "confirm",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Confirmer"
            };
            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Retour"
            };

            button1.Pressed += AddAirUnitConfirm_Pressed;
            button2.Pressed += AddAirUnitCancel_Pressed;

            window.Children.Add(labelNumberPlane);
            window.Children.Add(numberPlane);
            window.Children.Add(labelTimerAir);
            window.Children.Add(timerAir);
            window.Children.Add(labelTypeAir);
            window.Children.Add(typeAir);

            window.Children.Add(button1);
            window.Children.Add(button2);

            _gui.Screen.Desktop.Children.Add(window);

        }

        private void AddAirUnitCancel_Pressed(object sender, System.EventArgs e)
        {
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            ManagerAirPlane_Pressed();
        }

        private void AddAirUnitConfirm_Pressed(object sender, System.EventArgs e)
        {
            int _nbPlane = 0;
            int _timer = 0;
            PlaneType _type = PlaneType.PlaneSlow;

            
            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiListControl))
                {
                    GuiListControl input = (GuiListControl)control;
                    _type = (PlaneType)input.SelectedItems[0];
                }

                if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "numberPlane") _nbPlane = Convert.ToInt32(inputSize.Text);
                    if (control.Name == "timerAir") _timer = Convert.ToInt32(inputSize.Text);
                }
            }
            _map.AirUnits.Add(new AirUnitsCollection(_map, _timer*60, _nbPlane, _type));
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            ManagerAirPlane_Pressed();
        }
        #endregion

        #endregion

        public List<Texture2D> ImgMaps => _imgMaps;
        public Texture2D ImgWall => _imgWall;

        private static void SaveMap(object toSave, string path)
        {
            //On utilise la classe BinaryFormatter dans le namespace System.Runtime.Serialization.Formatters.Binary.
            BinaryFormatter formatter = new BinaryFormatter();
            //La classe BinaryFormatter ne fonctionne qu'avec un flux, et non pas un TextWriter.
            //Nous allons donc utiliser un FileStream. Remarquez que n'importe quel flux est
            //compatible.
            FileStream flux = null;
            try
            {
                //On ouvre le flux en mode création / écrasement de fichier et on
                //donne au flux le droit en écriture seulement.
                flux = new FileStream(path, FileMode.Create, FileAccess.Write);
                //Et hop ! On sérialise !
                formatter.Serialize(flux, toSave);
                //On s'assure que le tout soit écrit dans le fichier.
                flux.Flush();
            }
            catch { }
            finally
            {
                //Et on ferme le flux.
                if (flux != null)
                    flux.Close();
            }
        }

        private static T LoadMap<T>(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream flux = null;
            try
            {
                //On ouvre le fichier en mode lecture seule. De plus, puisqu'on a sélectionné le mode Open,
                //si le fichier n'existe pas, une exception sera levée.
                flux = new FileStream(path, FileMode.Open, FileAccess.Read);

                return (T)formatter.Deserialize(flux);
            }
            catch
            {
                //On retourne la valeur par défaut du type T.
                return default(T);
            }
            finally
            {
                if (flux != null)
                    flux.Close();
            }

        }
    }
}

