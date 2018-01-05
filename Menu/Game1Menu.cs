﻿using LibraryTrumpTower.Constants;
using Menu.ButtonsMenu;
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
        public enum MenuState
        {
            MAIN,
            CAMPAGNE,
            WORLD1,
            WORLD2,
            WORLD3,
            NONE
        }

        #region FIELDS
        MainButtons _mainButtons;
        CampagneButtons _campagneButtons;
        WorldButton _world1Button;
        WorldButton _world2Button;
        WorldButton _world3Button;

        //mouse pressed and mouse just pressed
        bool mpressed, prev_mpressed = false;
        //mouse location in window
        int mx, my;
        double frame_time;

        public MenuState state;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private readonly InputListenerComponent _inputManager;
        private readonly GuiManager _gui;
        #endregion

        public Game1Menu()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            state = MenuState.MAIN;

            // First, we create an input manager.
            _inputManager = new InputListenerComponent(this);
            // Then, we create GUI.
            var guiInputService = new GuiInputService(_inputManager);
            _gui = new GuiManager(Services, guiInputService);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before 
        /// ing to run.
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

            #region BUTTON MAIN
            int numberOfButtons = 5;
            Dictionary<int, string> indexOfButtons = new Dictionary<int, string>();
            indexOfButtons[0] = "mode_campagne";
            indexOfButtons[1] = "mode_custom";
            indexOfButtons[2] = "editeur_de_map";
            indexOfButtons[3] = "options";
            indexOfButtons[4] = "quitter";
            int buttonHeight = 100;
            int buttonWidth = 300;
            _mainButtons = new MainButtons(this, numberOfButtons, indexOfButtons, buttonHeight, buttonWidth);
            #endregion

            #region BUTTON CAMPAGNE
            numberOfButtons = 4;
            indexOfButtons = new Dictionary<int, string>();
            indexOfButtons[0] = "World1/World-1";
            indexOfButtons[1] = "World2/World-2";
            indexOfButtons[2] = "World3/World-3";
            indexOfButtons[3] = "return";
            buttonHeight = 100;
            buttonWidth = 300;
            _campagneButtons = new CampagneButtons(this, numberOfButtons, indexOfButtons, buttonHeight, buttonWidth);
            #endregion

            #region BUTTON WORLD
            #region BUTTON WORLD 1
            numberOfButtons = 6;
            indexOfButtons = new Dictionary<int, string>();
            indexOfButtons[0] = "World1/Map-1";
            indexOfButtons[1] = "World1/Map-2";
            indexOfButtons[2] = "World1/Map-3";
            indexOfButtons[3] = "World1/Map-4";
            indexOfButtons[4] = "World1/Map-5";
            indexOfButtons[5] = "return";
            List<string>nameMaps = new List<string>();
            nameMaps.Add("Map-1");
            nameMaps.Add("Map-2");
            nameMaps.Add("Map-3");
            nameMaps.Add("Map-4");
            nameMaps.Add("Map-5");
            buttonHeight = 100;
            buttonWidth = 300;
            _world1Button = new WorldButton(this, numberOfButtons, indexOfButtons, buttonHeight, buttonWidth, "World1", nameMaps);
            #endregion

            #region BUTTON WORLD 2
            numberOfButtons = 6;
            indexOfButtons = new Dictionary<int, string>();
            indexOfButtons[0] = "World2/Map-1";
            indexOfButtons[1] = "World2/Map-2";
            indexOfButtons[2] = "World2/Map-3";
            indexOfButtons[3] = "World2/Map-4";
            indexOfButtons[4] = "World2/Map-5";
            indexOfButtons[5] = "return";
            nameMaps = new List<string>();
            nameMaps.Add("Map-1");
            nameMaps.Add("Map-2");
            nameMaps.Add("Map-3");
            nameMaps.Add("Map-4");
            nameMaps.Add("Map-5");
            buttonHeight = 100;
            buttonWidth = 300;
            _world2Button = new WorldButton(this, numberOfButtons, indexOfButtons, buttonHeight, buttonWidth, "World2", nameMaps);
            #endregion

            #region BUTTON WORLD 3
            numberOfButtons = 6;
            indexOfButtons = new Dictionary<int, string>();
            indexOfButtons[0] = "World3/Map-1";
            indexOfButtons[1] = "World3/Map-2";
            indexOfButtons[2] = "World3/Map-3";
            indexOfButtons[3] = "World3/Map-4";
            indexOfButtons[4] = "World3/Map-5";
            indexOfButtons[5] = "return";
            nameMaps = new List<string>();
            nameMaps.Add("Map-1");
            nameMaps.Add("Map-2");
            nameMaps.Add("Map-3");
            nameMaps.Add("Map-4");
            nameMaps.Add("Map-5");
            buttonHeight = 100;
            buttonWidth = 300;
            _world3Button = new WorldButton(this, numberOfButtons, indexOfButtons, buttonHeight, buttonWidth, "World3", nameMaps);
            #endregion
            #endregion

            IsMouseVisible = true;
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
            _mainButtons.LoadContent();
            _campagneButtons.LoadContent();
            _world1Button.LoadContent();
            _world2Button.LoadContent();
            _world3Button.LoadContent();
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
            try { _inputManager.Update(gameTime); }
            catch { }
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
            {
                if (state == MenuState.MAIN)
                    _mainButtons.Update(frame_time, mx, my, prev_mpressed, mpressed);
                else if (state == MenuState.CAMPAGNE)
                    _campagneButtons.Update(frame_time, mx, my, prev_mpressed, mpressed);
                else if (state == MenuState.WORLD1)
                    _world1Button.Update(frame_time, mx, my, prev_mpressed, mpressed);
                else if (state == MenuState.WORLD2)
                    _world2Button.Update(frame_time, mx, my, prev_mpressed, mpressed);
                else if (state == MenuState.WORLD3)
                    _world3Button.Update(frame_time, mx, my, prev_mpressed, mpressed);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (state == MenuState.MAIN)
                _mainButtons.Draw(spriteBatch);
            else if (state == MenuState.CAMPAGNE)
                _campagneButtons.Draw(spriteBatch);
            else if (state == MenuState.WORLD1)
                _world1Button.Draw(spriteBatch);
            else if (state == MenuState.WORLD2)
                _world2Button.Draw(spriteBatch);
            else if (state == MenuState.WORLD3)
                _world3Button.Draw(spriteBatch);

            spriteBatch.End();

            _gui.Draw(gameTime);

            base.Draw(gameTime);
        }

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