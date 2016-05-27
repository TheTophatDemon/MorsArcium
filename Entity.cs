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

        protected Animation animation;
        protected int anim = 0;
        protected int frame = 0;
        protected int lastFrame = 0;
        protected Rectangle sourceRect;
        protected Gameplay game;
        protected bool collision_bottom;
        protected bool collision_top;
        protected bool collision_left;
        protected bool collision_right;
        public Entity(Gameplay g)
        {
            game = g;
        }
        public abstract void Update(GameTime gt);
        public abstract void Draw(SpriteBatch sp);
        public abstract void Collide(Entity perpetrator);
        protected void TryMove(Vector2 spd)
        {
            int tl = (int)Math.Floor((position.X - hitboxSize.X + hitboxOffset.X + spd.X) / 16) - 1;
            int tu = (int)Math.Floor((position.Y - hitboxSize.Y + hitboxOffset.Y + spd.Y) / 16) - 1;
            int tr = tl + (int)(hitboxSize.X / 8) + 2;
            int tb = tu + (int)(hitboxSize.Y / 8) + 2;
            tl = Math.Min(game.tilemap.width, Math.Max(0, tl));
            tr = Math.Min(game.tilemap.width, Math.Max(0, tr));
            tu = Math.Min(game.tilemap.height, Math.Max(0, tu));
            tb = Math.Min(game.tilemap.height, Math.Max(0, tb));
            collision_bottom = false;
            collision_left = false;
            collision_top = false;
            collision_right = false;
            for (int y = tu; y < tb; y++)
            {
                for (int x = tl; x < tr; x++)
                {
                    float tx = x * 16;
                    float ty = y * 16;
                    if (game.tilemap.data[x, y] != -1 && ((game.tilemap.data[x, y] != 0 && game.tilemap.data[x, y] != 2) || collision_top)) //Not a slope
                    {
                        //Horizontal Test
                        if (position.X + spd.X + hitboxSize.X + hitboxOffset.X > tx && position.X + spd.X - hitboxSize.X + hitboxOffset.X < tx + 16 && position.Y + hitboxSize.Y + hitboxOffset.Y > ty && position.Y - hitboxSize.Y + hitboxOffset.Y < ty + 16)
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
                        //Vertical Test
                        if (position.X + hitboxSize.X + hitboxOffset.X > tx && position.X - hitboxSize.X + hitboxOffset.X < tx + 16 && position.Y + spd.Y + hitboxSize.Y + hitboxOffset.Y > ty && position.Y - hitboxSize.Y + spd.Y + hitboxOffset.Y < ty + 16)
                        {
                            if (position.Y + hitboxSize.Y + hitboxOffset.Y + spd.Y > ty && position.Y + hitboxSize.Y + hitboxOffset.Y + spd.Y < ty + 16)
                            {
                                position.Y = ty - hitboxSize.Y - hitboxOffset.Y;
                                collision_bottom = true;
                            }
                            else
                            {
                                position.Y = ty + 16 + hitboxSize.Y - hitboxOffset.Y;
                                collision_top = true;
                            }
                            spd.Y = 0.0f;
                        }
                        if (!collision_top)
                        {
                            if (position.X - hitboxSize.X + hitboxOffset.X < tx + 16 && position.X + hitboxSize.X + hitboxOffset.X > tx && position.Y - hitboxSize.Y + hitboxOffset.Y > ty && position.Y - hitboxSize.Y - 1.0f + hitboxOffset.Y < ty + 16)
                            {
                                collision_top = true;
                                
                            }
                        }
                    }
                    else if (game.tilemap.data[x, y] == 0) // "/" slope
                    {
                        if (position.X + hitboxSize.X + hitboxOffset.X >= tx && position.X + hitboxSize.X + hitboxOffset.X <= tx + 16 && position.Y + hitboxSize.Y + hitboxOffset.Y <= ty + 16 && spd.Y >= 0.0f)
                        {
                            if (collision_top)
                            {
                                //position.X = tx - hitboxSize.X - hitboxOffset.X; //PROBLEM!!!
                            }
                            else
                            {
                                float dx = position.X + hitboxSize.X + hitboxOffset.X - tx;
                                if (position.Y + hitboxSize.Y + hitboxOffset.Y >= (ty + 16) - dx)
                                {
                                    position.Y = (ty + 16) - dx - hitboxSize.Y - hitboxOffset.Y;
                                    spd.Y = 0f;
                                }
                            }
                        }
                    }
                    else if (game.tilemap.data[x, y] == 2) // "\" slope
                    {
                        if (position.X - hitboxSize.X + hitboxOffset.X >= tx && position.X - hitboxSize.X + hitboxOffset.X <= tx + 16 && position.Y + hitboxSize.Y + hitboxOffset.Y <= ty + 16 && spd.Y >= 0.0f)
                        {
                            if (collision_top)
                            {
                                //position.X = tx + 16 + hitboxSize.X - hitboxOffset.X;
                            }
                            else
                            {
                                float dx = position.X - hitboxSize.X + hitboxOffset.X - tx;
                                if (position.Y + hitboxSize.Y >= ty + dx)
                                {
                                    position.Y = ty + dx - hitboxSize.Y - hitboxOffset.Y;
                                    spd.Y = 0f;
                                }
                            }
                        }
                    }
                }
            }
            //if (collision_top) Console.WriteLine("SCHWEINHUNTS");
            position += spd;
        }
    }
}
