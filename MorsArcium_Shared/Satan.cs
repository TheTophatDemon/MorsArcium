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
    public class Satan : Entity
    {
        Animation idleAnimation;
        Animation attackAnimation;
        float grenp = 0.0f;
        float targetX = 0.0f;
        int bombTimer = 0;
        int timer = 0;
        public Satan(Gameplay g, Vector2 pos) : base(g)
        {
            collisions = false;
            position = pos;
            type = Gameplay.TYPE_BEAM; //So he's behind the terrain
            origin = new Vector2(32, 36);
            sourceRect = new Rectangle(0, 128, 64, 72);
            idleAnimation.frames = new int[] { 0 };
            idleAnimation.looping = true;
            idleAnimation.speed = 1;
            attackAnimation.frames = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 3, 4, 5 };
            attackAnimation.speed = 3;
            attackAnimation.looping = false;
            animation = idleAnimation;
            texture = game.game.textures["characters"];
        }
        public override void Update(GameTime gt)
        {
            if (animation.frames == idleAnimation.frames)
            {
                if (speed.X > 3.0f) speed.X = 3.0f;
                if (speed.X < -3.0f) speed.X = -3.0f;
                if (targetX > position.X)
                {
                    speed.X += 0.1f;
                }
                else
                {
                    speed.X -= 0.1f;
                }
                if (Math.Abs(targetX - position.X) < 4)
                {
                    targetX = game.game.random.Next(32, (game.tilemap.width - 2) * 16);
                }
                if (Math.Abs(game.humanPlayer.position.X - position.X) < 96.0f)
                {
                    animation = attackAnimation;
                    frame = 0;
                    anim = 0;
                }
            }
            else if (animation.frames == attackAnimation.frames)
            {
                timer += 1;
                speed.X *= 0.9f;
                if (Math.Abs(speed.X) < 0.1f) speed.X = 0.0f;
                if (frame == animation.frames.Length - 1)
                {
                    bombTimer += 1;
                    if (bombTimer > 25)
                    {
                        bombTimer = 0;
                        Bomb b = new Bomb(game, position);
                        b.speed.X = ((float)game.game.random.NextDouble() - 0.5f) * 4.0f;
                        b.speed.Y = -2.0f;
                        game.AddEntity(b);
                    }
                }
                if (timer > 200)
                {
                    timer = 0;
                    animation = idleAnimation;
                    anim = 0;
                    frame = 0;
                }
            }
            if (game.satan == null)
            {
                speed.Y -= 1.0f;
            }
            Animate();
            position += speed;
            grenp += 0.05f;
            position.Y += (float)Math.Sin(grenp) * 2.0f;
            if (position.Y < -128.0f)
            {
                killMe = true;
            }
        }
        public void Animate()
        {
            if (animation.frames != null)
            {
                anim += 1;
                if (anim > animation.speed)
                {
                    anim = 0;
                    frame += 1;
                    if (frame > animation.frames.Length - 1)
                    {
                        frame = 0;
                        if (!animation.looping)
                        {
                            frame = animation.frames.Length - 1;
                        }
                    }
                }
                if (frame >= animation.frames.Length)
                {
                    frame = 0;
                }
                sourceRect.X = (animation.frames[frame] % (texture.Width / sourceRect.Width)) * sourceRect.Width;
            }
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, sourceRect, Color.White, rotation, origin, scale, SpriteEffects.None, 0);
        }
        public override void Collide(Entity perpetrator)
        {
            Console.WriteLine("UNACCEPTABLE!!!!!!!!! GET OUT OF HERE! GO! AWAY WITH YOU! YOU SNIVELING HERETIC! JUST STOP! JEEZ! GOD! GRAND DAD! FLEENSTONES! WOW!");
        }
    }
}
