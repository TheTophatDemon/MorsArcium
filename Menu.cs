using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium
{
    public struct Button
    {
        public Vector2 position;
        public Rectangle source;
        public Action function;
        public bool hover;
    }
    public class Menu
    {
        public Button[] buttons;
        protected MorsArcium game;
        public Menu(MorsArcium g)
        {
            game = g;
        }
        public virtual void Update(GameTime g)
        {
            MouseState mouseState = Mouse.GetState(game.Window);
            TouchCollection touches = TouchPanel.GetState();
            int mx = (int)(mouseState.Position.X / game.scaleFactor);
            int my = (int)(mouseState.Position.Y / game.scaleFactor);
            int tx = -256;
            int ty = -256;
            if (touches.Count > 0)
            {
                tx = (int)(touches[0].Position.X / game.scaleFactor);
                ty = (int)(touches[0].Position.Y / game.scaleFactor);
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].hover = false;
                if (mx > buttons[i].position.X && mx < buttons[i].position.X + buttons[i].source.Width
                    && my > buttons[i].position.Y && my < buttons[i].position.Y + buttons[i].source.Height)
                {
                    buttons[i].hover = true;
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        buttons[i].function();
                    }
                }
                if (touches.Count > 0)
                {
                    if (touches[0].Position.X > buttons[i].position.X && touches[0].Position.X < buttons[i].position.X + buttons[i].source.Width
                        && touches[0].Position.Y > buttons[i].position.Y && touches[0].Position.Y < buttons[i].position.Y + buttons[i].source.Height)
                    {
                        buttons[i].hover = true;
                        if (touches[0].State == TouchLocationState.Pressed)
                        {
                            buttons[i].function();
                        }
                    }
                }
                
            }
        }
        public virtual void Draw(SpriteBatch sp)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                sp.Draw(game.textures[2], buttons[i].position, buttons[i].source, (buttons[i].hover == true) ? Color.Gray : Color.White);
            }
        }
    }
}
