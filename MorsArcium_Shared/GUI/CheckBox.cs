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
