using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.NuclexGui;
using MonoGame.Extended.NuclexGui.Controls.Desktop;
using System;
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

        public Map _map;

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
            /*graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = true;*/
            graphics.ApplyChanges();

            _gui.Screen = new GuiScreen(800, 400);
            _gui.Screen.Desktop.Bounds = new UniRectangle(new UniScalar(0f, 0), new UniScalar(0f, 0), new UniScalar(1f, 0), new UniScalar(1f, 0));
            // Perform second-stage initialization
            _gui.Initialize();

            // POUR AJOUTER UN BOUTON
            // Create few controls.
            /*var button = new GuiButtonControl
            {
                Name = "button",
                Bounds = new UniRectangle(new UniScalar(0.0f, 20), new UniScalar(0.0f, 20), new UniScalar(0f, 120), new UniScalar(0f, 50)),
                Text = "Creer Map"
            };

            // Si on press le bouton
            button.Pressed += Button2_Pressed;


            _gui.Screen.Desktop.Children.Add(button);*/
            Button2_Pressed;
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
                    _mapPoint[y,x] = (int)MapTexture.grass;
                }
            }

            _map = new Map(_mapPoint);

            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
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

            base.Update(gameTime);
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


            base.Draw(gameTime);
        }
    }
}
