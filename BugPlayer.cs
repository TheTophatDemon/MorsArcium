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
        Animation specialAnimation;
        Animation deathAnimation;
        Animation jumpAnimation;
        public BugPlayer(Gameplay g) : base(g)
        {
            attackSpeed = 20;
            maxHealth = 70;
            maxMagic = 50;
            health = 70;
            magic = 50;
            walkSpeed = 2.2f;
            jumpHeight = 7.0f;
            
            sheetOffset = 96;

            idleAnimation.frames = new int[] { 0, 1, 2, 1 };
            idleAnimation.looping = true;
            idleAnimation.speed = 5;
            walkAnimation.frames = new int[] { 0, 3, 4, 5, 6 };
            walkAnimation.speed = 2;
            walkAnimation.looping = true;
            jumpAnimation.frames = new int[] { 7 };
            jumpAnimation.speed = 5;
            jumpAnimation.looping = false;
            jumpAnimation.transition = true;
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
            specialAnimation.frames = new int[] { 26, 27 };
            specialAnimation.speed = 3; specialAnimation.looping = false;
            deathAnimation.frames = new int[] { 28, 29, 30 };
            deathAnimation.looping = false;
            deathAnimation.speed = 5;
            animation = idleAnimation;
        }
        protected override void ChangeAnimationState(string st)
        {
            string lastState = animationState;
            base.ChangeAnimationState(st);
            //Process transition animations
            if (animationState == "fly" && lastState != "fly")
            {
                animation = jumpAnimation;
            }
            else
            {
                SyncAnimationWithState();
            }
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
                    animation = flyAnimation;
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
                case "special":
                    animation = specialAnimation;
                    break;
            }

        }
        protected override void UpdateAnimationState()
        {
            base.UpdateAnimationState();
            //Actually determine what action triggers which state
            if (animationState != "dead" && animationState != "special")
            {
                if (jump != 0.0f)
                {
                    ChangeAnimationState("fly");
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
