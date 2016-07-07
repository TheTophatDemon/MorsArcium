using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mors_Arcium
{
    public class Bomb : Entity
    {
        static Rectangle rect = new Rectangle(0, 16, 16, 16);
        static int[] bombCollisionMask = new int[] { Gameplay.TYPE_PLAYER };
        int timer = 200;
        public Bomb(Gameplay g, Vector2 pos) : base(g)
        {
            collisionMask = bombCollisionMask;
            position = pos;
            type = Gameplay.TYPE_ITEM;
            hitboxSize = new Vector2(8, 8); //Half of the actual hitbox's size
            hitboxOffset = new Vector2(0, 0);
            origin = new Vector2(8, 8);
            sourceRect = rect;
        }
        public override void Update(GameTime gt)
        {
            if (position.X > game.cameraPosition.X && position.X < game.cameraPosition.X + 320 && timer % 2 == 0) {
                game.AddParticle(new Particle(game, position + new Vector2(-4, -8), new Vector2((float)(game.game.random.NextDouble() - 0.5f) * 10.0f, -4.0f), 3, 8, 6));
            }
            speed.Y += game.gravityAcceleration;
            if (speed.Y > 8.0f) speed.Y = 8.0f;
            if (collision_bottom)
            {
                speed.Y = 0.0f;
            }
            TryMove(speed);
            if (granddad)
            {
                timer = 0;
            }
            timer -= 1;
            if (timer <= 0)
            {
                game.Explode(position.X, position.Y, 16f, 20, false);
                game.PlaySound(1, position);
                killMe = true;
            }
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(game.game.textures[8], position - origin, sourceRect, Color.White);
        }
        public override void Collide(Entity perpetrator)
        {
            if (perpetrator is Player)
            {
                timer = 0;
            }
        }
    }
}
