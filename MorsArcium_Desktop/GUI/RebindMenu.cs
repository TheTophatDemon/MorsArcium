/*
Copyright (C) 2016-present Alexander Lunsford

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium.GUI
{
    public class RebindMenu : Menu
    {
        /// <summary>
        /// Represents a collection of properties related to a specific binding
        /// </summary>
        private class BindingGroup
        {
            public Button button;
            public Binding binding;
            public Action<Binding> setFunc;
            public string label;
            public BindingGroup(string label, Binding binding, Action<Binding> setFunc)
            {
                this.binding = binding;
                this.setFunc = setFunc;
                this.label = label;
            }
        }

        private static readonly Rectangle rebindButtonRect = new Rectangle(0, 352, 64, 16);

        BindingGroup[] bindingGroups;
        BindingGroup currentBindingGroup = null;

        MouseState prevMouseState;
        
        public RebindMenu(GameManager gMan) : base(gMan)
        {
            //Back button
            buttons.Add(new Button(gMan, gMan.textures["hud"], new Rectangle(384, 152, 128, 24), new Vector2(16, 16),
                (Button b) => {
                    gMan.ChangeMenuState(new OptionsMenu(gMan));
                }));

            Settings settings = gMan.platform.GameSettings;
            
            bindingGroups = new BindingGroup[]{
                new BindingGroup("AIM UP", settings.aimUp, 
                    (Binding b) => { settings.aimUp = b; }),
                new BindingGroup("AIM DOWN", settings.aimDown, 
                    (Binding b) => { settings.aimDown = b; }),
                new BindingGroup("MOVE LEFT", settings.moveLeft, 
                    (Binding b) => { settings.moveLeft = b; }),
                new BindingGroup("MOVE RIGHT", settings.moveRight, 
                    (Binding b) => { settings.moveRight = b; }),
                new BindingGroup("JUMP", settings.jump, 
                    (Binding b) => { settings.jump = b; }),
                new BindingGroup("ATTACK", settings.attack, 
                    (Binding b) => { settings.attack = b; }),
                new BindingGroup("SPECIAL ATTACK", settings.special, 
                    (Binding b) => { settings.special = b; }),
                new BindingGroup("PAUSE", settings.pause, 
                    (Binding b) => { settings.pause = b; }),
            };

            //Generate buttons for all of the bindings groups
            for (int i = 0; i < bindingGroups.Length; ++i)
            {
                BindingGroup bg = bindingGroups[i];
                bg.button = new Button(gMan, gMan.textures["hud"], rebindButtonRect, new Vector2(16, 64 + i * 20),
                    (Button b) => {
                        currentBindingGroup = bg;
                    });
                buttons.Add(bg.button);
            }

        }

        public void SetCurrentBinding(Binding binding)
        {
            currentBindingGroup.binding = binding;
            currentBindingGroup.setFunc?.Invoke(binding);
            currentBindingGroup = null;
        }
        
        public override void Update(GameTime g)
        {
            base.Update(g);

            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (currentBindingGroup != null)
            {
                //See if a keyboard or mouse button is pressed while in rebinding mode
                if (keyboard.GetPressedKeys().Length > 0)
                {
                    SetCurrentBinding(new KeyBinding(keyboard.GetPressedKeys()[0]));
                }
                else if (mouse.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton != ButtonState.Pressed)
                {
                    SetCurrentBinding(new MouseButtonBinding(MouseButtonBinding.Button.LEFT));
                }
                else if (mouse.RightButton == ButtonState.Pressed && prevMouseState.RightButton != ButtonState.Pressed)
                {
                    SetCurrentBinding(new MouseButtonBinding(MouseButtonBinding.Button.RIGHT));
                }
                else if (mouse.MiddleButton == ButtonState.Pressed && prevMouseState.MiddleButton != ButtonState.Pressed)
                {
                    SetCurrentBinding(new MouseButtonBinding(MouseButtonBinding.Button.MIDDLE));
                }

                for (int i = 0; i < 4; i++)
                {
                    JoystickCapabilities capabilities = Joystick.GetCapabilities(i);

                    if (capabilities.IsConnected)
                    {
                        JoystickState state = Joystick.GetState(i);

                        //Check joystick buttons
                        for (int j = 0; j < capabilities.ButtonCount; j++)
                        {
                            if (state.Buttons[j] == ButtonState.Pressed)
                            {
                                SetCurrentBinding(new JoystickButtonBinding(i, j));
                                break;
                            }
                        }
                        if (currentBindingGroup == null) break;

                        //Check joystick hats
                        for (int j = 0; j < capabilities.HatCount; j++)
                        {
                            if (state.Hats[j].Up == ButtonState.Pressed)
                            {
                                SetCurrentBinding(new JoystickHatBinding(i, j, JoystickHatBinding.Button.UP));
                                break;
                            }
                            else if (state.Hats[j].Down == ButtonState.Pressed)
                            {
                                SetCurrentBinding(new JoystickHatBinding(i, j, JoystickHatBinding.Button.DOWN));
                                break;
                            }
                            else if (state.Hats[j].Left == ButtonState.Pressed)
                            {
                                SetCurrentBinding(new JoystickHatBinding(i, j, JoystickHatBinding.Button.LEFT));
                                break;
                            }
                            else if (state.Hats[j].Right == ButtonState.Pressed)
                            {
                                SetCurrentBinding(new JoystickHatBinding(i, j, JoystickHatBinding.Button.RIGHT));
                                break;
                            }
                        }
                        if (currentBindingGroup == null) break;

                        //Check joystick axes
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

            prevMouseState = mouse;
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);

            DrawTrippyBackground(sp);
            DrawButtons(sp);

            foreach (BindingGroup bg in bindingGroups)
            {
                sp.DrawString(gMan.fonts["default"], bg.label + ": " + bg.binding.ToString(), bg.button.Position + new Vector2(72, 0), Color.White);
            }

            sp.End();
        }
    }

}
