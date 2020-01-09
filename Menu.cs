using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium
{
    public struct Button
    {
        public Vector2 position;
        public Rectangle source;
        public Action function;
        public bool hover;
        public SpriteEffects spriteEffect;
    }
    public class Menu
    {
        public Button[] buttons;
        protected MorsArcium game;
        MouseState prevMouseState;
        TouchCollection prevTouches;
        RenderTarget2D renderTarget;
        Rectangle displayRect;
        public Menu(MorsArcium g)
        {
            game = g;
            renderTarget = new RenderTarget2D(g.GraphicsDevice, 320, 240);
        }
        public virtual void Update(GameTime g)
        {
            displayRect = new Rectangle((int)(game.GraphicsDevice.Viewport.Width - (320 * game.scaleFactor)) / 2, 0, (int)(320 * game.scaleFactor), (int)(240 * game.scaleFactor));
            MouseState mouseState = Mouse.GetState(game.Window);
            TouchCollection touches = TouchPanel.GetState();
            int mx = (int)((mouseState.Position.X - displayRect.X) / game.scaleFactor);
            int my = (int)(mouseState.Position.Y / game.scaleFactor);
            int tx = -256;
            int ty = -256;
            if (touches.Count > 0)
            {
                tx = (int)((touches[0].Position.X - displayRect.X) / game.scaleFactor);
                ty = (int)(touches[0].Position.Y / game.scaleFactor);
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].hover = false;
                if (mx > buttons[i].position.X && mx < buttons[i].position.X + buttons[i].source.Width
                    && my > buttons[i].position.Y && my < buttons[i].position.Y + buttons[i].source.Height)
                {
                    buttons[i].hover = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        if (buttons[i].function != null) buttons[i].function();
                        OnButtonPress(buttons[i]);
                    }
                }
                if (touches.Count > 0)
                {
                    if (tx > buttons[i].position.X && tx < buttons[i].position.X + buttons[i].source.Width
                        && ty > buttons[i].position.Y && ty < buttons[i].position.Y + buttons[i].source.Height)
                    {
                        buttons[i].hover = true;
                        if (touches[0].State == TouchLocationState.Pressed && prevTouches.Count == 0)
                        {
                            if (buttons[i].function != null) buttons[i].function();
                            OnButtonPress(buttons[i]);
                        }
                    }
                }
                
            }
            prevMouseState = mouseState;
            prevTouches = touches;
        }
        public void DrawButtons(SpriteBatch sp)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                sp.Draw(game.textures[2], buttons[i].position, buttons[i].source, (buttons[i].hover == true) ? Color.Gray : Color.White, 0.0f, Vector2.Zero, 1.0f, buttons[i].spriteEffect, 0.0f);
            }
        }
        public void Draw(SpriteBatch sp)
        {
            sp.GraphicsDevice.SetRenderTarget(renderTarget);
            DrawExtra(sp);
            sp.GraphicsDevice.SetRenderTarget(null);
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, null, null, null);
            sp.Draw(renderTarget, displayRect, Color.White * game.fade);
            sp.End();
        }
        public virtual void DrawExtra(SpriteBatch sp)
        {

        }
        public virtual void OnButtonPress(Button source)
        {

        }
    }
}
