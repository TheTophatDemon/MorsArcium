using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System;
using Microsoft.Xna.Framework.Media;

namespace Mors_Arcium
{
    /// <summary>
    /// Handles transitions between menus and the game.
    /// Provides access to global subsystems.
    /// (For now) Contains all texture and font assets
    /// </summary>
    public class GameManager
    {
        //TODO: Ensure 64 bit & Android API Level 28
        //TODO: Fix player count bug
        //TODO: Reimplement saving and loading of settings
        //TODO: Refine resource loading system
        //TODO: Separate GUI class
        //TODO: Optimize GUI elements
        //TODO: Re-Implement fade
        //TODO: Redo class select menu
        //TODO: Restructure player class
        //TODO: Make enums for player & entity types
        //TODO: Make resolution dynamic
        //TODO: Make dynamic timing
        //TODO: Refine tutorial code
        //TODO: Refine Joystick Hat Capability
        //TODO: Add Joystick Axis Capability
        //TODO: Add proper keyboard key names
        //TODO: Polish player class change
        //TODO: Polish new Eli attack
        //TODO: Change "Zero Gravity" to "Low Gravity"
        //TODO: Add crossfading music system
        //TODO: Add pitch variations to sounds
        //TODO: Add Eli slicing sound
        //TODO: Remove automatic teleporting at edges for Mr.B
        //TODO: Polish player spawning
        //TODO: Add Android HUD customization

        SpriteBatch spriteBatch;

        public Gameplay game;
        public GUI.Menu currentMenu = null;
        private GUI.Menu nextMenu = null;
        private bool menutransition = false;
        public float fade = 1.0f;
        private bool fadeIn = false;

        public Texture2D[] textures;
        public SpriteFont font1;
        public Random random;

        public IPlatformOutlet platform;
        public AudioSystem audio;
        
        public GameManager(IPlatformOutlet platform)
        {
            this.platform = platform;
            audio = new AudioSystem(platform);
        }
        
        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            font1 = content.Load<SpriteFont>("Font1");

            textures = new Texture2D[512];

            Action<string, int> LoadTexture = (string path, int index) => {
                textures[index] = content.Load<Texture2D>("textures/" + Path.GetFileNameWithoutExtension(path));
            };
            LoadTexture("Content/textures/characters.png", 0);
            LoadTexture("Content/textures/sky.png", 1);
            LoadTexture("Content/textures/hud.png", 2);        
            LoadTexture("Content/textures/projectiles.png", 3);
            LoadTexture("Content/textures/credits.png", 4);
            LoadTexture("Content/textures/tileset.png", 5);    
            LoadTexture("Content/textures/hitbox.png", 6);    
            LoadTexture("Content/textures/particles.png", 7);  
            LoadTexture("Content/textures/misc.png", 8);

            audio.LoadContent(content);

            random = new Random(DateTime.Now.Millisecond);
            game = new Gameplay(this);
            currentMenu = new GUI.MainMenu(this);
        }

        public void Update(GameTime gameTime)
        {
            audio.Update(gameTime);

            if (menutransition)
            {
                if (!fadeIn)
                {
                    fade -= 0.1f;
                    if (fade <= 0.0f)
                    {
                        fade = 0.0f;
                        currentMenu?.OnLeave();
                        nextMenu?.OnEnter();
                        currentMenu = nextMenu;
                        nextMenu = null;
                        fadeIn = true;
                    }
                }
                else
                {
                    fade += 0.1f;
                    if (fade > 1.0f)
                    {
                        fade = 1.0f;
                        menutransition = false;
                        fadeIn = false;
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
        }

        public static float WeightedAverage(float x2, float x, float x1, float Q11, float Q21)
        {
            return ((x2 - x) / (x2 - x1)) * Q11 + ((x - x1) / (x2 - x1)) * Q21;
        }

        public void ChangeMenuState(GUI.Menu men)
        {
            nextMenu = men;
            menutransition = true;
        }
    }
}
