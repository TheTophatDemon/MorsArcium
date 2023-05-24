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
    public class Beam : Entity
    {
        public float length;
        public float alpha = 1.0f;
        float cos = 0.0f;
        float sin = 0.0f;
        public Beam(Gameplay g, float l, float r) : base(g)
        {
            length = l;
            rotation = r;
            collisions = false;
            type = Gameplay.TYPE_BEAM;
            origin = new Vector2(0, 4);
            sourceRect = new Rectangle(0, 16, (int)Math.Floor(l - 8.0f), 8);
            cos = (float)Math.Cos(r) * 16.0f;
            sin = (float)Math.Sin(r) * 16.0f;
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
            //sp.Draw(game.game.textures["projectiles"], position, sourceRect, Color.White * alpha, rotation, origin, 1.0f, SpriteEffects.None, 0);
            sp.Draw(game.game.textures["projectiles"], new Vector2((int)Math.Floor(position.X + cos), (int)Math.Floor(position.Y + sin)), sourceRect, Color.White * alpha, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }
        public override void Collide(Entity perpetrator)
        {
            
        }
    }
}
