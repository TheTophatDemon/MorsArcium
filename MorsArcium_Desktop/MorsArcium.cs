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

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;
        GameManager gMan;
        Rectangle gameViewport;

        MouseState mouseState;
        MouseState prevMouseState;
        Vector2 mouseMenuCoords;

        public MorsArcium()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Settings.Load();
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
            graphics.SynchronizeWithVerticalRetrace = Settings.vSync;
            graphics.ApplyChanges();

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
            prevMouseState = mouseState;
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

        public MenuButtonState ProcessMenuButton(MenuButton button)
        {
            if (mouseMenuCoords.X > button.position.X 
                && mouseMenuCoords.X < button.position.X + button.source.Width
                && mouseMenuCoords.Y > button.position.Y
                && mouseMenuCoords.Y < button.position.Y + button.source.Height)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    return MenuButtonState.PRESSED;
                }
                else
                {
                    return MenuButtonState.HOVER;
                }
            }
            return MenuButtonState.DEFAULT;
        }
    }
}
