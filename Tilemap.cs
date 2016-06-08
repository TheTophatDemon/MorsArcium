using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mors_Arcium
{
    public class Tilemap
    {
        public int[,] data;
        public int width;
        public int height;
        public Texture2D tileset;
        public Vector2[] jumpNodes;
        public int jumpNodeCount = 0;
        Gameplay game;
        public Tilemap(Gameplay g, Texture2D tile, int w, int h)
        {
            game = g;
            width = w;
            height = h;
            tileset = tile;
            data = new int[w, h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    data[x, y] = -1;
                }
            }
            float offset = (float)game.game.random.NextDouble() * 100.0f;
            int hh = 0;
            //int phh = 0;
            for (int x = 0; x < w; x++)
            {
                //phh = hh;
                hh = (int)Math.Floor(Noise.Generate((x * 0.0625f) + offset) * 10f);
                for (int y = (height / 2) + hh; y < height; y++)
                {
                    data[x, y] = 5;
                }
                if (g.game.random.Next(0, 10) == 1)
                {
                    Prop p = new Prop(game, Prop.cyberCactus, new Vector2(x * 16, ((height / 2) + hh - 2) * 16));
                    g.AddEntity(p);
                    p = null;
                }
            }
            jumpNodes = new Vector2[width];
            RefreshTiles();
            RefreshTiles(); //Fix jumpin off cliffs and walking into walls
        }
        public void Draw(SpriteBatch sp)
        {
            int xx = (int)Math.Floor(game.cameraPosition.X / 16); if (xx < 0) xx = 0;
            int yy = (int)Math.Floor(game.cameraPosition.Y / 16); if (yy < 0) yy = 0;
            //22, 17
            for (int y = yy; y < Math.Min(height, yy + 17); y++)
            {
                for (int x = xx; x < Math.Min(width, xx + 22); x++)
                {
                    if (data[x, y] > -1)
                    {
                        sp.Draw(tileset, new Vector2(x * 16, y * 16), new Rectangle(GetCellX(data[x,y]), GetCellY(data[x,y]), 16, 16), Color.White);
                    }
                }
            }
            /*for (int i = 0; i < jumpNodeCount; i++)
            {
                if (jumpNodes[i] != null)
                {
                    sp.Draw(tileset, jumpNodes[i], new Rectangle(32, 32, 16, 16), Color.FloralWhite * 0.5f);
                }
            }*/
        }
        public void RefreshTiles()
        {
            jumpNodeCount = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (data[x, y] > -1)
                    {

                        bool leftTile = false, rightTile = false, topTile = false, bottomTile = false, topLeftTile = false, topRightTile = false, bottomLeftTile = false, bottomRightTile = false;
                        if (x - 1 >= 0)
                        {
                            leftTile = (data[x - 1, y] > -1) ? true : false;
                            if (y - 1 >= 0)
                            {
                                topLeftTile = (data[x - 1, y - 1] > -1) ? true : false;
                            }
                            if (y + 1 < height)
                            {
                                bottomLeftTile = (data[x - 1, y + 1] > -1) ? true : false;
                            }
                        }
                        if (x + 1 < width)
                        {
                            rightTile = (data[x + 1, y] > -1) ? true : false;
                            if (y - 1 >= 0)
                            {
                                topRightTile = (data[x + 1, y - 1] > -1) ? true : false;
                            }
                            if (y + 1 < height)
                            {
                                bottomRightTile = (data[x + 1, y + 1] > -1) ? true : false;
                            }
                        }
                        if (y - 1 >= 0)
                        {
                            topTile = (data[x, y - 1] > -1) ? true : false;
                        }
                        if (y + 1 < height)
                        {
                            bottomTile = (data[x, y + 1] > -1) ? true : false;
                        }
                        if (data[x, y] < 11)
                        {
                            if (!topTile && !bottomTile && !rightTile && !leftTile)
                            {
                                data[x, y] = -1;
                            }
                            else
                            {
                                data[x, y] = 5;
                                if (!topTile && leftTile && rightTile)
                                {
                                    data[x, y] = 1;
                                }
                                else if (!topTile && !leftTile && rightTile && bottomTile && (topRightTile || bottomLeftTile))
                                {
                                    data[x, y] = 0;
                                }
                                else if (!topTile && leftTile && !rightTile && bottomTile && (topLeftTile || bottomRightTile))
                                {
                                    data[x, y] = 2;
                                }
                                else if (!bottomTile && !leftTile && rightTile && topTile)
                                {
                                    data[x, y] = 6;
                                }
                                else if (!bottomTile && leftTile && !rightTile && topTile)
                                {
                                    data[x, y] = 7;
                                }
                                else if (!topRightTile && rightTile && topTile)
                                {
                                    data[x, y] = 3;
                                }
                                else if (!topLeftTile && leftTile && topTile)
                                {
                                    data[x, y] = 4;
                                }
                                else if (!topTile && !leftTile && !rightTile && bottomTile)
                                {
                                    data[x, y] = 1;
                                }
                                else if (!topTile && !leftTile && rightTile && !topRightTile && !bottomLeftTile)
                                {
                                    data[x, y] = 1;
                                }
                                else if (!topTile && leftTile && !rightTile && !topLeftTile && !bottomRightTile)
                                {
                                    data[x, y] = 1;
                                }
                                else if (!topTile && !leftTile && rightTile && !bottomTile && (topRightTile || bottomLeftTile))
                                {
                                    data[x, y] = 1;
                                }
                                else if (!topTile && leftTile && !rightTile && !bottomTile && (topLeftTile || bottomRightTile))
                                {
                                    data[x, y] = 1;
                                }
                            }
                        }
                        if (!topTile && (!leftTile || !rightTile))
                        {
                            if ((!leftTile && (!bottomLeftTile || !topRightTile)) || (!rightTile && (!bottomRightTile || !topLeftTile)))
                            {
                                if (((x + 1 < width && data[x + 1, y] > 2 && !leftTile)
                                    || (x - 1 >= 0 && data[x - 1, y] > 2 && !rightTile)) || data[x, y] == 1)
                                {
                                    jumpNodes[jumpNodeCount] = new Vector2((x * 16), (y * 16));
                                    jumpNodeCount += 1;
                                }
                            }
                        }
                    }
                }
            }
        }
        private int GetCellX(int index)
        {
            return (index % (tileset.Width / 16)) * 16;
        }
        private int GetCellY(int index)
        {
            return (int)Math.Floor((float)index / (tileset.Width / 16)) * 16;
        }
        public bool CollideRect(float x, float y, float w, float h) //The rectangle is centered, mind you!
        {
            int tl = (int)Math.Floor((x - w) / 16.0f) - 1;
            int tu = (int)Math.Floor((y - h) / 16.0f) - 1;
            int tr = tl + (int)Math.Ceiling(w / 8.0f) + 2;
            int tb = tu + (int)Math.Ceiling(h / 8.0f) + 2;
            tl = (int)Math.Min(width, Math.Max(0, tl));
            tr = (int)Math.Min(width, Math.Max(0, tr));
            tu = (int)Math.Min(height, Math.Max(0, tu));
            tb = (int)Math.Min(height, Math.Max(0, tb));
            for (int yy = tu; yy < tb; yy++)
            {
                for (int xx = tl; xx < tr; xx++)
                {
                    int tx = xx * 16;
                    int ty = yy * 16;
                    if (data[xx, yy] != -1)
                    {
                        if (x + w > tx && x - w < tx + 16 && y + h > ty && y - h < ty + 16)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
