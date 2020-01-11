using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public interface IBinding
    {
        bool IsDown();
    }
    public class KeyBinding : IBinding
    {
        readonly Keys key;
        public KeyBinding(Keys key)
        {
            this.key = key;
        }
        public bool IsDown()
        {
            return Keyboard.GetState().IsKeyDown(key);
        }
        public override string ToString()
        {
            return key.ToString();
        }
    }
    public class MouseButtonBinding : IBinding
    {
        public enum Button
        {
            LEFT,
            RIGHT,
            MIDDLE
        }
        readonly Button button;
        public MouseButtonBinding(Button button)
        {
            this.button = button;
        }
        public bool IsDown()
        {
            switch (button)
            {
                case Button.LEFT:
                    return Mouse.GetState().LeftButton == ButtonState.Pressed;
                case Button.RIGHT:
                    return Mouse.GetState().RightButton == ButtonState.Pressed;
                case Button.MIDDLE:
                    return Mouse.GetState().MiddleButton == ButtonState.Pressed;
            }
            return false;
        }
        public override string ToString()
        {
            switch (button)
            {
                case Button.LEFT: return "Left Mouse";
                case Button.RIGHT: return "Right Mouse";
                case Button.MIDDLE: return "Middle Mouse";
            }
            return "Invalid Mouse";
        }
    }
    public class JoystickButtonBinding : IBinding
    {
        readonly int joyIndex;
        readonly int button;
        public JoystickButtonBinding(int joyIndex, int button)
        {
            this.joyIndex = joyIndex;
            this.button = button;
        }
        public bool IsDown()
        {
            return Joystick.GetState(joyIndex).Buttons[button] == ButtonState.Pressed;
        }
        public override string ToString()
        {
            return "Joystick Button " + button.ToString();
        }
    }
    public class JoystickHatBinding : IBinding
    {
        public enum Button
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }
        readonly int joyIndex;
        readonly int hat;
        readonly Button button;
        public JoystickHatBinding(int joyIndex, int hat, Button button)
        {
            this.joyIndex = joyIndex;
            this.hat = hat;
            this.button = button;
        }
        public bool IsDown()
        {
            switch (button)
            {
                case Button.UP: return Joystick.GetState(joyIndex).Hats[hat].Up == ButtonState.Pressed;
                case Button.DOWN: return Joystick.GetState(joyIndex).Hats[hat].Down == ButtonState.Pressed;
                case Button.LEFT: return Joystick.GetState(joyIndex).Hats[hat].Left == ButtonState.Pressed;
                case Button.RIGHT: return Joystick.GetState(joyIndex).Hats[hat].Right == ButtonState.Pressed;
            }
            return false;
        }
        //Forgot ToString!
    }
    public class JoystickAxisBinding : IBinding
    {
        readonly int joyIndex;
        readonly int axis;
        readonly bool positive;
        public JoystickAxisBinding(int joyIndex, int axis, bool positive)
        {
            this.joyIndex = joyIndex;
            this.axis = axis;
            this.positive = positive;
        }
        public bool IsDown()
        {
            if (positive)
            {
                return Joystick.GetState(joyIndex).Axes[axis] > 0.25f;
            }
            return Joystick.GetState(joyIndex).Axes[axis] < -0.25f;
        }
        public override string ToString()
        {
            return "Joystick Axis " + axis.ToString();
        }
    }
}
