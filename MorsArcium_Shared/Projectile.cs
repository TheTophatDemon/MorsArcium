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
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public class Projectile : Entity
    {
        public Entity owner;
        public float dodgeDistance;
        public Vector2 knockback;
        public Projectile(Gameplay g, Entity own) : base(g)
        {
            owner = own;
        }
        public override void Update(GameTime gt)
        {
            if (knockback.X != 0.0f)
            {
                knockback.X *= 0.93f;
                if (Math.Abs(knockback.X) < 0.1f) knockback.X = 0.0f;
            }
            if (knockback.Y != 0.0f)
            {
                knockback.Y *= 0.93f;
                if (Math.Abs(knockback.Y) < 0.1f) knockback.Y = 0.0f;
            }
        }
        public override void Draw(SpriteBatch sp)
        {
        }
        public override void Collide(Entity perpetrator)
        {
        }
    }
}
