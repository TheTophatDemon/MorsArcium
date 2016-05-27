using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public class Player : Entity
    {
        public const int TYPE_MR_B = 0;
        public const int TYPE_WIZARD = 1;
        public const int TYPE_ELI = 2;
        public const int TYPE_BUG = 3;

        SpriteEffects spriteEffects;
        public int playerType = 0;

        Animation idleAnimation;
        Animation walkAnimation;
        Animation jumpAnimation;

        public Player(Gameplay g, int pt) : base(g)
        {
            texture = g.game.textures[0];
            playerType = pt;
            type = Gameplay.TYPE_PLAYER;
            sourceRect = new Rectangle(0, pt * 32, 32, 32);
            hitboxSize = new Vector2(8, 16); //Half of the actual hitbox's size
            hitboxOffset = new Vector2(0, 0);
            origin = new Vector2(16, 16);
            switch (pt)
            {
                case TYPE_MR_B:
                    idleAnimation.frames = new int[] { 0, 1, 2, 1 };
                    idleAnimation.speed = 5;
                    idleAnimation.looping = true;
                    walkAnimation.frames = new int[] { 3, 4, 5, 6 };
                    walkAnimation.speed = 3;
                    walkAnimation.looping = true;
                    jumpAnimation.frames = new int[] { 7, 8, 9, 10 };
                    jumpAnimation.speed = 3;
                    jumpAnimation.looping = false;
                    break;
                case TYPE_WIZARD:

                    break;
            }
            animation = idleAnimation;
        }
        public override void Update(GameTime gt)
        {
            speed = Vector2.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                speed.X = 2f;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                speed.X = -2f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                speed.Y = -2f;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                speed.Y = 2f;
            }
            TryMove(speed);
            Animate();
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, sourceRect, Color.White, rotation, origin, scale, spriteEffects, 0);
        }
        public override void Collide(Entity perpetrator)
        {

        }
        private void Animate()
        {
            if (animation.frames != null)
            {
                anim += 1;
                if (anim > animation.speed)
                {
                    anim = 0;
                    frame += 1;
                    if (frame > animation.frames.Length - 1)
                    {
                        frame = 0;
                        if (!animation.looping)
                        {
                            frame = animation.frames.Length - 1;
                        }
                    }
                }
                if (frame != lastFrame)
                {
                    sourceRect.X = animation.frames[frame] * 32;
                }
                lastFrame = frame;
            }
        }
    }
}
