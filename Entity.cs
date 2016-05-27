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

        protected Animation animation;
        protected int anim = 0;
        protected int frame = 0;
        protected int lastFrame = 0;
        protected Rectangle sourceRect;
        protected Gameplay game;
        public Entity(Gameplay g)
        {
            game = g;
        }
        public abstract void Update(GameTime gt);
        public abstract void Draw(SpriteBatch sp);
        public abstract void Collide(Entity perpetrator);
        protected void TryMove(Vector2 spd)
        {
            int tl = (int)Math.Floor((position.X - hitboxSize.X + spd.X) / 16);
            int tu = (int)Math.Floor((position.Y - hitboxSize.Y + spd.Y) / 16);
            int tr = tl + (int)(hitboxSize.X / 8) + 1;
            int tb = tu + (int)(hitboxSize.Y / 8) + 1;
            tl = Math.Min(game.tilemap.width - 1, Math.Max(0, tl));
            tr = Math.Min(game.tilemap.width - 1, Math.Max(0, tr));
            tu = Math.Min(game.tilemap.height - 1, Math.Max(0, tu));
            tb = Math.Min(game.tilemap.height - 1, Math.Max(0, tb));
            for (int y = tu; y < tb; y++)
            {
                for (int x = tl; x < tr; x++)
                {
                    float tx = x * 16;
                    float ty = y * 16;
                    if (game.tilemap.data[x, y] > 0 && game.tilemap.data[x, y] != 2) //Not a slope
                    {
                        //Horizontal Test
                        if (position.X + spd.X + hitboxSize.X > tx && position.X + spd.X - hitboxSize.X < tx + 16 && position.Y + hitboxSize.Y > ty && position.Y - hitboxSize.Y < ty + 16)
                        {
                            if (position.X + hitboxSize.X + spd.X > tx && position.X + hitboxSize.X + spd.X < tx + 16)
                            {
                                position.X = tx - hitboxSize.X;
                            }
                            else
                            {
                                position.X = tx + 16 + hitboxSize.X;
                            }
                            spd.X = 0.0f;
                        }
                        //Vertical Test
                        if (position.X + hitboxSize.X > tx && position.X - hitboxSize.X < tx + 16 && position.Y + spd.Y + hitboxSize.Y > ty && position.Y - hitboxSize.Y + spd.Y < ty + 16)
                        {
                            if (position.Y + hitboxSize.Y + spd.Y > ty && position.Y + hitboxSize.Y + spd.Y < ty + 16)
                            {
                                position.Y = ty - hitboxSize.Y;
                            }
                            else
                            {
                                position.Y = ty + 16 + hitboxSize.Y;
                            }
                            spd.Y = 0.0f;
                        }
                    }
                    else if (game.tilemap.data[x, y] == 0) // "/" slope
                    {
                        if (position.X + hitboxSize.X >= tx && position.X + hitboxSize.X <= tx + 16 && position.Y + hitboxSize.Y <= ty + 16 && spd.Y >= 0.0f)
                        {
                            float dx = position.X + hitboxSize.X - tx;
                            if (position.Y + hitboxSize.Y >= (ty + 16) - dx)
                            {
                                position.Y = (ty + 16) - dx - hitboxSize.Y;
                                spd.Y = 0f;
                            }
                        }
                    }
                    else if (game.tilemap.data[x, y] == 2) // "\" slope
                    {
                        if (position.X - hitboxSize.X >= tx && position.X - hitboxSize.X <= tx + 16 && position.Y + hitboxSize.Y <= ty + 16 && spd.Y >= 0.0f)
                        {
                            float dx = position.X - hitboxSize.X - tx;
                            if (position.Y + hitboxSize.Y >= ty + dx)
                            {
                                position.Y = ty + dx - hitboxSize.Y;
                                spd.Y = 0f;
                            }
                        }
                    }
                }
            }
            position += spd;
        }
    }
}
