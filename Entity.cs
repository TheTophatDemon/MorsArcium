using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mors_Arcium
{
    public struct Animation
    {
        public int[] frames;
        public int speed;
        public bool looping;
        public bool transition;
    }
    public abstract class Entity
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 speed;
        public Vector2 origin;
        public float rotation;
        public float scale = 1.0f;
        public int type;
        public int index;
        public Vector2 hitboxSize;
        public Vector2 hitboxOffset;
        public bool killMe = false;
        public bool collisions = true;
        protected Animation animation;
        protected int anim = 0;
        protected int frame = 0;
        protected int lastFrame = 0;
        public Rectangle sourceRect;
        protected Gameplay game;
        protected bool collision_bottom;
        protected bool collision_top;
        protected bool collision_left;
        protected bool collision_right;
        protected bool granddad; //GRAND DAD!?!?!?!? FLEENSTONES!?!?!?!?!
        protected int onSlope = -1;
        protected int wasOnSlope = -1;
        public int[] collisionMask;
        public Entity(Gameplay g)
        {
            game = g;
        }
        public abstract void Update(GameTime gt);
        public abstract void Draw(SpriteBatch sp);
        public abstract void Collide(Entity perpetrator);
        protected void TryMove(Vector2 spd)
        {
            wasOnSlope = onSlope;
            int tl = (int)Math.Floor((position.X - hitboxSize.X + hitboxOffset.X + spd.X) / 16) - 1;
            int tu = (int)Math.Floor((position.Y - hitboxSize.Y + hitboxOffset.Y + spd.Y) / 16) - 1;
            int tr = tl + (int)Math.Ceiling(hitboxSize.X / 8) + 2;
            int tb = tu + (int)Math.Ceiling(hitboxSize.Y / 8) + 2;
            tl = Math.Min(game.tilemap.width, Math.Max(0, tl));
            tr = Math.Min(game.tilemap.width, Math.Max(0, tr));
            tu = Math.Min(game.tilemap.height, Math.Max(0, tu));
            tb = Math.Min(game.tilemap.height, Math.Max(0, tb));
            collision_bottom = false;
            collision_left = false;
            collision_top = false;
            collision_right = false;
            granddad = false;
            int numSlopes = 0;
            Vector2[] potentialSlopes = new Vector2[8];
            
            for (int y = tu; y < tb; y++)
            {
                for (int x = tl; x < tr; x++)
                {
                    float tx = x * 16;
                    float ty = y * 16;
                    float fhaack = 16f;
                    if (game.tilemap.data[x, y] != -1)
                    {
                        //Horizontal Test
                        bool mettaton = false;
                        if (position.X + spd.X + hitboxSize.X + hitboxOffset.X > tx && position.X + spd.X - hitboxSize.X + hitboxOffset.X < tx + 16 && position.Y + hitboxSize.Y + hitboxOffset.Y > ty && position.Y - hitboxSize.Y + hitboxOffset.Y < ty + fhaack)
                        {
                            if (game.tilemap.data[x, y] == 0 || game.tilemap.data[x, y] == 2)
                            {
                                //Add potential slope collisions to the queue
                                potentialSlopes[numSlopes] = new Vector2(x, y);
                                numSlopes += 1;
                                mettaton = true;
                                
                            }
                            else
                            {
                                if (position.X + hitboxSize.X + hitboxOffset.X + spd.X > tx && position.X + hitboxSize.X + hitboxOffset.X + spd.X < tx + 16)
                                {
                                    position.X = tx - hitboxSize.X - hitboxOffset.X;
                                    if (y * 16 < position.Y + hitboxOffset.Y + hitboxSize.Y - 4) collision_right = true;
                                }
                                else
                                {
                                    position.X = tx + 16 + hitboxSize.X - hitboxOffset.X;
                                    if (y * 16 < position.Y + hitboxOffset.Y + hitboxSize.Y - 4) collision_left = true;
                                }
                                spd.X = 0.0f;
                            }
                        }
                        //Vertical Test
                        if (position.X + hitboxSize.X + hitboxOffset.X > tx && position.X - hitboxSize.X + hitboxOffset.X < tx + 16 && position.Y + spd.Y + hitboxSize.Y + hitboxOffset.Y > ty && position.Y - hitboxSize.Y + spd.Y + hitboxOffset.Y < ty + fhaack)
                        {
                            if ((game.tilemap.data[x, y] == 0 || game.tilemap.data[x, y] == 2))
                            {
                                if (!mettaton)
                                {
                                    //Add potential slope collisions to the queue
                                    potentialSlopes[numSlopes] = new Vector2(x, y);
                                    numSlopes += 1;
                                }
                            }
                            else
                            {
                                if (position.Y + hitboxSize.Y + hitboxOffset.Y + spd.Y > ty && position.Y + hitboxSize.Y + hitboxOffset.Y + spd.Y < ty + 16)
                                {
                                    if (game.tilemap.data[x, y] != 3 && game.tilemap.data[x, y] != 4)
                                    {
                                        position.Y = ty - hitboxSize.Y - hitboxOffset.Y;
                                        collision_bottom = true;
                                        spd.Y = 0.0f;
                                    }
                                }
                                else
                                {
                                    position.Y = ty + 16 + hitboxSize.Y - hitboxOffset.Y;
                                    collision_top = true;
                                    spd.Y = 0.0f;
                                }
                            }
                        }
                        if (!collision_top)
                        {
                            if (position.X - hitboxSize.X + hitboxOffset.X < tx + 16 && position.X + hitboxSize.X + hitboxOffset.X > tx && position.Y - hitboxSize.Y + hitboxOffset.Y > ty && position.Y - hitboxSize.Y - 1.0f + hitboxOffset.Y < ty + 16)
                            {
                                collision_top = true;
                            }
                        }
                        //if (!collision_bottom)
                        //{
                            if (position.X - hitboxSize.X + hitboxOffset.X < tx + 16 && position.X + hitboxSize.X + hitboxOffset.X > tx && position.Y + hitboxSize.Y + 1.0f + hitboxOffset.Y > ty && position.Y + hitboxSize.Y + hitboxOffset.Y < ty + 16)
                            {
                                granddad = true;
                            }
                        //}
                    }
                }
            }
            onSlope = -1;
            //Collide with the slopes now
            for (int i = 0; i < numSlopes; i++)
            {
                /*
                float maxDisplacement = 0.0f; //How much the player can move up the slope before hitting the cieling;
                int xx = 1; //How many tiles up is that?
                int temmie = (int)Math.Ceiling(hitboxSize.Y / 8) + 1;
                while (game.tilemap.data[(int)potentialSlopes[i].X, (int)potentialSlopes[i].Y - xx] == -1 && xx < temmie)
                {
                    xx += 1;
                    if (potentialSlopes[i].Y - xx < 0)
                    {
                        xx = 16;
                        break;
                    }
                }
                maxDisplacement = (xx) * 16;
                if (potentialSlopes[i].X - 1 >= 0)
                {
                    xx = 1;
                    while (game.tilemap.data[(int)potentialSlopes[i].X - 1, (int)potentialSlopes[i].Y - xx] == -1 && xx < temmie)
                    {
                        xx += 1;
                        if (potentialSlopes[i].Y - xx < 0)
                        {
                            xx = 16;
                            break;
                        }
                    }
                    if (xx * 16 < maxDisplacement) maxDisplacement = xx * 16;
                }
                if (potentialSlopes[i].X + 1 < game.tilemap.width)
                {
                    xx = 1;
                    while (game.tilemap.data[(int)potentialSlopes[i].X + 1, (int)potentialSlopes[i].Y - xx] == -1 && xx < temmie)
                    {
                        xx += 1;
                        if (potentialSlopes[i].Y - xx < 0)
                        {
                            xx = 16;
                            break;
                        }
                    }
                    if (xx * 16 < maxDisplacement) maxDisplacement = xx * 16;
                }*/
                float tx = potentialSlopes[i].X * 16;
                float ty = potentialSlopes[i].Y * 16;
                //if (!collision_top)
               // {
                    if (game.tilemap.data[(int)potentialSlopes[i].X, (int)potentialSlopes[i].Y] == 0) // "/"
                    {
                        //This test is being failed (Spd.y > 0)
                        if (position.X + spd.X + hitboxSize.X + hitboxOffset.X >= tx && position.X + spd.X  <= tx + 16 && position.Y + hitboxSize.Y + hitboxOffset.Y <= ty + 16 )
                        {
                            float dx = position.X + spd.X + hitboxSize.X + hitboxOffset.X - tx;
                            if (dx > 16) dx = 16;
                            if (dx < 0) dx = 0;
                            if (position.Y + hitboxSize.Y + spd.Y + hitboxOffset.Y >= (ty + 16) - dx)
                            {
                                spd.Y = ((ty + 16) - dx - hitboxSize.Y - hitboxOffset.Y) - position.Y;
                                /*if ((dx + hitboxSize.Y + hitboxSize.Y - hitboxOffset.Y) > maxDisplacement)
                                {
                                    spd = Vector2.Zero;
                                }*/
                                collision_bottom = true;
                                onSlope = 0;
                            }
                        }
                    }
                    else // "\"
                    {
                        if (position.X + spd.X >= tx && position.X + spd.X - hitboxSize.X + hitboxOffset.X <= tx + 16 && position.Y + hitboxSize.Y + hitboxOffset.Y <= ty + 16)
                        {
                            float dx = position.X + spd.X - hitboxSize.X + hitboxOffset.X - tx;
                            if (dx > 16) dx = 16;
                            if (dx < 0) dx = 0;
                            if (position.Y + spd.Y + hitboxSize.Y >= ty + dx)
                            {
                                spd.Y = (ty + dx - hitboxSize.Y - hitboxOffset.Y) - position.Y;
                                collision_bottom = true;
                                onSlope = 1;
                            }
                        }
                    }
               // }
               /* else
                {
                    if (position.Y + hitboxSize.Y + hitboxOffset.Y + spd.Y > ty && position.Y - hitboxSize.Y + hitboxOffset.Y + spd.Y < ty + 16)
                    {
                        if (position.X + hitboxSize.X + hitboxOffset.X + spd.X > tx && position.X + hitboxSize.X + hitboxOffset.X + spd.X < tx + 16)
                        {
                            position.X = tx - hitboxSize.X - hitboxOffset.X;
                            collision_right = true;
                        }
                        else
                        {
                            position.X = tx + 16 + hitboxSize.X - hitboxOffset.X;
                            collision_left = true;
                        }
                        spd.X = 0.0f;
                    }
                }*/
            }
            if (onSlope == -1 && wasOnSlope != -1)
            {
                spd.Y = 0f;
            }
            //Stop the slopes from pushing you into the cieling;
            if (numSlopes > 0)
            {
                tl = (int)Math.Floor((position.X - hitboxSize.X + hitboxOffset.X + spd.X) / 16) - 1;
                tu = (int)Math.Floor((position.Y - hitboxSize.Y + hitboxOffset.Y + spd.Y) / 16) - 1;
                tr = tl + (int)Math.Ceiling(hitboxSize.X / 8) + 2;
                tb = tu + (int)Math.Ceiling(hitboxSize.Y / 8) + 2;
                tl = Math.Min(game.tilemap.width, Math.Max(0, tl));
                tr = Math.Min(game.tilemap.width, Math.Max(0, tr));
                tu = Math.Min(game.tilemap.height, Math.Max(0, tu));
                tb = Math.Min(game.tilemap.height, Math.Max(0, tb));
                for (int y = tu; y < tb; y++)
                {
                    for (int x = tl; x < tr; x++)
                    {
                        if (game.tilemap.data[x, y] != -1)
                        {
                            if (position.X + hitboxSize.X + spd.X + hitboxOffset.X > x * 16 && position.X - hitboxSize.X + spd.X + hitboxOffset.X < (x * 16) + 16 && position.Y + spd.Y - hitboxSize.Y + hitboxOffset.Y > y * 16 && position.Y - hitboxSize.Y + spd.Y + hitboxOffset.Y < (y * 16) + 16)
                            {
                                if ((onSlope == 0 && spd.X > 0.0f) || (onSlope == 1 && spd.X < 0.0f)) spd = Vector2.Zero;
                            }
                        }
                    }
                }
            }
            position += spd;
        }
    }
}
