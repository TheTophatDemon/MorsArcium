using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System;
using Microsoft.Xna.Framework.Media;

namespace Mors_Arcium
{
    public class MorsArcium : Game
    {
        //Android Controls
        //Difficulty Modes
        //Tutorial
        //Nerf Bug and Eli
        //Credits
        //Sounds (Add Health pickup, slot machine noises)
        public Keys UP = Keys.W;
        public Keys DOWN = Keys.S;
        public Keys RIGHT = Keys.D;
        public Keys LEFT = Keys.A;
        public Keys JUMP = Keys.Space;
        public Keys ATTACK = Keys.J;
        public Keys SPECIAL = Keys.K;
        public Keys PAUSE = Keys.Enter;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;

        public Gameplay game;
        public Menu currentMenu = null;
        private Menu nextMenu = null;
        private bool menutransition = false;
        private float fade = 1.0f;
        private bool fadeIn = false;

        public Texture2D[] textures;
        public Song[] music;
        public SoundEffect[] sounds;
        public SpriteFont font1;

        public Song currentMusic;
        private Song nextMusic;
        private bool musictransition = false;
        private float eeeeearnis = 1.0f;

        public KeyboardState prevState;

        public Random random;
        public bool musicEnabled = true;
        public bool soundEnabled = true;
        public bool fullscreen = false;
        public bool bugJumpFly = true;

        public float scaleFactor = 1.0f;

        public bool paused = false;
        private bool skip = false;
        private bool henry = false;
        private bool grecc = false;
        public Rectangle thing;
        MediaState prevMedState;
        public MorsArcium()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            LoadSettings();
        }
        protected override void Initialize()
        {
            base.Initialize();
#if WINDOWS
            Window.Title = "MORS ARCIUM";
            Window.Position = new Point(0, 0);
            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            IsMouseVisible = true;
#endif
            
            scaleFactor = GraphicsDevice.Viewport.Height / 240f;
            thing = new Rectangle((int)(GraphicsDevice.Viewport.Width - (320 * scaleFactor)) / 2, 0, (int)(320 * scaleFactor), (int)(240 * scaleFactor));
            renderTarget = new RenderTarget2D(GraphicsDevice, 320, 240);
            MediaPlayer.IsRepeating = true;
        }
        private void LoadTexture(string path, int index)
        {
            textures[index] = Content.Load<Texture2D>("textures/" + Path.GetFileNameWithoutExtension(path));
        }
        public void ChangeMusic(int i)
        {
            if (i != 15)
            {
                nextMusic = music[i];
            }
            else
            {
                nextMusic = null;
            }
            musictransition = true;
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

            music = new Song[16];
            music[0] = Content.Load<Song>("AsinosFacio");
            music[1] = Content.Load<Song>("frozenhell");
            music[2] = Content.Load<Song>("gasconade");
            music[3] = Content.Load<Song>("unholywars");

            sounds = new SoundEffect[32];
            sounds[0] = Content.Load<SoundEffect>("sounds/die");
            sounds[1] = Content.Load<SoundEffect>("sounds/explosion");
            sounds[2] = Content.Load<SoundEffect>("sounds/freeze");
            sounds[3] = Content.Load<SoundEffect>("sounds/jump");
            sounds[4] = Content.Load<SoundEffect>("sounds/land");
            sounds[5] = Content.Load<SoundEffect>("sounds/mrb_teleport");
            sounds[6] = Content.Load<SoundEffect>("sounds/throw");
            sounds[7] = Content.Load<SoundEffect>("sounds/wizard_blast");
            sounds[8] = Content.Load<SoundEffect>("sounds/wizard_fusdorah");

            random = new Random(DateTime.Now.Millisecond);
            game = new Gameplay(this);
            currentMenu = new MainMenu(this);
            //game.Initialize();
        }
        protected override void UnloadContent()
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i] != null) sounds[i].Dispose();
            }
            for (int i = 0; i < textures.Length; i++)
            {
                if (textures[i] != null) textures[i].Dispose();
            }
            for (int i = 0; i < music.Length; i++)
            {
                if (music[i] != null) music[i].Dispose();
            }
        }
        public void ToggleFullscreen()
        {
            Window.Position = new Point(0, 0);
            if (fullscreen)
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                graphics.PreferredBackBufferWidth = 960;
                graphics.PreferredBackBufferHeight = 720;
            }
            graphics.ApplyChanges();
            graphics.ToggleFullScreen();
            scaleFactor = GraphicsDevice.Viewport.Height / 240f;
            thing = new Rectangle((int)(GraphicsDevice.Viewport.Width - (320 * scaleFactor)) / 2, 0, (int)(320 * scaleFactor), (int)(240 * scaleFactor));
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
#if DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.P)) paused = true;
            if (Keyboard.GetState().IsKeyDown(Keys.O) && !henry) skip = true;
            if (Keyboard.GetState().IsKeyDown(Keys.I)) paused = false;
            henry = Keyboard.GetState().IsKeyDown(Keys.O);
#endif

            if (musictransition)
            {
                eeeeearnis -= 0.01f;
                if (eeeeearnis <= 0.0f)
                {
                    eeeeearnis = 1.0f;
                    musictransition = false;
                    if (currentMusic != null)
                    {
                        MediaPlayer.Stop();
                        //currentMusic.Dispose();
                    }
                    currentMusic = nextMusic;
                    if (currentMusic != null)
                    {
                        MediaPlayer.Play(currentMusic);
                        
                    }
                    nextMusic = null;
                }
            }
            else if (eeeeearnis < 1.0f)
            {
                //eeeeearnis += 0.1f;
                //if (eeeeearnis > 1.0f) eeeeearnis = 1.0f;
            }
            if (currentMusic != null)
            {
                if (!musicEnabled)
                {
                    MediaPlayer.Volume = 0.0f;
                }
                else
                {
                    MediaPlayer.Volume = eeeeearnis;
                }
            }
            if (Keyboard.GetState().IsKeyDown(PAUSE) && !grecc)
            {
                paused = !paused;
            }
            grecc = Keyboard.GetState().IsKeyDown(PAUSE);
            if (!paused || skip)
            {
                if (menutransition)
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
                skip = false;
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    paused = false;
                }
            }
            prevState = Keyboard.GetState();
            prevMedState = MediaPlayer.State;
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
            menutransition = true;
        }
        public void SaveSettings()
        {
#if WINDOWS
            StreamWriter chrick = new StreamWriter("settings.txt");
            chrick.WriteLine(fullscreen.ToString());
            chrick.WriteLine(soundEnabled.ToString());
            chrick.WriteLine(musicEnabled.ToString());
            chrick.WriteLine(bugJumpFly.ToString());
            chrick.WriteLine((int)UP);
            chrick.WriteLine((int)DOWN);
            chrick.WriteLine((int)LEFT);
            chrick.WriteLine((int)RIGHT);
            chrick.WriteLine((int)JUMP);
            chrick.WriteLine((int)ATTACK);
            chrick.WriteLine((int)SPECIAL);
            chrick.WriteLine((int)PAUSE);
            chrick.Close();
            chrick.Dispose();
#endif
        }
        public void LoadSettings()
        {
#if WINDOWS
            try
            {
                StreamReader asgore = new StreamReader("settings.txt");
                fullscreen = bool.Parse(asgore.ReadLine());
                soundEnabled = bool.Parse(asgore.ReadLine());
                musicEnabled = bool.Parse(asgore.ReadLine());
                bugJumpFly = bool.Parse(asgore.ReadLine());
                UP = (Keys)int.Parse(asgore.ReadLine());
                DOWN = (Keys)int.Parse(asgore.ReadLine());
                LEFT = (Keys)int.Parse(asgore.ReadLine());
                RIGHT = (Keys)int.Parse(asgore.ReadLine());
                JUMP = (Keys)int.Parse(asgore.ReadLine());
                ATTACK = (Keys)int.Parse(asgore.ReadLine());
                SPECIAL = (Keys)int.Parse(asgore.ReadLine());
                PAUSE = (Keys)int.Parse(asgore.ReadLine());
                asgore.Close();
                asgore.Dispose();
            }
            catch
            {
                Console.WriteLine("Well darn! It's not there!");
            }
#endif
        }
    }
}
