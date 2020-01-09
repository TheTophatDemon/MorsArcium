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
        //TODO: Ensure 64 bit & Android API Level 28
        //TODO: Refine sound system
        //TODO: Refine resource loading system
        //TODO: Separate GUI class
        //TODO: Move settings into separate class
        //TODO: Redo settings system to use .ini
        //TODO: Make resolution dynamic
        //TODO: Make dynamic timing
        //TODO: Refine tutorial startup
        //TODO: Refine Joystick Hat Capability
        //TODO: Add Joystick Axis Capability
        //TODO: Add proper keyboard key names
        //TODO: Restructure player class
        //TODO: Polish player class change
        //TODO: Optimize GUI elements
        //TODO: Polish new Eli attack
        //TODO: Change "Zero Gravity" to "Low Gravity"
        //TODO: Add Android HUD customization
        //TODO: Add crossfading music system
        //TODO: Add pitch variations to sounds
        //TODO: Add Eli slicing sound
        //TODO: Remove automatic teleporting at edges for Mr.B
        //TODO: Polish player spawning

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
        public PlayerBindings bindings = new PlayerBindings();

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
        public SpriteFont font1;

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
            graphics.SynchronizeWithVerticalRetrace = vsync;
            graphics.ApplyChanges();

            AudioSystem.Initialize(this);
        }
        private void LoadTexture(string path, int index)
        {
            textures[index] = Content.Load<Texture2D>("textures/" + Path.GetFileNameWithoutExtension(path));
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

            AudioSystem.LoadContent(this);

            random = new Random(DateTime.Now.Millisecond);
            game = new Gameplay(this);
            currentMenu = new MainMenu(this);
            //game.Initialize();
        }
        protected override void UnloadContent()
        {
            for (int i = 0; i < textures.Length; i++)
            {
                if (textures[i] != null) textures[i].Dispose();
            }
            AudioSystem.UnloadContent();
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
            if (Keyboard.GetState().IsKeyDown(Keys.O) && !prevState.IsKeyDown(Keys.O)) skip = true;
            if (Keyboard.GetState().IsKeyDown(Keys.I)) paused = false;
#endif

            AudioSystem.Update(gameTime);

            if ((bindings.PAUSE.IsDown() || android.pause) && !grecc)
            {
                paused = !paused;
            }
            grecc = bindings.PAUSE.IsDown();
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
            bindings.UP = new KeyBinding(Keys.W);
            bindings.DOWN = new KeyBinding(Keys.S);
            bindings.RIGHT = new KeyBinding(Keys.D);
            bindings.LEFT = new KeyBinding(Keys.A);
            bindings.JUMP = new KeyBinding(Keys.Space);
            bindings.ATTACK = new KeyBinding(Keys.J);
            bindings.SPECIAL = new KeyBinding(Keys.K);
            bindings.PAUSE = new KeyBinding(Keys.Enter);
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
