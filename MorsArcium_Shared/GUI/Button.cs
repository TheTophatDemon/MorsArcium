using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mors_Arcium.GUI
{
    public class Button
    {
        public enum State { DEFAULT, HOVER, PRESSED }

        public Vector2 Position { get => position; }
        public Rectangle Source { get => source; }
        public State Status { get => status; }

        protected GameManager gMan;
        protected Texture2D texture;
        protected Color color = Color.White;
        protected Vector2 position;
        protected Rectangle source;
        protected Action<Button> onClick;
        protected State status = State.DEFAULT;
        protected State prevStatus = State.DEFAULT;

        public Button(GameManager gMan, Texture2D texture, Rectangle source, Vector2 position, Action<Button> clickAction)
        {
            this.gMan = gMan;
            this.texture = texture;
            this.source = source;
            this.position = position;
            onClick = clickAction;
        }
        public virtual void Update(GameTime time)
        {
            prevStatus = status;
            status = gMan.platform.ProcessMenuButton(this);

            if (status != State.PRESSED && prevStatus == State.PRESSED) onClick?.Invoke(this);
            color = (status == State.DEFAULT) ? Color.White : Color.Gray;
        }
        public virtual void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, source, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
