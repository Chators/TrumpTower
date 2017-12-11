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

        private Texture2D _imgWall;
        private Texture2D _imgCursor;
        private Texture2D _imgNoSelect;
        private Texture2D _imgCloakTexture;
        private Texture2D _imgNextWaveIsComming;

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

            _imgWall = Content.Load<Texture2D>("Wall");
            _imgNoSelect = Content.Load <Texture2D>("select");
            _imgCloakTexture = Content.Load<Texture2D>("cloakImgMap");
            _debug = Content.Load<SpriteFont>("DefaultFont");
            _imgNextWaveIsComming = Content.Load<Texture2D>("north_korea_is_comming");

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
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirt], _debug, MapTexture.dirt, new Vector2(posMenuRight + 10, 100), "&",
                new List<Keys>(new Keys[] { Keys.D1 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.grass], _debug, MapTexture.grass, new Vector2(posMenuRight + 80, 100), "é",
                new List<Keys>(new Keys[] { Keys.D2 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.emptyTower], _debug, MapTexture.emptyTower, new Vector2(posMenuRight + 150, 100), "\"",
                new List<Keys>(new Keys[] { Keys.D3 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgWall, _debug, MapTexture.myBase, new Vector2(posMenuRight + 220, 100), "'",
                new List<Keys>(new Keys[] { Keys.D4 })));

            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtDownGrassUp], _debug, MapTexture.dirtDownGrassUp, new Vector2(posMenuRight + 10, 270), "CTRL+&",
                new List<Keys>(new Keys[] { Keys.LeftControl, Keys.D1 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtRightGrassLeft], _debug, MapTexture.dirtRightGrassLeft, new Vector2(posMenuRight + 80, 270), "CTRL+\"",
                new List<Keys>(new Keys[] { Keys.LeftControl, Keys.D2 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtUpGrassDown], _debug, MapTexture.dirtUpGrassDown, new Vector2(posMenuRight + 150, 270), "CTRL+'",
                new List<Keys>(new Keys[] { Keys.LeftControl, Keys.D3 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtLeftGrassRight], _debug, MapTexture.dirtLeftGrassRight, new Vector2(posMenuRight + 220, 270), "CTRL+é",
                new List<Keys>(new Keys[] { Keys.LeftControl, Keys.D4 })));

            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.bigDirtCornerLeftDown], _debug, MapTexture.bigDirtCornerLeftDown, new Vector2(posMenuRight + 10, 380), "ALT+&",
                new List<Keys>(new Keys[] { Keys.LeftAlt, Keys.D1 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.bigDirtCornerRightDown], _debug, MapTexture.bigDirtCornerRightDown, new Vector2(posMenuRight + 80, 380), "ALT+é",
                new List<Keys>(new Keys[] { Keys.LeftAlt, Keys.D2 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.bigDirtCornerRightUp], _debug, MapTexture.bigDirtCornerRightUp, new Vector2(posMenuRight + 150, 380), "ALT+\"",
                new List<Keys>(new Keys[] { Keys.LeftAlt, Keys.D3 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.bigDirtCornerLeftUp], _debug, MapTexture.bigDirtCornerLeftUp, new Vector2(posMenuRight + 220, 380), "ALT+'",
                new List<Keys>(new Keys[] { Keys.LeftAlt, Keys.D4 })));

            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtCornerLeftDown], _debug, MapTexture.dirtCornerLeftDown, new Vector2(posMenuRight + 10, 490), "SHIFT+&",
                new List<Keys>(new Keys[] { Keys.LeftShift, Keys.D1 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtCornerRightDown], _debug, MapTexture.dirtCornerRightDown, new Vector2(posMenuRight + 80, 490), "SHIFT+é",
                new List<Keys>(new Keys[] { Keys.LeftShift, Keys.D2 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtCornerRightUp], _debug, MapTexture.dirtCornerRightUp, new Vector2(posMenuRight + 150, 490), "SHIFT+\"",
                new List<Keys>(new Keys[] { Keys.LeftShift, Keys.D3 })));
            _buttonsTexture.Add(new ButtonTexture(this, _imgMaps[(int)MapTexture.dirtCornerLeftUp], _debug, MapTexture.dirtCornerLeftUp, new Vector2(posMenuRight + 220, 490), "SHIFT+'",
                new List<Keys>(new Keys[] { Keys.LeftShift, Keys.D4 })));

            _buttonsTexture.Add(new ButtonTexture(this, _imgNoSelect, _debug, MapTexture.None, new Vector2(posMenuRight + 115, 660), "A",
                new List<Keys>(new Keys[] { Keys.A })));
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
                    Console.WriteLine(_spawn.Waves.Count);
                    break;
                }
            }
            // Créer un chemin
            // Supprimer une vague
            // Voir les vagues
            // Retour
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

            /*var labelCreateSpawn = new GuiButtonControl()
            {
                Text = "Voulez-vous creer un spawn ?",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25))
            };*/

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
                wavesList.Items.Add("Vague " + (i+1) + " partira a " + wave.TimerBeforeStarting + " secondes" );
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
                    _spawn.CreateWave(new Wave(_spawn, new List<Enemy>(), 0));
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
            /*int _posX = 0;
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
            Spawn_Pressed(new Vector2(_posX, _posY));*/
        }
        #endregion

        public List<Texture2D> ImgMaps => _imgMaps;
        public Texture2D ImgWall => _imgWall;
    }
}

