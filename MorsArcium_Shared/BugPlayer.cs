﻿/*
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
    public class BugPlayer : Player
    {
        Animation idleAnimation;
        Animation walkAnimation;
        Animation flyAnimation;
        Animation attackAnimation;
        Animation attackUpAnimation;
        Animation attackDownAnimation;
        Animation walkAttackAnimation;
        Animation walkAttackUpAnimation;
        Animation walkAttackDownAnimation;
        Animation jumpAttackUpAnimation;
        Animation jumpAttackDownAnimation;
        Animation flyAttackUpAnimation;
        Animation flyAttackDownAnimation;
        Animation deathAnimation;
        Animation jumpAnimation;
        bool tryingToFly = false;
        bool flymode = false;
        bool wasJump = false;

        public BugPlayer(Gameplay g, int hhh = 0) : base(g, hhh)
        {
            attackSpeed = 35;
            maxHealth = 70 + healthHandicap;
            maxMagic = 100;
            health = 70 + healthHandicap;
            magic = 100;
            walkSpeed = 2.5f;
            jumpHeight = 5.2f;
            hitboxSize = new Vector2(6, 16); //Half of the actual hitbox's size
            sheetOffset = 96;
            sourceRect = new Rectangle(0, 96, 32, 32);

            idleAnimation.frames = new int[] { 0, 1, 2, 1 };
            idleAnimation.looping = true;
            idleAnimation.speed = 5;
            walkAnimation.frames = new int[] { 0, 3, 4, 5, 6 };
            walkAnimation.speed = 2;
            walkAnimation.looping = true;
            jumpAnimation.frames = new int[] { 7, 8 };
            jumpAnimation.speed = 3;
            jumpAnimation.looping = false;
            flyAnimation.frames = new int[] { 8, 9, 10, 9 };
            flyAnimation.speed = 3;
            flyAnimation.looping = true;
            attackAnimation.frames = new int[] { 11 };
            attackAnimation.looping = false;
            attackAnimation.speed = 3;
            walkAttackAnimation.frames = new int[] { 11, 12, 13, 14, 15 };
            walkAttackAnimation.speed = 2;
            walkAttackAnimation.looping = true;
            attackUpAnimation.frames = new int[] { 16 };
            attackUpAnimation.speed = 3;
            attackUpAnimation.looping = false;
            walkAttackUpAnimation.frames = new int[] { 16, 17, 18, 19, 20 };
            walkAttackUpAnimation.speed = 2; walkAttackUpAnimation.looping = true;
            attackDownAnimation.frames = new int[] { 21 };
            attackDownAnimation.speed = 3; attackDownAnimation.looping = false;
            walkAttackDownAnimation.frames = new int[] { 21, 22, 23, 24, 25 };
            walkAttackDownAnimation.speed = 2; walkAttackDownAnimation.looping = true;
            jumpAttackUpAnimation.frames = new int[] { 29 }; flyAttackUpAnimation.frames = new int[] { 29, 30, 31, 30 };
            jumpAttackUpAnimation.speed = flyAttackUpAnimation.speed = 3;
            jumpAttackUpAnimation.looping = flyAttackUpAnimation.looping = true;
            jumpAttackDownAnimation.frames = new int[] { 32 }; flyAttackDownAnimation.frames = new int[] { 32, 33, 34, 33 };
            jumpAttackDownAnimation.speed = flyAttackDownAnimation.speed = 3;
            jumpAttackDownAnimation.looping = flyAttackDownAnimation.looping = true;
            deathAnimation.frames = new int[] { 26, 27, 28 };
            deathAnimation.looping = false;
            deathAnimation.speed = 5;
            animation = idleAnimation;
        }
        public override void Update(GameTime gt)
        {
            if (game.IsHuman(this) && game.game.platform.GameSettings.jumpFly)
            {
                if (game.game.platform.GameSettings.jump.IsDown() && !wasJump && !granddad)
                {
                    flymode = true;
                }
                if (!game.game.platform.GameSettings.jump.IsDown() && wasJump)
                {
                    flymode = false;
                }
                if (flymode)
                {
                    Special();
                }
                wasJump = game.game.platform.GameSettings.jump.IsDown();
            }
            if (animationState == "fly")
            {
                jump = 0.0f;
                tryingToJump = false;
            }
            if (animationState != "fly" && !tryingToFly) magic += 1;
            if (magic > maxMagic) magic = maxMagic;
            base.Update(gt);
            if (collision_bottom && animationState == "fly" && (!tryingToFly || magic < 2))
            {
                ChangeAnimationState("idle");
            }
            tryingToFly = false;
        }
        public override void Attack()
        {
            base.Attack();
            if (cooldown == 0 && deathTimer == 0)
            {
                game.game.audio.Play3DSound("throw", position);
                attacking = true;
                cooldown = attackSpeed + game.reloadOffset;
                float spdx = 8.0f;
                if (spriteEffects == SpriteEffects.FlipHorizontally) spdx = -4.0f;
                Trident b = new Trident(game, position + new Vector2(spdx, (aimDirection * 6.0f) + 4), new Vector2(spdx, aimDirection * 4), this);
                
                if (spriteEffects == SpriteEffects.FlipHorizontally)
                {
                    b.rotation = MathHelper.ToRadians(180 - (aimDirection * 35.0f));
                }
                else
                {
                    b.rotation = MathHelper.ToRadians(aimDirection * 35.0f);
                }
                game.AddEntity(b);
                b = null;
            }
        }
        public override void Special()
        {
            tryingToFly = true;
            if (magic > 1 && granddad == false && deathTimer == 0 && animationState != "special")
            {
                magic -= 1;
                ChangeAnimationState("fly");
                gravity = 0.0f;
                jump = 0.0f;
            }
        }
        protected override void ChangeAnimationState(string st)
        {
            string lastState = animationState;
            base.ChangeAnimationState(st);
            //Process transition animations
            //if (animationState == "fly" && lastState != "fly")
           // {
            //    animation = jumpAnimation;
           // }
           // else
            //{
                SyncAnimationWithState();
           // }
        }
        protected override void SyncAnimationWithState()
        {
            base.SyncAnimationWithState();
            switch (animationState)
            {
                case "idle":
                    animation = idleAnimation;
                    break;
                case "walk":
                    animation = walkAnimation;
                    break;
                case "fly":
                    if (aimDirection == -1)
                    {
                        animation = flyAttackUpAnimation;
                    }
                    else if (aimDirection == 1)
                    {
                        animation = flyAttackDownAnimation;
                    }
                    else
                    {
                        animation = flyAnimation;
                    }
                    break;
                case "jump":
                    if (aimDirection == -1)
                    {
                        animation = jumpAttackUpAnimation;
                    }
                    else if (aimDirection == 1)
                    {
                        animation = jumpAttackDownAnimation;
                    }
                    else
                    {
                        animation = jumpAnimation;
                    }
                    break;
                case "attack":
                    if (aimDirection == -1)
                    {
                        animation = attackUpAnimation;
                    }
                    else if (aimDirection == 1)
                    {
                        animation = attackDownAnimation;
                    }
                    else
                    {
                        animation = attackAnimation;
                    }
                    break;
                case "walk_attack":
                    if (aimDirection == -1)
                    {
                        animation = walkAttackUpAnimation;
                    }
                    else if (aimDirection == 1)
                    {
                        animation = walkAttackDownAnimation;
                    }
                    else
                    {
                        animation = walkAttackAnimation;
                    }
                    break;
                case "dead":
                    animation = deathAnimation;
                    break;
            }

        }
        protected override void UpdateAnimationState()
        {
            base.UpdateAnimationState();
            //Actually determine what action triggers which state
            if (animationState != "dead" && animationState != "special" && animationState != "fly")
            {
                if (jump != 0.0f)
                {
                    ChangeAnimationState("jump");
                }
                else
                {
                    if (Math.Abs(walk) > 0.5f)
                    {
                        if (!attacking)
                        {
                            ChangeAnimationState("walk");
                        }
                        else
                        {
                            ChangeAnimationState("walk_attack");
                        }
                    }
                    else
                    {
                        if (!attacking)
                        {
                            ChangeAnimationState("idle");
                        }
                        else
                        {
                            ChangeAnimationState("attack");
                        }
                    }
                }
            }
        }
        public override void CPUAttack()
        {
            walk = 0.0f;
            if (target.position.X < position.X)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
            }
            Attack();
            if (target.position.Y + target.hitboxOffset.Y + target.hitboxSize.Y < position.Y)
            {
                aimDirection = -1;
            }
            else if (target.position.Y + target.hitboxOffset.Y - target.hitboxSize.Y > position.Y && !granddad)// + hitboxOffset.Y + hitboxSize.Y
            {
                aimDirection = 1;
            }
            else
            {
                aimDirection = 0;
            }
            float disp = Math.Abs(target.position.X - position.X);
            if (disp > 160.0f)
            {
                aiState = "chase";
            }
            if (!granddad && Math.Abs(gravity + jump) < 1.0f)
            {
                Special();
            }
            if (disp < 32.0f || magic == maxMagic)
            {
                Jump();
            }
        }
        public override void Collide(Entity perpetrator)
        {
            base.Collide(perpetrator);
        }
    }
}
