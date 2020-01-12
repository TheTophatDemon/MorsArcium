using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mors_Arcium
{
    public class Trident : Projectile
    {
        public int damage = 6;
        private int timer = 0;
        float alpha = 1.0f;
        bool landed = false;
        public Trident(Gameplay g, Vector2 pos, Vector2 spd, Entity own) : base(g, own)
        {
            owner = own;
            collisionMask = Crab.CrabCollisionMask;
            texture = g.game.textures[3];
            type = Gameplay.TYPE_PROJECTILE;
            sourceRect = new Rectangle(32, 0, 48, 16);
            hitboxSize = new Vector2(6, 6); //Half of the actual hitbox's size
            hitboxOffset = new Vector2(0, 0);
            origin = new Vector2(32, 8);
            speed = spd;
            position = pos;
            owner = own;
            dodgeDistance = 72.0f;
        }
        public override void Update(GameTime gt)
        {
            base.Update(gt);
            TryMove((speed + knockback) * game.projectileSpeedMultiplier, false);
            if ((collision_left || collision_right || collision_bottom || collision_top) && !landed)
            {
                game.game.audio.Play3DSound("land", position);
                landed = true;
                speed = Vector2.Zero;
            }
            timer += 1;
            if (timer > 90)
            {
                alpha -= 0.1f;
            }
            if (timer > 100)
            {
                killMe = true;
            }
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, sourceRect, Color.White * alpha, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }
        public override void Collide(Entity perpetrator)
        {
            if (perpetrator != owner && perpetrator.type == Gameplay.TYPE_PLAYER)
            {
                Player p = (Player)perpetrator;
                if (p.deathTimer == 0)
                {
                    killMe = true;
                    p.Damage(damage, this.owner);
                }
                p = null;
            }
        }
    }
}
