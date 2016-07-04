using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    //FULLSCREEN
    //SOUND
    //MUSIC
    //DOUBLE TAP JUMP KEY

    public class OptionsMenu : Menu
    {
        public OptionsMenu(MorsArcium g) : base(g)
        {
            buttons = new Button[6];
            buttons[0].source = new Rectangle(384, 152, 128, 24); //Back button
            buttons[0].position = new Vector2(16, 16);
#if WINDOWS
            buttons[1].source = new Rectangle(0, 320, 32, 32); //Fullscreen checkbox
            buttons[1].position = new Vector2(16, 60);
#endif
            buttons[2].source = new Rectangle(0, 320, 32, 32); //Sound checkbox
            buttons[2].position = new Vector2(16, 92);
            buttons[3].source = new Rectangle(0, 320, 32, 32); //Music checkbox
            buttons[3].position = new Vector2(16, 124);
            buttons[4].source = new Rectangle(0, 320, 32, 32); //Bug Jumping
            buttons[4].position = new Vector2(16, 156);
#if WINDOWS
            buttons[5].source = new Rectangle(384, 128, 128, 24); //Rebind keys
            buttons[5].position = new Vector2(16, 200);
#endif
        }
        public override void Update(GameTime g)
        {
            base.Update(g);
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);
            base.Draw(sp);
            sp.DrawString(game.font1, "FULLSCREEN", new Vector2(52, 68), Color.White);
            sp.End();
        }
        public override void OnButtonPress(Button source)
        {
            if (source.position == buttons[0].position)
            {
                game.ChangeMenuState(new MainMenu(game));
            }
            else if (source.position == buttons[1].position)
            {
                game.fullscreen = !game.fullscreen;
                game.ToggleFullscreen();
                if (game.fullscreen)
                {
                    buttons[1].source.X = 32;
                }
                else
                {
                    buttons[1].source.X = 0;
                }
            }
        }
    }
}
