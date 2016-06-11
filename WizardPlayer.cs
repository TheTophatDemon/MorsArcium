using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mors_Arcium
{
    public class WizardPlayer : Player
    {
        Animation idleAnimation;
        Animation walkAnimation;
        Animation jumpAnimation;
        Animation attackAnimation;
        Animation attackUpAnimation;
        Animation attackDownAnimation;
        Animation walkAttackAnimation;
        Animation walkAttackUpAnimation;
        Animation walkAttackDownAnimation;
        Animation jumpAttackUpAnimation;
        Animation jumpAttackDownAnimation;
        Animation jumpAttackAnimation;
        Animation deathAnimation;
        public WizardPlayer(Gameplay g) : base(g)
        {
            attackSpeed = 5;
            maxHealth = 150;
            maxMagic = 150;
            health = 150;
            magic = 150;
            walkSpeed = 2.2f;
            jumpHeight = 4.5f;
            //29 + frame
            idleAnimation.frames = new int[] { 0, 1 };
            idleAnimation.speed = 10;
            idleAnimation.looping = true;
            walkAnimation.frames = new int[] { 0, 2, 3, 2 };
            walkAnimation.speed = 3;
            walkAnimation.looping = true;
            jumpAnimation.frames = new int[] { 4, 5, 6 };
            jumpAnimation.speed = 3;
            jumpAnimation.looping = false;
            deathAnimation.frames = new int[] { 22, 23, 24 };
            deathAnimation.looping = false;
            deathAnimation.speed = 5;
            attackAnimation.frames = new int[] { 7, 8 };
            attackUpAnimation.frames = new int[] { 9, 10 };
            attackDownAnimation.frames = new int[] { 11, 12 };
            walkAttackAnimation.frames = new int[] { 13, 14 };
            walkAttackUpAnimation.frames = new int[] { 15, 16 };
            walkAttackDownAnimation.frames = new int[] { 17, 18 };
            jumpAttackAnimation.frames = new int[] { 25, 26 };
            jumpAttackUpAnimation.frames = new int[] { 27, 28 };
            jumpAttackDownAnimation.frames = new int[] { 29, 30 };
            jumpAttackAnimation.speed = jumpAttackUpAnimation.speed = jumpAttackDownAnimation.speed = walkAttackAnimation.speed = walkAttackUpAnimation.speed = walkAttackDownAnimation.speed = attackAnimation.speed = attackUpAnimation.speed = attackDownAnimation.speed = 3;
            jumpAttackAnimation.looping = jumpAttackUpAnimation.looping = jumpAttackDownAnimation.looping = walkAttackAnimation.looping = walkAttackUpAnimation.looping = walkAttackDownAnimation.looping = attackAnimation.looping = attackUpAnimation.looping = attackDownAnimation.looping = true;
            sheetOffset = 32;
            animation = idleAnimation;
        }
        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }
        public override void Attack()
        {
            base.Attack();
            if (cooldown == 0 && deathTimer == 0)
            {
                attacking = true;
                cooldown = attackSpeed;
            }
        }
        protected override void ChangeAnimationState(string st)
        {
            string lastState = animationState;
            base.ChangeAnimationState(st);
            //Process transition animations
            /*if (animationState == "attack" && lastState == "jump")
            {
                animation = jumpShootTransition;
            }
            else
            {*/
                SyncAnimationWithState();
            //}
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
                    if (aimDirection == 1)
                    {
                        animation = attackUpAnimation;
                    }
                    else if (aimDirection == -1)
                    {
                        animation = attackDownAnimation;
                    }
                    else
                    {
                        animation = attackAnimation;
                    }
                    break;
                case "walk_attack":
                    if (aimDirection == 1)
                    {
                        animation = walkAttackUpAnimation;
                    }
                    else if (aimDirection == -1)
                    {
                        animation = walkAttackDownAnimation;
                    }
                    else
                    {
                        animation = walkAttackAnimation;
                    }
                    break;
                case "jump_attack":
                    if (aimDirection == 1)
                    {
                        animation = jumpAttackUpAnimation;
                    }
                    else if (aimDirection == -1)
                    {
                        animation = jumpAttackDownAnimation;
                    }
                    else
                    {
                        animation = jumpAttackAnimation;
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
    }
}
