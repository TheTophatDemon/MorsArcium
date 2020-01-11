using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium
{
    public class CreditsMenu : Menu
    {
        public CreditsMenu(GameManager g) : base(g)
        {
            buttons = new MenuButton[1];
            buttons[0].source = new Rectangle(384, 152, 128, 24); //Back button
            buttons[0].position = new Vector2(16, 16);
            AudioSystem.ChangeMusic("tehcrankles");
        }
        public override void Draw(SpriteBatch sp)
        {
            base.Draw(sp);
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);
            sp.Draw(game.textures[4], Vector2.Zero, Color.White);
            DrawButtons(sp);
            sp.End();
        }
        public override void OnButtonPress(MenuButton source)
        {
            game.ChangeMenuState(new MainMenu(game));
        }
    }
}
