﻿using System;
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
        float floaty = 0.0f;
        Vector2 backgroundPosition;
        Color backgroundColor = Color.Gray;
        public OptionsMenu(GameManager g) : base(g)
        {
            buttons = new MenuButton[7];
            buttons[0].source = new Rectangle(384, 152, 128, 24); //Back button
            buttons[0].position = new Vector2(16, 16);
#if WINDOWS
            buttons[1].source = new Rectangle(Convert.ToInt32(Settings.fullScreen) * 32, 320, 32, 32); //Fullscreen checkbox
            buttons[1].position = new Vector2(16, 60);
#endif
            buttons[2].source = new Rectangle(Convert.ToInt32(Settings.soundEnabled) * 32, 320, 32, 32); //Sound checkbox
            buttons[2].position = new Vector2(16, 92);
            buttons[3].source = new Rectangle(Convert.ToInt32(Settings.musicEnabled) * 32, 320, 32, 32); //Music checkbox
            buttons[3].position = new Vector2(16, 124);
            buttons[4].source = new Rectangle(Convert.ToInt32(Settings.jumpFly) * 32, 320, 32, 32); //Bug Jumping
            buttons[4].position = new Vector2(16, 156);
            buttons[6].source = new Rectangle(Convert.ToInt32(Settings.vSync) * 32, 320, 32, 32); //Vsync
            buttons[6].position = new Vector2(160, 124);
#if WINDOWS
            buttons[5].source = new Rectangle(384, 128, 128, 24); //Rebind keys
            buttons[5].position = new Vector2(16, 200);
#endif
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
        }
        public override void Draw(SpriteBatch sp)
        {
            base.Draw(sp);
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);
            sp.Draw(game.textures[1], backgroundPosition, backgroundColor);
            sp.Draw(game.textures[1], backgroundPosition + new Vector2(320, 0), backgroundColor);
            sp.Draw(game.textures[1], backgroundPosition + new Vector2(320, 240), backgroundColor);
            sp.Draw(game.textures[1], backgroundPosition + new Vector2(0, 240), backgroundColor);
            DrawButtons(sp);
#if WINDOWS
            sp.DrawString(game.font1, "FULLSCREEN", new Vector2(52, 68), Color.White);
#endif
            sp.DrawString(game.font1, "SOUND", new Vector2(52, 100), Color.White);
            sp.DrawString(game.font1, "MUSIC", new Vector2(52, 132), Color.White);
            sp.DrawString(game.font1, "FLY BY JUMPING IN MIDAIR", new Vector2(52, 164), Color.White);
            sp.DrawString(game.font1, "VSYNC", new Vector2(196, 132), Color.White);
            sp.End();
        }
        public override void OnButtonPress(MenuButton source)
        {
            if (source.position == buttons[0].position)
            {
#if !DEBUG
                game.SaveSettings();
#endif
                game.ChangeMenuState(new MainMenu(game));
            }
            else if (source.position == buttons[1].position)
            {
                Settings.fullScreen = !Settings.fullScreen;
                //game.ToggleFullscreen();
                if (Settings.fullScreen)
                {
                    buttons[1].source.X = 32;
                }
                else
                {
                    buttons[1].source.X = 0;
                }
            }
            else if (source.position == buttons[2].position)
            {
                Settings.soundEnabled = !Settings.soundEnabled;
                if (Settings.soundEnabled)
                {
                    buttons[2].source.X = 32;
                }
                else
                {
                    buttons[2].source.X = 0;
                }
            }
            else if (source.position == buttons[3].position)
            {
                Settings.musicEnabled = !Settings.musicEnabled;
                if (Settings.musicEnabled)
                {
                    buttons[3].source.X = 32;
                }
                else
                {
                    buttons[3].source.X = 0;
                }
            }
            else if (source.position == buttons[4].position)
            {
                Settings.jumpFly = !Settings.jumpFly;
                if (Settings.jumpFly)
                {
                    buttons[4].source.X = 32;
                }
                else
                {
                    buttons[4].source.X = 0;
                }
            }
            else if (source.position == buttons[5].position)
            {
                game.ChangeMenuState(new RebindMenu(game));
            }
            else if (source.position == buttons[6].position)
            {
                //game.ToggleVsync();
                if (Settings.vSync)
                {
                    buttons[6].source.X = 32;
                }
                else
                {
                    buttons[6].source.X = 0;
                }
            }
        }
    }
}