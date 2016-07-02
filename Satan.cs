using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mors_Arcium
{
    public class Satan : Entity
    {
        Animation idleAnimation;
        Animation smileAnimation;
        Animation attackAnimation;
        float grenp = 0.0f;
        public Satan(Gameplay g, Vector2 pos) : base(g)
        {
            collisions = false;
            position = pos;
            type = Gameplay.TYPE_BEAM; //So he's behind the terrain
            origin = new Vector2(8, 8);
            sourceRect = new Rectangle(0, 128, 64, 72);
            idleAnimation.frames = new int[] { 0 };
            idleAnimation.looping = true;
            idleAnimation.speed = 1;
            smileAnimation.frames = new int[] { 1, 2 };
            smileAnimation.speed = 5;
            smileAnimation.looping = false;
            attackAnimation.frames = new int[] { 3, 4, 5 };
            attackAnimation.speed = 3;
            attackAnimation.looping = false;
            animation = idleAnimation;
            texture = game.game.textures[0];
        }
        public override void Update(GameTime gt)
        {
            if (game.satan == null)
            {
                speed.Y -= 1.0f;
            }
            Animate();
            position += speed;
            grenp += 0.1f;
            position.Y += (float)Math.Sin(grenp) * 2.0f;
            if (position.Y < -128.0f)
            {
                killMe = true;
            }
        }
        public void Animate()
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
                if (frame >= animation.frames.Length)
                {
                    frame = 0;
                }
                sourceRect.X = (animation.frames[frame] % (texture.Width / sourceRect.Width)) * sourceRect.Width;
            }
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, sourceRect, Color.White, rotation, origin, scale, SpriteEffects.None, 0);
        }
        public override void Collide(Entity perpetrator)
        {
            Console.WriteLine("UNACCEPTABLE!!!!!!!!! GET OUT OF HERE! GO! AWAY WITH YOU! YOU SNIVELING HERETIC! JUST STOP! JEEZ! GOD! GRAND DAD! FLEENSTONES! WOW!");
        }
    }
}
