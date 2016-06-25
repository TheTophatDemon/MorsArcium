using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mors_Arcium
{
    public class Bullet : Projectile
    {
        public int damage = 4;
        public Bullet(Gameplay g, Vector2 pos, Vector2 spd, Entity own) : base(g, own)
        {
            owner = own;
            collisionMask = Crab.CrabCollisionMask;
            texture = g.game.textures[3];
            type = Gameplay.TYPE_PROJECTILE;
            sourceRect = new Rectangle(32, 0, 8, 8);
            hitboxSize = new Vector2(4, 4); //Half of the actual hitbox's size
            hitboxOffset = new Vector2(0, 0);
            origin = new Vector2(4, 4);
            speed = spd;
            position = pos;
            owner = own;
            dodgeDistance = 72.0f;
        }
        public override void Update(GameTime gt)
        {
            base.Update(gt);
            TryMove(speed);
            if (collision_bottom || collision_top || collision_left || collision_right || position.X < game.cameraPosition.X || position.X > game.cameraPosition.X + 320 || position.Y < game.cameraPosition.Y || position.Y > game.cameraPosition.Y + 240)
            {
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
                    killMe = true;
                    p.Damage(damage, this.owner);
                }
                p = null;
            }
        }
    }
}
