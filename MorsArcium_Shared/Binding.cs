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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.Xml;

namespace Mors_Arcium
{
    public abstract class Binding
    {
        public abstract bool IsDown();
        public abstract XmlElement Serialize(XmlDocument doc);
    }
    public class KeyBinding : Binding
    {
        readonly Keys key;
        public KeyBinding(Keys key)
        {
            this.key = key;
        }
        public override bool IsDown()
        {
            return Keyboard.GetState().IsKeyDown(key);
        }
        public override string ToString()
        {
            return key.ToString();
        }
        public override XmlElement Serialize(XmlDocument doc)
        {
            XmlElement elem = doc.CreateElement("keyBinding");

            XmlAttribute keyAttr = doc.CreateAttribute("key");
            keyAttr.Value = ((int)key).ToString();
            elem.Attributes.Append(keyAttr);

            return elem;
        }
        public static KeyBinding Deserialize(XmlElement elem)
        {
            return new KeyBinding((Keys) int.Parse(elem.SelectSingleNode("./@key").Value));
        }
    }
    public class MouseButtonBinding : Binding
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
        public override bool IsDown()
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
        public override XmlElement Serialize(XmlDocument doc)
        {
            XmlElement elem = doc.CreateElement("mouseButtonBinding");

            XmlAttribute buttonAttr = doc.CreateAttribute("button");
            buttonAttr.Value = ((int)button).ToString();
            elem.Attributes.Append(buttonAttr);

            return elem;
        }
        public static MouseButtonBinding Deserialize(XmlElement elem)
        {
            return new MouseButtonBinding((Button) int.Parse(elem.SelectSingleNode("./@button").Value));
        }
    }
    public class JoystickButtonBinding : Binding
    {
        readonly int joyIndex;
        readonly int button;
        public JoystickButtonBinding(int joyIndex, int button)
        {
            this.joyIndex = joyIndex;
            this.button = button;
        }
        public override bool IsDown()
        {
            return Joystick.GetState(joyIndex).Buttons[button] == ButtonState.Pressed;
        }
        public override string ToString()
        {
            return "Joystick Button " + button.ToString();
        }
        public override XmlElement Serialize(XmlDocument doc)
        {
            XmlElement elem = doc.CreateElement("joystickButtonBinding");

            XmlAttribute buttonAttr = doc.CreateAttribute("button");
            buttonAttr.Value = button.ToString();
            elem.Attributes.Append(buttonAttr);

            XmlAttribute indAttr = doc.CreateAttribute("joystick");
            indAttr.Value = joyIndex.ToString();
            elem.Attributes.Append(indAttr);

            return elem;
        }
        public static JoystickButtonBinding Deserialize(XmlElement elem)
        {
            return new JoystickButtonBinding(
                int.Parse(elem.SelectSingleNode("./@joystick").Value),
                int.Parse(elem.SelectSingleNode("./@button").Value)
                );
        }
    }
    public class JoystickHatBinding : Binding
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
        public override bool IsDown()
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
        public override string ToString()
        {
            string s = "Joystick Hat " + hat.ToString() + " ";
            switch (button)
            {
                case Button.UP: s += "Up"; break;
                case Button.DOWN: s += "Down"; break;
                case Button.LEFT: s += "Left"; break;
                case Button.RIGHT: s += "Right"; break;
            }
            return s;
        }
        public override XmlElement Serialize(XmlDocument doc)
        {
            XmlElement elem = doc.CreateElement("joystickHatBinding");

            XmlAttribute joyAttr = doc.CreateAttribute("joystick");
            joyAttr.Value = joyIndex.ToString();
            elem.Attributes.Append(joyAttr);

            XmlAttribute hatAttr = doc.CreateAttribute("hat");
            hatAttr.Value = hat.ToString();
            elem.Attributes.Append(hatAttr);

            XmlAttribute buttAttr = doc.CreateAttribute("button");
            buttAttr.Value = ((int)button).ToString();
            elem.Attributes.Append(buttAttr);

            return elem;
        }
        public static JoystickHatBinding Deserialize(XmlElement elem)
        {
            return new JoystickHatBinding(
                int.Parse(elem.SelectSingleNode("./@joystick").Value),
                int.Parse(elem.SelectSingleNode("./@hat").Value),
                (Button) int.Parse(elem.SelectSingleNode("./@button").Value)
                );
        }
    }
    public class JoystickAxisBinding : Binding
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
        public override bool IsDown()
        {
            if (positive)
            {
                return Joystick.GetState(joyIndex).Axes[axis] > 0.25f;
            }
            return Joystick.GetState(joyIndex).Axes[axis] < -0.25f;
        }
        public override string ToString()
        {
            return "Joystick Axis " + axis.ToString() + (positive ? "+" : "-");
        }
        public override XmlElement Serialize(XmlDocument doc)
        {
            XmlElement elem = doc.CreateElement("joystickAxisBinding");

            XmlAttribute joyAttr = doc.CreateAttribute("joystick");
            joyAttr.Value = joyIndex.ToString();
            elem.Attributes.Append(joyAttr);

            XmlAttribute axisAttr = doc.CreateAttribute("axis");
            axisAttr.Value = axis.ToString();
            elem.Attributes.Append(axisAttr);

            XmlAttribute sgnAttr = doc.CreateAttribute("positive");
            sgnAttr.Value = positive.ToString();
            elem.Attributes.Append(sgnAttr);

            return elem;
        }

        public static JoystickAxisBinding Deserialize(XmlElement elem)
        {
            return new JoystickAxisBinding(
                int.Parse(elem.SelectSingleNode("./@joystick").Value),
                int.Parse(elem.SelectSingleNode("./@axis").Value),
                bool.Parse(elem.SelectSingleNode("./@positive").Value)
                );
        }
    }
}
