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
        MouseState prevMouseState;
        TouchCollection prevTouches;
        public Menu(MorsArcium g)
        {
            game = g;
        }
        public virtual void Update(GameTime g)
        {
            MouseState mouseState = Mouse.GetState(game.Window);
            TouchCollection touches = TouchPanel.GetState();
            int mx = (int)((mouseState.Position.X - game.thing.X) / game.scaleFactor);
            int my = (int)(mouseState.Position.Y / game.scaleFactor);
            int tx = -256;
            int ty = -256;
            if (touches.Count > 0)
            {
                tx = (int)((touches[0].Position.X - game.thing.X) / game.scaleFactor);
                ty = (int)(touches[0].Position.Y / game.scaleFactor);
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].hover = false;
                if (mx > buttons[i].position.X && mx < buttons[i].position.X + buttons[i].source.Width
                    && my > buttons[i].position.Y && my < buttons[i].position.Y + buttons[i].source.Height)
                {
                    buttons[i].hover = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        if (buttons[i].function != null) buttons[i].function();
                        OnButtonPress(buttons[i]);
                    }
                }
                if (touches.Count > 0)
                {
                    if (tx > buttons[i].position.X && tx < buttons[i].position.X + buttons[i].source.Width
                        && ty > buttons[i].position.Y && ty < buttons[i].position.Y + buttons[i].source.Height)
                    {
                        buttons[i].hover = true;
                        if (touches[0].State == TouchLocationState.Pressed && prevTouches.Count == 0)
                        {
                            if (buttons[i].function != null) buttons[i].function();
                            OnButtonPress(buttons[i]);
                        }
                    }
                }
                
            }
            prevMouseState = mouseState;
            prevTouches = touches;
        }
        public virtual void Draw(SpriteBatch sp)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                sp.Draw(game.textures[2], buttons[i].position, buttons[i].source, (buttons[i].hover == true) ? Color.Gray : Color.White);
            }
        }
        public virtual void OnButtonPress(Button source)
        {

        }
    }
}
