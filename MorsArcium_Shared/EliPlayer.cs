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
    public class EliPlayer : Player
    {
        Animation idleAnimation;
        Animation walkAnimation;
        Animation jumpAnimation;
        Animation idleAttackAnimation;
        Animation walkAttackAnimation;
        Animation jumpAttackAnimation;
        Animation deathAnimation;
        Vector2[] lastPositions;
        float afterImageCount = 8;
        int afterImageCounter = 0;
        float originalWalkSpeed;
        float damageMultiplier = 1.0f;
        float boost;
        public EliPlayer(Gameplay g, int hhh = 0) : base(g, hhh)
        {
            attackSpeed = 5;
            maxHealth = 80 + healthHandicap;
            maxMagic = 200;
            health = 80 + healthHandicap;
            magic = 200;
            walkSpeed = 2.2f;
            originalWalkSpeed = walkSpeed;
            boost = 0.0f;
            jumpHeight = 5.0f;
            acceleration = 0.20f;
            sheetOffset = 64;
            sourceRect = new Rectangle(0, 64, 32, 32);
            sourceRect = new Rectangle(0, 0, 40, 32);
            hitboxOffset = new Vector2(0, 0);
            hitboxSize = new Vector2(6, 16);
            origin = new Vector2(16, 16);
            idleAnimation.frames = new int[] { 0, 1, 2, 1 }; idleAnimation.speed = 5; idleAnimation.looping = true;
            walkAnimation.frames = new int[] { 3, 4, 5, 6, 7 }; walkAnimation.speed = 2; walkAnimation.looping = true;
            jumpAnimation.frames = new int[] { 5, 6, 7 }; jumpAnimation.speed = 3; jumpAnimation.looping = false;
            idleAttackAnimation.frames = new int[] { 8, 9, 10, 11, 12 }; idleAttackAnimation.speed = 2; idleAttackAnimation.looping = true;
            walkAttackAnimation.frames = new int[] { 13, 14, 15, 16, 17 }; walkAttackAnimation.speed = 2; walkAttackAnimation.looping = true;
            jumpAttackAnimation.frames = new int[] { 18, 19, 20, 21, 22 }; jumpAttackAnimation.speed = 2; jumpAttackAnimation.looping = true;
            deathAnimation.frames = new int[] { 23, 24, 25 };
            deathAnimation.speed = 5;
            deathAnimation.looping = false;
            lastPositions = new Vector2[8];
            for (int i = 0; i < lastPositions.Length; i++)
            {
                lastPositions[i] = position;
            }
        }
        public override void Update(GameTime gt)
        {
            magic += 1;
            base.Update(gt);
            if (boost > 0.0f)
            {
                controllable = false;
                gravity = 0.0f;
                jump = 0.0f;
                damageMultiplier = 4.0f;
                boost -= 1.0f;
                if (boost <= 0.0f)
                {
                    boost = 0.0f;
                }
                if (boost <= 25.0f)
                {
                    walk = walkSpeed;
                    if (spriteEffects == SpriteEffects.FlipHorizontally) walk = -walk;
                    cooldown = 0;
                    Attack();
                    knockback = Vector2.Zero;
                }
                if (boost <= 10.0f)
                {
                    afterImageCount -= 0.8f;
                }
                lastPositions[afterImageCounter] = position;
                afterImageCounter += 1;
                if (afterImageCounter >= afterImageCount) afterImageCounter = 0;
                walkSpeed = originalWalkSpeed * 5.0f;
            }
            else
            {
                controllable = true;
                damageMultiplier = 1.0f;
                walkSpeed = originalWalkSpeed;
            }
        }
        public override void Attack()
        {
            base.Attack();
            //if (boost <= 0.0f)
            {
                if (cooldown == 0 && deathTimer == 0)
                {
                    attacking = true;
                    cooldown = Math.Max(0, attackSpeed + game.reloadOffset);
                    for (int i = 0; i < game.entities.GetLength(1); i++)
                    {
                        if (game.entities[Gameplay.TYPE_PLAYER, i] != null)
                        {
                            if (Vector2.Distance(game.entities[Gameplay.TYPE_PLAYER, i].position, position) < 32)
                            {
                                Player p = (Player)game.entities[Gameplay.TYPE_PLAYER, i];
                                bool damage = false;
                                if (spriteEffects == SpriteEffects.None && p.position.X > position.X)
                                {
                                    p.knockback = new Vector2(8.0f, 0.0f);
                                    damage = true;
                                }
                                else if (spriteEffects == SpriteEffects.FlipHorizontally && p.position.X < position.X)
                                {
                                    p.knockback = new Vector2(-8.0f, 0.0f);
                                    damage = true;
                                }
                                if (damage)
                                {
                                    if (game.reloadOffset >= 0)
                                    {
                                        p.Damage((int)Math.Floor(7 * damageMultiplier), this);
                                    }
                                    else
                                    {
                                        p.Damage((int)Math.Floor(14 * damageMultiplier), this);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public override void Special()
        {
            if (magic == maxMagic && deathTimer == 0 && boost == 0.0f)
            {
                boost = 50.0f;
                magic = 0;
                afterImageCount = 8;
                afterImageCounter = 0;
                knockback = Vector2.Zero;
                jump = 0.0f;
                for (int i = 0; i < lastPositions.Length; i++)
                {
                    lastPositions[i] = position;
                }
            }
            base.Special();
            
        }
        public override void Damage(int amount, Entity perpetrator = null)
        {
            if (boost <= 0.0f) base.Damage(amount, perpetrator);
        }
        protected override void ChangeAnimationState(string st)
        {
            string lastState = animationState;
            base.ChangeAnimationState(st);
            SyncAnimationWithState();
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
                case "jump":
                    animation = jumpAnimation;
                    break;
                case "attack":
                    animation = idleAttackAnimation;
                    break;
                case "walk_attack":
                    animation = walkAttackAnimation;
                    break;
                case "jump_attack":
                    animation = jumpAttackAnimation;
                    break;
                case "dead":
                    animation = deathAnimation;
                    break;
            }

        }
        public override void Draw(SpriteBatch sp)
        {
            if (deathTimer == 0)
            {
                if (boost > 0.0f)
                {
                    for (int i = 0; i < afterImageCount; i++)
                    {
                        sp.Draw(texture, lastPositions[i] - origin, sourceRect, new Color(Color.White, 1.0f - ((float)i / lastPositions.Length)), rotation, Vector2.Zero, scale, spriteEffects, 0);
                    }
                }
                if (hurtTimer == 0 || hurtTimer % 4 == 0)
                {
                    if (spriteEffects == SpriteEffects.None)
                    {
                        sp.Draw(texture, position - origin, sourceRect, boost > 0 ? Color.AliceBlue : Color.White, rotation, Vector2.Zero, scale, spriteEffects, 0);
                    }
                    else
                    {
                        sp.Draw(texture, position - origin - new Vector2(8, 0), sourceRect, boost > 0 ? Color.AliceBlue : Color.White, rotation, Vector2.Zero, scale, spriteEffects, 0);
                    }
                }
            }
            else
            {
                sp.Draw(texture, position, sourceRect, Color.White, rotation, origin, scale, spriteEffects, 0);
            }
        }
        protected override void UpdateAnimationState()
        {
            base.UpdateAnimationState();
            //Actually determine what action triggers which state
            if (animationState != "dead")
            {
                if (jump != 0.0f)
                {
                    if (attacking)
                    {
                        ChangeAnimationState("jump_attack");
                    }
                    else
                    {
                        ChangeAnimationState("jump");
                    }
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
            float dist = Math.Abs(target.position.X - position.X);
            if (target.position.X < position.X)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
            }
            if (dist > 16) Walk();
            Attack();
            
            if (dist > 160.0f)
            {
                aiState = "chase";
            }
        }
    }
}
