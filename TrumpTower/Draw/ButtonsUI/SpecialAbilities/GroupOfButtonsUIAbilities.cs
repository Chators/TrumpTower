using LibraryTrumpTower.SpecialAbilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrumpTower.Draw.Animations;
using TrumpTower.LibraryTrumpTower.Constants;

namespace TrumpTower.Draw.ButtonsUI.SpecialAbilities
{
    class GroupOfButtonsUIAbilities
    {
        public Game1 Ctx { get; private set; }
        public SpriteFont CooldownSprite { get; private set; }
        public Explosion Explosion { get; private set; }
        public Dictionary<string, ButtonUIAbility> ButtonsUIArray { get; private set; }
        public ButtonUIAbility ButtonHover { get; set; }
        public ButtonUIAbility ButtonActivated { get; set; }

        public GroupOfButtonsUIAbilities(Game1 ctx, Explosion explosion, SpriteFont cooldownSprite)
        {
            Ctx = ctx;
            Explosion = explosion;
            CooldownSprite = cooldownSprite;
            ButtonsUIArray = new Dictionary<string, ButtonUIAbility>();
        }

        public void HandleInput(MouseState newStateMouse, MouseState lastStateMouse, KeyboardState newStateKeyboard, KeyboardState lastStateKeyboard)
        {
            ButtonHover = null;
            if (newStateMouse.RightButton == ButtonState.Pressed && lastStateMouse.RightButton == ButtonState.Released) ButtonActivated = null;

            // First Selection
            if (ButtonActivated == null)
            {
                ButtonUIAbility button = null;
                button = ButtonsUIArray["explosionAbility"];
                if (newStateMouse.X > button.Position.X && newStateMouse.X < button.Position.X + button.Texture.Width &&
                    newStateMouse.Y > button.Position.Y && newStateMouse.Y < button.Position.Y + button.Texture.Height ||
                    newStateKeyboard.IsKeyDown(Keys.A) &&
                    Ctx.Map.Explosion.IsReloaded)
                {
                    if (newStateMouse.LeftButton == ButtonState.Pressed && lastStateMouse.LeftButton == ButtonState.Released ||
                        newStateKeyboard.IsKeyDown(Keys.A) && lastStateKeyboard.IsKeyDown(Keys.A))
                    {
                        ManagerSound.PlayButtonExplosionAbility();
                        ButtonActivated = button;
                    }
                    ButtonHover = button;
                }

                button = ButtonsUIArray["sniperAbility"];
                if (newStateMouse.X > button.Position.X && newStateMouse.X < button.Position.X + button.Texture.Width &&
                    newStateMouse.Y > button.Position.Y && newStateMouse.Y < button.Position.Y + button.Texture.Height ||
                    newStateKeyboard.IsKeyDown(Keys.Z) &&
                    Ctx.Map.Dollars >= Ctx.Map.Sniper.Cost &&
                    Ctx.Map.Sniper.IsReload)
                {
                    if (newStateMouse.LeftButton == ButtonState.Pressed && lastStateMouse.LeftButton == ButtonState.Released ||
                        newStateKeyboard.IsKeyDown(Keys.Z) && lastStateKeyboard.IsKeyDown(Keys.Z))
                    {
                        ManagerSound.PlayReloadSniper();
                        ButtonActivated = button;
                    }
                    ButtonHover = button;
                }

                button = ButtonsUIArray["stickyRiceAbility"];
                if (newStateMouse.X > button.Position.X && newStateMouse.X < button.Position.X + button.Texture.Width &&
                    newStateMouse.Y > button.Position.Y && newStateMouse.Y < button.Position.Y + button.Texture.Height ||
                    newStateKeyboard.IsKeyDown(Keys.E) &&
                    Ctx.Map.StickyRice.IsReloaded)
                {
                    if (newStateMouse.LeftButton == ButtonState.Pressed && lastStateMouse.LeftButton == ButtonState.Released ||
                        newStateKeyboard.IsKeyDown(Keys.E) && lastStateKeyboard.IsKeyDown(Keys.E))
                    {
                        ManagerSound.PlayRice();
                        ButtonActivated = button;
                    }
                    ButtonHover = button;
                }
                if (Ctx.Map.Name == "mapCampagne5")
                {
                    button = ButtonsUIArray["wallBossAbility"];
                    if (Ctx.Map.WallBoss._isUsed == false)
                    {
                        if (newStateMouse.X > button.Position.X && newStateMouse.X < button.Position.X + button.Texture.Width &&
                            newStateMouse.Y > button.Position.Y && newStateMouse.Y < button.Position.Y + button.Texture.Height ||
                            newStateKeyboard.IsKeyDown(Keys.R))

                        {
                            if (newStateMouse.LeftButton == ButtonState.Pressed && lastStateMouse.LeftButton == ButtonState.Released || newStateKeyboard.IsKeyDown(Keys.R) && lastStateKeyboard.IsKeyDown(Keys.R))
                            {
                                ButtonActivated = button;
                            }
                            ButtonHover = button;
                        }
                    }
                }
                button = ButtonsUIArray["entityAbility"];
                if (newStateMouse.X > button.Position.X && newStateMouse.X < button.Position.X + button.Texture.Width &&
                    newStateMouse.Y > button.Position.Y && newStateMouse.Y < button.Position.Y + button.Texture.Height ||
                    newStateKeyboard.IsKeyDown(Keys.P))
                {
                    if (newStateMouse.LeftButton == ButtonState.Pressed && lastStateMouse.LeftButton == ButtonState.Released ||
                        newStateKeyboard.IsKeyDown(Keys.P) && !lastStateKeyboard.IsKeyDown(Keys.P))
                    {
                        Ctx.Map.UseEntity();
                    }
                    ButtonHover = button;
                }
            }
            // Second Selection
            else if (newStateMouse.LeftButton == ButtonState.Pressed && lastStateMouse.LeftButton == ButtonState.Released)
            {
                if (ButtonActivated.Name == "explosionAbility" && !Ctx.GameIsPaused)
                {
                    Ctx.Map.UseExplosionAbility(new Vector2(newStateMouse.X, newStateMouse.Y));
                    Ctx.AnimSprites[0].AnimatedSprite.Add(new SimpleAnimationSprite(Ctx.AnimSprites[0], newStateMouse.X - 32, newStateMouse.Y - 32));
                    ManagerSound.PlayExplosionAbility();
                }

                if (ButtonActivated.Name == "sniperAbility" && !Ctx.GameIsPaused)
                {
                    Ctx.Map.UseSniperAbility(new Vector2(newStateMouse.X, newStateMouse.Y));
                    ManagerSound.PlaySniperShoot();
                }

                if (ButtonActivated.Name == "stickyRiceAbility" && !Ctx.GameIsPaused)
                {
                    Ctx.Map.UseStickyRiceAbility(new Vector2(newStateMouse.X, newStateMouse.Y));
                }


                if (ButtonActivated.Name == "wallBossAbility" && !Ctx.GameIsPaused)
                {
                    Ctx.Map.UseWallBossAbility(new Vector2(newStateMouse.X - Ctx._wallBoss.Width/2, newStateMouse.Y - Ctx._wallBoss.Height/2));
                }

                ButtonActivated = null;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(ButtonUIAbility button in ButtonsUIArray.Values) button.Draw(spriteBatch);
        }

        public void CreateButtonUI(ButtonUIAbility buttonUI)
        {
            ButtonsUIArray[buttonUI.Name] = buttonUI;
        }
    }
}
