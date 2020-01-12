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
            buttons.Add(new Button(gMan, gMan.textures[2], new Rectangle(128, 0, 128, 24), new Vector2(96, 144),
                (Button b) => {
                    gMan.game.difficulty = "easy";
                    gMan.ChangeMenuState(new ClassMenu(gMan));
                }));

            //Medium button
            buttons.Add(new Button(gMan, gMan.textures[2], new Rectangle(128, 24, 128, 24), new Vector2(96, 112), 
                (Button b) => {
                    gMan.game.difficulty = "normal";
                    gMan.ChangeMenuState(new ClassMenu(gMan));
                }));

            //Hard button
            buttons.Add(new Button(gMan, gMan.textures[2], new Rectangle(128, 48, 128, 24), new Vector2(96, 80),
                (Button b) => {
                    gMan.game.difficulty = "hard";
                    gMan.ChangeMenuState(new ClassMenu(gMan));
                }));

            //Random difficulty
            buttons.Add(new Button(gMan, gMan.textures[2], new Rectangle(128, 72, 128, 24), new Vector2(96, 176),
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

            sp.Draw(gMan.textures[2], new Vector2(53, 20), textRect, Color.White);

            sp.End();
        }
    }
}
