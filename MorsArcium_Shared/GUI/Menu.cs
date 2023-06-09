﻿/*
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
using System.Collections.Generic;

namespace Mors_Arcium.GUI
{
    public class Menu
    {
        protected List<Button> buttons = new List<Button>();
        protected GameManager gMan;

        private float backgroundTimer = 0.0f;
        private Vector2 backgroundPosition = Vector2.Zero;
        private Color backgroundColor = Color.Gray;

        protected Menu(GameManager gMan)
        {
            this.gMan = gMan;
        }
        public virtual void OnEnter() { }
        public virtual void OnLeave() { }
        public virtual void Update(GameTime gameTime)
        {
            foreach (Button b in buttons)
            {
                b.Update(gameTime);
            }
        }
        public void DrawButtons(SpriteBatch sp)
        {
            foreach (Button b in buttons)
            {
                b.Draw(sp);
            }
        }
        public void DrawTrippyBackground(SpriteBatch sp)
        {
            backgroundTimer += 0.05f;

            byte b = (byte)Math.Round(Math.Sin(backgroundTimer * 0.5f));
            backgroundColor.R += b;
            backgroundColor.G += b;
            backgroundColor.B += b;

            backgroundPosition.X -= 1.0f;
            backgroundPosition.Y -= 1.0f;
            if (backgroundPosition.X < -320.0f) backgroundPosition.X = 0.0f;
            if (backgroundPosition.Y < -240.0f) backgroundPosition.Y = 0.0f;

            sp.Draw(gMan.textures["sky"], backgroundPosition, backgroundColor);
            sp.Draw(gMan.textures["sky"], backgroundPosition + new Vector2(320, 0), backgroundColor);
            sp.Draw(gMan.textures["sky"], backgroundPosition + new Vector2(320, 240), backgroundColor);
            sp.Draw(gMan.textures["sky"], backgroundPosition + new Vector2(0, 240), backgroundColor);
        }
        public virtual void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);

            DrawTrippyBackground(sp);
            DrawButtons(sp);

            sp.End();
        }
    }
}
