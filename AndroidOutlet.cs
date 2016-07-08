using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium
{
    public class AndroidOutlet
    {
        public bool up = false;
        public bool down = false;
        public bool left = false;
        public bool right = false;
        public bool jump = false;
        public bool attack = false;
        public bool special = false;
        public bool pause = false;
        public bool exit = false;
        bool move = false;
        protected Button[] buttons;
        MouseState prevMouseState;
        TouchCollection prevTouches;
        public MorsArcium game;
        public AndroidOutlet()
        {
            buttons = new Button[6];
            buttons[0].source = new Rectangle(128, 96, 32, 32); //1 button (Attack)
            buttons[0].position = new Vector2(208, 160);
            buttons[1].source = new Rectangle(128, 128, 32, 32); //2 button (Special)
            buttons[1].position = new Vector2(272, 160);
            buttons[2].source = new Rectangle(128, 160, 32, 32); //Jump button
            buttons[2].position = new Vector2(240, 192);
            //buttons[3].source = new Rectangle(192, 96, 32, 32); //Pause Button
            //buttons[3].position = new Vector2(216, 4);
            buttons[4].source = new Rectangle(448, 176, 64, 32); //Menu Button
            buttons[4].position = new Vector2(252, 4);
            buttons[5].source = new Rectangle(160, 96, 32, 32); //Joystick
            buttons[5].position = new Vector2(32, 192);
        }
        public virtual void UpdateControls(GameTime gt)
        {
            up = false;
            down = false;
            right = false;
            left = false;
            jump = false;
            special = false;
            exit = false;
            pause = false;
            attack = false;
            MouseState mouseState = Mouse.GetState(game.Window);
            TouchCollection touches = TouchPanel.GetState();
            int mx = (int)((mouseState.Position.X - game.thing.X) / game.scaleFactor);
            int my = (int)(mouseState.Position.Y / game.scaleFactor);
            int tx = -256;
            int ty = -256;
            if (Mouse.GetState().LeftButton == ButtonState.Released && touches.Count == 0)
            {
                move = false;
            }
            if (touches.Count > 0)
            {
                tx = (int)((touches[0].Position.X - game.thing.X) / game.scaleFactor);
                ty = (int)(touches[0].Position.Y / game.scaleFactor);
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].position != null)
                {
                    buttons[i].hover = false;
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        if (mx > buttons[i].position.X && mx < buttons[i].position.X + buttons[i].source.Width
                            && my > buttons[i].position.Y && my < buttons[i].position.Y + buttons[i].source.Height)
                        {
                            buttons[i].hover = true;
                            OnButtonPress(buttons[i]);
                        }
                    }
                    if (touches.Count > 0)
                    {
                        if (tx > buttons[i].position.X && tx < buttons[i].position.X + buttons[i].source.Width
                            && ty > buttons[i].position.Y && ty < buttons[i].position.Y + buttons[i].source.Height)
                        {
                            buttons[i].hover = true;
                            if (buttons[i].function != null) buttons[i].function();
                            OnButtonPress(buttons[i]);
                        }
                    }
                }
            }
            if (move)
            {
                buttons[5].position.X = tx - 16;
                buttons[5].position.Y = ty - 16;
#if WINDOWS
                buttons[5].position.X = mx - 16;
                buttons[5].position.Y = my - 16;
#endif
                if (buttons[5].position.X > 48) buttons[5].position.X = 64;
                if (buttons[5].position.X < 16) buttons[5].position.X = 16;
                if (buttons[5].position.Y > 208) buttons[5].position.Y = 208;
                if (buttons[5].position.Y < 176) buttons[5].position.Y = 176;
            }
            else
            {
                buttons[5].position.X = 32;
                buttons[5].position.Y = 192;
            }
            float ofsx = buttons[5].position.X - 32;
            float ofsy = buttons[5].position.Y - 192;
            if (ofsx > 4.0f) right = true;
            if (ofsx < -4.0f) left = true;
            if (ofsy > 4.0f) down = true;
            if (ofsy < -4.0f) up = true;
            prevMouseState = mouseState;
            prevTouches = touches;
        }
        public virtual void DrawControls(SpriteBatch sp)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].source != null)
                {
                    sp.Draw(game.textures[2], buttons[i].position, buttons[i].source, (buttons[i].hover == true) ? Color.Gray : Color.White);
                }
            }
        }
        public virtual void SaveSettings() { }
        public virtual void LoadSettings() { }
        protected void OnButtonPress(Button source)
        {
            if (source.position == buttons[0].position)
            {
                attack = true;
            }
            else if (source.position == buttons[1].position)
            {
                special = true;
            }
            else if (source.position == buttons[2].position)
            {
                jump = true;
            }
            else if (source.position == buttons[3].position)
            {
                pause = true;
            }
            else if (source.position == buttons[4].position)
            {
                exit = true;
            }
            else if (source.position == buttons[5].position)
            {
                move = true;
            }
        }
    }
}
