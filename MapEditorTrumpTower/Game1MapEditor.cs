﻿using LibraryTrumpTower;
using LibraryTrumpTower.AirUnits;
using LibraryTrumpTower.Constants;
using LibraryTrumpTower.Decors;
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
using System.Linq;
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
        private Dictionary<string, List<Texture2D>> _imgThemesMaps;
        private Dictionary<string, List<Texture2D>> _imgThemesDecorsMaps;
        public Map _map;

        public SelectorTexture SelectTexture { get; private set; }

        private SpriteFont _debug;

        public List<Texture2D> _imgDecors;
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

        private Texture2D _imgMenuTexture;
        private Texture2D _imgOptions;
        private Texture2D _imgOutilDeSelection;
        private Texture2D _imgTextureDecors;
        private Texture2D _imgTextureUtile;
        private Texture2D _imgValidation;
        private Texture2D _imgSelectionActuelle;
        private Texture2D _imgInformation;
        private Texture2D _imgEntity;
        private Texture2D _imgTree;
        private Texture2D _imgChangeTheme;

        private SpriteFont _imgString;
        private Texture2D _imgHappyFace;
        #region gameState
        public GameState State { get; set; }
        public ActionCreatePath CurrentActionCreatePath { get; set; }
        #endregion

        int posMenuRight;
        int _lastRefresh;

        #region mesure menu droite
        double distanceUpMainTitle;
        double distanceDownMainTitle;
        double distanceDownTitle;
        double distanceDownImg;
        #endregion

        int _timerInfo;
        double _timerTransparancy;

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
            Window.IsBorderless = true;
            {
                int attempts = 0;
                while (true)
                {
                    try
                    {
                        graphics.IsFullScreen = true;
                        graphics.ApplyChanges();
                        break;
                    }
                    catch (SharpDX.SharpDXException ex)
                    {
                        if (ex.HResult != -2005270494 || attempts > 150) throw;
                        attempts++;
                    }
                }
            }
            graphics.ApplyChanges();

            _gui.Screen = new GuiScreen(graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height);

            _gui.Screen.Desktop.Bounds = new UniRectangle(new UniScalar(0f, 0), new UniScalar(0f, 0), new UniScalar(1f, 0), new UniScalar(1f, 0));
            // Perform second-stage initialization
            _gui.Initialize();

            if (File.Exists(BinarySerializer.pathCurrentMapXml))
            {
                _map = BinarySerializer.Deserialize<Map>(BinarySerializer.pathCurrentMapXml);
                foreach (Spawn spawn in _map.SpawnsEnemies)
                {
                    foreach (Wave wave in spawn.Waves) Map.WavesTotals++;
                }
            }
            else Button2_Pressed();

            _lastRefresh = 0;
            posMenuRight = GraphicsDevice.Viewport.Width * 85 / 100;
            _timerInfo = 0;
            _timerTransparancy = 1;

            distanceUpMainTitle = 10;
            distanceDownMainTitle = 80;
            distanceDownTitle = 20;
            distanceDownImg = 70;
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
            _imgThemesMaps = new Dictionary<string, List<Texture2D>>();
            _imgThemesMaps["World_Jungle"] = new List<Texture2D>();
            _imgThemesMaps["World_Snow"] = new List<Texture2D>();
            _imgThemesMaps["World_City"] = new List<Texture2D>();

            _imgThemesDecorsMaps = new Dictionary<string, List<Texture2D>>();
            _imgThemesDecorsMaps["World_Jungle"] = new List<Texture2D>();
            _imgThemesDecorsMaps["World_Snow"] = new List<Texture2D>();
            _imgThemesDecorsMaps["World_City"] = new List<Texture2D>();

            foreach (string nameWorld in _imgThemesMaps.Keys)
            {
                foreach (string name in Enum.GetNames(typeof(MapTexture)))
                {
                    if (name == "dirt" || name == "grass" || name == "emptyTower" || name == "notEmptyTower" || name == "myBase")
                        _imgThemesMaps[nameWorld].Add(Content.Load<Texture2D>("Map/" + nameWorld + "/" + name));
                    else if (name != "None") _imgThemesMaps[nameWorld].Add(null);
                }
                for (int y = 1; y < 9; y++)
                {
                    _imgThemesDecorsMaps[nameWorld].Add(Content.Load<Texture2D>("Map/" + nameWorld + "/decor/decor" + y));
                }
            }

            if (_map != null)
            {
                if (_map.ThemeOfMap != ThemeMap.None)
                {
                    _imgMaps = _imgThemesMaps[nameof(_map.ThemeOfMap)];
                    _imgDecors = _imgThemesMaps[nameof(_map.ThemeOfMap)];
                }
                else
                {
                    _imgMaps = _imgThemesMaps[nameof(ThemeMap.World_Jungle)];
                    _imgDecors = _imgThemesMaps[nameof(ThemeMap.World_Jungle)];
                }
            }
            else
            {
                _imgMaps = _imgThemesMaps[nameof(ThemeMap.World_Jungle)];
                _imgDecors = _imgThemesMaps[nameof(ThemeMap.World_Jungle)];
            }
            #endregion

            #region Load Name Menu
            _imgMenuTexture = Content.Load<Texture2D>("NameMenu/Menu-Texture");
            _imgOptions = Content.Load<Texture2D>("NameMenu/Options");
            _imgOutilDeSelection = Content.Load<Texture2D>("NameMenu/Outil-de-selection");
            _imgTextureDecors = Content.Load<Texture2D>("NameMenu/Texture-Decors");
            _imgTextureUtile = Content.Load<Texture2D>("NameMenu/Texture-Utile");
            _imgValidation = Content.Load<Texture2D>("NameMenu/Validation");
            _imgSelectionActuelle = Content.Load<Texture2D>("NameMenu/Selection-Actuelle");
            _imgTree = Content.Load<Texture2D>("NameMenu/tree");
            _imgChangeTheme = Content.Load<Texture2D>("NameMenu/changeTheme");
            #endregion

            _imgAccept = Content.Load<Texture2D>("accept");
            _imgWall = Content.Load<Texture2D>("Wall");
            _imgNoSelect = Content.Load<Texture2D>("select");
            _imgCloakTexture = Content.Load<Texture2D>("cloakImgMap");
            _debug = Content.Load<SpriteFont>("DefaultFont");
            _imgNextWaveIsComming = Content.Load<Texture2D>("north_korea_is_comming");
            _imgClipBoards = Content.Load<Texture2D>("clipboards");

            _imgPlane1 = Content.Load<Texture2D>("Enemies/plane1");
            _imgEnemy1 = Content.Load<Texture2D>("Enemies/enemy1");
            _imgKamikaze = Content.Load<Texture2D>("Enemies/kamikaze");
            _imgDoctor = Content.Load<Texture2D>("Enemies/doctor");
            _imgSaboteur = Content.Load<Texture2D>("Enemies/saboteur");

            _imgHappyFace = Content.Load<Texture2D>("Entity/happy");
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

            _imgString = Content.Load<SpriteFont>("info");

            ManagerSound.LoadContent(Content);

            if (_map != null)
                InitMapCreator();
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

            if (State.ActualState == StateType.Default)
            {
                // On actualise toutes les 5 secondes
                if (_lastRefresh + 15 == gameTime.TotalGameTime.Seconds)
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

                if (newStateKeyboard.IsKeyDown(Keys.L) && !lastStateKeyboard.IsKeyDown(Keys.L))
                    _map.Decors = GeneratorDecors.Generate(_map.MapArray);
                else if (newStateKeyboard.IsKeyDown(Keys.H) && !lastStateKeyboard.IsKeyDown(Keys.H))
                    MapSetting_Pressed();
                else if (newStateKeyboard.IsKeyDown(Keys.J) && !lastStateKeyboard.IsKeyDown(Keys.J))
                    ManagerAirPlane_Pressed();
                else if (newStateKeyboard.IsKeyDown(Keys.K) && !lastStateKeyboard.IsKeyDown(Keys.K))
                {
                    /*string[] themesMap = Enum.GetNames(typeof(ThemeMap));
                    int compteur = 0;
                    foreach(string theme in themesMap)
                    {
                        if (theme == nameof(_map.ThemeOfMap))
                        {
                            compteur++;
                            break;
                        }
                        compteur++;
                    }*/
                    ChangeTheme(Extensions.Next<ThemeMap>(_map.ThemeOfMap));
                }
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
                        bool nameIsExisting = false;
                        if (hasName)
                        {
                            string[] filesInDirectory = Directory.GetFiles(BinarySerializer.pathCustomMap);
                            for (int i = 0; i < filesInDirectory.Length; i++)
                            {
                                if (filesInDirectory[i] == (BinarySerializer.pathCustomMap + "\\" + _map.Name + ".xml")) nameIsExisting = true;
                            }
                        }

                        if (nameIsExisting)
                            ConfirmEraseMap_Pressed();
                        else
                        {
                            BinarySerializer.Serialize(_map, "CustomMap/" + _map.Name + ".xml");
                            Exit();
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
            GraphicsDevice.Clear(Color.SlateGray);

            // TODO: Add your drawing code here

            if (_map != null)
            {
                #region Draw Right Menu
                spriteBatch.Begin();


                if (State.ActualState == StateType.Default)
                {
                    int pos = posMenuRight + ((graphics.GraphicsDevice.Viewport.Width - posMenuRight) / 2);
                    float positionCursor = 0;
                    positionCursor = (float)distanceUpMainTitle;
                    spriteBatch.Draw(_imgMenuTexture, new Vector2(pos - _imgMenuTexture.Width / 2, positionCursor), Color.White);
                    positionCursor += _imgMenuTexture.Height + (float)distanceDownMainTitle;
                    spriteBatch.Draw(_imgTextureUtile, new Vector2(pos - _imgTextureUtile.Width / 2, positionCursor), Color.White);
                    positionCursor += 64 + _imgTextureUtile.Height + (float)distanceDownTitle + (float)distanceDownImg;
                    spriteBatch.Draw(_imgOutilDeSelection, new Vector2(pos - _imgOutilDeSelection.Width / 2, positionCursor), Color.White);
                    positionCursor += 64 + _imgOutilDeSelection.Height + (float)distanceDownTitle + (float)distanceDownImg;
                    spriteBatch.Draw(_imgOptions, new Vector2(pos - _imgOptions.Width / 2, positionCursor), Color.White);
                    positionCursor += 64 + _imgOptions.Height + (float)distanceDownTitle + (float)distanceDownImg;
                    spriteBatch.Draw(_imgValidation, new Vector2(pos - _imgValidation.Width / 2, positionCursor), Color.White);
                    positionCursor += 64 + _imgValidation.Height + (float)distanceDownTitle + (float)distanceDownImg;
                    spriteBatch.Draw(_imgSelectionActuelle, new Vector2(pos - _imgSelectionActuelle.Width / 2, positionCursor), Color.White);

                    Texture2D currentSelectionTexture = null;
                    if (SelectTexture.Texture == MapTexture.None)
                        currentSelectionTexture = _imgNoSelect;
                    else if (SelectTexture.Texture == MapTexture.myBase)
                        currentSelectionTexture = ImgWall;
                    else
                        currentSelectionTexture = ImgMaps[(int)SelectTexture.Texture];
                    spriteBatch.Draw(currentSelectionTexture, new Vector2(posMenuRight + 115, 910), Color.White);




                    for (int i = 0; i < _buttonsTexture.Count; i++)
                    {
                        ButtonTexture _buttonTexture = _buttonsTexture[i];
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


                foreach (Decor decor in _map.Decors)
                    spriteBatch.Draw(_imgDecors[decor._numberDecor], new Vector2(decor._position.X - _imgDecors[decor._numberDecor].Width, decor._position.Y - _imgDecors[decor._numberDecor].Height), Color.White);

                #region Draw Spawn
                for (int i = 0; i < _map.SpawnsEnemies.Count; i++)
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
                spriteBatch.DrawString(_imgNextWave, "Waves " + Map.WavesCounter + "/" + Map.WavesTotals, new Vector2(50, 57), Color.White);
                #endregion

                #region Entity
                spriteBatch.Draw(_backgroundDollars, new Vector2(5, 90), sourceRectanglee, Color.Black * 0.6f);
                spriteBatch.Draw(_imgHappyFace, new Vector2(10, 90), Color.White);
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
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -200), new UniScalar(0.5f, -90)), new UniVector(new UniScalar(400), new UniScalar(180))),
                Title = "Map size",
                EnableDragging = true
            };

            var labelChoiceX = new GuiLabelControl()
            {
                Text = "Indicate the size of the map ( Min " + Constant.MinWidthMap + ", Max " + Constant.MaxWidthMap + " )",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var choiceX = new GuiInputControl
            {
                Name = "choiceX",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 55), new UniScalar(100), new UniScalar(25)),
                Text = Constant.MinWidthMap + ""
            };

            var button1 = new GuiButtonControl
            {
                Name = "confirm",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Confirm"
            };
            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Return"
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

                _sizeY = (int)Math.Truncate(_size / (16.0f / 9));
                int[,] _mapPoint2D = new int[(int)_sizeY, _size];
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

                InitMapCreator();

                _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
                GuiButtonControl deleteButton = null;
                foreach (GuiButtonControl button in _gui.Screen.Desktop.Children)
                {
                    if (button.Name == "button") deleteButton = button;
                }

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
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -150), new UniScalar(0.5f, -100)), new UniVector(new UniScalar(300), new UniScalar(200))),
                Title = "Defining the health points of the base",
                EnableDragging = true
            };

            var labelHealthWall = new GuiLabelControl()
            {
                Text = "Health points ( Min " + Constant.MinWallHp + ", Max " + Constant.MaxWallHp + " )",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var healthWall = new GuiInputControl
            {
                Name = "healthWall",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 55), new UniScalar(100), new UniScalar(25)),
                Text = _map.Wall.CurrentHp + ""
            };

            var button1 = new GuiButtonControl
            {
                Name = "confirm",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Confirm"
            };
            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Return"
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
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -150), new UniScalar(0.5f, -100)), new UniVector(new UniScalar(300), new UniScalar(200))),
                Title = "Spawn",
                EnableDragging = true
            };

            var labelCreateSpawn = new GuiLabelControl()
            {
                Text = "Create a spawn ?",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var button1 = new GuiButtonControl
            {
                Name = "confirm",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Confirm"
            };
            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Return"
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
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -200), new UniScalar(0.5f, -150)), new UniVector(new UniScalar(400), new UniScalar(300))),
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
                Text = "Add wave"
            };

            var modifyWave = new GuiButtonControl
            {
                Name = "ModifyWave",
                Bounds = new UniRectangle(new UniScalar(1.0f, -250), new UniScalar(0.0f, 120), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Modify wave"
            };

            var deleteWave = new GuiButtonControl
            {
                Name = "DeleteWave",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(0.0f, 120), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Delete wave"
            };

            var deleteSpawn = new GuiButtonControl
            {
                Name = "deleteSpawn",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Delete spawn"
            };

            var pathNotFound = new GuiLabelControl
            {
                Name = "pathNotFound",
                Bounds = new UniRectangle(new UniScalar(0.0f, 90), new UniScalar(0.0f, 185), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "No valid path to the base !"
            };

            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Return"
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
                wavesList.Items.Add("Wave " + (i + 1) + " : Departure " + wave.TimerBeforeStarting / 60 + " seconds and contains " + wave.Enemies.Count + " enemies");
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
                    _spawn.CreateWave(new Wave(_spawn, new List<Enemy>(), 5 * 60));
                    break;
                }
            }

            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            Spawn_Pressed(new Vector2(_posX, _posY));

            // Spawn_Pressed ne s'actualise pas si on ne change pas la position de la souris aller savoir pourquoi ...
            _gui.InputCapturer.InputReceiver.InjectMouseMove(0.0001f,0.0001f);
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
                Text = nbWaveX + ""
            };

            var window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -250), new UniScalar(0.5f, -175)), new UniVector(new UniScalar(500), new UniScalar(350))),
                Title = "Wave",
                EnableDragging = true
            };

            var labelTimerWave = new GuiLabelControl()
            {
                Text = "Timer before the start of the wave in seconds ( Min " + Constant.MinEarthlyTimer + ", Max " + Constant.MaxEarthlyTimer + " )",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var timerWave = new GuiInputControl
            {
                Name = "timerWave",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 55), new UniScalar(100), new UniScalar(25)),
                Text = wave.TimerBeforeStarting / 60 + ""
            };

            var labelEnemyDefault = new GuiLabelControl()
            {
                Text = "Number of Basic enemies ( Min " + Constant.MinDefaultUnit + ", Max " + Constant.MaxDefaultUnit + " )",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 80), new UniScalar(100), new UniScalar(25))
            };

            Dictionary<EnemyType, List<Enemy>> _dicEnemies = wave.GetAllEnemiesByType();
            var enemyDefault = new GuiInputControl
            {
                Name = "enemyDefault",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 105), new UniScalar(100), new UniScalar(25)),
                Text = _dicEnemies[EnemyType.defaultSoldier].Count + ""
            };

            var labelKamikaze = new GuiLabelControl()
            {
                Text = "Number of Kamikaze ( Min " + Constant.MinKamikazeUnit + ", Max " + Constant.MaxKamikazeUnit + " )",
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
                Text = "Number of Saboteur ( Min " + Constant.MinSaboteurUnit + ", Max " + Constant.MaxSaboteurUnit + " )",
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
                Text = "Number of Doctor ( Min " + Constant.MinDoctorUnit + ", Max " + Constant.MaxDoctorUnit + " )",
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
                Text = "Confirm"
            };

            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Return"
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
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -200), new UniScalar(0.5f, -175)), new UniVector(new UniScalar(400), new UniScalar(250))),
                Title = "Options Map",
                EnableDragging = true
            };

            var labelNameMap = new GuiLabelControl()
            {
                Text = "Map name (Min Carac " + Constant.MinNameMap + ", Max Carac " + Constant.MaxNameMap + " )",
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
                Text = "Initial map dollars ( Min " + Constant.MinDollarsMap + ", Max " + Constant.MaxDollarsMap + " )",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 85), new UniScalar(100), new UniScalar(25))
            };

            var dollarsMap = new GuiInputControl
            {
                Name = "dollarsMap",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 105), new UniScalar(100), new UniScalar(25)),
                Text = _map.Dollars + ""
            };

            var labelGauge = new GuiLabelControl()
            {
                Text = "Entity Gauge Speed ( Min " + Constant.MINLOSTGAUGE+ ", Max " + Constant.MAXLOSTGAUGE + ", decimal number)",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 135), new UniScalar(100), new UniScalar(25))
            };

            var Gauge = new GuiInputControl
            {
                Name = "lostGauge",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 160), new UniScalar(100), new UniScalar(25)),
                Text = (float)_map.Entity.LostGauge + ""
            };

            var button1 = new GuiButtonControl
            {
                Name = "confirm",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Confirm"
            };
            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Return"
            };

            button1.Pressed += MapSettingConfirm_Pressed;
            button2.Pressed += MapSettingCancel_Pressed;

            window.Children.Add(labelNameMap);
            window.Children.Add(nameMap);
            window.Children.Add(labelDollarsMap);
            window.Children.Add(dollarsMap);
            window.Children.Add(labelGauge);
            window.Children.Add(Gauge);
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
            float _lostGauge = 0;
            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiInputControl))
                {
                    GuiInputControl inputSize = (GuiInputControl)control;
                    if (control.Name == "nameMap") _nameMap = inputSize.Text;
                    if (control.Name == "dollarsMap") Int32.TryParse(inputSize.Text, out _dollarsMap);
                    if (control.Name == "lostGauge") float.TryParse(inputSize.Text, out _lostGauge);
                }
            }

            if ((_nameMap.Length >= Constant.MinNameMap && _nameMap.Length <= Constant.MaxNameMap) &&
                (_dollarsMap >= Constant.MinDollarsMap && _dollarsMap <= Constant.MaxDollarsMap) &&
                (_lostGauge >= Constant.MINLOSTGAUGE && _lostGauge <= Constant.MAXLOSTGAUGE))
            {
                _map.SettingTheMap(_nameMap, _dollarsMap);
                _map.Entity.LostGauge = _lostGauge;
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
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -270), new UniScalar(0.5f, -110)), new UniVector(new UniScalar(540), new UniScalar(220))),
                Title = "Air Wave Menu",
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
                Text = "You must have a base before you create waves of airplanes !"
            };

            var addWaveAir = new GuiButtonControl
            {
                Name = "AddWaveAir",
                Bounds = new UniRectangle(new UniScalar(0.0f, 37), new UniScalar(0.0f, 120), new UniScalar(0f, 130), new UniScalar(0f, 30)),
                Text = "Add Wave"
            };

            var modifyWaveAir = new GuiButtonControl
            {
                Name = "ModifyWaveAir",
                Bounds = new UniRectangle(new UniScalar(0.0f, 204), new UniScalar(0.0f, 120), new UniScalar(0f, 130), new UniScalar(0f, 30)),
                Text = "Modify Wave"
            };

            var deleteWaveAir = new GuiButtonControl
            {
                Name = "DeleteWave",
                Bounds = new UniRectangle(new UniScalar(0.0f, 371), new UniScalar(0.0f, 120), new UniScalar(0f, 130), new UniScalar(0f, 30)),
                Text = "Delete Wave"
            };

            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(0.0f, 225), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Return"
            };

            addWaveAir.Pressed += ManagerAirUnitAdd_Pressed;
            modifyWaveAir.Pressed += ManagerAirUnitModify_Pressed;
            deleteWaveAir.Pressed += ManagerAirUnitDelete_Pressed;
            button2.Pressed += ManagerAirUnitCancel_Pressed;

            window.Children.Add(wavesListAir);

            for (int i = 0; i < _map.AirUnits.Count; i++)
            {
                AirUnitsCollection collectionUnit = _map.AirUnits[i];
                wavesListAir.Items.Add("Air Wave " + (i + 1) + " : Departure " + collectionUnit.TimerBeforeStarting / 60 + " seconds, " + collectionUnit.Array.Count + " airplanes and type " + collectionUnit.Array[0].Name.ToLower());
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
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -200), new UniScalar(0.5f, -150)), new UniVector(new UniScalar(400), new UniScalar(300))),
                Title = "Add Air Plane",
                EnableDragging = true
            };

            var labelTimerAir = new GuiLabelControl()
            {
                Text = "Time before start of the wave ( Min " + Constant.MinPlaneTimer + ", Max " + Constant.MaxPlaneTimer + " )",
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
                Text = "Number of Airplanes ( Min " + Constant.MinPlaneInWave + ", Max " + Constant.MaxPlaneInWave + " )",
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
                Text = "Type of Airplanes",
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
                { typeAir.Items.Add("Slow Airplane"); }
                else if (i == 1)
                { typeAir.Items.Add("Normal Airplane"); }
                else if (i == 2)
                { typeAir.Items.Add("Fast Airplane"); }
            }

            typeAir.SelectionMode = ListSelectionMode.Single;
            #endregion

            var button1 = new GuiButtonControl
            {
                Name = "confirm",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Confirm"
            };
            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Return"
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
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -200), new UniScalar(0.5f, -90)), new UniVector(new UniScalar(400), new UniScalar(180))),
                Title = "Save Map Error",
                EnableDragging = true
            };

            var labelInfoSauvegarde = new GuiLabelControl()
            {
                Text = "The map could not be saved because : ",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var labelName = new GuiLabelControl()
            {
                Text = "It doesn't have a name",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var labelBase = new GuiLabelControl()
            {
                Text = "It does not have a base",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var labelPath = new GuiLabelControl()
            {
                Text = "Not all enemy spawns have a valid path to the base (the spawn(s) are colored red)",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Return"
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

        #region Window ConfirmeEraseMap Map
        private void ConfirmEraseMap_Pressed()
        {
            var window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -200), new UniScalar(0.5f, -90)), new UniVector(new UniScalar(400), new UniScalar(180))),
                Title = "Existing map name",
                EnableDragging = true
            };

            var labelEraseMap = new GuiLabelControl()
            {
                Text = "A map already has this name !",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };

            var labelEraseMap2 = new GuiLabelControl()
            {
                Text = "Are you sure you want to overwrite him?",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 55), new UniScalar(100), new UniScalar(25))
            };

            var button1 = new GuiButtonControl
            {
                Name = "confirm",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Confirm"
            };

            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Return"
            };

            button1.Pressed += ConfirmEraseMap_Pressed;
            button2.Pressed += CancelEraseMap_Pressed;

            window.Children.Add(labelEraseMap);
            window.Children.Add(labelEraseMap2);

            window.Children.Add(button1);
            window.Children.Add(button2);

            _gui.Screen.Desktop.Children.Add(window);
        }

        private void ConfirmEraseMap_Pressed(object sender, System.EventArgs e)
        {
            BinarySerializer.Serialize(_map, "CustomMap/" + _map.Name + ".xml");
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            Exit();
        }

        private void CancelEraseMap_Pressed(object sender, System.EventArgs e)
        {
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
        }
        #endregion

        #endregion

        public List<Texture2D> ImgMaps => _imgMaps;
        public Texture2D ImgWall => _imgWall;
        public void InitMapCreator()
        {
            SelectTexture = new SelectorTexture(this, _map, _imgCloakTexture);
           
            float positionCursor = (float)distanceUpMainTitle + _imgMenuTexture.Height + (float)distanceDownMainTitle + _imgTextureUtile.Height + (float)distanceDownTitle;
            #region Button Left Menu Default
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirt], _debug, MapTexture.dirt, new Vector2(posMenuRight + 10, positionCursor), "1",
                new List<Keys>(new Keys[] { Keys.D1 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.grass], _debug, MapTexture.grass, new Vector2(posMenuRight + 80, positionCursor), "2",
                new List<Keys>(new Keys[] { Keys.D2 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.emptyTower], _debug, MapTexture.emptyTower, new Vector2(posMenuRight + 150, positionCursor), "3",
                new List<Keys>(new Keys[] { Keys.D3 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgWall, _debug, MapTexture.myBase, new Vector2(posMenuRight + 220, positionCursor), "4",
                new List<Keys>(new Keys[] { Keys.D4 })));

            positionCursor += 64 + (float)distanceDownImg + _imgTextureUtile.Height + (float)distanceDownTitle;
            _buttonsTexture.Add(new ButtonTexture(this, _imgNoSelect, _debug, MapTexture.None, new Vector2(posMenuRight + 115, positionCursor), "A",
                new List<Keys>(new Keys[] { Keys.A })));

            positionCursor += 64 + (float)distanceDownImg + _imgOutilDeSelection.Height + (float)distanceDownTitle;
            _buttonsTexture.Add(new ButtonTexture(this, _imgClipBoards, _debug, MapTexture.None, new Vector2(posMenuRight + 10, positionCursor), "H",
                new List<Keys>(new Keys[] { Keys.H })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgPlane1, _debug, MapTexture.None, new Vector2(posMenuRight + 80, positionCursor), "J",
               new List<Keys>(new Keys[] { Keys.J })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgChangeTheme, _debug, MapTexture.None, new Vector2(posMenuRight + 150, positionCursor), "K",
               new List<Keys>(new Keys[] { Keys.K })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgTree, _debug, MapTexture.None, new Vector2(posMenuRight + 220, positionCursor), "L",
               new List<Keys>(new Keys[] { Keys.L })));

            positionCursor += 64 + (float)distanceDownImg + _imgOptions.Height + (float)distanceDownTitle;
            _buttonsTexture.Add(new ButtonTexture(this, _imgAccept, _debug, MapTexture.None, new Vector2(posMenuRight + 115, positionCursor), "Entrer",
               new List<Keys>(new Keys[] { Keys.Enter })));

            #endregion

            VirtualWidth = _map.WidthArrayMap * Constant.imgSizeMap;
            VirtualHeight = _map.HeightArrayMap * Constant.imgSizeMap;
            scale = Matrix.CreateScale(
                            (GraphicsDevice.Viewport.Width * 85 / 100) / VirtualWidth,
                            GraphicsDevice.Viewport.Height / VirtualHeight,
                            1f);

            State.ActualState = StateType.Default;
        }
        public void ChangeTheme(ThemeMap newTheme)
        {
            _map.ThemeOfMap = newTheme;
            string nameTheme = Enum.GetName(typeof(ThemeMap), newTheme);
            _imgMaps = _imgThemesMaps[nameTheme];
            _imgDecors = _imgThemesDecorsMaps[nameTheme];
        }
    }
}