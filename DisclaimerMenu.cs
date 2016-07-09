using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium
{
    public class DisclaimerMenu : Menu
    {
        float f = 0.0f;
        Vector2 backgroundPosition;
        Color backgroundColor = Color.Gray;
        public DisclaimerMenu(MorsArcium g) : base(g)
        {
            buttons = new Button[2];
            buttons[0].source = new Rectangle(0, 152, 128, 24); //How 2 play
            buttons[0].position = new Vector2(16, 200);
            buttons[1].source = new Rectangle(0, 128, 128, 24); // Start game
            buttons[1].position = new Vector2(176, 200);
        }
        public override void Update(GameTime g)
        {
            base.Update(g);
            f += 0.05f;
            byte b = (byte)Math.Round(Math.Sin(f * 0.5f));
            backgroundColor.R += b;
            backgroundColor.G += b;
            backgroundColor.B += b;
            backgroundPosition.X -= 1.0f;
            backgroundPosition.Y -= 1.0f;
            if (backgroundPosition.X < -320.0f) backgroundPosition.X = 0.0f;
            if (backgroundPosition.Y < -240.0f) backgroundPosition.Y = 0.0f;
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);
            sp.Draw(game.textures[1], backgroundPosition, backgroundColor);
            sp.Draw(game.textures[1], backgroundPosition + new Vector2(320, 0), backgroundColor);
            sp.Draw(game.textures[1], backgroundPosition + new Vector2(320, 240), backgroundColor);
            sp.Draw(game.textures[1], backgroundPosition + new Vector2(0, 240), backgroundColor);
            base.Draw(sp);
            sp.DrawString(game.font1, "IF YOU DO NOT WANT YOUR BUTT\nHANDED TO YOU ON A SILVER PLATTER, \nYOU SHOULD CLICK ON THAT \n'HOW TO PLAY' BUTTON.\n\nESPECIALLY YOU, JACOB.", new Vector2(8, 8), Color.White);
            sp.End();
        }
        public override void OnButtonPress(Button source)
        {
            if (source.position == buttons[0].position)
            {
                game.ChangeMenuState(null);
                game.game.tutorial = true;
                game.game.Initialize(0);
            }
            else
            {
                game.ChangeMenuState(new DifficultyMenu(game));
                game.game.tutorial = false;
            }
        }
    }
}
