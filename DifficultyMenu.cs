using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium
{
    public class DifficultyMenu : Menu
    {
        float floaty = 0.0f;
        Vector2 backgroundPosition;
        Color backgroundColor = Color.Gray;
        Rectangle ass = new Rectangle(0, 432, 215, 25);
        public DifficultyMenu(MorsArcium g) : base(g)
        {
            buttons = new Button[4];
            buttons[0].source = new Rectangle(128, 0, 128, 24); //Easy
            buttons[0].position = new Vector2(96, 144);
            buttons[1].source = new Rectangle(128, 24, 128, 24); //Medium
            buttons[1].position = new Vector2(96, 112);
            buttons[2].source = new Rectangle(128, 48, 128, 24); //Hard
            buttons[2].position = new Vector2(96, 80);
            buttons[3].source = new Rectangle(128, 72, 128, 24); //????
            buttons[3].position = new Vector2(96, 176);
            
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
            base.Draw(sp);
            sp.Draw(game.textures[2], new Vector2(53, 20), ass, Color.White);
            sp.End();
        }
        public override void OnButtonPress(Button source)
        {
            base.OnButtonPress(source);
            if (source.position == buttons[0].position)
            {
                game.game.difficulty = "easy";
            }
            else if (source.position == buttons[1].position)
            {
                game.game.difficulty = "normal";
            }
            else if (source.position == buttons[2].position)
            {
                game.game.difficulty = "hard";
            }
            else if (source.position == buttons[3].position)
            {
                game.game.difficulty = "????";
            }
            game.ChangeMenuState(new ClassMenu(game));
        }
    }
}
