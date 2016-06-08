using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public class Crab : Projectile
    {
        Vector2 initSpd;
        Vector2 lastPos;
        float timer = 100;
        public static int[] CrabCollisionMask = new int[] { Gameplay.TYPE_PLAYER };
        public Crab(Gameplay g, Vector2 pos, Vector2 initialSpeed, Entity own) : base(g, own)
        {
            collisionMask = CrabCollisionMask;
            texture = g.game.textures[3];
            type = Gameplay.TYPE_PROJECTILE;
            sourceRect = new Rectangle(0, 0, 16, 16);
            hitboxSize = new Vector2(8, 8); //Half of the actual hitbox's size
            hitboxOffset = new Vector2(0, 0);
            origin = new Vector2(8, 8);
            speed = initialSpeed;
            position = pos;
            initSpd = new Vector2(Math.Abs(initialSpeed.X), Math.Abs(initialSpeed.Y));
            owner = own;
            dodgeDistance = 32.0f;
        }
        public override void Update(GameTime gt)
        {
            if (position.X == lastPos.X)
            {
                if (speed.X > 0.0f)
                {
                    speed.X = -initSpd.X;
                }
                else if (speed.X < 0.0f)
                {
                    speed.X = initSpd.X;
                }
            }
            speed.Y += 0.15f;
            if (speed.Y > 8.0f) speed.Y = 8.0f;
            if ((collision_bottom && onSlope == -1) || (onSlope == -1 && wasOnSlope != -1))
            {
                speed.Y = 0.0f;
            }
            if (collision_top)
            {
                speed.Y = 0.5f;
            }
            lastPos = position;
            TryMove(speed);
            anim += 1;
            if (anim > 5)
            {
                anim = 0;
                if (sourceRect.X == 0)
                {
                    sourceRect.X = 16;
                }
                else
                {
                    sourceRect.X = 0;
                }
            }
            timer -= 1;
            if (timer == 0)
            {
                timer -= 1;
                game.Explode(position.X, position.Y + 8.0f, 24f, 7);
                killMe = true;
            }
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position - origin, sourceRect, Color.White);
        }
        public override void Collide(Entity perpetrator)
        {
            if (perpetrator != owner && perpetrator.type == Gameplay.TYPE_PLAYER)
            {
                Player p = (Player)perpetrator;
                if (p.deathTimer == 0)
                {
                    timer = 1;
                }
                p = null;
            }
        }
    }
}
