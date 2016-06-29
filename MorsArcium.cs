using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System;

namespace Mors_Arcium
{
    public class MorsArcium : Game
    {
        //Android Controls
        //Water, more background stuff, rain?
        //Difficulty Modes
        //Wave Events
        public Keys UP = Keys.W;
        public Keys DOWN = Keys.S;
        public Keys RIGHT = Keys.D;
        public Keys LEFT = Keys.A;
        public Keys JUMP = Keys.Space;
        public Keys ATTACK = Keys.J;
        public Keys SPECIAL = Keys.K;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;

        public Gameplay game;
        public Menu currentMenu = null;
        private Menu nextMenu = null;
        private bool transition = false;
        private float fade = 1.0f;
        private bool fadeIn = false;

        public Texture2D[] textures;
        public SoundEffect[] sounds;
        public SpriteFont font1;

        public KeyboardState prevState;

        public Random random;

        public float scaleFactor = 1.0f;

        public bool pause = false;
        private bool skip = false;
        private bool henry = false;
        private bool grecc = false;
        Rectangle thing;
        public MorsArcium()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();
#if WINDOWS
            Window.Title = "MORS ARCIUM";
            Window.Position = new Point(64, 64);
            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            IsMouseVisible = true;
#endif
            
            scaleFactor = GraphicsDevice.Viewport.Height / 240f;
            thing = new Rectangle((int)(GraphicsDevice.Viewport.Width - (320 * scaleFactor)) / 2, 0, (int)(320 * scaleFactor), (int)(240 * scaleFactor));
            renderTarget = new RenderTarget2D(GraphicsDevice, 320, 240);
        }
        private void LoadTexture(string path, int index)
        {
            textures[index] = Content.Load<Texture2D>("textures/" + Path.GetFileNameWithoutExtension(path));
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font1 = Content.Load<SpriteFont>("Font1");

            textures = new Texture2D[512];

            LoadTexture("Content/textures/characters.png", 0);
            LoadTexture("Content/textures/sky.png", 1);
            LoadTexture("Content/textures/hud.png", 2);        
            LoadTexture("Content/textures/projectiles.png", 3);   
            LoadTexture("Content/textures/tileset.png", 5);    
            LoadTexture("Content/textures/hitbox.png", 6);    
            LoadTexture("Content/textures/particles.png", 7);  
            LoadTexture("Content/textures/misc.png", 8); 

            random = new Random(DateTime.Now.Millisecond);
            game = new Gameplay(this);
            currentMenu = new MainMenu(this);
            //game.Initialize();
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
#if DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.P)) pause = true;
            if (Keyboard.GetState().IsKeyDown(Keys.O) && !henry) skip = true;
            if (Keyboard.GetState().IsKeyDown(Keys.I)) pause = false;
            henry = Keyboard.GetState().IsKeyDown(Keys.O);
#endif
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !grecc)
            {
                pause = !pause;
            }
            grecc = Keyboard.GetState().IsKeyDown(Keys.Enter);
            
            if (!pause || skip)
            {
                if (transition)
                {
                    if (!fadeIn)
                    {
                        fade -= 0.1f;
                        if (fade <= 0.0f)
                        {
                            fade = 0.0f;
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
                            transition = false;
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
                skip = false;
            }
            prevState = Keyboard.GetState();
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.DarkBlue);
            if (currentMenu != null)
            {
                currentMenu.Draw(spriteBatch);
            }
            else
            {
                game.Draw(spriteBatch);
            }
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, null, null, null);
            spriteBatch.Draw(renderTarget, thing, Color.White * fade);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public static float WeightedAverage(float x2, float x, float x1, float Q11, float Q21)
        {
            return ((x2 - x) / (x2 - x1)) * Q11 + ((x - x1) / (x2 - x1)) * Q21;
        }
        public void ChangeMenuState(Menu men)
        {
            nextMenu = men;
            transition = true;
        }
    }
}
