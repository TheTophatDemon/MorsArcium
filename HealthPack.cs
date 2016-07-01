using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mors_Arcium
{
    public class HealthPack : Entity
    {
        static Rectangle rect = new Rectangle(0, 0, 16, 16);
        static int[] healthPackCollisionMask = new int[] { Gameplay.TYPE_PLAYER };
        public HealthPack(Gameplay g, Vector2 pos) : base(g)
        {
            collisionMask = healthPackCollisionMask;
            position = pos;
            type = Gameplay.TYPE_ITEM;
            hitboxSize = new Vector2(8, 8); //Half of the actual hitbox's size
            hitboxOffset = new Vector2(0, 0);
            origin = new Vector2(8, 8);
            sourceRect = rect;
        }
        public override void Update(GameTime gt)
        {
            anim += 1;
            if (anim > 20)
            {
                anim = 0;
                sourceRect.X += 16;
                if (sourceRect.X == 48)
                {
                    sourceRect.X = 0;
                }
            }
            speed.Y += game.gravityAcceleration;
            if (speed.Y > 8.0f) speed.Y = 8.0f;
            if (collision_bottom)
            {
                speed.Y = 0.0f;
            }
            TryMove(speed);
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(game.game.textures[8], position - origin, sourceRect, Color.White);
        }
        public override void Collide(Entity perpetrator)
        {
            if (perpetrator is Player)
            {
                Player p = (Player)perpetrator;
                p.health += 25;
                if (p.health > p.maxHealth) p.health = p.maxHealth;
                p = null;
                killMe = true;
                for (int i = 0; i < 15; i++)
                {
                    game.AddParticle(new Particle(game, position, new Vector2(((float)game.game.random.NextDouble() * 2.5f) - 1.25f, ((float)game.game.random.NextDouble() * 2.5f) - 1.25f), 25, 8, 4));
                }
            }
        }
    }
}
