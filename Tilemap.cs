﻿using System;
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
            int hh = 0;
            for (int x = 0; x < w; x++)
            {
                hh = (int)Math.Floor(Noise.Generate(x * 0.0625f) * 10f);
                for (int y = (height / 2) + hh; y < height; y++)
                {
                    data[x, y] = 5;
                }
            }
            RefreshTiles();
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
        }
        public void RefreshTiles()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (data[x, y] > -1 && data[x, y] < 11)
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
                            else if (!topTile && !leftTile && rightTile && bottomTile && (topRightTile || bottomLeftTile) )
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
    }
}
