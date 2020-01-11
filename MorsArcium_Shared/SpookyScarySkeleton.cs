using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mors_Arcium
{
    public class SpookyScarySkeleton : Entity
    {
        public SpookyScarySkeleton(Gameplay g) : base(g)
        {
            collisions = false;
            sourceRect = new Rectangle(384, 128, 32, 32);
            type = Gameplay.TYPE_ITEM;
            hitboxSize = new Vector2(4, 16); //Half of the actual hitbox's size
            hitboxOffset = new Vector2(0, 0);
            origin = new Vector2(16, 16);
        }
        public override void Update(GameTime gt)
        {
            speed.Y += game.gravityAcceleration;
            if (speed.Y > 8.0f) speed.Y = 8.0f;
            if ((collision_bottom && onSlope == -1) || (onSlope == -1 && wasOnSlope != -1))
            {
                speed.Y = 0.0f;
            }
            TryMove(speed);
            anim += 1;
            if (anim > 5)
            {
                anim = 0;
                if (sourceRect.X == 384)
                {
                    sourceRect.X = 416;
                }
                else
                {
                    sourceRect.X = 384;
                }
            }
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(game.game.textures[0], position - origin, sourceRect, Color.White);
        }
        public override void Collide(Entity perpetrator)
        {
            
        }
    }
}
