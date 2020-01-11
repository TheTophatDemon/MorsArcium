using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium
{
    public struct MenuButton
    {
        public Vector2 position;
        public Rectangle source;
        public Action function;
        public MenuButtonState state;
        public SpriteEffects spriteEffect;
    }
    public enum MenuButtonState
    {
        DEFAULT,
        HOVER,
        PRESSED
    }
    
    public class Menu
    {
        public MenuButton[] buttons;
        protected GameManager game;

        public Menu(GameManager g)
        {
            game = g;
        }
        public virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].state = game.platform.ProcessMenuButton(buttons[i]);
                if (buttons[i].state == MenuButtonState.PRESSED)
                {
                    //Cool syntax!!!
                    buttons[i].function?.Invoke();
                    OnButtonPress(buttons[i]);
                }
            }
        }
        public void DrawButtons(SpriteBatch sp)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                sp.Draw(game.textures[2], buttons[i].position, buttons[i].source, 
                    (buttons[i].state == MenuButtonState.DEFAULT) ? Color.White : Color.Gray, 
                    0.0f, Vector2.Zero, 1.0f, buttons[i].spriteEffect, 0.0f);
            }
        }
        public virtual void Draw(SpriteBatch sp)
        {
            
        }
        public virtual void OnButtonPress(MenuButton source)
        {

        }
    }
}
