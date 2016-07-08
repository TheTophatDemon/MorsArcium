using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium
{
    public class MainMenu : Menu
    {
        Rectangle titleRect = new Rectangle(272, 0, 192, 120);
        float floaty = 0.0f;
        Vector2 backgroundPosition;
        Color backgroundColor = Color.Gray;
        public MainMenu(MorsArcium g) : base(g)
        {
            if (g.game.started)
            {
                buttons = new Button[5];
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
                buttons[4].position = new Vector2(16, 208);
                buttons[4].source = new Rectangle(384, 176, 64, 24);
            }
            else
            {
                buttons = new Button[4];
                buttons[0].position = new Vector2(96, 128);
                buttons[0].source = new Rectangle(0, 128, 128, 24);
                buttons[0].function = StartGame;
                buttons[1].position = new Vector2(96, 168);
                buttons[1].source = new Rectangle(0, 152, 128, 24);
                buttons[1].function = GotoTutorial;
                buttons[2].position = new Vector2(96, 208);
                buttons[2].source = new Rectangle(0, 176, 128, 24);
                buttons[2].function = GotoOptions;
                buttons[3].position = new Vector2(16, 208);
                buttons[3].source = new Rectangle(384, 176, 64, 24);
            }
            backgroundPosition = Vector2.Zero;
            backgroundColor = new Color(32, 32, 32);
        }
        public override void Update(GameTime g)
        {
            base.Update(g);
            floaty += 0.05f;
            byte b = (byte)Math.Round(Math.Sin(floaty * 0.5f));
            backgroundColor.R += b;
            backgroundColor.G += b;
            backgroundColor.B += b;
            backgroundPosition.X -= 1.0f;
            backgroundPosition.Y -= 1.0f;
            if (backgroundPosition.X < -320.0f) backgroundPosition.X = 0.0f;
            if (backgroundPosition.Y < -240.0f) backgroundPosition.Y = 0.0f;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) game.Exit();
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);
            sp.Draw(game.textures[1], backgroundPosition, backgroundColor);
            sp.Draw(game.textures[1], backgroundPosition + new Vector2(320, 0), backgroundColor);
            sp.Draw(game.textures[1], backgroundPosition + new Vector2(320, 240), backgroundColor);
            sp.Draw(game.textures[1], backgroundPosition + new Vector2(0, 240), backgroundColor);
            sp.Draw(game.textures[2], new Vector2(64, 4 + (float)Math.Sin(floaty) * 4.0f), titleRect, Color.White);
            base.Draw(sp);
            sp.End();
        }
        private void StartGame()
        {
            game.ChangeMenuState(new ClassMenu(game));
        }
        private void ResumeGame()
        {
            game.ChangeMenuState(null);
        }
        private void GotoTutorial()
        {
            Console.WriteLine("VIDEO TUTORALS!?!?!!?!");
        }
        private void GotoOptions()
        {
            game.ChangeMenuState(new OptionsMenu(game));
        }
        public override void OnButtonPress(Button source)
        {
            if (source.source.X == 384)
            {
                game.ChangeMenuState(new CreditsMenu(game));
            }
        }
    }
}
