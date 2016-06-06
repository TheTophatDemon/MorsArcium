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
        public Projectile(Gameplay g, Entity own) : base(g)
        {
            owner = own;
        }
        public override void Update(GameTime gt)
        {
        }
        public override void Draw(SpriteBatch sp)
        {
        }
        public override void Collide(Entity perpetrator)
        {
        }
    }
}
