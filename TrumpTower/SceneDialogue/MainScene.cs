using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.SceneDialogue
{
    class MainScene
    {
        Game1 Ctx { get; set; }
        public float WHeight { get; set; }
        public float WWidth { get; set; }
        bool FirstApparition { get; set; }

        public Texture2D _imgEntityLeft;
        Vector2 _positionEntityLeft;
        public Texture2D _imgEntityRight;
        Vector2 _positionEntityRight;

        List<Talk> AllDialogue { get; set; }
        int CurrentDialogue { get; set; }
        bool CurrentSceneIsFinish { get; set; }

        float Transparancy { get; set; }

        public MainScene (Game1 ctx, float wHeight, float wWidth, Texture2D imgEntityLeft, Texture2D imgEntityRight = null)
        {
            Ctx = ctx;
            WHeight = wHeight;
            WWidth = wWidth;
            FirstApparition = true;
            _imgEntityLeft = imgEntityLeft;
            _positionEntityLeft = new Vector2(0, WHeight);
            _imgEntityRight = imgEntityRight;
            _positionEntityRight = new Vector2(WWidth - _imgEntityRight.Width, WHeight);
            AllDialogue = new List<Talk>();
            CurrentDialogue = 0;
            CurrentSceneIsFinish = false;
            Transparancy = 1;
        }

        public void HandleInput(MouseState newStateMouse, MouseState lastStateMouse, KeyboardState newStateKeyboard, KeyboardState lastStateKeyboard)
        {
            if (newStateKeyboard.IsKeyDown(Keys.Enter) && lastStateKeyboard.IsKeyUp(Keys.Enter) && CurrentSceneIsFinish == false)
                CurrentSceneIsFinish = true;
            else if (newStateKeyboard.IsKeyDown(Keys.Enter) && lastStateKeyboard.IsKeyUp(Keys.Enter))
            {
                CurrentDialogue++;
                CurrentSceneIsFinish = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            // Apparition des deux entités
            if (FirstApparition)
            {
                if (_positionEntityLeft.Y > WHeight - _imgEntityLeft.Height)
                    _positionEntityLeft.Y -= 25f;
                if (_positionEntityRight.Y > WHeight - _imgEntityRight.Height)
                    _positionEntityRight.Y -= 25f;
                if (_positionEntityLeft.Y < WHeight - _imgEntityLeft.Height && _positionEntityRight.Y < WHeight - _imgEntityRight.Height)
                    FirstApparition = false;
            }
            // Après que les 2 entités soient apparus
            else
            {
            }
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            Talk talk = AllDialogue[CurrentDialogue];
            float TransparancyVariance = 0;
            if (Transparancy < 0.4) TransparancyVariance = 0.03f;
            else if (Transparancy > 1) TransparancyVariance = -0.03f;
            Transparancy += TransparancyVariance;

            if (talk.IsLeft)
                spriteBatch.Draw(_imgEntityLeft, _positionEntityLeft, Color.White * Transparancy);
            else
                spriteBatch.Draw(_imgEntityLeft, _positionEntityLeft, Color.White);

            if (_imgEntityRight != null)
            {
                if (talk.IsLeft)
                    spriteBatch.Draw(_imgEntityRight, _positionEntityRight, Color.White);
                else
                    spriteBatch.Draw(_imgEntityRight, _positionEntityRight, Color.White * Transparancy);
            }

            talk.Draw(spriteBatch);
        }

        public void AddTalk (Talk talk) => AllDialogue.Add(talk);
        public bool SceneIsFinish => CurrentDialogue == AllDialogue.Count - 1 && CurrentSceneIsFinish == true; 
    }
}
