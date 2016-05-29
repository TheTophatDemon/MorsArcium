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
        private int frame = 0;
        private int size = 8;
        private int numFrames;
        private Rectangle rect;
        private Gameplay game;
        private int baseX, baseY;
        public bool killMe = false; //Later...
        public Particle(Gameplay g, Vector2 pos, Vector2 vel, float anmSpd, int sz, int type)
        {
            game = g;
            position = pos;
            velocity = vel;
            animSpeed = anmSpd;
            size = sz;
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
            }
            rect = new Rectangle(baseX, baseY, size, size);
        }
        public void Update(GameTime gt)
        {
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
            sp.Draw(game.game.textures[7], position, rect, Color.White);
        }
    }
}
