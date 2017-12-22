using LibraryTrumpTower.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.NuclexGui;
using MonoGame.Extended.NuclexGui.Controls;
using MonoGame.Extended.NuclexGui.Controls.Desktop;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using TrumpTower.LibraryTrumpTower;
using static System.Net.Mime.MediaTypeNames;

namespace Menu
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1Menu : Game
    {
        // Global variables
        enum BState
        {
            HOVER,
            UP,
            JUST_RELEASED,
            DOWN
        }
        const int NUMBER_OF_BUTTONS = 5,
            MODE_CAMPAGNE_BUTTON_INDEX = 0,
            MODE_CUSTOM_BUTTON_INDEX = 1,
            EDITEUR_MAP_BUTTON_INDEX = 2,
            OPTIONS_BUTTON_INDEX = 3,
            QUIT_BUTTON_INDEX = 4,
            BUTTON_HEIGHT = 100,
            BUTTON_WIDTH = 300;
        Color background_color;
        Color[] button_color = new Color[NUMBER_OF_BUTTONS];
        Rectangle[] button_rectangle = new Rectangle[NUMBER_OF_BUTTONS];
        BState[] button_state = new BState[NUMBER_OF_BUTTONS];
        Texture2D[] button_texture = new Texture2D[NUMBER_OF_BUTTONS];
        double[] button_timer = new double[NUMBER_OF_BUTTONS];
        //mouse pressed and mouse just pressed
        bool mpressed, prev_mpressed = false;
        //mouse location in window
        int mx, my;
        double frame_time;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private readonly InputListenerComponent _inputManager;
        private readonly GuiManager _gui;

        public Game1Menu()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

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
            Directory.CreateDirectory(BinarySerializer.pathCustomMap);
            Directory.CreateDirectory(BinarySerializer.pathCampagneMap);

            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            // starting x and y locations to stack buttons 
            // vertically in the middle of the screen
            int x = Window.ClientBounds.Width / 2 - BUTTON_WIDTH / 2;
            int y = Window.ClientBounds.Height / 2 -
                NUMBER_OF_BUTTONS / 2 * BUTTON_HEIGHT -
                (NUMBER_OF_BUTTONS % 2) * BUTTON_HEIGHT / 2;
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {
                button_state[i] = BState.UP;
                button_color[i] = Color.White;
                button_timer[i] = 0.0;
                button_rectangle[i] = new Rectangle(x, y, BUTTON_WIDTH, BUTTON_HEIGHT);
                y += BUTTON_HEIGHT;
            }
            IsMouseVisible = true;
            background_color = Color.Black;

            _gui.Screen = new GuiScreen(graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height);

            _gui.Screen.Desktop.Bounds = new UniRectangle(new UniScalar(0f, 0), new UniScalar(0f, 0), new UniScalar(1f, 0), new UniScalar(1f, 0));
            // Perform second-stage initialization
            _gui.Initialize();

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
            button_texture[MODE_CAMPAGNE_BUTTON_INDEX] =
                Content.Load<Texture2D>("mode_campagne");
            button_texture[MODE_CUSTOM_BUTTON_INDEX] =
                Content.Load<Texture2D>("mode_custom");
            button_texture[EDITEUR_MAP_BUTTON_INDEX] =
                Content.Load<Texture2D>("editeur_de_map");
            button_texture[OPTIONS_BUTTON_INDEX] =
                Content.Load<Texture2D>("options");
            button_texture[QUIT_BUTTON_INDEX] =
                Content.Load<Texture2D>("quitter");
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
            /*try { _inputManager.Update(gameTime); }
            catch { }*/
            _inputManager.Update(gameTime);

            // get elapsed frame time in seconds
            frame_time = gameTime.ElapsedGameTime.Milliseconds / 1000.0;

            // update mouse variables
            MouseState mouse_state = Mouse.GetState();
            mx = mouse_state.X;
            my = mouse_state.Y;
            prev_mpressed = mpressed;
            mpressed = mouse_state.LeftButton == ButtonState.Pressed;

            if (_gui.Screen.Desktop.Children.Count == 0)
            update_buttons();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            GraphicsDevice.Clear(background_color);

            spriteBatch.Begin();
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
                spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);
            spriteBatch.End();

            _gui.Draw(gameTime);

            base.Draw(gameTime);
        }

        #region Class BUTTON
        // wrapper for hit_image_alpha taking Rectangle and Texture
        Boolean hit_image_alpha(Rectangle rect, Texture2D tex, int x, int y)
        {
            return hit_image_alpha(0, 0, tex, tex.Width * (x - rect.X) /
                rect.Width, tex.Height * (y - rect.Y) / rect.Height);
        }

        // wraps hit_image then determines if hit a transparent part of image 
        Boolean hit_image_alpha(float tx, float ty, Texture2D tex, int x, int y)
        {
            if (hit_image(tx, ty, tex, x, y))
            {
                uint[] data = new uint[tex.Width * tex.Height];
                tex.GetData<uint>(data);
                if ((x - (int)tx) + (y - (int)ty) *
                    tex.Width < tex.Width * tex.Height)
                {
                    return ((data[
                        (x - (int)tx) + (y - (int)ty) * tex.Width
                        ] &
                                0xFF000000) >> 24) > 20;
                }
            }
            return false;
        }

        // determine if x,y is within rectangle formed by texture located at tx,ty
        Boolean hit_image(float tx, float ty, Texture2D tex, int x, int y)
        {
            return (x >= tx &&
                x <= tx + tex.Width &&
                y >= ty &&
                y <= ty + tex.Height);
        }

        // determine state and color of button
        void update_buttons()
        {
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {

                if (hit_image_alpha(
                    button_rectangle[i], button_texture[i], mx, my))
                {
                    button_timer[i] = 0.0;
                    if (mpressed)
                    {
                        // mouse is currently down
                        button_state[i] = BState.DOWN;
                        button_color[i] = Color.Blue;
                    }
                    else if (!mpressed && prev_mpressed)
                    {
                        // mouse was just released
                        if (button_state[i] == BState.DOWN)
                        {
                            // button i was just down
                            button_state[i] = BState.JUST_RELEASED;
                        }
                    }
                    else
                    {
                        button_state[i] = BState.HOVER;
                        button_color[i] = Color.LightBlue;
                    }
                }
                else
                {
                    button_state[i] = BState.UP;
                    if (button_timer[i] > 0)
                    {
                        button_timer[i] = button_timer[i] - frame_time;
                    }
                    else
                    {
                        button_color[i] = Color.White;
                    }
                }

                if (button_state[i] == BState.JUST_RELEASED)
                {
                    take_action_on_button(i);
                }
            }
        }


        // Logic for each button click goes here
        void take_action_on_button(int i)
        {
            //take action corresponding to which button was clicked
            switch (i)
            {
                case MODE_CAMPAGNE_BUTTON_INDEX:
                    Exit();
                    break;
                case MODE_CUSTOM_BUTTON_INDEX:
                    MapPlay_Pressed();
                    break;
                case EDITEUR_MAP_BUTTON_INDEX:
                    MapEditor_Pressed();
                    break;
                case OPTIONS_BUTTON_INDEX:
                    break;
                case QUIT_BUTTON_INDEX:
                    Exit();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region WINDOW

        #region Window MapPlay
        public void MapPlay_Pressed()
        {
            var Window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(400), new UniScalar(300))),
                Title = "Vos maps",
                EnableDragging = true
            };

            var ListMap = new GuiListControl()
            {
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(1.0f, -20), new UniScalar(0f, 150)),
            };

            var DeleteMapButton = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -100), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Suppr map"
            };

            var ConfirmMapButton = new GuiButtonControl
            {
                Name = "confirm",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -100), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Jouer"
            };

            var BackMapButton = new GuiButtonControl
            {
                Name = "back",
                Bounds = new UniRectangle(new UniScalar(0.0f, 155), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Retour"
            };

            DeleteMapButton.Pressed += DeleteMap_Pressed;
            ConfirmMapButton.Pressed += ConfirmChoiceMap_Pressed;
            BackMapButton.Pressed += CancelWindowMap_Pressed;

            Window.Children.Add(ListMap);
            string[] filesInDirectory = Directory.GetFileSystemEntries(BinarySerializer.pathCustomMap);
            for (int i = 0; i < filesInDirectory.Length; i++)
                ListMap.Items.Add(filesInDirectory[i].Split('\\')[1].Split('.')[0]);
            ListMap.SelectionMode = ListSelectionMode.Single;

            Window.Children.Add(DeleteMapButton);
            Window.Children.Add(ConfirmMapButton);
            Window.Children.Add(BackMapButton);

            _gui.Screen.Desktop.Children.Add(Window);

        }

        private void DeleteMap_Pressed(object sender, System.EventArgs e)
        {
            int? _nbMap = null;

            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiListControl))
                {
                    GuiListControl input = (GuiListControl)control;
                    if (input.SelectedItems.Count > 0)
                        _nbMap = input.SelectedItems[0];
                }
            }

            if (_nbMap != null)
            {
                string[] filesInDirectory = Directory.GetFileSystemEntries(BinarySerializer.pathCustomMap);
                FileInfo file = new FileInfo(filesInDirectory[(int)_nbMap]);
                file.Delete();

                _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
                MapPlay_Pressed();
            }
        }

        private void ConfirmChoiceMap_Pressed(object sender, System.EventArgs e)
        {
            int? _nbMap = null;

            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiListControl))
                {
                    GuiListControl input = (GuiListControl)control;
                    if (input.SelectedItems.Count > 0)
                        _nbMap = input.SelectedItems[0];
                }
            }

            if (_nbMap != null)
            {
                string[] filesInDirectory = Directory.GetFileSystemEntries(BinarySerializer.pathCustomMap);
                FileInfo file = new FileInfo(filesInDirectory[(int)_nbMap]);
                // On copie le fichier dans CurrentMap
                file.CopyTo(BinarySerializer.pathCurrentMapXml, true);

                _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
                Process.Start("TrumpTower");
                Exit();
            }
        }

        private void CancelWindowMap_Pressed(object sender, System.EventArgs e)
        {
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
        }
        #endregion

        #region Window MapEditor
        public void MapEditor_Pressed()
        {
            var Window = new GuiWindowControl
            {
                Name = "window",
                Bounds = new UniRectangle(new UniVector(new UniScalar(0.5f, -100), new UniScalar(0.5f, -60)), new UniVector(new UniScalar(400), new UniScalar(300))),
                Title = "Editeur de map",
                EnableDragging = true
            };

            var ListMapEditor = new GuiListControl()
            {
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(0.0f, 30), new UniScalar(1.0f, -20), new UniScalar(0f, 150)),
            };

            var DeleteMapEditorButton = new GuiButtonControl
            {
                Name = "cancel",
                Bounds = new UniRectangle(new UniScalar(1.0f, -100), new UniScalar(1.0f, -100), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Supprimer"
            };

            var CreateNewMapEditorButton = new GuiButtonControl
            {
                Name = "createNewMap",
                Bounds = new UniRectangle(new UniScalar(0.0f, 10), new UniScalar(1.0f, -100), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Nouvelle"
            };

            var ModifyMapEditorButton = new GuiButtonControl
            {
                Name = "modifyMapEditor",
                Bounds = new UniRectangle(new UniScalar(0.0f, 155), new UniScalar(1.0f, -100), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Modifier"
            };

            var BackMapEditorButton = new GuiButtonControl
            {
                Name = "back",
                Bounds = new UniRectangle(new UniScalar(0.0f, 155), new UniScalar(1.0f, -40), new UniScalar(0f, 90), new UniScalar(0f, 30)),
                Text = "Retour"
            };

            DeleteMapEditorButton.Pressed += DeleteMapEditor_Pressed;
            CreateNewMapEditorButton.Pressed += CreateNewMapEditor_Pressed;
            ModifyMapEditorButton.Pressed += ModifyMapEditor_Pressed;
            BackMapEditorButton.Pressed += CancelWindowMapEditor_Pressed;

            Window.Children.Add(ListMapEditor);
            string[] filesInDirectory = Directory.GetFileSystemEntries(BinarySerializer.pathCustomMap);
            for (int i = 0; i < filesInDirectory.Length; i++)
                ListMapEditor.Items.Add(filesInDirectory[i].Split('\\')[1].Split('.')[0]);
            ListMapEditor.SelectionMode = ListSelectionMode.Single;

            Window.Children.Add(DeleteMapEditorButton);
            Window.Children.Add(CreateNewMapEditorButton);
            Window.Children.Add(ModifyMapEditorButton);
            Window.Children.Add(BackMapEditorButton);

            _gui.Screen.Desktop.Children.Add(Window);

        }

        private void DeleteMapEditor_Pressed(object sender, System.EventArgs e)
        {
            int? _nbMap = null;

            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiListControl))
                {
                    GuiListControl input = (GuiListControl)control;
                    if (input.SelectedItems.Count > 0)
                        _nbMap = input.SelectedItems[0];
                }
            }

            if (_nbMap != null)
            {
                string[] filesInDirectory = Directory.GetFileSystemEntries(BinarySerializer.pathCustomMap);
                FileInfo file = new FileInfo(filesInDirectory[(int)_nbMap]);
                file.Delete();

                _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
                MapEditor_Pressed();
            }
        }

        private void CreateNewMapEditor_Pressed(object sender, System.EventArgs e)
        {
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
            FileInfo file = new FileInfo(BinarySerializer.pathCurrentMapXml);
            file.Delete();
            Process.Start("MapEditorTrumpTower");
            Exit();
        }

        private void ModifyMapEditor_Pressed(object sender, System.EventArgs e)
        {
            int? _nbMap = null;

            foreach (var control in ((GuiButtonControl)sender).Parent.Children)
            {
                if (control.GetType() == typeof(GuiListControl))
                {
                    GuiListControl input = (GuiListControl)control;
                    if (input.SelectedItems.Count > 0)
                        _nbMap = input.SelectedItems[0];
                }
            }

            if (_nbMap != null)
            {
                string[] filesInDirectory = Directory.GetFileSystemEntries(BinarySerializer.pathCustomMap);
                FileInfo file = new FileInfo(filesInDirectory[(int)_nbMap]);
                // On copie le fichier dans CurrentMap
                file.CopyTo(BinarySerializer.pathCurrentMapXml, true);
                Process.Start("MapEditorTrumpTower");
                Exit();
            }
        }

        private void CancelWindowMapEditor_Pressed(object sender, System.EventArgs e)
        {
            _gui.Screen.Desktop.Children.Remove(((GuiButtonControl)sender).Parent);
        }
        #endregion 

        #endregion
    }
}
