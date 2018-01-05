using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.ButtonsMenu
{
    enum BState
    {
        HOVER,
        UP,
        JUST_RELEASED,
        DOWN
    }

    abstract public class GroupOfButtons
    {
        internal Game1Menu _ctx;

        private int NumberOfButtons { get; set; } // The number of buttons
        internal Dictionary<int, string> IndexOfButtons { get; set; } // The position of the different buttons in order with the exact path IN CONTENT
        private int ButtonHeight { get; set; } // Height of ONE button
        private int ButtonWidth { get; set; } // Width of ONE button in the middle of the screen

        private Color[] _buttonColor;
        private Rectangle[] _buttonRectangle;
        private BState[] _buttonState;
        private Texture2D[] _buttonTexture;
        private double[] _buttonTimer;

        public GroupOfButtons(Game1Menu ctx, int numberOfButtons, Dictionary<int, string> indexOfButtons, int buttonHeight, int buttonWidth)
        {
            _ctx = ctx;

            NumberOfButtons = numberOfButtons;
            IndexOfButtons = indexOfButtons;
            ButtonHeight = buttonHeight;
            ButtonWidth = buttonWidth;

            _buttonColor = new Color[NumberOfButtons];
            _buttonRectangle = new Rectangle[NumberOfButtons];
            _buttonState = new BState[NumberOfButtons];
            _buttonTexture = new Texture2D[NumberOfButtons];
            _buttonTimer = new double[NumberOfButtons];

            // starting x and y locations to stack buttons
            int x = _ctx.Window.ClientBounds.Width / 2 - ButtonWidth / 2;
            int y = _ctx.Window.ClientBounds.Height / 2 -
                NumberOfButtons / 2 * ButtonHeight -
                (NumberOfButtons % 2) * ButtonHeight / 2;

            for (int i = 0; i < NumberOfButtons; i++)
            {
                _buttonState[i] = BState.UP;
                _buttonColor[i] = Color.White;
                _buttonTimer[i] = 0.0;
                _buttonRectangle[i] = new Rectangle(x, y, ButtonWidth, ButtonHeight);
                y += ButtonHeight;
            }
        }

        public void LoadContent()
        {
            foreach (int key in IndexOfButtons.Keys)
            {
                string value = IndexOfButtons[key];
                _buttonTexture[key] = _ctx.Content.Load<Texture2D>(value); 
            }
        }

        public void Update(double frame_time, int mx, int my, bool prev_mpressed, bool mpressed)
        {
            update_buttons(frame_time, mx, my, prev_mpressed, mpressed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < NumberOfButtons; i++)
                spriteBatch.Draw(_buttonTexture[i], _buttonRectangle[i], _buttonColor[i]);
        }

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
        void update_buttons(double frame_time, int mx, int my, bool prev_mpressed, bool mpressed)
        {
            for (int i = 0; i < NumberOfButtons; i++)
            {

                if (hit_image_alpha(
                    _buttonRectangle[i], _buttonTexture[i], mx, my))
                {
                    _buttonTimer[i] = 0.0;
                    if (mpressed)
                    {
                        // mouse is currently down
                        _buttonState[i] = BState.DOWN;
                        _buttonColor[i] = Color.Blue;
                    }
                    else if (!mpressed && prev_mpressed)
                    {
                        // mouse was just released
                        if (_buttonState[i] == BState.DOWN)
                        {
                            // button i was just down
                            _buttonState[i] = BState.JUST_RELEASED;
                        }
                    }
                    else
                    {
                        _buttonState[i] = BState.HOVER;
                        _buttonColor[i] = Color.LightBlue;
                    }
                }
                else
                {
                    _buttonState[i] = BState.UP;
                    if (_buttonTimer[i] > 0)
                    {
                        _buttonTimer[i] = _buttonTimer[i] - frame_time;
                    }
                    else
                    {
                        _buttonColor[i] = Color.White;
                    }
                }

                if (_buttonState[i] == BState.JUST_RELEASED)
                {
                    take_action_on_button(i);
                }
            }

        }

        // Logic for each button click goes here
        abstract public void take_action_on_button(int i);
    }
}