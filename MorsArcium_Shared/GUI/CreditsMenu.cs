using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium.GUI
{
    public class CreditsMenu : Menu
    {
        public CreditsMenu(GameManager gMan) : base(gMan)
        {
            //Back button
            buttons.Add(new Button(gMan, gMan.textures[2], new Rectangle(384, 152, 128, 24), new Vector2(16, 16),
                (Button b) => {
                    gMan.ChangeMenuState(new MainMenu(gMan));
                }));
            
            gMan.audio.ChangeMusic("tehcrankles");
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);

            sp.Draw(gMan.textures[4], Vector2.Zero, Color.White);
            DrawButtons(sp);

            sp.End();
        }
    }
}
