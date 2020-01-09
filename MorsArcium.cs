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
        //TODO: Optimize GUI elements
        //TODO: Make new rebindings savable to settings
        //TODO: Add Joystick Hat Capability
        //TODO: Add Joystick Axis Capability
        //TODO: Add proper keyboard key names
        //TODO: Add Android HUD customization

        //TODO: Multiplayer
        //TODO: Multiplayer match setup interface
        //TODO: Determine whose PAUSE button does the thing
        //TODO: Fix Event HUD in multiplayer

        public struct PlayerBindings
        {
            public IBinding UP;
            public IBinding DOWN;
            public IBinding RIGHT;
            public IBinding LEFT;
            public IBinding JUMP;
            public IBinding ATTACK;
            public IBinding SPECIAL;
            public IBinding PAUSE;
        }
        public PlayerBindings[] bindings = new PlayerBindings[4];

        public AndroidOutlet android;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Gameplay game;
        public Menu currentMenu = null;
        private Menu nextMenu = null;
        private bool menutransition = false;
        public float fade = 1.0f;
        private bool fadeIn = false;
        public bool vsync = false;
        public Texture2D[] textures;
        public Song[] music;
        public SoundEffect[] sounds;
        public SoundEffectInstance[] soundInstances;
        public SpriteFont font1;

        public Song currentMusic;
        private Song nextMusic;
        private bool musictransition = false;
        private float eeeeearnis = 1.0f;

        public KeyboardState prevState;
        public bool prevJump = false;

        public Random random;
        public bool musicEnabled = true;
        public bool soundEnabled = true;
        public bool fullscreen = false;
        public bool bugJumpFly = true;

        public float scaleFactor = 1.0f;

        public bool paused = false;
        private bool skip = false;
        private bool grecc = false;
        MediaState prevMedState;
        public bool playedBefore = false;
        public MorsArcium(AndroidOutlet a)
        {
            android = a;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            LoadSettings();
        }
        protected override void Initialize()
        {
            base.Initialize();
            Window.Title = "MORS ARCIUM";
            Window.Position = new Point(64, 64);
            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 720;
            IsMouseVisible = true;
            graphics.ApplyChanges();

            scaleFactor = GraphicsDevice.Viewport.Height / 240f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
            graphics.SynchronizeWithVerticalRetrace = vsync;
            graphics.ApplyChanges();
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
        public void ToggleVsync()
        {
            vsync = !vsync;
            graphics.SynchronizeWithVerticalRetrace = vsync;
            graphics.ApplyChanges();
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
            LoadTexture("Content/textures/credits.png", 4);
            LoadTexture("Content/textures/tileset.png", 5);    
            LoadTexture("Content/textures/hitbox.png", 6);    
            LoadTexture("Content/textures/particles.png", 7);  
            LoadTexture("Content/textures/misc.png", 8);

            music = new Song[16];
            music[0] = Content.Load<Song>("AsinosFacio");
            music[1] = Content.Load<Song>("frozenhell");
            music[2] = Content.Load<Song>("gasconade");
            music[3] = Content.Load<Song>("unholywars");
            music[11] = Content.Load<Song>("welcometohell");
            music[12] = Content.Load<Song>("skelesong");
            music[14] = Content.Load<Song>("tehcrankles");

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
            sounds[9] = Content.Load<SoundEffect>("sounds/powerup");
            sounds[10] = Content.Load<SoundEffect>("sounds/slotmachine");
            sounds[11] = Content.Load<SoundEffect>("sounds/hurt");

            soundInstances = new SoundEffectInstance[sounds.Length];
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i] != null)
                {
                    soundInstances[i] = sounds[i].CreateInstance();
                }
            }

            random = new Random(DateTime.Now.Millisecond);
            game = new Gameplay(this);
            currentMenu = new MainMenu(this);
            //game.Initialize();
        }
        protected override void UnloadContent()
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (soundInstances[i] != null) soundInstances[i].Dispose();
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
#if WINDOWS
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
#endif
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
                    eeeeearnis = 0.5f;
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
            if ((bindings[0].PAUSE.IsDown() || android.pause) && !grecc)
            {
                paused = !paused;
            }
            grecc = bindings[0].PAUSE.IsDown();
#if ANDROID
            grecc = android.pause;
#endif
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
                if (Keyboard.GetState().IsKeyDown(Keys.Escape) || android.exit)
                {
                    paused = false;
                }
            }
            prevState = Keyboard.GetState();
            prevMedState = MediaPlayer.State;
            prevJump = android.jump;
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);
            if (currentMenu != null)
            {
                currentMenu.Draw(spriteBatch);
            }
            else
            {
                game.Draw(spriteBatch);
            }
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
            /*StreamWriter chrick = new StreamWriter("settings.txt");
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
            chrick.WriteLine(playedBefore);
            chrick.WriteLine(vsync.ToString());
            chrick.Close();
            chrick.Dispose();*/
#endif

#if ANDROID
            android.SaveSettings();
#endif
        }
        public void LoadSettings()
        {
            for (int i = 0; i < bindings.Length; i++)
            {
                bindings[i].UP = new KeyBinding(Keys.W);
                bindings[i].DOWN = new KeyBinding(Keys.S);
                bindings[i].RIGHT = new KeyBinding(Keys.D);
                bindings[i].LEFT = new KeyBinding(Keys.A);
                bindings[i].JUMP = new KeyBinding(Keys.Space);
                bindings[i].ATTACK = new KeyBinding(Keys.J);
                bindings[i].SPECIAL = new KeyBinding(Keys.K);
                bindings[i].PAUSE = new KeyBinding(Keys.Enter);
            }
            try
            {
                /*StreamReader asgore = new StreamReader("settings.txt");
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
                playedBefore = bool.Parse(asgore.ReadLine());
                vsync = bool.Parse(asgore.ReadLine());
                asgore.Close();
                asgore.Dispose();*/
            }
            catch
            {
                Console.WriteLine("Well darn! It's not there!");
            }
#if ANDROID
                android.LoadSettings();
#endif
        }
    }
}
