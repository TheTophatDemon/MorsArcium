﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium
{
    public class ClassMenu : Menu
    {
        Rectangle goldFrame = new Rectangle(64, 320, 96, 80);
        public ClassMenu(MorsArcium g) : base(g)
        {
            buttons = new Button[4];
            buttons[0].source = new Rectangle(160, 128, 96, 96);
            buttons[0].position = new Vector2(56, 40);
            buttons[0].function = StartGame;
            buttons[1].source = new Rectangle(160, 224, 96, 96);
            buttons[1].position = new Vector2(168, 40);
            buttons[1].function = StartGame;
            buttons[2].source = new Rectangle(64, 224, 96, 96);
            buttons[2].position = new Vector2(56, 136);
            buttons[2].function = StartGame;
            buttons[3].source = new Rectangle(160, 320, 96, 96);
            buttons[3].position = new Vector2(168, 136);
            buttons[3].function = StartGame;
        }
        public override void Update(GameTime g)
        {
            base.Update(g);
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);
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
            sp.End();
        }
        private void StartGame()
        {
            game.ChangeMenuState(null);
            for (int i = 0; i < 4; i++)
            {
                if (buttons[i].hover)
                {
                    game.game.Initialize(i);
                }
            }
        }
    }
}
