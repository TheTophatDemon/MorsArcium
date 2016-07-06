using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public class RebindMenu : Menu
    {
        float floaty = 0.0f;
        Vector2 backgroundPosition;
        Color backgroundColor = Color.Gray;
        bool rebinding = false;
        Button undyne;
        public RebindMenu(MorsArcium g) : base(g)
        {
            buttons = new Button[8];
            buttons[0].source = new Rectangle(384, 152, 128, 24); //Back button
            buttons[0].position = new Vector2(16, 16);
            buttons[1].source = new Rectangle(0, 352, 64, 16); //Up
            buttons[1].position = new Vector2(16, 36);
            buttons[2].source = buttons[1].source; //Down
            buttons[2].position = new Vector2(16, 56);
            buttons[3].source = buttons[1].source; //Left
            buttons[3].position = new Vector2(16, 76);
            buttons[4].source = buttons[1].source; //Right
            buttons[4].position = new Vector2(16, 96);
            buttons[5].source = buttons[1].source; //Jump
            buttons[5].position = new Vector2(16, 116);
            buttons[6].source = buttons[1].source; //Attack
            buttons[6].position = new Vector2(16, 136);
            buttons[7].source = buttons[1].source; //Special
            buttons[7].position = new Vector2(16, 156);
            for (int i = 1; i < buttons.Length; i++)
            {
                buttons[i].position.Y += 32;
            }
        }
        public override void OnButtonPress(Button source)
        {
            if (source.position.Y == 16)
            {
                game.ChangeMenuState(new OptionsMenu(game));
            }
            else
            {
                rebinding = true;
                undyne = source;
            }
        }
        public override void Update(GameTime g)
        {
            if (rebinding)
            {
                Keys[] k = Keyboard.GetState().GetPressedKeys();
                if (k.Length > 0)
                {
                    if (undyne.position == buttons[1].position)
                    {
                        game.UP = k[0];
                    }
                    else if (undyne.position == buttons[2].position)
                    {
                        game.DOWN = k[0];
                    }
                    else if (undyne.position == buttons[3].position)
                    {
                        game.LEFT = k[0];
                    }
                    else if (undyne.position == buttons[4].position)
                    {
                        game.RIGHT = k[0];
                    }
                    else if (undyne.position == buttons[5].position)
                    {
                        game.JUMP = k[0];
                    }
                    else if (undyne.position == buttons[6].position)
                    {
                        game.ATTACK = k[0];
                    }
                    else if (undyne.position == buttons[7].position)
                    {
                        game.SPECIAL = k[0];
                    }
                    rebinding = false;
                }
            }
            else
            {
                base.Update(g);
            }
            floaty += 0.05f;
            byte b = (byte)Math.Round(Math.Sin(floaty * 0.5f));
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
            sp.DrawString(game.font1, "AIM UP: " + game.UP.ToString(), buttons[1].position + new Vector2(68, 0), Color.White);
            sp.DrawString(game.font1, "AIM DOWN: " + game.DOWN.ToString(), buttons[2].position + new Vector2(68, 0), Color.White);
            sp.DrawString(game.font1, "WALK LEFT: " + game.LEFT.ToString(), buttons[3].position + new Vector2(68, 0), Color.White);
            sp.DrawString(game.font1, "WALK RIGHT: " + game.RIGHT.ToString(), buttons[4].position + new Vector2(68, 0), Color.White);
            sp.DrawString(game.font1, "JUMP: " + game.JUMP.ToString(), buttons[5].position + new Vector2(68, 0), Color.White);
            sp.DrawString(game.font1, "ATTACK: " + game.ATTACK.ToString(), buttons[6].position + new Vector2(68, 0), Color.White);
            sp.DrawString(game.font1, "SPECIAL ATTACK: " + game.SPECIAL.ToString(), buttons[7].position + new Vector2(68, 0), Color.White);
            sp.End();
        }
    }

}
