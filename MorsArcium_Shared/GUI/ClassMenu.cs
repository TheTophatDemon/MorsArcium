using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium.GUI
{
    public class ClassMenu : Menu
    {
        protected static readonly Rectangle goldFrame = new Rectangle(64, 320, 96, 80);
        protected static readonly Rectangle selectClass = new Rectangle(0, 416, 180, 16);
        
        public ClassMenu(GameManager g) : base(g)
        {
            //Mr. B's portrait
            buttons.Add(new Button(gMan, gMan.textures[2], new Rectangle(160, 128, 96, 96), new Vector2(56, 40), 
                (Button b) => {
                    SelectPlayer(0);
                }));

            //Wizard's portrait
            buttons.Add(new Button(gMan, gMan.textures[2], new Rectangle(160, 224, 96, 96), new Vector2(168, 40),
                (Button b) => {
                    SelectPlayer(1);
                }));

            //Fingers' portrait
            buttons.Add(new Button(gMan, gMan.textures[2], new Rectangle(64, 224, 96, 96), new Vector2(56, 136),
                (Button b) => {
                    SelectPlayer(2);
                }));

            //Bug's portrait
            buttons.Add(new Button(gMan, gMan.textures[2], new Rectangle(160, 320, 96, 96), new Vector2(168, 136),
                (Button b) => {
                    SelectPlayer(3);
                }));

            gMan.game.started = false;
        }

        public override void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);

            DrawTrippyBackground(sp);
            DrawButtons(sp);

            foreach (Button b in buttons)
            {
                if (b.Status != Button.State.DEFAULT)
                {
                    sp.Draw(gMan.textures[2], b.Position, goldFrame, Color.White);
                }
            }

            sp.Draw(gMan.textures[2], new Vector2(70, 16), selectClass, Color.White);

            sp.End();
        }

        protected void SelectPlayer(int type)
        {
            gMan.ChangeMenuState(null);
            if (gMan.game.started == false) gMan.game.Initialize(type);
        }
    }
}
