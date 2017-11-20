using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Menu.Controls;
using Microsoft.Xna.Framework.Media;
using TrumpTower;

namespace Menu.States
{
    public class MenuState : State
    {
        private List<Component> _components;
        Texture2D _title;
        Texture2D _trump;
        Texture2D _kim;

        Texture2D _dirtUp;
        Texture2D _dirtDown;
        Texture2D _dirt;
        Vector2 _dirtUpPosition;
        Vector2 _dirtDownPosition;
        Vector2 _dirtPosition;

        Vector2 _trumpPosition;
        Vector2 _titlePosition;
        Vector2 _kimPosition;



        public MenuState(Game2 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            _title = content.Load<Texture2D>("Title");
            _titlePosition = new Vector2(0, 0);
            _trump = content.Load<Texture2D>("Trump");
            _trumpPosition = new Vector2(600, 210);
            _kim = content.Load<Texture2D>("Kim");
            _kimPosition = new Vector2(0, 210);
            //IsLaunch = false;


            _dirtUp = content.Load<Texture2D>("dirtUpGrassDown");
            _dirtDown = content.Load<Texture2D>("dirtDownGrassUp");
            _dirt = content.Load<Texture2D>("dirt");

            _dirtUpPosition = new Vector2(0, 184);
            _dirtDownPosition = new Vector2(0, 56);
            _dirtPosition = new Vector2(0, 120);

            var buttonTexture = content.Load<Texture2D>("Controls/Button");
            var buttonFont = content.Load<SpriteFont>("Fonts/Font");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(260, 100),
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            var loadGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(260, 200),
                Text = "Load Game",
            };

            loadGameButton.Click += LoadGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(260, 300),
                Text = "Quit",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Component>()
            {
                newGameButton,
                loadGameButton,
                quitGameButton,
            };



        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
            Console.WriteLine("Quit");
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("New Game");
            
            using (var game = new Game1()) game.Run();
            //_game.Exit();

        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_title, _titlePosition, null, Color.White);
            spriteBatch.Draw(_trump, _trumpPosition, null, Color.White);
            spriteBatch.Draw(_kim, _kimPosition, null, Color.White);


            /*  spriteBatch.Draw(_dirtUp, _dirtUpPosition, null, Color.White);
              spriteBatch.Draw(_dirtDown, _dirtDownPosition, null, Color.White);
              spriteBatch.Draw(_dirt, _dirtPosition, null, Color.White);*/

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //remove sprites not if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
