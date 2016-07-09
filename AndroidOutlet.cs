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
        protected Button[] buttons;
        MouseState prevMouseState;
        TouchCollection prevTouches;
        public MorsArcium game;
        public AndroidOutlet()
        {
            buttons = new Button[8];
            buttons[0].source = new Rectangle(432, 208, 40, 32); //1 button (Attack)
            buttons[0].position = new Vector2(224, 184);
            buttons[1].source = new Rectangle(432, 240, 32, 32); //2 button (Special)
            buttons[1].position = new Vector2(288, 184);
            buttons[2].source = new Rectangle(432, 272, 40, 32); //Jump button
            buttons[2].position = new Vector2(256, 208);
            buttons[3].source = new Rectangle(384, 208, 48, 32); //Right Button
            buttons[3].position = new Vector2(40, 192);
            buttons[4].source = new Rectangle(448, 176, 64, 32); //Menu Button
            buttons[4].position = new Vector2(252, 36);
            buttons[5].source = new Rectangle(160, 96, 32, 32); //Left Button
            buttons[5].position = new Vector2(0, 192);
            buttons[6].source = new Rectangle(432, 304, 48, 32); //Up attack!
            buttons[6].position = new Vector2(232, 152);
            buttons[7].source = new Rectangle(432, 336, 48, 24); // Down Attack!
            buttons[7].position = new Vector2(216, 216);
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
                    for (int j = 0; j < touches.Count; j++)
                    {
                        int tx = (int)((touches[j].Position.X - game.thing.X) / game.scaleFactor);
                        int ty = (int)(touches[j].Position.Y / game.scaleFactor);
                        if (i == 3 || i == 5)
                        {
                            if ((tx > buttons[i].position.X || i == 5) && tx < buttons[i].position.X + buttons[i].source.Width)
                            {
                                buttons[i].hover = true;
                                if (buttons[i].function != null) buttons[i].function();
                                OnButtonPress(buttons[i], tx, ty);
                            }
                        }
                        else
                        {
                            if (tx > buttons[i].position.X && tx < buttons[i].position.X + buttons[i].source.Width
                                && ty > buttons[i].position.Y && ty < buttons[i].position.Y + buttons[i].source.Height)
                            {
                                buttons[i].hover = true;
                                if (buttons[i].function != null) buttons[i].function();
                                OnButtonPress(buttons[i], tx, ty);
                            }
                        }
                    }
                }
            }
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
        public virtual void LoadDifficulty(string path)
        { }
        protected void OnButtonPress(Button source, int tx = 0, int ty = 0)
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
                right = true;
            }
            else if (source.position == buttons[4].position)
            {
                exit = true;
            }
            else if (source.position == buttons[5].position)
            {
                left = true;
            }
            else if (source.position == buttons[6].position)
            {
                up = true;
                attack = true;
            }
            else if (source.position == buttons[7].position)
            {
                down = true;
                attack = true;
            }
        }
    }
}
