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
    public class SpookyScarySkeleton : Entity
    {
        public SpookyScarySkeleton(Gameplay g) : base(g)
        {
            collisions = false;
            sourceRect = new Rectangle(384, 128, 32, 32);
            type = Gameplay.TYPE_ITEM;
            hitboxSize = new Vector2(4, 16); //Half of the actual hitbox's size
            hitboxOffset = new Vector2(0, 0);
            origin = new Vector2(16, 16);
        }
        public override void Update(GameTime gt)
        {
            speed.Y += game.gravityAcceleration;
            if (speed.Y > 8.0f) speed.Y = 8.0f;
            if ((collision_bottom && onSlope == -1) || (onSlope == -1 && wasOnSlope != -1))
            {
                speed.Y = 0.0f;
            }
            TryMove(speed);
            anim += 1;
            if (anim > 5)
            {
                anim = 0;
                if (sourceRect.X == 384)
                {
                    sourceRect.X = 416;
                }
                else
                {
                    sourceRect.X = 384;
                }
            }
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(game.game.textures["characters"], position - origin, sourceRect, Color.White);
        }
        public override void Collide(Entity perpetrator)
        {
            
        }
    }
}
