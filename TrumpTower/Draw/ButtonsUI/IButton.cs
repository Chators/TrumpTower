using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrumpTower.Draw.ButtonsUI
{
    interface IButton
    {
        /*
         newStateMouse.X > button.Position.X && newStateMouse.X<button.Position.X + button.Texture.Width &&
                newStateMouse.Y> button.Position.Y && newStateMouse.Y<button.Position.Y + button.Texture.Height
                */
        bool HasBeenClicked(MouseState newStateMouse, Vector2 buttonPosition, int slt);
    }
}
