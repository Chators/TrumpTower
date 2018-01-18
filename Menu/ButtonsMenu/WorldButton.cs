using LibraryTrumpTower.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Menu.Game1Menu;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Menu.ButtonsMenu
{
    class WorldButton : GroupOfButtons
    {
        string _nameWorld; // Name of World
        List<string> _nameMaps; // Name of Map in file for deserialization
        int _lvlWorld; // example World 1 = 1, World 2 = 2, World 3 = 3

        public WorldButton(Game1Menu ctx, int numberOfButtons, Dictionary<int, string> indexOfButtons, int buttonHeight, int buttonWidth, string nameWorld, List<string> nameMaps, int lvlWorld) : base(ctx, numberOfButtons, indexOfButtons, buttonHeight, buttonWidth)
        {
            _nameWorld = nameWorld;
            _nameMaps = nameMaps;
            _lvlWorld = lvlWorld;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int lvl = _ctx._player._lvlAccess - (_lvlWorld * 5) - 1;
            if (lvl < 5)
            {
                Texture2D _imgIndicate = _ctx._imgArrowRight;
                spriteBatch.Draw(_imgIndicate, new Vector2(_buttonRectangle[lvl].X - _imgIndicate.Width, _buttonRectangle[lvl].Y + 18), Color.White);
            }
            base.Draw(spriteBatch);
        }

        public override void take_action_on_button(int i)
        {
            if (IndexOfButtons[i] == "return")
            {
                _ctx.state = MenuState.CAMPAGNE;
            }
            // when you click on a map
            else
            {
                if (_ctx._player._lvlAccess > (_lvlWorld * 5) + i)
                {
                    string[] filesInDirectory = Directory.GetFileSystemEntries(BinarySerializer.pathCampagneMap + "/" + _nameWorld);
                    FileInfo file = new FileInfo(filesInDirectory[i]);
                    // On copie le fichier dans CurrentMap
                    file.CopyTo(BinarySerializer.pathCurrentMapXml, true);
                    Process.Start("TrumpTower");
                    _ctx.Exit();
                }
                else
                    Console.WriteLine("LOOOL TU PEUX PAS JOUUERRRR");
            }
        }
    }
}
