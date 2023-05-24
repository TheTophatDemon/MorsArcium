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
    public class Button
    {
        public enum State { DEFAULT, HOVER, PRESSED }

        public Vector2 Position { get => position; }
        public Rectangle Source { get => source; }
        public State Status { get => status; }

        protected GameManager gMan;
        protected Texture2D texture;
        protected Color color = Color.White;
        protected Vector2 position;
        protected Rectangle source;
        protected Action<Button> onClick;
        protected State status = State.DEFAULT;
        protected State prevStatus = State.DEFAULT;

        public Button(GameManager gMan, Texture2D texture, Rectangle source, Vector2 position, Action<Button> clickAction)
        {
            this.gMan = gMan;
            this.texture = texture;
            this.source = source;
            this.position = position;
            onClick = clickAction;
        }
        public virtual void Update(GameTime time)
        {
            prevStatus = status;
            status = gMan.platform.ProcessMenuButton(this);

            if (status != State.PRESSED && prevStatus == State.PRESSED) onClick?.Invoke(this);
            color = (status == State.DEFAULT) ? Color.White : Color.Gray;
        }
        public virtual void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, source, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
