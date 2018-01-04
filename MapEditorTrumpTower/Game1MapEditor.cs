using LibraryTrumpTower;
using LibraryTrumpTower.AirUnits;
using LibraryTrumpTower.Constants;
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
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;
using TrumpTower.LibraryTrumpTower.Spawns;

namespace MapEditorTrumpTower
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1MapEditor : Game
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
        int _lastRefresh;

        public Game1MapEditor()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = false;
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

            Button2_Pressed();
            _lastRefresh = 0;
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

            try { _inputManager.Update(gameTime); }
            catch { }
            //_inputManager.Update(gameTime);

            if (State.ActualState == StateType.Default)
            {
                // On actualise toutes les 5 secondes
                if (_lastRefresh + 5 == gameTime.TotalGameTime.Seconds)
                {
                    _lastRefresh = gameTime.TotalGameTime.Seconds;

                    if (_map.Wall != null)
                    {
                        for (int i = 0; i < _map.SpawnsEnemies.Count; i++)
                            _map.SpawnsEnemies[i].ResetShortestWay();
                    }
                }
            }

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


            base.Update(gameTime);
            
        }

        protected void HandleInput(MouseState newStateMouse, MouseState lastStateMouse, KeyboardState newStateKeyboard, KeyboardState lastStateKeyboard)
        {
            // Si on est en state default et si on a pas de fenetre ouverte
            if (State.ActualState == StateType.Default && _gui.Screen.Desktop.Children.Count == 0)
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
                    // On reactualise tous les chemins des spawns
                    for (int i = 0; i < _map.SpawnsEnemies.Count; i++)
                        _map.SpawnsEnemies[i].ResetShortestWay();

                    bool hasPath = true;
                    foreach (Spawn spawn in _map.SpawnsEnemies)
                    {
                        if (spawn.ShortestWay == null) hasPath = false;
                    }
                    bool hasName = _map.Name != null;
                    bool hasBase = _map.Wall != null;
                    if (!hasName || !hasBase || !hasPath)
                        InfoSerialization_Pressed(hasName, hasBase, hasPath);
                    else
                    {
                        BinarySerializer.Serialize(_map, "map1.xml");
                        Exit();
                    }
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
            //_gui.Draw(gameTime);
            
            if (_map != null)
            {
                #region Draw Right Menu
                spriteBatch.Begin();
                spriteBatch.Draw(_imgCursor, new Vector2(newStateMouse.X, newStateMouse.Y), Color.Red);
                if (State.ActualState == StateType.Default)
                {
                    spriteBatch.DrawString(_debug, "Menu de Texture", new Vector2(85 + posMenuRight, 10), Color.DarkRed);
                    spriteBatch.DrawString(_debug, "Texture Utile", new Vector2(90 + posMenuRight, 50), Color.Black);
                    spriteBatch.DrawString(_debug, "Texture Decors", new Vector2(90 + posMenuRight, 220), Color.Black);
                    spriteBatch.DrawString(_debug, "Outil de Selection", new Vector2(90 + posMenuRight, 610), Color.Black);
                    spriteBatch.DrawString(_debug, "Options", new Vector2(90 + posMenuRight, 780), Color.Black);
                    spriteBatch.DrawString(_debug, "Validation", new Vector2(90 + posMenuRight, 950), Color.Black);
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
                //spriteBatch.DrawString(_debug, SelectTexture.Texture + "", new Vector2(150, 150), Color.Red);
                //spriteBatch.DrawString(_debug, CurrentActionCreatePath + "", new Vector2(150, 200), Color.Red);
                #endregion

                #region Draw SelectorTexture
                SelectTexture.Draw(spriteBatch);
                #endregion

                #region Draw Wall
                if (_map.Wall != null)
                {
                    
                    Wall _wall = _map.Wall;
                    spriteBatch.Draw(_imgWall, _wall.Position, Color.White);
                }
                #endregion

                #region Draw Spawn
                for(int i = 0; i < _map.SpawnsEnemies.Count; i++)
                {
                    Spawn spawn = _map.SpawnsEnemies[i];
                    spriteBatch.Draw(_imgNextWaveIsComming, spawn.Position, Color.White);
                    if (spawn.ShortestWay == null)
                        spriteBatch.Draw(_imgCloakTexture, spawn.Position, Color.DarkRed * 0.6f);
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

            if (_gui.Screen.Desktop.Children.Count == 0)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, scale);
                spriteBatch.Draw(_imgCursor, new Vector2(newStateMouse.X, newStateMouse.Y), Color.White);
                spriteBatch.End();
            }
            else
            {
                newStateMouse = Mouse.GetState();
                spriteBatch.Begin();
                spriteBatch.Draw(_imgCursor, new Vector2(newStateMouse.X, newStateMouse.Y), Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        #region WINDOW

        #region Window Map Size
        private void Button2_Pressed()
        {
            var window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(400), new UniScalar(180))),
                Title = "Taille de la map",
                EnableDragging = true
            };

            var labelChoiceX = new GuiLabelControl()
            {
                Text = "Veuillez indiquer la taille de la map ( Min " + Constant.MinWidthMap + ", Max " + Constant.MaxWidthMap+ " )",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var choiceX = new GuiInputControl
            {
                Name = "choiceX",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 55), new UniScalar(100), new UniScalar(25)),
                Text = Constant.MinWidthMap+""
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
            window.Children.Add(button1);
            window.Children.Add(button2);

            _gui.Screen.Desktop.Children.Add(window);
        }

        private void DialogueCancel_Pressed(object sender, System.EventArgs e)
        {
            Exit();
        }

        private void DialogueConfirm_Pressed(object sender, System.EventArgs e)
        {
            int _size = 0;

            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                bool _try;
                if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "choiceX") _try = Int32.TryParse(inputSize.Text, out _size);
                }
            }

            double _sizeY;
            if (_size >= Constant.MinWidthMap && _size <= Constant.MaxWidthMap)
            {
                
                _sizeY = (int)Math.Truncate(_size/(16.0f/9));
                int[,] _mapPoint2D = new int[ (int)_sizeY, _size];
                for (int y = 0; y < _mapPoint2D.GetLength(0); y++)
                {
                    for (int x = 0; x < _mapPoint2D.GetLength(1); x++)
                    {
                        _mapPoint2D[y, x] = (int)MapTexture.grass;
                    }
                }

                int[][] _mapPointJagged = new int[(int)_sizeY][];

                for (int y = 0; y < _mapPoint2D.GetLength(0); y++)
                {
                    _mapPointJagged[y] = new int[_mapPoint2D.GetLength(1)];
                    for (int x = 0; x < _mapPoint2D.GetLength(1); x++)
                    {
                        _mapPointJagged[y][x] = _mapPoint2D[y, x];
                    }
                }

                _map = new Map(_mapPointJagged);

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

                _buttonsTexture.Add(new ButtonTexture(this, _imgNoSelect, _debug, MapTexture.None, new Vector2(posMenuRight + 115, 670), "A",
                    new List<Keys>(new Keys[] { Keys.A })));

                _buttonsTexture.Add(new ButtonTexture(this, _imgClipBoards, _debug, MapTexture.None, new Vector2(posMenuRight + 75, 830), "I",
                    new List<Keys>(new Keys[] { Keys.I })));

                _buttonsTexture.Add(new ButtonTexture(this, _imgPlane1, _debug, MapTexture.None, new Vector2(posMenuRight + 160, 830), "P",
                   new List<Keys>(new Keys[] { Keys.P })));

                _buttonsTexture.Add(new ButtonTexture(this, _imgAccept, _debug, MapTexture.None, new Vector2(posMenuRight + 115, 1000), "Entrer",
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



















                //_gui.Screen = new GuiScreen((GraphicsDevice.Viewport.Width * 85 / 100) / VirtualWidth, GraphicsDevice.Viewport.Height / VirtualHeight);
































                _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
                GuiButtonControl deleteButton = null;
                foreach (GuiButtonControl button in _gui.Screen.Desktop.Children)
                {
                    if (button.Name == "button") deleteButton = button;
                }

                State.ActualState = StateType.Default;
                _gui.Screen.Desktop.Children.Remove(deleteButton);
            }
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
                Text = "Points de vie ( Min " + Constant.MinWallHp + ", Max " + Constant.MaxWallHp + " )",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var healthWall = new GuiInputControl
            {
                Name = "healthWall",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 55), new UniScalar(100), new UniScalar(25)),
                Text = _map.Wall.CurrentHp+""
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
                bool _try;
                if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "healthWall") _try = Int32.TryParse(inputSize.Text, out _healthWall);
                }
            }

            if (_healthWall >= Constant.MinWallHp && _healthWall <= Constant.MaxWallHp)
            {
                _map.Wall.ChangeHp(_healthWall);

                _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            }
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
                    if (control.Name == "posX") Int32.TryParse(inputSize.Text, out _posX);
                    if (control.Name == "posY") Int32.TryParse(inputSize.Text, out _posY);
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

            var pathNotFound = new GuiLabelControl
            {
                Name = "pathNotFound",
                Bounds = new UniRectangle(new UniScalar(0.0f, 90), new UniScalar(0.0f, 185), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Aucun chemin valide vers la base !"
            };

            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Retour"
            };

            deleteSpawn.Pressed += DeleteSpawn_Pressed;
            modifyWave.Pressed += ModifyWave_Pressed;
            button2.Pressed += SpawnCancel_Pressed;
            addWave.Pressed += CreateWave_Pressed;
            deleteWave.Pressed += DeleteWave_Pressed;

            window.Children.Add(wavesList);
            for (int i = 0; i < _spawn.Waves.Count; i++)
            {
                Wave wave = _spawn.Waves[i];
                wavesList.Items.Add("Vague " + (i+1) + " : Depart " + wave.TimerBeforeStarting/60 + " secondes et contient " + wave.Enemies.Count + " enemies" );
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
            if (_spawn.ShortestWay == null)
                window.Children.Add(pathNotFound);
            window.Children.Add(button2);
            _gui.Screen.Desktop.Children.Add(window);

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
            int? _nbWave = null;
            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiListControl))
                {
                    GuiListControl input = (GuiListControl)control;
                    if (input.SelectedItems.Count > 0)
                        _nbWave = input.SelectedItems[0];
                }
                else if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "posX") _posX = Convert.ToInt32(inputSize.Text);
                    if (control.Name == "posY") _posY = Convert.ToInt32(inputSize.Text);
                }
            }

            if (_nbWave != null)
            {
                foreach (Spawn _spawn in _map.SpawnsEnemies)
                {
                    // On cible le spawn
                    if (_spawn.Position == new Vector2(_posX, _posY))
                        _spawn.DeleteWave((int)_nbWave);
                }

                _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
                Spawn_Pressed(new Vector2(_posX, _posY));
            }
        }

        private void ModifyWave_Pressed(object sender, System.EventArgs e)
        {
            int _posX = 0;
            int _posY = 0;
            int? _nbWave = null;
            Spawn _spawn = null;
            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiListControl))
                {
                    GuiListControl input = (GuiListControl)control;
                    if (input.SelectedItems.Count > 0)
                        _nbWave = input.SelectedItems[0];
                }
                else if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "posX") _posX = Convert.ToInt32(inputSize.Text);
                    if (control.Name == "posY") _posY = Convert.ToInt32(inputSize.Text);
                }
            }

            if (_nbWave != null)
            {
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
                WindowModifyWave_Pressed(_spawn.Waves[(int)_nbWave], _nbSpawn, (int)_nbWave);
            }
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
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(500), new UniScalar(350))),
                Title = "Vague",
                EnableDragging = true
            };

            var labelTimerWave = new GuiLabelControl()
            {
                Text = "Temps avant depart de la vague en secondes ( Min " + Constant.MinEarthlyTimer + ", Max " + Constant.MaxEarthlyTimer + " )",
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
                Text = "Nombres d'ennemis de base ( Min " + Constant.MinDefaultUnit + ", Max " + Constant.MaxDefaultUnit + " )",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 80), new UniScalar(100), new UniScalar(25))
            };

            Dictionary<EnemyType, List<Enemy>> _dicEnemies = wave.GetAllEnemiesByType();
            var enemyDefault = new GuiInputControl
            {
                Name = "enemyDefault",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 105), new UniScalar(100), new UniScalar(25)),
                Text = _dicEnemies[EnemyType.defaultSoldier].Count +""
            };

            var labelKamikaze = new GuiLabelControl()
            {
                Text = "Nombres de Kamikaze ( Min " + Constant.MinKamikazeUnit + ", Max " + Constant.MaxKamikazeUnit + " )",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 130), new UniScalar(100), new UniScalar(25))
            };

            var kamikaze = new GuiInputControl
            {
                Name = "kamikaze",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 155), new UniScalar(100), new UniScalar(25)),
                Text = _dicEnemies[EnemyType.kamikaze].Count + ""
            };

            var labelSaboteur = new GuiLabelControl()
            {
                Text = "Nombres de Saboteur ( Min " + Constant.MinSaboteurUnit + ", Max " + Constant.MaxSaboteurUnit + " )",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 180), new UniScalar(100), new UniScalar(25))
            };

            var saboteur = new GuiInputControl
            {
                Name = "saboteur",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 205), new UniScalar(100), new UniScalar(25)),
                Text = _dicEnemies[EnemyType.saboteur].Count + ""
            };

            var labelDoctor = new GuiLabelControl()
            {
                Text = "Nombres de Docteur ( Min " + Constant.MinDoctorUnit + ", Max " + Constant.MaxDoctorUnit + " )",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 230), new UniScalar(100), new UniScalar(25))
            };

            var doctor = new GuiInputControl
            {
                Name = "doctor",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 255), new UniScalar(100), new UniScalar(25)),
                Text = _dicEnemies[EnemyType.doctor].Count + ""
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

            window.Children.Add(nbSpawn);
            window.Children.Add(nbWave);
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
            int _nbSpawn = 0;
            int _nbWave = 0;
            int _timer = 0;
            int _defaultUnit = 0;
            int _saboteurUnit = 0;
            int _doctorUnit = 0;
            int _kamikazeUnit = 0;


            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "nbSpawn") Int32.TryParse(inputSize.Text, out _nbSpawn);
                    if (control.Name == "nbWave") Int32.TryParse(inputSize.Text, out _nbWave);
                    if (control.Name == "timerWave") Int32.TryParse(inputSize.Text, out _timer);
                    if (control.Name == "enemyDefault") Int32.TryParse(inputSize.Text, out _defaultUnit);
                    if (control.Name == "saboteur") Int32.TryParse(inputSize.Text, out _saboteurUnit);
                    if (control.Name == "doctor") Int32.TryParse(inputSize.Text, out _doctorUnit);
                    if (control.Name == "kamikaze") Int32.TryParse(inputSize.Text, out _kamikazeUnit);
                }
            }

            Wave _wave = _map.SpawnsEnemies[_nbSpawn].Waves[_nbWave];

            if (_timer >= Constant.MinEarthlyTimer && _timer <= Constant.MaxEarthlyTimer &&
                _defaultUnit >= Constant.MinDefaultUnit && _defaultUnit <= Constant.MaxDefaultUnit &&
                _saboteurUnit >= Constant.MinSaboteurUnit && _saboteurUnit <= Constant.MaxSaboteurUnit &&
                _doctorUnit >= Constant.MinDoctorUnit && _doctorUnit <= Constant.MaxDoctorUnit &&
                _kamikazeUnit >= Constant.MinKamikazeUnit && _kamikazeUnit <= Constant.MaxKamikazeUnit)
            {
                _wave.TimerBeforeStarting = _timer * 60;
                Dictionary<EnemyType, List<Enemy>> _dicUnits = _wave.GetAllEnemiesByType();

                int _nbUnit = _dicUnits[EnemyType.defaultSoldier].Count;
                if (_defaultUnit > _nbUnit)
                    _wave.CreateEnemies(EnemyType.defaultSoldier, _defaultUnit - _nbUnit);
                else if (_defaultUnit < _nbUnit)
                {
                    _dicUnits[EnemyType.defaultSoldier].RemoveRange(0, _defaultUnit);
                    _wave.DeleteEnemies(EnemyType.defaultSoldier, _dicUnits[EnemyType.defaultSoldier]);
                }

                _nbUnit = _dicUnits[EnemyType.saboteur].Count;
                if (_saboteurUnit > _nbUnit)
                    _wave.CreateEnemies(EnemyType.saboteur, _saboteurUnit - _nbUnit);
                else if (_saboteurUnit < _nbUnit)
                {
                    _dicUnits[EnemyType.saboteur].RemoveRange(0, _saboteurUnit);
                    _wave.DeleteEnemies(EnemyType.saboteur, _dicUnits[EnemyType.saboteur]);
                }

                _nbUnit = _dicUnits[EnemyType.doctor].Count;
                if (_doctorUnit > _nbUnit)
                    _wave.CreateEnemies(EnemyType.doctor, _doctorUnit - _nbUnit);
                else if (_doctorUnit < _nbUnit)
                {
                    _dicUnits[EnemyType.doctor].RemoveRange(0, _doctorUnit);
                    _wave.DeleteEnemies(EnemyType.doctor, _dicUnits[EnemyType.doctor]);
                }

                _nbUnit = _dicUnits[EnemyType.kamikaze].Count;
                if (_kamikazeUnit > _nbUnit)
                    _wave.CreateEnemies(EnemyType.kamikaze, _kamikazeUnit - _nbUnit);
                else if (_kamikazeUnit < _nbUnit)
                {
                    _dicUnits[EnemyType.kamikaze].RemoveRange(0, _kamikazeUnit);
                    _wave.DeleteEnemies(EnemyType.kamikaze, _dicUnits[EnemyType.kamikaze]);
                }

                _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
                Spawn_Pressed(new Vector2(_map.SpawnsEnemies[_nbSpawn].Position.X, _map.SpawnsEnemies[_nbSpawn].Position.Y));
            }
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
                    if (control.Name == "nbSpawnX") Int32.TryParse(inputSize.Text, out _nbSpawn);
                    if (control.Name == "nbWaveX") Int32.TryParse(inputSize.Text, out _nbWave);
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
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(320), new UniScalar(200))),
                Title = "Option Map",
                EnableDragging = true
            };

            var labelNameMap = new GuiLabelControl()
            {
                Text = "Nom de la map (Min Carac " + Constant.MinNameMap + ", Max Carac " + Constant.MaxNameMap + " )",
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
                Text = "Dollars initial de la map ( Min " + Constant.MinDollarsMap + ", Max " + Constant.MaxDollarsMap + " )",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 85), new UniScalar(100), new UniScalar(25))
            };

            var dollarsMap = new GuiInputControl
            {
                Name = "dollarsMap",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 105), new UniScalar(100), new UniScalar(25)),
                Text = _map.Dollars+""
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
                    if (control.Name == "dollarsMap") Int32.TryParse(inputSize.Text, out _dollarsMap);
                }
            }

            if ((_nameMap.Length >= Constant.MinNameMap && _nameMap.Length <= Constant.MaxNameMap) &&
                (_dollarsMap >= Constant.MinDollarsMap && _dollarsMap <= Constant.MaxDollarsMap))
            {
                _map.SettingTheMap(_nameMap, _dollarsMap);

                _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            }
        }
        #endregion

        #region Window Manager Air Plane
        public void ManagerAirPlane_Pressed()
        {
            var window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(540), new UniScalar(220))),
                Title = "Menu vague aerienne",
                EnableDragging = true
            };

            var wavesListAir = new GuiListControl()
            {
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(1.0f, -20), new UniScalar(0f, 80)),
            };

            var labelInfoWarning = new GuiLabelControl
            {
                Name = "InfoWarning",
                Bounds = new UniRectangle(new UniScalar(0.0f, 50), new UniScalar(0.0f, 120), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Vous devez avoir une base avant de creer des vagues d'avions !"
            };

            var addWaveAir = new GuiButtonControl
            {
                Name = "AddWaveAir",
                Bounds = new UniRectangle(new UniScalar(0.0f, 37), new UniScalar(0.0f, 120), new UniScalar(0f, 130), new UniScalar(0f, 30)),
                Text = "Ajouter une vague"
            };

            var modifyWaveAir = new GuiButtonControl
            {
                Name = "ModifyWaveAir",
                Bounds = new UniRectangle(new UniScalar(0.0f, 204), new UniScalar(0.0f, 120), new UniScalar(0f, 130), new UniScalar(0f, 30)),
                Text = "Modifier vague"
            };

            var deleteWaveAir = new GuiButtonControl
            {
                Name = "DeleteWave",
                Bounds = new UniRectangle(new UniScalar(0.0f, 371), new UniScalar(0.0f, 120), new UniScalar(0f, 130), new UniScalar(0f, 30)),
                Text = "Suppr vague"
            };

            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(0.0f, 225), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
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
                wavesListAir.Items.Add("Vague aerienne " + (i + 1) + " : Depart " + collectionUnit.TimerBeforeStarting/60 + " secondes, " + collectionUnit.Array.Count + " avions et de type " + collectionUnit.Array[0].Name.ToLower());
            }
            wavesListAir.SelectionMode = ListSelectionMode.Single;
           
            if (_map.Wall == null)
            {
                window.Children.Add(labelInfoWarning);
            }
            else
            {
                window.Children.Add(addWaveAir);
                if (_map.AirUnits.Count > 0)
                {
                    window.Children.Add(modifyWaveAir);
                    window.Children.Add(deleteWaveAir);
                }
            }

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
                    if (input.SelectedItems.Count > 0)
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
                    if (input.SelectedItems.Count > 0)
                        _unitsCollection = _map.AirUnits[input.SelectedItems[0]];
                }
            }
            if (_unitsCollection != null)
            {
                _map.DeleteAirWave(_unitsCollection);
                _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
                ManagerAirPlane_Pressed();
            }
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
                Text = "Temps avant depart de la vague ( Min " + Constant.MinPlaneTimer + ", Max " + Constant.MaxPlaneTimer + " )",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var timerAir = new GuiInputControl
            {
                Name = "timerAir",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 55), new UniScalar(100), new UniScalar(25)),
            };
            if (unitsCollection != null) timerAir.Text = unitsCollection.TimerBeforeStarting/60+"";
            else timerAir.Text = Constant.MinPlaneTimer+"";

            var labelNumberPlane = new GuiLabelControl()
            {
                Text = "Nombres d'avions ( Min " + Constant.MinPlaneInWave + ", Max " + Constant.MaxPlaneInWave + " )",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 80), new UniScalar(100), new UniScalar(25))
            };

            var numberPlane = new GuiInputControl
            {
                Name = "numberPlane",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 105), new UniScalar(100), new UniScalar(25)),
            };
            if (unitsCollection != null) numberPlane.Text = unitsCollection.Array.Count + "";
            else numberPlane.Text = Constant.MinPlaneInWave+"";

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
            {
                if (i == 0)
                { typeAir.Items.Add("Avion Lent"); }
                else if (i == 1)
                { typeAir.Items.Add("Avion Normal"); }
                else if (i == 2)
                { typeAir.Items.Add("Avion Rapide"); }
            }

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
            if (unitsCollection != null)
            {
                var unitCollection = new GuiInputControl
                {
                    Name = "unitCollection",
                    Text = unitsCollection.Ctx.AirUnits.IndexOf(unitsCollection) + ""
                };

                window.Children.Add(unitCollection);
            }
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
            int? _unitCollectionNumber = null;
            int _nbPlane = 0;
            int _timer = 0;
            PlaneType _type = PlaneType.None;

            try
            {
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
                        if (control.Name == "numberPlane") Int32.TryParse(inputSize.Text, out _nbPlane);
                        if (control.Name == "timerAir") Int32.TryParse(inputSize.Text, out _timer);
                        if (control.Name == "unitCollection") _unitCollectionNumber = Convert.ToInt32(inputSize.Text);
                    }
                }

                if (_type != PlaneType.None &&
                    _timer >= Constant.MinPlaneTimer && _timer <= Constant.MaxPlaneTimer &&
                    _nbPlane >= Constant.MinPlaneInWave && _nbPlane <= Constant.MaxPlaneInWave)
                {
                    if (_unitCollectionNumber == null)
                        _map.AirUnits.Add(new AirUnitsCollection(_map, _timer * 60, _nbPlane, _type, _map.Wall));
                    else
                    {
                        _map.AirUnits.RemoveAt((int)_unitCollectionNumber);
                        _map.AirUnits.Add(new AirUnitsCollection(_map, _timer * 60, _nbPlane, _type, _map.Wall));
                    }

                    _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
                    ManagerAirPlane_Pressed();
                }
            }
            catch { }
        }
        #endregion

        #region Window Info Serialization Map
        private void InfoSerialization_Pressed(bool hasName, bool hasBase, bool hasPath)
        {
            var window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(400), new UniScalar(180))),
                Title = "Erreur sauvegarde de la map",
                EnableDragging = true
            };

            var labelInfoSauvegarde = new GuiLabelControl()
            {
                Text = "La map n'a pas pu etre sauvegarde car : ",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var labelName = new GuiLabelControl()
            {
                Text = "Elle ne possede pas de nom",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var labelBase = new GuiLabelControl()
            {
                Text = "Elle ne possede pas de base",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var labelPath = new GuiLabelControl()
            {
                Text = "Tous les spawns d'ennemis ne possede pas un chemin valide les reliant a la base (le ou les spawn(s) sont colorie en rouge)",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Retour"
            };

            button2.Pressed += InfoSerCancel_Pressed;

            window.Children.Add(labelInfoSauvegarde);
            int positionX = 0;
            if (!hasName)
            {
                window.Children.Add(labelName);
                positionX += 30;
                labelName.Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30 + positionX), new UniScalar(100), new UniScalar(25));
            }
            if (!hasBase)
            {
                window.Children.Add(labelBase);
                positionX += 30;
                labelBase.Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30 + positionX), new UniScalar(100), new UniScalar(25));
            }
            if (!hasPath)
            {
                window.Children.Add(labelPath);
                positionX += 30;
                labelPath.Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30 + positionX), new UniScalar(100), new UniScalar(25));
                window.Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(900), new UniScalar(180)));
            }
            window.Children.Add(button2);

            _gui.Screen.Desktop.Children.Add(window);
        }

        private void InfoSerCancel_Pressed(object sender, System.EventArgs e)
        {
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
        }
        #endregion

        #endregion

        public List<Texture2D> ImgMaps => _imgMaps;
        public Texture2D ImgWall => _imgWall;

    }
}

