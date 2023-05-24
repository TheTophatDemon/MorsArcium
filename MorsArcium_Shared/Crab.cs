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
    public class Crab : Projectile
    {
        Vector2 initSpd;
        Vector2 lastPos;
        float timer = 100;
        public static int[] CrabCollisionMask = new int[] { Gameplay.TYPE_PLAYER };
        public Crab(Gameplay g, Vector2 pos, Vector2 initialSpeed, Entity own) : base(g, own)
        {
            collisionMask = CrabCollisionMask;
            texture = g.game.textures["projectiles"];
            type = Gameplay.TYPE_PROJECTILE;
            sourceRect = new Rectangle(0, 0, 16, 16);
            hitboxSize = new Vector2(8, 8); //Half of the actual hitbox's size
            hitboxOffset = new Vector2(0, 0);
            origin = new Vector2(8, 8);
            speed = initialSpeed;
            position = pos;
            initSpd = new Vector2(Math.Abs(initialSpeed.X), Math.Abs(initialSpeed.Y));
            owner = own;
            dodgeDistance = 32.0f;
        }
        public override void Update(GameTime gt)
        {
            base.Update(gt);
            if (position.X == lastPos.X)
            {
                if (speed.X > 0.0f)
                {
                    speed.X = -initSpd.X;
                }
                else if (speed.X < 0.0f)
                {
                    speed.X = initSpd.X;
                }
            }
            speed.Y += game.gravityAcceleration;
            if (speed.Y > 8.0f) speed.Y = 8.0f;
            if ((collision_bottom && onSlope == -1) || (onSlope == -1 && wasOnSlope != -1))
            {
                speed.Y = 0.0f;
            }
            if (collision_top)
            {
                speed.Y = 0.5f;
            }
            lastPos = position;
            TryMove((speed + knockback) * game.projectileSpeedMultiplier);
            anim += 1;
            if (anim > 5)
            {
                anim = 0;
                if (sourceRect.X == 0)
                {
                    sourceRect.X = 16;
                }
                else
                {
                    sourceRect.X = 0;
                }
            }
            timer -= 1;
            if (timer == 0)
            {
                timer -= 1;
                game.Explode(position.X, position.Y + 8.0f, 16f, 8, false, this.owner);
                game.game.audio.Play3DSound("explosion", position);
                killMe = true;
            }
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position - origin, sourceRect, Color.White);
        }
        public override void Collide(Entity perpetrator)
        {
            if (perpetrator != owner && perpetrator.type == Gameplay.TYPE_PLAYER)
            {
                Player p = (Player)perpetrator;
                if (p.deathTimer == 0)
                {
                    timer = 1;
                }
                p = null;
            }
        }
    }
}
