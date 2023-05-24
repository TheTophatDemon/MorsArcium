/*
Copyright (C) 2016-present Alexander Lunsford

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Mors_Arcium
{
    /// <summary>
    /// Handles transitions between menus and the game.
    /// Provides access to global subsystems.
    /// (For now) Contains all texture and font assets
    /// </summary>
    public class GameManager
    {
        private static readonly Rectangle FADE_RECT = new Rectangle(496, 0, 16, 16);

        SpriteBatch spriteBatch;

        public Gameplay game;
        public GUI.Menu currentMenu = null;
        private GUI.Menu nextMenu = null;
        private bool menuTransition = false;
        public float menuFadeAlpha = 0.0f;
        private bool menuFadingIn = false;

        public Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        public Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
        public Random random;

        public IPlatformOutlet platform;
        public AudioSystem audio;
        
        public GameManager(IPlatformOutlet platform)
        {
            this.platform = platform;
            audio = new AudioSystem(platform);
        }

        private void LoadTexture(ContentManager content, string name)
        {
            textures.Add(name, content.Load<Texture2D>("textures/" + name));
        }

        private void LoadFont(ContentManager content, string name)
        {
            fonts.Add(name, content.Load<SpriteFont>(name));
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);

            LoadFont(content, "default");

            LoadTexture(content, "characters");
            LoadTexture(content, "sky");
            LoadTexture(content, "hud");        
            LoadTexture(content, "projectiles");
            LoadTexture(content, "credits");
            LoadTexture(content, "tileset");
            LoadTexture(content, "hitbox");    
            LoadTexture(content, "particles");  
            LoadTexture(content, "misc");

            audio.LoadContent(content);

            random = new Random(DateTime.Now.Millisecond);
            game = new Gameplay(this);
            currentMenu = new GUI.MainMenu(this);
        }

        public void Update(GameTime gameTime)
        {
            audio.Update(gameTime);

            if (menuTransition)
            {
                if (!menuFadingIn)
                {
                    menuFadeAlpha += 0.1f;
                    if (menuFadeAlpha >= 1.0f)
                    {
                        menuFadeAlpha = 1.0f;
                        currentMenu?.OnLeave();
                        nextMenu?.OnEnter();
                        currentMenu = nextMenu;
                        nextMenu = null;
                        menuFadingIn = true;
                    }
                }
                else
                {
                    menuFadeAlpha -= 0.1f;
                    if (menuFadeAlpha < 0.0f)
                    {
                        menuFadeAlpha = 0.0f;
                        menuTransition = false;
                        menuFadingIn = false;
                    }
                }
            }

            if (currentMenu != null)
            {
                currentMenu.Update(gameTime);
            }
            else
            {
                game.Update(gameTime);
            }
        }
        
        public void Draw(GraphicsDevice device, GameTime gameTime)
        {
            device.Clear(Color.DarkBlue);
            if (currentMenu != null)
            {
                currentMenu.Draw(spriteBatch);
            }
            else
            {
                game.Draw(spriteBatch);
            }

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(textures["hud"], device.Viewport.Bounds, FADE_RECT, new Color(Color.White, menuFadeAlpha));
            spriteBatch.End();
        }

        public static float WeightedAverage(float x2, float x, float x1, float Q11, float Q21)
        {
            return ((x2 - x) / (x2 - x1)) * Q11 + ((x - x1) / (x2 - x1)) * Q21;
        }

        public void ChangeMenuState(GUI.Menu men)
        {
            nextMenu = men;
            menuTransition = true;
        }
    }
}
