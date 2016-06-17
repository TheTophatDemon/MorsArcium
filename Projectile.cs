using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public class Projectile : Entity
    {
        public Entity owner;
        public float dodgeDistance;
        public Vector2 knockback;
        public Projectile(Gameplay g, Entity own) : base(g)
        {
            owner = own;
        }
        public override void Update(GameTime gt)
        {
            if (knockback.X != 0.0f)
            {
                knockback.X *= 0.93f;
                if (Math.Abs(knockback.X) < 0.1f) knockback.X = 0.0f;
            }
            if (knockback.Y != 0.0f)
            {
                knockback.Y *= 0.93f;
                if (Math.Abs(knockback.Y) < 0.1f) knockback.Y = 0.0f;
            }
        }
        public override void Draw(SpriteBatch sp)
        {
        }
        public override void Collide(Entity perpetrator)
        {
        }
    }
}
