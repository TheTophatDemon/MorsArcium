using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mors_Arcium
{
    public class Particle
    {
        public Vector2 position;
        public Vector2 velocity;
        private float animSpeed = 5;
        private float anim = 0;
        public int frame = 0;
        private int size = 8;
        private int numFrames;
        public Rectangle rect;
        private Gameplay game;
        public int baseX, baseY;
        public bool killMe = false; //Later...
        public int particleType;
        private float alpha = 1.0f;
        private float scale = 1.0f;
        Vector2 origin = Vector2.Zero;
        public Particle(Gameplay g, Vector2 pos, Vector2 vel, float anmSpd, int sz, int type)
        {
            game = g;
            position = pos;
            velocity = vel;
            animSpeed = anmSpd;
            size = sz;
            particleType = type;
            switch (type)
            {
                case 0: //Mr./b/'s eye flash thing
                    numFrames = 3;
                    baseX = 0;
                    baseY = 0;
                    break;
                case 1: //Smoke or Dust
                    numFrames = 3;
                    baseX = 24;
                    baseY = 0;
                    break;
                case 2: //Explosion
                    numFrames = 4;
                    baseX = 0;
                    baseY = 8;
                    break;
                case 3: //Gore
                    numFrames = 1;
                    break;
                case 4: //Health
                    numFrames = 1;
                    baseX = (14 + g.game.random.Next(0, 2)) * 8;
                    baseY = 0;
                    break;
                case 5: //FUS RO DA!!!!!
                    numFrames = 1;
                    baseX = 0;
                    baseY = 40;
                    scale = 0.1f;
                    origin = new Vector2(32, 32);
                    break;
            }
            rect = new Rectangle(baseX, baseY, size, size);
        }
        public void Update(GameTime gt)
        {
            if (particleType == 3)
            {
                velocity.Y += 0.178f;
            }
            if (particleType == 3 || particleType == 4 || particleType == 5)
            {
                rect.X = baseX + (frame * size);
            }
            if ((particleType == 4 && anim > 15) || (particleType == 5 && anim > 15))
            {
                alpha -= 0.1f;
            }
            if (particleType == 5)
            {
                scale += 0.1f;
                //if (scale > 1.0f) scale = 1.0f;
            }
            position += velocity;
            anim += 1;
            if (anim > animSpeed)
            {
                anim = 0;
                frame += 1;
                rect.X = baseX + (frame * size);
                if (frame >= numFrames)
                {
                    frame = 0;
                    killMe = true;
                }
            }
        }
        public void Draw(SpriteBatch sp)
        {
            sp.Draw(game.game.textures[7], position, rect, Color.White * alpha, 0.0f, origin, scale, SpriteEffects.None, 0);
        }
    }
}
