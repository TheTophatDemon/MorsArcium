using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public class Settings
    {
        public struct PlayerBindings
        {
            public IBinding up;
            public IBinding down;
            public IBinding right;
            public IBinding left;
            public IBinding jump;
            public IBinding attack;
            public IBinding special;
            public IBinding pause;
        }
        public static PlayerBindings bindings = new PlayerBindings();
        public static bool musicEnabled = true;
        public static bool soundEnabled = true;
        public static bool fullScreen = false;
        public static bool jumpFly = true;
        public static bool vSync = false;
        public static bool playedBefore = false;
        public static void Load()
        {
            bindings.up = new KeyBinding(Keys.W);
            bindings.down = new KeyBinding(Keys.S);
            bindings.right = new KeyBinding(Keys.D);
            bindings.left = new KeyBinding(Keys.A);
            bindings.jump = new KeyBinding(Keys.Space);
            bindings.attack = new KeyBinding(Keys.J);
            bindings.special = new KeyBinding(Keys.K);
            bindings.pause = new KeyBinding(Keys.Enter);
        }
        public static void Save()
        {

        }
    }
}
