using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium
{
    public class CreditsMenu : Menu
    {
        public CreditsMenu(MorsArcium g) : base(g)
        {
            buttons = new Button[1];
            buttons[0].source = new Rectangle(384, 152, 128, 24); //Back button
            buttons[0].position = new Vector2(16, 16);
            game.ChangeMusic(14);
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);
            sp.Draw(game.textures[4], Vector2.Zero, Color.White);
            base.Draw(sp);
            sp.End();
        }
        public override void OnButtonPress(Button source)
        {
            game.ChangeMenuState(new MainMenu(game));
        }
    }
}
