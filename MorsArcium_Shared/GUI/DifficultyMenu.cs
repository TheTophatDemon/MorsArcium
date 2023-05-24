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
    public class DifficultyMenu : Menu
    {
        private static readonly Rectangle textRect = new Rectangle(0, 432, 215, 25);

        public DifficultyMenu(GameManager gMan) : base(gMan)
        {
            //Easy button
            buttons.Add(new Button(gMan, gMan.textures["hud"], new Rectangle(128, 0, 128, 24), new Vector2(96, 144),
                (Button b) => {
                    gMan.game.difficulty = "easy";
                    gMan.ChangeMenuState(new ClassMenu(gMan));
                }));

            //Medium button
            buttons.Add(new Button(gMan, gMan.textures["hud"], new Rectangle(128, 24, 128, 24), new Vector2(96, 112), 
                (Button b) => {
                    gMan.game.difficulty = "normal";
                    gMan.ChangeMenuState(new ClassMenu(gMan));
                }));

            //Hard button
            buttons.Add(new Button(gMan, gMan.textures["hud"], new Rectangle(128, 48, 128, 24), new Vector2(96, 80),
                (Button b) => {
                    gMan.game.difficulty = "hard";
                    gMan.ChangeMenuState(new ClassMenu(gMan));
                }));

            //Random difficulty
            buttons.Add(new Button(gMan, gMan.textures["hud"], new Rectangle(128, 72, 128, 24), new Vector2(96, 176),
                (Button b) => {
                    gMan.game.difficulty = "????";
                    gMan.ChangeMenuState(new ClassMenu(gMan));
                }));
        }

        public override void Update(GameTime g)
        {
            base.Update(g);
            
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) gMan.platform.Exit();
        }

        public override void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);

            DrawTrippyBackground(sp);
            DrawButtons(sp);

            sp.Draw(gMan.textures["hud"], new Vector2(53, 20), textRect, Color.White);

            sp.End();
        }
    }
}
