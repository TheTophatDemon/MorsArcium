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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;

        Gameplay game;

        public Texture2D[] textures;
        public SoundEffect[] sounds;
        public SpriteFont font1;

        public Random random;

        public float scaleFactor = 1.0f;

        int textureIndex = 0;
        private bool pause = false;
        private bool skip = false;
        private bool henry = false;
        Rectangle thing;
        public MorsArcium()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();
            Window.Title = "MORS ARCIUM";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            scaleFactor = graphics.PreferredBackBufferHeight / 240f;
            thing = new Rectangle(0, 0, (int)(320 * scaleFactor), (int)(240 * scaleFactor));
            renderTarget = new RenderTarget2D(GraphicsDevice, 320, 240);
        }
        private void LoadTexture(string path)
        {
            textures[textureIndex] = Content.Load<Texture2D>("textures/" + Path.GetFileNameWithoutExtension(path));
            textureIndex += 1;
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureIndex = 0;
            font1 = Content.Load<SpriteFont>("Font1");

            textures = new Texture2D[512];

            LoadTexture("Content/textures/characters.png"); //0
            LoadTexture("Content/textures/explosion.png");  //1
            LoadTexture("Content/textures/hud.png");        //2
            LoadTexture("Content/textures/projectiles.png");//3
            LoadTexture("Content/textures/buttons.png");    //4
            LoadTexture("Content/textures/tileset.png");    //5
            LoadTexture("Content/textures/hitbox.png");     //6
            LoadTexture("Content/textures/particles.png");  //7

            random = new Random(DateTime.Now.Millisecond);
            game = new Gameplay(this);
            game.Initialize();
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
            if (!pause || skip)
            {
                game.Update(gameTime);
                skip = false;
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.DarkBlue);
            game.Draw(spriteBatch);
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, null, null, null);
            spriteBatch.Draw(renderTarget, thing, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public static float WeightedAverage(float x2, float x, float x1, float Q11, float Q21)
        {
            return ((x2 - x) / (x2 - x1)) * Q11 + ((x - x1) / (x2 - x1)) * Q21;
        }
    }
}
