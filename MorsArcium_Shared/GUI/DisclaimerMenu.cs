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
    public class DisclaimerMenu : Menu
    {
        public DisclaimerMenu(GameManager gMan) : base(gMan)
        {
            //Tutorial button
            buttons.Add(new Button(gMan, gMan.textures["hud"], new Rectangle(0, 152, 128, 24), new Vector2(16, 200),
                (Button b) => {
                    gMan.ChangeMenuState(null);
                    gMan.game.tutorial = true;
                    gMan.game.Initialize(0);
                }));

            //Continue button
            buttons.Add(new Button(gMan, gMan.textures["hud"], new Rectangle(0, 128, 128, 24), new Vector2(176, 200),
                (Button b) => {
                    gMan.ChangeMenuState(new DifficultyMenu(gMan));
                    gMan.game.tutorial = false;
                }));
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);

            DrawTrippyBackground(sp);
            DrawButtons(sp);

            sp.DrawString(gMan.fonts["default"], 
                "IF YOU DO NOT WANT YOUR BUTT\n" +
                "HANDED TO YOU ON A SILVER PLATTER, \n" +
                "YOU SHOULD CLICK ON THAT \n" +
                "'HOW TO PLAY' BUTTON." +
                "\n\n" +
                "ESPECIALLY YOU, JACOB.", new Vector2(8, 8), Color.White);

            sp.End();
        }
    }
}
