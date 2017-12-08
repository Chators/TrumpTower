using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.NuclexGui;
using MonoGame.Extended.NuclexGui.Controls.Desktop;
using System;
using System.Collections.Generic;
using TrumpTower.LibraryTrumpTower;
using TrumpTower.LibraryTrumpTower.Constants;

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

        private SelectorTexture _selectTexture;

        private SpriteFont _debug;

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
            Window.Title = "TT Map Editor";
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            _gui.Screen = new GuiScreen(800, 400);
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

            base.Initialize();
        }

        #region ButtonMapSize
        private void Button2_Pressed(object sender, System.EventArgs e)
        {

            var window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(200), new UniScalar(130))),
                Title = "Taille de la map",
                EnableDragging = true
            };
            var choiceX = new GuiInputControl
            {
                Name = "choiceX",
                GuideTitle = "Veuillez choisir la largeur de la map : ",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(100), new UniScalar(25)),
                Text = "20"
            };
            var choiceY = new GuiInputControl
            {
                Name = "choiceY",
                GuideTitle = "Veuillez choisir la longueur de la map : ",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 60), new UniScalar(100), new UniScalar(25)),
                Text = "20"
            };
            var button1 = new GuiButtonControl
            {
                Name = "confirm",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -40), new UniScalar(0f, 80), new UniScalar(0f, 30)),
                Text = "Confirmer"
            };
            var button2 = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -90), new UniScalar(1.0f, -40), new UniScalar(0f, 80), new UniScalar(0f, 30)),
                Text = "Retour"
            };

            button1.Pressed += DialogueConfirm_Pressed;
            button2.Pressed += DialogueCancel_Pressed;

            window.Children.Add(choiceX);
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

            _selectTexture = new SelectorTexture(this, _map);

            VirtualWidth = _map.WidthArrayMap * Constant.imgSizeMap;
            VirtualHeight = _map.HeightArrayMap * Constant.imgSizeMap;
            scale = Matrix.CreateScale(
                            GraphicsDevice.Viewport.Width / VirtualWidth,
                            GraphicsDevice.Viewport.Height / VirtualHeight,
                            1f);
            /*scale = Matrix.CreateScale(
                            (GraphicsDevice.Viewport.Width * 85 / 100) / VirtualWidth,
                            GraphicsDevice.Viewport.Height / VirtualHeight,
                            1f);*/

            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            GuiButtonControl deleteButton = null;
            foreach (GuiButtonControl button in _gui.Screen.Desktop.Children)
            {
                if (button.Name == "button") deleteButton = button;
            }
            _gui.Screen.Desktop.Children.Remove(deleteButton);
        }
        #endregion

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

            _debug = Content.Load<SpriteFont>("DefaultFont");
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
            _inputManager.Update(gameTime);
            _gui.Update(gameTime);

            if (_map != null)
            {
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
            }

            base.Update(gameTime);
        }

        protected void HandleInput(MouseState newStateMouse, MouseState lastStateMouse, KeyboardState newStateKeyboard, KeyboardState lastStateKeyboard)
        {
            _selectTexture.HandleInput(newStateMouse, lastStateMouse, newStateKeyboard, lastStateKeyboard);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _gui.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, scale);

            #region Draw Map
            if (_map != null)
            {
                for (int y = 0; y < _map.HeightArrayMap; y++)
                {
                    for (int x = 0; x < _map.WidthArrayMap; x++)
                    {
                        spriteBatch.Draw(_imgMaps[_map.GetTypeArray(x, y)], new Vector2(x * Constant.imgSizeMap, y * Constant.imgSizeMap), null, Color.White);
                    }
                }


                #region Draw Debug
                spriteBatch.DrawString(_debug, _selectTexture.Texture + "", new Vector2(150, 150), Color.Red);
                #endregion

                #region Draw SelectorTexture
                _selectTexture.Draw(spriteBatch);
                #endregion
            }
            #endregion


            spriteBatch.End();

            base.Draw(gameTime);
        }

        public List<Texture2D> ImgMaps => _imgMaps;
    }
}
