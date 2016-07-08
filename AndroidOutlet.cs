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
        public AndroidOutlet()
        {

        }
        public virtual void UpdateControls(GameTime gt) { }
        public virtual void DrawControls(SpriteBatch sp) { }
        public virtual void SaveSettings() { }
        public virtual void LoadSettings() { }

    }
}
