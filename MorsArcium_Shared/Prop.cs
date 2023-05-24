/*
Copyright (C) 2016-present Alexander Lunsford

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Mors_Arcium
{
    public class Prop : Entity
    {
        public static Rectangle cyberCactus = new Rectangle(0, 48, 16, 32);
        public static Rectangle pillar = new Rectangle(48, 16, 48, 64);
        public static Rectangle pyramid = new Rectangle(16, 48, 32, 32);
        int ty, tx, th, tw;
        bool airborne = false;
        bool czech = false;
        public Prop(Gameplay g, Rectangle src, Vector2 pos, bool a = false) : base(g)
        {
            type = Gameplay.TYPE_PROP;
            origin = Vector2.Zero;
            texture = g.game.textures["tileset"];
            sourceRect = src;
            position = pos;
            collisions = false;
            tx = (int)Math.Floor(pos.X / 16.0f);
            ty = (int)Math.Floor(pos.Y / 16.0f);
            th = (int)Math.Floor(src.Height / 16.0f);
            tw = (int)Math.Floor(src.Width / 16.0f);
            if (tx + tw >= game.tilemap.width) tw = game.tilemap.width - tx;
            if (ty + th >= game.tilemap.height) th = game.tilemap.height - ty;
            if (tx < 0) tx = 0;
            if (ty < 0) ty = 0;
            airborne = a;
        }
        public override void Update(GameTime gt)
        {
            //Make_Money();
            if (!airborne && !czech)
            {
                for (int x = tx; x < tx + tw; x++)
                {
                    if (game.tilemap.data[x, ty + th] == 0 || game.tilemap.data[x, ty + th] == 2 || game.tilemap.data[x, ty + th] == -1)
                    {
                        killMe = true;
                        break;
                    }
                    for (int y = ty; y < ty + th; y++)
                    {
                        if (game.tilemap.data[x, y] != -1)
                        {
                            killMe = true;
                            break;
                        }
                    }
                }
                czech = true;
            }
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, sourceRect, Color.LightGray);
        }
        public override void Collide(Entity perpetrator)
        {
            
        }
    }
}
