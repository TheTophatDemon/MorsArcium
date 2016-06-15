using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mors_Arcium
{
    public class Beam : Entity
    {
        public float length;
        public float alpha = 1.0f;
        Rectangle rect2;
        Vector2 origin2;
        float cos = 0.0f;
        float sin = 0.0f;
        public Beam(Gameplay g, float l, float r) : base(g)
        {
            length = l;
            rotation = r;
            collisions = false;
            type = Gameplay.TYPE_BEAM;
            origin = new Vector2(0, 4);
            sourceRect = new Rectangle(0, 16, 16, 8);
            origin2 = new Vector2(0, 4);
            rect2 = new Rectangle(16, 16, 16, 8);
            cos = (float)Math.Cos(r) * 16;
            sin = (float)Math.Sin(r) * 16;
        }
        public override void Update(GameTime gt)
        {
            alpha -= 0.05f;
            if (alpha <= 0.0f)
            {
                killMe = true;
            }
        }
        public override void Draw(SpriteBatch sp)
        {
            //sp.Draw(game.game.textures[3], position, sourceRect, Color.White * alpha, rotation, origin, 1.0f, SpriteEffects.None, 0);
            sp.Draw(game.game.textures[3], new Rectangle((int)Math.Floor(position.X + cos), (int)Math.Floor(position.Y + sin), (int)length - 16, 8), rect2, Color.White * alpha, rotation, origin2, SpriteEffects.None, 0);
        }
        public override void Collide(Entity perpetrator)
        {
            
        }
    }
}
