using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium
{
    public class MainMenu : Menu
    {
        public MainMenu(MorsArcium g) : base(g)
        {
            if (g.game.started)
            {
                buttons = new Button[4];
                buttons[0].position = new Vector2(96, 88);
                buttons[0].source = new Rectangle(0, 200, 128, 24);
                buttons[0].function = ResumeGame;
                buttons[1].position = new Vector2(96, 128);
                buttons[1].source = new Rectangle(0, 128, 128, 24);
                buttons[1].function = StartGame;
                buttons[2].position = new Vector2(96, 168);
                buttons[2].source = new Rectangle(0, 152, 128, 24);
                buttons[2].function = GotoTutorial;
                buttons[3].position = new Vector2(96, 208);
                buttons[3].source = new Rectangle(0, 176, 128, 24);
                buttons[3].function = GotoOptions;
            }
            else
            {
                buttons = new Button[3];
                buttons[0].position = new Vector2(96, 128);
                buttons[0].source = new Rectangle(0, 128, 128, 24);
                buttons[0].function = StartGame;
                buttons[1].position = new Vector2(96, 168);
                buttons[1].source = new Rectangle(0, 152, 128, 24);
                buttons[1].function = GotoTutorial;
                buttons[2].position = new Vector2(96, 208);
                buttons[2].source = new Rectangle(0, 176, 128, 24);
                buttons[2].function = GotoOptions;
            }
        }
        public override void Update(GameTime g)
        {
            base.Update(g);
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);
            base.Draw(sp);
            sp.End();
        }
        private void StartGame()
        {
            game.currentMenu = null;
            game.game.Initialize();
        }
        private void ResumeGame()
        {
            game.currentMenu = null;
        }
        private void GotoTutorial()
        {
            Console.WriteLine("VIDEO TUTORALS!?!?!!?!");
        }
        private void GotoOptions()
        {
            Console.WriteLine("THE ONLY OPTION IS DEATH!!!!");
        }
    }
}
