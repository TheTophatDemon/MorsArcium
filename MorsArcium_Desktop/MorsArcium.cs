using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System;
using Mors_Arcium;

namespace MorsArcium_Desktop
{
    public class MorsArcium : Game, IPlatformOutlet
    {
        public Rectangle GameViewport { get => gameViewport; }
        public Settings GameSettings { get => settings; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;
        Rectangle gameViewport;

        GameManager gMan;
        Settings settings;

        MouseState mouseState;
        Vector2 mouseMenuCoords;

        public MorsArcium()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //TODO: Load settings from .ini
            settings = new Settings();
            settings.aimUp = new KeyBinding(Keys.W);
            settings.aimDown = new KeyBinding(Keys.S);
            settings.moveLeft = new KeyBinding(Keys.A);
            settings.moveRight = new KeyBinding(Keys.D);
            settings.jump = new KeyBinding(Keys.Space);
            settings.attack = new KeyBinding(Keys.J);
            settings.special = new KeyBinding(Keys.K);
            settings.pause = new KeyBinding(Keys.Enter);

            gMan = new GameManager(this);
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

            ApplyVideoSettings();

            gameViewport = GraphicsDevice.Viewport.Bounds;

            renderTarget = new RenderTarget2D(GraphicsDevice, 320, 240);
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            gMan.LoadContent(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            mouseState = Mouse.GetState();
            mouseMenuCoords = new Vector2(mouseState.X / 3.0f, mouseState.Y / 3.0f);

            gMan.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(renderTarget);
            gMan.Draw(GraphicsDevice, gameTime);
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, null, null, null);
            spriteBatch.Draw(renderTarget, gameViewport, Color.White);
            spriteBatch.End();
        }

        public Mors_Arcium.GUI.Button.State ProcessMenuButton(Mors_Arcium.GUI.Button button)
        {
            if (mouseMenuCoords.X > button.Position.X 
                && mouseMenuCoords.X < button.Position.X + button.Source.Width
                && mouseMenuCoords.Y > button.Position.Y
                && mouseMenuCoords.Y < button.Position.Y + button.Source.Height)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    return Mors_Arcium.GUI.Button.State.PRESSED;
                }
                else
                {
                    return Mors_Arcium.GUI.Button.State.HOVER;
                }
            }
            return Mors_Arcium.GUI.Button.State.DEFAULT;
        }

        public void ApplyVideoSettings()
        {
            graphics.SynchronizeWithVerticalRetrace = settings.vSync;
            if (graphics.IsFullScreen != settings.fullScreen) graphics.ToggleFullScreen();
            graphics.ApplyChanges();
        }

        public void SaveSettings()
        {

        }
    }
}
