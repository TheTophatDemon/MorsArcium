using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium.GUI
{
    public class CheckBox : Button
    {
        public bool IsChecked { get => isChecked; }

        protected bool isChecked = false;
        public CheckBox(GameManager gMan, Vector2 position, Action<Button> clickAction, bool isChecked = false)
            : base(gMan, gMan.textures["hud"], new Rectangle(isChecked ? 32 : 0, 320, 32, 32), position, clickAction)
        {
            this.isChecked = isChecked;
        }
        public override void Update(GameTime time)
        {
            prevStatus = status;
            status = gMan.platform.ProcessMenuButton(this);

            color = (status == State.DEFAULT) ? Color.White : Color.Gray;
            if (status == State.PRESSED && prevStatus != State.PRESSED)
            {
                isChecked = !isChecked;
                source.X = isChecked ? 32 : 0;
                onClick?.Invoke(this);
            }
        }
    }
}
