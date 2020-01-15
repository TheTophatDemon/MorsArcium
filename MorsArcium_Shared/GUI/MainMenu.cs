using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium.GUI
{
    public class MainMenu : Menu
    {
        private static readonly Rectangle titleRect = new Rectangle(272, 0, 192, 120);

        float timer = 0.0f;
        public MainMenu(GameManager gMan) : base(gMan)
        {
            if (gMan.game.started)
            {
                //Resume game button
                buttons.Add(new Button(gMan, gMan.textures["hud"], new Rectangle(0, 200, 128, 24), new Vector2(96, 88), 
                    (Button b) => {
                        gMan.ChangeMenuState(null);
                    }));
            }

            //Start game button
            buttons.Add(new Button(gMan, gMan.textures["hud"], new Rectangle(0, 128, 128, 24), new Vector2(96, 128),
                (Button b) => {
                    if (!gMan.platform.GameSettings.playedBefore)
                    {
                        gMan.ChangeMenuState(new DisclaimerMenu(gMan));
                    }
                    else
                    {
                        gMan.ChangeMenuState(new DifficultyMenu(gMan));
                        gMan.game.tutorial = false;
                    }
                }));

            //Tutorial button
            buttons.Add(new Button(gMan, gMan.textures["hud"], new Rectangle(0, 152, 128, 24), new Vector2(96, 168),
                (Button b) => {
                    gMan.ChangeMenuState(null);
                    gMan.game.tutorial = true;
                    gMan.game.Initialize(0);
                }));

            //Options button
            buttons.Add(new Button(gMan, gMan.textures["hud"], new Rectangle(0, 176, 128, 24), new Vector2(96, 208),
                (Button b) => {
                    gMan.ChangeMenuState(new OptionsMenu(gMan));
                }));

            //Credits button
            buttons.Add(new Button(gMan, gMan.textures["hud"], new Rectangle(384, 176, 64, 24), new Vector2(16, 208),
                (Button b) => {
                    gMan.ChangeMenuState(new CreditsMenu(gMan));
                }));
        }

        public override void Update(GameTime g)
        {
            base.Update(g);

            timer += g.ElapsedGameTime.Milliseconds / 1000.0f;
            
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) gMan.platform.Exit();
        }

        public override void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);

            DrawTrippyBackground(sp);
            sp.Draw(gMan.textures["hud"], new Vector2(64, 4 + (float)Math.Sin(timer) * 4.0f), titleRect, Color.White);
            DrawButtons(sp);

            sp.End();
        }
    }
}
