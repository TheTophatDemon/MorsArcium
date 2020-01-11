using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public class RebindMenu : Menu
    {
        float floaty = 0.0f;
        Vector2 backgroundPosition;
        Color backgroundColor = Color.Gray;
        bool rebinding = false;
        MenuButton buttonLastPressed;
        MouseState prevMouse;
        public RebindMenu(GameManager g) : base(g)
        {
            buttons = new MenuButton[11];
            buttons[0].source = new Rectangle(384, 152, 128, 24); //Back button
            buttons[0].position = new Vector2(16, 16);
            buttons[1].source = new Rectangle(0, 352, 64, 16); //Up
            buttons[1].position = new Vector2(16, 36+32);
            buttons[2].source = buttons[1].source; //Down
            buttons[2].position = new Vector2(16, 56+32);
            buttons[3].source = buttons[1].source; //Left
            buttons[3].position = new Vector2(16, 76+32);
            buttons[4].source = buttons[1].source; //Right
            buttons[4].position = new Vector2(16, 96+32);
            buttons[5].source = buttons[1].source; //Jump
            buttons[5].position = new Vector2(16, 116+32);
            buttons[6].source = buttons[1].source; //Attack
            buttons[6].position = new Vector2(16, 136+32);
            buttons[7].source = buttons[1].source; //Special
            buttons[7].position = new Vector2(16, 156+32);
            buttons[8].source = buttons[1].source; //PAUSE
            buttons[8].position = new Vector2(16, 176+32);
        }
        public override void OnButtonPress(MenuButton source)
        {
            int index = 0;
            for (; index < buttons.Length; index++)
            {
                if (buttons[index].Equals(source)) break;
            }
            if (index == 0)
            {
                game.ChangeMenuState(new OptionsMenu(game));
            }
            else
            {
                rebinding = true;
                buttonLastPressed = source;
            }
        }
        public void SetBinding(MenuButton button, IBinding newBinding)
        {
            int index = 0;
            for (; index < buttons.Length; index++)
            {
                if (buttons[index].Equals(button)) break;
            }
            switch (index)
            {
                case 1: Settings.bindings.up = newBinding; break;
                case 2: Settings.bindings.down = newBinding; break;
                case 3: Settings.bindings.left = newBinding; break;
                case 4: Settings.bindings.right = newBinding; break;
                case 5: Settings.bindings.jump = newBinding; break;
                case 6: Settings.bindings.attack = newBinding; break;
                case 7: Settings.bindings.special = newBinding; break;
                case 8: Settings.bindings.pause = newBinding; break;
            }
        }
        public override void Update(GameTime g)
        {
            if (rebinding)
            {
                KeyboardState keyboard = Keyboard.GetState();
                MouseState mouse = Mouse.GetState();
                if (keyboard.GetPressedKeys().Length > 0)
                {
                    SetBinding(buttonLastPressed, new KeyBinding(keyboard.GetPressedKeys()[0]));
                    rebinding = false;
                }
                else if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                {
                    SetBinding(buttonLastPressed, new MouseButtonBinding(MouseButtonBinding.Button.LEFT));
                    rebinding = false;
                }
                else if (mouse.RightButton == ButtonState.Pressed && prevMouse.RightButton != ButtonState.Pressed)
                {
                    SetBinding(buttonLastPressed, new MouseButtonBinding(MouseButtonBinding.Button.RIGHT));
                    rebinding = false;
                }
                else if (mouse.MiddleButton == ButtonState.Pressed && prevMouse.MiddleButton != ButtonState.Pressed)
                {
                    SetBinding(buttonLastPressed, new MouseButtonBinding(MouseButtonBinding.Button.MIDDLE));
                    rebinding = false;
                }
                for (int i = 0; i < 4; i++)
                {
                    JoystickCapabilities capabilities = Joystick.GetCapabilities(i);
                    if (capabilities.IsConnected)
                    {
                        JoystickState state = Joystick.GetState(i);
                        for (int j = 0; j < capabilities.ButtonCount; j++)
                        {
                            if (state.Buttons[j] == ButtonState.Pressed)
                            {
                                SetBinding(buttonLastPressed, new JoystickButtonBinding(i, j));
                                rebinding = false;
                                break;
                            }
                        }
                        if (!rebinding) break;
                        for (int j = 0; j < capabilities.HatCount; j++)
                        {
                            if (state.Hats[j].Up == ButtonState.Pressed)
                            {
                                SetBinding(buttonLastPressed, new JoystickHatBinding(i, j, JoystickHatBinding.Button.UP));
                                rebinding = false;
                                break;
                            }
                            else if (state.Hats[j].Down == ButtonState.Pressed)
                            {
                                SetBinding(buttonLastPressed, new JoystickHatBinding(i, j, JoystickHatBinding.Button.DOWN));
                                rebinding = false;
                                break;
                            }
                            else if (state.Hats[j].Left == ButtonState.Pressed)
                            {
                                SetBinding(buttonLastPressed, new JoystickHatBinding(i, j, JoystickHatBinding.Button.LEFT));
                                rebinding = false;
                                break;
                            }
                            else if (state.Hats[j].Right == ButtonState.Pressed)
                            {
                                SetBinding(buttonLastPressed, new JoystickHatBinding(i, j, JoystickHatBinding.Button.RIGHT));
                                rebinding = false;
                                break;
                            }
                        }
                        if (!rebinding) break;
                        /*for (int j = 0; j < capabilities.AxisCount; j++)
                        {
                            if (Math.Abs(state.Axes[j]) > 0.25f)
                            {
                                SetBinding(buttonLastPressed, new JoystickAxisBinding(i, j, Math.Sign(state.Axes[j]) > 0));
                                rebinding = false;
                                break;
                            }
                        }
                        if (!rebinding) break;*/
                    }
                }
            }
            else
            {
                base.Update(g);
            }
            prevMouse = Mouse.GetState();
            floaty += 0.05f;
            byte b = (byte)Math.Round(Math.Sin(floaty * 0.5f));
            backgroundColor.R += b;
            backgroundColor.G += b;
            backgroundColor.B += b;
            backgroundPosition.X -= 1.0f;
            backgroundPosition.Y -= 1.0f;
            if (backgroundPosition.X < -320.0f) backgroundPosition.X = 0.0f;
            if (backgroundPosition.Y < -240.0f) backgroundPosition.Y = 0.0f;
        }
        public override void Draw(SpriteBatch sp)
        {
            base.Draw(sp);
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);
            sp.Draw(game.textures[1], backgroundPosition, backgroundColor);
            sp.Draw(game.textures[1], backgroundPosition + new Vector2(320, 0), backgroundColor);
            sp.Draw(game.textures[1], backgroundPosition + new Vector2(320, 240), backgroundColor);
            sp.Draw(game.textures[1], backgroundPosition + new Vector2(0, 240), backgroundColor);
            DrawButtons(sp);
            sp.DrawString(game.font1, "AIM UP: " +     Settings.bindings.up.ToString(), buttons[1].position + new Vector2(68, 0), Color.White);
            sp.DrawString(game.font1, "AIM DOWN: " +   Settings.bindings.down.ToString(), buttons[2].position + new Vector2(68, 0), Color.White);
            sp.DrawString(game.font1, "WALK LEFT: " +  Settings.bindings.left.ToString(), buttons[3].position + new Vector2(68, 0), Color.White);
            sp.DrawString(game.font1, "WALK RIGHT: " + Settings.bindings.right.ToString(), buttons[4].position + new Vector2(68, 0), Color.White);
            sp.DrawString(game.font1, "JUMP: " +       Settings.bindings.jump.ToString(), buttons[5].position + new Vector2(68, 0), Color.White);
            sp.DrawString(game.font1, "ATTACK: " +     Settings.bindings.attack.ToString(), buttons[6].position + new Vector2(68, 0), Color.White);
            sp.DrawString(game.font1, "SPECIAL: " +    Settings.bindings.special.ToString(), buttons[7].position + new Vector2(68, 0), Color.White);
            sp.DrawString(game.font1, "PAUSE: " +      Settings.bindings.pause.ToString(), buttons[8].position + new Vector2(68, 0), Color.White);
            sp.End();
        }
    }

}
