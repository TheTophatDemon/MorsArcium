using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium
{
    public class ClassMenu : Menu
    {
        Rectangle goldFrame = new Rectangle(64, 320, 96, 80);
        Rectangle selectClass = new Rectangle(0, 416, 180, 16);
        Rectangle ech = new Rectangle(0, 65, 1, 1);
        float f = 0.0f;
        Vector2 backgroundPosition;
        Color backgroundColor = Color.Gray;
        public ClassMenu(MorsArcium g) : base(g)
        {
            buttons = new Button[4];
            buttons[0].source = new Rectangle(160, 128, 96, 96);
            buttons[0].position = new Vector2(56, 40);
            buttons[0].function = SelectPlayer;
            buttons[1].source = new Rectangle(160, 224, 96, 96);
            buttons[1].position = new Vector2(168, 40);
            buttons[1].function = SelectPlayer;
            buttons[2].source = new Rectangle(64, 224, 96, 96);
            buttons[2].position = new Vector2(56, 136);
            buttons[2].function = SelectPlayer;
            buttons[3].source = new Rectangle(160, 320, 96, 96);
            buttons[3].position = new Vector2(168, 136);
            buttons[3].function = SelectPlayer;
            game.game.started = false;
        }
        public override void Update(GameTime g)
        {
            base.Update(g);
            f += 0.025f;
            byte b = (byte)Math.Round(Math.Sin(f));
            backgroundColor.R += b;
            backgroundColor.G += b;
            backgroundColor.B += b;
            backgroundPosition.X -= 1.0f;
            backgroundPosition.Y -= 1.0f;
            if (backgroundPosition.X < -320.0f) backgroundPosition.X = 0.0f;
            if (backgroundPosition.Y < -240.0f) backgroundPosition.Y = 0.0f;
        }
        public override void DrawExtra(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);
            sp.Draw(game.textures[1], backgroundPosition, backgroundColor);
            sp.Draw(game.textures[1], backgroundPosition + new Vector2(320, 0), backgroundColor);
            sp.Draw(game.textures[1], backgroundPosition + new Vector2(320, 240), backgroundColor);
            sp.Draw(game.textures[1], backgroundPosition + new Vector2(0, 240), backgroundColor);
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].hover)
                {
                    sp.Draw(game.textures[2], buttons[i].position, buttons[i].source, Color.White);
                    sp.Draw(game.textures[2], buttons[i].position, goldFrame, Color.White);
                }
                else
                {
                    sp.Draw(game.textures[2], buttons[i].position, buttons[i].source, Color.White);
                }
            }
            sp.Draw(game.textures[2], new Vector2(70, 16), selectClass, Color.White);
            sp.End();
        }
        private void SelectPlayer()
        {
            for (int i = 0; i < 4; i++)
            {
                if (buttons[i].hover)
                {
                    game.ChangeMenuState(null);
                    if (game.game.started == false) game.game.Initialize(i);
                    break;
                }
            }
        }
    }
}
