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
